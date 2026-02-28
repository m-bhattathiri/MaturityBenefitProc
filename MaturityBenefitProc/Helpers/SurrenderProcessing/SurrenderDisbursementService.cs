using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    // Fixed implementation — correct business logic
    public class SurrenderDisbursementService : ISurrenderDisbursementService
    {
        private const int PROCESSING_DEADLINE_DAYS = 30;
        private const decimal MINIMUM_DISBURSEMENT_AMOUNT = 10.00m;
        private readonly HashSet<string> _communityPropertyStates = new HashSet<string>(StringComparer.OrdinalIgnoreCase) 
        { 
            "AZ", "CA", "ID", "LA", "NV", "NM", "TX", "WA", "WI" 
        };

        public decimal CalculateTotalSurrenderValue(string policyId, DateTime effectiveDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));

            // Mock base values for demonstration
            decimal baseValue = 50000m; 
            decimal proratedDividends = CalculateProratedDividends(policyId, effectiveDate);
            decimal mva = CalculateMarketValueAdjustment(policyId, baseValue);
            decimal penalties = CalculatePenalties(policyId, baseValue);
            decimal loanBalance = GetOutstandingLoanBalance(policyId);

            decimal totalSurrenderValue = (baseValue + proratedDividends + mva) - penalties - loanBalance;
            return Math.Max(0m, totalSurrenderValue);
        }

        public decimal CalculatePenalties(string policyId, decimal baseValue)
        {
            if (baseValue <= 0) return 0m;
            int policyYears = GetActivePolicyMonths(policyId) / 12;
            double penaltyRate = GetSurrenderChargeRate(policyId, policyYears);
            return baseValue * (decimal)penaltyRate;
        }

        public decimal GetOutstandingLoanBalance(string policyId)
        {
            // Mock logic: return a simulated loan balance based on policy ID length
            return string.IsNullOrEmpty(policyId) ? 0m : (policyId.Length * 150.50m);
        }

        public decimal CalculateTaxWithholding(decimal taxableAmount, double taxRate)
        {
            if (taxableAmount <= 0 || taxRate <= 0) return 0m;
            return Math.Round(taxableAmount * (decimal)taxRate, 2);
        }

        public decimal GetFinalDisbursementAmount(string policyId)
        {
            decimal totalValue = CalculateTotalSurrenderValue(policyId, DateTime.UtcNow);
            double taxRate = GetApplicableTaxRate("TX"); // Defaulting to TX for mock
            decimal taxWithholding = CalculateTaxWithholding(totalValue, taxRate);
            
            decimal finalAmount = totalValue - taxWithholding;
            return finalAmount > 0 ? finalAmount : 0m;
        }

        public decimal CalculateProratedDividends(string policyId, DateTime surrenderDate)
        {
            // Mock annual dividend of $1200
            decimal annualDividend = 1200m;
            int dayOfYear = surrenderDate.DayOfYear;
            int daysInYear = DateTime.IsLeapYear(surrenderDate.Year) ? 366 : 365;
            
            return Math.Round(annualDividend * ((decimal)dayOfYear / daysInYear), 2);
        }

        public decimal CalculateMarketValueAdjustment(string policyId, decimal currentFundValue)
        {
            // Mock MVA calculation (+/- 2% based on policy ID hash)
            if (currentFundValue <= 0) return 0m;
            int hash = policyId?.GetHashCode() ?? 0;
            decimal adjustmentFactor = (hash % 2 == 0) ? 0.02m : -0.02m;
            return Math.Round(currentFundValue * adjustmentFactor, 2);
        }

        public double GetSurrenderChargeRate(string policyId, int policyYears)
        {
            if (policyYears < 0) return 0.10; // 10% max
            if (policyYears >= 10) return 0.0; // No charge after 10 years
            
            // Sliding scale: 10% year 0, decreasing by 1% each year
            return (10 - policyYears) / 100.0;
        }

        public double GetApplicableTaxRate(string stateCode)
        {
            if (string.IsNullOrWhiteSpace(stateCode)) return 0.20; // Default 20% federal

            if (stateCode.ToUpper() == "CA")
            {
                return 0.25;
            }
            else if (stateCode.ToUpper() == "NY")
            {
                return 0.24;
            }
            else if (stateCode.ToUpper() == "TX")
            {
                return 0.20;
            }
            else if (stateCode.ToUpper() == "FL")
            {
                return 0.20;
            }
            else
            {
                return 0.22;
            }
        }

        public double CalculateVestingPercentage(string policyId, int monthsActive)
        {
            if (monthsActive < 12) return 0.0;
            if (monthsActive >= 60) return 1.0;
            
            // 20% per year after year 1
            return (monthsActive / 12) * 0.20;
        }

        public double GetInterestRateForDelayedPayment(string policyId)
        {
            return 0.035; // 3.5% statutory interest rate
        }

        public bool IsEligibleForSurrender(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            return CountPendingDisbursementHolds(policyId) == 0 && GetActivePolicyMonths(policyId) > 0;
        }

        public bool ValidateBankRoutingNumber(string routingNumber)
        {
            if (string.IsNullOrWhiteSpace(routingNumber) || routingNumber.Length != 9) return false;
            if (!routingNumber.All(char.IsDigit)) return false;

            // Simple checksum validation for US routing numbers
            int[] weights = { 3, 7, 1, 3, 7, 1, 3, 7, 1 };
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += (routingNumber[i] - '0') * weights[i];
            }
            return sum % 10 == 0;
        }

        public bool RequiresSpousalConsent(string policyId, string stateCode)
        {
            if (string.IsNullOrWhiteSpace(stateCode)) return false;
            return _communityPropertyStates.Contains(stateCode);
        }

        public bool HasActiveGarnishments(string policyId)
        {
            // Mock logic
            return policyId?.EndsWith("-G") ?? false;
        }

        public bool IsDisbursementApproved(string policyId, decimal amount)
        {
            if (amount < MINIMUM_DISBURSEMENT_AMOUNT) return false;
            if (HasActiveGarnishments(policyId)) return false;
            return IsEligibleForSurrender(policyId);
        }

        public bool VerifyBeneficiarySignatures(string policyId, int requiredSignatures)
        {
            if (requiredSignatures <= 0) return true;
            // Mock logic: assume we always have 1 signature on file
            int signaturesOnFile = 1; 
            return signaturesOnFile >= requiredSignatures;
        }

        public int GetDaysUntilProcessingDeadline(DateTime requestDate)
        {
            DateTime deadline = requestDate.AddDays(PROCESSING_DEADLINE_DAYS);
            int daysRemaining = (deadline - DateTime.UtcNow).Days;
            return Math.Max(0, daysRemaining);
        }

        public int GetActivePolicyMonths(string policyId)
        {
            // Mock logic: 5 years active
            return 60;
        }

        public int CountPendingDisbursementHolds(string policyId)
        {
            // Mock logic
            return policyId?.Contains("HOLD") == true ? 1 : 0;
        }

        public int GetGracePeriodDays(string policyId)
        {
            return 31; // Standard insurance grace period
        }

        public string GenerateDisbursementTransactionId(string policyId, DateTime processingDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID required.");
            return $"SUR-{policyId.ToUpper()}-{processingDate:yyyyMMddHHmmss}";
        }

        public string GetTaxFormTypeCode(decimal disbursementAmount, bool isQualifiedPlan)
        {
            if (disbursementAmount < 10m) return "NONE";
            return isQualifiedPlan ? "1099-R-QUAL" : "1099-R-NONQUAL";
        }

        public string GetPaymentMethodCode(string policyId)
        {
            // Mock logic
            return policyId?.StartsWith("E-") == true ? "ACH" : "CHK";
        }

        public string DetermineDisbursementStatus(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return "INVALID";
            if (transactionId.Contains("HOLD")) return "PENDING_REVIEW";
            return "PROCESSED";
        }
    }
}