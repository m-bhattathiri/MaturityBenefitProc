using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.EndowmentPlan
{
    public class EndowmentPlanResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string PlanCode { get; set; }
        public int PolicyTerm { get; set; }
        public int PremiumsPaid { get; set; }
        public decimal PaidUpValue { get; set; }
        public decimal SurrenderValue { get; set; }
        public decimal MaturityBenefit { get; set; }

        public EndowmentPlanResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
