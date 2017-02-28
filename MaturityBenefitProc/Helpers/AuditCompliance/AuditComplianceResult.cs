using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AuditCompliance
{
    public class AuditComplianceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string AuditId { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string Action { get; set; }
        public string PerformedBy { get; set; }
        public string ComplianceStatus { get; set; }
        public string FlagReason { get; set; }

        public AuditComplianceResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
