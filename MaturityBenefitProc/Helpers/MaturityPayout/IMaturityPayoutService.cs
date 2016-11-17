using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.MaturityPayout
{
    public interface IMaturityPayoutService
    {
        MaturityPayoutResult ProcessMaturityPayout(string policyNumber, decimal totalAmount);

        MaturityPayoutResult ValidateMaturityPayout(string policyNumber);

        decimal CalculateMaturityAmount(decimal sumAssured, decimal accruedBonus, decimal terminalBonus, decimal loyaltyAddition);

        decimal CalculateNetPayableAmount(decimal grossAmount, decimal tdsAmount, decimal otherDeductions);

        MaturityPayoutResult GetPayoutDetails(string payoutReferenceId);

        decimal GetTotalDeductions(string policyNumber, decimal grossAmount);

        bool IsPayoutEligible(string policyNumber);

        MaturityPayoutResult ApproveMaturityPayout(string payoutReferenceId, string approvedBy);

        MaturityPayoutResult RejectMaturityPayout(string payoutReferenceId, string reason);

        decimal GetMaximumPayoutAmount();

        decimal GetMinimumPayoutAmount();

        bool ValidatePayoutAmount(decimal amount);

        List<MaturityPayoutResult> GetPayoutHistory(string policyNumber, DateTime fromDate, DateTime toDate);

        decimal CalculatePayoutTax(decimal amount, bool hasPanCard);

        MaturityPayoutResult SuspendPayout(string payoutReferenceId, string reason);
    }
}
