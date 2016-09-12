using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    /// <summary>
    /// Aggregates all simple or compound reversionary bonuses vested over the years.
    /// </summary>
    public interface IVestedBonusService
    {
        decimal CalculateTotalVestedBonus(string policyId, DateTime calculationDate);
        decimal GetSimpleReversionaryBonus(string policyId, int policyYear);
        decimal GetCompoundReversionaryBonus(string policyId, int policyYear);
        decimal CalculateInterimBonus(string policyId, DateTime dateOfDeathOrMaturity);
        decimal CalculateTerminalBonus(string policyId, decimal sumAssured);
        decimal CalculateLoyaltyAddition(string policyId, int completedYears);
        
        double GetBonusRateForYear(int year, string planCode);
        double GetTerminalBonusRate(string planCode, int termInYears);
        double GetLoyaltyAdditionPercentage(string planCode, decimal premiumAmount);
        double GetInterimBonusRate(string planCode, DateTime currentFinancialYear);
        
        bool IsEligibleForTerminalBonus(string policyId, int activeYears);
        bool IsEligibleForLoyaltyAddition(string policyId, decimal totalPremiumsPaid);
        bool HasSurrenderedPolicy(string policyId);
        bool IsPolicyActive(string policyId, DateTime checkDate);
        bool ValidateBonusRates(string planCode, double simpleRate, double compoundRate);
        bool CheckMinimumVestingPeriod(string policyId, int minimumYearsRequired);
        
        int GetCompletedPolicyYears(string policyId, DateTime currentDate);
        int GetRemainingTermInMonths(string policyId, DateTime currentDate);
        int GetTotalPremiumsPaidCount(string policyId);
        int GetMissedPremiumsCount(string policyId);
        int GetBonusDeclarationYear(string bonusId);
        
        string GetApplicableBonusTableCode(string planCode, DateTime issueDate);
        string GetBonusStatus(string policyId);
        string GenerateBonusStatementId(string policyId, DateTime statementDate);
        string GetFundCodeForBonus(string planCode);
        
        decimal CalculateGuaranteedAdditions(string policyId, int yearsApplicable);
        decimal GetSpecialOneTimeBonus(string policyId, string eventCode);
        decimal CalculateProRataBonus(string policyId, DateTime startDate, DateTime endDate);
        decimal GetTotalAccruedBonus(string policyId);
        decimal CalculateSurrenderValueOfBonus(string policyId, decimal accruedBonus);
        
        double GetSurrenderValueFactor(int yearOfSurrender, string planCode);
        double GetDiscountRateForEarlyWithdrawal(string planCode);
        
        bool IsBonusVested(string policyId, int policyYear);
        bool CanWithdrawBonus(string policyId, decimal requestedAmount);
        
        int GetDaysSinceLastBonusDeclaration(string planCode, DateTime currentDate);
        
        string GetCurrencyCode(string policyId);
        
        decimal AdjustBonusForPaidUpPolicy(string policyId, decimal originalBonus);
        decimal CalculateFinalMaturityBonus(string policyId, decimal baseSumAssured, decimal accruedBonuses);
    }
}