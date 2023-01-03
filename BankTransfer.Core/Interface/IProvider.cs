using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Interface
{
    public interface IProvider  
    {
        Task<T?> FetchBanks<T>() where T : class;

        Task<T> ResolveAccountNumber<T>(string accountNumber, string bankCode) where T: class;

        Task<TOutPut?> SendMoney<TOutPut, TInput>(TInput param) where TInput : class where TOutPut : class;

        Task<T?> FetchTransactionStatus<T>(string transactionReference) where T: class;
    }
}
