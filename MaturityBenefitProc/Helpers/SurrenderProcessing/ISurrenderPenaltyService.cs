using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    /// <summary>Determines penalties and deduction charges for early policy surrender.</summary>
    public interface ISurrenderPenaltyService
    {
        decimal CalculateBasePenaltyAmount(string policyId, decimal surrenderValue);

        double GetPenaltyPercentage(string policyId, DateTime surrenderDate);

        bool IsEligibleForPenaltyWaiver(string policyId, string waiverCode);

        int GetRemainingLockInDays(string policyId, DateTime currentDate);

        string GetApplicablePenaltyTierCode(string policyId, int activeYears);

        decimal CalculateMarketValueAdjustment(string policyId, decimal currentFundValue, double marketRate);

        decimal GetTotalDeductionCharges(string policyId, DateTime effectiveDate);

        bool ValidateSurrenderDate(string policyId, DateTime requestedDate);

        double CalculateProratedBonusRecoveryRate(string policyId, int monthsActive);

        decimal CalculateTaxWithholdingAmount(string policyId, decimal netSurrenderAmount, double taxRate);

        int GetCompletedPolicyYears(string policyId, DateTime surrenderDate);

        string RetrievePenaltyRuleId(string productCode, DateTime issueDate);

        bool HasOutstandingLoans(string policyId);

        decimal CalculateLoanInterestDeduction(string policyId, DateTime calculationDate);

        double GetSurrenderChargeFactor(string policyId, int durationInMonths);

        decimal CalculateFinalNetSurrenderValue(string policyId, decimal grossValue, decimal totalPenalties);

        bool RequiresManagerApproval(string policyId, decimal penaltyAmount);

        int GetFreeWithdrawalCount(string policyId, DateTime currentYearStart);
    }
}