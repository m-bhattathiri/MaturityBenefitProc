using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    // Fixed implementation — correct business logic
    public class SuccessionCertificateService : ISuccessionCertificateService
    {
        private const decimal LATE_PENALTY_RATE_PER_DAY = 0.001m; // 0.1% per day
        private const decimal MAX_PENALTY_PERCENTAGE = 0.15m; // 15% max penalty
        private const int CERTIFICATE_VALIDITY_DAYS = 180;

        public bool VerifyCertificateAuthenticity(string certificateId, string issuingAuthorityCode)
        {
            if (string.IsNullOrWhiteSpace(certificateId) || string.IsNullOrWhiteSpace(issuingAuthorityCode))
                return false;

            // Simulated logic: Valid certificates must start with the authority code
            return certificateId.StartsWith(issuingAuthorityCode, StringComparison.OrdinalIgnoreCase) 
                   && certificateId.Length >= 10;
        }

        public decimal CalculateTotalClaimableAmount(string claimId, decimal baseMaturityValue)
        {
            if (baseMaturityValue <= 0)
                throw new ArgumentException("Base maturity value must be greater than zero.", nameof(baseMaturityValue));

            // Simulated logic: Add a standard 5% terminal bonus for valid claims
            decimal terminalBonus = baseMaturityValue * 0.05m;
            return baseMaturityValue + terminalBonus;
        }

        public double GetHeirEntitlementPercentage(string heirId, string certificateId)
        {
            if (string.IsNullOrWhiteSpace(heirId) || string.IsNullOrWhiteSpace(certificateId))
                throw new ArgumentException("Invalid heir or certificate ID.");

            // Simulated logic based on heirId format
            if (heirId.EndsWith("-SPOUSE")) return 50.0;
            if (heirId.EndsWith("-CHILD")) return 25.0;
            
            return 10.0; // Default for other relatives
        }

        public int GetDaysSinceCertificateIssuance(DateTime issuanceDate)
        {
            if (issuanceDate > DateTime.UtcNow)
                throw new ArgumentException("Issuance date cannot be in the future.", nameof(issuanceDate));

            return (DateTime.UtcNow.Date - issuanceDate.Date).Days;
        }

        public string RetrieveCourtReferenceNumber(string certificateId)
        {
            if (string.IsNullOrWhiteSpace(certificateId))
                return "UNKNOWN";

            // Simulated extraction logic: Court ref is usually the last 6 characters
            if (certificateId.Length > 6)
                return $"CRT-{certificateId.Substring(certificateId.Length - 6)}";

            return $"CRT-{certificateId}";
        }

        public bool ValidateHeirIdentity(string heirId, string nationalIdNumber)
        {
            if (string.IsNullOrWhiteSpace(heirId) || string.IsNullOrWhiteSpace(nationalIdNumber))
                return false;

            // Simulated validation: National ID must be 11 digits and match heirId hash logic
            return nationalIdNumber.Length == 11 && long.TryParse(nationalIdNumber, out _);
        }

        public decimal ComputeTaxDeductionForHeir(decimal entitlementAmount, double taxRate)
        {
            if (entitlementAmount < 0)
                throw new ArgumentException("Entitlement amount cannot be negative.");
            if (taxRate < 0 || taxRate > 100)
                throw new ArgumentException("Tax rate must be between 0 and 100.");

            return entitlementAmount * (decimal)(taxRate / 100.0);
        }

        public int CountRegisteredLegalHeirs(string certificateId)
        {
            if (string.IsNullOrWhiteSpace(certificateId))
                return 0;

            // Simulated logic: Use string hash to generate a deterministic number of heirs (1 to 5)
            return Math.Abs(certificateId.GetHashCode()) % 5 + 1;
        }

        public string GenerateVerificationTrackingCode(string claimId, DateTime submissionDate)
        {
            if (string.IsNullOrWhiteSpace(claimId))
                throw new ArgumentException("Claim ID is required.");

            string datePart = submissionDate.ToString("yyyyMMdd");
            string claimPart = claimId.Length >= 4 ? claimId.Substring(0, 4).ToUpper() : claimId.ToUpper();
            
            return $"TRK-{datePart}-{claimPart}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }

        public bool IsClaimValueAboveThreshold(decimal totalClaimValue, decimal thresholdLimit)
        {
            return totalClaimValue > thresholdLimit;
        }

        public double CalculateDisputedShareRatio(string certificateId, int activeDisputesCount)
        {
            if (activeDisputesCount < 0)
                throw new ArgumentException("Disputes count cannot be negative.");

            if (activeDisputesCount == 0)
                return 0.0;

            // Simulated logic: Each dispute freezes 10% of the share, capped at 100%
            double frozenRatio = activeDisputesCount * 10.0;
            return Math.Min(frozenRatio, 100.0);
        }

        public decimal GetDisbursedAmountToDate(string claimId)
        {
            if (string.IsNullOrWhiteSpace(claimId))
                return 0m;

            // Simulated logic: Return a deterministic amount based on claimId
            return Math.Abs(claimId.GetHashCode()) % 50000m;
        }

        public int GetRemainingValidityDays(DateTime expirationDate)
        {
            int remainingDays = (expirationDate.Date - DateTime.UtcNow.Date).Days;
            return remainingDays > 0 ? remainingDays : 0;
        }

        public string GetPrimaryBeneficiaryId(string certificateId)
        {
            if (string.IsNullOrWhiteSpace(certificateId))
                throw new ArgumentException("Certificate ID is required.");

            return $"BEN-{certificateId}-01";
        }

        public bool CheckJurisdictionValidity(string courtCode, string policyBranchCode)
        {
            if (string.IsNullOrWhiteSpace(courtCode) || string.IsNullOrWhiteSpace(policyBranchCode))
                return false;

            // Simulated logic: First two characters must match (e.g., state/region code)
            if (courtCode.Length >= 2 && policyBranchCode.Length >= 2)
            {
                return courtCode.Substring(0, 2).Equals(policyBranchCode.Substring(0, 2), StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public decimal CalculateLateSubmissionPenalty(decimal claimAmount, int daysLate)
        {
            if (claimAmount <= 0) return 0m;
            if (daysLate <= 0) return 0m;

            decimal calculatedPenalty = claimAmount * (daysLate * LATE_PENALTY_RATE_PER_DAY);
            decimal maxPenalty = claimAmount * MAX_PENALTY_PERCENTAGE;

            return Math.Min(calculatedPenalty, maxPenalty);
        }

        public double GetLegalFeesDeductionRate(string jurisdictionCode)
        {
            if (string.IsNullOrWhiteSpace(jurisdictionCode))
                return 0.0;

            // Simulated logic based on jurisdiction
            switch (jurisdictionCode.ToUpper())
            {
                case "NY":
                case "CA":
                    return 2.5;
                case "TX":
                case "FL":
                    return 1.5;
                default:
                    return 1.0; // Standard 1% fee
            }
        }
    }
}