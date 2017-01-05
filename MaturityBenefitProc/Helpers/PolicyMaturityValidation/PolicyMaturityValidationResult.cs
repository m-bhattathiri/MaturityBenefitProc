using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.PolicyMaturityValidation
{
    public class PolicyMaturityValidationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string ValidationStatus { get; set; }
        public bool DocumentsVerified { get; set; }
        public string PremiumStatus { get; set; }
        public string KycStatus { get; set; }
        public decimal OutstandingLoan { get; set; }
        public int DaysToMaturity { get; set; }

        public PolicyMaturityValidationResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
