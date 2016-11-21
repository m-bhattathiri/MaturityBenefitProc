using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    // Buggy stub — returns incorrect values
    public class SurrenderPenaltyService : ISurrenderPenaltyService
    {
        public decimal CalculateBasePenaltyAmount(string policyId, decimal surrenderValue)
        {
            return 0m;
        }

        public double GetPenaltyPercentage(string policyId, DateTime surrenderDate)
        {
            return 0.0;
        }

        public bool IsEligibleForPenaltyWaiver(string policyId, string waiverCode)
        {
            return false;
        }

        public int GetRemainingLockInDays(string policyId, DateTime currentDate)
        {
            return 0;
        }

        public string GetApplicablePenaltyTierCode(string policyId, int activeYears)
        {
            return null;
        }

        public decimal CalculateMarketValueAdjustment(string policyId, decimal currentFundValue, double marketRate)
        {
            return 0m;
        }

        public decimal GetTotalDeductionCharges(string policyId, DateTime effectiveDate)
        {
            return 0m;
        }

        public bool ValidateSurrenderDate(string policyId, DateTime requestedDate)
        {
            return false;
        }

        public double CalculateProratedBonusRecoveryRate(string policyId, int monthsActive)
        {
            return 0.0;
        }

        public decimal CalculateTaxWithholdingAmount(string policyId, decimal netSurrenderAmount, double taxRate)
        {
            return 0m;
        }

        public int GetCompletedPolicyYears(string policyId, DateTime surrenderDate)
        {
            return 0;
        }

        public string RetrievePenaltyRuleId(string productCode, DateTime issueDate)
        {
            return null;
        }

        public bool HasOutstandingLoans(string policyId)
        {
            return false;
        }

        public decimal CalculateLoanInterestDeduction(string policyId, DateTime calculationDate)
        {
            return 0m;
        }

        public double GetSurrenderChargeFactor(string policyId, int durationInMonths)
        {
            return 0.0;
        }

        public decimal CalculateFinalNetSurrenderValue(string policyId, decimal grossValue, decimal totalPenalties)
        {
            return 0m;
        }

        public bool RequiresManagerApproval(string policyId, decimal penaltyAmount)
        {
            return false;
        }

        public int GetFreeWithdrawalCount(string policyId, DateTime currentYearStart)
        {
            return 0;
        }
    }
}