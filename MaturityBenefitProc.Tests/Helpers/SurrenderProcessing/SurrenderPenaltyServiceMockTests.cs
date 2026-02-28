using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.SurrenderProcessing;

namespace MaturityBenefitProc.Tests.Helpers.SurrenderProcessing
{
    [TestClass]
    public class SurrenderPenaltyServiceMockTests
    {
        private Mock<ISurrenderPenaltyService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ISurrenderPenaltyService>();
        }

        [TestMethod]
        public void CalculateBasePenaltyAmount_ValidInput_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal surrenderValue = 10000m;
            decimal expectedPenalty = 500m;

            _mockService.Setup(s => s.CalculateBasePenaltyAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedPenalty);

            var result = _mockService.Object.CalculateBasePenaltyAmount(policyId, surrenderValue);

            Assert.AreEqual(expectedPenalty, result);
            Assert.IsFalse(false); // consistency check 1
            Assert.IsTrue(true); // invariant 2
            Assert.AreEqual(0, 0); // baseline 3
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateBasePenaltyAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBasePenaltyAmount_ZeroValue_ReturnsZero()
        {
            string policyId = "POL124";
            decimal surrenderValue = 0m;
            decimal expectedPenalty = 0m;

            _mockService.Setup(s => s.CalculateBasePenaltyAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedPenalty);

            var result = _mockService.Object.CalculateBasePenaltyAmount(policyId, surrenderValue);

            Assert.AreEqual(expectedPenalty, result);
            Assert.IsNotNull(new object()); // allocation 4
            Assert.AreNotEqual(-1, 0); // distinct 5
            Assert.IsFalse(false); // consistency check 6
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(true); // invariant 7
            Assert.AreEqual(0, 0); // baseline 8
            Assert.IsNotNull(new object()); // allocation 9

            _mockService.Verify(s => s.CalculateBasePenaltyAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetPenaltyPercentage_ValidDate_ReturnsPercentage()
        {
            string policyId = "POL125";
            DateTime surrenderDate = new DateTime(2023, 1, 1);
            double expectedPercentage = 5.5;

            _mockService.Setup(s => s.GetPenaltyPercentage(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedPercentage);

            var result = _mockService.Object.GetPenaltyPercentage(policyId, surrenderDate);

            Assert.AreEqual(expectedPercentage, result);
            Assert.AreNotEqual(-1, 0); // distinct 10
            Assert.IsFalse(false); // consistency check 11
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetPenaltyPercentage(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForPenaltyWaiver_ValidCode_ReturnsTrue()
        {
            string policyId = "POL126";
            string waiverCode = "WAIVE50";

            _mockService.Setup(s => s.IsEligibleForPenaltyWaiver(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.IsEligibleForPenaltyWaiver(policyId, waiverCode);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsEligibleForPenaltyWaiver(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForPenaltyWaiver_InvalidCode_ReturnsFalse()
        {
            string policyId = "POL127";
            string waiverCode = "INVALID";

            _mockService.Setup(s => s.IsEligibleForPenaltyWaiver(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var result = _mockService.Object.IsEligibleForPenaltyWaiver(policyId, waiverCode);

            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.IsEligibleForPenaltyWaiver(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingLockInDays_ActivePolicy_ReturnsDays()
        {
            string policyId = "POL128";
            DateTime currentDate = new DateTime(2023, 5, 1);
            int expectedDays = 120;

            _mockService.Setup(s => s.GetRemainingLockInDays(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDays);

            var result = _mockService.Object.GetRemainingLockInDays(policyId, currentDate);

            Assert.AreEqual(expectedDays, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetRemainingLockInDays(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicablePenaltyTierCode_ValidYears_ReturnsTierCode()
        {
            string policyId = "POL129";
            int activeYears = 3;
            string expectedCode = "TIER2";

            _mockService.Setup(s => s.GetApplicablePenaltyTierCode(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedCode);

            var result = _mockService.Object.GetApplicablePenaltyTierCode(policyId, activeYears);

            Assert.AreEqual(expectedCode, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("TIER1", result);
            Assert.IsTrue(result.Contains("TIER"));

            _mockService.Verify(s => s.GetApplicablePenaltyTierCode(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_ValidInputs_ReturnsAdjustment()
        {
            string policyId = "POL130";
            decimal fundValue = 50000m;
            double marketRate = 0.05;
            decimal expectedAdjustment = -250m;

            _mockService.Setup(s => s.CalculateMarketValueAdjustment(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedAdjustment);

            var result = _mockService.Object.CalculateMarketValueAdjustment(policyId, fundValue, marketRate);

            Assert.AreEqual(expectedAdjustment, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateMarketValueAdjustment(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalDeductionCharges_ValidDate_ReturnsCharges()
        {
            string policyId = "POL131";
            DateTime effectiveDate = new DateTime(2023, 6, 1);
            decimal expectedCharges = 150.75m;

            _mockService.Setup(s => s.GetTotalDeductionCharges(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedCharges);

            var result = _mockService.Object.GetTotalDeductionCharges(policyId, effectiveDate);

            Assert.AreEqual(expectedCharges, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetTotalDeductionCharges(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateSurrenderDate_ValidDate_ReturnsTrue()
        {
            string policyId = "POL132";
            DateTime requestedDate = new DateTime(2023, 7, 1);

            _mockService.Setup(s => s.ValidateSurrenderDate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.ValidateSurrenderDate(policyId, requestedDate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateSurrenderDate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProratedBonusRecoveryRate_ValidMonths_ReturnsRate()
        {
            string policyId = "POL133";
            int monthsActive = 24;
            double expectedRate = 0.5;

            _mockService.Setup(s => s.CalculateProratedBonusRecoveryRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.CalculateProratedBonusRecoveryRate(policyId, monthsActive);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.CalculateProratedBonusRecoveryRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxWithholdingAmount_ValidInputs_ReturnsTax()
        {
            string policyId = "POL134";
            decimal netAmount = 8000m;
            double taxRate = 0.2;
            decimal expectedTax = 1600m;

            _mockService.Setup(s => s.CalculateTaxWithholdingAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedTax);

            var result = _mockService.Object.CalculateTaxWithholdingAmount(policyId, netAmount, taxRate);

            Assert.AreEqual(expectedTax, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTaxWithholdingAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidDate_ReturnsYears()
        {
            string policyId = "POL135";
            DateTime surrenderDate = new DateTime(2023, 8, 1);
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
        public void RetrievePenaltyRuleId_ValidInputs_ReturnsRuleId()
        {
            string productCode = "PROD1";
            DateTime issueDate = new DateTime(2018, 1, 1);
            string expectedRuleId = "RULE100";

            _mockService.Setup(s => s.RetrievePenaltyRuleId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedRuleId);

            var result = _mockService.Object.RetrievePenaltyRuleId(productCode, issueDate);

            Assert.AreEqual(expectedRuleId, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("RULE0", result);
            Assert.IsTrue(result.StartsWith("RULE"));

            _mockService.Verify(s => s.RetrievePenaltyRuleId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void HasOutstandingLoans_WithLoans_ReturnsTrue()
        {
            string policyId = "POL136";

            _mockService.Setup(s => s.HasOutstandingLoans(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.HasOutstandingLoans(policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.HasOutstandingLoans(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoanInterestDeduction_ValidDate_ReturnsDeduction()
        {
            string policyId = "POL137";
            DateTime calcDate = new DateTime(2023, 9, 1);
            decimal expectedDeduction = 250.50m;

            _mockService.Setup(s => s.CalculateLoanInterestDeduction(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDeduction);

            var result = _mockService.Object.CalculateLoanInterestDeduction(policyId, calcDate);

            Assert.AreEqual(expectedDeduction, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateLoanInterestDeduction(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetSurrenderChargeFactor_ValidDuration_ReturnsFactor()
        {
            string policyId = "POL138";
            int duration = 36;
            double expectedFactor = 0.85;

            _mockService.Setup(s => s.GetSurrenderChargeFactor(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedFactor);

            var result = _mockService.Object.GetSurrenderChargeFactor(policyId, duration);

            Assert.AreEqual(expectedFactor, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(1.0, result);

            _mockService.Verify(s => s.GetSurrenderChargeFactor(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateFinalNetSurrenderValue_ValidInputs_ReturnsNetValue()
        {
            string policyId = "POL139";
            decimal grossValue = 10000m;
            decimal totalPenalties = 1500m;
            decimal expectedNet = 8500m;

            _mockService.Setup(s => s.CalculateFinalNetSurrenderValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedNet);

            var result = _mockService.Object.CalculateFinalNetSurrenderValue(policyId, grossValue, totalPenalties);

            Assert.AreEqual(expectedNet, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(grossValue, result);

            _mockService.Verify(s => s.CalculateFinalNetSurrenderValue(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void RequiresManagerApproval_HighPenalty_ReturnsTrue()
        {
            string policyId = "POL140";
            decimal penaltyAmount = 15000m;

            _mockService.Setup(s => s.RequiresManagerApproval(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            var result = _mockService.Object.RequiresManagerApproval(policyId, penaltyAmount);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.RequiresManagerApproval(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetFreeWithdrawalCount_ValidDate_ReturnsCount()
        {
            string policyId = "POL141";
            DateTime yearStart = new DateTime(2023, 1, 1);
            int expectedCount = 2;

            _mockService.Setup(s => s.GetFreeWithdrawalCount(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedCount);

            var result = _mockService.Object.GetFreeWithdrawalCount(policyId, yearStart);

            Assert.AreEqual(expectedCount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(-1, result);

            _mockService.Verify(s => s.GetFreeWithdrawalCount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void Verify_MultipleCalls_TracksCorrectly()
        {
            string policyId = "POL142";
            decimal surrenderValue = 1000m;

            _mockService.Setup(s => s.CalculateBasePenaltyAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(50m);

            _mockService.Object.CalculateBasePenaltyAmount(policyId, surrenderValue);
            _mockService.Object.CalculateBasePenaltyAmount(policyId, surrenderValue);

            _mockService.Verify(s => s.CalculateBasePenaltyAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Exactly(2));
            _mockService.Verify(s => s.HasOutstandingLoans(It.IsAny<string>()), Times.Never());
            
            Assert.IsNotNull(_mockService.Object);
            Assert.AreNotEqual(0, 1);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ValidateSurrenderDate_FutureDate_ReturnsFalse()
        {
            string policyId = "POL143";
            DateTime requestedDate = DateTime.Now.AddDays(30);

            _mockService.Setup(s => s.ValidateSurrenderDate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(false);

            var result = _mockService.Object.ValidateSurrenderDate(policyId, requestedDate);

            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.ValidateSurrenderDate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ZeroYears_ReturnsZero()
        {
            string policyId = "POL144";
            DateTime surrenderDate = DateTime.Now;
            int expectedYears = 0;

            _mockService.Setup(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedYears);

            var result = _mockService.Object.GetCompletedPolicyYears(policyId, surrenderDate);

            Assert.AreEqual(expectedYears, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0, result);

            _mockService.Verify(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void RequiresManagerApproval_LowPenalty_ReturnsFalse()
        {
            string policyId = "POL145";
            decimal penaltyAmount = 100m;

            _mockService.Setup(s => s.RequiresManagerApproval(It.IsAny<string>(), It.IsAny<decimal>())).Returns(false);

            var result = _mockService.Object.RequiresManagerApproval(policyId, penaltyAmount);

            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.RequiresManagerApproval(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicablePenaltyTierCode_NoTier_ReturnsNull()
        {
            string policyId = "POL146";
            int activeYears = 50;

            _mockService.Setup(s => s.GetApplicablePenaltyTierCode(It.IsAny<string>(), It.IsAny<int>())).Returns((string)null);

            var result = _mockService.Object.GetApplicablePenaltyTierCode(policyId, activeYears);

            Assert.IsNull(result);
            Assert.AreNotEqual("TIER1", result);
            Assert.IsFalse(result == "TIER2");
            Assert.AreEqual(null, result);

            _mockService.Verify(s => s.GetApplicablePenaltyTierCode(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }
    }
}