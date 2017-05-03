using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.MoneyBackPlan
{
    public class MoneyBackPlanService : IMoneyBackPlanService
    {
        public MoneyBackPlanResult ProcessMoneyBackPayout(string policyNumber, int installmentNumber)
        {
            return new MoneyBackPlanResult { Success = false, Message = "Not implemented" };
        }

        public MoneyBackPlanResult ValidateMoneyBackPlan(string policyNumber)
        {
            return new MoneyBackPlanResult { Success = false, Message = "Not implemented" };
        }

        public decimal CalculateMoneyBackAmount(decimal sumAssured, decimal payoutPercentage)
        {
            return 0m;
        }

        public decimal GetPayoutPercentage(string planCode, int installmentNumber)
        {
            return 0m;
        }

        public MoneyBackPlanResult GetPayoutSchedule(string policyNumber)
        {
            return new MoneyBackPlanResult { Success = false, Message = "Not implemented" };
        }

        public bool IsMoneyBackDue(string policyNumber, DateTime checkDate)
        {
            return false;
        }

        public int GetNextPayoutYear(string policyNumber)
        {
            return 0;
        }

        public decimal GetTotalMoneyBackPaid(string policyNumber)
        {
            return 0m;
        }

        public decimal GetRemainingMoneyBackPayable(string policyNumber)
        {
            return 0m;
        }

        public MoneyBackPlanResult ApproveMoneyBackPayout(string referenceId, string approvedBy)
        {
            return new MoneyBackPlanResult { Success = false, Message = "Not implemented" };
        }

        public MoneyBackPlanResult RejectMoneyBackPayout(string referenceId, string reason)
        {
            return new MoneyBackPlanResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetFinalInstallmentAmount(decimal sumAssured, decimal totalPaidOut, decimal accruedBonus)
        {
            return 0m;
        }

        public bool ValidateMoneyBackAmount(decimal amount)
        {
            return false;
        }

        public List<MoneyBackPlanResult> GetMoneyBackPayoutHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<MoneyBackPlanResult>();
        }

        public decimal CalculateMoneyBackTax(decimal amount, bool hasPanCard)
        {
            return 0m;
        }
    }
}
