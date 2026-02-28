using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    // Fixed implementation — correct business logic
    public class FatcaComplianceService : IFatcaComplianceService
    {
        private const decimal DefaultReportingThreshold = 50000m;
        private const double DefaultWithholdingRate = 0.30;
        private const double TreatyWithholdingRate = 0.15;

        public bool ValidateFatcaStatus(string policyNumber, string customerId)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || string.IsNullOrWhiteSpace(customerId))
                return false;

            // Simulate FATCA status validation (e.g., checking database)
            return customerId.StartsWith("CUST-") && policyNumber.Length == 10;
        }

        public bool IsCrsDeclarationRequired(string customerId, decimal disbursementAmount)
        {
            if (string.IsNullOrWhiteSpace(customerId) || disbursementAmount < 0)
                throw new ArgumentException("Invalid input parameters.");

            return disbursementAmount >= DefaultReportingThreshold || HasActiveIndicia(customerId);
        }

        public string GetTaxResidencyCountryCode(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return "UNKNOWN";

            // Simulated logic: Even IDs are US, odd are UK
            int idNumber;
            if (int.TryParse(customerId.Replace("CUST-", ""), out idNumber))
            {
                return idNumber % 2 == 0 ? "US" : "GB";
            }
            return "US";
        }

        public decimal CalculateWithholdingTax(decimal grossAmount, double withholdingRate)
        {
            if (grossAmount <= 0) return 0m;
            if (withholdingRate < 0 || withholdingRate > 1)
                throw new ArgumentOutOfRangeException(nameof(withholdingRate), "Rate must be between 0 and 1.");

            return Math.Round(grossAmount * (decimal)withholdingRate, 2);
        }

        public double GetApplicableWithholdingRate(string countryCode, bool hasValidW8Ben)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return DefaultWithholdingRate;

            if (countryCode.ToUpper() == "US") return 0.0; // US persons don't have FATCA withholding, they have backup withholding if TIN is missing

            if (hasValidW8Ben)
            {
                // Simulated treaty countries
                var treatyCountries = new HashSet<string> { "GB", "CA", "AU", "DE", "FR" };
                return treatyCountries.Contains(countryCode.ToUpper()) ? TreatyWithholdingRate : DefaultWithholdingRate;
            }

            return DefaultWithholdingRate;
        }

        public int GetDaysUntilDeclarationExpiry(string customerId, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 0;

            // Simulate expiry date calculation (e.g., W-8BEN expires after 3 years)
            DateTime expiryDate = new DateTime(currentDate.Year, 12, 31).AddYears(1);
            int days = (expiryDate - currentDate).Days;
            
            return days > 0 ? days : 0;
        }

        public bool VerifyTinFormat(string taxIdentificationNumber, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(taxIdentificationNumber) || string.IsNullOrWhiteSpace(countryCode))
                return false;

            switch (countryCode.ToUpper())
            {
                case "US":
                    // SSN or EIN format
                    return Regex.IsMatch(taxIdentificationNumber, @"^\d{3}-\d{2}-\d{4}$") || 
                           Regex.IsMatch(taxIdentificationNumber, @"^\d{2}-\d{7}$");
                case "GB":
                    // UK NINO format
                    return Regex.IsMatch(taxIdentificationNumber, @"^[A-CEGHJ-PR-TW-Z]{1}[A-CEGHJ-NPR-TW-Z]{1}[0-9]{6}[A-D\s]{1}$", RegexOptions.IgnoreCase);
                default:
                    // Generic fallback: alphanumeric, 5-20 chars
                    return Regex.IsMatch(taxIdentificationNumber, @"^[A-Z0-9]{5,20}$", RegexOptions.IgnoreCase);
            }
        }

        public string GenerateComplianceReportId(string customerId, DateTime disbursementDate)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty.");

            return $"FATCA-{customerId}-{disbursementDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public int CountMissingComplianceDocuments(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 3; // Assume worst case if invalid

            // Simulated logic based on customer ID length
            return customerId.Length % 3;
        }

        public decimal GetThresholdAmountForReporting(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return DefaultReportingThreshold;

            switch (countryCode.ToUpper())
            {
                case "US": return 50000m;
                case "GB": return 75000m; // Equivalent in GBP approx
                case "CH": return 100000m;
                default: return DefaultReportingThreshold;
            }
        }

        public bool CheckHighValueDisbursementEligibility(string policyNumber, decimal disbursementAmount)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || disbursementAmount <= 0)
                return false;

            // High value requires more stringent checks
            if (disbursementAmount > 250000m)
            {
                // Simulate checking if policy is in good standing
                return policyNumber.StartsWith("POL-HV");
            }

            return true;
        }

        public string RetrieveFatcaClassificationCode(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return "UNKNOWN";

            // Simulated classification logic
            if (customerId.Contains("CORP")) return "Active NFFE";
            if (customerId.Contains("BANK")) return "FFI";
            return "Individual";
        }

        public double CalculateCrsRiskScore(string customerId, int numberOfForeignAccounts)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 100.0; // Max risk
            if (numberOfForeignAccounts < 0) return 0.0;

            double baseScore = HasActiveIndicia(customerId) ? 50.0 : 10.0;
            double accountMultiplier = numberOfForeignAccounts * 5.0;
            
            return Math.Min(100.0, baseScore + accountMultiplier);
        }

        public bool HasActiveIndicia(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return false;

            // Simulated logic: check if customer has foreign phone number, address, or standing instructions
            return customerId.EndsWith("-F");
        }

        public int GetGracePeriodDays(string customerId, string classificationCode)
        {
            if (string.IsNullOrWhiteSpace(classificationCode)) return 0;

            switch (classificationCode.ToUpper())
            {
                case "INDIVIDUAL": return 30;
                case "ACTIVE NFFE": return 60;
                case "FFI": return 90;
                default: return 15;
            }
        }

        public decimal ComputeNetDisbursementAmount(decimal grossAmount, decimal totalWithheld)
        {
            if (grossAmount < 0 || totalWithheld < 0)
                throw new ArgumentException("Amounts cannot be negative.");

            if (totalWithheld > grossAmount)
                throw new InvalidOperationException("Withheld amount cannot exceed gross amount.");

            return grossAmount - totalWithheld;
        }

        public string SubmitCrsReport(string customerId, decimal reportableAmount, DateTime reportingPeriod)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.");
            if (reportableAmount < 0)
                throw new ArgumentException("Reportable amount cannot be negative.");

            string reportId = GenerateComplianceReportId(customerId, reportingPeriod);
            
            // Simulate API call or database insert
            string status = reportableAmount > DefaultReportingThreshold ? "SUBMITTED" : "IGNORED_BELOW_THRESHOLD";
            
            return $"{reportId}|{status}";
        }
    }
}