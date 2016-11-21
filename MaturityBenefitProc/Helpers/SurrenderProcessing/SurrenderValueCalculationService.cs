using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    // Buggy stub — returns incorrect values
    public class SurrenderValueCalculationService : ISurrenderValueCalculationService
    {
        public decimal CalculateGuaranteedSurrenderValue(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public decimal CalculateSpecialSurrenderValue(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public bool IsPolicyEligibleForSurrender(string policyId, DateTime requestDate)
        {
            return false;
        }

        public int GetCompletedPolicyYears(string policyId, DateTime surrenderDate)
        {
            return 0;
        }

        public double GetSurrenderValueFactor(int completedYears, string planCode)
        {
            return 0.0;
        }

        public decimal CalculateAccruedBonuses(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public decimal CalculateTerminalBonus(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public double GetSpecialSurrenderValueFactor(int completedYears, string planCode)
        {
            return 0.0;
        }

        public decimal CalculateTotalPaidPremiums(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public bool ValidateSurrenderRequest(string policyId, string customerId, DateTime requestDate)
        {
            return false;
        }

        public int GetDaysSinceLastPremiumPaid(string policyId, DateTime surrenderDate)
        {
            return 0;
        }

        public decimal CalculateLoanOutstanding(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public decimal CalculateLoanInterestOutstanding(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public decimal CalculateNetSurrenderValue(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public string GetSurrenderStatus(string policyId)
        {
            return null;
        }

        public double GetPaidUpValueRatio(string policyId, DateTime surrenderDate)
        {
            return 0.0;
        }

        public decimal CalculatePaidUpValue(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public bool HasActiveAssignments(string policyId)
        {
            return false;
        }

        public int GetRemainingPolicyTerm(string policyId, DateTime surrenderDate)
        {
            return 0;
        }

        public decimal CalculateSurrenderCharges(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public string GenerateSurrenderQuoteId(string policyId, DateTime requestDate)
        {
            return null;
        }

        public bool IsWithinCoolingOffPeriod(string policyId, DateTime requestDate)
        {
            return false;
        }

        public decimal CalculateVestedReversionaryBonus(string policyId, DateTime surrenderDate)
        {
            return 0m;
        }

        public double GetDiscountRate(string planCode, DateTime surrenderDate)
        {
            return 0.0;
        }

        public decimal CalculateDiscountedValue(decimal futureValue, double discountRate, int remainingYears)
        {
            return 0m;
        }
    }
}