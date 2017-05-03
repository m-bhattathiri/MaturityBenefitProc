using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.EndowmentPlan
{
    public class EndowmentPlanService : IEndowmentPlanService
    {
        public EndowmentPlanResult ProcessEndowmentMaturity(string policyNumber, decimal sumAssured)
        {
            return new EndowmentPlanResult { Success = false, Message = "Not implemented" };
        }

        public EndowmentPlanResult ValidateEndowmentMaturity(string policyNumber)
        {
            return new EndowmentPlanResult { Success = false, Message = "Not implemented" };
        }

        public decimal CalculatePaidUpValue(decimal sumAssured, int premiumsPaid, int totalPremiumsDue)
        {
            return 0m;
        }

        public decimal CalculateSurrenderValue(decimal paidUpValue, decimal accruedBonus, int completedYears)
        {
            return 0m;
        }

        public decimal GetGuaranteedSurrenderValue(string policyNumber, int completedYears)
        {
            return 0m;
        }

        public decimal GetSpecialSurrenderValue(string policyNumber, int completedYears)
        {
            return 0m;
        }

        public bool IsEligibleForFullMaturity(string policyNumber, int premiumsPaid, int totalDue)
        {
            return false;
        }

        public EndowmentPlanResult CalculateEndowmentBenefit(string policyNumber, decimal sumAssured, decimal bonus)
        {
            return new EndowmentPlanResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetEndowmentMaturityFactor(string planCode, int policyTerm)
        {
            return 0m;
        }

        public EndowmentPlanResult GetEndowmentPlanDetails(string policyNumber)
        {
            return new EndowmentPlanResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetMinimumPremiumsPaidForSurrender(string planCode)
        {
            return 0m;
        }

        public bool IsWithinGracePeriod(string policyNumber, DateTime checkDate)
        {
            return false;
        }

        public List<EndowmentPlanResult> GetEndowmentPlanHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<EndowmentPlanResult>();
        }

        public decimal CalculateReducedPaidUpAmount(decimal sumAssured, int premiumsPaid, int totalDue, decimal bonus)
        {
            return 0m;
        }

        public EndowmentPlanResult ReinstateEndowmentPolicy(string policyNumber, decimal arrearPremium)
        {
            return new EndowmentPlanResult { Success = false, Message = "Not implemented" };
        }
    }
}
