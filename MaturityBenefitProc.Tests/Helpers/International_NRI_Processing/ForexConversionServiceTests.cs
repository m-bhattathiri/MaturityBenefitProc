using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.InternationalAndNRIProcessing;

namespace MaturityBenefitProc.Tests.Helpers.InternationalAndNRIProcessing
{
    [TestClass]
    public class ForexConversionServiceTests
    {
        private IForexConversionService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming ForexConversionService implements IForexConversionService
            _service = new ForexConversionService();
        }

        [TestMethod]
        public void ConvertCurrency_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.ConvertCurrency(100m, "USD", "INR");
            var result2 = _service.ConvertCurrency(50m, "EUR", "USD");
            var result3 = _service.ConvertCurrency(0m, "GBP", "INR");
            var result4 = _service.ConvertCurrency(1000m, "INR", "INR");

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(1000m, result4);
        }

        [TestMethod]
        public void CalculatePayoutAmount_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculatePayoutAmount("POL123", "USD", DateTime.Now);
            var result2 = _service.CalculatePayoutAmount("POL456", "EUR", DateTime.Now.AddDays(-1));
            var result3 = _service.CalculatePayoutAmount("POL789", "GBP", DateTime.Now.AddDays(1));
            var result4 = _service.CalculatePayoutAmount("POL000", "INR", DateTime.MinValue);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
        }

        [TestMethod]
        public void GetExchangeRate_ValidInputs_ReturnsRate()
        {
            var result1 = _service.GetExchangeRate("USD", "INR", DateTime.Now);
            var result2 = _service.GetExchangeRate("EUR", "USD", DateTime.Now);
            var result3 = _service.GetExchangeRate("GBP", "INR", DateTime.Now.AddDays(-5));
            var result4 = _service.GetExchangeRate("INR", "INR", DateTime.Now);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreEqual(1m, result4);
        }

        [TestMethod]
        public void IsCurrencySupported_VariousCurrencies_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsCurrencySupported("USD");
            var result2 = _service.IsCurrencySupported("INR");
            var result3 = _service.IsCurrencySupported("XYZ");
            var result4 = _service.IsCurrencySupported("");
            var result5 = _service.IsCurrencySupported(null);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.IsFalse(result5);
        }

        [TestMethod]
        public void ValidateFEMACompliance_VariousInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.ValidateFEMACompliance("CUST123", 1000m);
            var result2 = _service.ValidateFEMACompliance("CUST456", 500000m);
            var result3 = _service.ValidateFEMACompliance("CUST789", 0m);
            var result4 = _service.ValidateFEMACompliance("", 100m);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void GenerateTransactionReference_ValidInputs_ReturnsReferenceString()
        {
            var result1 = _service.GenerateTransactionReference("POL123", "USD");
            var result2 = _service.GenerateTransactionReference("POL456", "EUR");
            var result3 = _service.GenerateTransactionReference("", "GBP");
            var result4 = _service.GenerateTransactionReference("POL789", "");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNull(result3);
            Assert.IsNull(result4);
            Assert.IsTrue(result1.Contains("POL123"));
        }

        [TestMethod]
        public void GetSettlementDays_VariousPairs_ReturnsExpectedDays()
        {
            var result1 = _service.GetSettlementDays("USD", "INR");
            var result2 = _service.GetSettlementDays("EUR", "USD");
            var result3 = _service.GetSettlementDays("GBP", "JPY");
            var result4 = _service.GetSettlementDays("INR", "INR");

            Assert.AreEqual(2, result1);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(3, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetMarkupPercentage_VariousPairs_ReturnsExpectedPercentage()
        {
            var result1 = _service.GetMarkupPercentage("USD/INR");
            var result2 = _service.GetMarkupPercentage("EUR/USD");
            var result3 = _service.GetMarkupPercentage("GBP/INR");
            var result4 = _service.GetMarkupPercentage("INR/INR");

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void CalculateTaxOnForex_ValidInputs_ReturnsTaxAmount()
        {
            var result1 = _service.CalculateTaxOnForex(1000m, "IN");
            var result2 = _service.CalculateTaxOnForex(5000m, "US");
            var result3 = _service.CalculateTaxOnForex(0m, "UK");
            var result4 = _service.CalculateTaxOnForex(100m, "");

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void IsRepatriable_VariousTypes_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsRepatriable("NRE", "Life");
            var result2 = _service.IsRepatriable("NRO", "Life");
            var result3 = _service.IsRepatriable("NRE", "Pension");
            var result4 = _service.IsRepatriable("", "Life");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void GetRepatriableAmount_ValidInputs_ReturnsAmount()
        {
            var result1 = _service.GetRepatriableAmount("POL123", 10000m);
            var result2 = _service.GetRepatriableAmount("POL456", 5000m);
            var result3 = _service.GetRepatriableAmount("POL789", 0m);
            var result4 = _service.GetRepatriableAmount("", 1000m);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetNonRepatriableAmount_ValidInputs_ReturnsAmount()
        {
            var result1 = _service.GetNonRepatriableAmount("POL123", 10000m);
            var result2 = _service.GetNonRepatriableAmount("POL456", 5000m);
            var result3 = _service.GetNonRepatriableAmount("POL789", 0m);
            var result4 = _service.GetNonRepatriableAmount("", 1000m);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetBaseCurrencyCode_ReturnsExpectedCode()
        {
            var result1 = _service.GetBaseCurrencyCode();

            Assert.IsNotNull(result1);
            Assert.AreEqual("INR", result1);
            Assert.AreNotEqual("USD", result1);
            Assert.AreNotEqual("", result1);
        }

        [TestMethod]
        public void GetTaxRateForNRI_VariousCountries_ReturnsExpectedRate()
        {
            var result1 = _service.GetTaxRateForNRI("US");
            var result2 = _service.GetTaxRateForNRI("UK");
            var result3 = _service.GetTaxRateForNRI("AE");
            var result4 = _service.GetTaxRateForNRI("");

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void IsNREAccountValid_VariousInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsNREAccountValid("1234567890", "HDFC");
            var result2 = _service.IsNREAccountValid("0987654321", "ICICI");
            var result3 = _service.IsNREAccountValid("", "SBI");
            var result4 = _service.IsNREAccountValid("12345", "");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsNROAccountValid_VariousInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsNROAccountValid("1234567890", "HDFC");
            var result2 = _service.IsNROAccountValid("0987654321", "ICICI");
            var result3 = _service.IsNROAccountValid("", "SBI");
            var result4 = _service.IsNROAccountValid("12345", "");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void GetLockInPeriod_VariousPolicies_ReturnsExpectedDays()
        {
            var result1 = _service.GetLockInPeriod("POL123");
            var result2 = _service.GetLockInPeriod("POL456");
            var result3 = _service.GetLockInPeriod("");
            var result4 = _service.GetLockInPeriod(null);

            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void CalculateConversionFee_VariousInputs_ReturnsExpectedFee()
        {
            var result1 = _service.CalculateConversionFee(1000m, "USD");
            var result2 = _service.CalculateConversionFee(5000m, "EUR");
            var result3 = _service.CalculateConversionFee(0m, "GBP");
            var result4 = _service.CalculateConversionFee(100m, "");

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetSwiftCode_VariousInputs_ReturnsExpectedCode()
        {
            var result1 = _service.GetSwiftCode("HDFC", "MUM");
            var result2 = _service.GetSwiftCode("ICICI", "DEL");
            var result3 = _service.GetSwiftCode("", "MUM");
            var result4 = _service.GetSwiftCode("SBI", "");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNull(result3);
            Assert.IsNull(result4);
        }

        [TestMethod]
        public void ValidateSwiftCode_VariousInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.ValidateSwiftCode("HDFCINBBXXX");
            var result2 = _service.ValidateSwiftCode("ICICINBBXXX");
            var result3 = _service.ValidateSwiftCode("INVALID");
            var result4 = _service.ValidateSwiftCode("");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void GetVolatilityIndex_VariousInputs_ReturnsExpectedIndex()
        {
            var result1 = _service.GetVolatilityIndex("USD", 30);
            var result2 = _service.GetVolatilityIndex("EUR", 60);
            var result3 = _service.GetVolatilityIndex("GBP", 0);
            var result4 = _service.GetVolatilityIndex("", 30);

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void HasForexLimitExceeded_VariousInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.HasForexLimitExceeded("CUST123", 10000m, DateTime.Now.AddYears(-1));
            var result2 = _service.HasForexLimitExceeded("CUST456", 500000m, DateTime.Now.AddYears(-1));
            var result3 = _service.HasForexLimitExceeded("CUST789", 0m, DateTime.Now);
            var result4 = _service.HasForexLimitExceeded("", 100m, DateTime.Now);

            Assert.IsFalse(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void GetTransactionCount_VariousInputs_ReturnsExpectedCount()
        {
            var result1 = _service.GetTransactionCount("CUST123", DateTime.Now.AddDays(-30), DateTime.Now);
            var result2 = _service.GetTransactionCount("CUST456", DateTime.Now.AddDays(-60), DateTime.Now);
            var result3 = _service.GetTransactionCount("CUST789", DateTime.Now, DateTime.Now.AddDays(-1));
            var result4 = _service.GetTransactionCount("", DateTime.Now.AddDays(-30), DateTime.Now);

            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void CalculateForwardRate_VariousInputs_ReturnsExpectedRate()
        {
            var result1 = _service.CalculateForwardRate("USD/INR", 30, 0.05);
            var result2 = _service.CalculateForwardRate("EUR/USD", 60, 0.02);
            var result3 = _service.CalculateForwardRate("GBP/INR", 0, 0.05);
            var result4 = _service.CalculateForwardRate("", 30, 0.05);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateSpotRate_VariousInputs_ReturnsExpectedRate()
        {
            var result1 = _service.CalculateSpotRate("USD/INR");
            var result2 = _service.CalculateSpotRate("EUR/USD");
            var result3 = _service.CalculateSpotRate("GBP/INR");
            var result4 = _service.CalculateSpotRate("");

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void IsSanctionedCountry_VariousInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsSanctionedCountry("IR");
            var result2 = _service.IsSanctionedCountry("KP");
            var result3 = _service.IsSanctionedCountry("US");
            var result4 = _service.IsSanctionedCountry("");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void GetIbanFormat_VariousInputs_ReturnsExpectedFormat()
        {
            var result1 = _service.GetIbanFormat("GB");
            var result2 = _service.GetIbanFormat("DE");
            var result3 = _service.GetIbanFormat("US");
            var result4 = _service.GetIbanFormat("");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNull(result3);
            Assert.IsNull(result4);
        }

        [TestMethod]
        public void GetRemainingForexLimitDays_VariousInputs_ReturnsExpectedDays()
        {
            var result1 = _service.GetRemainingForexLimitDays("CUST123");
            var result2 = _service.GetRemainingForexLimitDays("CUST456");
            var result3 = _service.GetRemainingForexLimitDays("");
            var result4 = _service.GetRemainingForexLimitDays(null);

            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetDiscountFactor_VariousInputs_ReturnsExpectedFactor()
        {
            var result1 = _service.GetDiscountFactor("USD", 30);
            var result2 = _service.GetDiscountFactor("EUR", 60);
            var result3 = _service.GetDiscountFactor("GBP", 0);
            var result4 = _service.GetDiscountFactor("", 30);

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreEqual(1.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetFemaDeclarationId_VariousInputs_ReturnsExpectedId()
        {
            var result1 = _service.GetFemaDeclarationId("CUST123", DateTime.Now);
            var result2 = _service.GetFemaDeclarationId("CUST456", DateTime.Now.AddDays(-1));
            var result3 = _service.GetFemaDeclarationId("", DateTime.Now);
            var result4 = _service.GetFemaDeclarationId("CUST789", DateTime.MinValue);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNull(result3);
            Assert.IsNull(result4);
        }

        [TestMethod]
        public void IsRateLocked_VariousInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsRateLocked("TXN123");
            var result2 = _service.IsRateLocked("TXN456");
            var result3 = _service.IsRateLocked("");
            var result4 = _service.IsRateLocked(null);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void ApplyLockedRate_VariousInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.ApplyLockedRate(1000m, "TXN123");
            var result2 = _service.ApplyLockedRate(5000m, "TXN456");
            var result3 = _service.ApplyLockedRate(0m, "TXN789");
            var result4 = _service.ApplyLockedRate(100m, "");

            Assert.AreNotEqual(0m, result1);
            Assert.AreEqual(5000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(100m, result4);
        }
    }
}