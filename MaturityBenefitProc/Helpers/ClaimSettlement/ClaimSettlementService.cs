using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.ClaimSettlement
{
    public class ClaimSettlementService : IClaimSettlementService
    {
        public ClaimSettlementResult InitiateClaimSettlement(string policyNumber, string claimantCif)
        {
            return new ClaimSettlementResult { Success = false, Message = "Not implemented" };
        }

        public ClaimSettlementResult ValidateClaimSettlement(string claimNumber)
        {
            return new ClaimSettlementResult { Success = false, Message = "Not implemented" };
        }

        public ClaimSettlementResult ApproveSettlement(string claimNumber, string approvedBy)
        {
            return new ClaimSettlementResult { Success = false, Message = "Not implemented" };
        }

        public ClaimSettlementResult RejectSettlement(string claimNumber, string reason)
        {
            return new ClaimSettlementResult { Success = false, Message = "Not implemented" };
        }

        public ClaimSettlementResult ProcessDischargeVoucher(string claimNumber, string signatoryName)
        {
            return new ClaimSettlementResult { Success = false, Message = "Not implemented" };
        }

        public bool IsClaimSettlementEligible(string policyNumber, string claimantCif)
        {
            return false;
        }

        public decimal CalculateSettlementAmount(decimal sumAssured, decimal bonus, decimal deductions)
        {
            return 0m;
        }

        public ClaimSettlementResult VerifyNomineeIdentity(string claimNumber, string nomineeId, string documentNumber)
        {
            return new ClaimSettlementResult { Success = false, Message = "Not implemented" };
        }

        public ClaimSettlementResult GetSettlementDetails(string claimNumber)
        {
            return new ClaimSettlementResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetSettlementCharges(string settlementType)
        {
            return 0m;
        }

        public bool HasDischargeVoucher(string claimNumber)
        {
            return false;
        }

        public ClaimSettlementResult EscalateSettlement(string claimNumber, string escalationReason)
        {
            return new ClaimSettlementResult { Success = false, Message = "Not implemented" };
        }

        public List<ClaimSettlementResult> GetSettlementHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<ClaimSettlementResult>();
        }

        public decimal GetMaximumSettlementAmount()
        {
            return 0m;
        }

        public ClaimSettlementResult SuspendSettlement(string claimNumber, string reason)
        {
            return new ClaimSettlementResult { Success = false, Message = "Not implemented" };
        }
    }
}
