using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    /// <summary>Manages the final disbursement process specific to surrendered policies.</summary>
    public interface ISurrenderDisbursementService
    {
        decimal CalculateTotalSurrenderValue(string policyId, DateTime effectiveDate);
        
        decimal CalculatePenalties(string policyId, decimal baseValue);
        
        decimal GetOutstandingLoanBalance(string policyId);
        
        decimal CalculateTaxWithholding(decimal taxableAmount, double taxRate);
        
        decimal GetFinalDisbursementAmount(string policyId);
        
        decimal CalculateProratedDividends(string policyId, DateTime surrenderDate);
        
        decimal CalculateMarketValueAdjustment(string policyId, decimal currentFundValue);
        
        double GetSurrenderChargeRate(string policyId, int policyYears);
        
        double GetApplicableTaxRate(string stateCode);
        
        double CalculateVestingPercentage(string policyId, int monthsActive);
        
        double GetInterestRateForDelayedPayment(string policyId);
        
        bool IsEligibleForSurrender(string policyId);
        
        bool ValidateBankRoutingNumber(string routingNumber);
        
        bool RequiresSpousalConsent(string policyId, string stateCode);
        
        bool HasActiveGarnishments(string policyId);
        
        bool IsDisbursementApproved(string policyId, decimal amount);
        
        bool VerifyBeneficiarySignatures(string policyId, int requiredSignatures);
        
        int GetDaysUntilProcessingDeadline(DateTime requestDate);
        
        int GetActivePolicyMonths(string policyId);
        
        int CountPendingDisbursementHolds(string policyId);
        
        int GetGracePeriodDays(string policyId);
        
        string GenerateDisbursementTransactionId(string policyId, DateTime processingDate);
        
        string GetTaxFormTypeCode(decimal disbursementAmount, bool isQualifiedPlan);
        
        string GetPaymentMethodCode(string policyId);
        
        string DetermineDisbursementStatus(string transactionId);
    }
}