using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Response.CBA
{
    public class TransferResponse
    {
        public string amount { get; set; }
        public string beneciaryAccountNumber { get; set; }
        public string beneciaryAccountName { get; set; }
        public string beneciaryBankCode { get; set; }
        public string transactionReference { get; set; }
        public string transactionDateTime { get; set; }
        public string currencyCode { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public string sessionId { get; set; }
        public string status { get; set; }
    }


}
