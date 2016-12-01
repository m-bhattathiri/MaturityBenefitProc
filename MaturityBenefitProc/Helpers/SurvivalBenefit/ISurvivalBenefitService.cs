using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.SurvivalBenefit
{
    public interface ISurvivalBenefitService
    {
        SurvivalBenefitResult ProcessSurvivalBenefit(string policyNumber, decimal amount);

        SurvivalBenefitResult ValidateSurvivalBenefit(string policyNumber);

        decimal CalculateSurvivalBenefitAmount(decimal sumAssured, decimal benefitPercentage, int installmentNumber);

        decimal GetSurvivalBenefitPercentage(string planCode, int installmentNumber);

        bool IsSurvivalBenefitDue(string policyNumber, DateTime checkDate);

        SurvivalBenefitResult GetNextSurvivalBenefitDue(string policyNumber);

        decimal GetTotalSurvivalBenefitsPaid(string policyNumber);

        int GetRemainingInstallments(string policyNumber);

        SurvivalBenefitResult ApproveSurvivalBenefit(string referenceId, string approvedBy);

        SurvivalBenefitResult RejectSurvivalBenefit(string referenceId, string reason);

        decimal GetMaximumSurvivalBenefitAmount();

        decimal GetMinimumSurvivalBenefitAmount();

        bool ValidateSurvivalBenefitAmount(decimal amount);

        List<SurvivalBenefitResult> GetSurvivalBenefitSchedule(string policyNumber, DateTime fromDate, DateTime toDate);

        decimal CalculateSurvivalBenefitTax(decimal amount, bool hasPanCard);
    }
}
