namespace BankTransfer.API.Providers.Flutterwave
{
    public class FlutterwaveProviderService : IFlutterwaveProvider
    {
        public async Task<T?> FetchBanks<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public async Task<T?> FetchTransactionStatus<T>(string transactionReference) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> ResolveAccountNumber<T>(string accountNumber, string bankCode) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task<TOutPut?> SendMoney<TOutPut, TInput>(TInput param)
            where TOutPut : class
            where TInput : class
        {
            throw new NotImplementedException();
        }
    }
}
