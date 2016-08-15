using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans
{
    /// <summary>Calculates refunds of mortality charges for applicable ULIP products.</summary>
    public interface IMortalityChargeRefundService
    {
        decimal CalculateTotalRefundAmount(string policyId, DateTime calculationDate);
        
        bool IsPolicyEligibleForRefund(string policyId);
        
        decimal GetMonthlyMortalityCharge(string policyId, int policyYear, int policyMonth);
        
        double GetRefundPercentage(string productId, int policyTerm);
        
        int GetTotalMonthsCharged(string policyId);
        
        string GetRefundStatus(string policyId);
        
        decimal CalculateInterestOnRefund(decimal baseRefundAmount, double interestRate, int daysDelayed);
        
        bool ValidateFundValueSufficiency(string policyId, decimal requiredAmount);
        
        int GetApplicablePolicyTerm(string policyId);
        
        double GetMortalityRate(int attainedAge, string genderCode);
        
        decimal GetSumAtRisk(string policyId, DateTime valuationDate);
        
        string GenerateRefundTransactionId(string policyId, DateTime processingDate);
        
        bool HasPreviousRefundBeenProcessed(string policyId);
        
        decimal CalculateTaxOnRefund(decimal refundAmount, double taxRate);
        
        int GetDaysSinceMaturity(string policyId, DateTime currentDate);
        
        double GetPersistencyBonusRatio(string policyId);
        
        decimal GetTotalAccumulatedCharges(string policyId);
        
        bool VerifyRiderExclusions(string policyId, string riderCode);
    }
}