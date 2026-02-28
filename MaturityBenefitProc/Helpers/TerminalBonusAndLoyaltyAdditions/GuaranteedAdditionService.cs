using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    // Fixed implementation — correct business logic
    public class GuaranteedAdditionService : IGuaranteedAdditionService
    {
        private readonly HashSet<string> _eligibleProducts = new HashSet<string> { "PRD-001", "PRD-002", "PRD-005" };
        private const decimal DaysInYear = 365.25m;

        public decimal CalculateTotalGuaranteedAdditions(string policyId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.");
            
            // Mocking a retrieval of policy years. In a real app, this comes from a repository.
            int completedYears = GetCompletedPolicyYears(policyId, calculationDate);
            decimal totalAdditions = 0m;

            for (int year = 1; year <= completedYears; year++)
            {
                totalAdditions += CalculateAccruedAdditionsForYear(policyId, year);
            }

            return totalAdditions;
        }

        public decimal CalculateAccruedAdditionsForYear(string policyId, int policyYear)
        {
            if (policyYear <= 0) return 0m;

            // Mocking sum assured and rate
            decimal sumAssured = GetSumAssuredForAdditions(policyId, DateTime.UtcNow);
            double rate = GetApplicableAdditionRate("PRD-001", 10); // Defaulting for mock

            return sumAssured * (decimal)rate;
        }

        public bool IsPolicyEligibleForGuaranteedAdditions(string policyId, string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return false;
            return _eligibleProducts.Contains(productCode.ToUpperInvariant());
        }

        public double GetApplicableAdditionRate(string productCode, int policyTerm)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return 0.0;

            if (productCode.ToUpperInvariant() == "PRD-001")
            {
                return 0.03;
            }
            else if (productCode.ToUpperInvariant() == "PRD-002")
            {
                return 0.045;
            }
            else if (productCode.ToUpperInvariant() == "PRD-005")
            {
                return 0.06;
            }
            else
            {
                return 0.0;
            }
        }

        public int GetAccrualPeriodInDays(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate) return 0;
            return (endDate - startDate).Days;
        }

        public string GetAdditionCalculationBasisCode(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return "UNKNOWN";
            
            if (productCode.ToUpperInvariant() == "PRD-001")
            {
                return "SUM_ASSURED_BASIS";
            }
            else if (productCode.ToUpperInvariant() == "PRD-002")
            {
                return "PREMIUM_BASIS";
            }
            else
            {
                return "STANDARD_BASIS";
            }
        }

        public decimal CalculateProRataAdditions(string policyId, decimal baseAmount, int daysActive)
        {
            if (daysActive <= 0 || baseAmount <= 0) return 0m;
            return Math.Round(baseAmount * (daysActive / DaysInYear), 2);
        }

        public bool ValidateAdditionRateLimits(double rate, string productCode)
        {
            double maxRate = productCode == "PRD-005" ? 0.10 : 0.08;
            return rate >= 0.0 && rate <= maxRate;
        }

        public decimal GetSumAssuredForAdditions(string policyId, DateTime effectiveDate)
        {
            // Mock implementation: generate a deterministic sum assured based on policyId length
            if (string.IsNullOrWhiteSpace(policyId)) return 0m;
            return 10000m + (policyId.Length * 1000m);
        }

        public int GetCompletedPolicyYears(string policyId, DateTime maturityDate)
        {
            // Mock issue date 10 years prior to maturity
            DateTime issueDate = maturityDate.AddYears(-10);
            if (DateTime.UtcNow < issueDate) return 0;

            int years = DateTime.UtcNow.Year - issueDate.Year;
            if (DateTime.UtcNow.DayOfYear < issueDate.DayOfYear) years--;
            
            return Math.Max(0, years);
        }

        public double CalculateVestingPercentage(int completedYears, int totalTerm)
        {
            if (totalTerm <= 0 || completedYears <= 0) return 0.0;
            if (completedYears >= totalTerm) return 1.0;

            // Simple linear vesting
            return Math.Round((double)completedYears / totalTerm, 4);
        }

        public decimal CalculateVestedGuaranteedAdditions(string policyId, decimal totalAdditions, double vestingPercentage)
        {
            if (totalAdditions <= 0 || vestingPercentage <= 0) return 0m;
            double clampedVesting = Math.Min(1.0, Math.Max(0.0, vestingPercentage));
            return Math.Round(totalAdditions * (decimal)clampedVesting, 2);
        }

        public string RetrieveAdditionRuleId(string productCode, DateTime issueDate)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return "DEFAULT_RULE";
            return $"RULE-{productCode.ToUpperInvariant()}-{issueDate.Year}";
        }

        public bool HasLapsedPeriodAffectingAdditions(string policyId)
        {
            // Mock logic: policies ending in 'L' have lapsed periods
            return !string.IsNullOrWhiteSpace(policyId) && policyId.EndsWith("L", StringComparison.OrdinalIgnoreCase);
        }

        public decimal DeductUnpaidPremiumsFromAdditions(decimal grossAdditions, decimal unpaidPremiums)
        {
            if (unpaidPremiums <= 0) return grossAdditions;
            return Math.Max(0m, grossAdditions - unpaidPremiums);
        }

        public int GetMissedPremiumCount(string policyId)
        {
            // Mock logic
            return HasLapsedPeriodAffectingAdditions(policyId) ? 3 : 0;
        }

        public double GetLoyaltyMultiplier(string policyId, int activeYears)
        {
            if (activeYears < 5) return 1.0;
            if (activeYears < 10) return 1.05;
            return 1.10; // 10% bonus for 10+ years
        }

        public decimal ApplyLoyaltyMultiplierToAdditions(decimal baseAdditions, double multiplier)
        {
            if (baseAdditions <= 0 || multiplier < 1.0) return baseAdditions;
            return Math.Round(baseAdditions * (decimal)multiplier, 2);
        }

        public bool CheckMinimumTermForAdditions(string policyId, int minimumYearsRequired)
        {
            // Mock check
            int actualTerm = 10; 
            return actualTerm >= minimumYearsRequired;
        }

        public string GetCurrencyCodeForAdditions(string policyId)
        {
            // Defaulting to USD for mock
            return "USD";
        }

        public decimal CalculateTerminalBonus(string policyId, decimal sumAssured, double bonusRate)
        {
            if (sumAssured <= 0 || bonusRate <= 0) return 0m;
            return Math.Round(sumAssured * (decimal)bonusRate, 2);
        }

        public double GetTerminalBonusRate(string productCode, int policyYear)
        {
            if (policyYear < 10) return 0.0; // Terminal bonuses usually apply at maturity/later years
            
            if (productCode == "PRD-001")
            {
                return 0.15;
            }
            else if (productCode == "PRD-002")
            {
                return 0.20;
            }
            else
            {
                return 0.10;
            }
        }

        public bool IsTerminalBonusGuaranteed(string productCode)
        {
            // Terminal bonuses are typically NOT guaranteed, but PRD-005 is a special case
            return productCode == "PRD-005";
        }

        public decimal CalculateSpecialGuaranteedAddition(string policyId, decimal premiumAmount)
        {
            if (premiumAmount <= 0) return 0m;
            // Flat 2% special addition on premium
            return Math.Round(premiumAmount * 0.02m, 2);
        }

        public int GetRemainingDaysToMaturity(string policyId, DateTime currentDate)
        {
            // Mock maturity date
            DateTime maturityDate = new DateTime(2030, 1, 1);
            if (currentDate >= maturityDate) return 0;
            
            return (maturityDate - currentDate).Days;
        }
    }
}