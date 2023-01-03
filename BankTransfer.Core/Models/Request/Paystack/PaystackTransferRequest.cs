using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Request.Paystack
{
    public class PaystackTransferRequest
    {
        public string source { get; set; }
        public string reason { get; set; }
        public int amount { get; set; }
        public string recipient { get; set; }
    }
}
