using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.NomineeVerification
{
    public class NomineeVerificationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string NomineeName { get; set; }
        public string Relation { get; set; }
        public decimal SharePercentage { get; set; }
        public bool KycVerified { get; set; }
        public bool IsMinor { get; set; }
        public string GuardianName { get; set; }

        public NomineeVerificationResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
