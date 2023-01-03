using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Response.Paystack
{
    public class Datum
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string code { get; set; }
        public string longcode { get; set; }
        public string gateway { get; set; }
        public bool pay_with_bank { get; set; }
        public bool active { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        public bool is_deleted { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class FetchBanksResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
    }


}
