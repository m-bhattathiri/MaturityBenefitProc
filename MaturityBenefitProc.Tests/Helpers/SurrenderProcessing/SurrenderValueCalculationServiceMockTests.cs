using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.SurrenderProcessing;

namespace MaturityBenefitProc.Tests.Helpers.SurrenderProcessing
{
    [TestClass]
    public class SurrenderValueCalculationServiceMockTests
    {
        private Mock<ISurrenderValueCalculationService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ISurrenderValueCalculationService>();
        }

        [TestMethod]
        public void CalculateGuaranteedSurrenderValue_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime surrenderDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 15000.50m;

            _mockService.Setup(s => s.CalculateGuaranteedSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateGuaranteedSurrenderValue(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateGuaranteedSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSpecialSurrenderValue_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL124";
            DateTime surrenderDate = new DateTime(2023, 2, 1);
            decimal expectedValue = 20000.75m;

            _mockService.Setup(s => s.CalculateSpecialSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateSpecialSurrenderValue(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 10000m);
            Assert.AreNotEqual(100m, result);

            _mockService.Verify(s => s.CalculateSpecialSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForSurrender_Eligible_ReturnsTrue()
        {
            string policyId = "POL125";
            DateTime requestDate = new DateTime(2023, 3, 1);

            _mockService.Setup(s => s.IsPolicyEligibleForSurrender(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.IsPolicyEligibleForSurrender(policyId, requestDate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.IsPolicyEligibleForSurrender(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForSurrender_NotEligible_ReturnsFalse()
        {
            string policyId = "POL126";
            DateTime requestDate = new DateTime(2023, 3, 1);

            _mockService.Setup(s => s.IsPolicyEligibleForSurrender(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(false);

            var result = _mockService.Object.IsPolicyEligibleForSurrender(policyId, requestDate);

            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);

            _mockService.Verify(s => s.IsPolicyEligibleForSurrender(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidInput_ReturnsYears()
        {
            string policyId = "POL127";
            DateTime surrenderDate = new DateTime(2023, 4, 1);
            int expectedYears = 5;

            _mockService.Setup(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedYears);

            var result = _mockService.Object.GetCompletedPolicyYears(policyId, surrenderDate);

            Assert.AreEqual(expectedYears, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetSurrenderValueFactor_ValidInput_ReturnsFactor()
        {
            int completedYears = 5;
            string planCode = "PLAN_A";
            double expectedFactor = 0.35;

            _mockService.Setup(s => s.GetSurrenderValueFactor(It.IsAny<int>(), It.IsAny<string>())).Returns(expectedFactor);

            var result = _mockService.Object.GetSurrenderValueFactor(completedYears, planCode);

            Assert.AreEqual(expectedFactor, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0.0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetSurrenderValueFactor(It.IsAny<int>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateAccruedBonuses_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL128";
            DateTime surrenderDate = new DateTime(2023, 5, 1);
            decimal expectedValue = 5000.00m;

            _mockService.Setup(s => s.CalculateAccruedBonuses(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateAccruedBonuses(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateAccruedBonuses(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL129";
            DateTime surrenderDate = new DateTime(2023, 6, 1);
            decimal expectedValue = 2500.00m;

            _mockService.Setup(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTerminalBonus(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetSpecialSurrenderValueFactor_ValidInput_ReturnsFactor()
        {
            int completedYears = 10;
            string planCode = "PLAN_B";
            double expectedFactor = 0.65;

            _mockService.Setup(s => s.GetSpecialSurrenderValueFactor(It.IsAny<int>(), It.IsAny<string>())).Returns(expectedFactor);

            var result = _mockService.Object.GetSpecialSurrenderValueFactor(completedYears, planCode);

            Assert.AreEqual(expectedFactor, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0.0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetSpecialSurrenderValueFactor(It.IsAny<int>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalPaidPremiums_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL130";
            DateTime surrenderDate = new DateTime(2023, 7, 1);
            decimal expectedValue = 50000.00m;

            _mockService.Setup(s => s.CalculateTotalPaidPremiums(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalPaidPremiums(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTotalPaidPremiums(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateSurrenderRequest_ValidInput_ReturnsTrue()
        {
            string policyId = "POL131";
            string customerId = "CUST001";
            DateTime requestDate = new DateTime(2023, 8, 1);

            _mockService.Setup(s => s.ValidateSurrenderRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.ValidateSurrenderRequest(policyId, customerId, requestDate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.ValidateSurrenderRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysSinceLastPremiumPaid_ValidInput_ReturnsDays()
        {
            string policyId = "POL132";
            DateTime surrenderDate = new DateTime(2023, 9, 1);
            int expectedDays = 45;

            _mockService.Setup(s => s.GetDaysSinceLastPremiumPaid(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDays);

            var result = _mockService.Object.GetDaysSinceLastPremiumPaid(policyId, surrenderDate);

            Assert.AreEqual(expectedDays, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetDaysSinceLastPremiumPaid(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoanOutstanding_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL133";
            DateTime surrenderDate = new DateTime(2023, 10, 1);
            decimal expectedValue = 10000.00m;

            _mockService.Setup(s => s.CalculateLoanOutstanding(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateLoanOutstanding(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateLoanOutstanding(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoanInterestOutstanding_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL134";
            DateTime surrenderDate = new DateTime(2023, 11, 1);
            decimal expectedValue = 1500.00m;

            _mockService.Setup(s => s.CalculateLoanInterestOutstanding(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateLoanInterestOutstanding(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateLoanInterestOutstanding(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateNetSurrenderValue_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL135";
            DateTime surrenderDate = new DateTime(2023, 12, 1);
            decimal expectedValue = 25000.00m;

            _mockService.Setup(s => s.CalculateNetSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateNetSurrenderValue(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateNetSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetSurrenderStatus_ValidInput_ReturnsStatus()
        {
            string policyId = "POL136";
            string expectedStatus = "Pending";

            _mockService.Setup(s => s.GetSurrenderStatus(It.IsAny<string>())).Returns(expectedStatus);

            var result = _mockService.Object.GetSurrenderStatus(policyId);

            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("Approved", result);

            _mockService.Verify(s => s.GetSurrenderStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPaidUpValueRatio_ValidInput_ReturnsRatio()
        {
            string policyId = "POL137";
            DateTime surrenderDate = new DateTime(2024, 1, 1);
            double expectedRatio = 0.5;

            _mockService.Setup(s => s.GetPaidUpValueRatio(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedRatio);

            var result = _mockService.Object.GetPaidUpValueRatio(policyId, surrenderDate);

            Assert.AreEqual(expectedRatio, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0.0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetPaidUpValueRatio(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculatePaidUpValue_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL138";
            DateTime surrenderDate = new DateTime(2024, 2, 1);
            decimal expectedValue = 12000.00m;

            _mockService.Setup(s => s.CalculatePaidUpValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculatePaidUpValue(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculatePaidUpValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void HasActiveAssignments_HasAssignments_ReturnsTrue()
        {
            string policyId = "POL139";

            _mockService.Setup(s => s.HasActiveAssignments(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.HasActiveAssignments(policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.HasActiveAssignments(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingPolicyTerm_ValidInput_ReturnsTerm()
        {
            string policyId = "POL140";
            DateTime surrenderDate = new DateTime(2024, 3, 1);
            int expectedTerm = 15;

            _mockService.Setup(s => s.GetRemainingPolicyTerm(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedTerm);

            var result = _mockService.Object.GetRemainingPolicyTerm(policyId, surrenderDate);

            Assert.AreEqual(expectedTerm, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetRemainingPolicyTerm(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSurrenderCharges_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL141";
            DateTime surrenderDate = new DateTime(2024, 4, 1);
            decimal expectedValue = 500.00m;

            _mockService.Setup(s => s.CalculateSurrenderCharges(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateSurrenderCharges(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateSurrenderCharges(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GenerateSurrenderQuoteId_ValidInput_ReturnsQuoteId()
        {
            string policyId = "POL142";
            DateTime requestDate = new DateTime(2024, 5, 1);
            string expectedQuoteId = "QUOTE-999";

            _mockService.Setup(s => s.GenerateSurrenderQuoteId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedQuoteId);

            var result = _mockService.Object.GenerateSurrenderQuoteId(policyId, requestDate);

            Assert.AreEqual(expectedQuoteId, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("QUOTE-000", result);

            _mockService.Verify(s => s.GenerateSurrenderQuoteId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsWithinCoolingOffPeriod_WithinPeriod_ReturnsTrue()
        {
            string policyId = "POL143";
            DateTime requestDate = new DateTime(2024, 6, 1);

            _mockService.Setup(s => s.IsWithinCoolingOffPeriod(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.IsWithinCoolingOffPeriod(policyId, requestDate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);

            _mockService.Verify(s => s.IsWithinCoolingOffPeriod(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateVestedReversionaryBonus_ValidInput_ReturnsExpectedValue()
        {
            string policyId = "POL144";
            DateTime surrenderDate = new DateTime(2024, 7, 1);
            decimal expectedValue = 3000.00m;

            _mockService.Setup(s => s.CalculateVestedReversionaryBonus(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateVestedReversionaryBonus(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateVestedReversionaryBonus(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetDiscountRate_ValidInput_ReturnsRate()
        {
            string planCode = "PLAN_C";
            DateTime surrenderDate = new DateTime(2024, 8, 1);
            double expectedRate = 0.05;

            _mockService.Setup(s => s.GetDiscountRate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedRate);

            var result = _mockService.Object.GetDiscountRate(planCode, surrenderDate);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0.0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetDiscountRate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDiscountedValue_ValidInput_ReturnsExpectedValue()
        {
            decimal futureValue = 10000m;
            double discountRate = 0.05;
            int remainingYears = 5;
            decimal expectedValue = 7835.26m;

            _mockService.Setup(s => s.CalculateDiscountedValue(It.IsAny<decimal>(), It.IsAny<double>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateDiscountedValue(futureValue, discountRate, remainingYears);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateDiscountedValue(It.IsAny<decimal>(), It.IsAny<double>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void MultipleCalls_VerifyTimes()
        {
            string policyId = "POL999";
            DateTime date = DateTime.Now;

            _mockService.Setup(s => s.GetSurrenderStatus(It.IsAny<string>())).Returns("Active");
            _mockService.Setup(s => s.HasActiveAssignments(It.IsAny<string>())).Returns(false);

            _mockService.Object.GetSurrenderStatus(policyId);
            _mockService.Object.GetSurrenderStatus(policyId);
            _mockService.Object.HasActiveAssignments(policyId);

            _mockService.Verify(s => s.GetSurrenderStatus(It.IsAny<string>()), Times.Exactly(2));
            _mockService.Verify(s => s.HasActiveAssignments(It.IsAny<string>()), Times.Once());
            _mockService.Verify(s => s.CalculateGuaranteedSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never());

            Assert.IsNotNull(_mockService.Object);
            Assert.AreEqual("Active", _mockService.Object.GetSurrenderStatus(policyId));
            Assert.IsFalse(_mockService.Object.HasActiveAssignments(policyId));
        }
    }
}