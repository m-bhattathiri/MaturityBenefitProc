using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.MaturityPayout
{
    public class MaturityPayoutService : IMaturityPayoutService
    {
        public MaturityPayoutResult ProcessMaturityPayout(string policyNumber, decimal totalAmount)
        {
            return new MaturityPayoutResult { Success = false, Message = "Not implemented" };
        }

        public MaturityPayoutResult ValidateMaturityPayout(string policyNumber)
        {
            return new MaturityPayoutResult { Success = false, Message = "Not implemented" };
        }

        public decimal CalculateMaturityAmount(decimal sumAssured, decimal accruedBonus, decimal terminalBonus, decimal loyaltyAddition)
        {
            return 0m;
        }

        public decimal CalculateNetPayableAmount(decimal grossAmount, decimal tdsAmount, decimal otherDeductions)
        {
            return 0m;
        }

        public MaturityPayoutResult GetPayoutDetails(string payoutReferenceId)
        {
            return new MaturityPayoutResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetTotalDeductions(string policyNumber, decimal grossAmount)
        {
            return 0m;
        }

        public bool IsPayoutEligible(string policyNumber)
        {
            return false;
        }

        public MaturityPayoutResult ApproveMaturityPayout(string payoutReferenceId, string approvedBy)
        {
            return new MaturityPayoutResult { Success = false, Message = "Not implemented" };
        }

        public MaturityPayoutResult RejectMaturityPayout(string payoutReferenceId, string reason)
        {
            return new MaturityPayoutResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetMaximumPayoutAmount()
        {
            return 0m;
        }

        public decimal GetMinimumPayoutAmount()
        {
            return 0m;
        }

        public bool ValidatePayoutAmount(decimal amount)
        {
            return false;
        }

        public List<MaturityPayoutResult> GetPayoutHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<MaturityPayoutResult>();
        }

        public decimal CalculatePayoutTax(decimal amount, bool hasPanCard)
        {
            return 0m;
        }

        public MaturityPayoutResult SuspendPayout(string payoutReferenceId, string reason)
        {
            return new MaturityPayoutResult { Success = false, Message = "Not implemented" };
        }
    }
}
