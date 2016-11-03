using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.BonusComputation
{
    public interface IBonusComputationService
    {
        BonusComputationResult ComputeSimpleReversionaryBonus(string policyNumber, decimal sumAssured, int policyYears);

        BonusComputationResult ComputeCompoundReversionaryBonus(string policyNumber, decimal sumAssured, decimal existingBonus, int policyYears);

        BonusComputationResult ComputeTerminalBonus(string policyNumber, decimal sumAssured, int completedYears, string policyType);

        decimal GetBonusRate(string policyType, int policyYear);

        decimal CalculateLoyaltyAddition(decimal sumAssured, int completedYears, string policyType);

        decimal GetAccruedBonusAmount(string policyNumber);

        bool IsBonusEligible(string policyNumber, int minimumYears);

        decimal CalculateInterimBonus(decimal sumAssured, decimal bonusRate, int daysFromLastValuation);

        BonusComputationResult GetBonusBreakdown(string policyNumber);

        decimal GetMaximumBonusRate(string policyType);

        decimal GetMinimumBonusRate(string policyType);

        bool ValidateBonusComputation(string policyNumber, decimal computedAmount);

        List<BonusComputationResult> GetBonusHistory(string policyNumber, DateTime fromDate, DateTime toDate);

        int GetBonusDeclarationYear(string policyNumber);

        decimal CalculateTotalAccruedBonus(decimal sumAssured, decimal bonusRate, int years);
    }
}
