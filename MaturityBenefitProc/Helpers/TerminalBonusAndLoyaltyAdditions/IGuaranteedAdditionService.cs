using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    /// <summary>Calculates contractually guaranteed additions accrued over the policy term.</summary>
    public interface IGuaranteedAdditionService
    {
        decimal CalculateTotalGuaranteedAdditions(string policyId, DateTime calculationDate);
        decimal CalculateAccruedAdditionsForYear(string policyId, int policyYear);
        bool IsPolicyEligibleForGuaranteedAdditions(string policyId, string productCode);
        double GetApplicableAdditionRate(string productCode, int policyTerm);
        int GetAccrualPeriodInDays(DateTime startDate, DateTime endDate);
        string GetAdditionCalculationBasisCode(string productCode);
        decimal CalculateProRataAdditions(string policyId, decimal baseAmount, int daysActive);
        bool ValidateAdditionRateLimits(double rate, string productCode);
        decimal GetSumAssuredForAdditions(string policyId, DateTime effectiveDate);
        int GetCompletedPolicyYears(string policyId, DateTime maturityDate);
        double CalculateVestingPercentage(int completedYears, int totalTerm);
        decimal CalculateVestedGuaranteedAdditions(string policyId, decimal totalAdditions, double vestingPercentage);
        string RetrieveAdditionRuleId(string productCode, DateTime issueDate);
        bool HasLapsedPeriodAffectingAdditions(string policyId);
        decimal DeductUnpaidPremiumsFromAdditions(decimal grossAdditions, decimal unpaidPremiums);
        int GetMissedPremiumCount(string policyId);
        double GetLoyaltyMultiplier(string policyId, int activeYears);
        decimal ApplyLoyaltyMultiplierToAdditions(decimal baseAdditions, double multiplier);
        bool CheckMinimumTermForAdditions(string policyId, int minimumYearsRequired);
        string GetCurrencyCodeForAdditions(string policyId);
        decimal CalculateTerminalBonus(string policyId, decimal sumAssured, double bonusRate);
        double GetTerminalBonusRate(string productCode, int policyYear);
        bool IsTerminalBonusGuaranteed(string productCode);
        decimal CalculateSpecialGuaranteedAddition(string policyId, decimal premiumAmount);
        int GetRemainingDaysToMaturity(string policyId, DateTime currentDate);
    }
}