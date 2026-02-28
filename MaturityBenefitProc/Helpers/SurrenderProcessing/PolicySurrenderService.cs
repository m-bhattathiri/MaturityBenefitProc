using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    // Fixed implementation — correct business logic
    public class PolicySurrenderService : IPolicySurrenderService
    {
        private readonly Dictionary<string, string> _workflowStatuses = new Dictionary<string, string>();
        private const decimal AML_THRESHOLD = 10000m;

        public bool ValidatePolicyEligibility(string policyId, DateTime surrenderDate)
        {
            if (string.IsNullOrEmpty(policyId)) throw new ArgumentNullException(nameof(policyId));
            return IsPolicyInForce(policyId) && !IsIrrevocableBeneficiaryPresent(policyId);
        }

        public decimal CalculateBaseSurrenderValue(string policyId, DateTime effectiveDate)
        {
            if (string.IsNullOrEmpty(policyId)) throw new ArgumentNullException(nameof(policyId));
            // Simulated base value calculation
            return 50000m + (GetYearsInForce(policyId, effectiveDate) * 1000m);
        }

        public decimal CalculateMarketValueAdjustment(string policyId, decimal baseValue, double currentMarketRate)
        {
            double factor = GetMarketValueAdjustmentFactor(policyId, DateTime.UtcNow);
            return baseValue * (decimal)factor * (decimal)(currentMarketRate / 100.0);
        }

        public decimal CalculateSurrenderCharge(string policyId, decimal baseValue, int yearsInForce)
        {
            double rate = GetCurrentSurrenderChargeRate(policyId, yearsInForce);
            return baseValue * (decimal)rate;
        }

        public decimal CalculateTerminalBonus(string policyId, decimal baseValue)
        {
            int years = GetYearsInForce(policyId, DateTime.UtcNow);
            double rate = GetTerminalBonusRate(policyId, years);
            return baseValue * (decimal)rate;
        }

        public decimal CalculateUnearnedPremiumRefund(string policyId, DateTime surrenderDate)
        {
            double factor = GetProratedPremiumFactor(policyId, surrenderDate);
            return 1200m * (decimal)factor; // Assuming $1200 annual premium
        }

        public decimal CalculateOutstandingLoanBalance(string policyId, DateTime calculationDate)
        {
            return HasOutstandingLoans(policyId) ? 5000m : 0m;
        }

        public decimal CalculateLoanInterestAccrued(string policyId, DateTime calculationDate)
        {
            return CalculateOutstandingLoanBalance(policyId, calculationDate) * 0.05m; // 5% interest
        }

        public decimal CalculateGrossSurrenderValue(string policyId, DateTime effectiveDate)
        {
            decimal baseValue = CalculateBaseSurrenderValue(policyId, effectiveDate);
            decimal mva = CalculateMarketValueAdjustment(policyId, baseValue, 3.5);
            decimal bonus = CalculateTerminalBonus(policyId, baseValue);
            decimal refund = CalculateUnearnedPremiumRefund(policyId, effectiveDate);
            
            return baseValue + mva + bonus + refund;
        }

        public decimal CalculateNetSurrenderValue(string policyId, DateTime effectiveDate)
        {
            decimal gross = CalculateGrossSurrenderValue(policyId, effectiveDate);
            int years = GetYearsInForce(policyId, effectiveDate);
            decimal charge = CalculateSurrenderCharge(policyId, gross, years);
            decimal loan = CalculateOutstandingLoanBalance(policyId, effectiveDate);
            decimal interest = CalculateLoanInterestAccrued(policyId, effectiveDate);
            
            return Math.Max(0, gross - charge - loan - interest);
        }

        public double GetCurrentSurrenderChargeRate(string policyId, int policyYear)
        {
            if (policyYear < 0) return 0.0;
            if (policyYear <= 1) return 0.10;
            if (policyYear <= 5) return 0.05;
            if (policyYear <= 10) return 0.02;
            return 0.0;
        }

        public double GetMarketValueAdjustmentFactor(string policyId, DateTime calculationDate)
        {
            return 1.05; // Simulated factor
        }

        public double GetTerminalBonusRate(string policyId, int yearsInForce)
        {
            return yearsInForce > 10 ? 0.05 : 0.0;
        }

        public double GetTaxWithholdingRate(string policyId, string stateCode)
        {
            return stateCode == "CA" ? 0.10 : 0.20; // Federal + State mock
        }

        public double GetProratedPremiumFactor(string policyId, DateTime surrenderDate)
        {
            int daysPassed = surrenderDate.DayOfYear;
            return Math.Max(0, (365 - daysPassed) / 365.0);
        }

        public bool IsPolicyInForce(string policyId) => !string.IsNullOrEmpty(policyId) && policyId.StartsWith("POL");
        public bool HasOutstandingLoans(string policyId) => policyId.EndsWith("L");
        public bool IsWithinFreeLookPeriod(string policyId, DateTime requestDate) => GetFreeLookDaysRemaining(policyId, requestDate) > 0;
        public bool RequiresSpousalConsent(string policyId, string stateCode) => stateCode == "CA" || stateCode == "TX";
        public bool IsIrrevocableBeneficiaryPresent(string policyId) => policyId.Contains("IRR");
        public bool ValidateSignatureRequirements(string policyId, string documentId) => !string.IsNullOrEmpty(documentId);
        public bool CheckAntiMoneyLaunderingStatus(string policyId, decimal netSurrenderValue) => netSurrenderValue < AML_THRESHOLD;
        public bool IsVestingScheduleMet(string policyId, DateTime requestDate) => GetYearsInForce(policyId, requestDate) >= 3;

        public int GetYearsInForce(string policyId, DateTime surrenderDate)
        {
            // Mock issue date 5 years ago
            DateTime issueDate = DateTime.UtcNow.AddYears(-5);
            return Math.Max(0, surrenderDate.Year - issueDate.Year);
        }

        public int GetDaysToNextAnniversary(string policyId, DateTime currentDate)
        {
            DateTime nextAnniversary = new DateTime(currentDate.Year, 1, 1).AddYears(1);
            return (nextAnniversary - currentDate).Days;
        }

        public int GetRemainingSurrenderChargeYears(string policyId)
        {
            int yearsInForce = GetYearsInForce(policyId, DateTime.UtcNow);
            return Math.Max(0, 10 - yearsInForce);
        }

        public int GetFreeLookDaysRemaining(string policyId, DateTime requestDate)
        {
            DateTime issueDate = DateTime.UtcNow.AddDays(-10);
            int daysPassed = (requestDate - issueDate).Days;
            return Math.Max(0, 30 - daysPassed);
        }

        public int GetActiveLoanCount(string policyId) => HasOutstandingLoans(policyId) ? 1 : 0;

        public string InitiateSurrenderWorkflow(string policyId, string requestedBy)
        {
            string workflowId = $"WF-{Guid.NewGuid().ToString().Substring(0, 8)}";
            _workflowStatuses[workflowId] = "Initiated";
            return workflowId;
        }

        public string GetSurrenderStatus(string workflowId)
        {
            return _workflowStatuses.TryGetValue(workflowId, out var status) ? status : "NotFound";
        }

        public string GenerateSurrenderQuoteId(string policyId, DateTime quoteDate)
        {
            return $"SQ-{policyId}-{quoteDate:yyyyMMdd}";
        }

        public string GetTaxFormRequirement(string policyId, decimal taxableAmount)
        {
            return taxableAmount > 10 ? "1099-R" : "None";
        }

        public string DeterminePaymentRoutingCode(string policyId, string bankId)
        {
            return $"ROUT-{bankId.PadLeft(9, '0')}";
        }

        public string GetStateOfIssue(string policyId) => "NY";
        public string GetProductCode(string policyId) => "UL-100";

        public bool ApproveSurrenderRequest(string workflowId, string approverId)
        {
            if (_workflowStatuses.ContainsKey(workflowId))
            {
                _workflowStatuses[workflowId] = "Approved";
                return true;
            }
            return false;
        }

        public bool RejectSurrenderRequest(string workflowId, string reasonCode, string rejectedBy)
        {
            if (_workflowStatuses.ContainsKey(workflowId))
            {
                _workflowStatuses[workflowId] = $"Rejected: {reasonCode}";
                return true;
            }
            return false;
        }

        public bool SuspendSurrenderWorkflow(string workflowId, string reasonCode)
        {
            if (_workflowStatuses.ContainsKey(workflowId))
            {
                _workflowStatuses[workflowId] = $"Suspended: {reasonCode}";
                return true;
            }
            return false;
        }

        public bool ResumeSurrenderWorkflow(string workflowId)
        {
            if (_workflowStatuses.ContainsKey(workflowId) && _workflowStatuses[workflowId].StartsWith("Suspended"))
            {
                _workflowStatuses[workflowId] = "In Progress";
                return true;
            }
            return false;
        }

        public string FinalizeSurrenderTransaction(string workflowId, DateTime processingDate)
        {
            if (_workflowStatuses.TryGetValue(workflowId, out var status) && status == "Approved")
            {
                _workflowStatuses[workflowId] = "Completed";
                return $"TXN-{Guid.NewGuid()}";
            }
            throw new InvalidOperationException("Workflow must be approved before finalizing.");
        }
    }
}