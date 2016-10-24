using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.Document_Workflow_Management
{
    /// <summary>
    /// Validates NACH and e-mandate records for direct account crediting.
    /// </summary>
    public interface IBankMandateService
    {
        bool ValidateNachMandate(string mandateId, string bankAccountNumber);
        bool VerifyEMandateStatus(string eMandateId, DateTime verificationDate);
        string RegisterNewMandate(string customerId, string bankCode, string accountNumber);
        decimal CalculateMaxDebitAmount(string mandateId, decimal requestedAmount);
        double GetMandateSuccessRate(string bankCode, DateTime startDate, DateTime endDate);
        int GetRemainingValidityDays(string mandateId);
        bool IsAccountEligibleForDirectCredit(string accountNumber, string ifscCode);
        string UpdateMandateStatus(string mandateId, string newStatusCode);
        int CountActiveMandatesForCustomer(string customerId);
        decimal GetTotalCreditedAmount(string mandateId, DateTime financialYearStart);
        bool CheckMandateLimitExceeded(string mandateId, decimal transactionAmount);
        string RetrieveBankIfscFromMandate(string mandateId);
        double CalculateRejectionRatio(string bankCode, int month, int year);
        bool CancelMandate(string mandateId, string reasonCode);
        int GetPendingMandateAuthorizations(string branchCode);
        decimal ComputeMandateProcessingFee(string mandateType, bool isPriority);
        string GenerateMandateReferenceNumber(string customerId, DateTime requestDate);
        bool ValidateCustomerSignature(string mandateId, string signatureHash);
    }
}