// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    public class JointLifeAnnuityService : IJointLifeAnnuityService
    {
        private const decimal MEDALLION_SIGNATURE_THRESHOLD = 100000m;
        private const double DEFAULT_TAX_RATE = 0.20;
        private const int MAX_AGE = 120;

        public decimal CalculateSurvivorBenefitAmount(string policyId, decimal baseAnnuityAmount, double survivorPercentage)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (baseAnnuityAmount < 0) throw new ArgumentOutOfRangeException(nameof(baseAnnuityAmount), "Base amount cannot be negative.");
            if (survivorPercentage < 0 || survivorPercentage > 1) throw new ArgumentOutOfRangeException(nameof(survivorPercentage), "Percentage must be between 0 and 1.");

            return Math.Round(baseAnnuityAmount * (decimal)survivorPercentage, 2);
        }

        public decimal CalculateSecondaryAnnuitantPayout(string policyId, DateTime primaryDeathDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Mock logic: Base payout of 2000, adjusted if death was recent
            decimal basePayout = 2000m;
            if (primaryDeathDate > DateTime.Now) throw new ArgumentException("Death date cannot be in the future.");
            
            return basePayout;
        }

        public decimal GetTotalPaidToPrimaryAnnuitant(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Mock database retrieval
            return 150000.50m;
        }

        public decimal CalculateProRataPayment(string policyId, DateTime deathDate, decimal monthlyBenefit)
        {
            if (monthlyBenefit < 0) throw new ArgumentOutOfRangeException(nameof(monthlyBenefit));
            
            int daysInMonth = DateTime.DaysInMonth(deathDate.Year, deathDate.Month);
            decimal dailyRate = monthlyBenefit / daysInMonth;
            
            return Math.Round(dailyRate * deathDate.Day, 2);
        }

        public decimal ComputeLumpSumDeathBenefit(string policyId, decimal accountValue, decimal totalPaid)
        {
            if (accountValue < 0) throw new ArgumentOutOfRangeException(nameof(accountValue));
            if (totalPaid < 0) throw new ArgumentOutOfRangeException(nameof(totalPaid));

            decimal remainingValue = accountValue - totalPaid;
            return remainingValue > 0 ? remainingValue : 0m;
        }

        public decimal CalculateTaxablePortion(string policyId, decimal payoutAmount)
        {
            if (payoutAmount < 0) throw new ArgumentOutOfRangeException(nameof(payoutAmount));
            
            // Simplified logic assuming 80% is return of principal, 20% is taxable earnings
            return Math.Round(payoutAmount * (decimal)DEFAULT_TAX_RATE, 2);
        }

        public decimal GetGuaranteedMinimumDeathBenefit(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Mock value
            return 50000m;
        }

        public double GetSurvivorReductionRate(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Standard 50% reduction for joint and survivor
            return 0.50;
        }

        public double CalculateActuarialAdjustmentFactor(int primaryAge, int secondaryAge)
        {
            if (primaryAge <= 0 || primaryAge > MAX_AGE) throw new ArgumentOutOfRangeException(nameof(primaryAge));
            if (secondaryAge <= 0 || secondaryAge > MAX_AGE) throw new ArgumentOutOfRangeException(nameof(secondaryAge));

            int ageDifference = primaryAge - secondaryAge;
            
            // Simplified actuarial adjustment based on age difference
            double baseFactor = 1.0;
            double adjustment = ageDifference * 0.015;
            
            return Math.Max(0.5, Math.Min(1.5, baseFactor + adjustment));
        }

        public double GetCostOfLivingAdjustmentRate(string policyId, DateTime adjustmentDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Fixed 2% COLA for demonstration
            return 0.02;
        }

        public double ComputeJointLifeExpectancyFactor(int primaryAge, int secondaryAge, string mortalityTableId)
        {
            if (primaryAge <= 0 || secondaryAge <= 0) throw new ArgumentOutOfRangeException("Ages must be positive.");
            if (string.IsNullOrWhiteSpace(mortalityTableId)) throw new ArgumentException("Mortality table ID required.");

            // Simplified joint life expectancy calculation
            double primaryExpectancy = Math.Max(1, 85 - primaryAge);
            double secondaryExpectancy = Math.Max(1, 85 - secondaryAge);
            
            return primaryExpectancy + secondaryExpectancy - (primaryExpectancy * secondaryExpectancy / 100.0);
        }

        public bool IsSecondaryAnnuitantEligible(string policyId, string secondaryAnnuitantId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(secondaryAnnuitantId)) 
                return false;
                
            // Mock validation
            return secondaryAnnuitantId.StartsWith("SA-");
        }

        public bool ValidateSpousalContinuation(string policyId, DateTime primaryDeathDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            
            // Must claim within 1 year of death
            return (DateTime.Now - primaryDeathDate).TotalDays <= 365;
        }

        public bool HasGuaranteedPeriodExpired(string policyId, DateTime evaluationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Mock issue date 10 years ago, 10 year guarantee period
            DateTime issueDate = DateTime.Now.AddYears(-10);
            DateTime guaranteeExpiration = issueDate.AddYears(10);
            
            return evaluationDate > guaranteeExpiration;
        }

        public bool IsJointLifePolicyActive(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            
            return policyId.StartsWith("JL-");
        }

        public bool RequiresMedallionSignatureGuarantee(decimal payoutAmount)
        {
            return payoutAmount >= MEDALLION_SIGNATURE_THRESHOLD;
        }

        public bool CheckBeneficiaryOverrideExists(string policyId, string secondaryAnnuitantId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(secondaryAnnuitantId)) 
                return false;
                
            // Mock check
            return false;
        }

        public int GetRemainingGuaranteedPayments(string policyId, DateTime primaryDeathDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Mock: 120 total guaranteed payments, 100 made
            int totalGuaranteed = 120;
            int paymentsMade = 100;
            
            return Math.Max(0, totalGuaranteed - paymentsMade);
        }

        public int CalculateDaysBetweenPayments(DateTime lastPaymentDate, DateTime nextPaymentDate)
        {
            if (nextPaymentDate < lastPaymentDate) throw new ArgumentException("Next payment date must be after last payment date.");
            
            return (int)(nextPaymentDate - lastPaymentDate).TotalDays;
        }

        public int GetSecondaryAnnuitantAge(string secondaryAnnuitantId, DateTime evaluationDate)
        {
            if (string.IsNullOrWhiteSpace(secondaryAnnuitantId)) throw new ArgumentException("ID cannot be null or empty.", nameof(secondaryAnnuitantId));
            
            // Mock DOB
            DateTime dob = new DateTime(1960, 1, 1);
            int age = evaluationDate.Year - dob.Year;
            if (evaluationDate.DayOfYear < dob.DayOfYear) age--;
            
            return age;
        }

        public int CountProcessedSurvivorClaims(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Mock count
            return 1;
        }

        public string GetSecondaryAnnuitantId(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            return $"SA-{policyId.Substring(Math.Max(0, policyId.Length - 4))}";
        }

        public string DeterminePayoutTransitionStatus(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            return "PENDING_DOCUMENTS";
        }

        public string GenerateSurvivorClaimReference(string policyId, string secondaryAnnuitantId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(secondaryAnnuitantId)) 
                throw new ArgumentException("Parameters cannot be null or empty.");
                
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            return $"CLM-{policyId}-{secondaryAnnuitantId}-{dateStr}";
        }

        public string GetApplicableMortalityTableCode(string policyId, DateTime issueDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            if (issueDate.Year >= 2012) return "IAM-2012";
            if (issueDate.Year >= 2000) return "A2000";
            return "1983-A";
        }
    }
}