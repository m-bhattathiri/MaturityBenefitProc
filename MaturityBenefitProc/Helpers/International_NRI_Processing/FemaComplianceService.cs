using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.International_NRI_Processing
{
    // Fixed implementation — correct business logic
    public class FemaComplianceService : IFemaComplianceService
    {
        private const decimal LRS_ANNUAL_LIMIT_USD = 250000m;
        private const decimal FORM_15CB_THRESHOLD_INR = 500000m;
        private const decimal NRO_TO_NRE_LIMIT_USD = 1000000m;

        private readonly Dictionary<string, double> _dtaaTaxRates = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "US", 15.0 },
            { "UK", 10.0 },
            { "AE", 0.0 }, // UAE
            { "SG", 15.0 },
            { "CA", 15.0 }
        };

        private readonly HashSet<string> _validFundSources = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "NRE_ACCOUNT", "FCNR_ACCOUNT", "FOREIGN_REMITTANCE"
        };

        public bool ValidateRepatriationEligibility(string policyNumber, string nriCustomerId)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || string.IsNullOrWhiteSpace(nriCustomerId))
                return false;

            // Simulate DB check for policy and customer status
            bool isPolicyActive = policyNumber.Length > 5;
            bool isKycCompliant = nriCustomerId.StartsWith("NRI");

            return isPolicyActive && isKycCompliant;
        }

        public decimal CalculatePermissibleRepatriationAmount(string policyNumber, decimal totalMaturityAmount)
        {
            if (totalMaturityAmount <= 0) return 0m;

            // Under FEMA, if premiums were paid from NRE/FCNR, the entire amount is repatriable.
            // Simulating a 100% repatriable policy if policy number ends with 'R', else 50%.
            if (policyNumber != null && policyNumber.EndsWith("R", StringComparison.OrdinalIgnoreCase))
            {
                return totalMaturityAmount;
            }

            return totalMaturityAmount * 0.5m;
        }

        public double GetCurrentFemaWithholdingTaxRate(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                return 31.2; // Default maximum marginal rate for non-residents

            if (_dtaaTaxRates.TryGetValue(countryCode, out double rate))
            {
                return rate;
            }

            return 20.0; // Standard base rate without DTAA benefit
        }

        public int GetDaysSinceLastRepatriation(string nriCustomerId)
        {
            if (string.IsNullOrWhiteSpace(nriCustomerId))
                throw new ArgumentException("Customer ID cannot be null or empty.", nameof(nriCustomerId));

            // Simulate fetching last transaction date from database
            int mockDays = Math.Abs(nriCustomerId.GetHashCode()) % 365;
            return mockDays;
        }

        public string GenerateFemaComplianceCertificateId(string transactionReference)
        {
            if (string.IsNullOrWhiteSpace(transactionReference))
                throw new ArgumentException("Transaction reference is required.");

            string datePrefix = DateTime.UtcNow.ToString("yyyyMMdd");
            string uniqueSuffix = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            return $"FEMA-{datePrefix}-{transactionReference.Substring(0, Math.Min(4, transactionReference.Length)).ToUpper()}-{uniqueSuffix}";
        }

        public bool CheckNroToNreTransferValidity(string sourceAccount, string destinationAccount, decimal transferAmount)
        {
            if (string.IsNullOrWhiteSpace(sourceAccount) || string.IsNullOrWhiteSpace(destinationAccount))
                return false;

            if (transferAmount <= 0)
                return false;

            // FEMA allows up to USD 1 Million per financial year from NRO to NRE
            // Assuming transferAmount is in USD for this check
            if (transferAmount > NRO_TO_NRE_LIMIT_USD)
                return false;

            return sourceAccount.StartsWith("NRO") && destinationAccount.StartsWith("NRE");
        }

        public decimal ComputeTdsOnNonRepatriableAmount(decimal nonRepatriableAmount, double currentTaxRate)
        {
            if (nonRepatriableAmount <= 0 || currentTaxRate <= 0)
                return 0m;

            decimal taxRateDecimal = (decimal)(currentTaxRate / 100.0);
            return Math.Round(nonRepatriableAmount * taxRateDecimal, 2);
        }

        public int GetAnnualRepatriationTransactionCount(string nriCustomerId, DateTime currentFinancialYearStart)
        {
            if (string.IsNullOrWhiteSpace(nriCustomerId)) return 0;
            if (currentFinancialYearStart > DateTime.UtcNow) return 0;

            // Mock logic based on customer ID length
            return nriCustomerId.Length % 5;
        }

        public string RetrieveAuthorizedDealerBankCode(string bankName, string branchCode)
        {
            if (string.IsNullOrWhiteSpace(bankName) || string.IsNullOrWhiteSpace(branchCode))
                return "UNKNOWN_AD_CODE";

            string cleanBankName = new string(bankName.Where(char.IsLetterOrDigit).ToArray()).ToUpper();
            string cleanBranchCode = new string(branchCode.Where(char.IsLetterOrDigit).ToArray()).ToUpper();

            return $"AD-{cleanBankName.Substring(0, Math.Min(4, cleanBankName.Length))}-{cleanBranchCode}";
        }

        public bool IsForm15CbRequired(decimal payoutAmount, string countryCode)
        {
            if (payoutAmount <= 0) return false;

            // Form 15CB is generally required by a Chartered Accountant if remittance exceeds INR 5,00,000
            return payoutAmount > FORM_15CB_THRESHOLD_INR;
        }

        public decimal CalculateExchangeRateVariance(decimal baseAmount, double appliedExchangeRate, double standardExchangeRate)
        {
            if (baseAmount <= 0 || appliedExchangeRate <= 0 || standardExchangeRate <= 0)
                return 0m;

            decimal appliedValue = baseAmount * (decimal)appliedExchangeRate;
            decimal standardValue = baseAmount * (decimal)standardExchangeRate;

            return Math.Round(Math.Abs(appliedValue - standardValue), 2);
        }

        public double GetApplicableSurchargePercentage(decimal totalPayoutAmount)
        {
            // Indian tax surcharge slabs for non-residents
            if (totalPayoutAmount > 50000000m) return 37.0; // > 5 Cr
            if (totalPayoutAmount > 20000000m) return 25.0; // > 2 Cr
            if (totalPayoutAmount > 10000000m) return 15.0; // > 1 Cr
            if (totalPayoutAmount > 5000000m) return 10.0;  // > 50 Lakhs
            
            return 0.0;
        }

        public int CalculateRemainingDaysForLrsLimit(string nriCustomerId, DateTime transactionDate)
        {
            // LRS limits reset every financial year (April 1st to March 31st)
            DateTime financialYearEnd;
            if (transactionDate.Month >= 4)
            {
                financialYearEnd = new DateTime(transactionDate.Year + 1, 3, 31);
            }
            else
            {
                financialYearEnd = new DateTime(transactionDate.Year, 3, 31);
            }

            return (financialYearEnd - transactionDate).Days;
        }

        public string GetFemaViolationCode(string policyNumber, decimal attemptedAmount)
        {
            if (attemptedAmount > LRS_ANNUAL_LIMIT_USD)
                return "ERR_FEMA_LRS_EXCEEDED";

            if (string.IsNullOrWhiteSpace(policyNumber))
                return "ERR_FEMA_INVALID_POLICY";

            return "NO_VIOLATION";
        }

        public bool VerifyOciPioStatus(string customerId, string documentReference)
        {
            if (string.IsNullOrWhiteSpace(customerId) || string.IsNullOrWhiteSpace(documentReference))
                return false;

            // Mock verification: document reference must contain OCI or PIO
            string upperDocRef = documentReference.ToUpper();
            return upperDocRef.Contains("OCI") || upperDocRef.Contains("PIO");
        }

        public decimal GetTotalRepatriatedAmountYearToDate(string nriCustomerId, DateTime financialYearStart)
        {
            if (string.IsNullOrWhiteSpace(nriCustomerId)) return 0m;

            // Mock logic: return a deterministic amount based on customer ID
            int seed = nriCustomerId.GetHashCode();
            Random rand = new Random(seed);
            return (decimal)(rand.NextDouble() * 100000);
        }

        public bool ValidateSourceOfFunds(string policyNumber, string fundSourceCode)
        {
            if (string.IsNullOrWhiteSpace(fundSourceCode)) return false;

            return _validFundSources.Contains(fundSourceCode);
        }

        public string RequestReserveBankApproval(string nriCustomerId, decimal requestedAmount, string purposeCode)
        {
            if (string.IsNullOrWhiteSpace(nriCustomerId) || requestedAmount <= 0)
                return "REJECTED_INVALID_INPUT";

            if (requestedAmount <= LRS_ANNUAL_LIMIT_USD)
                return "AUTO_APPROVED_WITHIN_LIMITS";

            if (string.IsNullOrWhiteSpace(purposeCode))
                return "REJECTED_MISSING_PURPOSE_CODE";

            // Mock an RBI approval reference
            string dateStr = DateTime.UtcNow.ToString("yyMMdd");
            return $"RBI-APV-{dateStr}-{purposeCode.ToUpper()}-{new Random().Next(1000, 9999)}";
        }
    }
}