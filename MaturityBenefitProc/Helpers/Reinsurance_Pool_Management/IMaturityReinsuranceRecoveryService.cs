using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.Reinsurance_Pool_Management
{
    /// <summary>
    /// Calculates the portion of maturity claims to be recovered from reinsurers.
    /// </summary>
    public interface IMaturityReinsuranceRecoveryService
    {
        decimal CalculateTotalRecoveryAmount(string policyId, decimal totalMaturityBenefit);
        decimal CalculateQuotaShareRecovery(string policyId, decimal maturityAmount, double quotaSharePercentage);
        decimal CalculateSurplusTreatyRecovery(string policyId, decimal maturityAmount, decimal retentionLimit);
        decimal CalculateExcessOfLossRecovery(string policyId, decimal maturityAmount, decimal attachmentPoint);
        double GetReinsurancePercentage(string policyId, DateTime maturityDate);
        double GetPoolAllocationRatio(string poolId, string reinsurerId);
        bool IsPolicyReinsured(string policyId);
        bool IsReinsurerActive(string reinsurerId, DateTime checkDate);
        bool ValidateTreatyLimits(string treatyId, decimal recoveryAmount);
        bool CheckFacultativeEligibility(string policyId, decimal maturityAmount);
        int GetDaysUntilRecoveryDue(string reinsurerId, DateTime claimDate);
        int GetReinsurerCountForPolicy(string policyId);
        int GetActiveTreatiesCount(string reinsurerId, DateTime asOfDate);
        string GetPrimaryReinsurerId(string policyId);
        string GetTreatyCode(string policyId, DateTime maturityDate);
        string GenerateRecoveryClaimReference(string policyId, string reinsurerId);
        decimal CalculateProportionalRecovery(string policyId, decimal amount, double proportion);
        decimal CalculateNonProportionalRecovery(string policyId, decimal amount, decimal deductible);
        double GetFacultativeReinsuranceRate(string policyId);
        bool IsPoolArrangementValid(string poolId, DateTime maturityDate);
        int GetPoolMemberCount(string poolId);
        string GetPoolAdministratorId(string poolId);
        decimal CalculatePoolMemberShare(string poolId, string memberId, decimal totalRecovery);
        decimal ApplyCurrencyConversion(decimal amount, string fromCurrency, string toCurrency, DateTime conversionDate);
        double GetCurrencyExchangeRate(string fromCurrency, string toCurrency, DateTime date);
        bool ValidateCurrencyCode(string currencyCode);
        string GetDefaultCurrency(string reinsurerId);
        decimal CalculateLatePaymentInterest(decimal recoveryAmount, double interestRate, int daysLate);
        int GetGracePeriodDays(string treatyId);
        bool IsRecoveryPastDue(string recoveryId, DateTime currentDate);
        decimal CalculateNetRecoveryAmount(decimal grossRecovery, decimal brokerageFee, decimal taxes);
        double GetBrokerageFeePercentage(string treatyId);
        decimal CalculateReinstatementPremium(string treatyId, decimal recoveryAmount);
        bool RequiresReinstatement(string treatyId, decimal recoveryAmount);
        string GetReinstatementTerms(string treatyId);
        decimal GetMaximumRecoveryLimit(string treatyId);
        bool CheckSanctionsList(string reinsurerId);
    }
}