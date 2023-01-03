using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Response.CBA
{
    public class AccountLookUpResponse
    {
        public string accountNumber { get; set; }
        public string accountName { get; set; }
        public string bankCode { get; set; }
        public string bankName { get; set; }
    }

}
