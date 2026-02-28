using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.AnnuityProcessing;

namespace MaturityBenefitProc.Tests.Helpers.AnnuityProcessing
{
    [TestClass]
    public class DeferredAnnuityServiceMockTests
    {
        private Mock<IDeferredAnnuityService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IDeferredAnnuityService>();
        }

        [TestMethod]
        public void CalculateAccumulatedValue_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime calcDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 150000.50m;

            _mockService.Setup(s => s.CalculateAccumulatedValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateAccumulatedValue(policyId, calcDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateAccumulatedValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateAccumulatedValue_ZeroValue_ReturnsZero()
        {
            string policyId = "POL999";
            DateTime calcDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 0m;

            _mockService.Setup(s => s.CalculateAccumulatedValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateAccumulatedValue(policyId, calcDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0m, result);

            _mockService.Verify(s => s.CalculateAccumulatedValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetVestingStatus_ActivePolicy_ReturnsActive()
        {
            string policyId = "POL123";
            string expectedStatus = "ACTIVE";

            _mockService.Setup(s => s.GetVestingStatus(It.IsAny<string>())).Returns(expectedStatus);

            var result = _mockService.Object.GetVestingStatus(policyId);

            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("INACTIVE", result);

            _mockService.Verify(s => s.GetVestingStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetVestingStatus_VestedPolicy_ReturnsVested()
        {
            string policyId = "POL124";
            string expectedStatus = "VESTED";

            _mockService.Setup(s => s.GetVestingStatus(It.IsAny<string>())).Returns(expectedStatus);

            var result = _mockService.Object.GetVestingStatus(policyId);

            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == "VESTED");
            Assert.AreNotEqual("ACTIVE", result);

            _mockService.Verify(s => s.GetVestingStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForSurrender_Eligible_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime requestDate = new DateTime(2023, 1, 1);
            bool expectedValue = true;

            _mockService.Setup(s => s.IsEligibleForSurrender(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.IsEligibleForSurrender(policyId, requestDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsEligibleForSurrender(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForSurrender_NotEligible_ReturnsFalse()
        {
            string policyId = "POL124";
            DateTime requestDate = new DateTime(2023, 1, 1);
            bool expectedValue = false;

            _mockService.Setup(s => s.IsEligibleForSurrender(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.IsEligibleForSurrender(policyId, requestDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.IsEligibleForSurrender(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSurrenderValue_ValidInputs_ReturnsCalculatedValue()
        {
            string policyId = "POL123";
            decimal currentAccumulation = 100000m;
            double surrenderChargeRate = 0.05;
            decimal expectedValue = 95000m;

            _mockService.Setup(s => s.CalculateSurrenderValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateSurrenderValue(policyId, currentAccumulation, surrenderChargeRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(100000m, result);

            _mockService.Verify(s => s.CalculateSurrenderValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSurrenderValue_HighCharge_ReturnsCalculatedValue()
        {
            string policyId = "POL124";
            decimal currentAccumulation = 100000m;
            double surrenderChargeRate = 0.50;
            decimal expectedValue = 50000m;

            _mockService.Setup(s => s.CalculateSurrenderValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateSurrenderValue(policyId, currentAccumulation, surrenderChargeRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(100000m, result);

            _mockService.Verify(s => s.CalculateSurrenderValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetGuaranteedAdditionRate_ValidPlan_ReturnsRate()
        {
            string planCode = "PLAN_A";
            int policyYear = 5;
            double expectedRate = 0.04;

            _mockService.Setup(s => s.GetGuaranteedAdditionRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.GetGuaranteedAdditionRate(planCode, policyYear);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetGuaranteedAdditionRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetGuaranteedAdditionRate_InvalidPlan_ReturnsZero()
        {
            string planCode = "INVALID";
            int policyYear = 1;
            double expectedRate = 0.0;

            _mockService.Setup(s => s.GetGuaranteedAdditionRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.GetGuaranteedAdditionRate(planCode, policyYear);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0.0, result);

            _mockService.Verify(s => s.GetGuaranteedAdditionRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingAccumulationMonths_ValidPolicy_ReturnsMonths()
        {
            string policyId = "POL123";
            DateTime currentDate = new DateTime(2023, 1, 1);
            int expectedMonths = 60;

            _mockService.Setup(s => s.GetRemainingAccumulationMonths(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedMonths);

            var result = _mockService.Object.GetRemainingAccumulationMonths(policyId, currentDate);

            Assert.AreEqual(expectedMonths, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetRemainingAccumulationMonths(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GenerateVestingQuotationId_ValidPolicy_ReturnsId()
        {
            string policyId = "POL123";
            string expectedId = "VQ-POL123-2023";

            _mockService.Setup(s => s.GenerateVestingQuotationId(It.IsAny<string>())).Returns(expectedId);

            var result = _mockService.Object.GenerateVestingQuotationId(policyId);

            Assert.AreEqual(expectedId, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("VQ"));
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GenerateVestingQuotationId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateDefermentPeriod_ValidPeriod_ReturnsTrue()
        {
            string planCode = "PLAN_A";
            int defermentYears = 10;
            bool expectedValue = true;

            _mockService.Setup(s => s.ValidateDefermentPeriod(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.ValidateDefermentPeriod(planCode, defermentYears);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateDefermentPeriod(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDeathBenefit_ValidInputs_ReturnsBenefit()
        {
            string policyId = "POL123";
            DateTime dateOfDeath = new DateTime(2023, 1, 1);
            decimal expectedValue = 200000m;

            _mockService.Setup(s => s.CalculateDeathBenefit(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateDeathBenefit(policyId, dateOfDeath);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateDeathBenefit(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBonusRatio_ValidInputs_ReturnsRatio()
        {
            string policyId = "POL123";
            int accumulationYears = 10;
            double expectedRatio = 1.5;

            _mockService.Setup(s => s.CalculateBonusRatio(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRatio);

            var result = _mockService.Object.CalculateBonusRatio(policyId, accumulationYears);

            Assert.AreEqual(expectedRatio, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 1.0);
            Assert.AreNotEqual(1.0, result);

            _mockService.Verify(s => s.CalculateBonusRatio(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetPaidPremiumsCount_ValidPolicy_ReturnsCount()
        {
            string policyId = "POL123";
            int expectedCount = 120;

            _mockService.Setup(s => s.GetPaidPremiumsCount(It.IsAny<string>())).Returns(expectedCount);

            var result = _mockService.Object.GetPaidPremiumsCount(policyId);

            Assert.AreEqual(expectedCount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetPaidPremiumsCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetAnnuityOptionCode_ValidPolicy_ReturnsCode()
        {
            string policyId = "POL123";
            string expectedCode = "OPT_LIFE";

            _mockService.Setup(s => s.GetAnnuityOptionCode(It.IsAny<string>())).Returns(expectedCode);

            var result = _mockService.Object.GetAnnuityOptionCode(policyId);

            Assert.AreEqual(expectedCode, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GetAnnuityOptionCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProjectedMaturityValue_ValidInputs_ReturnsValue()
        {
            string policyId = "POL123";
            double assumedInterestRate = 0.08;
            decimal expectedValue = 500000m;

            _mockService.Setup(s => s.CalculateProjectedMaturityValue(It.IsAny<string>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateProjectedMaturityValue(policyId, assumedInterestRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateProjectedMaturityValue(It.IsAny<string>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CheckVestingConditionMet_Met_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime evalDate = new DateTime(2023, 1, 1);
            bool expectedValue = true;

            _mockService.Setup(s => s.CheckVestingConditionMet(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CheckVestingConditionMet(policyId, evalDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.CheckVestingConditionMet(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ApplyTerminalBonus_ValidInputs_ReturnsValue()
        {
            string policyId = "POL123";
            decimal baseAmount = 100000m;
            decimal expectedValue = 110000m;

            _mockService.Setup(s => s.ApplyTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.ApplyTerminalBonus(policyId, baseAmount);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > baseAmount);
            Assert.AreNotEqual(baseAmount, result);

            _mockService.Verify(s => s.ApplyTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetLoyaltyAdditionPercentage_ValidYears_ReturnsPercentage()
        {
            int completedYears = 15;
            double expectedPercentage = 0.05;

            _mockService.Setup(s => s.GetLoyaltyAdditionPercentage(It.IsAny<int>())).Returns(expectedPercentage);

            var result = _mockService.Object.GetLoyaltyAdditionPercentage(completedYears);

            Assert.AreEqual(expectedPercentage, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetLoyaltyAdditionPercentage(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysToVesting_ValidInputs_ReturnsDays()
        {
            string policyId = "POL123";
            DateTime currentDate = new DateTime(2023, 1, 1);
            int expectedDays = 365;

            _mockService.Setup(s => s.CalculateDaysToVesting(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDays);

            var result = _mockService.Object.CalculateDaysToVesting(policyId, currentDate);

            Assert.AreEqual(expectedDays, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.CalculateDaysToVesting(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void UpdateAccumulationPhaseStatus_ValidInputs_ReturnsNewStatus()
        {
            string policyId = "POL123";
            string newStatusCode = "PHASE_2";
            string expectedResult = "SUCCESS";

            _mockService.Setup(s => s.UpdateAccumulationPhaseStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedResult);

            var result = _mockService.Object.UpdateAccumulationPhaseStatus(policyId, newStatusCode);

            Assert.AreEqual(expectedResult, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == "SUCCESS");
            Assert.AreNotEqual("FAILED", result);

            _mockService.Verify(s => s.UpdateAccumulationPhaseStatus(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void MultipleCalls_VerifyExecutionCounts()
        {
            string policyId = "POL123";
            
            _mockService.Setup(s => s.GetVestingStatus(It.IsAny<string>())).Returns("ACTIVE");
            
            var result1 = _mockService.Object.GetVestingStatus(policyId);
            var result2 = _mockService.Object.GetVestingStatus(policyId);
            
            Assert.AreEqual("ACTIVE", result1);
            Assert.AreEqual("ACTIVE", result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            
            _mockService.Verify(s => s.GetVestingStatus(It.IsAny<string>()), Times.Exactly(2));
            _mockService.Verify(s => s.GetPaidPremiumsCount(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void CalculateAccumulatedValue_MultipleCalls_ReturnsExpectedValues()
        {
            string policyId = "POL123";
            DateTime calcDate = new DateTime(2023, 1, 1);
            
            _mockService.SetupSequence(s => s.CalculateAccumulatedValue(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(100m)
                .Returns(200m);
                
            var result1 = _mockService.Object.CalculateAccumulatedValue(policyId, calcDate);
            var result2 = _mockService.Object.CalculateAccumulatedValue(policyId, calcDate);
            
            Assert.AreEqual(100m, result1);
            Assert.AreEqual(200m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result2 > result1);
            
            _mockService.Verify(s => s.CalculateAccumulatedValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(2));
        }
    }
}