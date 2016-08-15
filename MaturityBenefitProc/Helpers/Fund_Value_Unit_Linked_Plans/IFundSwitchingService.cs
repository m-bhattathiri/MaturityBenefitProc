using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.Fund_Value_Unit_Linked_Plans
{
    /// <summary>
    /// Handles automatic fund switches to debt/liquid funds as maturity approaches.
    /// </summary>
    public interface IFundSwitchingService
    {
        bool IsPolicyEligibleForAutoSwitch(string policyId);
        int GetDaysToMaturity(string policyId, DateTime currentDate);
        decimal CalculateCurrentEquityValue(string policyId);
        decimal CalculateCurrentDebtValue(string policyId);
        double GetTargetDebtAllocationPercentage(int daysToMaturity);
        double GetCurrentEquityAllocationPercentage(string policyId);
        decimal CalculateRequiredSwitchAmount(string policyId, double targetDebtPercentage);
        bool ValidateSwitchAmountLimits(decimal switchAmount);
        string InitiateFundSwitch(string policyId, string sourceFundId, string targetFundId, decimal amount);
        bool CheckPendingSwitchRequests(string policyId);
        int GetCompletedSwitchCount(string policyId, DateTime startDate, DateTime endDate);
        decimal GetApplicableNav(string fundId, DateTime transactionDate);
        double CalculateSwitchChargePercentage(string policyId);
        decimal CalculateSwitchChargeAmount(decimal switchAmount, double chargePercentage);
        string GetDefaultLiquidFundId(string planCode);
        string GetDefaultDebtFundId(string planCode);
        bool VerifyFundActiveStatus(string fundId);
        decimal GetMinimumSwitchAmount(string planCode);
        decimal GetMaximumSwitchAmount(string planCode);
        int GetRemainingFreeSwitches(string policyId, int currentYear);
        bool ProcessSystematicTransferPlan(string policyId, DateTime executionDate);
        string GenerateSwitchTransactionReference(string policyId, DateTime executionDate);
        double CalculateMarketVolatilityIndex(DateTime evaluationDate);
        bool ShouldAccelerateSwitch(double volatilityIndex, int daysToMaturity);
        decimal CalculateProjectedMaturityValue(string policyId, double assumedGrowthRate);
    }
}