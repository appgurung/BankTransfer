using BankTransfer.API.Providers.Paystack.Models.Request;
using BankTransfer.API.Providers.Paystack.Models.Response;
using BankTransfer.Core.Enums;
using BankTransfer.Core.Helpers;
using BankTransfer.Core.Interface;
using Newtonsoft.Json;

namespace BankTransfer.API.Providers.Paystack
{
    public class PaystackProviderService : IPaystackProvider
    {
        private readonly IRestClientHelper _http;
        private readonly ConfigHelper _config;
        public PaystackProviderService(IRestClientHelper http, ConfigHelper config)
        {
            _http = http;
            _config = config;
            _http.SetBaseUrl(config.GetBaseUrl(ProviderType.PAYSTACK));
        }

        public async Task<T?> ResolveAccountNumber<T>(string accountNumber, string bankCode) where T : class
        {
            Dictionary<string, string> header = new();

            header.Add("Authorization", _config.GetSecreteKey(ProviderType.PAYSTACK));

            var resp = await _http.Get("/resolve", true, header, new string[] { $"?account_number={accountNumber}&bank_code={bankCode}" });

            while (resp.statusCode == System.Net.HttpStatusCode.OK)
            {
                var lookup = JsonConvert.DeserializeObject<AccountEnquiryResponse>(resp.content!);

                return lookup as T;
            }

            return null;
        }

        public async Task<T?> FetchBanks<T>() where T : class
        {
            Dictionary<string, string> header = new();

            header.Add("Authorization", _config.GetSecreteKey(ProviderType.PAYSTACK));

            var resp = await _http.Get("/bank", true, header, new string[] { "?currency=NGN" });

            while (resp.statusCode == System.Net.HttpStatusCode.OK)
            {
                var banks = JsonConvert.DeserializeObject<FetchBanksResponse>(resp.content!);
                return banks as T;
            }
            return null;
        }

        public async Task<T?> FetchTransactionStatus<T>(string transactionReference) where T : class
        {
            Dictionary<string, string> header = new();

            header.Add("Authorization", _config.GetSecreteKey(ProviderType.PAYSTACK));

            var resp = await _http.Get($"/transferrecipient/{transactionReference}");

            while (resp.statusCode == System.Net.HttpStatusCode.OK)
            {
                var transaction = JsonConvert.DeserializeObject<PaystackTransferResponse>(resp.content!);

                return transaction as T;
            }

            return null;
        }

        public async Task<TOutPut?> SendMoney<TOutPut, TInput>(TInput param) where TInput : class
                                                                            where TOutPut : class
        {
            Dictionary<string, string> header = new();

            header.Add("Authorization", _config.GetSecreteKey(ProviderType.PAYSTACK));

            object paramObj = param;

            var payload = (PaystackTransferRequest)Convert.ChangeType(paramObj, typeof(PaystackTransferRequest));

            var resp = await _http.Post("/transfer", true, header, JsonConvert.SerializeObject(payload), CancellationToken.None);

            while (resp.statusCode == System.Net.HttpStatusCode.OK)
            {
                var transactionResult = JsonConvert.DeserializeObject<PaystackTransferResponse>(resp.content!);

                return transactionResult as TOutPut;
            }

            return null;
        }

        public async Task<TOutPut?> CreateTransferRecipient<TOutPut, TInput>(TInput param) where TOutPut : class
                                                                               where TInput : class
        {

            Dictionary<string, string> header = new();

            header.Add("Authorization", _config.GetSecreteKey(ProviderType.PAYSTACK));
            object paramObj = param;

            var payload = (PaystackAddRecipientRequest)Convert.ChangeType(paramObj, typeof(PaystackAddRecipientRequest));

            var resp = await _http.Post("/transferrecipient", true, header, JsonConvert.SerializeObject(param), CancellationToken.None);

            while (resp.statusCode == System.Net.HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<PaystackAddRecipientResponse>(resp.content!);
                return result as TOutPut;
            }
            return null;
        }
    }
}

