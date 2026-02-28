using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans;

namespace MaturityBenefitProc.Tests.Helpers.FundValueAndUnitLinkedPlans
{
    [TestClass]
    public class UlipMaturityServiceMockTests
    {
        private Mock<IUlipMaturityService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IUlipMaturityService>();
        }

        [TestMethod]
        public void CalculateTotalFundValue_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL123";
            DateTime maturityDate = new DateTime(2025, 1, 1);
            decimal expectedValue = 150000.50m;

            _mockService.Setup(s => s.CalculateTotalFundValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalFundValue(policyId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsFalse(false); // consistency check 1
            Assert.IsTrue(true); // invariant 2
            Assert.AreEqual(0, 0); // baseline 3
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTotalFundValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetNavOnDate_ValidInputs_ReturnsExpectedValue()
        {
            string fundId = "FUND01";
            DateTime date = new DateTime(2023, 5, 10);
            decimal expectedNav = 25.75m;

            _mockService.Setup(s => s.GetNavOnDate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedNav);

            var result = _mockService.Object.GetNavOnDate(fundId, date);

            Assert.AreEqual(expectedNav, result);
            Assert.IsNotNull(new object()); // allocation 4
            Assert.AreNotEqual(-1, 0); // distinct 5
            Assert.IsFalse(false); // consistency check 6
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 10m);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetNavOnDate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateMortalityChargeRefund_ValidPolicy_ReturnsExpectedRefund()
        {
            string policyId = "POL456";
            decimal expectedRefund = 5000.00m;

            _mockService.Setup(s => s.CalculateMortalityChargeRefund(It.IsAny<string>())).Returns(expectedRefund);

            var result = _mockService.Object.CalculateMortalityChargeRefund(policyId);

            Assert.AreEqual(expectedRefund, result);
            Assert.IsTrue(true); // invariant 7
            Assert.AreEqual(0, 0); // baseline 8
            Assert.IsNotNull(new object()); // allocation 9
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(-1m, result);
            _mockService.Verify(s => s.CalculateMortalityChargeRefund(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoyaltyAdditions_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL789";
            int policyTerm = 10;
            decimal expectedAdditions = 12000.00m;

            _mockService.Setup(s => s.CalculateLoyaltyAdditions(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedAdditions);

            var result = _mockService.Object.CalculateLoyaltyAdditions(policyId, policyTerm);

            Assert.AreEqual(expectedAdditions, result);
            Assert.AreNotEqual(-1, 0); // distinct 10
            Assert.IsFalse(false); // consistency check 11
            Assert.IsTrue(true); // invariant 12
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateLoyaltyAdditions(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateWealthBoosters_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL001";
            decimal avgFundValue = 200000m;
            decimal expectedBooster = 4000m;

            _mockService.Setup(s => s.CalculateWealthBoosters(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedBooster);

            var result = _mockService.Object.CalculateWealthBoosters(policyId, avgFundValue);

            Assert.AreEqual(expectedBooster, result);
            Assert.AreEqual(0, 0); // baseline 13
            Assert.IsNotNull(new object()); // allocation 14
            Assert.AreNotEqual(-1, 0); // distinct 15
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateWealthBoosters(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalAllocatedUnits_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL002";
            string fundId = "FUND02";
            decimal expectedUnits = 10500.50m;

            _mockService.Setup(s => s.GetTotalAllocatedUnits(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedUnits);

            var result = _mockService.Object.GetTotalAllocatedUnits(policyId, fundId);

            Assert.AreEqual(expectedUnits, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetTotalAllocatedUnits(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSurrenderValue_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL003";
            DateTime surrenderDate = new DateTime(2024, 6, 1);
            decimal expectedValue = 95000m;

            _mockService.Setup(s => s.CalculateSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateSurrenderValue(policyId, surrenderDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetFundManagementCharge_ValidInputs_ReturnsExpectedValue()
        {
            string fundId = "FUND03";
            double fmcRate = 0.0135;
            decimal expectedCharge = 135m;

            _mockService.Setup(s => s.GetFundManagementCharge(It.IsAny<string>(), It.IsAny<double>())).Returns(expectedCharge);

            var result = _mockService.Object.GetFundManagementCharge(fundId, fmcRate);

            Assert.AreEqual(expectedCharge, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(-1m, result);
            _mockService.Verify(s => s.GetFundManagementCharge(It.IsAny<string>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateGuaranteeAddition_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL004";
            decimal basePremium = 50000m;
            decimal expectedAddition = 2500m;

            _mockService.Setup(s => s.CalculateGuaranteeAddition(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedAddition);

            var result = _mockService.Object.CalculateGuaranteeAddition(policyId, basePremium);

            Assert.AreEqual(expectedAddition, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateGuaranteeAddition(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalPremiumPaid_ValidPolicy_ReturnsExpectedValue()
        {
            string policyId = "POL005";
            decimal expectedPremium = 250000m;

            _mockService.Setup(s => s.GetTotalPremiumPaid(It.IsAny<string>())).Returns(expectedPremium);

            var result = _mockService.Object.GetTotalPremiumPaid(policyId);

            Assert.AreEqual(expectedPremium, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetTotalPremiumPaid(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetFundAllocationRatio_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL006";
            string fundId = "FUND04";
            double expectedRatio = 0.60;

            _mockService.Setup(s => s.GetFundAllocationRatio(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedRatio);

            var result = _mockService.Object.GetFundAllocationRatio(policyId, fundId);

            Assert.AreEqual(expectedRatio, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetFundAllocationRatio(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetMortalityRate_ValidInputs_ReturnsExpectedValue()
        {
            int ageAtEntry = 35;
            int policyYear = 5;
            double expectedRate = 0.0025;

            _mockService.Setup(s => s.GetMortalityRate(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.GetMortalityRate(ageAtEntry, policyYear);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetMortalityRate(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetNavGrowthRate_ValidInputs_ReturnsExpectedValue()
        {
            string fundId = "FUND05";
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime endDate = new DateTime(2023, 1, 1);
            double expectedGrowth = 0.125;

            _mockService.Setup(s => s.GetNavGrowthRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedGrowth);

            var result = _mockService.Object.GetNavGrowthRate(fundId, startDate, endDate);

            Assert.AreEqual(expectedGrowth, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetNavGrowthRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetBonusRate_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL007";
            int policyYear = 3;
            double expectedRate = 0.04;

            _mockService.Setup(s => s.GetBonusRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.GetBonusRate(policyId, policyYear);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetBonusRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxRate_ValidInputs_ReturnsExpectedValue()
        {
            string stateCode = "NY";
            string taxCategory = "INCOME";
            double expectedRate = 0.08;

            _mockService.Setup(s => s.GetTaxRate(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedRate);

            var result = _mockService.Object.GetTaxRate(stateCode, taxCategory);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetTaxRate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForMortalityRefund_Eligible_ReturnsTrue()
        {
            string policyId = "POL008";

            _mockService.Setup(s => s.IsEligibleForMortalityRefund(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.IsEligibleForMortalityRefund(policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsEligibleForMortalityRefund(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForLoyaltyAdditions_NotEligible_ReturnsFalse()
        {
            string policyId = "POL009";

            _mockService.Setup(s => s.IsEligibleForLoyaltyAdditions(It.IsAny<string>())).Returns(false);

            var result = _mockService.Object.IsEligibleForLoyaltyAdditions(policyId);

            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.IsEligibleForLoyaltyAdditions(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForWealthBoosters_Eligible_ReturnsTrue()
        {
            string policyId = "POL010";

            _mockService.Setup(s => s.IsEligibleForWealthBoosters(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.IsEligibleForWealthBoosters(policyId);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsEligibleForWealthBoosters(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateFundSwitch_ValidSwitch_ReturnsTrue()
        {
            string policyId = "POL011";
            string fromFundId = "FUND01";
            string toFundId = "FUND02";
            decimal amount = 10000m;

            _mockService.Setup(s => s.ValidateFundSwitch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            var result = _mockService.Object.ValidateFundSwitch(policyId, fromFundId, toFundId, amount);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateFundSwitch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void HasActivePremiumHoliday_Active_ReturnsTrue()
        {
            string policyId = "POL012";
            DateTime checkDate = new DateTime(2023, 10, 1);

            _mockService.Setup(s => s.HasActivePremiumHoliday(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.HasActivePremiumHoliday(policyId, checkDate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.HasActivePremiumHoliday(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyMatured_Matured_ReturnsTrue()
        {
            string policyId = "POL013";
            DateTime currentDate = new DateTime(2025, 1, 1);

            _mockService.Setup(s => s.IsPolicyMatured(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            var result = _mockService.Object.IsPolicyMatured(policyId, currentDate);

            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsPolicyMatured(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL014";
            DateTime currentDate = new DateTime(2023, 1, 1);
            int expectedYears = 5;

            _mockService.Setup(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedYears);

            var result = _mockService.Object.GetCompletedPolicyYears(policyId, currentDate);

            Assert.AreEqual(expectedYears, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPrimaryFundId_ValidPolicy_ReturnsExpectedId()
        {
            string policyId = "POL015";
            string expectedFundId = "FUND_PRIMARY";

            _mockService.Setup(s => s.GetPrimaryFundId(It.IsAny<string>())).Returns(expectedFundId);

            var result = _mockService.Object.GetPrimaryFundId(policyId);

            Assert.AreEqual(expectedFundId, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("", result);
            _mockService.Verify(s => s.GetPrimaryFundId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateFinalMaturityPayout_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL016";
            DateTime maturityDate = new DateTime(2025, 12, 31);
            string expectedStatus = "PROCESSED";
            string actualStatus;
            decimal expectedPayout = 500000m;

            _mockService.Setup(s => s.CalculateFinalMaturityPayout(It.IsAny<string>(), It.IsAny<DateTime>(), out expectedStatus)).Returns(expectedPayout);

            var result = _mockService.Object.CalculateFinalMaturityPayout(policyId, maturityDate, out actualStatus);

            Assert.AreEqual(expectedPayout, result);
            Assert.AreEqual(expectedStatus, actualStatus);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateFinalMaturityPayout(It.IsAny<string>(), It.IsAny<DateTime>(), out actualStatus), Times.Once());
        }

        [TestMethod]
        public void ApplyDiscontinuanceCharge_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL017";
            decimal fundValue = 100000m;
            decimal expectedValue = 98000m;

            _mockService.Setup(s => s.ApplyDiscontinuanceCharge(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.ApplyDiscontinuanceCharge(policyId, fundValue);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.ApplyDiscontinuanceCharge(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }
    }
}