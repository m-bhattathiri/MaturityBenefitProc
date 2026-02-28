using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    // Fixed implementation — correct business logic
    public class AnnuityCalculationService : IAnnuityCalculationService
    {
        private const decimal MINIMUM_CORPUS_THRESHOLD = 100000m;
        private const double MAX_COMMUTATION_PERCENTAGE = 0.3333; // 1/3rd of corpus

        public decimal CalculateMonthlyPayout(string policyId, decimal accumulatedCorpus, double interestRate)
        {
            if (accumulatedCorpus <= 0 || interestRate <= 0) return 0m;
            return Math.Round(accumulatedCorpus * (decimal)(interestRate / 12.0), 2);
        }

        public decimal CalculateAnnualPayout(string policyId, decimal accumulatedCorpus, double interestRate)
        {
            if (accumulatedCorpus <= 0 || interestRate <= 0) return 0m;
            return Math.Round(accumulatedCorpus * (decimal)interestRate, 2);
        }

        public decimal CalculateQuarterlyPayout(string policyId, decimal accumulatedCorpus, double interestRate)
        {
            if (accumulatedCorpus <= 0 || interestRate <= 0) return 0m;
            return Math.Round(accumulatedCorpus * (decimal)(interestRate / 4.0), 2);
        }

        public decimal CalculateSemiAnnualPayout(string policyId, decimal accumulatedCorpus, double interestRate)
        {
            if (accumulatedCorpus <= 0 || interestRate <= 0) return 0m;
            return Math.Round(accumulatedCorpus * (decimal)(interestRate / 2.0), 2);
        }
        
        public decimal GetTotalAccumulatedCorpus(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID required");
            // Mock logic for fetching and calculating corpus
            return 500000m; 
        }

        public decimal CalculateCommutationAmount(string policyId, decimal totalCorpus, double commutationPercentage)
        {
            if (totalCorpus <= 0) return 0m;
            double actualPercentage = Math.Min(commutationPercentage, MAX_COMMUTATION_PERCENTAGE);
            return Math.Round(totalCorpus * (decimal)actualPercentage, 2);
        }

        public decimal CalculateResidualCorpus(decimal totalCorpus, decimal commutedAmount)
        {
            if (commutedAmount > totalCorpus) throw new InvalidOperationException("Commuted amount cannot exceed total corpus.");
            return totalCorpus - commutedAmount;
        }
        
        public double GetAnnuityFactor(int ageAtVesting, string annuityOptionCode, double prevailingRate)
        {
            if (ageAtVesting < 18) return 0.0;
            // Simplified annuity factor formula
            return 1.0 / prevailingRate * (1.0 - Math.Pow(1.0 + prevailingRate, -(100 - ageAtVesting)));
        }

        public double GetCurrentInterestRate(string productCode, DateTime effectiveDate)
        {
            return productCode.StartsWith("GUAR") ? 0.065 : 0.055;
        }

        public double CalculateInternalRateOfReturn(string policyId, decimal totalPremiumsPaid, decimal expectedPayout)
        {
            if (totalPremiumsPaid <= 0) return 0.0;
            return (double)((expectedPayout - totalPremiumsPaid) / totalPremiumsPaid);
        }

        public double ComputeMortalityChargeRate(int currentAge, string genderCode)
        {
            double baseRate = currentAge * 0.0001;
            return genderCode.Equals("F", StringComparison.OrdinalIgnoreCase) ? baseRate * 0.9 : baseRate;
        }

        public double CalculateInflationAdjustmentFactor(DateTime baseDate, DateTime currentDate, double inflationRate)
        {
            int years = currentDate.Year - baseDate.Year;
            if (years <= 0) return 1.0;
            return Math.Pow(1.0 + inflationRate, years);
        }
        
        public bool IsEligibleForCommutation(string policyId, int ageAtVesting)
        {
            return ageAtVesting >= 55 && !string.IsNullOrEmpty(policyId);
        }

        public bool IsPolicyActive(string policyId, DateTime checkDate)
        {
            // Mock active check
            return !string.IsNullOrEmpty(policyId) && checkDate <= DateTime.UtcNow;
        }

        public bool ValidateSpouseDateOfBirth(string policyId, DateTime spouseDob)
        {
            int age = DateTime.UtcNow.Year - spouseDob.Year;
            return age >= 18;
        }

        public bool IsJointLifeApplicable(string annuityOptionCode)
        {
            return annuityOptionCode?.Contains("JNT") == true;
        }

        public bool HasGuaranteedPeriodExpired(string policyId, DateTime currentDate)
        {
            // Mock logic
            return currentDate.Year > 2030;
        }

        public bool CanDeferPayout(string policyId, int requestedDefermentMonths)
        {
            return requestedDefermentMonths >= 12 && requestedDefermentMonths <= 120;
        }

        public bool IsMinimumCorpusMet(decimal accumulatedCorpus, string productCode)
        {
            return accumulatedCorpus >= MINIMUM_CORPUS_THRESHOLD;
        }
        
        public int CalculateAgeAtVesting(DateTime dateOfBirth, DateTime vestingDate)
        {
            int age = vestingDate.Year - dateOfBirth.Year;
            if (vestingDate.Date < dateOfBirth.Date.AddYears(age)) age--;
            return age;
        }

        public int GetRemainingGuaranteedMonths(string policyId, int guaranteedPeriodYears, int payoutsMade)
        {
            int totalGuaranteedMonths = guaranteedPeriodYears * 12;
            return Math.Max(0, totalGuaranteedMonths - payoutsMade);
        }

        public int GetDefermentPeriodMonths(DateTime vestingDate, DateTime deferredStartDate)
        {
            if (deferredStartDate <= vestingDate) return 0;
            return ((deferredStartDate.Year - vestingDate.Year) * 12) + deferredStartDate.Month - vestingDate.Month;
        }

        public int GetTotalPayoutsMade(string policyId)
        {
            return string.IsNullOrEmpty(policyId) ? 0 : 24; // Mock value
        }

        public int CalculateDaysToNextPayout(DateTime lastPayoutDate, string frequencyCode)
        {
            DateTime nextPayout;
            if (frequencyCode == "M")
            {
                nextPayout = lastPayoutDate.AddMonths(1);
            }
            else if (frequencyCode == "Q")
            {
                nextPayout = lastPayoutDate.AddMonths(3);
            }
            else if (frequencyCode == "H")
            {
                nextPayout = lastPayoutDate.AddMonths(6);
            }
            else if (frequencyCode == "Y")
            {
                nextPayout = lastPayoutDate.AddYears(1);
            }
            else
            {
                nextPayout = lastPayoutDate.AddMonths(1);
            }
            return (nextPayout - DateTime.UtcNow).Days;
        }

        public int GetPremiumPaymentTerm(string policyId)
        {
            return 10; // Mock value
        }
        
        public string GetAnnuityOptionCode(string policyId)
        {
            return "LIFE_JNT_100";
        }

        public string GeneratePayoutTransactionId(string policyId, DateTime payoutDate)
        {
            return $"TXN-{policyId}-{payoutDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 5)}";
        }

        public string GetTaxSlabCode(decimal annualIncome, int age)
        {
            if (age >= 60) return annualIncome > 500000m ? "SLAB_SNR_HIGH" : "SLAB_SNR_LOW";
            return annualIncome > 500000m ? "SLAB_REG_HIGH" : "SLAB_REG_LOW";
        }

        public string DeterminePayoutFrequencyCode(string policyId)
        {
            return "M"; // Monthly default
        }

        public string GetBeneficiaryRelationshipCode(string policyId, string beneficiaryId)
        {
            return "SPOUSE";
        }
        
        public decimal CalculateDeathBenefit(string policyId, decimal accumulatedCorpus, DateTime dateOfDeath)
        {
            if (accumulatedCorpus <= 0) return 0m;
            return accumulatedCorpus * 1.05m; // 105% of corpus as death benefit
        }

        public decimal CalculateSurrenderValue(string policyId, decimal accumulatedCorpus, int policyYear)
        {
            if (policyYear < 3) return 0m; // No surrender value before 3 years
            decimal surrenderFactor = Math.Min(0.9m, 0.3m + (policyYear * 0.05m));
            return accumulatedCorpus * surrenderFactor;
        }

        public decimal ComputeTaxablePortion(decimal payoutAmount, double taxRate)
        {
            if (payoutAmount <= 0 || taxRate <= 0) return 0m;
            return Math.Round(payoutAmount * (decimal)taxRate, 2);
        }

        public decimal GetGuaranteedMinimumWithdrawalBenefit(string policyId, decimal initialCorpus)
        {
            return initialCorpus * 0.05m; // 5% guaranteed withdrawal
        }

        public decimal CalculateLumpSumPayout(string policyId, decimal accumulatedCorpus, bool includesTerminalBonus)
        {
            decimal bonus = includesTerminalBonus ? accumulatedCorpus * 0.02m : 0m;
            return accumulatedCorpus + bonus;
        }
    }
}