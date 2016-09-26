using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    /// <summary>
    /// Places maturity payouts on hold in case of legal disputes or rival claims.
    /// </summary>
    public interface IDisputeResolutionService
    {
        bool InitiateDisputeHold(string policyId, string claimantId, string disputeReasonCode);
        string RegisterRivalClaim(string policyId, string primaryClaimantId, string rivalClaimantId);
        bool VerifyLegalInjunction(string injunctionId, string courtCode);
        decimal CalculateWithheldAmount(string policyId, decimal totalMaturityValue);
        int GetDisputeDurationDays(string disputeId);
        double CalculateEscrowInterestRate(string disputeId);
        decimal ComputeAccruedEscrowInterest(string disputeId, decimal withheldAmount, int daysHeld);
        bool ReleaseHold(string disputeId, string resolutionCode, string authorizedBy);
        string GenerateDisputeReferenceNumber(string policyId);
        int CountActiveDisputes(string policyId);
        bool ValidateLegalHeirCertificate(string certificateId, string issuingAuthority);
        decimal ApportionPayout(string policyId, string claimantId, double entitlementPercentage);
        bool IsPolicyUnderLitigation(string policyId);
        string GetLitigationStatus(string policyId);
        int GetPendingCourtHearingsCount(string disputeId);
        DateTime GetNextHearingDate(string disputeId);
        bool UpdateCourtOrderDetails(string disputeId, string orderReference, DateTime orderDate);
        double GetClaimantEntitlementRatio(string disputeId, string claimantId);
        decimal CalculateLegalFeesDeduction(string disputeId, decimal baseAmount);
        bool FlagForFraudInvestigation(string disputeId, string reasonCode);
        string GetFraudInvestigationStatus(string disputeId);
        int GetDaysSinceDisputeInitiation(string disputeId);
        bool EscalateToLegalDepartment(string disputeId, string escalationReason);
        decimal GetTotalDisputedAmount(string policyId);
        bool SettleDispute(string disputeId, string settlementAgreementId);
        string RecordMediationOutcome(string disputeId, string mediatorId, bool isResolved);
        double CalculateMediationSuccessProbability(string disputeType);
        bool CheckStatuteOfLimitations(string disputeId, DateTime claimDate);
        int GetStatuteOfLimitationsRemainingDays(string disputeId);
        decimal CalculatePartialReleaseAmount(string disputeId, string claimantId);
        bool AuthorizePartialRelease(string disputeId, string claimantId, decimal amount);
        string GetDisputeCategoryCode(string disputeId);
        bool RequireAdditionalDocumentation(string disputeId, string documentTypeCode);
        int GetMissingDocumentsCount(string disputeId);
        bool ValidateIndemnityBond(string bondId, decimal bondValue);
        decimal CalculateRequiredIndemnityValue(string policyId);
        string AssignLegalCounsel(string disputeId, string counselId);
        bool TerminateDispute(string disputeId, string terminationReasonCode);
    }
}