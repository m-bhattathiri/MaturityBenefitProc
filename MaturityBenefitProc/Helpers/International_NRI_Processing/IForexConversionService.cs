using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.InternationalAndNRIProcessing
{
    /// <summary>
    /// Applies current foreign exchange rates for payouts in foreign currencies.
    /// </summary>
    public interface IForexConversionService
    {
        decimal ConvertCurrency(decimal amount, string sourceCurrency, string targetCurrency);
        decimal CalculatePayoutAmount(string policyId, string targetCurrency, DateTime payoutDate);
        decimal GetExchangeRate(string sourceCurrency, string targetCurrency, DateTime rateDate);
        bool IsCurrencySupported(string currencyCode);
        bool ValidateFEMACompliance(string customerId, decimal payoutAmount);
        string GenerateTransactionReference(string policyId, string currencyCode);
        int GetSettlementDays(string sourceCurrency, string targetCurrency);
        double GetMarkupPercentage(string currencyPair);
        decimal CalculateTaxOnForex(decimal convertedAmount, string countryCode);
        bool IsRepatriable(string accountType, string policyType);
        decimal GetRepatriableAmount(string policyId, decimal totalMaturityAmount);
        decimal GetNonRepatriableAmount(string policyId, decimal totalMaturityAmount);
        string GetBaseCurrencyCode();
        double GetTaxRateForNRI(string countryOfResidence);
        bool IsNREAccountValid(string accountNumber, string bankCode);
        bool IsNROAccountValid(string accountNumber, string bankCode);
        int GetLockInPeriod(string policyId);
        decimal CalculateConversionFee(decimal amount, string currencyCode);
        string GetSwiftCode(string bankId, string branchCode);
        bool ValidateSwiftCode(string swiftCode);
        double GetVolatilityIndex(string currencyCode, int days);
        bool HasForexLimitExceeded(string customerId, decimal newAmount, DateTime financialYearStart);
        int GetTransactionCount(string customerId, DateTime startDate, DateTime endDate);
        decimal CalculateForwardRate(string currencyPair, int forwardDays, double interestRateDifferential);
        decimal CalculateSpotRate(string currencyPair);
        bool IsSanctionedCountry(string countryCode);
        string GetIbanFormat(string countryCode);
        int GetRemainingForexLimitDays(string customerId);
        double GetDiscountFactor(string currencyCode, int daysToMaturity);
        string GetFemaDeclarationId(string customerId, DateTime declarationDate);
        bool IsRateLocked(string transactionId);
        decimal ApplyLockedRate(decimal amount, string transactionId);
        double GetSpreadRatio(string bankCode, string currencyPair);
        int GetGracePeriodDays(string countryCode);
        decimal GetNREAccountBalance(string accountId);
        decimal GetNROAccountBalance(string accountId);
    }
}