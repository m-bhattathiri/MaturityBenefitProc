using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.InternationalAndNRIProcessing
{
    // Fixed implementation — correct business logic
    public class ForexConversionService : IForexConversionService
    {
        private readonly string _baseCurrency = "INR";
        private readonly decimal _femaLimitUsd = 250000m;
        private readonly HashSet<string> _supportedCurrencies = new HashSet<string> { "USD", "EUR", "GBP", "AUD", "CAD", "SGD", "AED", "INR" };
        private readonly HashSet<string> _sanctionedCountries = new HashSet<string> { "IR", "KP", "SY", "CU" };
        private readonly Dictionary<string, decimal> _mockExchangeRates = new Dictionary<string, decimal>
        {
            { "USD", 83.50m },
            { "EUR", 90.20m },
            { "GBP", 105.40m },
            { "AUD", 54.30m }
        };

        public decimal ConvertCurrency(decimal amount, string sourceCurrency, string targetCurrency)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.");
            if (!IsCurrencySupported(sourceCurrency) || !IsCurrencySupported(targetCurrency))
                throw new ArgumentException("Unsupported currency.");

            if (sourceCurrency == targetCurrency) return amount;

            decimal rate = GetExchangeRate(sourceCurrency, targetCurrency, DateTime.UtcNow);
            return amount * rate;
        }

        public decimal CalculatePayoutAmount(string policyId, string targetCurrency, DateTime payoutDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            
            // Mock base payout amount in INR
            decimal baseAmount = 1000000m; 
            return ConvertCurrency(baseAmount, _baseCurrency, targetCurrency);
        }

        public decimal GetExchangeRate(string sourceCurrency, string targetCurrency, DateTime rateDate)
        {
            if (sourceCurrency == targetCurrency) return 1m;

            // Simplified logic: convert through INR
            decimal sourceToInr = sourceCurrency == _baseCurrency ? 1m : _mockExchangeRates.ContainsKey(sourceCurrency) ? _mockExchangeRates[sourceCurrency] : 1m;
            decimal targetToInr = targetCurrency == _baseCurrency ? 1m : _mockExchangeRates.ContainsKey(targetCurrency) ? _mockExchangeRates[targetCurrency] : 1m;

            return sourceToInr / targetToInr;
        }

        public bool IsCurrencySupported(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode)) return false;
            return _supportedCurrencies.Contains(currencyCode.ToUpperInvariant());
        }

        public bool ValidateFEMACompliance(string customerId, decimal payoutAmount)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return false;
            if (payoutAmount < 0) return false;

            // LRS limit is $250,000 per financial year
            decimal convertedToUsd = ConvertCurrency(payoutAmount, _baseCurrency, "USD");
            return convertedToUsd <= _femaLimitUsd;
        }

        public string GenerateTransactionReference(string policyId, string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException("Invalid inputs for transaction reference.");

            return $"FX-{policyId}-{currencyCode}-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        public int GetSettlementDays(string sourceCurrency, string targetCurrency)
        {
            if (sourceCurrency == "USD" || targetCurrency == "USD") return 2;
            if (sourceCurrency == "EUR" || targetCurrency == "EUR") return 2;
            return 3;
        }

        public double GetMarkupPercentage(string currencyPair)
        {
            if (string.IsNullOrWhiteSpace(currencyPair)) return 0.0;
            return currencyPair.Contains("USD") ? 0.015 : 0.025;
        }

        public decimal CalculateTaxOnForex(decimal convertedAmount, string countryCode)
        {
            if (convertedAmount <= 0) return 0m;
            
            // TCS (Tax Collected at Source) logic
            if (convertedAmount > 700000m)
            {
                return (convertedAmount - 700000m) * 0.05m;
            }
            return 0m;
        }

        public bool IsRepatriable(string accountType, string policyType)
        {
            if (string.IsNullOrWhiteSpace(accountType)) return false;
            return accountType.Equals("NRE", StringComparison.OrdinalIgnoreCase) || 
                   accountType.Equals("FCNR", StringComparison.OrdinalIgnoreCase);
        }

        public decimal GetRepatriableAmount(string policyId, decimal totalMaturityAmount)
        {
            if (totalMaturityAmount <= 0) return 0m;
            // Mock logic: 80% is repatriable
            return totalMaturityAmount * 0.8m;
        }

        public decimal GetNonRepatriableAmount(string policyId, decimal totalMaturityAmount)
        {
            return totalMaturityAmount - GetRepatriableAmount(policyId, totalMaturityAmount);
        }

        public string GetBaseCurrencyCode()
        {
            return _baseCurrency;
        }

        public double GetTaxRateForNRI(string countryOfResidence)
        {
            if (string.IsNullOrWhiteSpace(countryOfResidence)) return 0.30; // Default DTAA rate
            
            // Mock DTAA rates
            if (countryOfResidence.ToUpperInvariant() == "US")
            {
                return 0.15;
            }
            else if (countryOfResidence.ToUpperInvariant() == "UK")
            {
                return 0.15;
            }
            else if (countryOfResidence.ToUpperInvariant() == "AE")
            {
                return 0.0;
            }
            else if (countryOfResidence.ToUpperInvariant() == "SG")
            {
                return 0.10;
            }
            else
            {
                return 0.30;
            }
        }

        public bool IsNREAccountValid(string accountNumber, string bankCode)
        {
            if (string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(bankCode)) return false;
            return accountNumber.Length >= 10 && accountNumber.Length <= 18 && bankCode.Length == 11;
        }

        public bool IsNROAccountValid(string accountNumber, string bankCode)
        {
            return IsNREAccountValid(accountNumber, bankCode); // Same basic validation
        }

        public int GetLockInPeriod(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Invalid policy ID");
            return policyId.StartsWith("ULIP") ? 5 : 3;
        }

        public decimal CalculateConversionFee(decimal amount, string currencyCode)
        {
            if (amount <= 0) return 0m;
            decimal feePercentage = currencyCode == "USD" ? 0.005m : 0.01m;
            decimal fee = amount * feePercentage;
            return Math.Max(fee, 500m); // Minimum fee of 500 INR
        }

        public string GetSwiftCode(string bankId, string branchCode)
        {
            if (string.IsNullOrWhiteSpace(bankId)) return null;
            string branch = string.IsNullOrWhiteSpace(branchCode) ? "XXX" : branchCode.PadRight(3, 'X').Substring(0, 3);
            return $"{bankId.PadRight(8, 'X').Substring(0, 8)}{branch}".ToUpperInvariant();
        }

        public bool ValidateSwiftCode(string swiftCode)
        {
            if (string.IsNullOrWhiteSpace(swiftCode)) return false;
            return swiftCode.Length == 8 || swiftCode.Length == 11;
        }

        public double GetVolatilityIndex(string currencyCode, int days)
        {
            if (!IsCurrencySupported(currencyCode)) return 0.0;
            return days > 30 ? 0.05 : 0.02; // Mock volatility
        }

        public bool HasForexLimitExceeded(string customerId, decimal newAmount, DateTime financialYearStart)
        {
            if (newAmount < 0) return false;
            decimal currentUsageUsd = 150000m; // Mock existing usage
            decimal newAmountUsd = ConvertCurrency(newAmount, _baseCurrency, "USD");
            return (currentUsageUsd + newAmountUsd) > _femaLimitUsd;
        }

        public int GetTransactionCount(string customerId, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Start date cannot be after end date");
            return 5; // Mock count
        }

        public decimal CalculateForwardRate(string currencyPair, int forwardDays, double interestRateDifferential)
        {
            if (forwardDays <= 0) return CalculateSpotRate(currencyPair);
            decimal spotRate = CalculateSpotRate(currencyPair);
            decimal forwardPoints = spotRate * (decimal)interestRateDifferential * (forwardDays / 360m);
            return spotRate + forwardPoints;
        }

        public decimal CalculateSpotRate(string currencyPair)
        {
            if (string.IsNullOrWhiteSpace(currencyPair) || currencyPair.Length != 6) return 1m;
            string source = currencyPair.Substring(0, 3);
            string target = currencyPair.Substring(3, 3);
            return GetExchangeRate(source, target, DateTime.UtcNow);
        }

        public bool IsSanctionedCountry(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return false;
            return _sanctionedCountries.Contains(countryCode.ToUpperInvariant());
        }

        public string GetIbanFormat(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return null;
            if (countryCode.ToUpperInvariant() == "GB")
            {
                return "^GB\\d{2}[A-Z]{4}\\d{14}$";
            }
            else if (countryCode.ToUpperInvariant() == "AE")
            {
                return "^AE\\d{21}$";
            }
            else if (countryCode.ToUpperInvariant() == "DE")
            {
                return "^DE\\d{20}$";
            }
            else
            {
                return "^[A-Z]{2}\\d{2}[A-Z0-9]{11,30}$";
            }
        }

        public int GetRemainingForexLimitDays(string customerId)
        {
            DateTime now = DateTime.UtcNow;
            DateTime endOfFinancialYear = new DateTime(now.Month > 3 ? now.Year + 1 : now.Year, 3, 31);
            return (endOfFinancialYear - now).Days;
        }

        public double GetDiscountFactor(string currencyCode, int daysToMaturity)
        {
            if (daysToMaturity <= 0) return 1.0;
            double riskFreeRate = currencyCode == "USD" ? 0.05 : 0.07;
            return 1.0 / Math.Pow(1.0 + riskFreeRate, daysToMaturity / 365.0);
        }

        public string GetFemaDeclarationId(string customerId, DateTime declarationDate)
        {
            if (string.IsNullOrWhiteSpace(customerId)) throw new ArgumentNullException(nameof(customerId));
            return $"FEMA-{customerId}-{declarationDate:yyyyMMdd}";
        }

        public bool IsRateLocked(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return false;
            return transactionId.StartsWith("LOCK");
        }

        public decimal ApplyLockedRate(decimal amount, string transactionId)
        {
            if (amount <= 0) return 0m;
            if (!IsRateLocked(transactionId)) return amount;
            
            // Mock locked rate multiplier
            return amount * 1.02m; 
        }

        public double GetSpreadRatio(string bankCode, string currencyPair)
        {
            if (string.IsNullOrWhiteSpace(bankCode) || string.IsNullOrWhiteSpace(currencyPair)) return 0.0;
            return currencyPair.IndexOf("USD") >= 0 ? 0.001 : 0.0025;
        }

        public int GetGracePeriodDays(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return 15;
            return countryCode.Equals("US", StringComparison.OrdinalIgnoreCase) ? 30 : 15;
        }

        public decimal GetNREAccountBalance(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentException("Invalid account ID");
            return 500000m; // Mock balance
        }

        public decimal GetNROAccountBalance(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentException("Invalid account ID");
            return 250000m; // Mock balance
        }
    }
}