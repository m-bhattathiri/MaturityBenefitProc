using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PremiumReconciliation
{
    public class PremiumReconciliationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalDue { get; set; }
        public int PremiumsPaidCount { get; set; }
        public decimal ArrearAmount { get; set; }
        public int LapsedMonths { get; set; }
        public decimal RevivalCharges { get; set; }

        public PremiumReconciliationResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
