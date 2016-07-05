using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    /// <summary>
    /// Orchestrates the overall policy surrender workflow and state transitions.
    /// </summary>
    public interface IPolicySurrenderService
    {
        bool ValidatePolicyEligibility(string policyId, DateTime surrenderDate);
        decimal CalculateBaseSurrenderValue(string policyId, DateTime effectiveDate);
        decimal CalculateMarketValueAdjustment(string policyId, decimal baseValue, double currentMarketRate);
        decimal CalculateSurrenderCharge(string policyId, decimal baseValue, int yearsInForce);
        decimal CalculateTerminalBonus(string policyId, decimal baseValue);
        decimal CalculateUnearnedPremiumRefund(string policyId, DateTime surrenderDate);
        decimal CalculateOutstandingLoanBalance(string policyId, DateTime calculationDate);
        decimal CalculateLoanInterestAccrued(string policyId, DateTime calculationDate);
        decimal CalculateGrossSurrenderValue(string policyId, DateTime effectiveDate);
        decimal CalculateNetSurrenderValue(string policyId, DateTime effectiveDate);
        
        double GetCurrentSurrenderChargeRate(string policyId, int policyYear);
        double GetMarketValueAdjustmentFactor(string policyId, DateTime calculationDate);
        double GetTerminalBonusRate(string policyId, int yearsInForce);
        double GetTaxWithholdingRate(string policyId, string stateCode);
        double GetProratedPremiumFactor(string policyId, DateTime surrenderDate);
        
        bool IsPolicyInForce(string policyId);
        bool HasOutstandingLoans(string policyId);
        bool IsWithinFreeLookPeriod(string policyId, DateTime requestDate);
        bool RequiresSpousalConsent(string policyId, string stateCode);
        bool IsIrrevocableBeneficiaryPresent(string policyId);
        bool ValidateSignatureRequirements(string policyId, string documentId);
        bool CheckAntiMoneyLaunderingStatus(string policyId, decimal netSurrenderValue);
        bool IsVestingScheduleMet(string policyId, DateTime requestDate);
        
        int GetYearsInForce(string policyId, DateTime surrenderDate);
        int GetDaysToNextAnniversary(string policyId, DateTime currentDate);
        int GetRemainingSurrenderChargeYears(string policyId);
        int GetFreeLookDaysRemaining(string policyId, DateTime requestDate);
        int GetActiveLoanCount(string policyId);
        
        string InitiateSurrenderWorkflow(string policyId, string requestedBy);
        string GetSurrenderStatus(string workflowId);
        string GenerateSurrenderQuoteId(string policyId, DateTime quoteDate);
        string GetTaxFormRequirement(string policyId, decimal taxableAmount);
        string DeterminePaymentRoutingCode(string policyId, string bankId);
        string GetStateOfIssue(string policyId);
        string GetProductCode(string policyId);
        
        bool ApproveSurrenderRequest(string workflowId, string approverId);
        bool RejectSurrenderRequest(string workflowId, string reasonCode, string rejectedBy);
        bool SuspendSurrenderWorkflow(string workflowId, string reasonCode);
        bool ResumeSurrenderWorkflow(string workflowId);
        string FinalizeSurrenderTransaction(string workflowId, DateTime processingDate);
    }
}