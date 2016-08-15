using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.Fund_Value_Unit_Linked_Plans
{
    /// <summary>
    /// Retrieves and applies the correct Net Asset Value for maturity dates.
    /// </summary>
    public interface IUlipNavCalculationService
    {
        decimal GetNavForDate(string fundCode, DateTime maturityDate);
        decimal CalculateFundValue(string policyId, DateTime calculationDate);
        bool IsNavAvailable(string fundCode, DateTime targetDate);
        int GetNavDelayDays(string fundCode);
        string GetFallbackFundCode(string primaryFundCode);
        double GetFundAllocationRatio(string policyId, string fundCode);
        decimal GetTotalAllocatedUnits(string policyId, string fundCode);
        decimal CalculateUnitValue(decimal units, decimal nav);
        bool ValidateFundStatus(string fundCode);
        decimal GetHistoricalNav(string fundCode, DateTime historicalDate);
        double CalculateNavGrowthRate(string fundCode, DateTime startDate, DateTime endDate);
        int GetTotalActiveFunds(string policyId);
        string GetDominantFundCode(string policyId);
        decimal CalculateTotalFundValue(string policyId, DateTime maturityDate);
        bool IsPolicyEligibleForNavAdjustment(string policyId);
        decimal ApplyNavAdjustment(string policyId, decimal baseNav, double adjustmentRate);
        int GetUnitsPrecision(string fundCode);
        decimal RoundUnits(decimal rawUnits, int precision);
        string GetNavCurrency(string fundCode);
        double GetExchangeRate(string fromCurrency, string toCurrency, DateTime date);
        decimal ConvertFundValueCurrency(decimal amount, double exchangeRate);
        bool HasPendingUnitAllocations(string policyId);
        int GetPendingAllocationCount(string policyId);
        decimal GetPendingAllocationAmount(string policyId);
        string GetLatestNavBatchId(DateTime date);
        bool ValidateNavBatch(string batchId);
        decimal GetAverageNav(string fundCode, DateTime startDate, DateTime endDate);
        double GetFundVolatilityIndex(string fundCode);
        decimal CalculateSurrenderValue(string policyId, DateTime surrenderDate);
        bool IsLockInPeriodActive(string policyId, DateTime checkDate);
        int GetRemainingLockInDays(string policyId, DateTime checkDate);
        decimal CalculateMortalityCharge(string policyId, decimal fundValue);
        double GetFundManagementChargeRate(string fundCode);
        decimal CalculateFundManagementCharge(decimal fundValue, double fmcRate);
        string GetFundCategory(string fundCode);
        bool IsEquityFund(string fundCode);
        decimal CalculateBonusUnits(string policyId, decimal currentUnits);
        int GetBonusEligibilityYears(string policyId);
        string GetNavSourceSystem(string fundCode);
        decimal CalculateFinalMaturityPayout(string policyId, DateTime maturityDate);
    }
}