using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.MoneyBackPlan
{
    public interface IMoneyBackPlanService
    {
        MoneyBackPlanResult ProcessMoneyBackPayout(string policyNumber, int installmentNumber);

        MoneyBackPlanResult ValidateMoneyBackPlan(string policyNumber);

        decimal CalculateMoneyBackAmount(decimal sumAssured, decimal payoutPercentage);

        decimal GetPayoutPercentage(string planCode, int installmentNumber);

        MoneyBackPlanResult GetPayoutSchedule(string policyNumber);

        bool IsMoneyBackDue(string policyNumber, DateTime checkDate);

        int GetNextPayoutYear(string policyNumber);

        decimal GetTotalMoneyBackPaid(string policyNumber);

        decimal GetRemainingMoneyBackPayable(string policyNumber);

        MoneyBackPlanResult ApproveMoneyBackPayout(string referenceId, string approvedBy);

        MoneyBackPlanResult RejectMoneyBackPayout(string referenceId, string reason);

        decimal GetFinalInstallmentAmount(decimal sumAssured, decimal totalPaidOut, decimal accruedBonus);

        bool ValidateMoneyBackAmount(decimal amount);

        List<MoneyBackPlanResult> GetMoneyBackPayoutHistory(string policyNumber, DateTime fromDate, DateTime toDate);

        decimal CalculateMoneyBackTax(decimal amount, bool hasPanCard);
    }
}
