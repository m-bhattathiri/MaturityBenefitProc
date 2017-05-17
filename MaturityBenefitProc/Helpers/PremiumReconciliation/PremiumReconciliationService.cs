using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.PremiumReconciliation
{
    public class PremiumReconciliationService : IPremiumReconciliationService
    {
        public PremiumReconciliationResult ReconcilePremiumPayment(string policyNumber, decimal amount)
        {
            return new PremiumReconciliationResult { Success = false, Message = "Not implemented" };
        }

        public PremiumReconciliationResult ValidatePremiumReconciliation(string policyNumber)
        {
            return new PremiumReconciliationResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetTotalPremiumPaid(string policyNumber)
        {
            return 0m;
        }

        public decimal GetTotalPremiumDue(string policyNumber, DateTime asOfDate)
        {
            return 0m;
        }

        public int GetPremiumsPaidCount(string policyNumber)
        {
            return 0;
        }

        public int GetPremiumsDueCount(string policyNumber)
        {
            return 0;
        }

        public bool IsPolicyLapsed(string policyNumber, int gracePeriodDays)
        {
            return false;
        }

        public PremiumReconciliationResult ProcessPolicyRevival(string policyNumber, decimal arrearAmount)
        {
            return new PremiumReconciliationResult { Success = false, Message = "Not implemented" };
        }

        public decimal CalculateRevivalCharges(string policyNumber, int monthsLapsed)
        {
            return 0m;
        }

        public decimal GetPremiumArrears(string policyNumber, DateTime asOfDate)
        {
            return 0m;
        }

        public bool IsRevivalEligible(string policyNumber, int maxLapsedMonths)
        {
            return false;
        }

        public PremiumReconciliationResult GetPremiumStatement(string policyNumber)
        {
            return new PremiumReconciliationResult { Success = false, Message = "Not implemented" };
        }

        public List<PremiumReconciliationResult> GetPremiumPaymentHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<PremiumReconciliationResult>();
        }

        public decimal GetMaximumRevivalPeriod()
        {
            return 0m;
        }

        public decimal CalculatePaidUpValue(decimal sumAssured, int premiumsPaid, int totalDue)
        {
            return 0m;
        }
    }
}
