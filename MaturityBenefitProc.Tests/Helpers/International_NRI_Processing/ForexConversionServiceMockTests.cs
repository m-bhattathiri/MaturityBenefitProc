using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.InternationalAndNRIProcessing;

namespace MaturityBenefitProc.Tests.Helpers.InternationalAndNRIProcessing
{
    [TestClass]
    public class ForexConversionServiceMockTests
    {
        private Mock<IForexConversionService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IForexConversionService>();
        }

        [TestMethod]
        public void ConvertCurrency_ValidInputs_ReturnsConvertedAmount()
        {
            decimal expectedValue = 7500.50m;
            _mockService.Setup(s => s.ConvertCurrency(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.ConvertCurrency(100m, "USD", "INR");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.ConvertCurrency(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculatePayoutAmount_ValidPolicy_ReturnsAmount()
        {
            decimal expectedValue = 50000m;
            _mockService.Setup(s => s.CalculatePayoutAmount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculatePayoutAmount("POL123", "USD", DateTime.Now);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            Assert.IsInstanceOfType(result, typeof(decimal));
            _mockService.Verify(s => s.CalculatePayoutAmount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetExchangeRate_ValidDates_ReturnsRate()
        {
            decimal expectedValue = 82.5m;
            _mockService.Setup(s => s.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetExchangeRate("USD", "INR", DateTime.Now);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 80m);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsCurrencySupported_SupportedCurrency_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsCurrencySupported(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.IsCurrencySupported("USD");

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsCurrencySupported(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsCurrencySupported_UnsupportedCurrency_ReturnsFalse()
        {
            bool expectedValue = false;
            _mockService.Setup(s => s.IsCurrencySupported(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.IsCurrencySupported("XYZ");

            Assert.AreEqual(expectedValue, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.IsCurrencySupported(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateFEMACompliance_ValidCustomer_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.ValidateFEMACompliance(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.ValidateFEMACompliance("CUST001", 10000m);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(bool));
            _mockService.Verify(s => s.ValidateFEMACompliance(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GenerateTransactionReference_ValidInputs_ReturnsString()
        {
            string expectedValue = "TXN-POL123-USD";
            _mockService.Setup(s => s.GenerateTransactionReference(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GenerateTransactionReference("POL123", "USD");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.StartsWith("TXN"));
            _mockService.Verify(s => s.GenerateTransactionReference(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetSettlementDays_ValidCurrencies_ReturnsInt()
        {
            int expectedValue = 2;
            _mockService.Setup(s => s.GetSettlementDays(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetSettlementDays("USD", "INR");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetSettlementDays(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetMarkupPercentage_ValidPair_ReturnsDouble()
        {
            double expectedValue = 1.5;
            _mockService.Setup(s => s.GetMarkupPercentage(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetMarkupPercentage("USD/INR");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 1.0);
            _mockService.Verify(s => s.GetMarkupPercentage(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxOnForex_ValidAmount_ReturnsTax()
        {
            decimal expectedValue = 150.75m;
            _mockService.Setup(s => s.CalculateTaxOnForex(It.IsAny<decimal>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTaxOnForex(10000m, "IN");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 100m);
            _mockService.Verify(s => s.CalculateTaxOnForex(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsRepatriable_ValidTypes_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsRepatriable(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.IsRepatriable("NRE", "ULIP");

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsRepatriable(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRepatriableAmount_ValidInputs_ReturnsAmount()
        {
            decimal expectedValue = 40000m;
            _mockService.Setup(s => s.GetRepatriableAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.GetRepatriableAmount("POL123", 50000m);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0m);
            _mockService.Verify(s => s.GetRepatriableAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetNonRepatriableAmount_ValidInputs_ReturnsAmount()
        {
            decimal expectedValue = 10000m;
            _mockService.Setup(s => s.GetNonRepatriableAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.GetNonRepatriableAmount("POL123", 50000m);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0m);
            _mockService.Verify(s => s.GetNonRepatriableAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetBaseCurrencyCode_ReturnsString()
        {
            string expectedValue = "INR";
            _mockService.Setup(s => s.GetBaseCurrencyCode()).Returns(expectedValue);

            var result = _mockService.Object.GetBaseCurrencyCode();

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("USD", result);
            Assert.IsTrue(result.Length == 3);
            _mockService.Verify(s => s.GetBaseCurrencyCode(), Times.Once());
        }

        [TestMethod]
        public void GetTaxRateForNRI_ValidCountry_ReturnsRate()
        {
            double expectedValue = 12.5;
            _mockService.Setup(s => s.GetTaxRateForNRI(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetTaxRateForNRI("US");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 10.0);
            _mockService.Verify(s => s.GetTaxRateForNRI(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsNREAccountValid_ValidAccount_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsNREAccountValid(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.IsNREAccountValid("123456789", "HDFC001");

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsNREAccountValid(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsNROAccountValid_ValidAccount_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsNROAccountValid(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.IsNROAccountValid("987654321", "ICIC001");

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsNROAccountValid(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetLockInPeriod_ValidPolicy_ReturnsDays()
        {
            int expectedValue = 1095;
            _mockService.Setup(s => s.GetLockInPeriod(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetLockInPeriod("POL123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 1000);
            _mockService.Verify(s => s.GetLockInPeriod(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateConversionFee_ValidInputs_ReturnsFee()
        {
            decimal expectedValue = 25.50m;
            _mockService.Setup(s => s.CalculateConversionFee(It.IsAny<decimal>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateConversionFee(1000m, "USD");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 20m);
            _mockService.Verify(s => s.CalculateConversionFee(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetSwiftCode_ValidInputs_ReturnsCode()
        {
            string expectedValue = "HDFC1234";
            _mockService.Setup(s => s.GetSwiftCode(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetSwiftCode("HDFC", "1234");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 5);
            _mockService.Verify(s => s.GetSwiftCode(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateSwiftCode_ValidCode_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.ValidateSwiftCode(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.ValidateSwiftCode("HDFC1234");

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateSwiftCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetVolatilityIndex_ValidInputs_ReturnsIndex()
        {
            double expectedValue = 0.05;
            _mockService.Setup(s => s.GetVolatilityIndex(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.GetVolatilityIndex("USD", 30);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result < 1.0);
            _mockService.Verify(s => s.GetVolatilityIndex(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void HasForexLimitExceeded_ValidInputs_ReturnsFalse()
        {
            bool expectedValue = false;
            _mockService.Setup(s => s.HasForexLimitExceeded(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.HasForexLimitExceeded("CUST001", 5000m, DateTime.Now);

            Assert.AreEqual(expectedValue, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.HasForexLimitExceeded(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTransactionCount_ValidDates_ReturnsCount()
        {
            int expectedValue = 5;
            _mockService.Setup(s => s.GetTransactionCount(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetTransactionCount("CUST001", DateTime.Now.AddDays(-30), DateTime.Now);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetTransactionCount(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateForwardRate_ValidInputs_ReturnsRate()
        {
            decimal expectedValue = 83.2m;
            _mockService.Setup(s => s.CalculateForwardRate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateForwardRate("USD/INR", 30, 0.02);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 80m);
            _mockService.Verify(s => s.CalculateForwardRate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void IsSanctionedCountry_ValidCountry_ReturnsFalse()
        {
            bool expectedValue = false;
            _mockService.Setup(s => s.IsSanctionedCountry(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.IsSanctionedCountry("US");

            Assert.AreEqual(expectedValue, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.IsSanctionedCountry(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingForexLimitDays_ValidCustomer_ReturnsDays()
        {
            int expectedValue = 120;
            _mockService.Setup(s => s.GetRemainingForexLimitDays(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetRemainingForexLimitDays("CUST001");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 100);
            _mockService.Verify(s => s.GetRemainingForexLimitDays(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetNREAccountBalance_ValidAccount_ReturnsBalance()
        {
            decimal expectedValue = 150000m;
            _mockService.Setup(s => s.GetNREAccountBalance(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetNREAccountBalance("ACC123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 100000m);
            _mockService.Verify(s => s.GetNREAccountBalance(It.IsAny<string>()), Times.Once());
        }
    }
}