using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans;

namespace MaturityBenefitProc.Tests.Helpers.FundValueAndUnitLinkedPlans
{
    [TestClass]
    public class MortalityChargeRefundServiceMockTests
    {
        private Mock<IMortalityChargeRefundService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IMortalityChargeRefundService>();
        }

        [TestMethod]
        public void CalculateTotalRefundAmount_ValidInputs_ReturnsExpectedAmount()
        {
            // Arrange
            string policyId = "POL12345";
            DateTime calcDate = new DateTime(2023, 1, 1);
            decimal expectedAmount = 1500.50m;
            
            _mockService.Setup(s => s.CalculateTotalRefundAmount(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expectedAmount);

            // Act
            var result = _mockService.Object.CalculateTotalRefundAmount(policyId, calcDate);

            // Assert
            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateTotalRefundAmount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalRefundAmount_ZeroRefund_ReturnsZero()
        {
            // Arrange
            string policyId = "POL99999";
            DateTime calcDate = new DateTime(2023, 5, 1);
            decimal expectedAmount = 0m;
            
            _mockService.Setup(s => s.CalculateTotalRefundAmount(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expectedAmount);

            // Act
            var result = _mockService.Object.CalculateTotalRefundAmount(policyId, calcDate);

            // Assert
            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateTotalRefundAmount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(1));
        }

        [TestMethod]
        public void IsPolicyEligibleForRefund_EligiblePolicy_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL11111";
            
            _mockService.Setup(s => s.IsPolicyEligibleForRefund(It.IsAny<string>()))
                        .Returns(true);

            // Act
            var result = _mockService.Object.IsPolicyEligibleForRefund(policyId);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);
            Assert.IsNotNull(result);
            
            _mockService.Verify(s => s.IsPolicyEligibleForRefund(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForRefund_IneligiblePolicy_ReturnsFalse()
        {
            // Arrange
            string policyId = "POL22222";
            
            _mockService.Setup(s => s.IsPolicyEligibleForRefund(It.IsAny<string>()))
                        .Returns(false);

            // Act
            var result = _mockService.Object.IsPolicyEligibleForRefund(policyId);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.AreNotEqual(true, result);
            Assert.IsNotNull(result);
            
            _mockService.Verify(s => s.IsPolicyEligibleForRefund(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void GetMonthlyMortalityCharge_ValidInputs_ReturnsExpectedCharge()
        {
            // Arrange
            string policyId = "POL33333";
            int year = 5;
            int month = 6;
            decimal expectedCharge = 45.75m;
            
            _mockService.Setup(s => s.GetMonthlyMortalityCharge(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                        .Returns(expectedCharge);

            // Act
            var result = _mockService.Object.GetMonthlyMortalityCharge(policyId, year, month);

            // Assert
            Assert.AreEqual(expectedCharge, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.GetMonthlyMortalityCharge(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetRefundPercentage_ValidInputs_ReturnsExpectedPercentage()
        {
            // Arrange
            string productId = "PROD_A";
            int term = 10;
            double expectedPercentage = 0.50;
            
            _mockService.Setup(s => s.GetRefundPercentage(It.IsAny<string>(), It.IsAny<int>()))
                        .Returns(expectedPercentage);

            // Act
            var result = _mockService.Object.GetRefundPercentage(productId, term);

            // Assert
            Assert.AreEqual(expectedPercentage, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            
            _mockService.Verify(s => s.GetRefundPercentage(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalMonthsCharged_ValidPolicy_ReturnsExpectedMonths()
        {
            // Arrange
            string policyId = "POL44444";
            int expectedMonths = 120;
            
            _mockService.Setup(s => s.GetTotalMonthsCharged(It.IsAny<string>()))
                        .Returns(expectedMonths);

            // Act
            var result = _mockService.Object.GetTotalMonthsCharged(policyId);

            // Assert
            Assert.AreEqual(expectedMonths, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.GetTotalMonthsCharged(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRefundStatus_ProcessedPolicy_ReturnsProcessed()
        {
            // Arrange
            string policyId = "POL55555";
            string expectedStatus = "Processed";
            
            _mockService.Setup(s => s.GetRefundStatus(It.IsAny<string>()))
                        .Returns(expectedStatus);

            // Act
            var result = _mockService.Object.GetRefundStatus(policyId);

            // Assert
            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("Pending", result);
            Assert.IsTrue(result.Length > 0);
            
            _mockService.Verify(s => s.GetRefundStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRefundStatus_PendingPolicy_ReturnsPending()
        {
            // Arrange
            string policyId = "POL66666";
            string expectedStatus = "Pending";
            
            _mockService.Setup(s => s.GetRefundStatus(It.IsAny<string>()))
                        .Returns(expectedStatus);

            // Act
            var result = _mockService.Object.GetRefundStatus(policyId);

            // Assert
            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("Processed", result);
            Assert.IsTrue(result.Length > 0);
            
            _mockService.Verify(s => s.GetRefundStatus(It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public void CalculateInterestOnRefund_ValidInputs_ReturnsExpectedInterest()
        {
            // Arrange
            decimal baseAmount = 1000m;
            double rate = 0.05;
            int days = 30;
            decimal expectedInterest = 4.11m;
            
            _mockService.Setup(s => s.CalculateInterestOnRefund(It.IsAny<decimal>(), It.IsAny<double>(), It.IsAny<int>()))
                        .Returns(expectedInterest);

            // Act
            var result = _mockService.Object.CalculateInterestOnRefund(baseAmount, rate, days);

            // Assert
            Assert.AreEqual(expectedInterest, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateInterestOnRefund(It.IsAny<decimal>(), It.IsAny<double>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ValidateFundValueSufficiency_SufficientFunds_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL77777";
            decimal required = 500m;
            
            _mockService.Setup(s => s.ValidateFundValueSufficiency(It.IsAny<string>(), It.IsAny<decimal>()))
                        .Returns(true);

            // Act
            var result = _mockService.Object.ValidateFundValueSufficiency(policyId, required);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            
            _mockService.Verify(s => s.ValidateFundValueSufficiency(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void ValidateFundValueSufficiency_InsufficientFunds_ReturnsFalse()
        {
            // Arrange
            string policyId = "POL88888";
            decimal required = 50000m;
            
            _mockService.Setup(s => s.ValidateFundValueSufficiency(It.IsAny<string>(), It.IsAny<decimal>()))
                        .Returns(false);

            // Act
            var result = _mockService.Object.ValidateFundValueSufficiency(policyId, required);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            
            _mockService.Verify(s => s.ValidateFundValueSufficiency(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicablePolicyTerm_ValidPolicy_ReturnsExpectedTerm()
        {
            // Arrange
            string policyId = "POL99999";
            int expectedTerm = 15;
            
            _mockService.Setup(s => s.GetApplicablePolicyTerm(It.IsAny<string>()))
                        .Returns(expectedTerm);

            // Act
            var result = _mockService.Object.GetApplicablePolicyTerm(policyId);

            // Assert
            Assert.AreEqual(expectedTerm, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.GetApplicablePolicyTerm(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetMortalityRate_ValidInputs_ReturnsExpectedRate()
        {
            // Arrange
            int age = 35;
            string gender = "M";
            double expectedRate = 0.0015;
            
            _mockService.Setup(s => s.GetMortalityRate(It.IsAny<int>(), It.IsAny<string>()))
                        .Returns(expectedRate);

            // Act
            var result = _mockService.Object.GetMortalityRate(age, gender);

            // Assert
            Assert.AreEqual(expectedRate, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            
            _mockService.Verify(s => s.GetMortalityRate(It.IsAny<int>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetSumAtRisk_ValidInputs_ReturnsExpectedSum()
        {
            // Arrange
            string policyId = "POL123123";
            DateTime date = new DateTime(2023, 1, 1);
            decimal expectedSum = 100000m;
            
            _mockService.Setup(s => s.GetSumAtRisk(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expectedSum);

            // Act
            var result = _mockService.Object.GetSumAtRisk(policyId, date);

            // Assert
            Assert.AreEqual(expectedSum, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.GetSumAtRisk(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GenerateRefundTransactionId_ValidInputs_ReturnsExpectedId()
        {
            // Arrange
            string policyId = "POL456456";
            DateTime date = new DateTime(2023, 1, 1);
            string expectedId = "TXN-POL456456-20230101";
            
            _mockService.Setup(s => s.GenerateRefundTransactionId(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expectedId);

            // Act
            var result = _mockService.Object.GenerateRefundTransactionId(policyId, date);

            // Assert
            Assert.AreEqual(expectedId, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("TXN"));
            Assert.AreNotEqual(string.Empty, result);
            
            _mockService.Verify(s => s.GenerateRefundTransactionId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void HasPreviousRefundBeenProcessed_ProcessedPolicy_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL789789";
            
            _mockService.Setup(s => s.HasPreviousRefundBeenProcessed(It.IsAny<string>()))
                        .Returns(true);

            // Act
            var result = _mockService.Object.HasPreviousRefundBeenProcessed(policyId);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            
            _mockService.Verify(s => s.HasPreviousRefundBeenProcessed(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void HasPreviousRefundBeenProcessed_UnprocessedPolicy_ReturnsFalse()
        {
            // Arrange
            string policyId = "POL000000";
            
            _mockService.Setup(s => s.HasPreviousRefundBeenProcessed(It.IsAny<string>()))
                        .Returns(false);

            // Act
            var result = _mockService.Object.HasPreviousRefundBeenProcessed(policyId);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            
            _mockService.Verify(s => s.HasPreviousRefundBeenProcessed(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxOnRefund_ValidInputs_ReturnsExpectedTax()
        {
            // Arrange
            decimal amount = 1000m;
            double rate = 0.10;
            decimal expectedTax = 100m;
            
            _mockService.Setup(s => s.CalculateTaxOnRefund(It.IsAny<decimal>(), It.IsAny<double>()))
                        .Returns(expectedTax);

            // Act
            var result = _mockService.Object.CalculateTaxOnRefund(amount, rate);

            // Assert
            Assert.AreEqual(expectedTax, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateTaxOnRefund(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysSinceMaturity_ValidInputs_ReturnsExpectedDays()
        {
            // Arrange
            string policyId = "POL111222";
            DateTime date = new DateTime(2023, 2, 1);
            int expectedDays = 31;
            
            _mockService.Setup(s => s.GetDaysSinceMaturity(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expectedDays);

            // Act
            var result = _mockService.Object.GetDaysSinceMaturity(policyId, date);

            // Assert
            Assert.AreEqual(expectedDays, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.GetDaysSinceMaturity(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPersistencyBonusRatio_ValidPolicy_ReturnsExpectedRatio()
        {
            // Arrange
            string policyId = "POL333444";
            double expectedRatio = 1.05;
            
            _mockService.Setup(s => s.GetPersistencyBonusRatio(It.IsAny<string>()))
                        .Returns(expectedRatio);

            // Act
            var result = _mockService.Object.GetPersistencyBonusRatio(policyId);

            // Assert
            Assert.AreEqual(expectedRatio, result);
            Assert.IsTrue(result > 1.0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            
            _mockService.Verify(s => s.GetPersistencyBonusRatio(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalAccumulatedCharges_ValidPolicy_ReturnsExpectedTotal()
        {
            // Arrange
            string policyId = "POL555666";
            decimal expectedTotal = 5000m;
            
            _mockService.Setup(s => s.GetTotalAccumulatedCharges(It.IsAny<string>()))
                        .Returns(expectedTotal);

            // Act
            var result = _mockService.Object.GetTotalAccumulatedCharges(policyId);

            // Assert
            Assert.AreEqual(expectedTotal, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.GetTotalAccumulatedCharges(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void VerifyRiderExclusions_ExcludedRider_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL777888";
            string riderCode = "RIDER_X";
            
            _mockService.Setup(s => s.VerifyRiderExclusions(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(true);

            // Act
            var result = _mockService.Object.VerifyRiderExclusions(policyId, riderCode);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            
            _mockService.Verify(s => s.VerifyRiderExclusions(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void VerifyRiderExclusions_IncludedRider_ReturnsFalse()
        {
            // Arrange
            string policyId = "POL999000";
            string riderCode = "RIDER_Y";
            
            _mockService.Setup(s => s.VerifyRiderExclusions(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(false);

            // Act
            var result = _mockService.Object.VerifyRiderExclusions(policyId, riderCode);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            
            _mockService.Verify(s => s.VerifyRiderExclusions(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalRefundAmount_MultipleCalls_ReturnsExpectedAmount()
        {
            // Arrange
            string policyId = "POL12345";
            DateTime calcDate = new DateTime(2023, 1, 1);
            decimal expectedAmount = 1500.50m;
            
            _mockService.Setup(s => s.CalculateTotalRefundAmount(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expectedAmount);

            // Act
            var result1 = _mockService.Object.CalculateTotalRefundAmount(policyId, calcDate);
            var result2 = _mockService.Object.CalculateTotalRefundAmount(policyId, calcDate);

            // Assert
            Assert.AreEqual(expectedAmount, result1);
            Assert.AreEqual(expectedAmount, result2);
            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            
            _mockService.Verify(s => s.CalculateTotalRefundAmount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(2));
        }
    }
}