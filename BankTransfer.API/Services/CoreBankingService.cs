using BankTransfer.API.Providers.Paystack;
using BankTransfer.API.Providers.Paystack.Models.Request;
using BankTransfer.API.Providers.Paystack.Models.Response;
using BankTransfer.Core.Enums;
using BankTransfer.Core.Helpers;
using BankTransfer.Core.Interface;
using BankTransfer.Core.Models.Request.CBA;
using BankTransfer.Core.Models.Response.CBA;
using BankTransfer.Infrastructure.Entities;
using BankTransfer.Infrastructure.Interface;

namespace BankTransfer.API.Services
{
    public class CoreBankingService : ICoreBanking
    {
        private readonly IPaystackProvider _paystack;
        private readonly ConfigHelper _config;
        private readonly IRepository<Transaction> _db;
        private readonly IServiceProvider _serviceProvider;
        public CoreBankingService(IPaystackProvider paystack, 
            ConfigHelper config, IRepository<Transaction> db, 
            IServiceProvider serviceProvider)
        {
            _paystack = paystack;
            _config = config;
            _db = db;
            _serviceProvider = serviceProvider;
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
                        transactionDateTime = resp.data!.createdAt.ToShortTimeString(),
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
            var transaction = await _db.SingleOrDefault(x => x.TransactionReference == param.transactionReference, CancellationToken.None);

            if(transaction != null)
            {
                return new TransferResponse()
                {
                    responseCode = "99", responseMessage = "Duplicate transaction recieved"
                };
            }

            //Save record to table first
            _ = await _db.Insert(new Transaction()
            {
                Amount = decimal.Parse(param.amount!),
                BeneficiaryAccountNumber = param.beneciaryAccountNumber,BeneficiaryBankCode = param.beneciaryBankCode,
                CallBackUrl = param.callBackUrl,MaximumRetryAttempt = param.maxRetryAttempt,
                CurrencyCode = param.currencyCode,Guid = Guid.NewGuid().ToString(),
                Status = nameof(TransactionType.PENDING),Provider = param.provider,
                TransactionDateTime = DateTime.Now,TransactionReference = param.transactionReference
            }, CancellationToken.None);

            //Process provider call task in background
            _ = Task.Run(() => CallProvider(param));       

            return new TransferResponse()
            {
                responseCode = "00",
                status = nameof(TransactionType.PENDING),
                transactionDateTime = DateTime.Now.ToShortDateString(),
                transactionReference = param.transactionReference!,
                amount = param.amount!, beneciaryAccountName = param.beneciaryAccountName!,
                currencyCode = param.currencyCode!,  beneciaryAccountNumber = param.beneciaryAccountNumber!,
                responseMessage = "Transaction In Progress",
                beneciaryBankCode = param.beneciaryBankCode!
            };

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

        private async void CallProvider(TransferRequest param)
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

                    var resp = await _paystack.SendMoney<PaystackTransferResponse, PaystackTransferRequest>(new PaystackTransferRequest()
                    {
                        amount = int.Parse(param.amount!),
                        source = "balance",
                        recipient = recipientResult!.data.recipient_code,
                        reason = param.narration!
                    });

                    var repository = _serviceProvider.GetService<IRepository<Transaction>>();

                    //Update db transaction record
                    var transaction = await repository.SingleOrDefault(x => x.TransactionReference == param.transactionReference, CancellationToken.None);

                    if (transaction != null)
                    {
                        transaction.Status = resp!.status ? nameof(TransactionType.SUCCESFUL) : nameof(TransactionType.FAILED);
                        transaction.ResponseCode = resp!.status ? "00" : "99";
                        transaction.ResponseMessage = resp!.message;
                    }
                    await repository.Update(transaction!, CancellationToken.None);                  
                    break;
            }
        }
    }
}
