using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    // Fixed implementation — correct business logic
    public class LoyaltyAdditionService : ILoyaltyAdditionService
    {
        private const decimal MINIMUM_SUM_ASSURED = 10000m;
        private const double BASE_MULTIPLIER = 1.0;
        private const int DAYS_IN_YEAR = 365;

        public decimal CalculateBaseLoyaltyAddition(string policyId, decimal sumAssured, int premiumTerm)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.");
            if (sumAssured < MINIMUM_SUM_ASSURED) return 0m;
            if (premiumTerm <= 0) return 0m;

            // Base calculation: 0.5% of sum assured per year of premium term
            decimal ratePerYear = 0.005m;
            return sumAssured * ratePerYear * premiumTerm;
        }

        public double GetLoyaltyAdditionRate(string productCode, int completedYears)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return 0.0;
            if (completedYears < 5) return 0.0; // No loyalty addition before 5 years

            double baseRate = productCode.StartsWith("ULIP") ? 0.02 : 0.015;
            double tenureBonus = (completedYears - 5) * 0.001;
            
            return Math.Min(baseRate + tenureBonus, 0.05); // Cap at 5%
        }

        public bool IsEligibleForLoyaltyAddition(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Eligible if maturity date is in the past or within the next 30 days
            return maturityDate <= DateTime.Now.AddDays(30);
        }

        public int GetCompletedPremiumYears(string policyId, DateTime inceptionDate)
        {
            if (inceptionDate > DateTime.Now) return 0;
            
            int years = DateTime.Now.Year - inceptionDate.Year;
            if (DateTime.Now.Date < inceptionDate.AddYears(years).Date)
            {
                years--;
            }
            return Math.Max(0, years);
        }

        public string GenerateLoyaltyTransactionId(string policyId, DateTime processingDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID required.");
            string datePart = processingDate.ToString("yyyyMMddHHmmss");
            string randomPart = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            return $"LA-{policyId}-{datePart}-{randomPart}";
        }

        public decimal ComputeFinalLoyaltyAmount(string policyId, decimal baseAmount, double multiplier)
        {
            if (baseAmount < 0) return 0m;
            double safeMultiplier = Math.Max(0, multiplier);
            return Math.Round(baseAmount * (decimal)safeMultiplier, 2);
        }

        public bool HasCompletedPremiumTerm(string policyId, int requiredTerm)
        {
            if (requiredTerm <= 0) return true;
            // Mocking a lookup for completed years, normally this would hit a DB
            int completedYears = GetMissedPremiumCount(policyId) == 0 ? requiredTerm : requiredTerm - 1;
            return completedYears >= requiredTerm;
        }

        public double CalculateLoyaltyMultiplier(int persistencyScore, double baseRate)
        {
            if (persistencyScore < 50) return baseRate * 0.5; // Penalty for poor persistency
            if (persistencyScore >= 90) return baseRate * 1.2; // Bonus for excellent persistency
            return baseRate;
        }

        public decimal GetAccruedLoyaltyValue(string policyId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId) || calculationDate > DateTime.Now) return 0m;
            // Mock calculation based on policy ID length for demonstration
            return policyId.Length * 150.50m;
        }

        public int CalculateDaysSinceLastAddition(string policyId, DateTime currentDate)
        {
            // Mocking last addition date as exactly one year ago for active policies
            DateTime lastAdditionDate = currentDate.AddYears(-1);
            return (currentDate - lastAdditionDate).Days;
        }

        public string GetLoyaltyFundCode(string productCategory, int issueYear)
        {
            if (string.IsNullOrWhiteSpace(productCategory)) return "DEFAULT-FUND";
            string categoryCode = productCategory.Substring(0, Math.Min(3, productCategory.Length)).ToUpper();
            return $"LF-{categoryCode}-{issueYear}";
        }

        public bool ValidateLoyaltyAdditionRules(string policyId, decimal calculatedAmount)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            if (calculatedAmount <= 0) return false;
            if (calculatedAmount > 500000m) return false; // Hard cap on loyalty additions
            return true;
        }

        public decimal CalculateProratedAddition(string policyId, decimal annualAmount, int daysActive)
        {
            if (annualAmount <= 0 || daysActive <= 0) return 0m;
            if (daysActive >= DAYS_IN_YEAR) return annualAmount;
            
            decimal dailyRate = annualAmount / DAYS_IN_YEAR;
            return Math.Round(dailyRate * daysActive, 2);
        }

        public double GetBonusInterestRate(string fundCode, DateTime effectiveDate)
        {
            if (string.IsNullOrWhiteSpace(fundCode)) return 0.0;
            
            // Base rates depending on fund type
            if (fundCode.Contains("EQ")) return 0.08; // Equity
            if (fundCode.Contains("DB")) return 0.05; // Debt
            return 0.04; // Default/Conservative
        }

        public int GetMissedPremiumCount(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;
            // Mock logic: policies ending in 'X' have missed premiums
            return policyId.EndsWith("X", StringComparison.OrdinalIgnoreCase) ? 2 : 0;
        }

        public string DetermineLoyaltyCategory(int completedYears, decimal sumAssured)
        {
            if (completedYears >= 15 && sumAssured >= 1000000m) return "PLATINUM";
            if (completedYears >= 10 && sumAssured >= 500000m) return "GOLD";
            if (completedYears >= 5) return "SILVER";
            return "BRONZE";
        }

        public decimal GetSurrenderLoyaltyValue(string policyId, decimal totalPremiumsPaid, double surrenderFactor)
        {
            if (totalPremiumsPaid <= 0 || surrenderFactor <= 0) return 0m;
            double safeFactor = Math.Min(surrenderFactor, 1.0); // Cannot exceed 100%
            return Math.Round(totalPremiumsPaid * (decimal)safeFactor, 2);
        }

        public bool IsPolicyInForce(string policyId, DateTime checkDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Mock logic: Assume policy is in force if not flagged with missed premiums
            return GetMissedPremiumCount(policyId) == 0;
        }

        public double ComputePersistencyRatio(string policyId, int expectedPremiums, int paidPremiums)
        {
            if (expectedPremiums <= 0) return 0.0;
            if (paidPremiums < 0) return 0.0;
            
            double ratio = (double)paidPremiums / expectedPremiums;
            return Math.Min(ratio, 1.0) * 100; // Return as percentage, max 100%
        }

        public int GetLoyaltyTierLevel(double persistencyRatio)
        {
            if (persistencyRatio >= 95.0) return 5;
            if (persistencyRatio >= 85.0) return 4;
            if (persistencyRatio >= 75.0) return 3;
            if (persistencyRatio >= 60.0) return 2;
            return 1;
        }

        public decimal CalculateSpecialLoyaltyBonus(string policyId, decimal baseLoyalty, bool isHighValuePolicy)
        {
            if (baseLoyalty <= 0) return 0m;
            decimal bonusRate = isHighValuePolicy ? 0.15m : 0.05m;
            return Math.Round(baseLoyalty * bonusRate, 2);
        }

        public bool CheckSpecialBonusEligibility(string policyId, int tierLevel)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Only top tiers are eligible for special bonuses
            return tierLevel >= 4;
        }

        public string GetApprovalStatusCode(decimal finalLoyaltyAmount, int tierLevel)
        {
            if (finalLoyaltyAmount < 0) return "REJECTED";
            if (finalLoyaltyAmount == 0) return "NOT_ELIGIBLE";
            
            // High amounts or low tiers require manual review
            if (finalLoyaltyAmount > 100000m || tierLevel <= 2)
            {
                return "PENDING_MANUAL_REVIEW";
            }
            
            return "AUTO_APPROVED";
        }

        public decimal ApplyTaxDeductionsToLoyalty(string policyId, decimal grossLoyaltyAmount, double taxRate)
        {
            if (grossLoyaltyAmount <= 0) return 0m;
            
            double safeTaxRate = Math.Max(0, Math.Min(taxRate, 0.5)); // Cap tax between 0% and 50%
            decimal taxAmount = grossLoyaltyAmount * (decimal)safeTaxRate;
            
            return Math.Round(grossLoyaltyAmount - taxAmount, 2);
        }
    }
}