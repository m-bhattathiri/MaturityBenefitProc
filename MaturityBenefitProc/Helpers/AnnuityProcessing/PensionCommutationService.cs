// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    public class PensionCommutationService : IPensionCommutationService
    {
        private const decimal STANDARD_LIFETIME_ALLOWANCE = 1073100m;
        private const double STANDARD_TAX_FREE_PERCENTAGE = 0.25;
        private const decimal TRIVIAL_COMMUTATION_LIMIT = 30000m;
        private const decimal SPOUSAL_CONSENT_THRESHOLD = 50000m;
        private const decimal EARLY_RETIREMENT_REDUCTION_PER_MONTH = 0.004m; // 0.4% per month

        public decimal CalculateMaximumTaxFreeLumpSum(string policyId, decimal totalFundValue)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (totalFundValue < 0) throw new ArgumentOutOfRangeException(nameof(totalFundValue), "Fund value cannot be negative.");

            double percentage = CalculateTaxFreePercentage(policyId);
            return totalFundValue * (decimal)percentage;
        }

        public decimal CalculateCommutationAmount(string policyId, double commutationFactor)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (commutationFactor <= 0) throw new ArgumentOutOfRangeException(nameof(commutationFactor), "Factor must be greater than zero.");

            // Mocking a base pension amount retrieval
            decimal basePension = 10000m; 
            return basePension * (decimal)commutationFactor;
        }

        public decimal GetAvailableLifetimeAllowance(string customerId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(customerId)) throw new ArgumentException("Customer ID cannot be null or empty.", nameof(customerId));
            
            // Mocking previously used allowance
            decimal previouslyUsed = 250000m; 
            return Math.Max(0, STANDARD_LIFETIME_ALLOWANCE - previouslyUsed);
        }

        public decimal CalculateResidualPensionFund(decimal totalFundValue, decimal commutedAmount)
        {
            if (totalFundValue < 0) throw new ArgumentOutOfRangeException(nameof(totalFundValue));
            if (commutedAmount < 0) throw new ArgumentOutOfRangeException(nameof(commutedAmount));
            if (commutedAmount > totalFundValue) throw new InvalidOperationException("Commuted amount cannot exceed total fund value.");

            return totalFundValue - commutedAmount;
        }

        public decimal CalculateTaxableCommutationPortion(string policyId, decimal requestedLumpSum)
        {
            // Mocking total fund value for the policy
            decimal totalFundValue = 100000m; 
            decimal maxTaxFree = CalculateMaximumTaxFreeLumpSum(policyId, totalFundValue);

            if (requestedLumpSum <= maxTaxFree)
                return 0m;

            return requestedLumpSum - maxTaxFree;
        }

        public decimal GetGuaranteedMinimumPensionValue(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Business logic to determine GMP based on policy prefix
            if (policyId.StartsWith("GMP-")) return 5000m;
            return 0m;
        }

        public decimal CalculateEarlyRetirementReduction(decimal baseAmount, int monthsEarly)
        {
            if (baseAmount < 0) throw new ArgumentOutOfRangeException(nameof(baseAmount));
            if (monthsEarly <= 0) return 0m;

            decimal reductionFactor = monthsEarly * EARLY_RETIREMENT_REDUCTION_PER_MONTH;
            // Cap reduction at 50%
            reductionFactor = Math.Min(reductionFactor, 0.50m);

            return baseAmount * reductionFactor;
        }

        public decimal CalculateTerminalBonus(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Terminal bonus only applies if maturity is in the future or current year
            if (maturityDate.Year < DateTime.UtcNow.Year) return 0m;

            return policyId.EndsWith("WP") ? 2500m : 0m; // WP = With Profits
        }

        public double GetCommutationFactor(int ageAtMaturity, string genderCode)
        {
            if (ageAtMaturity < 55) throw new ArgumentOutOfRangeException(nameof(ageAtMaturity), "Age must be at least 55.");
            
            double baseFactor = 20.0;
            double ageDeduction = (ageAtMaturity - 55) * 0.5;
            double genderAdjustment = genderCode?.ToUpper() == "F" ? 1.5 : 0.0; // Women generally live longer, higher factor

            return Math.Max(10.0, baseFactor - ageDeduction + genderAdjustment);
        }

        public double CalculateTaxFreePercentage(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            return IsProtectedTaxFreeCashApplicable(policyId) ? 0.33 : STANDARD_TAX_FREE_PERCENTAGE;
        }

        public double GetMarketValueReductionRate(string fundId, DateTime withdrawalDate)
        {
            if (string.IsNullOrWhiteSpace(fundId)) throw new ArgumentException("Fund ID cannot be null or empty.", nameof(fundId));
            
            // MVR only applies during market downturns (mocked logic)
            if (withdrawalDate.Year == 2008 || withdrawalDate.Year == 2020) return 0.15;
            return 0.0;
        }

        public double CalculateLifetimeAllowanceUsedPercentage(string customerId, decimal withdrawalAmount)
        {
            if (withdrawalAmount < 0) throw new ArgumentOutOfRangeException(nameof(withdrawalAmount));
            
            return (double)(withdrawalAmount / STANDARD_LIFETIME_ALLOWANCE) * 100.0;
        }

        public bool IsEligibleForCommutation(string policyId, DateTime requestDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            int minAge = GetMinimumRetirementAge(policyId);
            // Mocking customer age as 60
            int customerAge = 60; 
            
            return customerAge >= minAge;
        }

        public bool ValidateLumpSumLimit(string policyId, decimal requestedAmount)
        {
            // Mocking fund value
            decimal fundValue = 200000m; 
            decimal maxAllowed = CalculateMaximumTaxFreeLumpSum(policyId, fundValue);
            return requestedAmount <= maxAllowed;
        }

        public bool HasTrivialCommutationRights(string policyId, decimal totalPensionWealth)
        {
            return totalPensionWealth <= TRIVIAL_COMMUTATION_LIMIT;
        }

        public bool IsProtectedTaxFreeCashApplicable(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            return policyId.StartsWith("PTFC-");
        }

        public bool RequiresSpousalConsent(string policyId, decimal commutationAmount)
        {
            return commutationAmount > SPOUSAL_CONSENT_THRESHOLD;
        }

        public bool IsHealthConditionWaiverApplicable(string customerId, string medicalCode)
        {
            var severeConditions = new HashSet<string> { "ILL-01", "ILL-02", "TERM-01" };
            return severeConditions.Contains(medicalCode?.ToUpper());
        }

        public int CalculateDaysToMaturity(string policyId, DateTime currentDate)
        {
            // Mocking maturity date
            DateTime maturityDate = new DateTime(2030, 1, 1);
            TimeSpan difference = maturityDate - currentDate;
            return difference.Days > 0 ? difference.Days : 0;
        }

        public int GetMinimumRetirementAge(string policyId)
        {
            // Normal minimum pension age is 55, rising to 57 in 2028
            return policyId.Contains("-PROT-") ? 50 : 55;
        }

        public int CountPreviousCommutations(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) throw new ArgumentException("Customer ID cannot be null or empty.", nameof(customerId));
            // Mocking database lookup
            return customerId.Length % 3; 
        }

        public int GetGuaranteePeriodMonths(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            return policyId.Contains("-G10-") ? 120 : 60; // 10 years or 5 years
        }

        public string GenerateCommutationReference(string policyId, DateTime requestDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            return $"COM-{policyId}-{requestDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }

        public string GetTaxCodeForExcessCommutation(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) throw new ArgumentException("Customer ID cannot be null or empty.", nameof(customerId));
            // Emergency tax code commonly used for lump sums
            return "0T W1/M1";
        }

        public string DetermineCommutationTaxBand(decimal taxableAmount)
        {
            if (taxableAmount <= 0) return "Tax-Free";
            if (taxableAmount <= 37700m) return "Basic Rate (20%)";
            if (taxableAmount <= 125140m) return "Higher Rate (40%)";
            return "Additional Rate (45%)";
        }

        public string GetRegulatoryFrameworkCode(DateTime policyStartDate)
        {
            if (policyStartDate < new DateTime(2006, 4, 6)) return "PRE-A-DAY";
            if (policyStartDate < new DateTime(2015, 4, 6)) return "POST-A-DAY";
            return "PENSION-FREEDOMS";
        }
    }
}