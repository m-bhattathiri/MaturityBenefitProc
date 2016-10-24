using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement
{
    /// <summary>Enforces dual-approval workflows for high-value maturity transactions.</summary>
    public interface IMakerCheckerValidationService
    {
        bool ValidateMakerCheckerSeparation(string makerId, string checkerId);
        bool IsHighValueTransaction(decimal transactionAmount);
        bool RequiresDualApproval(string policyId, decimal maturityValue);
        bool VerifyCheckerAuthority(string checkerId, decimal approvalLimit);
        bool IsTransactionLocked(string transactionId);
        bool ValidateDocumentCompleteness(string transactionId, int requiredDocumentCount);
        bool ApproveTransaction(string transactionId, string checkerId, DateTime approvalDate);
        bool RejectTransaction(string transactionId, string checkerId, string rejectionReason);
        bool CheckEscalationEligibility(string transactionId, int daysPending);
        
        decimal CalculateMaturityPayout(string policyId, decimal baseAmount, double bonusRate);
        decimal GetApprovedTransactionLimit(string checkerRoleCode);
        decimal CalculateTaxDeduction(decimal grossPayout, double taxRate);
        decimal GetPendingApprovalTotal(string checkerId);
        decimal ComputePenaltyAmount(string policyId, int daysEarly);
        decimal GetMaximumAuthorizedAmount(string checkerId);

        double GetApprovalSuccessRate(string checkerId, DateTime startDate, DateTime endDate);
        double CalculateBonusPercentage(string policyId, int activeYears);
        double GetRejectionRatio(string departmentCode);
        double CalculateRiskScore(string policyId, decimal payoutAmount);

        int GetPendingTransactionsCount(string checkerId);
        int CalculateDaysInWorkflow(string transactionId, DateTime submissionDate);
        int GetEscalationLevel(string transactionId);
        int CountRequiredSignatures(decimal payoutAmount);
        int GetRemainingApprovalWindow(string transactionId, DateTime currentDate);

        string GenerateWorkflowRoutingCode(string policyType, decimal amount);
        string GetTransactionStatus(string transactionId);
        string AssignCheckerToTransaction(string transactionId, string departmentCode);
        string GetMakerIdForTransaction(string transactionId);
    }
}