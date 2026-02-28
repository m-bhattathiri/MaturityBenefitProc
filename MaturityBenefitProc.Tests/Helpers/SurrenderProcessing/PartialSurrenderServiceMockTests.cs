using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.SurrenderProcessing;

namespace MaturityBenefitProc.Tests.Helpers.SurrenderProcessing
{
    [TestClass]
    public class PartialSurrenderServiceMockTests
    {
        private Mock<IPartialSurrenderService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IPartialSurrenderService>();
        }

        [TestMethod]
        public void CalculateMaximumWithdrawalAmount_ValidPolicy_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            decimal expected = 50000m;

            _mockService.Setup(s => s.CalculateMaximumWithdrawalAmount(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateMaximumWithdrawalAmount(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateMaximumWithdrawalAmount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSurrenderCharge_ValidAmount_ReturnsExpectedCharge()
        {
            string policyId = "POL123";
            decimal amount = 10000m;
            decimal expected = 500m;

            _mockService.Setup(s => s.CalculateSurrenderCharge(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateSurrenderCharge(policyId, amount);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(-1m, result);

            _mockService.Verify(s => s.CalculateSurrenderCharge(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetAvailableFreeWithdrawalAmount_ValidRequest_ReturnsExpected()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            decimal expected = 10000m;

            _mockService.Setup(s => s.GetAvailableFreeWithdrawalAmount(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetAvailableFreeWithdrawalAmount(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetAvailableFreeWithdrawalAmount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateNetPayoutAmount_ValidInputs_ReturnsExpected()
        {
            decimal gross = 10000m;
            decimal charge = 500m;
            decimal tax = 2000m;
            decimal expected = 7500m;

            _mockService.Setup(s => s.CalculateNetPayoutAmount(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateNetPayoutAmount(gross, charge, tax);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(gross, result);

            _mockService.Verify(s => s.CalculateNetPayoutAmount(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetMinimumRemainingBalanceRequired_ValidProduct_ReturnsExpected()
        {
            string productCode = "PROD1";
            decimal expected = 2000m;

            _mockService.Setup(s => s.GetMinimumRemainingBalanceRequired(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetMinimumRemainingBalanceRequired(productCode);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetMinimumRemainingBalanceRequired(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProratedRiderDeduction_ValidInputs_ReturnsExpected()
        {
            string policyId = "POL123";
            decimal amount = 5000m;
            decimal expected = 50m;

            _mockService.Setup(s => s.CalculateProratedRiderDeduction(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateProratedRiderDeduction(policyId, amount);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(-1m, result);

            _mockService.Verify(s => s.CalculateProratedRiderDeduction(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_ValidInputs_ReturnsExpected()
        {
            string policyId = "POL123";
            decimal amount = 10000m;
            DateTime date = new DateTime(2023, 1, 1);
            decimal expected = -150m;

            _mockService.Setup(s => s.CalculateMarketValueAdjustment(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateMarketValueAdjustment(policyId, amount, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateMarketValueAdjustment(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForPartialSurrender_Eligible_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.IsEligibleForPartialSurrender(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsEligibleForPartialSurrender(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsEligibleForPartialSurrender(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateMinimumWithdrawalAmount_ValidAmount_ReturnsTrue()
        {
            string productCode = "PROD1";
            decimal amount = 1000m;
            bool expected = true;

            _mockService.Setup(s => s.ValidateMinimumWithdrawalAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.ValidateMinimumWithdrawalAmount(productCode, amount);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateMinimumWithdrawalAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void HasExceededAnnualWithdrawalLimit_NotExceeded_ReturnsFalse()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            bool expected = false;

            _mockService.Setup(s => s.HasExceededAnnualWithdrawalLimit(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.HasExceededAnnualWithdrawalLimit(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.HasExceededAnnualWithdrawalLimit(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyInLockupPeriod_InLockup_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.IsPolicyInLockupPeriod(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsPolicyInLockupPeriod(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsPolicyInLockupPeriod(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void RequiresSpousalConsent_Requires_ReturnsTrue()
        {
            string policyId = "POL123";
            decimal amount = 50000m;
            bool expected = true;

            _mockService.Setup(s => s.RequiresSpousalConsent(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.RequiresSpousalConsent(policyId, amount);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.RequiresSpousalConsent(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsSystematicWithdrawalActive_Active_ReturnsTrue()
        {
            string policyId = "POL123";
            bool expected = true;

            _mockService.Setup(s => s.IsSystematicWithdrawalActive(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.IsSystematicWithdrawalActive(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsSystematicWithdrawalActive(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetSurrenderChargePercentage_ValidInputs_ReturnsExpected()
        {
            string policyId = "POL123";
            int year = 3;
            double expected = 0.05;

            _mockService.Setup(s => s.GetSurrenderChargePercentage(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetSurrenderChargePercentage(policyId, year);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetSurrenderChargePercentage(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxWithholdingRate_ValidInputs_ReturnsExpected()
        {
            string state = "NY";
            bool isFederal = true;
            double expected = 0.10;

            _mockService.Setup(s => s.CalculateTaxWithholdingRate(It.IsAny<string>(), It.IsAny<bool>())).Returns(expected);

            var result = _mockService.Object.CalculateTaxWithholdingRate(state, isFederal);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.CalculateTaxWithholdingRate(It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void GetFreeWithdrawalPercentage_ValidProduct_ReturnsExpected()
        {
            string productCode = "PROD1";
            double expected = 0.10;

            _mockService.Setup(s => s.GetFreeWithdrawalPercentage(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetFreeWithdrawalPercentage(productCode);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetFreeWithdrawalPercentage(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProRataReductionFactor_ValidInputs_ReturnsExpected()
        {
            decimal amount = 1000m;
            decimal value = 10000m;
            double expected = 0.10;

            _mockService.Setup(s => s.CalculateProRataReductionFactor(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateProRataReductionFactor(amount, value);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.CalculateProRataReductionFactor(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingFreeWithdrawalsCount_ValidInputs_ReturnsExpected()
        {
            string policyId = "POL123";
            int year = 2023;
            int expected = 2;

            _mockService.Setup(s => s.GetRemainingFreeWithdrawalsCount(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetRemainingFreeWithdrawalsCount(policyId, year);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetRemainingFreeWithdrawalsCount(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysUntilSurrenderChargeExpires_ValidInputs_ReturnsExpected()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            int expected = 365;

            _mockService.Setup(s => s.GetDaysUntilSurrenderChargeExpires(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetDaysUntilSurrenderChargeExpires(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetDaysUntilSurrenderChargeExpires(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPolicyYear_ValidInputs_ReturnsExpected()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            int expected = 5;

            _mockService.Setup(s => s.GetPolicyYear(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetPolicyYear(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetPolicyYear(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetMaximumAllowedWithdrawalsPerYear_ValidProduct_ReturnsExpected()
        {
            string productCode = "PROD1";
            int expected = 4;

            _mockService.Setup(s => s.GetMaximumAllowedWithdrawalsPerYear(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetMaximumAllowedWithdrawalsPerYear(productCode);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetMaximumAllowedWithdrawalsPerYear(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateSurrenderTransactionId_ValidInputs_ReturnsExpected()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            string expected = "TXN123";

            _mockService.Setup(s => s.GenerateSurrenderTransactionId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GenerateSurrenderTransactionId(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GenerateSurrenderTransactionId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetSurrenderChargeScheduleCode_ValidPolicy_ReturnsExpected()
        {
            string policyId = "POL123";
            string expected = "SCHED1";

            _mockService.Setup(s => s.GetSurrenderChargeScheduleCode(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetSurrenderChargeScheduleCode(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GetSurrenderChargeScheduleCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void DetermineTaxDistributionCode_ValidInputs_ReturnsExpected()
        {
            int age = 65;
            bool isQualified = true;
            string expected = "7";

            _mockService.Setup(s => s.DetermineTaxDistributionCode(It.IsAny<int>(), It.IsAny<bool>())).Returns(expected);

            var result = _mockService.Object.DetermineTaxDistributionCode(age, isQualified);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.DetermineTaxDistributionCode(It.IsAny<int>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void GetWithdrawalDenialReasonCode_ValidInputs_ReturnsExpected()
        {
            string policyId = "POL123";
            decimal amount = 100000m;
            string expected = "EXCEEDS_MAX";

            _mockService.Setup(s => s.GetWithdrawalDenialReasonCode(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.GetWithdrawalDenialReasonCode(policyId, amount);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GetWithdrawalDenialReasonCode(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }
    }
}