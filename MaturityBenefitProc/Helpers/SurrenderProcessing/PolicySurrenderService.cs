using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    // Buggy stub — returns incorrect values
    public class PolicySurrenderService : IPolicySurrenderService
    {
        public bool ValidatePolicyEligibility(string policyId, DateTime surrenderDate) => false;
        public decimal CalculateBaseSurrenderValue(string policyId, DateTime effectiveDate) => 0m;
        public decimal CalculateMarketValueAdjustment(string policyId, decimal baseValue, double currentMarketRate) => 0m;
        public decimal CalculateSurrenderCharge(string policyId, decimal baseValue, int yearsInForce) => 0m;
        public decimal CalculateTerminalBonus(string policyId, decimal baseValue) => 0m;
        public decimal CalculateUnearnedPremiumRefund(string policyId, DateTime surrenderDate) => 0m;
        public decimal CalculateOutstandingLoanBalance(string policyId, DateTime calculationDate) => 0m;
        public decimal CalculateLoanInterestAccrued(string policyId, DateTime calculationDate) => 0m;
        public decimal CalculateGrossSurrenderValue(string policyId, DateTime effectiveDate) => 0m;
        public decimal CalculateNetSurrenderValue(string policyId, DateTime effectiveDate) => 0m;
        
        public double GetCurrentSurrenderChargeRate(string policyId, int policyYear) => 0.0;
        public double GetMarketValueAdjustmentFactor(string policyId, DateTime calculationDate) => 0.0;
        public double GetTerminalBonusRate(string policyId, int yearsInForce) => 0.0;
        public double GetTaxWithholdingRate(string policyId, string stateCode) => 0.0;
        public double GetProratedPremiumFactor(string policyId, DateTime surrenderDate) => 0.0;
        
        public bool IsPolicyInForce(string policyId) => false;
        public bool HasOutstandingLoans(string policyId) => false;
        public bool IsWithinFreeLookPeriod(string policyId, DateTime requestDate) => false;
        public bool RequiresSpousalConsent(string policyId, string stateCode) => false;
        public bool IsIrrevocableBeneficiaryPresent(string policyId) => false;
        public bool ValidateSignatureRequirements(string policyId, string documentId) => false;
        public bool CheckAntiMoneyLaunderingStatus(string policyId, decimal netSurrenderValue) => false;
        public bool IsVestingScheduleMet(string policyId, DateTime requestDate) => false;
        
        public int GetYearsInForce(string policyId, DateTime surrenderDate) => 0;
        public int GetDaysToNextAnniversary(string policyId, DateTime currentDate) => 0;
        public int GetRemainingSurrenderChargeYears(string policyId) => 0;
        public int GetFreeLookDaysRemaining(string policyId, DateTime requestDate) => 0;
        public int GetActiveLoanCount(string policyId) => 0;
        
        public string InitiateSurrenderWorkflow(string policyId, string requestedBy) => null;
        public string GetSurrenderStatus(string workflowId) => null;
        public string GenerateSurrenderQuoteId(string policyId, DateTime quoteDate) => null;
        public string GetTaxFormRequirement(string policyId, decimal taxableAmount) => null;
        public string DeterminePaymentRoutingCode(string policyId, string bankId) => null;
        public string GetStateOfIssue(string policyId) => null;
        public string GetProductCode(string policyId) => null;
        
        public bool ApproveSurrenderRequest(string workflowId, string approverId) => false;
        public bool RejectSurrenderRequest(string workflowId, string reasonCode, string rejectedBy) => false;
        public bool SuspendSurrenderWorkflow(string workflowId, string reasonCode) => false;
        public bool ResumeSurrenderWorkflow(string workflowId) => false;
        public string FinalizeSurrenderTransaction(string workflowId, DateTime processingDate) => null;
    }
}