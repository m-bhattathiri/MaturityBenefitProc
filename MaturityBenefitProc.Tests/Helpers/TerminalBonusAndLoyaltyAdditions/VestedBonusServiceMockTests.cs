using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class VestedBonusServiceMockTests
    {
        private Mock<IVestedBonusService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IVestedBonusService>();
        }

        [TestMethod]
        public void CalculateTotalVestedBonus_ValidInput_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            DateTime calcDate = new DateTime(2023, 1, 1);
            decimal expected = 1500.50m;

            _mockService.Setup(s => s.CalculateTotalVestedBonus(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateTotalVestedBonus(policyId, calcDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.CalculateTotalVestedBonus(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetSimpleReversionaryBonus_ValidInput_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            int year = 5;
            decimal expected = 500m;

            _mockService.Setup(s => s.GetSimpleReversionaryBonus(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetSimpleReversionaryBonus(policyId, year);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            Assert.IsTrue(result == 500m);

            _mockService.Verify(s => s.GetSimpleReversionaryBonus(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetCompoundReversionaryBonus_ValidInput_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            int year = 3;
            decimal expected = 750.25m;

            _mockService.Setup(s => s.GetCompoundReversionaryBonus(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetCompoundReversionaryBonus(policyId, year);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetCompoundReversionaryBonus(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateInterimBonus_ValidInput_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 6, 15);
            decimal expected = 200m;

            _mockService.Setup(s => s.CalculateInterimBonus(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateInterimBonus(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            Assert.IsTrue(result == 200m);

            _mockService.Verify(s => s.CalculateInterimBonus(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInput_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal sumAssured = 100000m;
            decimal expected = 5000m;

            _mockService.Setup(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateTerminalBonus(policyId, sumAssured);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_ValidInput_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            int years = 10;
            decimal expected = 1000m;

            _mockService.Setup(s => s.CalculateLoyaltyAddition(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.CalculateLoyaltyAddition(policyId, years);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result == 1000m);

            _mockService.Verify(s => s.CalculateLoyaltyAddition(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetBonusRateForYear_ValidInput_ReturnsExpectedRate()
        {
            int year = 5;
            string planCode = "PLAN_A";
            double expected = 0.05;

            _mockService.Setup(s => s.GetBonusRateForYear(It.IsAny<int>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetBonusRateForYear(year, planCode);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetBonusRateForYear(It.IsAny<int>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidInput_ReturnsExpectedRate()
        {
            string planCode = "PLAN_A";
            int term = 20;
            double expected = 0.15;

            _mockService.Setup(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetTerminalBonusRate(planCode, term);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetLoyaltyAdditionPercentage_ValidInput_ReturnsExpectedPercentage()
        {
            string planCode = "PLAN_A";
            decimal premium = 1000m;
            double expected = 0.02;

            _mockService.Setup(s => s.GetLoyaltyAdditionPercentage(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.GetLoyaltyAdditionPercentage(planCode, premium);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetLoyaltyAdditionPercentage(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetInterimBonusRate_ValidInput_ReturnsExpectedRate()
        {
            string planCode = "PLAN_A";
            DateTime date = new DateTime(2023, 1, 1);
            double expected = 0.03;

            _mockService.Setup(s => s.GetInterimBonusRate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetInterimBonusRate(planCode, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetInterimBonusRate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForTerminalBonus_ValidInput_ReturnsTrue()
        {
            string policyId = "POL123";
            int activeYears = 15;

            _mockService.Setup(s => s.IsEligibleForTerminalBonus(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            var result = _mockService.Object.IsEligibleForTerminalBonus(policyId, activeYears);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.IsEligibleForTerminalBonus(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForLoyaltyAddition_ValidInput_ReturnsTrue()
        {
            string policyId = "POL123";
            decimal premiums = 50000m;

            _mockService.Setup(s => s.IsEligibleForLoyaltyAddition(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            var result = _mockService.Object.IsEligibleForLoyaltyAddition(policyId, premiums);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.IsEligibleForLoyaltyAddition(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void HasSurrenderedPolicy_ValidInput_ReturnsFalse()
        {
            string policyId = "POL123";

            _mockService.Setup(s => s.HasSurrenderedPolicy(It.IsAny<string>())).Returns(false);

            var result = _mockService.Object.HasSurrenderedPolicy(policyId);

            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);

            _mockService.Verify(s => s.HasSurrenderedPolicy(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyActive_ValidInput_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);

            _mockService.Setup(s => s.IsPolicyActive(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.IsPolicyActive(policyId, date);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.IsPolicyActive(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateBonusRates_ValidInput_ReturnsTrue()
        {
            string planCode = "PLAN_A";
            double simpleRate = 0.05;
            double compoundRate = 0.03;

            _mockService.Setup(s => s.ValidateBonusRates(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);

            var result = _mockService.Object.ValidateBonusRates(planCode, simpleRate, compoundRate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.ValidateBonusRates(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CheckMinimumVestingPeriod_ValidInput_ReturnsTrue()
        {
            string policyId = "POL123";
            int minYears = 3;

            _mockService.Setup(s => s.CheckMinimumVestingPeriod(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            var result = _mockService.Object.CheckMinimumVestingPeriod(policyId, minYears);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.CheckMinimumVestingPeriod(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidInput_ReturnsExpectedYears()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            int expected = 5;

            _mockService.Setup(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetCompletedPolicyYears(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingTermInMonths_ValidInput_ReturnsExpectedMonths()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            int expected = 120;

            _mockService.Setup(s => s.GetRemainingTermInMonths(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetRemainingTermInMonths(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetRemainingTermInMonths(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalPremiumsPaidCount_ValidInput_ReturnsExpectedCount()
        {
            string policyId = "POL123";
            int expected = 60;

            _mockService.Setup(s => s.GetTotalPremiumsPaidCount(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetTotalPremiumsPaidCount(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);

            _mockService.Verify(s => s.GetTotalPremiumsPaidCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetMissedPremiumsCount_ValidInput_ReturnsExpectedCount()
        {
            string policyId = "POL123";
            int expected = 2;

            _mockService.Setup(s => s.GetMissedPremiumsCount(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetMissedPremiumsCount(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(-1, result);
            Assert.IsTrue(result >= 0);

            _mockService.Verify(s => s.GetMissedPremiumsCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetBonusDeclarationYear_ValidInput_ReturnsExpectedYear()
        {
            string bonusId = "BONUS123";
            int expected = 2022;

            _mockService.Setup(s => s.GetBonusDeclarationYear(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetBonusDeclarationYear(bonusId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 2000);

            _mockService.Verify(s => s.GetBonusDeclarationYear(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableBonusTableCode_ValidInput_ReturnsExpectedCode()
        {
            string planCode = "PLAN_A";
            DateTime date = new DateTime(2023, 1, 1);
            string expected = "TABLE_A";

            _mockService.Setup(s => s.GetApplicableBonusTableCode(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetApplicableBonusTableCode(planCode, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.GetApplicableBonusTableCode(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetBonusStatus_ValidInput_ReturnsExpectedStatus()
        {
            string policyId = "POL123";
            string expected = "VESTED";

            _mockService.Setup(s => s.GetBonusStatus(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetBonusStatus(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("PENDING", result);
            Assert.IsTrue(result == "VESTED");

            _mockService.Verify(s => s.GetBonusStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateBonusStatementId_ValidInput_ReturnsExpectedId()
        {
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            string expected = "STMT123";

            _mockService.Setup(s => s.GenerateBonusStatementId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GenerateBonusStatementId(policyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.GenerateBonusStatementId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetFundCodeForBonus_ValidInput_ReturnsExpectedCode()
        {
            string planCode = "PLAN_A";
            string expected = "FUND_A";

            _mockService.Setup(s => s.GetFundCodeForBonus(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetFundCodeForBonus(planCode);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.GetFundCodeForBonus(It.IsAny<string>()), Times.Once());
        }
    }
}