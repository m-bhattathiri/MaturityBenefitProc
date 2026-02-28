// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    public class ReversionaryBonusService : IReversionaryBonusService
    {
        private const int MIN_YEARS_FOR_TERMINAL_BONUS = 10;
        private const int MIN_YEARS_FOR_VESTING = 3;
        private const decimal MAX_SURRENDER_FACTOR = 0.95m;

        public decimal CalculateAnnualBonus(string policyId, decimal sumAssured, double bonusRate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.");
            if (sumAssured <= 0) return 0m;
            if (bonusRate < 0) throw new ArgumentException("Bonus rate cannot be negative.");

            // Bonus is typically declared per 1000 Sum Assured
            return sumAssured * (decimal)(bonusRate / 1000.0);
        }

        public decimal CalculateAccruedReversionaryBonus(string policyId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.");
            
            // Mock logic: Assume 5000 per year for 5 years
            int years = Math.Max(0, calculationDate.Year - 2015);
            return 5000m * years;
        }

        public decimal GetTotalDeclaredBonus(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.");
            return 45000m; // Mocked total declared bonus
        }

        public decimal CalculateInterimBonus(string policyId, DateTime exitDate, decimal sumAssured)
        {
            if (sumAssured <= 0) return 0m;
            
            double interimRate = GetInterimBonusRate("DEFAULT", exitDate);
            int daysSinceDeclaration = CalculateDaysSinceLastDeclaration(policyId, exitDate);
            
            decimal annualInterim = CalculateAnnualBonus(policyId, sumAssured, interimRate);
            return annualInterim * (daysSinceDeclaration / 365.25m);
        }

        public decimal ComputeVestedBonusAmount(string policyId, int activeYears)
        {
            if (activeYears < MIN_YEARS_FOR_VESTING) return 0m;
            return GetTotalDeclaredBonus(policyId);
        }

        public decimal CalculateTerminalBonus(string policyId, decimal accruedBonus, double terminalBonusRate)
        {
            if (accruedBonus <= 0 || terminalBonusRate <= 0) return 0m;
            return accruedBonus * (decimal)(terminalBonusRate / 100.0);
        }

        public decimal GetSurrenderValueOfBonus(string policyId, decimal totalBonus, double surrenderFactor)
        {
            if (totalBonus <= 0) return 0m;
            decimal factor = Math.Min((decimal)surrenderFactor, MAX_SURRENDER_FACTOR);
            return totalBonus * factor;
        }

        public decimal CalculateLoyaltyAdditionAmount(string policyId, decimal baseAmount, double loyaltyRate)
        {
            if (baseAmount <= 0 || loyaltyRate <= 0) return 0m;
            return baseAmount * (decimal)(loyaltyRate / 100.0);
        }

        public double GetCurrentBonusRate(string planCode, int policyTerm)
        {
            if (string.IsNullOrWhiteSpace(planCode)) return 0.0;
            return policyTerm > 15 ? 45.0 : 35.0; // Rate per 1000 SA
        }

        public double GetInterimBonusRate(string planCode, DateTime exitDate)
        {
            return 30.0; // Interim rate is usually slightly lower
        }

        public double CalculateBonusCompoundingFactor(int yearsInForce, double annualRate)
        {
            if (yearsInForce <= 0 || annualRate <= 0) return 1.0;
            return Math.Pow(1.0 + (annualRate / 100.0), yearsInForce);
        }

        public double GetTerminalBonusRate(string planCode, int durationInYears)
        {
            if (durationInYears < MIN_YEARS_FOR_TERMINAL_BONUS) return 0.0;
            return durationInYears >= 20 ? 15.0 : 10.0; // Percentage of accrued bonus
        }

        public double FetchLoyaltyAdditionPercentage(string policyId, int premiumPayingTerm)
        {
            return premiumPayingTerm >= 10 ? 5.0 : 0.0;
        }

        public bool IsPolicyEligibleForBonus(string policyId, DateTime evaluationDate)
        {
            return IsParticipatingPolicy(policyId) && evaluationDate > new DateTime(2000, 1, 1);
        }

        public bool HasGuaranteedAdditions(string planCode)
        {
            return planCode?.StartsWith("GA") ?? false;
        }

        public bool IsParticipatingPolicy(string policyId)
        {
            return policyId?.StartsWith("PAR") ?? false;
        }

        public bool ValidateBonusRateApplicability(string planCode, double rate, DateTime effectiveDate)
        {
            return rate > 0 && rate <= 100 && effectiveDate <= DateTime.Now;
        }

        public bool CheckLoyaltyAdditionEligibility(string policyId, int paidPremiumsCount)
        {
            return paidPremiumsCount >= 5;
        }

        public bool IsBonusVested(string policyId, DateTime checkDate)
        {
            return GetYearsEligibleForBonus(policyId) >= MIN_YEARS_FOR_VESTING;
        }

        public int GetYearsEligibleForBonus(string policyId)
        {
            return 5; // Mocked active years
        }

        public int CalculateDaysSinceLastDeclaration(string policyId, DateTime currentDate)
        {
            DateTime lastDeclaration = new DateTime(currentDate.Year, 3, 31);
            if (currentDate < lastDeclaration) lastDeclaration = lastDeclaration.AddYears(-1);
            return (currentDate - lastDeclaration).Days;
        }

        public int GetPendingBonusDeclarationsCount(string policyId)
        {
            return 0; // Assuming up to date
        }

        public int GetMinimumTermForTerminalBonus(string planCode)
        {
            return MIN_YEARS_FOR_TERMINAL_BONUS;
        }

        public string GetBonusRateTableId(string planCode, DateTime issueDate)
        {
            return $"BRT_{planCode}_{issueDate.Year}";
        }

        public string DetermineBonusStatus(string policyId)
        {
            return IsBonusVested(policyId, DateTime.Now) ? "Vested" : "Accruing";
        }

        public string GetLoyaltyAdditionScaleCode(string planCode, int policyTerm)
        {
            return $"LA_{planCode}_{policyTerm}Y";
        }

        public string GetLastDeclarationFinancialYear(string policyId)
        {
            int year = DateTime.Now.Month >= 4 ? DateTime.Now.Year : DateTime.Now.Year - 1;
            return $"{year}-{year + 1}";
        }
    }
}