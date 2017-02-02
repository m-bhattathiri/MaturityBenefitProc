using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.ClaimSettlement
{
    public class ClaimSettlementResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string ClaimNumber { get; set; }
        public string ClaimantName { get; set; }
        public string SettlementType { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetAmount { get; set; }
        public string DischargeVoucherNumber { get; set; }

        public ClaimSettlementResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
