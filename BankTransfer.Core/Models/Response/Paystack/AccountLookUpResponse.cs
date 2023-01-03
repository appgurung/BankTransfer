using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Response.Paystack
{
    public class Data
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public int bank_id { get; set; }
    }

    public class AccountEnquiryResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

}
