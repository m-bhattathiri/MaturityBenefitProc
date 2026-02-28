using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.Fund_Value_Unit_Linked_Plans;

namespace MaturityBenefitProc.Tests.Helpers.Fund_Value_Unit_Linked_Plans
{
    [TestClass]
    public class UlipNavCalculationServiceMockTests
    {
        private Mock<IUlipNavCalculationService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IUlipNavCalculationService>();
        }

        [TestMethod]
        public void GetNavForDate_ValidInputs_ReturnsExpectedNav()
        {
            string fundCode = "FND001";
            DateTime date = new DateTime(2023, 10, 1);
            decimal expectedNav = 15.5m;

            _mockService.Setup(s => s.GetNavForDate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedNav);

            var result = _mockService.Object.GetNavForDate(fundCode, date);

            Assert.AreEqual(expectedNav, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetNavForDate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateFundValue_ValidPolicy_ReturnsCalculatedValue()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 10, 1);
            decimal expectedValue = 50000m;

            _mockService.Setup(s => s.CalculateFundValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateFundValue(policyId, date);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            Assert.IsTrue(result == 50000m);

            _mockService.Verify(s => s.CalculateFundValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsNavAvailable_DateHasNav_ReturnsTrue()
        {
            string fundCode = "FND002";
            DateTime date = new DateTime(2023, 10, 1);

            _mockService.Setup(s => s.IsNavAvailable(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.IsNavAvailable(fundCode, date);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.IsNavAvailable(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetNavDelayDays_ValidFund_ReturnsDays()
        {
            string fundCode = "FND003";
            int expectedDays = 2;

            _mockService.Setup(s => s.GetNavDelayDays(It.IsAny<string>())).Returns(expectedDays);

            var result = _mockService.Object.GetNavDelayDays(fundCode);

            Assert.AreEqual(expectedDays, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetNavDelayDays(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetFallbackFundCode_HasFallback_ReturnsFallbackCode()
        {
            string primaryCode = "FND004";
            string expectedFallback = "FND004_FB";

            _mockService.Setup(s => s.GetFallbackFundCode(It.IsAny<string>())).Returns(expectedFallback);

            var result = _mockService.Object.GetFallbackFundCode(primaryCode);

            Assert.AreEqual(expectedFallback, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(primaryCode, result);
            Assert.IsTrue(result.Contains("_FB"));

            _mockService.Verify(s => s.GetFallbackFundCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetFundAllocationRatio_ValidInputs_ReturnsRatio()
        {
            string policyId = "POL123";
            string fundCode = "FND001";
            double expectedRatio = 0.6;

            _mockService.Setup(s => s.GetFundAllocationRatio(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedRatio);

            var result = _mockService.Object.GetFundAllocationRatio(policyId, fundCode);

            Assert.AreEqual(expectedRatio, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result <= 1.0);

            _mockService.Verify(s => s.GetFundAllocationRatio(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalAllocatedUnits_ValidInputs_ReturnsUnits()
        {
            string policyId = "POL123";
            string fundCode = "FND001";
            decimal expectedUnits = 1500.5m;

            _mockService.Setup(s => s.GetTotalAllocatedUnits(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedUnits);

            var result = _mockService.Object.GetTotalAllocatedUnits(policyId, fundCode);

            Assert.AreEqual(expectedUnits, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 1000m);

            _mockService.Verify(s => s.GetTotalAllocatedUnits(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateUnitValue_ValidInputs_ReturnsValue()
        {
            decimal units = 100m;
            decimal nav = 15m;
            decimal expectedValue = 1500m;

            _mockService.Setup(s => s.CalculateUnitValue(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateUnitValue(units, nav);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result == 1500m);

            _mockService.Verify(s => s.CalculateUnitValue(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void ValidateFundStatus_ActiveFund_ReturnsTrue()
        {
            string fundCode = "FND001";

            _mockService.Setup(s => s.ValidateFundStatus(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.ValidateFundStatus(fundCode);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.ValidateFundStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetHistoricalNav_ValidInputs_ReturnsNav()
        {
            string fundCode = "FND001";
            DateTime date = new DateTime(2020, 1, 1);
            decimal expectedNav = 10.5m;

            _mockService.Setup(s => s.GetHistoricalNav(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedNav);

            var result = _mockService.Object.GetHistoricalNav(fundCode, date);

            Assert.AreEqual(expectedNav, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0m);

            _mockService.Verify(s => s.GetHistoricalNav(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateNavGrowthRate_ValidInputs_ReturnsRate()
        {
            string fundCode = "FND001";
            DateTime start = new DateTime(2020, 1, 1);
            DateTime end = new DateTime(2023, 1, 1);
            double expectedRate = 0.15;

            _mockService.Setup(s => s.CalculateNavGrowthRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedRate);

            var result = _mockService.Object.CalculateNavGrowthRate(fundCode, start, end);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0.0);

            _mockService.Verify(s => s.CalculateNavGrowthRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalActiveFunds_ValidPolicy_ReturnsCount()
        {
            string policyId = "POL123";
            int expectedCount = 3;

            _mockService.Setup(s => s.GetTotalActiveFunds(It.IsAny<string>())).Returns(expectedCount);

            var result = _mockService.Object.GetTotalActiveFunds(policyId);

            Assert.AreEqual(expectedCount, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetTotalActiveFunds(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetDominantFundCode_ValidPolicy_ReturnsFundCode()
        {
            string policyId = "POL123";
            string expectedFund = "FND001";

            _mockService.Setup(s => s.GetDominantFundCode(It.IsAny<string>())).Returns(expectedFund);

            var result = _mockService.Object.GetDominantFundCode(policyId);

            Assert.AreEqual(expectedFund, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.StartsWith("FND"));

            _mockService.Verify(s => s.GetDominantFundCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalFundValue_ValidInputs_ReturnsTotal()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 10, 1);
            decimal expectedTotal = 150000m;

            _mockService.Setup(s => s.CalculateTotalFundValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedTotal);

            var result = _mockService.Object.CalculateTotalFundValue(policyId, date);

            Assert.AreEqual(expectedTotal, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 100000m);

            _mockService.Verify(s => s.CalculateTotalFundValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForNavAdjustment_EligiblePolicy_ReturnsTrue()
        {
            string policyId = "POL123";

            _mockService.Setup(s => s.IsPolicyEligibleForNavAdjustment(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.IsPolicyEligibleForNavAdjustment(policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.IsPolicyEligibleForNavAdjustment(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ApplyNavAdjustment_ValidInputs_ReturnsAdjustedNav()
        {
            string policyId = "POL123";
            decimal baseNav = 10m;
            double rate = 1.05;
            decimal expectedNav = 10.5m;

            _mockService.Setup(s => s.ApplyNavAdjustment(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedNav);

            var result = _mockService.Object.ApplyNavAdjustment(policyId, baseNav, rate);

            Assert.AreEqual(expectedNav, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(baseNav, result);
            Assert.IsTrue(result > baseNav);

            _mockService.Verify(s => s.ApplyNavAdjustment(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetUnitsPrecision_ValidFund_ReturnsPrecision()
        {
            string fundCode = "FND001";
            int expectedPrecision = 4;

            _mockService.Setup(s => s.GetUnitsPrecision(It.IsAny<string>())).Returns(expectedPrecision);

            var result = _mockService.Object.GetUnitsPrecision(fundCode);

            Assert.AreEqual(expectedPrecision, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetUnitsPrecision(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RoundUnits_ValidInputs_ReturnsRoundedUnits()
        {
            decimal rawUnits = 100.12345m;
            int precision = 4;
            decimal expectedUnits = 100.1235m;

            _mockService.Setup(s => s.RoundUnits(It.IsAny<decimal>(), It.IsAny<int>())).Returns(expectedUnits);

            var result = _mockService.Object.RoundUnits(rawUnits, precision);

            Assert.AreEqual(expectedUnits, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(rawUnits, result);
            Assert.IsTrue(result == 100.1235m);

            _mockService.Verify(s => s.RoundUnits(It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetNavCurrency_ValidFund_ReturnsCurrency()
        {
            string fundCode = "FND001";
            string expectedCurrency = "USD";

            _mockService.Setup(s => s.GetNavCurrency(It.IsAny<string>())).Returns(expectedCurrency);

            var result = _mockService.Object.GetNavCurrency(fundCode);

            Assert.AreEqual(expectedCurrency, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length == 3);

            _mockService.Verify(s => s.GetNavCurrency(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetExchangeRate_ValidInputs_ReturnsRate()
        {
            string from = "USD";
            string to = "EUR";
            DateTime date = new DateTime(2023, 10, 1);
            double expectedRate = 0.85;

            _mockService.Setup(s => s.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedRate);

            var result = _mockService.Object.GetExchangeRate(from, to, date);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0.0);

            _mockService.Verify(s => s.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ConvertFundValueCurrency_ValidInputs_ReturnsConvertedAmount()
        {
            decimal amount = 1000m;
            double rate = 0.85;
            decimal expectedAmount = 850m;

            _mockService.Setup(s => s.ConvertFundValueCurrency(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedAmount);

            var result = _mockService.Object.ConvertFundValueCurrency(amount, rate);

            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(amount, result);
            Assert.IsTrue(result < amount);

            _mockService.Verify(s => s.ConvertFundValueCurrency(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void HasPendingUnitAllocations_HasPending_ReturnsTrue()
        {
            string policyId = "POL123";

            _mockService.Setup(s => s.HasPendingUnitAllocations(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.HasPendingUnitAllocations(policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.HasPendingUnitAllocations(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPendingAllocationCount_ValidPolicy_ReturnsCount()
        {
            string policyId = "POL123";
            int expectedCount = 2;

            _mockService.Setup(s => s.GetPendingAllocationCount(It.IsAny<string>())).Returns(expectedCount);

            var result = _mockService.Object.GetPendingAllocationCount(policyId);

            Assert.AreEqual(expectedCount, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetPendingAllocationCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPendingAllocationAmount_ValidPolicy_ReturnsAmount()
        {
            string policyId = "POL123";
            decimal expectedAmount = 5000m;

            _mockService.Setup(s => s.GetPendingAllocationAmount(It.IsAny<string>())).Returns(expectedAmount);

            var result = _mockService.Object.GetPendingAllocationAmount(policyId);

            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0m);

            _mockService.Verify(s => s.GetPendingAllocationAmount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetLatestNavBatchId_ValidDate_ReturnsBatchId()
        {
            DateTime date = new DateTime(2023, 10, 1);
            string expectedBatchId = "BATCH_20231001";

            _mockService.Setup(s => s.GetLatestNavBatchId(It.IsAny<DateTime>())).Returns(expectedBatchId);

            var result = _mockService.Object.GetLatestNavBatchId(date);

            Assert.AreEqual(expectedBatchId, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.StartsWith("BATCH"));

            _mockService.Verify(s => s.GetLatestNavBatchId(It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateNavBatch_ValidBatch_ReturnsTrue()
        {
            string batchId = "BATCH_20231001";

            _mockService.Setup(s => s.ValidateNavBatch(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.ValidateNavBatch(batchId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.ValidateNavBatch(It.IsAny<string>()), Times.Once());
        }
    }
}