using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    // Fixed implementation — correct business logic
    public class LegalHeirValidationService : ILegalHeirValidationService
    {
        private const int MINOR_AGE_THRESHOLD = 18;
        private const decimal DEFAULT_PROBATE_LIMIT = 50000m;

        public bool ValidateSuccessionCertificate(string certificateId, DateTime issueDate)
        {
            if (string.IsNullOrWhiteSpace(certificateId)) return false;
            
            // Certificate must be issued in the past, but not older than 10 years
            if (issueDate > DateTime.UtcNow || issueDate < DateTime.UtcNow.AddYears(-10))
            {
                return false;
            }

            return certificateId.StartsWith("SC-") && certificateId.Length >= 8;
        }

        public bool VerifyIndemnityBond(string bondId, decimal bondAmount)
        {
            if (string.IsNullOrWhiteSpace(bondId) || bondAmount <= 0) return false;
            
            // Bond ID must follow standard format
            return bondId.StartsWith("IND-") && bondAmount >= 1000m;
        }

        public int CalculateDaysSinceDeath(DateTime dateOfDeath, DateTime claimDate)
        {
            if (claimDate < dateOfDeath)
            {
                throw new ArgumentException("Claim date cannot be before date of death.");
            }

            return (claimDate.Date - dateOfDeath.Date).Days;
        }

        public decimal CalculateHeirShareAmount(decimal totalBenefitAmount, double heirSharePercentage)
        {
            if (totalBenefitAmount < 0) throw new ArgumentException("Benefit amount cannot be negative.");
            if (heirSharePercentage < 0 || heirSharePercentage > 100) throw new ArgumentException("Invalid percentage.");

            return Math.Round(totalBenefitAmount * (decimal)(heirSharePercentage / 100.0), 2);
        }

        public double GetStatutorySharePercentage(string relationshipCode, int totalHeirs)
        {
            if (totalHeirs <= 0) return 0.0;
            if (string.IsNullOrWhiteSpace(relationshipCode)) return 0.0;

            // Simplified statutory share logic
            switch (relationshipCode.ToUpperInvariant())
            {
                case "SPOUSE":
                    return 50.0; // Spouse gets 50%, rest divided among others
                case "CHILD":
                    return 50.0 / totalHeirs;
                case "PARENT":
                    return 25.0 / totalHeirs;
                default:
                    return 100.0 / totalHeirs;
            }
        }

        public string GenerateHeirReferenceId(string policyNumber, string nationalId)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || string.IsNullOrWhiteSpace(nationalId))
            {
                throw new ArgumentException("Policy number and National ID are required.");
            }

            string cleanPolicy = policyNumber.Replace("-", "").Trim();
            string cleanId = nationalId.Length > 4 ? nationalId.Substring(nationalId.Length - 4) : nationalId;
            
            return $"HR-{cleanPolicy}-{cleanId}-{DateTime.UtcNow:yyyyMMdd}";
        }

        public bool CheckMinorHeirStatus(DateTime dateOfBirth, DateTime claimDate)
        {
            if (dateOfBirth > claimDate) throw new ArgumentException("DOB cannot be after claim date.");

            int age = claimDate.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > claimDate.AddYears(-age)) age--;

            return age < MINOR_AGE_THRESHOLD;
        }

        public decimal CalculateGuardianshipBondAmount(decimal minorShareAmount, double riskFactor)
        {
            if (minorShareAmount <= 0) return 0m;
            
            double safeRiskFactor = Math.Max(1.0, Math.Min(riskFactor, 3.0)); // Cap risk factor between 1 and 3
            return Math.Round(minorShareAmount * (decimal)safeRiskFactor, 2);
        }

        public int GetRequiredAffidavitCount(string claimType, double totalBenefitValue)
        {
            if (string.IsNullOrWhiteSpace(claimType)) return 1;

            int count = 1;
            if (claimType.Equals("DISPUTED", StringComparison.OrdinalIgnoreCase)) count += 2;
            if (totalBenefitValue > 100000) count += 1;
            if (totalBenefitValue > 500000) count += 1;

            return count;
        }

        public string RetrieveCourtOrderCode(string courtName, DateTime orderDate)
        {
            if (string.IsNullOrWhiteSpace(courtName)) return "UNKNOWN-COURT";

            string prefix = courtName.Length >= 3 ? courtName.Substring(0, 3).ToUpperInvariant() : courtName.ToUpperInvariant();
            return $"CO-{prefix}-{orderDate:yyyyMMdd}";
        }

        public bool ValidateNotarySignature(string notaryId, DateTime notarizationDate)
        {
            if (string.IsNullOrWhiteSpace(notaryId)) return false;
            
            // Notarization cannot be in the future
            if (notarizationDate > DateTime.UtcNow) return false;

            return notaryId.Length >= 5 && char.IsLetter(notaryId[0]);
        }

        public double CalculateDisputedShareRatio(int disputingHeirs, double totalDisputedPercentage)
        {
            if (disputingHeirs <= 0 || totalDisputedPercentage <= 0) return 0.0;
            if (totalDisputedPercentage > 100.0) totalDisputedPercentage = 100.0;

            return Math.Round(totalDisputedPercentage / disputingHeirs, 4);
        }

        public decimal ComputeTaxWithholdingForHeir(decimal shareAmount, string taxCategoryCode)
        {
            if (shareAmount <= 0) return 0m;

            decimal taxRate;
            if (taxCategoryCode?.ToUpperInvariant() == "EXEMPT")
            {
                taxRate = 0.0m;
            }
            else if (taxCategoryCode?.ToUpperInvariant() == "STANDARD")
            {
                taxRate = 0.10m;
            }
            else if (taxCategoryCode?.ToUpperInvariant() == "FOREIGN")
            {
                taxRate = 0.30m;
            }
            else if (taxCategoryCode?.ToUpperInvariant() == "HIGH_NET")
            {
                taxRate = 0.20m;
            }
            else
            {
                taxRate = 0.15m; // Default fallback rate
            }

            return Math.Round(shareAmount * taxRate, 2);
        }

        public bool IsRelinquishmentDeedValid(string deedId, string relinquishingHeirId, string benefitingHeirId)
        {
            if (string.IsNullOrWhiteSpace(deedId) || 
                string.IsNullOrWhiteSpace(relinquishingHeirId) || 
                string.IsNullOrWhiteSpace(benefitingHeirId))
            {
                return false;
            }

            // Cannot relinquish to oneself
            if (relinquishingHeirId.Equals(benefitingHeirId, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return deedId.StartsWith("RD-");
        }

        public int GetPendingDocumentCount(string claimId, string heirId)
        {
            if (string.IsNullOrWhiteSpace(claimId) || string.IsNullOrWhiteSpace(heirId)) return -1;

            // Mock logic: In a real system, this would query a database.
            // Here we use a deterministic pseudo-random approach based on inputs.
            int hash = Math.Abs((claimId + heirId).GetHashCode());
            return hash % 5; // Returns 0 to 4 pending documents
        }

        public string DetermineNextActionCode(bool isDisputed, decimal totalClaimAmount)
        {
            if (isDisputed) return "LEGAL_REVIEW";
            if (totalClaimAmount > 1000000m) return "SENIOR_APPROVAL";
            if (totalClaimAmount > 250000m) return "STANDARD_REVIEW";
            
            return "AUTO_APPROVE";
        }

        public bool VerifyFamilyTreeDocument(string documentId, int declaredHeirCount)
        {
            if (string.IsNullOrWhiteSpace(documentId)) return false;
            if (declaredHeirCount <= 0) return false;

            return documentId.Contains("FT") && documentId.Length > 5;
        }

        public decimal GetMaximumAllowedWithoutProbate(string stateCode, DateTime dateOfDeath)
        {
            decimal baseLimit;
            if (stateCode?.ToUpperInvariant() == "CA")
            {
                baseLimit = 166250m;
            }
            else if (stateCode?.ToUpperInvariant() == "NY")
            {
                baseLimit = 30000m;
            }
            else if (stateCode?.ToUpperInvariant() == "TX")
            {
                baseLimit = 75000m;
            }
            else if (stateCode?.ToUpperInvariant() == "FL")
            {
                baseLimit = 75000m;
            }
            else
            {
                baseLimit = DEFAULT_PROBATE_LIMIT;
            }

            // Inflation adjustment mock logic based on year of death
            int yearsSince2000 = Math.Max(0, dateOfDeath.Year - 2000);
            decimal inflationMultiplier = 1.0m + (yearsSince2000 * 0.02m);

            return Math.Round(baseLimit * inflationMultiplier, 2);
        }
    }
}