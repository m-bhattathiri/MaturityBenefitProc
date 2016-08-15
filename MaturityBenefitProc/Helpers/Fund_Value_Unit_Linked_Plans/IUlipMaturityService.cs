using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans
{
    /// <summary>
    /// Consolidates fund values and mortality charge refunds for final ULIP payouts.
    /// </summary>
    public interface IUlipMaturityService
    {
        decimal CalculateTotalFundValue(string policyId, DateTime maturityDate);
        decimal GetNavOnDate(string fundId, DateTime date);
        decimal CalculateMortalityChargeRefund(string policyId);
        decimal CalculateLoyaltyAdditions(string policyId, int policyTerm);
        decimal CalculateWealthBoosters(string policyId, decimal averageFundValue);
        decimal GetTotalAllocatedUnits(string policyId, string fundId);
        decimal CalculateSurrenderValue(string policyId, DateTime surrenderDate);
        decimal GetFundManagementCharge(string fundId, double fmcRate);
        decimal CalculateGuaranteeAddition(string policyId, decimal basePremium);
        decimal GetTotalPremiumPaid(string policyId);

        double GetFundAllocationRatio(string policyId, string fundId);
        double GetMortalityRate(int ageAtEntry, int policyYear);
        double GetNavGrowthRate(string fundId, DateTime startDate, DateTime endDate);
        double GetBonusRate(string policyId, int policyYear);
        double GetTaxRate(string stateCode, string taxCategory);

        bool IsEligibleForMortalityRefund(string policyId);
        bool IsEligibleForLoyaltyAdditions(string policyId);
        bool IsEligibleForWealthBoosters(string policyId);
        bool ValidateFundSwitch(string policyId, string fromFundId, string toFundId, decimal amount);
        bool HasActivePremiumHoliday(string policyId, DateTime checkDate);
        bool IsPolicyMatured(string policyId, DateTime currentDate);
        bool ValidateNavDate(string fundId, DateTime navDate);
        bool IsTopUpAllowed(string policyId, decimal topUpAmount);

        int GetCompletedPolicyYears(string policyId, DateTime currentDate);
        int GetRemainingPremiumTerms(string policyId);
        int GetTotalFundSwitchesUsed(string policyId, int year);
        int GetDaysToMaturity(string policyId, DateTime currentDate);
        int GetGracePeriodDays(string policyId);
        int GetFreeLookPeriodDays(string policyId);

        string GetPrimaryFundId(string policyId);
        string GetPolicyStatus(string policyId);
        string GenerateMaturityStatementId(string policyId, DateTime maturityDate);
        string GetTaxCategoryCode(string policyId);
        string GetRiderCode(string policyId, string riderType);
        string GetFundName(string fundId);

        decimal CalculateFinalMaturityPayout(string policyId, DateTime maturityDate, out string payoutStatus);
        decimal CalculateTopUpFundValue(string policyId, DateTime maturityDate);
        decimal CalculateBaseFundValue(string policyId, DateTime maturityDate);
        decimal ApplyDiscontinuanceCharge(string policyId, decimal fundValue);
        decimal CalculatePartialWithdrawalImpact(string policyId, decimal withdrawalAmount);
    }
}