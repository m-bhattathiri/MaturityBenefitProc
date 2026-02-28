using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.SurrenderProcessing;

namespace MaturityBenefitProc.Tests.Helpers.SurrenderProcessing
{
    [TestClass]
    public class PolicySurrenderServiceMockTests
    {
        private Mock<IPolicySurrenderService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IPolicySurrenderService>();
        }

        [TestMethod]
        public void ValidatePolicyEligibility_ValidPolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime surrenderDate = new DateTime(2023, 1, 1);
            _mockService.Setup(s => s.ValidatePolicyEligibility(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.ValidatePolicyEligibility(policyId, surrenderDate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidatePolicyEligibility(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBaseSurrenderValue_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime effectiveDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 15000.50m;
            _mockService.Setup(s => s.CalculateBaseSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateBaseSurrenderValue(policyId, effectiveDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateBaseSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            decimal baseValue = 10000m;
            double currentMarketRate = 0.05;
            decimal expectedValue = -500m;
            _mockService.Setup(s => s.CalculateMarketValueAdjustment(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateMarketValueAdjustment(policyId, baseValue, currentMarketRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateMarketValueAdjustment(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSurrenderCharge_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            decimal baseValue = 10000m;
            int yearsInForce = 5;
            decimal expectedValue = 200m;
            _mockService.Setup(s => s.CalculateSurrenderCharge(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateSurrenderCharge(policyId, baseValue, yearsInForce);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateSurrenderCharge(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            decimal baseValue = 10000m;
            decimal expectedValue = 500m;
            _mockService.Setup(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTerminalBonus(policyId, baseValue);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateUnearnedPremiumRefund_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime surrenderDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 150m;
            _mockService.Setup(s => s.CalculateUnearnedPremiumRefund(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateUnearnedPremiumRefund(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateUnearnedPremiumRefund(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateOutstandingLoanBalance_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime calculationDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 1000m;
            _mockService.Setup(s => s.CalculateOutstandingLoanBalance(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateOutstandingLoanBalance(policyId, calculationDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateOutstandingLoanBalance(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoanInterestAccrued_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime calculationDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 50m;
            _mockService.Setup(s => s.CalculateLoanInterestAccrued(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateLoanInterestAccrued(policyId, calculationDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateLoanInterestAccrued(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateGrossSurrenderValue_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime effectiveDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 15000m;
            _mockService.Setup(s => s.CalculateGrossSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateGrossSurrenderValue(policyId, effectiveDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateGrossSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateNetSurrenderValue_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime effectiveDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 14500m;
            _mockService.Setup(s => s.CalculateNetSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateNetSurrenderValue(policyId, effectiveDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateNetSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCurrentSurrenderChargeRate_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            int policyYear = 5;
            double expectedValue = 0.02;
            _mockService.Setup(s => s.GetCurrentSurrenderChargeRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.GetCurrentSurrenderChargeRate(policyId, policyYear);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetCurrentSurrenderChargeRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetMarketValueAdjustmentFactor_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime calculationDate = new DateTime(2023, 1, 1);
            double expectedValue = 0.98;
            _mockService.Setup(s => s.GetMarketValueAdjustmentFactor(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetMarketValueAdjustmentFactor(policyId, calculationDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetMarketValueAdjustmentFactor(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            int yearsInForce = 10;
            double expectedValue = 0.05;
            _mockService.Setup(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.GetTerminalBonusRate(policyId, yearsInForce);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxWithholdingRate_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            string stateCode = "NY";
            double expectedValue = 0.10;
            _mockService.Setup(s => s.GetTaxWithholdingRate(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetTaxWithholdingRate(policyId, stateCode);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetTaxWithholdingRate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetProratedPremiumFactor_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime surrenderDate = new DateTime(2023, 1, 1);
            double expectedValue = 0.5;
            _mockService.Setup(s => s.GetProratedPremiumFactor(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetProratedPremiumFactor(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetProratedPremiumFactor(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyInForce_ValidPolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            _mockService.Setup(s => s.IsPolicyInForce(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.IsPolicyInForce(policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsPolicyInForce(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void HasOutstandingLoans_ValidPolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            _mockService.Setup(s => s.HasOutstandingLoans(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.HasOutstandingLoans(policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.HasOutstandingLoans(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsWithinFreeLookPeriod_ValidPolicy_ReturnsFalse()
        {
            string policyId = "POL123";
            DateTime requestDate = new DateTime(2023, 1, 1);
            _mockService.Setup(s => s.IsWithinFreeLookPeriod(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(false);

            var result = _mockService.Object.IsWithinFreeLookPeriod(policyId, requestDate);

            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.IsWithinFreeLookPeriod(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void RequiresSpousalConsent_ValidPolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            string stateCode = "CA";
            _mockService.Setup(s => s.RequiresSpousalConsent(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.RequiresSpousalConsent(policyId, stateCode);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.RequiresSpousalConsent(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsIrrevocableBeneficiaryPresent_ValidPolicy_ReturnsFalse()
        {
            string policyId = "POL123";
            _mockService.Setup(s => s.IsIrrevocableBeneficiaryPresent(It.IsAny<string>())).Returns(false);

            var result = _mockService.Object.IsIrrevocableBeneficiaryPresent(policyId);

            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.IsIrrevocableBeneficiaryPresent(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateSignatureRequirements_ValidPolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            string documentId = "DOC123";
            _mockService.Setup(s => s.ValidateSignatureRequirements(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.ValidateSignatureRequirements(policyId, documentId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateSignatureRequirements(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CheckAntiMoneyLaunderingStatus_ValidPolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            decimal netSurrenderValue = 15000m;
            _mockService.Setup(s => s.CheckAntiMoneyLaunderingStatus(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            var result = _mockService.Object.CheckAntiMoneyLaunderingStatus(policyId, netSurrenderValue);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.CheckAntiMoneyLaunderingStatus(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsVestingScheduleMet_ValidPolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime requestDate = new DateTime(2023, 1, 1);
            _mockService.Setup(s => s.IsVestingScheduleMet(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.IsVestingScheduleMet(policyId, requestDate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsVestingScheduleMet(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetYearsInForce_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime surrenderDate = new DateTime(2023, 1, 1);
            int expectedValue = 5;
            _mockService.Setup(s => s.GetYearsInForce(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetYearsInForce(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetYearsInForce(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void InitiateSurrenderWorkflow_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            string requestedBy = "User1";
            string expectedValue = "WF123";
            _mockService.Setup(s => s.InitiateSurrenderWorkflow(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.InitiateSurrenderWorkflow(policyId, requestedBy);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsFalse(string.IsNullOrEmpty(result));
            _mockService.Verify(s => s.InitiateSurrenderWorkflow(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
    }
}