using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.ReinsuranceAndPoolManagement
{
    /// <summary>
    /// Evaluates proportional and non-proportional treaty terms for maturities.
    /// </summary>
    public interface ITreatyCalculationService
    {
        decimal CalculateQuotaShareRetention(string treatyId, decimal maturityAmount);
        
        decimal CalculateQuotaShareCeded(string treatyId, decimal maturityAmount);
        
        double GetSurplusSharePercentage(string treatyId, decimal sumAssured, decimal retentionLimit);
        
        decimal CalculateSurplusCededAmount(string treatyId, decimal maturityAmount, double surplusPercentage);
        
        bool IsEligibleForProportionalTreaty(string policyId, string treatyId, DateTime maturityDate);
        
        decimal CalculateExcessOfLossRecovery(string treatyId, decimal totalLossAmount, decimal deductible);
        
        decimal CalculateStopLossRecovery(string poolId, decimal aggregateLosses, decimal attachmentPoint);
        
        bool ValidateLayerExhaustion(string layerId, decimal accumulatedLosses);
        
        int GetRemainingReinstatements(string treatyId, int usedReinstatements);
        
        decimal CalculateReinstatementPremium(string treatyId, decimal recoveredAmount, double proRataRate);
        
        decimal CalculatePoolCapacity(string poolId, DateTime effectiveDate);
        
        double GetParticipantShareRatio(string poolId, string participantId);
        
        decimal CalculateParticipantLiability(string poolId, string participantId, decimal totalMaturityPayout);
        
        string GetLeadReinsurerId(string treatyId);
        
        int GetActivePoolParticipantsCount(string poolId);
        
        decimal CalculateTerminalBonusCeded(string treatyId, decimal terminalBonus, double cessionRate);
        
        decimal CalculateGuaranteedAdditionRecovery(string treatyId, decimal guaranteedAdditionAmount);
        
        bool CheckMaturityDateWithinTreatyPeriod(string treatyId, DateTime maturityDate);
        
        string ResolveApplicableTreatyCode(string policyId, DateTime issueDate, DateTime maturityDate);
        
        int CalculateDaysInForce(DateTime issueDate, DateTime maturityDate);
        
        decimal GetTreatyCapacityLimit(string treatyId);
        
        bool ValidateMinimumCessionAmount(string treatyId, decimal cededAmount);
        
        double CalculateLossRatio(decimal incurredLosses, decimal earnedPremiums);
        
        string GetTreatyCurrencyCode(string treatyId);
        
        decimal ConvertCurrencyForTreaty(string treatyId, decimal amount, double exchangeRate);
    }
}