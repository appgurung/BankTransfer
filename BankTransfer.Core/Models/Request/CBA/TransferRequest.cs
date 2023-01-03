using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Core.Models.Request.CBA
{
    public class TransferRequest
    {
        [Required]
        public string? amount { get; set; }
        [Required]
        public string? currencyCode { get; set; }
        [Required]
        public string? narration { get; set; }
        [Required]
        public string? beneciaryAccountNumber { get; set; }
        [Required]
        public string? beneciaryAccountName { get; set; }
        [Required]
        public string? beneciaryBankCode { get; set; }
        [Required]
        public string? transactionReference { get; set; }
        public int? maxRetryAttempt { get; set; }
        public string? callBackUrl { get; set; }

        public string? provider { get; set; }
    }

}
