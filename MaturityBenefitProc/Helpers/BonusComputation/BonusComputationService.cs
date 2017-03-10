using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.BonusComputation
{
    public class BonusComputationService : IBonusComputationService
    {
        public BonusComputationResult ComputeSimpleReversionaryBonus(string policyNumber, decimal sumAssured, int policyYears)
        {
            return new BonusComputationResult { Success = false, Message = "Not implemented" };
        }

        public BonusComputationResult ComputeCompoundReversionaryBonus(string policyNumber, decimal sumAssured, decimal existingBonus, int policyYears)
        {
            return new BonusComputationResult { Success = false, Message = "Not implemented" };
        }

        public BonusComputationResult ComputeTerminalBonus(string policyNumber, decimal sumAssured, int completedYears, string policyType)
        {
            return new BonusComputationResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetBonusRate(string policyType, int policyYear)
        {
            return 0m;
        }

        public decimal CalculateLoyaltyAddition(decimal sumAssured, int completedYears, string policyType)
        {
            return 0m;
        }

        public decimal GetAccruedBonusAmount(string policyNumber)
        {
            return 0m;
        }

        public bool IsBonusEligible(string policyNumber, int minimumYears)
        {
            return false;
        }

        public decimal CalculateInterimBonus(decimal sumAssured, decimal bonusRate, int daysFromLastValuation)
        {
            return 0m;
        }

        public BonusComputationResult GetBonusBreakdown(string policyNumber)
        {
            return new BonusComputationResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetMaximumBonusRate(string policyType)
        {
            return 0m;
        }

        public decimal GetMinimumBonusRate(string policyType)
        {
            return 0m;
        }

        public bool ValidateBonusComputation(string policyNumber, decimal computedAmount)
        {
            return false;
        }

        public List<BonusComputationResult> GetBonusHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<BonusComputationResult>();
        }

        public int GetBonusDeclarationYear(string policyNumber)
        {
            return 0;
        }

        public decimal CalculateTotalAccruedBonus(decimal sumAssured, decimal bonusRate, int years)
        {
            return 0m;
        }
    }
}
