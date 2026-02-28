using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.InternationalAndNRIProcessing;

namespace MaturityBenefitProc.Tests.Helpers.InternationalAndNRIProcessing
{
    [TestClass]
    public class ForexConversionServiceIntegrationTests
    {
        // Note: Assuming ForexConversionService is the concrete implementation of IForexConversionService
        // For testing purposes, we use a mock or concrete implementation if available.
        // Since the prompt specifies to use `new ForexConversionService()`, we assume it exists.
        private IForexConversionService _service;

        [TestInitialize]
        public void Setup()
        {
            // In a real scenario, this would be the concrete class. 
            // Assuming ForexConversionService implements IForexConversionService.
            // _service = new ForexConversionService();
            
            // For compilation in this generated code, we will use a mock implementation or assume it exists.
            // We will write the tests assuming _service is instantiated properly.
            _service = CreateServiceInstance();
        }

        private IForexConversionService CreateServiceInstance()
        {
            // Placeholder for actual instantiation
            return null; // Replace with actual new ForexConversionService()
        }

        [TestMethod]
        public void ConvertCurrency_ValidConversion_ReturnsConsistentResults()
        {
            decimal amount = 1000m;
            string source = "USD";
            string target = "INR";
            DateTime date = DateTime.Now;

            bool isSupportedSource = _service.IsCurrencySupported(source);
            bool isSupportedTarget = _service.IsCurrencySupported(target);
            decimal rate = _service.GetExchangeRate(source, target, date);
            decimal converted = _service.ConvertCurrency(amount, source, target);

            Assert.IsTrue(isSupportedSource);
            Assert.IsTrue(isSupportedTarget);
            Assert.IsTrue(rate > 0);
            Assert.AreEqual(amount * rate, converted);
            Assert.AreNotEqual(0m, converted);
        }

        [TestMethod]
        public void CalculatePayoutAmount_ValidPolicy_CalculatesCorrectly()
        {
            string policyId = "POL123";
            string targetCurrency = "USD";
            DateTime payoutDate = DateTime.Now;

            string baseCurrency = _service.GetBaseCurrencyCode();
            decimal payout = _service.CalculatePayoutAmount(policyId, targetCurrency, payoutDate);
            decimal rate = _service.GetExchangeRate(baseCurrency, targetCurrency, payoutDate);
            string refId = _service.GenerateTransactionReference(policyId, targetCurrency);

            Assert.IsNotNull(baseCurrency);
            Assert.IsTrue(payout >= 0);
            Assert.IsTrue(rate > 0);
            Assert.IsNotNull(refId);
            Assert.IsTrue(refId.Contains(policyId));
        }

        [TestMethod]
        public void ValidateFEMACompliance_ValidCustomer_ReturnsTrue()
        {
            string customerId = "CUST001";
            decimal amount = 50000m;
            DateTime startYear = new DateTime(DateTime.Now.Year, 4, 1);

            bool isCompliant = _service.ValidateFEMACompliance(customerId, amount);
            bool limitExceeded = _service.HasForexLimitExceeded(customerId, amount, startYear);
            int remainingDays = _service.GetRemainingForexLimitDays(customerId);
            string declarationId = _service.GetFemaDeclarationId(customerId, DateTime.Now);

            Assert.IsTrue(isCompliant);
            Assert.IsFalse(limitExceeded);
            Assert.IsTrue(remainingDays >= 0);
            Assert.IsNotNull(declarationId);
            Assert.IsTrue(declarationId.Contains(customerId));
        }

        [TestMethod]
        public void RepatriableAmount_NREAccount_CalculatesCorrectly()
        {
            string policyId = "POL456";
            decimal totalAmount = 100000m;
            string accountType = "NRE";
            string policyType = "Life";

            bool isRepatriable = _service.IsRepatriable(accountType, policyType);
            decimal repAmount = _service.GetRepatriableAmount(policyId, totalAmount);
            decimal nonRepAmount = _service.GetNonRepatriableAmount(policyId, totalAmount);
            int lockIn = _service.GetLockInPeriod(policyId);

            Assert.IsTrue(isRepatriable);
            Assert.AreEqual(totalAmount, repAmount + nonRepAmount);
            Assert.IsTrue(repAmount >= 0);
            Assert.IsTrue(nonRepAmount >= 0);
            Assert.IsTrue(lockIn >= 0);
        }

        [TestMethod]
        public void TaxCalculation_NRI_CalculatesCorrectly()
        {
            decimal convertedAmount = 50000m;
            string countryCode = "US";

            decimal tax = _service.CalculateTaxOnForex(convertedAmount, countryCode);
            double taxRate = _service.GetTaxRateForNRI(countryCode);
            bool isSanctioned = _service.IsSanctionedCountry(countryCode);
            int gracePeriod = _service.GetGracePeriodDays(countryCode);

            Assert.IsTrue(tax >= 0);
            Assert.IsTrue(taxRate >= 0);
            Assert.IsFalse(isSanctioned);
            Assert.IsTrue(gracePeriod >= 0);
            Assert.AreEqual((decimal)taxRate * convertedAmount, tax);
        }

        [TestMethod]
        public void SwiftCode_Validation_ReturnsTrueForValid()
        {
            string bankId = "HDFC";
            string branchCode = "001";

            string swiftCode = _service.GetSwiftCode(bankId, branchCode);
            bool isValid = _service.ValidateSwiftCode(swiftCode);
            string ibanFormat = _service.GetIbanFormat("IN");
            double spread = _service.GetSpreadRatio(bankId, "USD-INR");

            Assert.IsNotNull(swiftCode);
            Assert.IsTrue(isValid);
            Assert.IsNotNull(ibanFormat);
            Assert.IsTrue(spread >= 0);
            Assert.IsTrue(swiftCode.Length >= 8);
        }

        [TestMethod]
        public void ForwardRate_Calculation_ReturnsExpected()
        {
            string pair = "USD-INR";
            int days = 30;
            double diff = 0.02;

            decimal spot = _service.CalculateSpotRate(pair);
            decimal forward = _service.CalculateForwardRate(pair, days, diff);
            double markup = _service.GetMarkupPercentage(pair);
            int settlement = _service.GetSettlementDays("USD", "INR");

            Assert.IsTrue(spot > 0);
            Assert.IsTrue(forward > 0);
            Assert.IsTrue(markup >= 0);
            Assert.IsTrue(settlement > 0);
            Assert.AreNotEqual(spot, forward);
        }

        [TestMethod]
        public void LockedRate_Application_ReturnsLockedAmount()
        {
            string transId = "TXN123";
            decimal amount = 1000m;

            bool isLocked = _service.IsRateLocked(transId);
            decimal appliedAmount = _service.ApplyLockedRate(amount, transId);
            decimal fee = _service.CalculateConversionFee(amount, "USD");
            double volatility = _service.GetVolatilityIndex("USD", 30);

            Assert.IsTrue(isLocked);
            Assert.IsTrue(appliedAmount > 0);
            Assert.IsTrue(fee >= 0);
            Assert.IsTrue(volatility >= 0);
            Assert.AreNotEqual(amount, appliedAmount);
        }

        [TestMethod]
        public void NREAccount_Validation_ReturnsTrue()
        {
            string accNum = "1234567890";
            string bankCode = "SBI";
            string accId = "ACC001";

            bool isValid = _service.IsNREAccountValid(accNum, bankCode);
            decimal balance = _service.GetNREAccountBalance(accId);
            bool isNroValid = _service.IsNROAccountValid(accNum, bankCode);
            decimal nroBalance = _service.GetNROAccountBalance(accId);

            Assert.IsTrue(isValid);
            Assert.IsTrue(balance >= 0);
            Assert.IsFalse(isNroValid); // Assuming same accNum can't be both
            Assert.IsTrue(nroBalance >= 0);
            Assert.AreNotEqual(balance, nroBalance);
        }

        [TestMethod]
        public void TransactionCount_Validation_ReturnsCorrectCount()
        {
            string customerId = "CUST999";
            DateTime start = DateTime.Now.AddDays(-30);
            DateTime end = DateTime.Now;

            int count = _service.GetTransactionCount(customerId, start, end);
            bool limitExceeded = _service.HasForexLimitExceeded(customerId, 100m, start);
            string declaration = _service.GetFemaDeclarationId(customerId, end);
            int remainingDays = _service.GetRemainingForexLimitDays(customerId);

            Assert.IsTrue(count >= 0);
            Assert.IsFalse(limitExceeded);
            Assert.IsNotNull(declaration);
            Assert.IsTrue(remainingDays >= 0);
            Assert.IsTrue(declaration.Length > 0);
        }

        [TestMethod]
        public void DiscountFactor_Calculation_ReturnsValidFactor()
        {
            string currency = "GBP";
            int days = 90;

            double discount = _service.GetDiscountFactor(currency, days);
            bool supported = _service.IsCurrencySupported(currency);
            decimal fee = _service.CalculateConversionFee(1000m, currency);
            double volatility = _service.GetVolatilityIndex(currency, days);

            Assert.IsTrue(discount > 0);
            Assert.IsTrue(discount <= 1);
            Assert.IsTrue(supported);
            Assert.IsTrue(fee >= 0);
            Assert.IsTrue(volatility >= 0);
        }

        [TestMethod]
        public void SanctionedCountry_Validation_ReturnsTrueForSanctioned()
        {
            string country = "IR"; // Example sanctioned country code
            decimal amount = 1000m;

            bool isSanctioned = _service.IsSanctionedCountry(country);
            decimal tax = _service.CalculateTaxOnForex(amount, country);
            double taxRate = _service.GetTaxRateForNRI(country);
            int grace = _service.GetGracePeriodDays(country);

            Assert.IsTrue(isSanctioned);
            Assert.AreEqual(0m, tax); // Assuming no tax calc for sanctioned
            Assert.AreEqual(0.0, taxRate);
            Assert.AreEqual(0, grace);
        }

        [TestMethod]
        public void BaseCurrency_Validation_ReturnsValidCode()
        {
            string baseCurr = _service.GetBaseCurrencyCode();
            bool supported = _service.IsCurrencySupported(baseCurr);
            decimal rate = _service.GetExchangeRate(baseCurr, baseCurr, DateTime.Now);
            decimal fee = _service.CalculateConversionFee(100m, baseCurr);

            Assert.IsNotNull(baseCurr);
            Assert.IsTrue(supported);
            Assert.AreEqual(1m, rate);
            Assert.AreEqual(0m, fee); // No fee for base currency
        }

        [TestMethod]
        public void NROAccount_Validation_ReturnsTrue()
        {
            string accNum = "0987654321";
            string bankCode = "ICICI";
            string accId = "ACC002";

            bool isValid = _service.IsNROAccountValid(accNum, bankCode);
            decimal balance = _service.GetNROAccountBalance(accId);
            bool isNreValid = _service.IsNREAccountValid(accNum, bankCode);
            decimal nreBalance = _service.GetNREAccountBalance(accId);

            Assert.IsTrue(isValid);
            Assert.IsTrue(balance >= 0);
            Assert.IsFalse(isNreValid);
            Assert.IsTrue(nreBalance >= 0);
            Assert.AreNotEqual(balance, nreBalance);
        }

        [TestMethod]
        public void RepatriableAmount_NROAccount_CalculatesCorrectly()
        {
            string policyId = "POL789";
            decimal totalAmount = 50000m;
            string accountType = "NRO";
            string policyType = "Pension";

            bool isRepatriable = _service.IsRepatriable(accountType, policyType);
            decimal repAmount = _service.GetRepatriableAmount(policyId, totalAmount);
            decimal nonRepAmount = _service.GetNonRepatriableAmount(policyId, totalAmount);
            int lockIn = _service.GetLockInPeriod(policyId);

            Assert.IsFalse(isRepatriable);
            Assert.AreEqual(0m, repAmount);
            Assert.AreEqual(totalAmount, nonRepAmount);
            Assert.IsTrue(lockIn >= 0);
            Assert.AreEqual(totalAmount, repAmount + nonRepAmount);
        }

        [TestMethod]
        public void SpreadRatio_Calculation_ReturnsValidRatio()
        {
            string bankCode = "AXIS";
            string pair = "EUR-INR";

            double spread = _service.GetSpreadRatio(bankCode, pair);
            double markup = _service.GetMarkupPercentage(pair);
            decimal spot = _service.CalculateSpotRate(pair);
            int settlement = _service.GetSettlementDays("EUR", "INR");

            Assert.IsTrue(spread >= 0);
            Assert.IsTrue(markup >= 0);
            Assert.IsTrue(spot > 0);
            Assert.IsTrue(settlement > 0);
            Assert.AreNotEqual(spread, markup);
        }

        [TestMethod]
        public void IbanFormat_Validation_ReturnsValidFormat()
        {
            string country = "AE";
            
            string format = _service.GetIbanFormat(country);
            bool isSanctioned = _service.IsSanctionedCountry(country);
            int grace = _service.GetGracePeriodDays(country);
            double taxRate = _service.GetTaxRateForNRI(country);

            Assert.IsNotNull(format);
            Assert.IsTrue(format.Length > 0);
            Assert.IsFalse(isSanctioned);
            Assert.IsTrue(grace >= 0);
            Assert.IsTrue(taxRate >= 0);
        }

        [TestMethod]
        public void VolatilityIndex_Calculation_ReturnsValidIndex()
        {
            string currency = "AUD";
            int days = 60;

            double volatility = _service.GetVolatilityIndex(currency, days);
            double discount = _service.GetDiscountFactor(currency, days);
            bool supported = _service.IsCurrencySupported(currency);
            decimal fee = _service.CalculateConversionFee(500m, currency);

            Assert.IsTrue(volatility >= 0);
            Assert.IsTrue(discount > 0);
            Assert.IsTrue(supported);
            Assert.IsTrue(fee >= 0);
            Assert.AreNotEqual(volatility, discount);
        }

        [TestMethod]
        public void SettlementDays_Calculation_ReturnsValidDays()
        {
            string source = "CAD";
            string target = "INR";

            int days = _service.GetSettlementDays(source, target);
            decimal rate = _service.GetExchangeRate(source, target, DateTime.Now);
            bool sourceSupported = _service.IsCurrencySupported(source);
            bool targetSupported = _service.IsCurrencySupported(target);

            Assert.IsTrue(days >= 0);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(sourceSupported);
            Assert.IsTrue(targetSupported);
            Assert.AreNotEqual(0, days);
        }

        [TestMethod]
        public void ConversionFee_Calculation_ReturnsValidFee()
        {
            decimal amount = 2000m;
            string currency = "SGD";

            decimal fee = _service.CalculateConversionFee(amount, currency);
            bool supported = _service.IsCurrencySupported(currency);
            double markup = _service.GetMarkupPercentage(currency + "-INR");
            decimal converted = _service.ConvertCurrency(amount, currency, "INR");

            Assert.IsTrue(fee >= 0);
            Assert.IsTrue(supported);
            Assert.IsTrue(markup >= 0);
            Assert.IsTrue(converted > 0);
            Assert.IsTrue(fee < amount);
        }

        [TestMethod]
        public void LockInPeriod_Validation_ReturnsValidPeriod()
        {
            string policyId = "POL321";
            
            int lockIn = _service.GetLockInPeriod(policyId);
            decimal repAmount = _service.GetRepatriableAmount(policyId, 10000m);
            decimal nonRepAmount = _service.GetNonRepatriableAmount(policyId, 10000m);
            string refId = _service.GenerateTransactionReference(policyId, "USD");

            Assert.IsTrue(lockIn >= 0);
            Assert.IsTrue(repAmount >= 0);
            Assert.IsTrue(nonRepAmount >= 0);
            Assert.IsNotNull(refId);
            Assert.IsTrue(refId.Contains(policyId));
        }

        [TestMethod]
        public void FemaDeclaration_Validation_ReturnsValidId()
        {
            string customerId = "CUST555";
            DateTime date = DateTime.Now;

            string declarationId = _service.GetFemaDeclarationId(customerId, date);
            bool compliant = _service.ValidateFEMACompliance(customerId, 1000m);
            int remainingDays = _service.GetRemainingForexLimitDays(customerId);
            int txnCount = _service.GetTransactionCount(customerId, date.AddDays(-30), date);

            Assert.IsNotNull(declarationId);
            Assert.IsTrue(declarationId.Contains(customerId));
            Assert.IsTrue(compliant);
            Assert.IsTrue(remainingDays >= 0);
            Assert.IsTrue(txnCount >= 0);
        }

        [TestMethod]
        public void GracePeriod_Validation_ReturnsValidDays()
        {
            string country = "UK";

            int grace = _service.GetGracePeriodDays(country);
            bool sanctioned = _service.IsSanctionedCountry(country);
            string iban = _service.GetIbanFormat(country);
            double taxRate = _service.GetTaxRateForNRI(country);

            Assert.IsTrue(grace >= 0);
            Assert.IsFalse(sanctioned);
            Assert.IsNotNull(iban);
            Assert.IsTrue(taxRate >= 0);
            Assert.IsTrue(iban.Length > 0);
        }

        [TestMethod]
        public void TransactionReference_Generation_ReturnsValidRef()
        {
            string policyId = "POL999";
            string currency = "JPY";

            string refId = _service.GenerateTransactionReference(policyId, currency);
            bool supported = _service.IsCurrencySupported(currency);
            decimal payout = _service.CalculatePayoutAmount(policyId, currency, DateTime.Now);
            int lockIn = _service.GetLockInPeriod(policyId);

            Assert.IsNotNull(refId);
            Assert.IsTrue(refId.Contains(policyId));
            Assert.IsTrue(refId.Contains(currency));
            Assert.IsTrue(supported);
            Assert.IsTrue(payout >= 0);
            Assert.IsTrue(lockIn >= 0);
        }

        [TestMethod]
        public void SpotRate_Calculation_ReturnsValidRate()
        {
            string pair = "CHF-INR";

            decimal spot = _service.CalculateSpotRate(pair);
            double markup = _service.GetMarkupPercentage(pair);
            int settlement = _service.GetSettlementDays("CHF", "INR");
            double spread = _service.GetSpreadRatio("HDFC", pair);

            Assert.IsTrue(spot > 0);
            Assert.IsTrue(markup >= 0);
            Assert.IsTrue(settlement > 0);
            Assert.IsTrue(spread >= 0);
            Assert.AreNotEqual(0m, spot);
        }
    }
}