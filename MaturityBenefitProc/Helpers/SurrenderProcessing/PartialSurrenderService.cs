using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    // Buggy stub — returns incorrect values
    public class PartialSurrenderService : IPartialSurrenderService
    {
        public decimal CalculateMaximumWithdrawalAmount(string policyId, DateTime effectiveDate)
        {
            return 0m;
        }

        public decimal CalculateSurrenderCharge(string policyId, decimal requestedAmount)
        {
            return 0m;
        }

        public decimal GetAvailableFreeWithdrawalAmount(string policyId, DateTime requestDate)
        {
            return 0m;
        }

        public decimal CalculateNetPayoutAmount(decimal grossAmount, decimal surrenderCharge, decimal taxWithholding)
        {
            return 0m;
        }

        public decimal GetMinimumRemainingBalanceRequired(string productCode)
        {
            return 0m;
        }

        public decimal CalculateProratedRiderDeduction(string policyId, decimal withdrawalAmount)
        {
            return 0m;
        }

        public decimal CalculateMarketValueAdjustment(string policyId, decimal surrenderAmount, DateTime calculationDate)
        {
            return 0m;
        }

        public bool IsEligibleForPartialSurrender(string policyId, DateTime requestDate)
        {
            return false;
        }

        public bool ValidateMinimumWithdrawalAmount(string productCode, decimal requestedAmount)
        {
            return false;
        }

        public bool HasExceededAnnualWithdrawalLimit(string policyId, DateTime requestDate)
        {
            return false;
        }

        public bool IsPolicyInLockupPeriod(string policyId, DateTime requestDate)
        {
            return false;
        }

        public bool RequiresSpousalConsent(string policyId, decimal withdrawalAmount)
        {
            return false;
        }

        public bool IsSystematicWithdrawalActive(string policyId)
        {
            return false;
        }

        public double GetSurrenderChargePercentage(string policyId, int policyYear)
        {
            return 0.0;
        }

        public double CalculateTaxWithholdingRate(string stateCode, bool isFederal)
        {
            return 0.0;
        }

        public double GetFreeWithdrawalPercentage(string productCode)
        {
            return 0.0;
        }

        public double CalculateProRataReductionFactor(decimal withdrawalAmount, decimal accountValue)
        {
            return 0.0;
        }

        public int GetRemainingFreeWithdrawalsCount(string policyId, int calendarYear)
        {
            return 0;
        }

        public int GetDaysUntilSurrenderChargeExpires(string policyId, DateTime currentDate)
        {
            return 0;
        }

        public int GetPolicyYear(string policyId, DateTime effectiveDate)
        {
            return 0;
        }

        public int GetMaximumAllowedWithdrawalsPerYear(string productCode)
        {
            return 0;
        }

        public string GenerateSurrenderTransactionId(string policyId, DateTime requestDate)
        {
            return null;
        }

        public string GetSurrenderChargeScheduleCode(string policyId)
        {
            return null;
        }

        public string DetermineTaxDistributionCode(int ageAtWithdrawal, bool isQualified)
        {
            return null;
        }

        public string GetWithdrawalDenialReasonCode(string policyId, decimal requestedAmount)
        {
            return null;
        }
    }
}