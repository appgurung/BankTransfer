using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Response.CBA
{
    public class BanksResponse
    {
        public string code { get; set; }
        public string bankName { get; set; }
        public string longCode { get; set; }
    }

}
