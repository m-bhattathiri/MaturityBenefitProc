using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class LoyaltyAdditionServiceMockTests
    {
        private Mock<ILoyaltyAdditionService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ILoyaltyAdditionService>();
        }

        [TestMethod]
        public void CalculateBaseLoyaltyAddition_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal sumAssured = 100000m;
            int premiumTerm = 10;
            decimal expectedAmount = 5000m;

            _mockService.Setup(s => s.CalculateBaseLoyaltyAddition(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(expectedAmount);

            var result = _mockService.Object.CalculateBaseLoyaltyAddition(policyId, sumAssured, premiumTerm);

            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateBaseLoyaltyAddition(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBaseLoyaltyAddition_ZeroSumAssured_ReturnsZero()
        {
            string policyId = "POL124";
            decimal sumAssured = 0m;
            int premiumTerm = 10;
            decimal expectedAmount = 0m;

            _mockService.Setup(s => s.CalculateBaseLoyaltyAddition(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(expectedAmount);

            var result = _mockService.Object.CalculateBaseLoyaltyAddition(policyId, sumAssured, premiumTerm);

            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreNotEqual(100m, result);

            _mockService.Verify(s => s.CalculateBaseLoyaltyAddition(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetLoyaltyAdditionRate_ValidProduct_ReturnsRate()
        {
            string productCode = "PROD01";
            int completedYears = 5;
            double expectedRate = 0.05;

            _mockService.Setup(s => s.GetLoyaltyAdditionRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.GetLoyaltyAdditionRate(productCode, completedYears);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetLoyaltyAdditionRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForLoyaltyAddition_EligiblePolicy_ReturnsTrue()
        {
            string policyId = "POL125";
            DateTime maturityDate = new DateTime(2025, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.IsEligibleForLoyaltyAddition(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsEligibleForLoyaltyAddition(policyId, maturityDate);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsEligibleForLoyaltyAddition(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForLoyaltyAddition_IneligiblePolicy_ReturnsFalse()
        {
            string policyId = "POL126";
            DateTime maturityDate = new DateTime(2020, 1, 1);
            bool expected = false;

            _mockService.Setup(s => s.IsEligibleForLoyaltyAddition(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsEligibleForLoyaltyAddition(policyId, maturityDate);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.IsEligibleForLoyaltyAddition(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCompletedPremiumYears_ValidDates_ReturnsYears()
        {
            string policyId = "POL127";
            DateTime inceptionDate = new DateTime(2015, 1, 1);
            int expectedYears = 8;

            _mockService.Setup(s => s.GetCompletedPremiumYears(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedYears);

            var result = _mockService.Object.GetCompletedPremiumYears(policyId, inceptionDate);

            Assert.AreEqual(expectedYears, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetCompletedPremiumYears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GenerateLoyaltyTransactionId_ValidInputs_ReturnsId()
        {
            string policyId = "POL128";
            DateTime processingDate = new DateTime(2023, 10, 1);
            string expectedId = "TXN-POL128-20231001";

            _mockService.Setup(s => s.GenerateLoyaltyTransactionId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedId);

            var result = _mockService.Object.GenerateLoyaltyTransactionId(policyId, processingDate);

            Assert.AreEqual(expectedId, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("TXN"));
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GenerateLoyaltyTransactionId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ComputeFinalLoyaltyAmount_ValidInputs_ReturnsComputedAmount()
        {
            string policyId = "POL129";
            decimal baseAmount = 1000m;
            double multiplier = 1.5;
            decimal expectedAmount = 1500m;

            _mockService.Setup(s => s.ComputeFinalLoyaltyAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedAmount);

            var result = _mockService.Object.ComputeFinalLoyaltyAmount(policyId, baseAmount, multiplier);

            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > baseAmount);
            Assert.AreNotEqual(baseAmount, result);

            _mockService.Verify(s => s.ComputeFinalLoyaltyAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void HasCompletedPremiumTerm_Completed_ReturnsTrue()
        {
            string policyId = "POL130";
            int requiredTerm = 10;
            bool expected = true;

            _mockService.Setup(s => s.HasCompletedPremiumTerm(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.HasCompletedPremiumTerm(policyId, requiredTerm);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.HasCompletedPremiumTerm(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoyaltyMultiplier_HighPersistency_ReturnsHighMultiplier()
        {
            int persistencyScore = 95;
            double baseRate = 1.0;
            double expectedMultiplier = 1.2;

            _mockService.Setup(s => s.CalculateLoyaltyMultiplier(It.IsAny<int>(), It.IsAny<double>())).Returns(expectedMultiplier);

            var result = _mockService.Object.CalculateLoyaltyMultiplier(persistencyScore, baseRate);

            Assert.AreEqual(expectedMultiplier, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > baseRate);
            Assert.AreNotEqual(baseRate, result);

            _mockService.Verify(s => s.CalculateLoyaltyMultiplier(It.IsAny<int>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetAccruedLoyaltyValue_ValidPolicy_ReturnsValue()
        {
            string policyId = "POL131";
            DateTime calculationDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 2500m;

            _mockService.Setup(s => s.GetAccruedLoyaltyValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetAccruedLoyaltyValue(policyId, calculationDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetAccruedLoyaltyValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysSinceLastAddition_ValidDates_ReturnsDays()
        {
            string policyId = "POL132";
            DateTime currentDate = new DateTime(2023, 12, 31);
            int expectedDays = 365;

            _mockService.Setup(s => s.CalculateDaysSinceLastAddition(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDays);

            var result = _mockService.Object.CalculateDaysSinceLastAddition(policyId, currentDate);

            Assert.AreEqual(expectedDays, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.CalculateDaysSinceLastAddition(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetLoyaltyFundCode_ValidCategory_ReturnsCode()
        {
            string productCategory = "ULIP";
            int issueYear = 2015;
            string expectedCode = "FUND-ULIP-2015";

            _mockService.Setup(s => s.GetLoyaltyFundCode(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedCode);

            var result = _mockService.Object.GetLoyaltyFundCode(productCategory, issueYear);

            Assert.AreEqual(expectedCode, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("ULIP"));
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GetLoyaltyFundCode(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ValidateLoyaltyAdditionRules_ValidRules_ReturnsTrue()
        {
            string policyId = "POL133";
            decimal calculatedAmount = 5000m;
            bool expected = true;

            _mockService.Setup(s => s.ValidateLoyaltyAdditionRules(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.ValidateLoyaltyAdditionRules(policyId, calculatedAmount);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateLoyaltyAdditionRules(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProratedAddition_ValidInputs_ReturnsProratedAmount()
        {
            string policyId = "POL134";
            decimal annualAmount = 3650m;
            int daysActive = 100;
            decimal expectedAmount = 1000m;

            _mockService.Setup(s => s.CalculateProratedAddition(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(expectedAmount);

            var result = _mockService.Object.CalculateProratedAddition(policyId, annualAmount, daysActive);

            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < annualAmount);
            Assert.AreNotEqual(annualAmount, result);

            _mockService.Verify(s => s.CalculateProratedAddition(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetBonusInterestRate_ValidFund_ReturnsRate()
        {
            string fundCode = "FUND1";
            DateTime effectiveDate = new DateTime(2023, 1, 1);
            double expectedRate = 0.08;

            _mockService.Setup(s => s.GetBonusInterestRate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedRate);

            var result = _mockService.Object.GetBonusInterestRate(fundCode, effectiveDate);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetBonusInterestRate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetMissedPremiumCount_NoMissed_ReturnsZero()
        {
            string policyId = "POL135";
            int expectedCount = 0;

            _mockService.Setup(s => s.GetMissedPremiumCount(It.IsAny<string>())).Returns(expectedCount);

            var result = _mockService.Object.GetMissedPremiumCount(policyId);

            Assert.AreEqual(expectedCount, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreNotEqual(1, result);

            _mockService.Verify(s => s.GetMissedPremiumCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void DetermineLoyaltyCategory_ValidInputs_ReturnsCategory()
        {
            int completedYears = 10;
            decimal sumAssured = 500000m;
            string expectedCategory = "GOLD";

            _mockService.Setup(s => s.DetermineLoyaltyCategory(It.IsAny<int>(), It.IsAny<decimal>())).Returns(expectedCategory);

            var result = _mockService.Object.DetermineLoyaltyCategory(completedYears, sumAssured);

            Assert.AreEqual(expectedCategory, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("SILVER", result);

            _mockService.Verify(s => s.DetermineLoyaltyCategory(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetSurrenderLoyaltyValue_ValidInputs_ReturnsValue()
        {
            string policyId = "POL136";
            decimal totalPremiumsPaid = 100000m;
            double surrenderFactor = 0.5;
            decimal expectedValue = 50000m;

            _mockService.Setup(s => s.GetSurrenderLoyaltyValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.GetSurrenderLoyaltyValue(policyId, totalPremiumsPaid, surrenderFactor);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < totalPremiumsPaid);
            Assert.AreNotEqual(totalPremiumsPaid, result);

            _mockService.Verify(s => s.GetSurrenderLoyaltyValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyInForce_ActivePolicy_ReturnsTrue()
        {
            string policyId = "POL137";
            DateTime checkDate = new DateTime(2023, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.IsPolicyInForce(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsPolicyInForce(policyId, checkDate);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsPolicyInForce(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ComputePersistencyRatio_ValidInputs_ReturnsRatio()
        {
            string policyId = "POL138";
            int expectedPremiums = 120;
            int paidPremiums = 114;
            double expectedRatio = 0.95;

            _mockService.Setup(s => s.ComputePersistencyRatio(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedRatio);

            var result = _mockService.Object.ComputePersistencyRatio(policyId, expectedPremiums, paidPremiums);

            Assert.AreEqual(expectedRatio, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0.9);
            Assert.AreNotEqual(1.0, result);

            _mockService.Verify(s => s.ComputePersistencyRatio(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetLoyaltyTierLevel_HighRatio_ReturnsTopTier()
        {
            double persistencyRatio = 0.98;
            int expectedTier = 1;

            _mockService.Setup(s => s.GetLoyaltyTierLevel(It.IsAny<double>())).Returns(expectedTier);

            var result = _mockService.Object.GetLoyaltyTierLevel(persistencyRatio);

            Assert.AreEqual(expectedTier, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 1);
            Assert.AreNotEqual(2, result);

            _mockService.Verify(s => s.GetLoyaltyTierLevel(It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSpecialLoyaltyBonus_HighValue_ReturnsBonus()
        {
            string policyId = "POL139";
            decimal baseLoyalty = 10000m;
            bool isHighValuePolicy = true;
            decimal expectedBonus = 2000m;

            _mockService.Setup(s => s.CalculateSpecialLoyaltyBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<bool>())).Returns(expectedBonus);

            var result = _mockService.Object.CalculateSpecialLoyaltyBonus(policyId, baseLoyalty, isHighValuePolicy);

            Assert.AreEqual(expectedBonus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateSpecialLoyaltyBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void CheckSpecialBonusEligibility_EligibleTier_ReturnsTrue()
        {
            string policyId = "POL140";
            int tierLevel = 1;
            bool expected = true;

            _mockService.Setup(s => s.CheckSpecialBonusEligibility(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.CheckSpecialBonusEligibility(policyId, tierLevel);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.CheckSpecialBonusEligibility(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetApprovalStatusCode_ValidInputs_ReturnsCode()
        {
            decimal finalLoyaltyAmount = 50000m;
            int tierLevel = 1;
            string expectedCode = "APP-AUTO";

            _mockService.Setup(s => s.GetApprovalStatusCode(It.IsAny<decimal>(), It.IsAny<int>())).Returns(expectedCode);

            var result = _mockService.Object.GetApprovalStatusCode(finalLoyaltyAmount, tierLevel);

            Assert.AreEqual(expectedCode, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("APP"));
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GetApprovalStatusCode(It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ApplyTaxDeductionsToLoyalty_ValidInputs_ReturnsNet()
        {
            string policyId = "POL141";
            decimal grossLoyaltyAmount = 10000m;
            double taxRate = 0.1;
            decimal expectedNet = 9000m;

            _mockService.Setup(s => s.ApplyTaxDeductionsToLoyalty(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedNet);

            var result = _mockService.Object.ApplyTaxDeductionsToLoyalty(policyId, grossLoyaltyAmount, taxRate);

            Assert.AreEqual(expectedNet, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < grossLoyaltyAmount);
            Assert.AreNotEqual(grossLoyaltyAmount, result);

            _mockService.Verify(s => s.ApplyTaxDeductionsToLoyalty(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }
    }
}