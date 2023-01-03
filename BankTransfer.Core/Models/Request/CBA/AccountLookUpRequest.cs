using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Request.CBA
{
    public class AccountLookUpRequest
    {
        [Required]
        public string? accountNumber { get; set; }

        [Required]
        public string? code { get; set; }

        public string? provider { get; set; }
    }
}
