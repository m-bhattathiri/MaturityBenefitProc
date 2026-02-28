using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    // Fixed implementation — correct business logic
    public class DisputeResolutionService : IDisputeResolutionService
    {
        private readonly Dictionary<string, DateTime> _disputeStartDates = new Dictionary<string, DateTime>();
        private readonly Dictionary<string, string> _disputeStatuses = new Dictionary<string, string>();
        private readonly HashSet<string> _litigationPolicies = new HashSet<string>();
        private readonly Dictionary<string, decimal> _withheldAmounts = new Dictionary<string, decimal>();

        private const double BaseEscrowRate = 0.045; // 4.5%
        private const int StatuteOfLimitationsDays = 1095; // 3 years

        public bool InitiateDisputeHold(string policyId, string claimantId, string disputeReasonCode)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(claimantId))
                return false;

            string disputeId = GenerateDisputeReferenceNumber(policyId);
            _disputeStartDates[disputeId] = DateTime.UtcNow;
            _disputeStatuses[disputeId] = "ACTIVE";
            return true;
        }

        public string RegisterRivalClaim(string policyId, string primaryClaimantId, string rivalClaimantId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || primaryClaimantId == rivalClaimantId)
                throw new ArgumentException("Invalid claim parameters.");

            string disputeId = GenerateDisputeReferenceNumber(policyId);
            _disputeStartDates[disputeId] = DateTime.UtcNow;
            _disputeStatuses[disputeId] = "RIVAL_CLAIM_PENDING";
            return disputeId;
        }

        public bool VerifyLegalInjunction(string injunctionId, string courtCode)
        {
            if (string.IsNullOrWhiteSpace(injunctionId) || string.IsNullOrWhiteSpace(courtCode))
                return false;

            return injunctionId.StartsWith("INJ-") && courtCode.Length >= 3;
        }

        public decimal CalculateWithheldAmount(string policyId, decimal totalMaturityValue)
        {
            if (totalMaturityValue <= 0) return 0m;
            
            // If under litigation, withhold 100%. Otherwise withhold 50% for standard disputes.
            decimal withheld = IsPolicyUnderLitigation(policyId) ? totalMaturityValue : totalMaturityValue * 0.5m;
            _withheldAmounts[policyId] = withheld;
            return withheld;
        }

        public int GetDisputeDurationDays(string disputeId)
        {
            if (_disputeStartDates.TryGetValue(disputeId, out DateTime startDate))
            {
                return (int)(DateTime.UtcNow - startDate).TotalDays;
            }
            return 0;
        }

        public double CalculateEscrowInterestRate(string disputeId)
        {
            int duration = GetDisputeDurationDays(disputeId);
            // Add 0.5% for every year the dispute is active
            double premium = (duration / 365) * 0.005;
            return BaseEscrowRate + premium;
        }

        public decimal ComputeAccruedEscrowInterest(string disputeId, decimal withheldAmount, int daysHeld)
        {
            if (withheldAmount <= 0 || daysHeld <= 0) return 0m;

            double annualRate = CalculateEscrowInterestRate(disputeId);
            double dailyRate = annualRate / 365.0;
            
            return withheldAmount * (decimal)(dailyRate * daysHeld);
        }

        public bool ReleaseHold(string disputeId, string resolutionCode, string authorizedBy)
        {
            if (!_disputeStatuses.ContainsKey(disputeId) || string.IsNullOrWhiteSpace(authorizedBy))
                return false;

            _disputeStatuses[disputeId] = $"RESOLVED_{resolutionCode}";
            return true;
        }

        public string GenerateDisputeReferenceNumber(string policyId)
        {
            return $"DSP-{policyId}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }

        public int CountActiveDisputes(string policyId)
        {
            int count = 0;
            foreach (var kvp in _disputeStatuses)
            {
                if (kvp.Key.Contains(policyId) && kvp.Value == "ACTIVE")
                    count++;
            }
            return count;
        }

        public bool ValidateLegalHeirCertificate(string certificateId, string issuingAuthority)
        {
            if (string.IsNullOrWhiteSpace(certificateId) || string.IsNullOrWhiteSpace(issuingAuthority))
                return false;

            return certificateId.Length > 5 && issuingAuthority.IndexOf("Court", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public decimal ApportionPayout(string policyId, string claimantId, double entitlementPercentage)
        {
            if (entitlementPercentage < 0 || entitlementPercentage > 100)
                throw new ArgumentOutOfRangeException(nameof(entitlementPercentage));

            if (_withheldAmounts.TryGetValue(policyId, out decimal totalAmount))
            {
                return totalAmount * (decimal)(entitlementPercentage / 100.0);
            }
            return 0m;
        }

        public bool IsPolicyUnderLitigation(string policyId)
        {
            return _litigationPolicies.Contains(policyId);
        }

        public string GetLitigationStatus(string policyId)
        {
            return IsPolicyUnderLitigation(policyId) ? "UNDER_LITIGATION" : "CLEAR";
        }

        public int GetPendingCourtHearingsCount(string disputeId)
        {
            return _disputeStatuses.ContainsKey(disputeId) ? 1 : 0; // Mock logic
        }

        public DateTime GetNextHearingDate(string disputeId)
        {
            if (!_disputeStatuses.ContainsKey(disputeId)) return DateTime.MinValue;
            return DateTime.UtcNow.AddDays(30); // Mock next hearing in 30 days
        }

        public bool UpdateCourtOrderDetails(string disputeId, string orderReference, DateTime orderDate)
        {
            if (string.IsNullOrWhiteSpace(orderReference) || orderDate > DateTime.UtcNow)
                return false;

            _disputeStatuses[disputeId] = "COURT_ORDER_RECEIVED";
            return true;
        }

        public double GetClaimantEntitlementRatio(string disputeId, string claimantId)
        {
            return 0.5; // Mock 50/50 split
        }

        public decimal CalculateLegalFeesDeduction(string disputeId, decimal baseAmount)
        {
            if (baseAmount <= 0) return 0m;
            // Deduct 2% for legal administrative fees, capped at 5000
            decimal fee = baseAmount * 0.02m;
            return fee > 5000m ? 5000m : fee;
        }

        public bool FlagForFraudInvestigation(string disputeId, string reasonCode)
        {
            if (string.IsNullOrWhiteSpace(disputeId)) return false;
            _disputeStatuses[disputeId] = $"FRAUD_INVESTIGATION_{reasonCode}";
            return true;
        }

        public string GetFraudInvestigationStatus(string disputeId)
        {
            if (_disputeStatuses.TryGetValue(disputeId, out string status) && status.StartsWith("FRAUD"))
                return "INVESTIGATION_ONGOING";
            return "NONE";
        }

        public int GetDaysSinceDisputeInitiation(string disputeId)
        {
            return GetDisputeDurationDays(disputeId);
        }

        public bool EscalateToLegalDepartment(string disputeId, string escalationReason)
        {
            if (string.IsNullOrWhiteSpace(escalationReason)) return false;
            _disputeStatuses[disputeId] = "ESCALATED_TO_LEGAL";
            return true;
        }

        public decimal GetTotalDisputedAmount(string policyId)
        {
            return _withheldAmounts.TryGetValue(policyId, out decimal amount) ? amount : 0m;
        }

        public bool SettleDispute(string disputeId, string settlementAgreementId)
        {
            if (string.IsNullOrWhiteSpace(settlementAgreementId)) return false;
            _disputeStatuses[disputeId] = "SETTLED";
            return true;
        }

        public string RecordMediationOutcome(string disputeId, string mediatorId, bool isResolved)
        {
            string outcome = isResolved ? "MEDIATION_SUCCESS" : "MEDIATION_FAILED";
            _disputeStatuses[disputeId] = outcome;
            return outcome;
        }

        public double CalculateMediationSuccessProbability(string disputeType)
        {
            return disputeType.Equals("FAMILY_DISPUTE", StringComparison.OrdinalIgnoreCase) ? 0.65 : 0.30;
        }

        public bool CheckStatuteOfLimitations(string disputeId, DateTime claimDate)
        {
            return (DateTime.UtcNow - claimDate).TotalDays <= StatuteOfLimitationsDays;
        }

        public int GetStatuteOfLimitationsRemainingDays(string disputeId)
        {
            if (_disputeStartDates.TryGetValue(disputeId, out DateTime startDate))
            {
                int daysPassed = (int)(DateTime.UtcNow - startDate).TotalDays;
                return Math.Max(0, StatuteOfLimitationsDays - daysPassed);
            }
            return StatuteOfLimitationsDays;
        }

        public decimal CalculatePartialReleaseAmount(string disputeId, string claimantId)
        {
            return 10000m; // Mock safe harbor amount
        }

        public bool AuthorizePartialRelease(string disputeId, string claimantId, decimal amount)
        {
            if (amount <= 0) return false;
            return _disputeStatuses.ContainsKey(disputeId);
        }

        public string GetDisputeCategoryCode(string disputeId)
        {
            return "CAT-LGL-01"; // Mock category
        }

        public bool RequireAdditionalDocumentation(string disputeId, string documentTypeCode)
        {
            if (string.IsNullOrWhiteSpace(documentTypeCode)) return false;
            _disputeStatuses[disputeId] = "PENDING_DOCS";
            return true;
        }

        public int GetMissingDocumentsCount(string disputeId)
        {
            return _disputeStatuses.TryGetValue(disputeId, out string status) && status == "PENDING_DOCS" ? 2 : 0;
        }

        public bool ValidateIndemnityBond(string bondId, decimal bondValue)
        {
            return !string.IsNullOrWhiteSpace(bondId) && bondValue > 0;
        }

        public decimal CalculateRequiredIndemnityValue(string policyId)
        {
            decimal disputedAmount = GetTotalDisputedAmount(policyId);
            // Require 120% of the disputed amount as indemnity
            return disputedAmount * 1.2m;
        }

        public string AssignLegalCounsel(string disputeId, string counselId)
        {
            if (string.IsNullOrWhiteSpace(counselId)) throw new ArgumentNullException(nameof(counselId));
            return $"COUNSEL_{counselId}_ASSIGNED";
        }

        public bool TerminateDispute(string disputeId, string terminationReasonCode)
        {
            if (!_disputeStatuses.ContainsKey(disputeId)) return false;
            _disputeStatuses[disputeId] = $"TERMINATED_{terminationReasonCode}";
            return true;
        }
    }
}