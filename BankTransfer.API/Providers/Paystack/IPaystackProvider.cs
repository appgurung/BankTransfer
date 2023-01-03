using BankTransfer.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.API.Providers.Paystack
{
    public interface IPaystackProvider : IBaseProvider
    {
        Task<TOutPut?> CreateTransferRecipient<TOutPut, TInput>(TInput param) where TInput : class where TOutPut : class;
    }
}
