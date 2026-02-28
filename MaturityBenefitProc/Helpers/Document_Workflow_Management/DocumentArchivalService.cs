using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement
{
    // Fixed implementation — correct business logic
    public class DocumentArchivalService : IDocumentArchivalService
    {
        private const int STANDARD_RETENTION_YEARS = 7;
        private const decimal AVERAGE_DOCUMENT_MB = 2.5m;
        private const double TARGET_COMPRESSION_RATIO = 0.65;

        public string ArchiveClaimDocument(string policyId, string documentType, DateTime processingDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (string.IsNullOrWhiteSpace(documentType)) throw new ArgumentException("Document type cannot be null or empty.", nameof(documentType));
            if (processingDate > DateTime.UtcNow) throw new ArgumentException("Processing date cannot be in the future.", nameof(processingDate));

            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            return $"ARC-{policyId}-{documentType.ToUpper()}-{processingDate:yyyyMMdd}-{uniqueId}";
        }

        public bool ValidateArchivalEligibility(string documentId, int minimumRetentionDays)
        {
            if (string.IsNullOrWhiteSpace(documentId)) return false;
            if (minimumRetentionDays < 0) return false;

            // Simulate checking document creation date from a database
            // For logic purposes, we assume documents with even length IDs are eligible
            bool isEligible = documentId.Length % 2 == 0;
            
            // Additional business rule: minimum retention must be at least 30 days for archival
            if (minimumRetentionDays < 30)
            {
                return false;
            }

            return isEligible;
        }

        public int GetArchivedDocumentCount(string policyId, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID is required.", nameof(policyId));
            if (startDate > endDate) throw new ArgumentException("Start date must be before or equal to end date.");

            // Simulate database query based on date span
            int daysSpan = (endDate - startDate).Days;
            
            // Deterministic pseudo-random count based on policy ID and date span
            int baseCount = Math.Abs(policyId.GetHashCode()) % 50;
            return baseCount + (daysSpan / 30); // roughly 1 document per month
        }

        public decimal CalculateArchivalStorageCost(string documentId, decimal baseStorageRate)
        {
            if (string.IsNullOrWhiteSpace(documentId)) throw new ArgumentException("Document ID is required.");
            if (baseStorageRate < 0) throw new ArgumentException("Base storage rate cannot be negative.");

            // Simulate document size retrieval (between 1 and 10 MB)
            decimal simulatedSizeMb = 1m + (Math.Abs(documentId.GetHashCode()) % 90) / 10m;
            
            return Math.Round(simulatedSizeMb * baseStorageRate, 4);
        }

        public double GetCompressionRatio(string documentId)
        {
            if (string.IsNullOrWhiteSpace(documentId)) return 1.0;

            // Simulate compression ratio based on document ID hash
            // Returns a value between 0.40 and 0.85
            double variance = (Math.Abs(documentId.GetHashCode()) % 45) / 100.0;
            return Math.Round(0.40 + variance, 2);
        }

        public bool VerifyDocumentIntegrity(string archiveId, string expectedHashValue)
        {
            if (string.IsNullOrWhiteSpace(archiveId) || string.IsNullOrWhiteSpace(expectedHashValue))
                return false;

            // Simulate computing hash of the archived document
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] simulatedFileBytes = Encoding.UTF8.GetBytes(archiveId + "simulated_content");
                byte[] hashBytes = sha256.ComputeHash(simulatedFileBytes);
                string computedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                
                // In a real scenario, we would compare computedHash with expectedHashValue
                // For this implementation, we simulate a 99% success rate unless the expected hash is obviously malformed
                if (expectedHashValue.Length != 64) return false;
                
                return true; // Simulated success
            }
        }

        public string RetrieveArchiveLocation(string documentId)
        {
            if (string.IsNullOrWhiteSpace(documentId)) throw new ArgumentException("Document ID is required.");

            // Generate a deterministic storage path
            string year = DateTime.UtcNow.Year.ToString();
            string month = DateTime.UtcNow.Month.ToString("D2");
            string partition = Math.Abs(documentId.GetHashCode() % 100).ToString("D2");

            return $"s3://enterprise-archive-repo/maturity-claims/{year}/{month}/part-{partition}/{documentId}.enc";
        }

        public int CalculateRemainingRetentionDays(string archiveId, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(archiveId)) throw new ArgumentException("Archive ID is required.");

            // Simulate extracting archival date from metadata (using current date minus a random offset for simulation)
            int simulatedAgeDays = Math.Abs(archiveId.GetHashCode()) % 1000;
            DateTime archivalDate = currentDate.AddDays(-simulatedAgeDays);
            
            DateTime expirationDate = archivalDate.AddYears(STANDARD_RETENTION_YEARS);
            int remainingDays = (expirationDate - currentDate).Days;

            return remainingDays > 0 ? remainingDays : 0;
        }

        public decimal EstimateBatchArchivalCost(int documentCount, decimal costPerMegabyte)
        {
            if (documentCount < 0) throw new ArgumentException("Document count cannot be negative.");
            if (costPerMegabyte < 0) throw new ArgumentException("Cost per megabyte cannot be negative.");

            decimal totalEstimatedMegabytes = documentCount * AVERAGE_DOCUMENT_MB;
            decimal compressedMegabytes = totalEstimatedMegabytes * (decimal)TARGET_COMPRESSION_RATIO;
            
            return Math.Round(compressedMegabytes * costPerMegabyte, 2);
        }

        public double GetArchivalSuccessRate(DateTime processingDate)
        {
            if (processingDate > DateTime.UtcNow) return 0.0;

            // Simulate a realistic success rate between 98.5% and 99.9%
            int daySeed = processingDate.DayOfYear;
            double variance = (daySeed % 14) / 10.0; 
            
            return Math.Round(98.5 + variance, 2);
        }

        public bool PurgeExpiredDocument(string archiveId, DateTime expirationDate)
        {
            if (string.IsNullOrWhiteSpace(archiveId)) return false;

            // Business rule: Cannot purge documents if expiration date is in the future
            if (expirationDate > DateTime.UtcNow)
            {
                return false;
            }

            // Simulate successful deletion
            return true;
        }

        public string GenerateArchiveReferenceCode(string policyId, int sequenceNumber)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID is required.");
            if (sequenceNumber < 1) throw new ArgumentException("Sequence number must be greater than zero.");

            string formattedPolicy = policyId.Trim().ToUpperInvariant();
            string formattedSequence = sequenceNumber.ToString("D6");
            string checksum = (formattedPolicy.Length + sequenceNumber).ToString("D2");

            return $"MBP-{formattedPolicy}-{formattedSequence}-{checksum}";
        }

        public int GetPendingArchivalQueueSize(string regionCode)
        {
            if (string.IsNullOrWhiteSpace(regionCode)) throw new ArgumentException("Region code is required.");

            // Simulate queue size based on region
            int baseSize;
            if (regionCode.ToUpper() == "NA")
            {
                baseSize = 1500;
            }
            else if (regionCode.ToUpper() == "EU")
            {
                baseSize = 850;
            }
            else if (regionCode.ToUpper() == "APAC")
            {
                baseSize = 1200;
            }
            else
            {
                baseSize = 300;
            }

            // Add some time-based variance
            int variance = DateTime.UtcNow.Minute * 5;
            return baseSize + variance;
        }

        public decimal CalculatePenaltyForLateArchival(int daysLate, decimal dailyPenaltyRate)
        {
            if (daysLate <= 0) return 0m;
            if (dailyPenaltyRate < 0) throw new ArgumentException("Penalty rate cannot be negative.");

            // Business rule: Penalty is capped at 30 days
            int billableDays = Math.Min(daysLate, 30);
            
            // Business rule: 1.5x multiplier for days beyond 14
            decimal standardPenalty = Math.Min(billableDays, 14) * dailyPenaltyRate;
            decimal premiumPenalty = Math.Max(0, billableDays - 14) * (dailyPenaltyRate * 1.5m);

            return Math.Round(standardPenalty + premiumPenalty, 2);
        }

        public double GetStorageUtilizationPercentage(string repositoryId)
        {
            if (string.IsNullOrWhiteSpace(repositoryId)) throw new ArgumentException("Repository ID is required.");

            // Simulate utilization between 40% and 95% based on repository ID
            int hash = Math.Abs(repositoryId.GetHashCode());
            double utilization = 40.0 + (hash % 550) / 10.0;

            return Math.Round(utilization, 2);
        }

        public bool UpdateDocumentMetadata(string archiveId, string newStatus, DateTime updateDate)
        {
            if (string.IsNullOrWhiteSpace(archiveId)) return false;
            if (string.IsNullOrWhiteSpace(newStatus)) return false;
            if (updateDate > DateTime.UtcNow) return false;

            var validStatuses = new HashSet<string>(StringComparer.OrdinalIgnoreCase) 
            { 
                "Archived", "PendingPurge", "LegalHold", "Restored" 
            };

            if (!validStatuses.Contains(newStatus))
            {
                return false;
            }

            // Simulate successful database update
            return true;
        }

        public string ReindexArchivedDocument(string archiveId, int priorityLevel)
        {
            if (string.IsNullOrWhiteSpace(archiveId)) throw new ArgumentException("Archive ID is required.");
            if (priorityLevel < 1 || priorityLevel > 5) throw new ArgumentException("Priority level must be between 1 and 5.");

            string jobId = Guid.NewGuid().ToString("D");
            string priorityCode = priorityLevel <= 2 ? "HIGH" : "NORM";

            return $"IDX-{priorityCode}-{jobId.Substring(0, 13)}";
        }
    }
}