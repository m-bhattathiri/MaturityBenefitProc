using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    /// <summary>Tracks and validates annual life certificates required for continuous annuity payouts.</summary>
    public interface IAnnuityLifeCertificateService
    {
        bool ValidateCertificate(string certificateId, string annuitantId);
        
        string SubmitLifeCertificate(string annuitantId, DateTime submissionDate, string verificationMethod);
        
        int GetDaysUntilExpiration(string annuitantId);
        
        bool IsEligibleForAutoVerification(string annuitantId, int age);
        
        decimal CalculateSuspendedPayoutAmount(string annuitantId, DateTime suspensionDate);
        
        double GetVerificationConfidenceScore(string certificateId);
        
        string GetLatestCertificateId(string annuitantId);
        
        int CountPendingVerifications(string regionCode);
        
        bool RevokeCertificate(string certificateId, string reasonCode);
        
        decimal GetReinstatementArrears(string annuitantId, DateTime reinstatementDate);
        
        double CalculateComplianceRate(int year, string branchCode);
        
        string GenerateVerificationToken(string annuitantId, DateTime expiryDate);
        
        bool ExtendGracePeriod(string annuitantId, int additionalDays);
        
        int GetConsecutiveVerifiedYears(string annuitantId);
        
        decimal ApplyLateSubmissionPenalty(string annuitantId, decimal baseAnnuityAmount, double penaltyRate);
        
        bool RequiresPhysicalPresence(string annuitantId, int riskScore);
        
        string UpdateBiometricStatus(string certificateId, bool isMatch, double matchThreshold);
    }
}