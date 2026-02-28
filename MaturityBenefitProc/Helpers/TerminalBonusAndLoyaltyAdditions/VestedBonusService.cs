// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    public class VestedBonusService : IVestedBonusService
    {
        private const decimal BaseSumAssured = 100000m;
        private const int MinimumTerminalBonusYears = 10;
        private const int MinimumLoyaltyAdditionYears = 5;

        public decimal CalculateTotalVestedBonus(string policyId, DateTime calculationDate)
        {
            if (string.IsNullOrEmpty(policyId)) throw new ArgumentNullException(nameof(policyId));
            
            int completedYears = GetCompletedPolicyYears(policyId, calculationDate);
            decimal totalBonus = 0m;
            
            for (int i = 1; i <= completedYears; i++)
            {
                totalBonus += GetSimpleReversionaryBonus(policyId, i);
                totalBonus += GetCompoundReversionaryBonus(policyId, i);
            }
            
            return totalBonus;
        }

        public decimal GetSimpleReversionaryBonus(string policyId, int policyYear)
        {
            if (policyYear <= 0) return 0m;
            double rate = GetBonusRateForYear(policyYear, "DEFAULT");
            return BaseSumAssured * (decimal)rate / 1000m;
        }

        public decimal GetCompoundReversionaryBonus(string policyId, int policyYear)
        {
            if (policyYear <= 0) return 0m;
            decimal previousBonus = GetSimpleReversionaryBonus(policyId, policyYear - 1);
            double rate = GetBonusRateForYear(policyYear, "DEFAULT") * 1.1; // 10% higher for compound base
            return (BaseSumAssured + previousBonus) * (decimal)rate / 1000m;
        }

        public decimal CalculateInterimBonus(string policyId, DateTime dateOfDeathOrMaturity)
        {
            double rate = GetInterimBonusRate("DEFAULT", dateOfDeathOrMaturity);
            int days = dateOfDeathOrMaturity.DayOfYear;
            return BaseSumAssured * (decimal)rate / 1000m * (days / 365m);
        }

        public decimal CalculateTerminalBonus(string policyId, decimal sumAssured)
        {
            if (sumAssured <= 0) return 0m;
            int years = GetCompletedPolicyYears(policyId, DateTime.Now);
            if (!IsEligibleForTerminalBonus(policyId, years)) return 0m;
            
            double rate = GetTerminalBonusRate("DEFAULT", years);
            return sumAssured * (decimal)rate / 1000m;
        }

        public decimal CalculateLoyaltyAddition(string policyId, int completedYears)
        {
            if (completedYears < MinimumLoyaltyAdditionYears) return 0m;
            double percentage = GetLoyaltyAdditionPercentage("DEFAULT", 50000m);
            return BaseSumAssured * (decimal)(percentage / 100);
        }

        public double GetBonusRateForYear(int year, string planCode)
        {
            if (year < 1) return 0.0;
            return planCode == "ULIP" ? 35.0 : 45.0;
        }

        public double GetTerminalBonusRate(string planCode, int termInYears)
        {
            if (termInYears < MinimumTerminalBonusYears) return 0.0;
            return termInYears * 2.5;
        }

        public double GetLoyaltyAdditionPercentage(string planCode, decimal premiumAmount)
        {
            if (premiumAmount < 10000m) return 0.0;
            return premiumAmount > 100000m ? 5.0 : 2.5;
        }

        public double GetInterimBonusRate(string planCode, DateTime currentFinancialYear)
        {
            return 40.0;
        }

        public bool IsEligibleForTerminalBonus(string policyId, int activeYears)
        {
            return activeYears >= MinimumTerminalBonusYears && !HasSurrenderedPolicy(policyId);
        }

        public bool IsEligibleForLoyaltyAddition(string policyId, decimal totalPremiumsPaid)
        {
            return totalPremiumsPaid >= 50000m && !HasSurrenderedPolicy(policyId);
        }

        public bool HasSurrenderedPolicy(string policyId)
        {
            return policyId.StartsWith("SURR_");
        }

        public bool IsPolicyActive(string policyId, DateTime checkDate)
        {
            return !HasSurrenderedPolicy(policyId) && checkDate <= DateTime.Now;
        }

        public bool ValidateBonusRates(string planCode, double simpleRate, double compoundRate)
        {
            return simpleRate >= 0 && compoundRate >= 0 && compoundRate <= simpleRate * 1.5;
        }

        public bool CheckMinimumVestingPeriod(string policyId, int minimumYearsRequired)
        {
            return GetCompletedPolicyYears(policyId, DateTime.Now) >= minimumYearsRequired;
        }

        public int GetCompletedPolicyYears(string policyId, DateTime currentDate)
        {
            return 12; // Mocked for implementation
        }

        public int GetRemainingTermInMonths(string policyId, DateTime currentDate)
        {
            return 36; // Mocked for implementation
        }

        public int GetTotalPremiumsPaidCount(string policyId)
        {
            return 120; // Mocked
        }

        public int GetMissedPremiumsCount(string policyId)
        {
            return 0; // Mocked
        }

        public int GetBonusDeclarationYear(string bonusId)
        {
            if (string.IsNullOrEmpty(bonusId) || bonusId.Length < 4) return DateTime.Now.Year;
            int.TryParse(bonusId.Substring(0, 4), out int year);
            return year > 0 ? year : DateTime.Now.Year;
        }

        public string GetApplicableBonusTableCode(string planCode, DateTime issueDate)
        {
            return $"{planCode}_TBL_{issueDate.Year}";
        }

        public string GetBonusStatus(string policyId)
        {
            return IsPolicyActive(policyId, DateTime.Now) ? "ACCRUING" : "FROZEN";
        }

        public string GenerateBonusStatementId(string policyId, DateTime statementDate)
        {
            return $"STMT_{policyId}_{statementDate:yyyyMMdd}";
        }

        public string GetFundCodeForBonus(string planCode)
        {
            return planCode == "ULIP" ? "FND_EQ" : "FND_DEBT";
        }

        public decimal CalculateGuaranteedAdditions(string policyId, int yearsApplicable)
        {
            if (yearsApplicable <= 0) return 0m;
            return BaseSumAssured * 0.05m * yearsApplicable;
        }

        public decimal GetSpecialOneTimeBonus(string policyId, string eventCode)
        {
            return eventCode == "CENTENARY" ? 5000m : 0m;
        }

        public decimal CalculateProRataBonus(string policyId, DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate) return 0m;
            var days = (endDate - startDate).TotalDays;
            return GetSimpleReversionaryBonus(policyId, 1) * (decimal)(days / 365.0);
        }

        public decimal GetTotalAccruedBonus(string policyId)
        {
            return CalculateTotalVestedBonus(policyId, DateTime.Now);
        }

        public decimal CalculateSurrenderValueOfBonus(string policyId, decimal accruedBonus)
        {
            int years = GetCompletedPolicyYears(policyId, DateTime.Now);
            double factor = GetSurrenderValueFactor(years, "DEFAULT");
            return accruedBonus * (decimal)factor;
        }

        public double GetSurrenderValueFactor(int yearOfSurrender, string planCode)
        {
            if (yearOfSurrender < 3) return 0.0;
            if (yearOfSurrender < 5) return 0.30;
            if (yearOfSurrender < 10) return 0.50;
            return 0.80;
        }

        public double GetDiscountRateForEarlyWithdrawal(string planCode)
        {
            return 8.5;
        }

        public bool IsBonusVested(string policyId, int policyYear)
        {
            return GetCompletedPolicyYears(policyId, DateTime.Now) >= policyYear;
        }

        public bool CanWithdrawBonus(string policyId, decimal requestedAmount)
        {
            decimal available = CalculateSurrenderValueOfBonus(policyId, GetTotalAccruedBonus(policyId));
            return requestedAmount > 0 && requestedAmount <= available;
        }

        public int GetDaysSinceLastBonusDeclaration(string planCode, DateTime currentDate)
        {
            DateTime lastDeclaration = new DateTime(currentDate.Year, 3, 31);
            if (currentDate < lastDeclaration) lastDeclaration = lastDeclaration.AddYears(-1);
            return (int)(currentDate - lastDeclaration).TotalDays;
        }

        public string GetCurrencyCode(string policyId)
        {
            return "INR";
        }

        public decimal AdjustBonusForPaidUpPolicy(string policyId, decimal originalBonus)
        {
            int paid = GetTotalPremiumsPaidCount(policyId);
            int missed = GetMissedPremiumsCount(policyId);
            if (missed == 0) return originalBonus;
            
            decimal ratio = (decimal)paid / (paid + missed);
            return originalBonus * ratio;
        }

        public decimal CalculateFinalMaturityBonus(string policyId, decimal baseSumAssured, decimal accruedBonuses)
        {
            decimal terminal = CalculateTerminalBonus(policyId, baseSumAssured);
            decimal loyalty = CalculateLoyaltyAddition(policyId, GetCompletedPolicyYears(policyId, DateTime.Now));
            return accruedBonuses + terminal + loyalty;
        }
    }
}