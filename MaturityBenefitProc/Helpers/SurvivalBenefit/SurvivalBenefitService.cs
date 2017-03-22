using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.SurvivalBenefit
{
    public class SurvivalBenefitService : ISurvivalBenefitService
    {
        public SurvivalBenefitResult ProcessSurvivalBenefit(string policyNumber, decimal amount)
        {
            return new SurvivalBenefitResult { Success = false, Message = "Not implemented" };
        }

        public SurvivalBenefitResult ValidateSurvivalBenefit(string policyNumber)
        {
            return new SurvivalBenefitResult { Success = false, Message = "Not implemented" };
        }

        public decimal CalculateSurvivalBenefitAmount(decimal sumAssured, decimal benefitPercentage, int installmentNumber)
        {
            return 0m;
        }

        public decimal GetSurvivalBenefitPercentage(string planCode, int installmentNumber)
        {
            return 0m;
        }

        public bool IsSurvivalBenefitDue(string policyNumber, DateTime checkDate)
        {
            return false;
        }

        public SurvivalBenefitResult GetNextSurvivalBenefitDue(string policyNumber)
        {
            return new SurvivalBenefitResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetTotalSurvivalBenefitsPaid(string policyNumber)
        {
            return 0m;
        }

        public int GetRemainingInstallments(string policyNumber)
        {
            return 0;
        }

        public SurvivalBenefitResult ApproveSurvivalBenefit(string referenceId, string approvedBy)
        {
            return new SurvivalBenefitResult { Success = false, Message = "Not implemented" };
        }

        public SurvivalBenefitResult RejectSurvivalBenefit(string referenceId, string reason)
        {
            return new SurvivalBenefitResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetMaximumSurvivalBenefitAmount()
        {
            return 0m;
        }

        public decimal GetMinimumSurvivalBenefitAmount()
        {
            return 0m;
        }

        public bool ValidateSurvivalBenefitAmount(decimal amount)
        {
            return false;
        }

        public List<SurvivalBenefitResult> GetSurvivalBenefitSchedule(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<SurvivalBenefitResult>();
        }

        public decimal CalculateSurvivalBenefitTax(decimal amount, bool hasPanCard)
        {
            return 0m;
        }
    }
}
