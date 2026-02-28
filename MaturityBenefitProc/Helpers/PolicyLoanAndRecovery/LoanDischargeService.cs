using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    // Fixed implementation — correct business logic
    public class LoanDischargeService : ILoanDischargeService
    {
        private const decimal MINIMUM_MATURITY_THRESHOLD = 100.00m;
        private const int DEFAULT_GRACE_PERIOD = 30;

        public string GenerateDischargeLetter(string policyId, string loanId, DateTime dischargeDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be empty.", nameof(policyId));
            if (string.IsNullOrWhiteSpace(loanId)) throw new ArgumentException("Loan ID cannot be empty.", nameof(loanId));

            string referenceNumber = $"DIS-{DateTime.UtcNow:yyyyMMdd}-{loanId.Substring(Math.Max(0, loanId.Length - 4))}";
            
            return $"DISCHARGE LETTER\n" +
                   $"Ref: {referenceNumber}\n" +
                   $"Date: {dischargeDate:yyyy-MM-dd}\n" +
                   $"Policy: {policyId}\n" +
                   $"Loan: {loanId}\n" +
                   $"This document certifies that the aforementioned loan has been fully discharged from the maturity benefits.";
        }

        public bool ValidateLoanClosureEligibility(string loanId, decimal maturityAmount)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return false;
            
            // Business rule: Maturity amount must be above a minimum threshold to process an automated closure
            if (maturityAmount < MINIMUM_MATURITY_THRESHOLD)
            {
                return false;
            }

            // In a real scenario, we would check the database for loan status.
            // Here we simulate eligibility based on valid inputs.
            return true;
        }

        public decimal CalculateFinalSettlementAmount(decimal outstandingPrincipal, decimal accruedInterest, decimal penaltyFees)
        {
            if (outstandingPrincipal < 0) throw new ArgumentOutOfRangeException(nameof(outstandingPrincipal), "Principal cannot be negative.");
            if (accruedInterest < 0) throw new ArgumentOutOfRangeException(nameof(accruedInterest), "Interest cannot be negative.");
            if (penaltyFees < 0) throw new ArgumentOutOfRangeException(nameof(penaltyFees), "Penalty fees cannot be negative.");

            return outstandingPrincipal + accruedInterest + penaltyFees;
        }

        public double GetEffectiveRecoveryRate(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0.0;

            // Simulated business logic: older policies have a slightly lower recovery rate
            int policyAgeYears = Math.Max(0, maturityDate.Year - 2010);
            double baseRate = 0.98; // 98% base recovery
            
            double effectiveRate = baseRate - (policyAgeYears * 0.005);
            
            return Math.Max(0.50, Math.Min(1.00, effectiveRate)); // Cap between 50% and 100%
        }

        public int GetDaysInArrears(string loanId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return 0;

            // Simulated logic: extract a date from a mock loan ID or return a calculated value
            // For demonstration, we'll use a deterministic hash of the loanId
            int hash = Math.Abs(loanId.GetHashCode());
            int simulatedDays = hash % 120; // 0 to 119 days
            
            return simulatedDays;
        }

        public string IssueClearanceCertificate(string policyId, string customerId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("Policy ID and Customer ID are required.");
            }

            string certId = Guid.NewGuid().ToString("N").ToUpper();
            return $"CERTIFICATE OF CLEARANCE | ID: {certId} | Policy: {policyId} | Customer: {customerId} | Status: CLEARED";
        }

        public bool IsFullRecoveryAchieved(decimal totalRecovered, decimal totalDue)
        {
            if (totalDue <= 0) return true;
            
            // Business rule: Allow a 1 cent rounding tolerance
            decimal tolerance = 0.01m;
            return totalRecovered >= (totalDue - tolerance);
        }

        public decimal ComputeWriteOffAmount(decimal outstandingBalance, double writeOffPercentage)
        {
            if (outstandingBalance <= 0) return 0m;
            if (writeOffPercentage < 0 || writeOffPercentage > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(writeOffPercentage), "Percentage must be between 0.0 and 1.0");
            }

            decimal writeOff = outstandingBalance * (decimal)writeOffPercentage;
            return Math.Round(writeOff, 2, MidpointRounding.AwayFromZero);
        }

        public int CountActiveLiens(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;

            // Simulated database lookup
            return policyId.StartsWith("LIEN") ? 2 : 0;
        }

        public string RetrieveDischargeTemplateId(string productCode, int regionCode)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return "TPL-DEFAULT";

            string normalizedCode = productCode.ToUpperInvariant();
            
            if (normalizedCode == "LIFE" && regionCode == 1) return "TPL-LIFE-NA";
            if (normalizedCode == "LIFE" && regionCode == 2) return "TPL-LIFE-EU";
            if (normalizedCode == "ENDOWMENT") return "TPL-END-GEN";
            
            return "TPL-DEFAULT";
        }

        public double CalculateInterestRebate(decimal principal, double standardRate, double rebateRate, int daysEarly)
        {
            if (principal <= 0 || daysEarly <= 0) return 0.0;
            if (rebateRate >= standardRate) return 0.0;

            double rateDifference = standardRate - rebateRate;
            double annualRebate = (double)principal * rateDifference;
            
            // Calculate daily rebate based on a 365-day year
            double actualRebate = (annualRebate / 365.0) * daysEarly;
            
            return Math.Round(actualRebate, 2);
        }

        public bool VerifySignatures(string documentId, int requiredSignatures)
        {
            if (string.IsNullOrWhiteSpace(documentId)) return false;
            if (requiredSignatures <= 0) return true;

            // Simulated verification logic
            int actualSignatures = documentId.Contains("SIGNED") ? requiredSignatures : 0;
            return actualSignatures >= requiredSignatures;
        }

        public decimal GetTotalRecoveredFromMaturity(string policyId, string loanId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(loanId)) return 0m;

            // Simulated retrieval of recovered funds
            return 5000.00m; 
        }

        public string ArchiveDischargeRecord(string policyId, string documentPath, DateTime archiveDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID required.");
            if (string.IsNullOrWhiteSpace(documentPath)) throw new ArgumentException("Document path required.");

            string archiveId = $"ARC-{archiveDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
            // In a real system, we would save the path and archiveId to a database here
            
            return archiveId;
        }

        public int GetGracePeriodDays(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return DEFAULT_GRACE_PERIOD;

            switch (productCode.ToUpperInvariant())
            {
                case "TERM":
                    return 15;
                case "WHOLELIFE":
                    return 45;
                case "UL":
                    return 60;
                default:
                    return DEFAULT_GRACE_PERIOD;
            }
        }

        public bool CheckPendingDisputes(string loanId)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return false;

            // Simulated logic: loans ending in 'D' have disputes
            return loanId.EndsWith("D", StringComparison.OrdinalIgnoreCase);
        }

        public decimal CalculateTaxOnForgivenDebt(decimal forgivenAmount, double taxRate)
        {
            if (forgivenAmount <= 0) return 0m;
            if (taxRate < 0) throw new ArgumentOutOfRangeException(nameof(taxRate), "Tax rate cannot be negative.");

            decimal tax = forgivenAmount * (decimal)taxRate;
            return Math.Round(tax, 2, MidpointRounding.AwayFromZero);
        }
    }
}