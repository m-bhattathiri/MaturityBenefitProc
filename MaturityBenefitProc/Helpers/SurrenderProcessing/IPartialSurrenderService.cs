using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    /// <summary>Handles rules and limits for partial withdrawals and surrenders.</summary>
    public interface IPartialSurrenderService
    {
        decimal CalculateMaximumWithdrawalAmount(string policyId, DateTime effectiveDate);
        
        decimal CalculateSurrenderCharge(string policyId, decimal requestedAmount);
        
        decimal GetAvailableFreeWithdrawalAmount(string policyId, DateTime requestDate);
        
        decimal CalculateNetPayoutAmount(decimal grossAmount, decimal surrenderCharge, decimal taxWithholding);
        
        decimal GetMinimumRemainingBalanceRequired(string productCode);
        
        decimal CalculateProratedRiderDeduction(string policyId, decimal withdrawalAmount);
        
        decimal CalculateMarketValueAdjustment(string policyId, decimal surrenderAmount, DateTime calculationDate);

        bool IsEligibleForPartialSurrender(string policyId, DateTime requestDate);
        
        bool ValidateMinimumWithdrawalAmount(string productCode, decimal requestedAmount);
        
        bool HasExceededAnnualWithdrawalLimit(string policyId, DateTime requestDate);
        
        bool IsPolicyInLockupPeriod(string policyId, DateTime requestDate);
        
        bool RequiresSpousalConsent(string policyId, decimal withdrawalAmount);
        
        bool IsSystematicWithdrawalActive(string policyId);

        double GetSurrenderChargePercentage(string policyId, int policyYear);
        
        double CalculateTaxWithholdingRate(string stateCode, bool isFederal);
        
        double GetFreeWithdrawalPercentage(string productCode);
        
        double CalculateProRataReductionFactor(decimal withdrawalAmount, decimal accountValue);

        int GetRemainingFreeWithdrawalsCount(string policyId, int calendarYear);
        
        int GetDaysUntilSurrenderChargeExpires(string policyId, DateTime currentDate);
        
        int GetPolicyYear(string policyId, DateTime effectiveDate);
        
        int GetMaximumAllowedWithdrawalsPerYear(string productCode);

        string GenerateSurrenderTransactionId(string policyId, DateTime requestDate);
        
        string GetSurrenderChargeScheduleCode(string policyId);
        
        string DetermineTaxDistributionCode(int ageAtWithdrawal, bool isQualified);
        
        string GetWithdrawalDenialReasonCode(string policyId, decimal requestedAmount);
    }
}