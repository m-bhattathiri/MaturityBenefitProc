using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.ClaimSettlement
{
    public interface IClaimSettlementService
    {
        ClaimSettlementResult InitiateClaimSettlement(string policyNumber, string claimantCif);

        ClaimSettlementResult ValidateClaimSettlement(string claimNumber);

        ClaimSettlementResult ApproveSettlement(string claimNumber, string approvedBy);

        ClaimSettlementResult RejectSettlement(string claimNumber, string reason);

        ClaimSettlementResult ProcessDischargeVoucher(string claimNumber, string signatoryName);

        bool IsClaimSettlementEligible(string policyNumber, string claimantCif);

        decimal CalculateSettlementAmount(decimal sumAssured, decimal bonus, decimal deductions);

        ClaimSettlementResult VerifyNomineeIdentity(string claimNumber, string nomineeId, string documentNumber);

        ClaimSettlementResult GetSettlementDetails(string claimNumber);

        decimal GetSettlementCharges(string settlementType);

        bool HasDischargeVoucher(string claimNumber);

        ClaimSettlementResult EscalateSettlement(string claimNumber, string escalationReason);

        List<ClaimSettlementResult> GetSettlementHistory(string policyNumber, DateTime fromDate, DateTime toDate);

        decimal GetMaximumSettlementAmount();

        ClaimSettlementResult SuspendSettlement(string claimNumber, string reason);
    }
}
