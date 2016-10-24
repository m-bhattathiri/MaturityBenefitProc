using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement
{
    /// <summary>Generates and processes pre-filled maturity discharge vouchers.</summary>
    public interface IMaturityDischargeFormService
    {
        string GenerateDischargeVoucher(string policyNumber, DateTime maturityDate);
        bool ValidateVoucherSignatures(string voucherId, int signatureCount);
        string ProcessReturnedVoucher(string voucherId, DateTime receivedDate);
        decimal CalculateGrossMaturityValue(string policyNumber);
        decimal CalculateTerminalBonus(string policyNumber, decimal baseAmount);
        decimal CalculateOutstandingLoanDeduction(string policyNumber, DateTime calculationDate);
        decimal CalculateNetPayableAmount(string policyNumber, decimal grossAmount, decimal deductions);
        decimal GetPenalInterestAmount(string policyNumber, int delayedDays);
        double GetBonusRate(string policyNumber, int policyTerm);
        double CalculateTaxDeductionPercentage(string customerId, bool isPanProvided);
        double GetLoanInterestRate(string policyNumber);
        bool IsPolicyEligibleForMaturity(string policyNumber, DateTime currentDate);
        bool VerifyCustomerBankDetails(string customerId, string accountNumber);
        bool CheckNeftMandateStatus(string policyNumber);
        bool IsDischargeFormPrinted(string voucherId);
        bool ValidateWitnessDetails(string voucherId, string witnessId);
        int GetRemainingDaysToMaturity(string policyNumber, DateTime currentDate);
        int CountPendingRequirements(string voucherId);
        int GetPolicyTermInYears(string policyNumber);
        int GetDelayedProcessingDays(string voucherId, DateTime maturityDate);
        string GetVoucherStatus(string voucherId);
        string RetrieveDocumentReferenceNumber(string policyNumber);
        string GetTaxExemptionCode(string customerId);
        string AssignProcessingUser(string voucherId, int workloadCount);
        string GenerateNeftReference(string policyNumber, decimal amount);
    }
}