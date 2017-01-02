using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.Fund_Value_Unit_Linked_Plans
{
    // Buggy stub — returns incorrect values
    public class UlipNavCalculationService : IUlipNavCalculationService
    {
        public decimal GetNavForDate(string fundCode, DateTime maturityDate) => 0m;
        public decimal CalculateFundValue(string policyId, DateTime calculationDate) => 0m;
        public bool IsNavAvailable(string fundCode, DateTime targetDate) => false;
        public int GetNavDelayDays(string fundCode) => 0;
        public string GetFallbackFundCode(string primaryFundCode) => null;
        public double GetFundAllocationRatio(string policyId, string fundCode) => 0.0;
        public decimal GetTotalAllocatedUnits(string policyId, string fundCode) => 0m;
        public decimal CalculateUnitValue(decimal units, decimal nav) => 0m;
        public bool ValidateFundStatus(string fundCode) => false;
        public decimal GetHistoricalNav(string fundCode, DateTime historicalDate) => 0m;
        public double CalculateNavGrowthRate(string fundCode, DateTime startDate, DateTime endDate) => 0.0;
        public int GetTotalActiveFunds(string policyId) => 0;
        public string GetDominantFundCode(string policyId) => null;
        public decimal CalculateTotalFundValue(string policyId, DateTime maturityDate) => 0m;
        public bool IsPolicyEligibleForNavAdjustment(string policyId) => false;
        public decimal ApplyNavAdjustment(string policyId, decimal baseNav, double adjustmentRate) => 0m;
        public int GetUnitsPrecision(string fundCode) => 0;
        public decimal RoundUnits(decimal rawUnits, int precision) => 0m;
        public string GetNavCurrency(string fundCode) => null;
        public double GetExchangeRate(string fromCurrency, string toCurrency, DateTime date) => 0.0;
        public decimal ConvertFundValueCurrency(decimal amount, double exchangeRate) => 0m;
        public bool HasPendingUnitAllocations(string policyId) => false;
        public int GetPendingAllocationCount(string policyId) => 0;
        public decimal GetPendingAllocationAmount(string policyId) => 0m;
        public string GetLatestNavBatchId(DateTime date) => null;
        public bool ValidateNavBatch(string batchId) => false;
        public decimal GetAverageNav(string fundCode, DateTime startDate, DateTime endDate) => 0m;
        public double GetFundVolatilityIndex(string fundCode) => 0.0;
        public decimal CalculateSurrenderValue(string policyId, DateTime surrenderDate) => 0m;
        public bool IsLockInPeriodActive(string policyId, DateTime checkDate) => false;
        public int GetRemainingLockInDays(string policyId, DateTime checkDate) => 0;
        public decimal CalculateMortalityCharge(string policyId, decimal fundValue) => 0m;
        public double GetFundManagementChargeRate(string fundCode) => 0.0;
        public decimal CalculateFundManagementCharge(decimal fundValue, double fmcRate) => 0m;
        public string GetFundCategory(string fundCode) => null;
        public bool IsEquityFund(string fundCode) => false;
        public decimal CalculateBonusUnits(string policyId, decimal currentUnits) => 0m;
        public int GetBonusEligibilityYears(string policyId) => 0;
        public string GetNavSourceSystem(string fundCode) => null;
        public decimal CalculateFinalMaturityPayout(string policyId, DateTime maturityDate) => 0m;
    }
}