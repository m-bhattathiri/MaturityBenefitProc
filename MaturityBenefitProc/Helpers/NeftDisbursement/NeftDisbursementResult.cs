using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.NeftDisbursement
{
    public class NeftDisbursementResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string UtrNumber { get; set; }
        public string BankName { get; set; }
        public string IfscCode { get; set; }
        public string AccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime SettlementDate { get; set; }

        public NeftDisbursementResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
