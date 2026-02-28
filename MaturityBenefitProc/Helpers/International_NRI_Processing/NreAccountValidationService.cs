using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MaturityBenefitProc.Helpers.InternationalAndNriProcessing
{
    // Fixed implementation — correct business logic
    public class NreAccountValidationService : INreAccountValidationService
    {
        private const decimal MAX_REPATRIATION_LIMIT_USD = 1000000m; // 1 Million USD per financial year
        private const double DEFAULT_NRO_TDS_RATE = 31.2; // 30% + surcharge + cess
        private const double PAN_PROVIDED_NRO_TDS_RATE = 20.8;

        public bool ValidateNreAccountFormat(string accountNumber, string ifscCode)
        {
            if (string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(ifscCode))
                return false;

            // Basic Indian bank account number validation (9 to 18 digits)
            if (!Regex.IsMatch(accountNumber, @"^\d{9,18}$"))
                return false;

            // IFSC code validation (4 letters, 0, 6 alphanumeric)
            if (!Regex.IsMatch(ifscCode, @"^[A-Z]{4}0[A-Z0-9]{6}$"))
                return false;

            return true;
        }

        public bool IsAccountRepatriable(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
                return false;

            // NRE and FCNR accounts are freely repatriable. NRO is restricted.
            return accountId.StartsWith("NRE", StringComparison.OrdinalIgnoreCase) || 
                   accountId.StartsWith("FCNR", StringComparison.OrdinalIgnoreCase);
        }

        public decimal CalculateRepatriationLimit(string customerId, DateTime financialYearStart)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty.");

            // Under Liberalised Remittance Scheme (LRS) / FEMA guidelines for NRO, 
            // limit is typically 1 Million USD per financial year.
            decimal remittedYtd = GetTotalRemittedAmountYtd(customerId, financialYearStart.Year);
            decimal remainingLimit = MAX_REPATRIATION_LIMIT_USD - remittedYtd;

            return remainingLimit > 0 ? remainingLimit : 0m;
        }

        public double GetCurrentExchangeRate(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException("Currency code is required.");

            // Mocked exchange rates for demonstration
            if (currencyCode.ToUpper() == "USD")
            {
                return 83.25;
            }
            else if (currencyCode.ToUpper() == "GBP")
            {
                return 105.40;
            }
            else if (currencyCode.ToUpper() == "EUR")
            {
                return 89.60;
            }
            else if (currencyCode.ToUpper() == "AUD")
            {
                return 54.10;
            }
            else if (currencyCode.ToUpper() == "CAD")
            {
                return 61.20;
            }
            else if (currencyCode.ToUpper() == "INR")
            {
                return 1.0;
            }
            else
            {
                throw new NotSupportedException($"Currency {currencyCode} is not supported.");
            }
        }

        public int GetDaysSinceLastKycUpdate(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return 9999;

            // Mocking a database lookup based on customer ID length/hash
            int hash = Math.Abs(customerId.GetHashCode());
            return hash % 1000; // Returns a value between 0 and 999 days
        }

        public string GetAuthorizedDealerCode(string ifscCode)
        {
            if (string.IsNullOrWhiteSpace(ifscCode) || ifscCode.Length != 11)
                return null;

            string bankCode = ifscCode.Substring(0, 4);
            
            // Mock AD codes for major Indian banks
            if (bankCode == "SBIN")
            {
                return "AD0001234";
            }
            else if (bankCode == "HDFC")
            {
                return "AD0005678";
            }
            else if (bankCode == "ICIC")
            {
                return "AD0009012";
            }
            else if (bankCode == "UTIB")
            {
                return "AD0003456";
            }
            else
            {
                return $"AD999{bankCode}";
            }
        }

        public bool VerifyFemaCompliance(string customerId, decimal payoutAmount)
        {
            if (payoutAmount <= 0) return false;

            int pendingDeclarations = GetPendingFemaDeclarationsCount(customerId);
            if (pendingDeclarations > 0)
                return false; // Cannot process if FEMA declarations are pending

            int daysSinceKyc = GetDaysSinceLastKycUpdate(customerId);
            if (daysSinceKyc > 365)
                return false; // KYC must be updated annually for NRI accounts

            return true;
        }

        public decimal ComputeTdsOnNroPayout(decimal payoutAmount, double dtaaRate)
        {
            if (payoutAmount <= 0) return 0m;

            // If DTAA rate is applicable and lower than standard rate, use it
            double effectiveRate = (dtaaRate > 0 && dtaaRate < DEFAULT_NRO_TDS_RATE) 
                ? dtaaRate 
                : DEFAULT_NRO_TDS_RATE;

            return payoutAmount * (decimal)(effectiveRate / 100.0);
        }

        public double GetApplicableDtaaRate(string countryOfResidence)
        {
            if (string.IsNullOrWhiteSpace(countryOfResidence))
                return 0.0;

            // Mock DTAA (Double Taxation Avoidance Agreement) rates
            if (countryOfResidence.ToUpper() == "USA")
            {
                return 15.0;
            }
            else if (countryOfResidence.ToUpper() == "UK")
            {
                return 15.0;
            }
            else if (countryOfResidence.ToUpper() == "UAE")
            {
                return 10.0;
            }
            else if (countryOfResidence.ToUpper() == "SINGAPORE")
            {
                return 15.0;
            }
            else if (countryOfResidence.ToUpper() == "AUSTRALIA")
            {
                return 15.0;
            }
            else if (countryOfResidence.ToUpper() == "CANADA")
            {
                return 15.0;
            }
            else
            {
                return 0.0;
            }
        }

        public int CountActiveNreAccounts(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 0;
            
            // Mock logic: derive from customer ID length
            return Math.Max(1, customerId.Length % 4);
        }

        public string GenerateForm15CbReference(string customerId, decimal remittanceAmount)
        {
            if (string.IsNullOrWhiteSpace(customerId) || remittanceAmount <= 0)
                throw new ArgumentException("Invalid parameters for Form 15CB generation.");

            string datePrefix = DateTime.UtcNow.ToString("yyyyMMdd");
            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            
            return $"15CB-{datePrefix}-{customerId.Substring(0, Math.Min(4, customerId.Length)).ToUpper()}-{uniqueId}";
        }

        public bool CheckOciPioStatusValid(string documentId, DateTime expiryDate)
        {
            if (string.IsNullOrWhiteSpace(documentId))
                return false;

            // OCI/PIO cards must not be expired
            return expiryDate.Date > DateTime.UtcNow.Date;
        }

        public decimal GetTotalRemittedAmountYtd(string customerId, int financialYear)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 0m;

            // Mock logic: return a deterministic amount based on customer ID
            int hash = Math.Abs(customerId.GetHashCode());
            return (hash % 500000) + 10000m; // Between 10k and 510k USD
        }

        public double CalculateNroWithholdingTaxPercentage(bool hasPanCard, string countryCode)
        {
            if (!hasPanCard)
                return DEFAULT_NRO_TDS_RATE; // Max marginal rate if no PAN

            double dtaaRate = GetApplicableDtaaRate(countryCode);
            if (dtaaRate > 0)
            {
                // Surcharge and health/education cess might apply on top of DTAA, simplified here
                return dtaaRate + (dtaaRate * 0.04); // Adding 4% cess
            }

            return PAN_PROVIDED_NRO_TDS_RATE;
        }

        public int GetPendingFemaDeclarationsCount(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 1;

            // Mock logic
            return customerId.Contains("PENDING") ? 2 : 0;
        }

        public string ResolveSwiftCode(string bankName, string branchCode)
        {
            if (string.IsNullOrWhiteSpace(bankName) || string.IsNullOrWhiteSpace(branchCode))
                return null;

            string bankPrefix = bankName.Length >= 4 ? bankName.Substring(0, 4).ToUpper() : bankName.PadRight(4, 'X').ToUpper();
            string countryCode = "IN";
            string locationCode = "BB";
            string branchSuffix = branchCode.Length >= 3 ? branchCode.Substring(0, 3).ToUpper() : "XXX";

            return $"{bankPrefix}{countryCode}{locationCode}{branchSuffix}";
        }

        public bool ValidateNroToNreTransferEligibility(string sourceAccountId, string targetAccountId, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(sourceAccountId) || string.IsNullOrWhiteSpace(targetAccountId))
                return false;

            if (amount <= 0)
                return false;

            bool isSourceNro = sourceAccountId.StartsWith("NRO", StringComparison.OrdinalIgnoreCase);
            bool isTargetNre = targetAccountId.StartsWith("NRE", StringComparison.OrdinalIgnoreCase);

            if (!isSourceNro || !isTargetNre)
                return false; // Transfer must be strictly NRO to NRE

            // Limit check (1 Million USD equivalent)
            decimal usdEquivalent = amount / (decimal)GetCurrentExchangeRate("USD");
            if (usdEquivalent > MAX_REPATRIATION_LIMIT_USD)
                return false;

            return true;
        }
    }
}