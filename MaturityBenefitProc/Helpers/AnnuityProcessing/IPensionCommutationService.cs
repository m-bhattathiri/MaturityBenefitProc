using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    /// <summary>Calculates lump sum withdrawal limits from pension policies upon maturity.</summary>
    public interface IPensionCommutationService
    {
        decimal CalculateMaximumTaxFreeLumpSum(string policyId, decimal totalFundValue);
        
        decimal CalculateCommutationAmount(string policyId, double commutationFactor);
        
        decimal GetAvailableLifetimeAllowance(string customerId, DateTime calculationDate);
        
        decimal CalculateResidualPensionFund(decimal totalFundValue, decimal commutedAmount);
        
        decimal CalculateTaxableCommutationPortion(string policyId, decimal requestedLumpSum);
        
        decimal GetGuaranteedMinimumPensionValue(string policyId);
        
        decimal CalculateEarlyRetirementReduction(decimal baseAmount, int monthsEarly);
        
        decimal CalculateTerminalBonus(string policyId, DateTime maturityDate);

        double GetCommutationFactor(int ageAtMaturity, string genderCode);
        
        double CalculateTaxFreePercentage(string policyId);
        
        double GetMarketValueReductionRate(string fundId, DateTime withdrawalDate);
        
        double CalculateLifetimeAllowanceUsedPercentage(string customerId, decimal withdrawalAmount);

        bool IsEligibleForCommutation(string policyId, DateTime requestDate);
        
        bool ValidateLumpSumLimit(string policyId, decimal requestedAmount);
        
        bool HasTrivialCommutationRights(string policyId, decimal totalPensionWealth);
        
        bool IsProtectedTaxFreeCashApplicable(string policyId);
        
        bool RequiresSpousalConsent(string policyId, decimal commutationAmount);
        
        bool IsHealthConditionWaiverApplicable(string customerId, string medicalCode);

        int CalculateDaysToMaturity(string policyId, DateTime currentDate);
        
        int GetMinimumRetirementAge(string policyId);
        
        int CountPreviousCommutations(string customerId);
        
        int GetGuaranteePeriodMonths(string policyId);

        string GenerateCommutationReference(string policyId, DateTime requestDate);
        
        string GetTaxCodeForExcessCommutation(string customerId);
        
        string DetermineCommutationTaxBand(decimal taxableAmount);
        
        string GetRegulatoryFrameworkCode(DateTime policyStartDate);
    }
}