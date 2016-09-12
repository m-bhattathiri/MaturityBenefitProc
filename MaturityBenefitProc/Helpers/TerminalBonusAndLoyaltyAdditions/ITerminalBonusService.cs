using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    /// <summary>Calculates terminal or final additional bonuses declared at maturity.</summary>
    public interface ITerminalBonusService
    {
        decimal CalculateBaseTerminalBonus(string policyId, decimal sumAssured, DateTime maturityDate);
        
        decimal CalculateLoyaltyAdditionAmount(string policyId, int premiumPayingYears);
        
        decimal GetAccruedReversionaryBonus(string policyId);
        
        decimal ComputeFinalAdditionalBonus(string policyId, decimal totalPremiumsPaid);
        
        decimal CalculateVestedBonusTotal(string policyId, DateTime calculationDate);
        
        decimal GetSpecialSurrenderValueBonus(string policyId, decimal baseSurrenderValue);
        
        decimal CalculateProratedTerminalBonus(string policyId, DateTime exitDate, int daysActive);
        
        decimal ApplyBonusMultiplier(decimal baseBonus, double multiplierRate);

        double GetTerminalBonusRate(string planCode, int policyTerm);
        
        double GetLoyaltyAdditionRate(string planCode, int completedYears);
        
        double CalculateBonusYield(decimal totalBonus, decimal totalPremiums);
        
        double GetFundPerformanceFactor(string fundId, DateTime evaluationDate);
        
        double GetParticipatingFundRatio(string cohortId);

        bool IsEligibleForTerminalBonus(string policyId, string status);
        
        bool IsLoyaltyAdditionApplicable(string planCode, int elapsedYears);
        
        bool ValidateBonusDeclaration(string declarationId, DateTime effectiveDate);
        
        bool HasClaimedPreviousBonuses(string policyId);
        
        bool IsPolicyInParticipatingFund(string policyId);

        int GetCompletedPolicyYears(string policyId, DateTime maturityDate);
        
        int GetMinimumYearsForTerminalBonus(string planCode);
        
        int CalculateDaysSinceLastBonusDeclaration(DateTime lastDeclarationDate);
        
        int GetTotalBonusUnitsAllocated(string policyId);

        string GetBonusDeclarationId(string planCode, DateTime declarationYear);
        
        string DetermineBonusCohort(string policyId, DateTime issueDate);
        
        string GetTerminalBonusStatus(string policyId);
    }
}