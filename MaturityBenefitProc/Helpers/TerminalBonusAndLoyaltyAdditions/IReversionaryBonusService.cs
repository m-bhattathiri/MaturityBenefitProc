using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    /// <summary>
    /// Applies annual bonus rates to sum assured for participating policies.
    /// </summary>
    public interface IReversionaryBonusService
    {
        decimal CalculateAnnualBonus(string policyId, decimal sumAssured, double bonusRate);
        decimal CalculateAccruedReversionaryBonus(string policyId, DateTime calculationDate);
        decimal GetTotalDeclaredBonus(string policyId);
        decimal CalculateInterimBonus(string policyId, DateTime exitDate, decimal sumAssured);
        decimal ComputeVestedBonusAmount(string policyId, int activeYears);
        decimal CalculateTerminalBonus(string policyId, decimal accruedBonus, double terminalBonusRate);
        decimal GetSurrenderValueOfBonus(string policyId, decimal totalBonus, double surrenderFactor);
        decimal CalculateLoyaltyAdditionAmount(string policyId, decimal baseAmount, double loyaltyRate);
        
        double GetCurrentBonusRate(string planCode, int policyTerm);
        double GetInterimBonusRate(string planCode, DateTime exitDate);
        double CalculateBonusCompoundingFactor(int yearsInForce, double annualRate);
        double GetTerminalBonusRate(string planCode, int durationInYears);
        double FetchLoyaltyAdditionPercentage(string policyId, int premiumPayingTerm);
        
        bool IsPolicyEligibleForBonus(string policyId, DateTime evaluationDate);
        bool HasGuaranteedAdditions(string planCode);
        bool IsParticipatingPolicy(string policyId);
        bool ValidateBonusRateApplicability(string planCode, double rate, DateTime effectiveDate);
        bool CheckLoyaltyAdditionEligibility(string policyId, int paidPremiumsCount);
        bool IsBonusVested(string policyId, DateTime checkDate);
        
        int GetYearsEligibleForBonus(string policyId);
        int CalculateDaysSinceLastDeclaration(string policyId, DateTime currentDate);
        int GetPendingBonusDeclarationsCount(string policyId);
        int GetMinimumTermForTerminalBonus(string planCode);
        
        string GetBonusRateTableId(string planCode, DateTime issueDate);
        string DetermineBonusStatus(string policyId);
        string GetLoyaltyAdditionScaleCode(string planCode, int policyTerm);
        string GetLastDeclarationFinancialYear(string policyId);
    }
}