using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    // Fixed implementation — correct business logic
    public class SurrenderPenaltyService : ISurrenderPenaltyService
    {
        private const decimal AdminFee = 50.00m;
        private const decimal ManagerApprovalThreshold = 10000.00m;
        private const int LockInPeriodDays = 1095; // 3 years
        
        private readonly HashSet<string> _validWaiverCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "DEATH", "CRITICAL_ILLNESS", "TERMINAL_ILLNESS", "HARDSHIP"
        };

        public decimal CalculateBasePenaltyAmount(string policyId, decimal surrenderValue)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            if (surrenderValue <= 0) return 0m;

            // Base penalty is 5% of the surrender value, capped at 5000
            decimal penalty = surrenderValue * 0.05m;
            return Math.Min(penalty, 5000m);
        }

        public double GetPenaltyPercentage(string policyId, DateTime surrenderDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            
            int completedYears = GetCompletedPolicyYears(policyId, surrenderDate);
            
            if (completedYears < 1) return 10.0;
            if (completedYears < 3) return 7.5;
            if (completedYears < 5) return 5.0;
            if (completedYears < 7) return 2.5;
            
            return 0.0; // No penalty after 7 years
        }

        public bool IsEligibleForPenaltyWaiver(string policyId, string waiverCode)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(waiverCode)) 
                return false;

            return _validWaiverCodes.Contains(waiverCode);
        }

        public int GetRemainingLockInDays(string policyId, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));

            DateTime mockIssueDate = GetMockIssueDate(policyId);
            int daysActive = (int)(currentDate - mockIssueDate).TotalDays;
            
            int remainingDays = LockInPeriodDays - daysActive;
            return remainingDays > 0 ? remainingDays : 0;
        }

        public string GetApplicablePenaltyTierCode(string policyId, int activeYears)
        {
            if (activeYears < 0) throw new ArgumentOutOfRangeException(nameof(activeYears));

            if (activeYears < 2) return "TIER_1_HIGH";
            if (activeYears < 5) return "TIER_2_MED";
            if (activeYears < 10) return "TIER_3_LOW";
            
            return "TIER_4_NONE";
        }

        public decimal CalculateMarketValueAdjustment(string policyId, decimal currentFundValue, double marketRate)
        {
            if (currentFundValue <= 0) return 0m;

            // MVA = Fund Value * (Guaranteed Rate - Market Rate)
            // Assuming a fixed guaranteed rate of 3.0% (0.03) for this example
            double guaranteedRate = 0.03;
            double adjustmentFactor = guaranteedRate - marketRate;

            // Only apply negative adjustments (penalties)
            if (adjustmentFactor >= 0) return 0m;

            return Math.Round(currentFundValue * (decimal)Math.Abs(adjustmentFactor), 2);
        }

        public decimal GetTotalDeductionCharges(string policyId, DateTime effectiveDate)
        {
            decimal basePenalty = CalculateBasePenaltyAmount(policyId, 10000m); // Mocking 10k value
            decimal mva = CalculateMarketValueAdjustment(policyId, 10000m, 0.04);
            
            return basePenalty + mva + AdminFee;
        }

        public bool ValidateSurrenderDate(string policyId, DateTime requestedDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            
            DateTime issueDate = GetMockIssueDate(policyId);
            DateTime today = DateTime.Today;

            // Surrender date cannot be before issue date and cannot be more than 30 days in the future
            return requestedDate >= issueDate && requestedDate <= today.AddDays(30);
        }

        public double CalculateProratedBonusRecoveryRate(string policyId, int monthsActive)
        {
            if (monthsActive < 0) return 0.0;
            if (monthsActive >= 60) return 0.0; // No recovery after 5 years

            // Recover 100% in year 1, decreasing by 20% each year
            double recoveryRate = 1.0 - (monthsActive / 12 * 0.20);
            return Math.Max(0.0, recoveryRate);
        }

        public decimal CalculateTaxWithholdingAmount(string policyId, decimal netSurrenderAmount, double taxRate)
        {
            if (netSurrenderAmount <= 0 || taxRate <= 0) return 0m;
            if (taxRate > 1.0) throw new ArgumentOutOfRangeException(nameof(taxRate), "Tax rate cannot exceed 100%.");

            return Math.Round(netSurrenderAmount * (decimal)taxRate, 2);
        }

        public int GetCompletedPolicyYears(string policyId, DateTime surrenderDate)
        {
            DateTime issueDate = GetMockIssueDate(policyId);
            if (surrenderDate < issueDate) return 0;

            int years = surrenderDate.Year - issueDate.Year;
            if (surrenderDate.Date < issueDate.AddYears(years).Date)
            {
                years--;
            }
            return Math.Max(0, years);
        }

        public string RetrievePenaltyRuleId(string productCode, DateTime issueDate)
        {
            if (string.IsNullOrWhiteSpace(productCode)) throw new ArgumentNullException(nameof(productCode));
            
            return $"PR-{productCode.ToUpper()}-{issueDate.Year}";
        }

        public bool HasOutstandingLoans(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Mock logic: policies ending in 'L' have loans
            return policyId.EndsWith("L", StringComparison.OrdinalIgnoreCase);
        }

        public decimal CalculateLoanInterestDeduction(string policyId, DateTime calculationDate)
        {
            if (!HasOutstandingLoans(policyId)) return 0m;

            // Mock outstanding loan amount and interest calculation
            decimal outstandingPrincipal = 5000m; 
            decimal annualInterestRate = 0.06m;
            
            return Math.Round(outstandingPrincipal * annualInterestRate / 12, 2); // 1 month interest
        }

        public double GetSurrenderChargeFactor(string policyId, int durationInMonths)
        {
            if (durationInMonths < 0) return 1.0;
            if (durationInMonths >= 120) return 0.0; // 10 years

            // Linear decrease over 120 months
            return 1.0 - (durationInMonths / 120.0);
        }

        public decimal CalculateFinalNetSurrenderValue(string policyId, decimal grossValue, decimal totalPenalties)
        {
            if (grossValue < 0) throw new ArgumentOutOfRangeException(nameof(grossValue));
            if (totalPenalties < 0) throw new ArgumentOutOfRangeException(nameof(totalPenalties));

            decimal netValue = grossValue - totalPenalties;
            return Math.Max(0m, netValue); // Cannot be negative
        }

        public bool RequiresManagerApproval(string policyId, decimal penaltyAmount)
        {
            return penaltyAmount >= ManagerApprovalThreshold;
        }

        public int GetFreeWithdrawalCount(string policyId, DateTime currentYearStart)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;
            
            // Standard policies get 1 free withdrawal per year, premium gets 3
            bool isPremium = policyId.StartsWith("PRM", StringComparison.OrdinalIgnoreCase);
            return isPremium ? 3 : 1;
        }

        // Helper method to mock an issue date based on the policy ID length
        private DateTime GetMockIssueDate(string policyId)
        {
            int yearsAgo = Math.Max(1, policyId.Length % 10);
            return DateTime.Today.AddYears(-yearsAgo).AddDays(-15);
        }
    }
}