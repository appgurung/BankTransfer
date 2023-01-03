using BankTransfer.Core.Models.Request.CBA;
using BankTransfer.Core.Models.Response.CBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Interface
{
    public interface ICoreBanking
    {
        Task<List<BanksResponse>> FetchBanks(string provider);

        Task<TransferResponse> TransferFunds(TransferRequest param);

        Task<AccountLookUpResponse> AccountNameEnquiry(AccountLookUpRequest param);

        Task<TransferResponse> FetchTransactionStatus(string transactionReference);
    }
}
