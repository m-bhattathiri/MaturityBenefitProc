// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    public class TaxReportingService : ITaxReportingService
    {
        private const decimal BASE_EXEMPTION_AMOUNT = 50000m;
        private const decimal DAILY_PENALTY_RATE = 15.50m;
        private const double MAX_PENALTY_PERCENTAGE = 25.0;

        public decimal CalculateTotalTaxableAmount(string policyId, DateTime assessmentDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            // Simulated business logic for taxable amount calculation
            decimal grossValue = 150000m; // Mock retrieval
            decimal deductions = 25000m;  // Mock retrieval
            
            decimal taxableAmount = grossValue - deductions;
            return Math.Max(0m, taxableAmount);
        }

        public decimal ComputeWithholdingTax(decimal grossAmount, double withholdingRate)
        {
            if (grossAmount < 0) throw new ArgumentOutOfRangeException(nameof(grossAmount), "Gross amount cannot be negative.");
            if (withholdingRate < 0 || withholdingRate > 1) throw new ArgumentOutOfRangeException(nameof(withholdingRate), "Rate must be between 0 and 1.");

            return Math.Round(grossAmount * (decimal)withholdingRate, 2);
        }

        public decimal GetYearToDateDeductions(string taxIdentificationNumber, int taxYear)
        {
            if (string.IsNullOrWhiteSpace(taxIdentificationNumber)) return 0m;
            if (taxYear > DateTime.UtcNow.Year) return 0m;

            // Mock database lookup
            return 12500.75m;
        }

        public decimal CalculatePenaltyAmount(string policyId, int daysLate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Invalid policy ID.");
            if (daysLate <= 0) return 0m;

            return Math.Round(daysLate * DAILY_PENALTY_RATE, 2);
        }

        public decimal GetExemptBenefitAmount(string beneficiaryId, decimal totalBenefit)
        {
            if (totalBenefit <= 0) return 0m;
            if (string.IsNullOrWhiteSpace(beneficiaryId)) return 0m;

            // Base exemption logic
            return Math.Min(totalBenefit, BASE_EXEMPTION_AMOUNT);
        }

        public decimal ComputeCapitalGainsTax(decimal gainAmount, DateTime acquisitionDate, DateTime disposalDate)
        {
            if (gainAmount <= 0) return 0m;
            if (disposalDate < acquisitionDate) throw new ArgumentException("Disposal date cannot be before acquisition date.");

            TimeSpan holdingPeriod = disposalDate - acquisitionDate;
            decimal taxRate = holdingPeriod.TotalDays > 365 ? 0.15m : 0.25m; // Long-term vs short-term

            return Math.Round(gainAmount * taxRate, 2);
        }

        public decimal CalculateNetPayableAfterTaxes(decimal grossMaturityValue, decimal totalTaxes)
        {
            if (grossMaturityValue < 0) return 0m;
            if (totalTaxes < 0) totalTaxes = 0m;

            return Math.Max(0m, grossMaturityValue - totalTaxes);
        }

        public double GetEffectiveTaxRate(string taxIdentificationNumber, decimal totalIncome)
        {
            if (totalIncome <= 0) return 0.0;
            
            // Mock logic based on income tiers
            if (totalIncome < 50000m) return 0.10;
            if (totalIncome < 150000m) return 0.22;
            return 0.35;
        }

        public double CalculateComplianceRatio(int compliantRecords, int totalRecords)
        {
            if (totalRecords <= 0) return 0.0;
            if (compliantRecords < 0) return 0.0;
            
            return Math.Round((double)compliantRecords / totalRecords * 100.0, 2);
        }

        public double GetMarginalTaxBracket(decimal taxableIncome, int taxYear)
        {
            if (taxableIncome <= 0) return 0.0;

            // Simplified brackets
            if (taxableIncome <= 10000m) return 0.10;
            if (taxableIncome <= 40000m) return 0.12;
            if (taxableIncome <= 85000m) return 0.22;
            if (taxableIncome <= 163000m) return 0.24;
            return 0.32;
        }

        public double ComputePenaltyPercentage(int daysOverdue)
        {
            if (daysOverdue <= 0) return 0.0;
            
            double calculatedPenalty = (daysOverdue / 30.0) * 5.0; // 5% per month
            return Math.Min(calculatedPenalty, MAX_PENALTY_PERCENTAGE);
        }

        public double GetHistoricalAverageTaxRate(string beneficiaryId, int yearsToAnalyze)
        {
            if (string.IsNullOrWhiteSpace(beneficiaryId) || yearsToAnalyze <= 0) return 0.0;
            
            // Mocked historical data
            return 0.185;
        }

        public bool ValidateTaxIdentificationNumber(string taxIdentificationNumber, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(taxIdentificationNumber) || string.IsNullOrWhiteSpace(countryCode)) return false;

            if (countryCode.ToUpper() == "US")
            {
                return Regex.IsMatch(taxIdentificationNumber, @"^\d{3}-\d{2}-\d{4}$") || Regex.IsMatch(taxIdentificationNumber, @"^\d{9}$");
            }
            
            return taxIdentificationNumber.Length >= 5;
        }

        public bool IsEligibleForTaxExemption(string policyId, int ageAtMaturity)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            return ageAtMaturity >= 60; // Senior citizen exemption
        }

        public bool CheckComplianceStatus(string submissionId)
        {
            if (string.IsNullOrWhiteSpace(submissionId)) return false;
            return submissionId.StartsWith("COMP-");
        }

        public bool VerifyRegulatoryExtractReady(string batchId, DateTime generationDate)
        {
            if (string.IsNullOrWhiteSpace(batchId)) return false;
            return generationDate.Date <= DateTime.UtcNow.Date;
        }

        public bool IsForeignAccountTaxCompliant(string accountId, string jurisdictionCode)
        {
            if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(jurisdictionCode)) return false;
            // FATCA compliance mock
            return jurisdictionCode != "NON-COOP";
        }

        public bool HasPendingTaxAudits(string taxIdentificationNumber)
        {
            if (string.IsNullOrWhiteSpace(taxIdentificationNumber)) return false;
            return taxIdentificationNumber.EndsWith("99"); // Mock condition
        }

        public int GetTotalReportableTransactions(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Start date must be before end date.");
            return (int)(endDate - startDate).TotalDays * 5; // Mock calculation
        }

        public int CalculateDaysUntilSubmissionDeadline(int taxYear, string jurisdictionCode)
        {
            DateTime deadline = new DateTime(taxYear + 1, 4, 15); // April 15th of following year
            TimeSpan difference = deadline - DateTime.UtcNow;
            return difference.Days > 0 ? difference.Days : 0;
        }

        public int GetActiveTaxExemptionCount(string taxIdentificationNumber)
        {
            if (string.IsNullOrWhiteSpace(taxIdentificationNumber)) return 0;
            return 2; // Mock value
        }

        public int CountMissingTaxCertificates(string batchId)
        {
            if (string.IsNullOrWhiteSpace(batchId)) return 0;
            return batchId.GetHashCode() % 10; // Mock randomish value based on batch ID
        }

        public int GetTaxYear(DateTime maturityDate)
        {
            return maturityDate.Year;
        }

        public string GenerateTaxExtractFileId(string batchId, string regionCode)
        {
            if (string.IsNullOrWhiteSpace(batchId)) throw new ArgumentNullException(nameof(batchId));
            string region = string.IsNullOrWhiteSpace(regionCode) ? "DEF" : regionCode.ToUpper();
            return $"TAX-{region}-{batchId}-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        public string GetRegulatorySubmissionCode(string policyType, decimal benefitAmount)
        {
            string typeCode = string.IsNullOrWhiteSpace(policyType) ? "UNK" : policyType.Substring(0, Math.Min(3, policyType.Length)).ToUpper();
            string tier = benefitAmount > 100000m ? "T1" : "T2";
            return $"{typeCode}-{tier}";
        }

        public string RetrieveTaxOfficeRoutingNumber(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode)) return "DEFAULT-ROUTING";
            return $"RT-{postalCode.PadLeft(5, '0')}";
        }

        public string FormatTaxPayerReference(string internalId, string taxIdentificationNumber)
        {
            string safeId = internalId ?? "000";
            string safeTin = taxIdentificationNumber ?? "000000000";
            
            // Mask TIN for reference
            string maskedTin = safeTin.Length > 4 ? new string('*', safeTin.Length - 4) + safeTin.Substring(safeTin.Length - 4) : safeTin;
            return $"REF-{safeId}-{maskedTin}";
        }

        public string GetAuditTraceIdentifier(string submissionId, DateTime timestamp)
        {
            string safeSubId = string.IsNullOrWhiteSpace(submissionId) ? "NOSUB" : submissionId;
            return $"AUD-{safeSubId}-{timestamp.Ticks}";
        }
    }
}