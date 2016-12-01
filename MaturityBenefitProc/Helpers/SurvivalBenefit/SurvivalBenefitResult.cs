using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.SurvivalBenefit
{
    public class SurvivalBenefitResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal BenefitPercentage { get; set; }
        public DateTime DueDate { get; set; }
        public string PlanCode { get; set; }
        public int PolicyYear { get; set; }

        public SurvivalBenefitResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
