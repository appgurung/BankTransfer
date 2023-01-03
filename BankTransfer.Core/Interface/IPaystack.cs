using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Interface
{
    public interface IPaystack : IProvider
    {
        Task<TOutPut?> CreateTransferRecipient<TOutPut, TInput>(TInput param) where TInput : class where TOutPut : class;
    }
}
