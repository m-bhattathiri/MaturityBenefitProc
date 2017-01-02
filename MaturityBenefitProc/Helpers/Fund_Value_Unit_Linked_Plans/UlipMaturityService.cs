using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans
{
    // Buggy stub — returns incorrect values
    public class UlipMaturityService : IUlipMaturityService
    {
        public decimal CalculateTotalFundValue(string policyId, DateTime maturityDate) => 0m;
        public decimal GetNavOnDate(string fundId, DateTime date) => 0m;
        public decimal CalculateMortalityChargeRefund(string policyId) => 0m;
        public decimal CalculateLoyaltyAdditions(string policyId, int policyTerm) => 0m;
        public decimal CalculateWealthBoosters(string policyId, decimal averageFundValue) => 0m;
        public decimal GetTotalAllocatedUnits(string policyId, string fundId) => 0m;
        public decimal CalculateSurrenderValue(string policyId, DateTime surrenderDate) => 0m;
        public decimal GetFundManagementCharge(string fundId, double fmcRate) => 0m;
        public decimal CalculateGuaranteeAddition(string policyId, decimal basePremium) => 0m;
        public decimal GetTotalPremiumPaid(string policyId) => 0m;

        public double GetFundAllocationRatio(string policyId, string fundId) => 0.0;
        public double GetMortalityRate(int ageAtEntry, int policyYear) => 0.0;
        public double GetNavGrowthRate(string fundId, DateTime startDate, DateTime endDate) => 0.0;
        public double GetBonusRate(string policyId, int policyYear) => 0.0;
        public double GetTaxRate(string stateCode, string taxCategory) => 0.0;

        public bool IsEligibleForMortalityRefund(string policyId) => false;
        public bool IsEligibleForLoyaltyAdditions(string policyId) => false;
        public bool IsEligibleForWealthBoosters(string policyId) => false;
        public bool ValidateFundSwitch(string policyId, string fromFundId, string toFundId, decimal amount) => false;
        public bool HasActivePremiumHoliday(string policyId, DateTime checkDate) => false;
        public bool IsPolicyMatured(string policyId, DateTime currentDate) => false;
        public bool ValidateNavDate(string fundId, DateTime navDate) => false;
        public bool IsTopUpAllowed(string policyId, decimal topUpAmount) => false;

        public int GetCompletedPolicyYears(string policyId, DateTime currentDate) => 0;
        public int GetRemainingPremiumTerms(string policyId) => 0;
        public int GetTotalFundSwitchesUsed(string policyId, int year) => 0;
        public int GetDaysToMaturity(string policyId, DateTime currentDate) => 0;
        public int GetGracePeriodDays(string policyId) => 0;
        public int GetFreeLookPeriodDays(string policyId) => 0;

        public string GetPrimaryFundId(string policyId) => null;
        public string GetPolicyStatus(string policyId) => null;
        public string GenerateMaturityStatementId(string policyId, DateTime maturityDate) => null;
        public string GetTaxCategoryCode(string policyId) => null;
        public string GetRiderCode(string policyId, string riderType) => null;
        public string GetFundName(string fundId) => null;

        public decimal CalculateFinalMaturityPayout(string policyId, DateTime maturityDate, out string payoutStatus)
        {
            payoutStatus = null;
            return 0m;
        }
        public decimal CalculateTopUpFundValue(string policyId, DateTime maturityDate) => 0m;
        public decimal CalculateBaseFundValue(string policyId, DateTime maturityDate) => 0m;
        public decimal ApplyDiscontinuanceCharge(string policyId, decimal fundValue) => 0m;
        public decimal CalculatePartialWithdrawalImpact(string policyId, decimal withdrawalAmount) => 0m;
    }
}