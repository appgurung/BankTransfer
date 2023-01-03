using System;
using System.Collections.Generic;

namespace BankTransfer.Infrastructure.Entities
{
    public partial class Transaction
    {
        public long Id { get; set; }
        public string? Guid { get; set; }
        public decimal? Amount { get; set; }
        public string? BeneficiaryAccountNumber { get; set; }
        public string? BeneficiaryBankCode { get; set; }
        public string? TransactionReference { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public string? CurrencyCode { get; set; }
        public string? ResponseMessage { get; set; }
        public string? ResponseCode { get; set; }
        public int? MaximumRetryAttempt { get; set; }
        public string? CallBackUrl { get; set; }
        public string? Provider { get; set; }
    }
}
