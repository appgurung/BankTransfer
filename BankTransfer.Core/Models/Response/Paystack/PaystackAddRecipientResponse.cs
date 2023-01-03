using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Response.Paystack
{

    public class AddRecipientDatum
    {
        public bool active { get; set; }
        public DateTime createdAt { get; set; }
        public string currency { get; set; }
        public object description { get; set; }
        public string domain { get; set; }
        public object email { get; set; }
        public int id { get; set; }
        public int integration { get; set; }
        public object metadata { get; set; }
        public string name { get; set; }
        public string recipient_code { get; set; }
        public string type { get; set; }
        public DateTime updatedAt { get; set; }
        public bool is_deleted { get; set; }
        public bool isDeleted { get; set; }
        public Details details { get; set; }
    }

    public class Details
    {
        public object authorization_code { get; set; }
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
    }

    public class Meta
    {
        public int total { get; set; }
        public int skipped { get; set; }
        public int perPage { get; set; }
        public int page { get; set; }
        public int pageCount { get; set; }
    }

    public class PaystackAddRecipientResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<AddRecipientDatum> data { get; set; }
        public Meta meta { get; set; }
    }


}
