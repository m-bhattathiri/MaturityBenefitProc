using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class GuaranteedAdditionServiceMockTests
    {
        private Mock<IGuaranteedAdditionService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IGuaranteedAdditionService>();
        }

        [TestMethod]
        public void CalculateTotalGuaranteedAdditions_ValidPolicy_ReturnsExpectedAmount()
        {
            // Arrange
            string policyId = "POL12345";
            DateTime calcDate = new DateTime(2023, 1, 1);
            decimal expected = 1500.50m;
            _mockService.Setup(s => s.CalculateTotalGuaranteedAdditions(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateTotalGuaranteedAdditions(policyId, calcDate);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTotalGuaranteedAdditions(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateAccruedAdditionsForYear_ValidYear_ReturnsAmount()
        {
            // Arrange
            string policyId = "POL12345";
            int year = 5;
            decimal expected = 300.00m;
            _mockService.Setup(s => s.CalculateAccruedAdditionsForYear(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateAccruedAdditionsForYear(policyId, year);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 300.00m);
            Assert.AreNotEqual(100m, result);
            _mockService.Verify(s => s.CalculateAccruedAdditionsForYear(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForGuaranteedAdditions_EligiblePolicy_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL12345";
            string productCode = "PROD01";
            _mockService.Setup(s => s.IsPolicyEligibleForGuaranteedAdditions(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.IsPolicyEligibleForGuaranteedAdditions(policyId, productCode);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsPolicyEligibleForGuaranteedAdditions(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForGuaranteedAdditions_IneligiblePolicy_ReturnsFalse()
        {
            // Arrange
            string policyId = "POL999";
            string productCode = "PROD99";
            _mockService.Setup(s => s.IsPolicyEligibleForGuaranteedAdditions(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = _mockService.Object.IsPolicyEligibleForGuaranteedAdditions(policyId, productCode);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.IsPolicyEligibleForGuaranteedAdditions(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableAdditionRate_ValidProduct_ReturnsRate()
        {
            // Arrange
            string productCode = "PROD01";
            int term = 10;
            double expected = 0.05;
            _mockService.Setup(s => s.GetApplicableAdditionRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetApplicableAdditionRate(productCode, term);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetApplicableAdditionRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetAccrualPeriodInDays_ValidDates_ReturnsDays()
        {
            // Arrange
            DateTime start = new DateTime(2023, 1, 1);
            DateTime end = new DateTime(2023, 1, 31);
            int expected = 30;
            _mockService.Setup(s => s.GetAccrualPeriodInDays(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetAccrualPeriodInDays(start, end);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 30);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetAccrualPeriodInDays(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetAdditionCalculationBasisCode_ValidProduct_ReturnsCode()
        {
            // Arrange
            string productCode = "PROD01";
            string expected = "BASIS_A";
            _mockService.Setup(s => s.GetAdditionCalculationBasisCode(It.IsAny<string>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetAdditionCalculationBasisCode(productCode);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("BASIS_B", result);
            _mockService.Verify(s => s.GetAdditionCalculationBasisCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProRataAdditions_ValidInputs_ReturnsAmount()
        {
            // Arrange
            string policyId = "POL123";
            decimal baseAmount = 1000m;
            int days = 180;
            decimal expected = 500m;
            _mockService.Setup(s => s.CalculateProRataAdditions(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateProRataAdditions(policyId, baseAmount, days);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 500m);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateProRataAdditions(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ValidateAdditionRateLimits_ValidRate_ReturnsTrue()
        {
            // Arrange
            double rate = 0.05;
            string productCode = "PROD01";
            _mockService.Setup(s => s.ValidateAdditionRateLimits(It.IsAny<double>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.ValidateAdditionRateLimits(rate, productCode);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateAdditionRateLimits(It.IsAny<double>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetSumAssuredForAdditions_ValidPolicy_ReturnsAmount()
        {
            // Arrange
            string policyId = "POL123";
            DateTime date = new DateTime(2023, 1, 1);
            decimal expected = 50000m;
            _mockService.Setup(s => s.GetSumAssuredForAdditions(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetSumAssuredForAdditions(policyId, date);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetSumAssuredForAdditions(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidPolicy_ReturnsYears()
        {
            // Arrange
            string policyId = "POL123";
            DateTime date = new DateTime(2030, 1, 1);
            int expected = 10;
            _mockService.Setup(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetCompletedPolicyYears(policyId, date);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 10);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateVestingPercentage_ValidYears_ReturnsPercentage()
        {
            // Arrange
            int completed = 5;
            int total = 10;
            double expected = 0.5;
            _mockService.Setup(s => s.CalculateVestingPercentage(It.IsAny<int>(), It.IsAny<int>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateVestingPercentage(completed, total);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.5);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.CalculateVestingPercentage(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateVestedGuaranteedAdditions_ValidInputs_ReturnsAmount()
        {
            // Arrange
            string policyId = "POL123";
            decimal additions = 1000m;
            double percentage = 0.5;
            decimal expected = 500m;
            _mockService.Setup(s => s.CalculateVestedGuaranteedAdditions(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateVestedGuaranteedAdditions(policyId, additions, percentage);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 500m);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateVestedGuaranteedAdditions(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void RetrieveAdditionRuleId_ValidInputs_ReturnsRuleId()
        {
            // Arrange
            string productCode = "PROD01";
            DateTime date = new DateTime(2020, 1, 1);
            string expected = "RULE_01";
            _mockService.Setup(s => s.RetrieveAdditionRuleId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            // Act
            var result = _mockService.Object.RetrieveAdditionRuleId(productCode, date);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("RULE_02", result);
            _mockService.Verify(s => s.RetrieveAdditionRuleId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void HasLapsedPeriodAffectingAdditions_HasLapse_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL123";
            _mockService.Setup(s => s.HasLapsedPeriodAffectingAdditions(It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.HasLapsedPeriodAffectingAdditions(policyId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.HasLapsedPeriodAffectingAdditions(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void DeductUnpaidPremiumsFromAdditions_ValidInputs_ReturnsNet()
        {
            // Arrange
            decimal gross = 1000m;
            decimal unpaid = 200m;
            decimal expected = 800m;
            _mockService.Setup(s => s.DeductUnpaidPremiumsFromAdditions(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            // Act
            var result = _mockService.Object.DeductUnpaidPremiumsFromAdditions(gross, unpaid);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 800m);
            Assert.AreNotEqual(1000m, result);
            _mockService.Verify(s => s.DeductUnpaidPremiumsFromAdditions(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetMissedPremiumCount_ValidPolicy_ReturnsCount()
        {
            // Arrange
            string policyId = "POL123";
            int expected = 2;
            _mockService.Setup(s => s.GetMissedPremiumCount(It.IsAny<string>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetMissedPremiumCount(policyId);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 2);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetMissedPremiumCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetLoyaltyMultiplier_ValidInputs_ReturnsMultiplier()
        {
            // Arrange
            string policyId = "POL123";
            int years = 10;
            double expected = 1.1;
            _mockService.Setup(s => s.GetLoyaltyMultiplier(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetLoyaltyMultiplier(policyId, years);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 1.0);
            Assert.AreNotEqual(1.0, result);
            _mockService.Verify(s => s.GetLoyaltyMultiplier(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ApplyLoyaltyMultiplierToAdditions_ValidInputs_ReturnsAmount()
        {
            // Arrange
            decimal baseAdditions = 1000m;
            double multiplier = 1.1;
            decimal expected = 1100m;
            _mockService.Setup(s => s.ApplyLoyaltyMultiplierToAdditions(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            // Act
            var result = _mockService.Object.ApplyLoyaltyMultiplierToAdditions(baseAdditions, multiplier);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 1100m);
            Assert.AreNotEqual(1000m, result);
            _mockService.Verify(s => s.ApplyLoyaltyMultiplierToAdditions(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CheckMinimumTermForAdditions_MeetsRequirement_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL123";
            int minYears = 5;
            _mockService.Setup(s => s.CheckMinimumTermForAdditions(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            // Act
            var result = _mockService.Object.CheckMinimumTermForAdditions(policyId, minYears);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.CheckMinimumTermForAdditions(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetCurrencyCodeForAdditions_ValidPolicy_ReturnsCode()
        {
            // Arrange
            string policyId = "POL123";
            string expected = "USD";
            _mockService.Setup(s => s.GetCurrencyCodeForAdditions(It.IsAny<string>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetCurrencyCodeForAdditions(policyId);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == "USD");
            Assert.AreNotEqual("EUR", result);
            _mockService.Verify(s => s.GetCurrencyCodeForAdditions(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInputs_ReturnsAmount()
        {
            // Arrange
            string policyId = "POL123";
            decimal sumAssured = 50000m;
            double rate = 0.02;
            decimal expected = 1000m;
            _mockService.Setup(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateTerminalBonus(policyId, sumAssured, rate);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 1000m);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidInputs_ReturnsRate()
        {
            // Arrange
            string productCode = "PROD01";
            int year = 10;
            double expected = 0.02;
            _mockService.Setup(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetTerminalBonusRate(productCode, year);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.02);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void IsTerminalBonusGuaranteed_GuaranteedProduct_ReturnsTrue()
        {
            // Arrange
            string productCode = "PROD01";
            _mockService.Setup(s => s.IsTerminalBonusGuaranteed(It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.IsTerminalBonusGuaranteed(productCode);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsTerminalBonusGuaranteed(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSpecialGuaranteedAddition_ValidInputs_ReturnsAmount()
        {
            // Arrange
            string policyId = "POL123";
            decimal premium = 5000m;
            decimal expected = 250m;
            _mockService.Setup(s => s.CalculateSpecialGuaranteedAddition(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateSpecialGuaranteedAddition(policyId, premium);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 250m);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateSpecialGuaranteedAddition(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingDaysToMaturity_ValidInputs_ReturnsDays()
        {
            // Arrange
            string policyId = "POL123";
            DateTime current = new DateTime(2023, 1, 1);
            int expected = 365;
            _mockService.Setup(s => s.GetRemainingDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetRemainingDaysToMaturity(policyId, current);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 365);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetRemainingDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void MultipleCalls_VerifyExecutionCounts()
        {
            // Arrange
            string policyId = "POL123";
            _mockService.Setup(s => s.GetMissedPremiumCount(It.IsAny<string>())).Returns(0);

            // Act
            _mockService.Object.GetMissedPremiumCount(policyId);
            _mockService.Object.GetMissedPremiumCount(policyId);

            // Assert
            Assert.IsNotNull(policyId);
            Assert.AreEqual("POL123", policyId);
            Assert.AreNotEqual("POL999", policyId);
            _mockService.Verify(s => s.GetMissedPremiumCount(It.IsAny<string>()), Times.Exactly(2));
            _mockService.Verify(s => s.HasLapsedPeriodAffectingAdditions(It.IsAny<string>()), Times.Never());
        }
    }
}