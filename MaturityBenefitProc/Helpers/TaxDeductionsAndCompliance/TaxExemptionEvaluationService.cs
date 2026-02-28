using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    // Fixed implementation — correct business logic
    public class TaxExemptionEvaluationService : ITaxExemptionEvaluationService
    {
        private const double StandardTdsRate = 5.0;
        private const double InvalidPanTdsRate = 20.0;
        private const decimal UlipPremiumThreshold = 250000m;
        private readonly DateTime _taxRuleChangeDate = new DateTime(2012, 4, 1);

        public bool IsEligibleForSection1010D(string policyId, DateTime issueDate)
        {
            if (string.IsNullOrWhiteSpace(policyId))
                throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));

            // In a real scenario, we would fetch the Sum Assured and Premium from a repository.
            // Here we simulate the eligibility check based on the issue date rule.
            bool isUlip = CheckIfPolicyIsUlip(policyId);
            if (isUlip && issueDate >= new DateTime(2021, 2, 1))
            {
                // ULIPs issued after Feb 1, 2021 have a 2.5L premium limit for exemption
                return false; // Requires deeper premium check
            }

            return true;
        }

        public decimal CalculateTaxableMaturityAmount(string policyId, decimal totalPremiumsPaid, decimal maturityAmount)
        {
            if (maturityAmount < 0 || totalPremiumsPaid < 0)
                throw new ArgumentException("Amounts cannot be negative.");

            decimal taxableAmount = maturityAmount - totalPremiumsPaid;
            return taxableAmount > 0 ? taxableAmount : 0m;
        }

        public double GetApplicableTdsRate(string panNumber, bool isPanValid)
        {
            if (string.IsNullOrWhiteSpace(panNumber) || !isPanValid)
            {
                return InvalidPanTdsRate;
            }
            return StandardTdsRate;
        }

        public decimal CalculateTdsAmount(decimal taxableAmount, double tdsRate)
        {
            if (taxableAmount <= 0) return 0m;
            if (tdsRate < 0 || tdsRate > 100) throw new ArgumentOutOfRangeException(nameof(tdsRate));

            return Math.Round(taxableAmount * (decimal)(tdsRate / 100), 2);
        }

        public int GetPolicyTermInYears(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;
            
            // Mocking policy term extraction from policy ID (e.g., POL-10Y-12345)
            if (policyId.Contains("-10Y-")) return 10;
            if (policyId.Contains("-20Y-")) return 20;
            
            return 15; // Default mock term
        }

        public bool ValidatePremiumToSumAssuredRatio(decimal annualPremium, decimal sumAssured, DateTime issueDate)
        {
            if (sumAssured <= 0) return false;

            double ratio = CalculatePremiumToSumAssuredPercentage(annualPremium, sumAssured);
            
            // Policies issued before April 1, 2012: Premium should not exceed 20% of Sum Assured
            if (issueDate < _taxRuleChangeDate)
            {
                return ratio <= 20.0;
            }
            
            // Policies issued on or after April 1, 2012: Premium should not exceed 10% of Sum Assured
            return ratio <= 10.0;
        }

        public string GetExemptionRejectionReasonCode(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return "INVALID_POLICY";
            
            if (CheckIfPolicyIsUlip(policyId))
                return "ULIP_PREMIUM_EXCEEDS_2.5L";
                
            return "PREMIUM_EXCEEDS_10_PERCENT_SA";
        }

        public decimal GetTotalPremiumsPaid(string policyId, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be after end date.");

            // Mock calculation: assume 50,000 paid per year
            int years = endDate.Year - startDate.Year;
            if (years < 0) years = 0;
            
            return 50000m * (years + 1);
        }

        public double CalculatePremiumToSumAssuredPercentage(decimal annualPremium, decimal sumAssured)
        {
            if (sumAssured == 0) return 0.0;
            return (double)((annualPremium / sumAssured) * 100m);
        }

        public int GetDaysUntilTaxFilingDeadline(DateTime currentProcessDate)
        {
            // Assuming tax filing deadline is July 31st of the assessment year
            int assessmentYear = currentProcessDate.Month > 3 ? currentProcessDate.Year + 1 : currentProcessDate.Year;
            DateTime deadline = new DateTime(assessmentYear, 7, 31);
            
            if (currentProcessDate > deadline)
            {
                deadline = new DateTime(assessmentYear + 1, 7, 31);
            }
            
            return (deadline - currentProcessDate).Days;
        }

        public bool CheckIfPolicyIsUlip(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            return policyId.StartsWith("ULIP", StringComparison.OrdinalIgnoreCase);
        }

        public decimal CalculateUlipExemptionLimit(decimal aggregatePremium, DateTime financialYearStart)
        {
            if (aggregatePremium <= UlipPremiumThreshold)
            {
                return aggregatePremium;
            }
            return UlipPremiumThreshold;
        }

        public string RetrieveCustomerPanStatus(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return "MISSING";
            
            // Mock logic
            return customerId.StartsWith("CUST-V") ? "VALID" : "INVALID";
        }

        public bool IsDeathBenefitExempt(string policyId, string causeOfDeathCode)
        {
            // Death benefits are generally exempt under 10(10D) regardless of premium ratio
            // Exceptions might exist for specific employer-employee schemes, but generally true.
            if (causeOfDeathCode == "SUICIDE" && GetPolicyTermInYears(policyId) < 1)
            {
                return false; // Example business rule
            }
            return true;
        }

        public decimal ComputeNetPayableAfterTaxes(decimal grossAmount, decimal tdsAmount, decimal surcharge)
        {
            if (grossAmount < 0) throw new ArgumentException("Gross amount cannot be negative.");
            
            decimal net = grossAmount - tdsAmount - surcharge;
            return net > 0 ? net : 0m;
        }

        public int CountPoliciesExceedingPremiumLimit(string customerId, decimal premiumLimit)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 0;
            
            // Mock repository call
            return customerId.Length % 3; // Random mock count between 0 and 2
        }

        public double GetSurchargeRate(decimal totalTaxableIncome)
        {
            if (totalTaxableIncome > 50000000m) return 37.0; // > 5 Cr
            if (totalTaxableIncome > 20000000m) return 25.0; // > 2 Cr
            if (totalTaxableIncome > 10000000m) return 15.0; // > 1 Cr
            if (totalTaxableIncome > 5000000m) return 10.0;  // > 50 Lakhs
            
            return 0.0;
        }

        public bool EvaluateNriTaxCompliance(string customerId, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return false;
            
            bool isNri = !countryCode.Equals("IN", StringComparison.OrdinalIgnoreCase);
            if (isNri)
            {
                // Additional compliance checks for NRIs (e.g., DTAA, FATCA)
                return customerId.Contains("-NRI-");
            }
            
            return true; // Resident Indians are compliant by default in this context
        }
    }
}