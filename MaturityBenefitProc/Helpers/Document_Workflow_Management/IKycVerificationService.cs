using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement
{
    /// <summary>Verifies PAN, Aadhar, and other KYC documents before maturity settlement.</summary>
    public interface IKycVerificationService
    {
        bool VerifyPanFormat(string panNumber);
        bool VerifyAadharChecksum(string aadharNumber);
        bool IsKycCompliant(string customerId);
        bool CheckNameMatch(string customerName, string documentName);
        bool IsAddressProofValid(string documentId, DateTime issueDate);
        bool ValidateSignature(string customerId, string signatureHash);
        bool IsCustomerMinor(string customerId, DateTime dateOfBirth);
        bool CheckPepStatus(string customerId);
        bool IsFatcaCompliant(string customerId);
        bool VerifyBankDetails(string accountNumber, string ifscCode);
        bool RequiresEnhancedDueDiligence(string customerId, decimal maturityAmount);
        
        decimal CalculateTotalMaturityAmount(string policyId, decimal baseAmount);
        decimal GetTdsDeductionAmount(string panNumber, decimal maturityAmount);
        decimal CalculatePenalInterest(string policyId, int daysDelayed);
        decimal GetSurrenderValue(string policyId, DateTime requestDate);
        decimal CalculateBonusAccrued(string policyId);
        decimal GetMaximumAllowableCashPayout(string customerId);
        decimal ComputeNetSettlementAmount(decimal grossAmount, decimal deductions);

        double GetTdsRate(string panNumber, bool isPanValid);
        double CalculateNameMatchConfidence(string name1, string name2);
        double GetRiskScore(string customerId);
        double GetFaceMatchPercentage(string photoId1, string photoId2);
        double GetDocumentClarityScore(string documentId);
        double GetCkycMatchProbability(string customerId, string ckycRecordId);

        int GetPendingKycDocumentCount(string customerId);
        int GetDaysUntilMaturity(string policyId, DateTime currentDate);
        int GetAadharLinkageStatusCode(string customerId);
        int CountFailedVerificationAttempts(string customerId);
        int GetPolicyDurationInMonths(string policyId);
        int GetAgeAtMaturity(string customerId, DateTime maturityDate);
        int GetDocumentExpiryDays(string documentId, DateTime currentDate);

        string GenerateKycReferenceNumber(string customerId);
        string GetCkycNumber(string panNumber);
        string RetrieveAadharVaultReference(string aadharNumber);
        string GetVerificationStatusDescription(int statusCode);
        string FetchPrimaryBankIfsc(string customerId);
        string GetRejectionReasonCode(string documentId);
        string GenerateSettlementTransactionId(string policyId, decimal amount);
        string GetTaxResidencyCountry(string customerId);
        string FetchDigiLockerDocumentUri(string documentType, string documentNumber);
    }
}