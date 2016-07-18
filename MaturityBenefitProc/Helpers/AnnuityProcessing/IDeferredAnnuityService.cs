using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    /// <summary>Manages accumulation phases and vesting dates for deferred annuity plans.</summary>
    public interface IDeferredAnnuityService
    {
        decimal CalculateAccumulatedValue(string policyId, DateTime calculationDate);
        
        string GetVestingStatus(string policyId);
        
        bool IsEligibleForSurrender(string policyId, DateTime requestDate);
        
        decimal CalculateSurrenderValue(string policyId, decimal currentAccumulation, double surrenderChargeRate);
        
        double GetGuaranteedAdditionRate(string planCode, int policyYear);
        
        int GetRemainingAccumulationMonths(string policyId, DateTime currentDate);
        
        string GenerateVestingQuotationId(string policyId);
        
        bool ValidateDefermentPeriod(string planCode, int defermentYears);
        
        decimal CalculateDeathBenefit(string policyId, DateTime dateOfDeath);
        
        double CalculateBonusRatio(string policyId, int accumulationYears);
        
        int GetPaidPremiumsCount(string policyId);
        
        string GetAnnuityOptionCode(string policyId);
        
        decimal CalculateProjectedMaturityValue(string policyId, double assumedInterestRate);
        
        bool CheckVestingConditionMet(string policyId, DateTime evaluationDate);
        
        decimal ApplyTerminalBonus(string policyId, decimal baseAmount);
        
        double GetLoyaltyAdditionPercentage(int completedYears);
        
        int CalculateDaysToVesting(string policyId, DateTime currentDate);
        
        string UpdateAccumulationPhaseStatus(string policyId, string newStatusCode);
    }
}