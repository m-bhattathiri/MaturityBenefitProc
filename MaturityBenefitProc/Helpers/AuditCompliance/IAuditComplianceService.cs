using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AuditCompliance
{
    public interface IAuditComplianceService
    {
        AuditComplianceResult LogAuditEntry(string entityType, string entityId, string action, string performedBy);

        AuditComplianceResult ValidateAuditEntry(string auditId);

        AuditComplianceResult GetAuditTrail(string entityType, string entityId);

        bool IsComplianceCheckRequired(string actionType, decimal amount);

        AuditComplianceResult PerformComplianceCheck(string claimNumber, string checkType);

        AuditComplianceResult FlagForReview(string entityId, string reason, string flaggedBy);

        int GetAuditEntryCount(string entityType, string entityId, DateTime fromDate, DateTime toDate);

        AuditComplianceResult GenerateComplianceReport(string reportType, DateTime fromDate, DateTime toDate);

        bool HasComplianceViolation(string entityId);

        AuditComplianceResult ResolveComplianceFlag(string flagId, string resolution, string resolvedBy);

        decimal GetComplianceThreshold(string actionType);

        AuditComplianceResult GetComplianceSummary(DateTime fromDate, DateTime toDate);

        List<AuditComplianceResult> GetAuditHistory(string entityType, DateTime fromDate, DateTime toDate);

        bool ValidateAuditCompleteness(string entityId);

        AuditComplianceResult EscalateComplianceIssue(string entityId, string issueType, string escalatedTo);
    }
}
