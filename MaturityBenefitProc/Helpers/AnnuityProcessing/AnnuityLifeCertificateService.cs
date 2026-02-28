// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    public class AnnuityLifeCertificateService : IAnnuityLifeCertificateService
    {
        // Mock data stores for business logic simulation
        private readonly Dictionary<string, CertificateRecord> _certificates = new Dictionary<string, CertificateRecord>();
        private readonly Dictionary<string, AnnuitantRecord> _annuitants = new Dictionary<string, AnnuitantRecord>();

        private const int STANDARD_VALIDITY_DAYS = 365;
        private const int MAX_GRACE_PERIOD_DAYS = 90;
        private const decimal DAILY_PAYOUT_RATE = 50.0m; // Simplified mock daily rate

        public bool ValidateCertificate(string certificateId, string annuitantId)
        {
            if (string.IsNullOrWhiteSpace(certificateId) || string.IsNullOrWhiteSpace(annuitantId))
                return false;

            if (_certificates.TryGetValue(certificateId, out var cert))
            {
                return cert.AnnuitantId == annuitantId && 
                       cert.Status == "Active" && 
                       cert.ExpiryDate > DateTime.UtcNow;
            }
            return false;
        }

        public string SubmitLifeCertificate(string annuitantId, DateTime submissionDate, string verificationMethod)
        {
            if (string.IsNullOrWhiteSpace(annuitantId))
                throw new ArgumentException("Annuitant ID cannot be null or empty.", nameof(annuitantId));

            if (submissionDate > DateTime.UtcNow)
                throw new ArgumentException("Submission date cannot be in the future.");

            string newCertId = $"CERT-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            
            _certificates[newCertId] = new CertificateRecord
            {
                CertificateId = newCertId,
                AnnuitantId = annuitantId,
                SubmissionDate = submissionDate,
                ExpiryDate = submissionDate.AddDays(STANDARD_VALIDITY_DAYS),
                VerificationMethod = verificationMethod,
                Status = "Pending"
            };

            // Update annuitant record
            if (!_annuitants.ContainsKey(annuitantId))
            {
                _annuitants[annuitantId] = new AnnuitantRecord { AnnuitantId = annuitantId };
            }
            _annuitants[annuitantId].LatestCertificateId = newCertId;

            return newCertId;
        }

        public int GetDaysUntilExpiration(string annuitantId)
        {
            string latestCertId = GetLatestCertificateId(annuitantId);
            if (latestCertId == null || !_certificates.TryGetValue(latestCertId, out var cert))
                return 0;

            var timeRemaining = cert.ExpiryDate - DateTime.UtcNow;
            return timeRemaining.Days > 0 ? timeRemaining.Days : 0;
        }

        public bool IsEligibleForAutoVerification(string annuitantId, int age)
        {
            if (string.IsNullOrWhiteSpace(annuitantId)) return false;

            // Business Rule: Annuitants over 85 require manual/physical verification
            if (age > 85) return false;

            // Business Rule: Must have at least 3 consecutive verified years for auto-verification
            return GetConsecutiveVerifiedYears(annuitantId) >= 3;
        }

        public decimal CalculateSuspendedPayoutAmount(string annuitantId, DateTime suspensionDate)
        {
            if (suspensionDate > DateTime.UtcNow) return 0m;

            int suspendedDays = (DateTime.UtcNow - suspensionDate).Days;
            return suspendedDays * DAILY_PAYOUT_RATE;
        }

        public double GetVerificationConfidenceScore(string certificateId)
        {
            if (!_certificates.TryGetValue(certificateId, out var cert))
                return 0.0;

            if (cert.VerificationMethod == "Biometric")
            {
                return 0.98;
            }
            else if (cert.VerificationMethod == "InPerson")
            {
                return 1.0;
            }
            else if (cert.VerificationMethod == "VideoCall")
            {
                return 0.85;
            }
            else if (cert.VerificationMethod == "DocumentUpload")
            {
                return 0.60;
            }
            else
            {
                return 0.10;
            }
        }

        public string GetLatestCertificateId(string annuitantId)
        {
            if (_annuitants.TryGetValue(annuitantId, out var annuitant))
            {
                return annuitant.LatestCertificateId;
            }
            return null;
        }

        public int CountPendingVerifications(string regionCode)
        {
            // In a real scenario, we'd filter by regionCode. 
            // Here we just count all pending for demonstration.
            return _certificates.Values.Count(c => c.Status == "Pending");
        }

        public bool RevokeCertificate(string certificateId, string reasonCode)
        {
            if (_certificates.TryGetValue(certificateId, out var cert))
            {
                cert.Status = "Revoked";
                cert.RevocationReason = reasonCode;
                return true;
            }
            return false;
        }

        public decimal GetReinstatementArrears(string annuitantId, DateTime reinstatementDate)
        {
            if (!_annuitants.TryGetValue(annuitantId, out var annuitant) || annuitant.SuspensionDate == null)
                return 0m;

            if (reinstatementDate < annuitant.SuspensionDate.Value)
                throw new ArgumentException("Reinstatement date cannot be before suspension date.");

            int daysInArrears = (reinstatementDate - annuitant.SuspensionDate.Value).Days;
            return daysInArrears * DAILY_PAYOUT_RATE;
        }

        public double CalculateComplianceRate(int year, string branchCode)
        {
            var yearCerts = _certificates.Values.Where(c => c.SubmissionDate.Year == year).ToList();
            if (!yearCerts.Any()) return 0.0;

            int compliant = yearCerts.Count(c => c.Status == "Active" || c.Status == "Verified");
            return Math.Round((double)compliant / yearCerts.Count * 100, 2);
        }

        public string GenerateVerificationToken(string annuitantId, DateTime expiryDate)
        {
            if (expiryDate <= DateTime.UtcNow)
                throw new ArgumentException("Expiry date must be in the future.");

            string rawToken = $"{annuitantId}:{expiryDate.Ticks}:{Guid.NewGuid()}";
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rawToken));
        }

        public bool ExtendGracePeriod(string annuitantId, int additionalDays)
        {
            if (additionalDays <= 0 || additionalDays > MAX_GRACE_PERIOD_DAYS)
                return false;

            string latestCertId = GetLatestCertificateId(annuitantId);
            if (latestCertId != null && _certificates.TryGetValue(latestCertId, out var cert))
            {
                cert.ExpiryDate = cert.ExpiryDate.AddDays(additionalDays);
                return true;
            }
            return false;
        }

        public int GetConsecutiveVerifiedYears(string annuitantId)
        {
            if (_annuitants.TryGetValue(annuitantId, out var annuitant))
            {
                return annuitant.ConsecutiveVerifiedYears;
            }
            return 0;
        }

        public decimal ApplyLateSubmissionPenalty(string annuitantId, decimal baseAnnuityAmount, double penaltyRate)
        {
            if (baseAnnuityAmount <= 0) return 0m;
            if (penaltyRate < 0 || penaltyRate > 1) throw new ArgumentOutOfRangeException(nameof(penaltyRate));

            decimal penaltyAmount = baseAnnuityAmount * (decimal)penaltyRate;
            return Math.Max(0, baseAnnuityAmount - penaltyAmount);
        }

        public bool RequiresPhysicalPresence(string annuitantId, int riskScore)
        {
            // Business Rule: High risk score (> 75) or previously flagged requires physical presence
            if (riskScore > 75) return true;

            if (_annuitants.TryGetValue(annuitantId, out var annuitant))
            {
                return annuitant.HasFraudHistory;
            }
            return false;
        }

        public string UpdateBiometricStatus(string certificateId, bool isMatch, double matchThreshold)
        {
            if (!_certificates.TryGetValue(certificateId, out var cert))
                return "CertificateNotFound";

            if (isMatch && matchThreshold >= 0.85)
            {
                cert.Status = "Active";
                return "Verified";
            }
            
            cert.Status = "FailedBiometric";
            return "ManualReviewRequired";
        }

        // Internal models for mock data
        private class CertificateRecord
        {
            public string CertificateId { get; set; }
            public string AnnuitantId { get; set; }
            public DateTime SubmissionDate { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string VerificationMethod { get; set; }
            public string Status { get; set; }
            public string RevocationReason { get; set; }
        }

        private class AnnuitantRecord
        {
            public string AnnuitantId { get; set; }
            public string LatestCertificateId { get; set; }
            public int ConsecutiveVerifiedYears { get; set; } = 3; // Default mock value
            public DateTime? SuspensionDate { get; set; }
            public bool HasFraudHistory { get; set; }
        }
    }
}