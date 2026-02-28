using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans;

namespace MaturityBenefitProc.Tests.Helpers.FundValueAndUnitLinkedPlans
{
    [TestClass]
    public class UnitCancellationServiceMockTests
    {
        private Mock<IUnitCancellationService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IUnitCancellationService>();
        }

        [TestMethod]
        public void CalculateTotalCancellationValue_ValidPolicy_ReturnsExpectedValue()
        {
            var policyId = "POL123";
            var maturityDate = new DateTime(2023, 1, 1);
            var expectedValue = 15000.50m;

            _mockService.Setup(s => s.CalculateTotalCancellationValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalCancellationValue(policyId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateTotalCancellationValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateFundEligibility_EligibleFund_ReturnsTrue()
        {
            var fundCode = "FND01";
            var policyId = "POL123";

            _mockService.Setup(s => s.ValidateFundEligibility(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.ValidateFundEligibility(fundCode, policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateFundEligibility(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateFundEligibility_IneligibleFund_ReturnsFalse()
        {
            var fundCode = "FND99";
            var policyId = "POL123";

            _mockService.Setup(s => s.ValidateFundEligibility(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var result = _mockService.Object.ValidateFundEligibility(fundCode, policyId);

            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.ValidateFundEligibility(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetActiveFundCount_ValidPolicy_ReturnsCount()
        {
            var policyId = "POL123";
            var expectedCount = 3;

            _mockService.Setup(s => s.GetActiveFundCount(It.IsAny<string>())).Returns(expectedCount);

            var result = _mockService.Object.GetActiveFundCount(policyId);

            Assert.AreEqual(expectedCount, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetActiveFundCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetFundAllocationRatio_ValidFund_ReturnsRatio()
        {
            var policyId = "POL123";
            var fundCode = "FND01";
            var expectedRatio = 0.45;

            _mockService.Setup(s => s.GetFundAllocationRatio(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedRatio);

            var result = _mockService.Object.GetFundAllocationRatio(policyId, fundCode);

            Assert.AreEqual(expectedRatio, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetFundAllocationRatio(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPrimaryFundCode_ValidPolicy_ReturnsCode()
        {
            var policyId = "POL123";
            var expectedCode = "FND-MAIN";

            _mockService.Setup(s => s.GetPrimaryFundCode(It.IsAny<string>())).Returns(expectedCode);

            var result = _mockService.Object.GetPrimaryFundCode(policyId);

            Assert.AreEqual(expectedCode, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GetPrimaryFundCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetCurrentNav_ValidFund_ReturnsNav()
        {
            var fundCode = "FND01";
            var valDate = new DateTime(2023, 1, 1);
            var expectedNav = 12.345m;

            _mockService.Setup(s => s.GetCurrentNav(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedNav);

            var result = _mockService.Object.GetCurrentNav(fundCode, valDate);

            Assert.AreEqual(expectedNav, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetCurrentNav(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateFundCancellationValue_ValidInputs_ReturnsValue()
        {
            var policyId = "POL123";
            var fundCode = "FND01";
            var nav = 10.5m;
            var expectedValue = 1050.00m;

            _mockService.Setup(s => s.CalculateFundCancellationValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateFundCancellationValue(policyId, fundCode, nav);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateFundCancellationValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CheckPendingTransactions_HasPending_ReturnsTrue()
        {
            var policyId = "POL123";

            _mockService.Setup(s => s.CheckPendingTransactions(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.CheckPendingTransactions(policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.CheckPendingTransactions(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysSinceLastValuation_ValidFund_ReturnsDays()
        {
            var fundCode = "FND01";
            var currentDate = new DateTime(2023, 1, 5);
            var expectedDays = 2;

            _mockService.Setup(s => s.GetDaysSinceLastValuation(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDays);

            var result = _mockService.Object.GetDaysSinceLastValuation(fundCode, currentDate);

            Assert.AreEqual(expectedDays, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(-1, result);

            _mockService.Verify(s => s.GetDaysSinceLastValuation(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void InitiateUnitCancellation_ValidRequest_ReturnsTransactionId()
        {
            var policyId = "POL123";
            var reqDate = new DateTime(2023, 1, 1);
            var expectedTxId = "TX-999";

            _mockService.Setup(s => s.InitiateUnitCancellation(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedTxId);

            var result = _mockService.Object.InitiateUnitCancellation(policyId, reqDate);

            Assert.AreEqual(expectedTxId, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("TX"));
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.InitiateUnitCancellation(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateCancellationPenaltyRate_ValidPolicy_ReturnsRate()
        {
            var policyId = "POL123";
            var term = 10;
            var expectedRate = 0.02;

            _mockService.Setup(s => s.CalculateCancellationPenaltyRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.CalculateCancellationPenaltyRate(policyId, term);

            Assert.AreEqual(expectedRate, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(-1.0, result);

            _mockService.Verify(s => s.CalculateCancellationPenaltyRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ApplyCancellationPenalty_ValidInputs_ReturnsNetValue()
        {
            var gross = 1000m;
            var rate = 0.05;
            var expectedNet = 950m;

            _mockService.Setup(s => s.ApplyCancellationPenalty(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedNet);

            var result = _mockService.Object.ApplyCancellationPenalty(gross, rate);

            Assert.AreEqual(expectedNet, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(gross, result);

            _mockService.Verify(s => s.ApplyCancellationPenalty(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void VerifyUnitBalance_CorrectBalance_ReturnsTrue()
        {
            var policyId = "POL123";
            var fundCode = "FND01";
            var expectedUnits = 100.5m;

            _mockService.Setup(s => s.VerifyUnitBalance(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            var result = _mockService.Object.VerifyUnitBalance(policyId, fundCode, expectedUnits);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.VerifyUnitBalance(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void RetrieveCancelledUnitCount_ValidFund_ReturnsCount()
        {
            var policyId = "POL123";
            var fundCode = "FND01";
            var expectedCount = 50;

            _mockService.Setup(s => s.RetrieveCancelledUnitCount(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedCount);

            var result = _mockService.Object.RetrieveCancelledUnitCount(policyId, fundCode);

            Assert.AreEqual(expectedCount, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.RetrieveCancelledUnitCount(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateCancellationReceipt_ValidInputs_ReturnsReceipt()
        {
            var policyId = "POL123";
            var totalValue = 5000m;
            var expectedReceipt = "RCPT-12345";

            _mockService.Setup(s => s.GenerateCancellationReceipt(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedReceipt);

            var result = _mockService.Object.GenerateCancellationReceipt(policyId, totalValue);

            Assert.AreEqual(expectedReceipt, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GenerateCancellationReceipt(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalUnitsHeld_ValidFund_ReturnsUnits()
        {
            var policyId = "POL123";
            var fundCode = "FND01";
            var expectedUnits = 250.75m;

            _mockService.Setup(s => s.GetTotalUnitsHeld(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedUnits);

            var result = _mockService.Object.GetTotalUnitsHeld(policyId, fundCode);

            Assert.AreEqual(expectedUnits, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetTotalUnitsHeld(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetMarketValueAdjustmentFactor_ValidFund_ReturnsFactor()
        {
            var fundCode = "FND01";
            var adjDate = new DateTime(2023, 1, 1);
            var expectedFactor = 0.98;

            _mockService.Setup(s => s.GetMarketValueAdjustmentFactor(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedFactor);

            var result = _mockService.Object.GetMarketValueAdjustmentFactor(fundCode, adjDate);

            Assert.AreEqual(expectedFactor, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(1.0, result);

            _mockService.Verify(s => s.GetMarketValueAdjustmentFactor(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ApplyMarketValueAdjustment_ValidInputs_ReturnsAdjustedValue()
        {
            var baseValue = 1000m;
            var factor = 0.95;
            var expectedValue = 950m;

            _mockService.Setup(s => s.ApplyMarketValueAdjustment(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.ApplyMarketValueAdjustment(baseValue, factor);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(baseValue, result);

            _mockService.Verify(s => s.ApplyMarketValueAdjustment(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void IsFundSuspended_SuspendedFund_ReturnsTrue()
        {
            var fundCode = "FND01";
            var checkDate = new DateTime(2023, 1, 1);

            _mockService.Setup(s => s.IsFundSuspended(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.IsFundSuspended(fundCode, checkDate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsFundSuspended(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingLockInPeriodDays_ValidPolicy_ReturnsDays()
        {
            var policyId = "POL123";
            var currentDate = new DateTime(2023, 1, 1);
            var expectedDays = 15;

            _mockService.Setup(s => s.GetRemainingLockInPeriodDays(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDays);

            var result = _mockService.Object.GetRemainingLockInPeriodDays(policyId, currentDate);

            Assert.AreEqual(expectedDays, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetRemainingLockInPeriodDays(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCancellationStatus_ValidTx_ReturnsStatus()
        {
            var txId = "TX-999";
            var expectedStatus = "Completed";

            _mockService.Setup(s => s.GetCancellationStatus(It.IsAny<string>())).Returns(expectedStatus);

            var result = _mockService.Object.GetCancellationStatus(txId);

            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("Pending", result);

            _mockService.Verify(s => s.GetCancellationStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInputs_ReturnsBonus()
        {
            var policyId = "POL123";
            var fundValue = 10000m;
            var expectedBonus = 500m;

            _mockService.Setup(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedBonus);

            var result = _mockService.Object.CalculateTerminalBonus(policyId, fundValue);

            Assert.AreEqual(expectedBonus, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void AuthorizeCancellation_ValidAuth_ReturnsTrue()
        {
            var policyId = "POL123";
            var authBy = "AdminUser";

            _mockService.Setup(s => s.AuthorizeCancellation(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.AuthorizeCancellation(policyId, authBy);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.AuthorizeCancellation(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ComputeNetMaturityValue_ValidInputs_ReturnsNet()
        {
            var policyId = "POL123";
            var gross = 10000m;
            var deductions = 500m;
            var expectedNet = 9500m;

            _mockService.Setup(s => s.ComputeNetMaturityValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedNet);

            var result = _mockService.Object.ComputeNetMaturityValue(policyId, gross, deductions);

            Assert.AreEqual(expectedNet, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(gross, result);

            _mockService.Verify(s => s.ComputeNetMaturityValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }
    }
}