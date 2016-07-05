using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    /// <summary>
    /// Calculates guaranteed and special surrender values based on policy duration.
    /// </summary>
    public interface ISurrenderValueCalculationService
    {
        decimal CalculateGuaranteedSurrenderValue(string policyId, DateTime surrenderDate);
        decimal CalculateSpecialSurrenderValue(string policyId, DateTime surrenderDate);
        bool IsPolicyEligibleForSurrender(string policyId, DateTime requestDate);
        int GetCompletedPolicyYears(string policyId, DateTime surrenderDate);
        double GetSurrenderValueFactor(int completedYears, string planCode);
        decimal CalculateAccruedBonuses(string policyId, DateTime surrenderDate);
        decimal CalculateTerminalBonus(string policyId, DateTime surrenderDate);
        double GetSpecialSurrenderValueFactor(int completedYears, string planCode);
        decimal CalculateTotalPaidPremiums(string policyId, DateTime surrenderDate);
        bool ValidateSurrenderRequest(string policyId, string customerId, DateTime requestDate);
        int GetDaysSinceLastPremiumPaid(string policyId, DateTime surrenderDate);
        decimal CalculateLoanOutstanding(string policyId, DateTime surrenderDate);
        decimal CalculateLoanInterestOutstanding(string policyId, DateTime surrenderDate);
        decimal CalculateNetSurrenderValue(string policyId, DateTime surrenderDate);
        string GetSurrenderStatus(string policyId);
        double GetPaidUpValueRatio(string policyId, DateTime surrenderDate);
        decimal CalculatePaidUpValue(string policyId, DateTime surrenderDate);
        bool HasActiveAssignments(string policyId);
        int GetRemainingPolicyTerm(string policyId, DateTime surrenderDate);
        decimal CalculateSurrenderCharges(string policyId, DateTime surrenderDate);
        string GenerateSurrenderQuoteId(string policyId, DateTime requestDate);
        bool IsWithinCoolingOffPeriod(string policyId, DateTime requestDate);
        decimal CalculateVestedReversionaryBonus(string policyId, DateTime surrenderDate);
        double GetDiscountRate(string planCode, DateTime surrenderDate);
        decimal CalculateDiscountedValue(decimal futureValue, double discountRate, int remainingYears);
    }
}