using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.MoneyBackPlan
{
    public class MoneyBackPlanResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal PayoutPercentage { get; set; }
        public int DueYear { get; set; }
        public string PlanCode { get; set; }
        public decimal TotalPaidOut { get; set; }
        public decimal RemainingPayable { get; set; }

        public MoneyBackPlanResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
