using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    // Fixed implementation — correct business logic
    public class DeferredAnnuityService : IDeferredAnnuityService
    {
        private const decimal BaseAccumulationMultiplier = 1.05m;
        private const decimal TerminalBonusRate = 0.075m;
        private const int MinimumSurrenderYears = 3;

        // Mock data helper
        private DateTime GetMockVestingDate(string policyId)
        {
            int hash = Math.Abs(policyId?.GetHashCode() ?? 0);
            return new DateTime(2025 + (hash % 10), (hash % 12) + 1, (hash % 28) + 1);
        }

        private DateTime GetMockIssueDate(string policyId)
        {
            int hash = Math.Abs(policyId?.GetHashCode() ?? 0);
            return new DateTime(2010 + (hash % 10), (hash % 12) + 1, (hash % 28) + 1);
        }

        public decimal CalculateAccumulatedValue(string policyId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.");
            
            DateTime issueDate = GetMockIssueDate(policyId);
            if (calculationDate < issueDate) return 0m;

            int yearsActive = calculationDate.Year - issueDate.Year;
            decimal basePremium = 10000m; // Mock base premium
            
            decimal accumulated = 0m;
            for (int i = 0; i < yearsActive; i++)
            {
                accumulated += basePremium;
                accumulated *= BaseAccumulationMultiplier;
            }
            
            return Math.Round(accumulated, 2);
        }

        public string GetVestingStatus(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return "Invalid";
            
            DateTime vestingDate = GetMockVestingDate(policyId);
            if (DateTime.UtcNow >= vestingDate)
            {
                return "Vested";
            }
            return "Accumulating";
        }

        public bool IsEligibleForSurrender(string policyId, DateTime requestDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            
            DateTime issueDate = GetMockIssueDate(policyId);
            int activeYears = requestDate.Year - issueDate.Year;
            if (requestDate < issueDate.AddYears(activeYears)) activeYears--;

            return activeYears >= MinimumSurrenderYears;
        }

        public decimal CalculateSurrenderValue(string policyId, decimal currentAccumulation, double surrenderChargeRate)
        {
            if (currentAccumulation <= 0) return 0m;
            if (surrenderChargeRate < 0 || surrenderChargeRate > 1) throw new ArgumentOutOfRangeException(nameof(surrenderChargeRate));

            decimal chargeAmount = currentAccumulation * (decimal)surrenderChargeRate;
            return Math.Max(0m, currentAccumulation - chargeAmount);
        }

        public double GetGuaranteedAdditionRate(string planCode, int policyYear)
        {
            if (string.IsNullOrWhiteSpace(planCode)) return 0.0;

            if (planCode.StartsWith("DEF-A"))
            {
                return policyYear <= 5 ? 0.05 : 0.03;
            }
            else if (planCode.StartsWith("DEF-B"))
            {
                return policyYear <= 10 ? 0.04 : 0.02;
            }
            
            return 0.01;
        }

        public int GetRemainingAccumulationMonths(string policyId, DateTime currentDate)
        {
            DateTime vestingDate = GetMockVestingDate(policyId);
            if (currentDate >= vestingDate) return 0;

            int months = ((vestingDate.Year - currentDate.Year) * 12) + vestingDate.Month - currentDate.Month;
            return months > 0 ? months : 0;
        }

        public string GenerateVestingQuotationId(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID required.");
            return $"VQ-{policyId.ToUpper()}-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        public bool ValidateDefermentPeriod(string planCode, int defermentYears)
        {
            if (string.IsNullOrWhiteSpace(planCode)) return false;

            // Business rule: Deferment period must be between 5 and 30 years
            return defermentYears >= 5 && defermentYears <= 30;
        }

        public decimal CalculateDeathBenefit(string policyId, DateTime dateOfDeath)
        {
            decimal accumulatedValue = CalculateAccumulatedValue(policyId, dateOfDeath);
            decimal guaranteedDeathBenefit = 50000m; // Mock guaranteed minimum
            
            return Math.Max(accumulatedValue, guaranteedDeathBenefit);
        }

        public double CalculateBonusRatio(string policyId, int accumulationYears)
        {
            if (accumulationYears < 5) return 0.0;
            if (accumulationYears < 10) return 0.02;
            if (accumulationYears < 15) return 0.035;
            return 0.05;
        }

        public int GetPaidPremiumsCount(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;
            
            DateTime issueDate = GetMockIssueDate(policyId);
            int years = DateTime.UtcNow.Year - issueDate.Year;
            return Math.Max(0, years * 12); // Assuming monthly
        }

        public string GetAnnuityOptionCode(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return "OPT-UNKNOWN";
            
            int hash = Math.Abs(policyId.GetHashCode());
            return (hash % 2 == 0) ? "LIFE-WITH-RETURN" : "LIFE-WITHOUT-RETURN";
        }

        public decimal CalculateProjectedMaturityValue(string policyId, double assumedInterestRate)
        {
            if (assumedInterestRate < 0) throw new ArgumentOutOfRangeException(nameof(assumedInterestRate));
            
            decimal currentVal = CalculateAccumulatedValue(policyId, DateTime.UtcNow);
            int remainingMonths = GetRemainingAccumulationMonths(policyId, DateTime.UtcNow);
            
            double remainingYears = remainingMonths / 12.0;
            decimal projectionMultiplier = (decimal)Math.Pow(1 + assumedInterestRate, remainingYears);
            
            return Math.Round(currentVal * projectionMultiplier, 2);
        }

        public bool CheckVestingConditionMet(string policyId, DateTime evaluationDate)
        {
            DateTime vestingDate = GetMockVestingDate(policyId);
            return evaluationDate >= vestingDate;
        }

        public decimal ApplyTerminalBonus(string policyId, decimal baseAmount)
        {
            if (baseAmount <= 0) return 0m;
            
            DateTime issueDate = GetMockIssueDate(policyId);
            int activeYears = DateTime.UtcNow.Year - issueDate.Year;
            
            // Terminal bonus only applies if active for more than 10 years
            if (activeYears >= 10)
            {
                return Math.Round(baseAmount + (baseAmount * TerminalBonusRate), 2);
            }
            
            return baseAmount;
        }

        public double GetLoyaltyAdditionPercentage(int completedYears)
        {
            if (completedYears >= 20) return 0.10;
            if (completedYears >= 15) return 0.07;
            if (completedYears >= 10) return 0.05;
            return 0.0;
        }

        public int CalculateDaysToVesting(string policyId, DateTime currentDate)
        {
            DateTime vestingDate = GetMockVestingDate(policyId);
            if (currentDate >= vestingDate) return 0;
            
            return (vestingDate - currentDate).Days;
        }

        public string UpdateAccumulationPhaseStatus(string policyId, string newStatusCode)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID required.");
            if (string.IsNullOrWhiteSpace(newStatusCode)) throw new ArgumentException("Status code required.");
            
            // In a real system, this would update a database.
            // Here we just return a confirmation string.
            return $"Policy {policyId} successfully updated to status: {newStatusCode.ToUpper()}";
        }
    }
}