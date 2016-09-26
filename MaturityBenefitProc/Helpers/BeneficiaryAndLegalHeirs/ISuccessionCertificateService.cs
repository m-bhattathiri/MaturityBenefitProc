using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    /// <summary>Verifies succession certificates for high-value un-nominated claims.</summary>
    public interface ISuccessionCertificateService
    {
        bool VerifyCertificateAuthenticity(string certificateId, string issuingAuthorityCode);
        
        decimal CalculateTotalClaimableAmount(string claimId, decimal baseMaturityValue);
        
        double GetHeirEntitlementPercentage(string heirId, string certificateId);
        
        int GetDaysSinceCertificateIssuance(DateTime issuanceDate);
        
        string RetrieveCourtReferenceNumber(string certificateId);
        
        bool ValidateHeirIdentity(string heirId, string nationalIdNumber);
        
        decimal ComputeTaxDeductionForHeir(decimal entitlementAmount, double taxRate);
        
        int CountRegisteredLegalHeirs(string certificateId);
        
        string GenerateVerificationTrackingCode(string claimId, DateTime submissionDate);
        
        bool IsClaimValueAboveThreshold(decimal totalClaimValue, decimal thresholdLimit);
        
        double CalculateDisputedShareRatio(string certificateId, int activeDisputesCount);
        
        decimal GetDisbursedAmountToDate(string claimId);
        
        int GetRemainingValidityDays(DateTime expirationDate);
        
        string GetPrimaryBeneficiaryId(string certificateId);
        
        bool CheckJurisdictionValidity(string courtCode, string policyBranchCode);
        
        decimal CalculateLateSubmissionPenalty(decimal claimAmount, int daysLate);
        
        double GetLegalFeesDeductionRate(string jurisdictionCode);
    }
}