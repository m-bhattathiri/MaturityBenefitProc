using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    // Fixed implementation — correct business logic
    public class TerminalBonusService : ITerminalBonusService
    {
        private readonly Dictionary<string, decimal> _mockAccruedBonuses = new Dictionary<string, decimal>
        {
            { "POL1001", 15000m },
            { "POL1002", 22500m }
        };

        public decimal CalculateBaseTerminalBonus(string policyId, decimal sumAssured, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (sumAssured <= 0) return 0m;

            // Extract a mock plan code from policy ID for demonstration
            string planCode = policyId.Length >= 3 ? policyId.Substring(0, 3) : "DEF";
            int completedYears = GetCompletedPolicyYears(policyId, maturityDate);
            
            double rate = GetTerminalBonusRate(planCode, completedYears);
            return sumAssured * (decimal)rate;
        }
        
        public decimal CalculateLoyaltyAdditionAmount(string policyId, int premiumPayingYears)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            string planCode = policyId.Length >= 3 ? policyId.Substring(0, 3) : "DEF";
            if (!IsLoyaltyAdditionApplicable(planCode, premiumPayingYears))
            {
                return 0m;
            }

            double rate = GetLoyaltyAdditionRate(planCode, premiumPayingYears);
            // Assuming a base factor of 1000 per premium paying year for the calculation
            decimal baseFactor = 1000m * premiumPayingYears;
            return baseFactor * (decimal)rate;
        }
        
        public decimal GetAccruedReversionaryBonus(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID required.", nameof(policyId));
            
            return _mockAccruedBonuses.TryGetValue(policyId, out decimal bonus) ? bonus : 5000m; // Default fallback
        }
        
        public decimal ComputeFinalAdditionalBonus(string policyId, decimal totalPremiumsPaid)
        {
            if (totalPremiumsPaid <= 0) return 0m;
            
            // Final additional bonus is typically a percentage of total premiums paid if policy term is long enough
            return totalPremiumsPaid * 0.025m; // 2.5% of total premiums
        }
        
        public decimal CalculateVestedBonusTotal(string policyId, DateTime calculationDate)
        {
            decimal accrued = GetAccruedReversionaryBonus(policyId);
            // Add a time-value factor based on calculation date
            decimal timeFactor = calculationDate.Year > 2020 ? 1.05m : 1.0m;
            return accrued * timeFactor;
        }
        
        public decimal GetSpecialSurrenderValueBonus(string policyId, decimal baseSurrenderValue)
        {
            if (baseSurrenderValue <= 0) return 0m;
            // Special surrender value bonus is a fraction of the base surrender value
            return baseSurrenderValue * 0.15m;
        }
        
        public decimal CalculateProratedTerminalBonus(string policyId, DateTime exitDate, int daysActive)
        {
            if (daysActive <= 0) return 0m;
            
            decimal baseBonus = CalculateBaseTerminalBonus(policyId, 100000m, exitDate); // Assuming 100k base for proration
            decimal prorationFactor = Math.Min((decimal)daysActive / 365m, 1.0m);
            
            return baseBonus * prorationFactor;
        }
        
        public decimal ApplyBonusMultiplier(decimal baseBonus, double multiplierRate)
        {
            if (baseBonus < 0) throw new ArgumentException("Base bonus cannot be negative.", nameof(baseBonus));
            if (multiplierRate < 0) throw new ArgumentException("Multiplier rate cannot be negative.", nameof(multiplierRate));
            
            return baseBonus * (decimal)multiplierRate;
        }

        public double GetTerminalBonusRate(string planCode, int policyTerm)
        {
            if (policyTerm < GetMinimumYearsForTerminalBonus(planCode)) return 0.0;

            if (planCode.ToUpper() == "END")
            {
                return 0.045;
            }
            else if (planCode.ToUpper() == "WHL")
            {
                return 0.060;
            }
            else if (planCode.ToUpper() == "ULP")
            {
                return 0.020;
            }
            else
            {
                return 0.030;
            }
        }
        
        public double GetLoyaltyAdditionRate(string planCode, int completedYears)
        {
            if (completedYears < 5) return 0.0;
            if (completedYears >= 15) return 0.08;
            if (completedYears >= 10) return 0.05;
            return 0.02;
        }
        
        public double CalculateBonusYield(decimal totalBonus, decimal totalPremiums)
        {
            if (totalPremiums <= 0) return 0.0;
            return (double)(totalBonus / totalPremiums);
        }
        
        public double GetFundPerformanceFactor(string fundId, DateTime evaluationDate)
        {
            // Mock logic: Funds perform better in recent years
            return evaluationDate.Year >= 2022 ? 1.12 : 1.05;
        }
        
        public double GetParticipatingFundRatio(string cohortId)
        {
            return string.IsNullOrWhiteSpace(cohortId) ? 0.0 : 0.90; // 90% goes to policyholders
        }

        public bool IsEligibleForTerminalBonus(string policyId, string status)
        {
            return status.Equals("Active", StringComparison.OrdinalIgnoreCase) || 
                   status.Equals("Matured", StringComparison.OrdinalIgnoreCase);
        }
        
        public bool IsLoyaltyAdditionApplicable(string planCode, int elapsedYears)
        {
            return elapsedYears >= 5 && !planCode.StartsWith("TRM", StringComparison.OrdinalIgnoreCase); // Not applicable for Term plans
        }
        
        public bool ValidateBonusDeclaration(string declarationId, DateTime effectiveDate)
        {
            return !string.IsNullOrWhiteSpace(declarationId) && effectiveDate <= DateTime.UtcNow;
        }
        
        public bool HasClaimedPreviousBonuses(string policyId)
        {
            // Mock implementation
            return policyId.EndsWith("-C");
        }
        
        public bool IsPolicyInParticipatingFund(string policyId)
        {
            return policyId.StartsWith("PAR", StringComparison.OrdinalIgnoreCase);
        }

        public int GetCompletedPolicyYears(string policyId, DateTime maturityDate)
        {
            // Mocking issue date as 15 years prior to maturity for demonstration
            DateTime issueDate = maturityDate.AddYears(-15);
            int years = maturityDate.Year - issueDate.Year;
            if (maturityDate.Date < issueDate.Date.AddYears(years)) years--;
            return Math.Max(0, years);
        }
        
        public int GetMinimumYearsForTerminalBonus(string planCode)
        {
            return planCode.StartsWith("WHL") ? 15 : 10;
        }
        
        public int CalculateDaysSinceLastBonusDeclaration(DateTime lastDeclarationDate)
        {
            TimeSpan difference = DateTime.UtcNow - lastDeclarationDate;
            return Math.Max(0, (int)difference.TotalDays);
        }
        
        public int GetTotalBonusUnitsAllocated(string policyId)
        {
            return string.IsNullOrWhiteSpace(policyId) ? 0 : 250;
        }

        public string GetBonusDeclarationId(string planCode, DateTime declarationYear)
        {
            return $"BDEC-{planCode.ToUpper()}-{declarationYear.Year}";
        }
        
        public string DetermineBonusCohort(string policyId, DateTime issueDate)
        {
            return $"COHORT-{issueDate.Year}-{(issueDate.Month <= 6 ? "H1" : "H2")}";
        }
        
        public string GetTerminalBonusStatus(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return "Unknown";
            return IsPolicyInParticipatingFund(policyId) ? "Accruing" : "Not Applicable";
        }
    }
}