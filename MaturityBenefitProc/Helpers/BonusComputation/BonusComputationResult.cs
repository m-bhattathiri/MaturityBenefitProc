using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.BonusComputation
{
    public class BonusComputationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string BonusType { get; set; }
        public decimal BonusRate { get; set; }
        public int PolicyYear { get; set; }
        public decimal SumAssured { get; set; }
        public decimal AccruedAmount { get; set; }

        public BonusComputationResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
