using BankTransfer.Core.Enums;
using BankTransfer.Core.Helpers;
using BankTransfer.Core.Interface;
using BankTransfer.Core.Models.Request.CBA;
using BankTransfer.Core.Models.Request.Paystack;
using BankTransfer.Core.Models.Response.CBA;
using BankTransfer.Core.Models.Response.Paystack;
using BankTransfer.Infrastructure.Entities;
using BankTransfer.Infrastructure.Interface;

namespace BankTransfer.API.Services
{
    public class CoreBankingService : ICoreBanking
    {
        private readonly IPaystack _paystack;
        private readonly ConfigHelper _config;
        private readonly IRepository<Transaction> _db;
        public CoreBankingService(IPaystack paystack, ConfigHelper config, IRepository<Transaction> db)
        {
            _paystack = paystack;
            _config = config;
            _db = db;
        }

        public async Task<AccountLookUpResponse> AccountNameEnquiry(AccountLookUpRequest param)
        {
            ProviderType providerType = ResolveProvider(param.provider!);
            var  lookUpResponse = new AccountLookUpResponse() { };
            switch (providerType)
            {
                case ProviderType.PAYSTACK:
                    var resp = await _paystack.ResolveAccountNumber<AccountEnquiryResponse>(param.accountNumber!, param.code!);

                    if(resp != null)
                    {
                        lookUpResponse.accountNumber = resp.data.account_number;
                        lookUpResponse.accountName = resp.data.account_name;
                        lookUpResponse.bankCode = param.code!;
                        return lookUpResponse;
                    }
                    break;
            }
            return lookUpResponse;
        }

        public async Task<List<BanksResponse>> FetchBanks(string provider)
        {
            ProviderType providerType = ResolveProvider(provider);
            var banks = new List<BanksResponse>() { };
            switch (providerType)
            {
                case ProviderType.PAYSTACK:
                var bankLists = await _paystack.FetchBanks<FetchBanksResponse>();

                if (bankLists!.data.Count() > 0)
                {
                    foreach (var item in bankLists.data)
                    {
                        banks.Add(new BanksResponse()
                        {
                            bankName = item.name,
                            code = item.code,
                            longCode = item.longcode,
                        });
                    }
                }
                break;
            }
            return banks;
        }

        public async Task<TransferResponse> FetchTransactionStatus(string transactionReference)
        {
            // Retrieve record by transaction reference
            var transaction = await _db.SingleOrDefault(x => x.TransactionReference == transactionReference, CancellationToken.None);

            if (transaction is null)
                return new TransferResponse() { };

            ProviderType providerType = ResolveProvider(transaction.Provider!);

            switch (providerType)
            {
                case ProviderType.PAYSTACK:

                    var resp = await _paystack.FetchTransactionStatus<PaystackTransferResponse>(transactionReference);

                    return new TransferResponse()
                    {
                        responseCode = resp!.status ? "00" : "99",
                        status = resp!.status ? "SUCCESS" : "FAILURE",
                        transactionDateTime = resp.data.createdAt.ToShortTimeString(),
                        transactionReference = resp.data.transfer_code,
                        amount = resp.data.amount.ToString(),
                        currencyCode = resp.data.currency,
                        responseMessage = resp!.status ? "Transfer_Succesful" : "Transfer_Not_Succesful",
                        beneciaryBankCode = transaction.BeneficiaryBankCode!
                    };
            }
            return new TransferResponse() { };
        }

        public async Task<TransferResponse> TransferFunds(TransferRequest param)
        {
            ProviderType providerType = ResolveProvider(param.provider!);
            
            switch (providerType)
            {
                case ProviderType.PAYSTACK:

                    //Add transfer recipient first (e.g beneficiary)
                    var recipientResult = await _paystack.CreateTransferRecipient<PaystackAddRecipientResponse, PaystackAddRecipientRequest>(new PaystackAddRecipientRequest()
                    {
                        account_number = param.beneciaryAccountNumber!,
                        bank_code = param.beneciaryBankCode!,
                        currency = param.currencyCode!,
                        type = "nuban",
                        name = param.beneciaryAccountName!
                    });

                    if (!recipientResult!.status)
                        return new TransferResponse() { };

                    var resp = await _paystack.SendMoney<PaystackTransferResponse, PaystackTransferRequest>(new PaystackTransferRequest() { 
                        
                        amount = int.Parse(param.amount!), 
                        source = "balance", 
                        recipient = recipientResult!.data[0].recipient_code, 
                        reason = param.narration!
                    });

                    //Save record to table
                    _ = _db.Insert(new Transaction() { 
                      Amount = decimal.Parse(param.amount!), 
                      BeneficiaryAccountNumber = param.beneciaryAccountNumber, BeneficiaryBankCode = param.beneciaryBankCode,
                      CallBackUrl = param.callBackUrl, MaximumRetryAttempt = param.maxRetryAttempt, CurrencyCode = param.currencyCode,
                      Guid = Guid.NewGuid().ToString(), Provider = param.provider, TransactionDateTime = DateTime.Now,
                      ResponseCode = resp!.status ? "00" : "99", ResponseMessage = resp.message, TransactionReference = param.transactionReference
                    }, CancellationToken.None);

                    return new TransferResponse()
                    {
                        responseCode = resp!.status ? "00" : "99",
                        status = resp!.status ? "SUCCESS" : "FAILURE",
                        transactionDateTime = resp.data.createdAt.ToShortTimeString(),
                        transactionReference = resp.data.transfer_code, amount = resp.data.amount.ToString(),
                        currencyCode = resp.data.currency,
                        responseMessage = resp!.status ? "Transfer_Succesful" : "Transfer_Not_Succesful", 
                        beneciaryBankCode = param.beneciaryBankCode!
                    };

            }
            return new TransferResponse() { };
        }

        private ProviderType ResolveProvider(string provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                string? defaultProvider = _config.GetDefaultProvider();
                return (ProviderType)Enum.Parse(typeof(ProviderType), defaultProvider, true);
            }
            else
            {
                return  (ProviderType)Enum.Parse(typeof(ProviderType), provider, true);
            }
        }
    }
}
