// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans
{
    public class MortalityChargeRefundService : IMortalityChargeRefundService
    {
        private const decimal MINIMUM_FUND_VALUE_THRESHOLD = 1000m;
        private const double DEFAULT_TAX_RATE = 0.18; // 18% GST for example
        private const int DAYS_IN_YEAR = 365;

        public decimal CalculateTotalRefundAmount(string policyId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId))
                throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));

            if (!IsPolicyEligibleForRefund(policyId))
                return 0m;

            if (HasPreviousRefundBeenProcessed(policyId))
                return 0m;

            decimal totalCharges = GetTotalAccumulatedCharges(policyId);
            int policyTerm = GetApplicablePolicyTerm(policyId);
            
            // Assuming product ID is derived from policy ID for this example
            string productId = policyId.Substring(0, 2); 
            double refundPercentage = GetRefundPercentage(productId, policyTerm);
            
            decimal baseRefund = totalCharges * (decimal)refundPercentage;
            
            double persistencyBonus = GetPersistencyBonusRatio(policyId);
            baseRefund += baseRefund * (decimal)persistencyBonus;

            int daysDelayed = GetDaysSinceMaturity(policyId, calculationDate);
            if (daysDelayed > 30)
            {
                decimal interest = CalculateInterestOnRefund(baseRefund, 0.06, daysDelayed - 30);
                baseRefund += interest;
            }

            decimal tax = CalculateTaxOnRefund(baseRefund, DEFAULT_TAX_RATE);
            return Math.Round(baseRefund - tax, 2);
        }

        public bool IsPolicyEligibleForRefund(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            
            // Business rule: Policy must have completed at least 10 years
            int term = GetApplicablePolicyTerm(policyId);
            if (term < 10) return false;

            // Business rule: Must not have active exclusions
            if (VerifyRiderExclusions(policyId, "WOP")) return false;

            return true;
        }

        public decimal GetMonthlyMortalityCharge(string policyId, int policyYear, int policyMonth)
        {
            if (policyYear <= 0 || policyMonth <= 0 || policyMonth > 12)
                throw new ArgumentOutOfRangeException("Invalid policy year or month.");

            // Mock logic: Charge increases slightly each year
            decimal baseCharge = 150.50m;
            return Math.Round(baseCharge * (1 + (policyYear * 0.05m)), 2);
        }

        public double GetRefundPercentage(string productId, int policyTerm)
        {
            if (policyTerm < 10) return 0.0;
            
            // Different products have different refund slabs
            if (productId == "UL")
            {
                if (policyTerm >= 20) return 1.0; // 100% refund
                if (policyTerm >= 15) return 0.75; // 75% refund
                return 0.50; // 50% refund
            }
            
            return policyTerm >= 15 ? 0.50 : 0.25;
        }

        public int GetTotalMonthsCharged(string policyId)
        {
            int term = GetApplicablePolicyTerm(policyId);
            return term * 12;
        }

        public string GetRefundStatus(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return "INVALID";
            
            if (HasPreviousRefundBeenProcessed(policyId))
                return "PROCESSED";
                
            if (IsPolicyEligibleForRefund(policyId))
                return "PENDING";
                
            return "NOT_ELIGIBLE";
        }

        public decimal CalculateInterestOnRefund(decimal baseRefundAmount, double interestRate, int daysDelayed)
        {
            if (baseRefundAmount <= 0 || daysDelayed <= 0) return 0m;
            
            // Simple interest calculation: P * R * T
            decimal timeInYears = (decimal)daysDelayed / DAYS_IN_YEAR;
            decimal interest = baseRefundAmount * (decimal)interestRate * timeInYears;
            
            return Math.Round(interest, 2);
        }

        public bool ValidateFundValueSufficiency(string policyId, decimal requiredAmount)
        {
            // Mock fund value retrieval
            decimal currentFundValue = 50000m; 
            return currentFundValue >= (requiredAmount + MINIMUM_FUND_VALUE_THRESHOLD);
        }

        public int GetApplicablePolicyTerm(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;
            
            // Mock logic: Extract term from policy ID or DB
            return policyId.EndsWith("20") ? 20 : 15;
        }

        public double GetMortalityRate(int attainedAge, string genderCode)
        {
            if (attainedAge < 0 || attainedAge > 120)
                throw new ArgumentOutOfRangeException(nameof(attainedAge));

            double baseRate = attainedAge * 0.0015;
            
            // Female mortality rate is typically lower
            if (genderCode?.ToUpper() == "F")
            {
                baseRate *= 0.85;
            }
            
            return Math.Min(baseRate, 1.0);
        }

        public decimal GetSumAtRisk(string policyId, DateTime valuationDate)
        {
            decimal sumAssured = 1000000m; // Mock value
            decimal fundValue = 450000m;   // Mock value
            
            decimal sumAtRisk = sumAssured - fundValue;
            return sumAtRisk > 0 ? sumAtRisk : 0m;
        }

        public string GenerateRefundTransactionId(string policyId, DateTime processingDate)
        {
            if (string.IsNullOrWhiteSpace(policyId))
                throw new ArgumentException("Policy ID required.");

            string datePart = processingDate.ToString("yyyyMMddHHmmss");
            string randomPart = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            
            return $"REF-{policyId}-{datePart}-{randomPart}";
        }

        public bool HasPreviousRefundBeenProcessed(string policyId)
        {
            // Mock DB check
            return policyId.StartsWith("PROCESSED_");
        }

        public decimal CalculateTaxOnRefund(decimal refundAmount, double taxRate)
        {
            if (refundAmount <= 0) return 0m;
            return Math.Round(refundAmount * (decimal)taxRate, 2);
        }

        public int GetDaysSinceMaturity(string policyId, DateTime currentDate)
        {
            // Mock maturity date 
            DateTime maturityDate = new DateTime(2023, 1, 1);
            if (currentDate <= maturityDate) return 0;
            
            return (int)(currentDate - maturityDate).TotalDays;
        }

        public double GetPersistencyBonusRatio(string policyId)
        {
            // Mock logic: 5% bonus if policy is fully paid up
            bool isFullyPaid = !policyId.Contains("LAPSED");
            return isFullyPaid ? 0.05 : 0.0;
        }

        public decimal GetTotalAccumulatedCharges(string policyId)
        {
            int months = GetTotalMonthsCharged(policyId);
            decimal total = 0m;
            
            for (int i = 1; i <= months; i++)
            {
                int year = ((i - 1) / 12) + 1;
                int month = ((i - 1) % 12) + 1;
                total += GetMonthlyMortalityCharge(policyId, year, month);
            }
            
            return total;
        }

        public bool VerifyRiderExclusions(string policyId, string riderCode)
        {
            if (string.IsNullOrWhiteSpace(riderCode)) return false;
            
            // Mock logic: Policies starting with EXC have exclusions
            return policyId.StartsWith("EXC");
        }
    }
}