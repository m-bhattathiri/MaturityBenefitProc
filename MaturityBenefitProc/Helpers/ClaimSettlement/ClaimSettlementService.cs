using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.ClaimSettlement
{
    public class ClaimSettlementService : IClaimSettlementService
    {
        private readonly Dictionary<string, ClaimSettlementResult> _claims;
        private readonly Dictionary<string, string> _policyClaimants;
        private readonly Dictionary<string, bool> _dischargeVouchers;
        private readonly Dictionary<string, List<ClaimSettlementResult>> _historyByPolicy;

        public ClaimSettlementService()
        {
            _claims = new Dictionary<string, ClaimSettlementResult>();
            _policyClaimants = new Dictionary<string, string>
            {
                { "POL001", "CIF001" }, { "POL002", "CIF002" }, { "POL003", "CIF003" },
                { "POL004", "CIF004" }, { "POL005", "CIF005" }, { "POL006", "CIF006" },
                { "POL007", "CIF007" }, { "POL008", "CIF008" }, { "POL009", "CIF009" }
            };
            _dischargeVouchers = new Dictionary<string, bool>
            {
                { "CLM001", true }, { "CLM002", false }, { "CLM003", true },
                { "CLM004", false }, { "CLM005", true }
            };
            _historyByPolicy = new Dictionary<string, List<ClaimSettlementResult>>();
        }

        public ClaimSettlementResult InitiateClaimSettlement(string policyNumber, string claimantCif)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new ClaimSettlementResult { Success = false, Message = "Policy number is required" };
            if (string.IsNullOrWhiteSpace(claimantCif))
                return new ClaimSettlementResult { Success = false, Message = "Claimant CIF is required" };

            if (!IsClaimSettlementEligible(policyNumber, claimantCif))
                return new ClaimSettlementResult { Success = false, Message = "Not eligible for claim settlement" };

            string claimNumber = "CLM-" + policyNumber.GetHashCode().ToString("X8");
            var result = new ClaimSettlementResult
            {
                Success = true,
                Message = "Claim settlement initiated",
                ReferenceId = claimNumber,
                ClaimNumber = claimNumber,
                SettlementType = "Standard",
                ProcessedDate = DateTime.UtcNow
            };
            result.Metadata["PolicyNumber"] = policyNumber;
            result.Metadata["ClaimantCIF"] = claimantCif;

            _claims[claimNumber] = result;
            StoreHistory(policyNumber, result);
            return result;
        }

        public ClaimSettlementResult ValidateClaimSettlement(string claimNumber)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new ClaimSettlementResult { Success = false, Message = "Claim number is required" };

            if (_claims.ContainsKey(claimNumber))
                return new ClaimSettlementResult { Success = true, Message = "Claim validated", ReferenceId = claimNumber, ClaimNumber = claimNumber };

            return new ClaimSettlementResult { Success = true, Message = "Claim not found in records", ReferenceId = claimNumber };
        }

        public ClaimSettlementResult ApproveSettlement(string claimNumber, string approvedBy)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new ClaimSettlementResult { Success = false, Message = "Claim number is required" };
            if (string.IsNullOrWhiteSpace(approvedBy))
                return new ClaimSettlementResult { Success = false, Message = "Approver name is required" };

            return new ClaimSettlementResult
            {
                Success = true,
                Message = "Settlement approved",
                ReferenceId = claimNumber,
                ClaimNumber = claimNumber
            };
        }

        public ClaimSettlementResult RejectSettlement(string claimNumber, string reason)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new ClaimSettlementResult { Success = false, Message = "Claim number is required" };
            if (string.IsNullOrWhiteSpace(reason))
                return new ClaimSettlementResult { Success = false, Message = "Rejection reason is required" };

            return new ClaimSettlementResult
            {
                Success = true,
                Message = "Settlement rejected",
                ReferenceId = claimNumber,
                ClaimNumber = claimNumber
            };
        }

        public ClaimSettlementResult ProcessDischargeVoucher(string claimNumber, string signatoryName)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new ClaimSettlementResult { Success = false, Message = "Claim number is required" };
            if (string.IsNullOrWhiteSpace(signatoryName))
                return new ClaimSettlementResult { Success = false, Message = "Signatory name is required" };

            string voucherNumber = "DV-" + claimNumber.GetHashCode().ToString("X8");
            _dischargeVouchers[claimNumber] = true;

            return new ClaimSettlementResult
            {
                Success = true,
                Message = "Discharge voucher processed",
                ReferenceId = claimNumber,
                ClaimNumber = claimNumber,
                DischargeVoucherNumber = voucherNumber
            };
        }

        public bool IsClaimSettlementEligible(string policyNumber, string claimantCif)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || string.IsNullOrWhiteSpace(claimantCif))
                return false;

            if (_policyClaimants.ContainsKey(policyNumber))
                return _policyClaimants[policyNumber] == claimantCif;

            return false;
        }

        public decimal CalculateSettlementAmount(decimal sumAssured, decimal bonus, decimal deductions)
        {
            if (sumAssured <= 0)
                return 0m;

            decimal total = sumAssured + bonus - deductions;
            return total > 0 ? total : 0m;
        }

        public ClaimSettlementResult VerifyNomineeIdentity(string claimNumber, string nomineeId, string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new ClaimSettlementResult { Success = false, Message = "Claim number is required" };
            if (string.IsNullOrWhiteSpace(nomineeId))
                return new ClaimSettlementResult { Success = false, Message = "Nominee ID is required" };
            if (string.IsNullOrWhiteSpace(documentNumber))
                return new ClaimSettlementResult { Success = false, Message = "Document number is required" };

            return new ClaimSettlementResult
            {
                Success = true,
                Message = "Nominee identity verified",
                ReferenceId = claimNumber,
                ClaimNumber = claimNumber,
                ClaimantName = nomineeId
            };
        }

        public ClaimSettlementResult GetSettlementDetails(string claimNumber)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new ClaimSettlementResult { Success = false, Message = "Claim number is required" };

            if (_claims.ContainsKey(claimNumber))
            {
                var claim = _claims[claimNumber];
                return new ClaimSettlementResult
                {
                    Success = true,
                    Message = "Settlement details retrieved",
                    ReferenceId = claimNumber,
                    ClaimNumber = claim.ClaimNumber,
                    SettlementType = claim.SettlementType,
                    GrossAmount = claim.GrossAmount,
                    Deductions = claim.Deductions,
                    NetAmount = claim.NetAmount
                };
            }

            return new ClaimSettlementResult { Success = false, Message = "Settlement details not found" };
        }

        public decimal GetSettlementCharges(string settlementType)
        {
            if (string.IsNullOrWhiteSpace(settlementType))
                return 0m;

            switch (settlementType)
            {
                case "Express":
                    return 500m;
                case "Standard":
                    return 0m;
                case "Priority":
                    return 250m;
                default:
                    return 0m;
            }
        }

        public bool HasDischargeVoucher(string claimNumber)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return false;

            return _dischargeVouchers.ContainsKey(claimNumber) && _dischargeVouchers[claimNumber];
        }

        public ClaimSettlementResult EscalateSettlement(string claimNumber, string escalationReason)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new ClaimSettlementResult { Success = false, Message = "Claim number is required" };
            if (string.IsNullOrWhiteSpace(escalationReason))
                return new ClaimSettlementResult { Success = false, Message = "Escalation reason is required" };

            return new ClaimSettlementResult
            {
                Success = true,
                Message = "Settlement escalated",
                ReferenceId = claimNumber,
                ClaimNumber = claimNumber
            };
        }

        public List<ClaimSettlementResult> GetSettlementHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new List<ClaimSettlementResult>();

            if (!_historyByPolicy.ContainsKey(policyNumber))
                return new List<ClaimSettlementResult>();

            return _historyByPolicy[policyNumber]
                .Where(h => h.ProcessedDate >= fromDate && h.ProcessedDate <= toDate)
                .ToList();
        }

        public decimal GetMaximumSettlementAmount()
        {
            return 100000000m;
        }

        public ClaimSettlementResult SuspendSettlement(string claimNumber, string reason)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new ClaimSettlementResult { Success = false, Message = "Claim number is required" };
            if (string.IsNullOrWhiteSpace(reason))
                return new ClaimSettlementResult { Success = false, Message = "Suspension reason is required" };

            return new ClaimSettlementResult
            {
                Success = true,
                Message = "Settlement suspended",
                ReferenceId = claimNumber,
                ClaimNumber = claimNumber
            };
        }

        private void StoreHistory(string policyNumber, ClaimSettlementResult result)
        {
            if (!_historyByPolicy.ContainsKey(policyNumber))
                _historyByPolicy[policyNumber] = new List<ClaimSettlementResult>();
            _historyByPolicy[policyNumber].Add(result);
        }
    }
}
