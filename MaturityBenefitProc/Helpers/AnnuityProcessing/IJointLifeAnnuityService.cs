using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    /// <summary>Calculates survivor benefits and secondary annuitant payout transitions.</summary>
    public interface IJointLifeAnnuityService
    {
        decimal CalculateSurvivorBenefitAmount(string policyId, decimal baseAnnuityAmount, double survivorPercentage);
        
        decimal CalculateSecondaryAnnuitantPayout(string policyId, DateTime primaryDeathDate);
        
        decimal GetTotalPaidToPrimaryAnnuitant(string policyId);
        
        decimal CalculateProRataPayment(string policyId, DateTime deathDate, decimal monthlyBenefit);
        
        decimal ComputeLumpSumDeathBenefit(string policyId, decimal accountValue, decimal totalPaid);
        
        decimal CalculateTaxablePortion(string policyId, decimal payoutAmount);
        
        decimal GetGuaranteedMinimumDeathBenefit(string policyId);

        double GetSurvivorReductionRate(string policyId);
        
        double CalculateActuarialAdjustmentFactor(int primaryAge, int secondaryAge);
        
        double GetCostOfLivingAdjustmentRate(string policyId, DateTime adjustmentDate);
        
        double ComputeJointLifeExpectancyFactor(int primaryAge, int secondaryAge, string mortalityTableId);

        bool IsSecondaryAnnuitantEligible(string policyId, string secondaryAnnuitantId);
        
        bool ValidateSpousalContinuation(string policyId, DateTime primaryDeathDate);
        
        bool HasGuaranteedPeriodExpired(string policyId, DateTime evaluationDate);
        
        bool IsJointLifePolicyActive(string policyId);
        
        bool RequiresMedallionSignatureGuarantee(decimal payoutAmount);
        
        bool CheckBeneficiaryOverrideExists(string policyId, string secondaryAnnuitantId);

        int GetRemainingGuaranteedPayments(string policyId, DateTime primaryDeathDate);
        
        int CalculateDaysBetweenPayments(DateTime lastPaymentDate, DateTime nextPaymentDate);
        
        int GetSecondaryAnnuitantAge(string secondaryAnnuitantId, DateTime evaluationDate);
        
        int CountProcessedSurvivorClaims(string policyId);

        string GetSecondaryAnnuitantId(string policyId);
        
        string DeterminePayoutTransitionStatus(string policyId);
        
        string GenerateSurvivorClaimReference(string policyId, string secondaryAnnuitantId);
        
        string GetApplicableMortalityTableCode(string policyId, DateTime issueDate);
    }
}