// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement
{
    public class MakerCheckerValidationService : IMakerCheckerValidationService
    {
        private const decimal HighValueThreshold = 50000m;
        private const int MaxApprovalWindowDays = 14;
        private const decimal BasePenaltyRatePerDay = 15.5m;

        // Mock databases for substantive logic
        private readonly Dictionary<string, string> _transactionMakers = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _transactionStatuses = new Dictionary<string, string>();
        private readonly Dictionary<string, decimal> _checkerLimits = new Dictionary<string, decimal>();
        private readonly HashSet<string> _lockedTransactions = new HashSet<string>();

        public MakerCheckerValidationService()
        {
            // Seed some mock data
            _checkerLimits.Add("CHK-001", 100000m);
            _checkerLimits.Add("CHK-002", 250000m);
            _checkerLimits.Add("CHK-MGR", 1000000m);
        }

        public bool ValidateMakerCheckerSeparation(string makerId, string checkerId)
        {
            if (string.IsNullOrWhiteSpace(makerId) || string.IsNullOrWhiteSpace(checkerId))
                return false;
            
            // The core principle of Maker-Checker: they cannot be the same person
            return !makerId.Equals(checkerId, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsHighValueTransaction(decimal transactionAmount)
        {
            return transactionAmount >= HighValueThreshold;
        }

        public bool RequiresDualApproval(string policyId, decimal maturityValue)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            
            // High value or specific risky policy prefixes require dual approval
            return IsHighValueTransaction(maturityValue) || policyId.StartsWith("RISK-");
        }

        public bool VerifyCheckerAuthority(string checkerId, decimal approvalLimit)
        {
            if (string.IsNullOrWhiteSpace(checkerId)) return false;

            decimal maxLimit = GetMaximumAuthorizedAmount(checkerId);
            return maxLimit >= approvalLimit;
        }

        public bool IsTransactionLocked(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return false;
            return _lockedTransactions.Contains(transactionId);
        }

        public bool ValidateDocumentCompleteness(string transactionId, int requiredDocumentCount)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return false;
            if (requiredDocumentCount <= 0) return true;

            // In a real system, this would query a document repository.
            // Mocking a check based on transaction ID length for substantive logic.
            int actualDocs = transactionId.Length % 5 + 1; 
            return actualDocs >= requiredDocumentCount;
        }

        public bool ApproveTransaction(string transactionId, string checkerId, DateTime approvalDate)
        {
            if (IsTransactionLocked(transactionId)) return false;
            
            string makerId = GetMakerIdForTransaction(transactionId);
            if (!ValidateMakerCheckerSeparation(makerId, checkerId)) return false;

            _transactionStatuses[transactionId] = "APPROVED";
            _lockedTransactions.Add(transactionId);
            return true;
        }

        public bool RejectTransaction(string transactionId, string checkerId, string rejectionReason)
        {
            if (IsTransactionLocked(transactionId)) return false;
            if (string.IsNullOrWhiteSpace(rejectionReason)) return false;

            _transactionStatuses[transactionId] = "REJECTED";
            _lockedTransactions.Add(transactionId);
            return true;
        }

        public bool CheckEscalationEligibility(string transactionId, int daysPending)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return false;
            
            // Escalate if pending for more than 5 days and not already locked
            return daysPending > 5 && !IsTransactionLocked(transactionId);
        }

        public decimal CalculateMaturityPayout(string policyId, decimal baseAmount, double bonusRate)
        {
            if (baseAmount < 0) throw new ArgumentException("Base amount cannot be negative");
            
            decimal bonusAmount = baseAmount * (decimal)Math.Max(0, bonusRate);
            return baseAmount + bonusAmount;
        }

        public decimal GetApprovedTransactionLimit(string checkerRoleCode)
        {
            switch (checkerRoleCode?.ToUpper())
            {
                case "JUNIOR": return 25000m;
                case "SENIOR": return 100000m;
                case "MANAGER": return 500000m;
                case "DIRECTOR": return decimal.MaxValue;
                default: return 0m;
            }
        }

        public decimal CalculateTaxDeduction(decimal grossPayout, double taxRate)
        {
            if (grossPayout <= 0 || taxRate <= 0) return 0m;
            return grossPayout * (decimal)taxRate;
        }

        public decimal GetPendingApprovalTotal(string checkerId)
        {
            // Mock implementation
            return string.IsNullOrWhiteSpace(checkerId) ? 0m : 150000m;
        }

        public decimal ComputePenaltyAmount(string policyId, int daysEarly)
        {
            if (daysEarly <= 0) return 0m;
            
            decimal penalty = daysEarly * BasePenaltyRatePerDay;
            // Cap penalty at 5000
            return Math.Min(penalty, 5000m);
        }

        public decimal GetMaximumAuthorizedAmount(string checkerId)
        {
            if (string.IsNullOrWhiteSpace(checkerId)) return 0m;
            return _checkerLimits.TryGetValue(checkerId, out decimal limit) ? limit : 0m;
        }

        public double GetApprovalSuccessRate(string checkerId, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) return 0.0;
            // Mock logic
            return 0.85; // 85% success rate
        }

        public double CalculateBonusPercentage(string policyId, int activeYears)
        {
            if (activeYears < 5) return 0.0;
            if (activeYears < 10) return 0.05;
            if (activeYears < 20) return 0.10;
            return 0.15;
        }

        public double GetRejectionRatio(string departmentCode)
        {
            if (string.IsNullOrWhiteSpace(departmentCode)) return 0.0;
            // Mock logic
            return departmentCode == "RISK" ? 0.25 : 0.05;
        }

        public double CalculateRiskScore(string policyId, decimal payoutAmount)
        {
            double baseScore = payoutAmount > HighValueThreshold ? 50.0 : 10.0;
            if (policyId?.StartsWith("ULIP") == true) baseScore += 25.0;
            return Math.Min(baseScore, 100.0);
        }

        public int GetPendingTransactionsCount(string checkerId)
        {
            return string.IsNullOrWhiteSpace(checkerId) ? 0 : 12;
        }

        public int CalculateDaysInWorkflow(string transactionId, DateTime submissionDate)
        {
            if (submissionDate > DateTime.Now) return 0;
            return (DateTime.Now - submissionDate).Days;
        }

        public int GetEscalationLevel(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return 0;
            // Mock logic based on transaction ID
            return transactionId.Contains("ESC") ? 2 : 0;
        }

        public int CountRequiredSignatures(decimal payoutAmount)
        {
            if (payoutAmount < 10000m) return 1;
            if (payoutAmount < HighValueThreshold) return 2;
            if (payoutAmount < 500000m) return 3;
            return 4;
        }

        public int GetRemainingApprovalWindow(string transactionId, DateTime currentDate)
        {
            // Assuming submission was 3 days ago for mock purposes
            DateTime submissionDate = currentDate.AddDays(-3);
            int daysPassed = (currentDate - submissionDate).Days;
            return Math.Max(0, MaxApprovalWindowDays - daysPassed);
        }

        public string GenerateWorkflowRoutingCode(string policyType, decimal amount)
        {
            string typeCode = string.IsNullOrWhiteSpace(policyType) ? "GEN" : policyType.Substring(0, Math.Min(3, policyType.Length)).ToUpper();
            string valueCode = IsHighValueTransaction(amount) ? "HV" : "LV";
            return $"{typeCode}-{valueCode}-{DateTime.UtcNow:yyyyMMdd}";
        }

        public string GetTransactionStatus(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return "UNKNOWN";
            return _transactionStatuses.TryGetValue(transactionId, out string status) ? status : "PENDING";
        }

        public string AssignCheckerToTransaction(string transactionId, string departmentCode)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) throw new ArgumentException("Invalid transaction ID");
            
            // Simple round-robin or logic-based assignment mock
            string assignedChecker = departmentCode == "VIP" ? "CHK-MGR" : "CHK-001";
            return assignedChecker;
        }

        public string GetMakerIdForTransaction(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return null;
            return _transactionMakers.TryGetValue(transactionId, out string makerId) ? makerId : "MKR-DEFAULT";
        }
    }
}