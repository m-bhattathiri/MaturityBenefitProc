using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.EndowmentPlan
{
    public interface IEndowmentPlanService
    {
        EndowmentPlanResult ProcessEndowmentMaturity(string policyNumber, decimal sumAssured);

        EndowmentPlanResult ValidateEndowmentMaturity(string policyNumber);

        decimal CalculatePaidUpValue(decimal sumAssured, int premiumsPaid, int totalPremiumsDue);

        decimal CalculateSurrenderValue(decimal paidUpValue, decimal accruedBonus, int completedYears);

        decimal GetGuaranteedSurrenderValue(string policyNumber, int completedYears);

        decimal GetSpecialSurrenderValue(string policyNumber, int completedYears);

        bool IsEligibleForFullMaturity(string policyNumber, int premiumsPaid, int totalDue);

        EndowmentPlanResult CalculateEndowmentBenefit(string policyNumber, decimal sumAssured, decimal bonus);

        decimal GetEndowmentMaturityFactor(string planCode, int policyTerm);

        EndowmentPlanResult GetEndowmentPlanDetails(string policyNumber);

        decimal GetMinimumPremiumsPaidForSurrender(string planCode);

        bool IsWithinGracePeriod(string policyNumber, DateTime checkDate);

        List<EndowmentPlanResult> GetEndowmentPlanHistory(string policyNumber, DateTime fromDate, DateTime toDate);

        decimal CalculateReducedPaidUpAmount(decimal sumAssured, int premiumsPaid, int totalDue, decimal bonus);

        EndowmentPlanResult ReinstateEndowmentPolicy(string policyNumber, decimal arrearPremium);
    }
}
