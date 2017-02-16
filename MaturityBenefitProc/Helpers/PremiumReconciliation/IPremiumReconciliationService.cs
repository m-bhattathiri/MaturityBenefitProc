using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PremiumReconciliation
{
    public interface IPremiumReconciliationService
    {
        PremiumReconciliationResult ReconcilePremiumPayment(string policyNumber, decimal amount);

        PremiumReconciliationResult ValidatePremiumReconciliation(string policyNumber);

        decimal GetTotalPremiumPaid(string policyNumber);

        decimal GetTotalPremiumDue(string policyNumber, DateTime asOfDate);

        int GetPremiumsPaidCount(string policyNumber);

        int GetPremiumsDueCount(string policyNumber);

        bool IsPolicyLapsed(string policyNumber, int gracePeriodDays);

        PremiumReconciliationResult ProcessPolicyRevival(string policyNumber, decimal arrearAmount);

        decimal CalculateRevivalCharges(string policyNumber, int monthsLapsed);

        decimal GetPremiumArrears(string policyNumber, DateTime asOfDate);

        bool IsRevivalEligible(string policyNumber, int maxLapsedMonths);

        PremiumReconciliationResult GetPremiumStatement(string policyNumber);

        List<PremiumReconciliationResult> GetPremiumPaymentHistory(string policyNumber, DateTime fromDate, DateTime toDate);

        decimal GetMaximumRevivalPeriod();

        decimal CalculatePaidUpValue(decimal sumAssured, int premiumsPaid, int totalDue);
    }
}
