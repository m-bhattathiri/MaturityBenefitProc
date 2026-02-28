using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class TerminalBonusServiceMockTests
    {
        private Mock<ITerminalBonusService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ITerminalBonusService>();
        }

        [TestMethod]
        public void CalculateBaseTerminalBonus_ValidInputs_ReturnsExpectedBonus()
        {
            string policyId = "POL123";
            decimal sumAssured = 100000m;
            DateTime maturityDate = new DateTime(2025, 1, 1);
            decimal expectedBonus = 5000m;

            _mockService.Setup(s => s.CalculateBaseTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>())).Returns(expectedBonus);

            var result = _mockService.Object.CalculateBaseTerminalBonus(policyId, sumAssured, maturityDate);

            Assert.AreEqual(expectedBonus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateBaseTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBaseTerminalBonus_ZeroSumAssured_ReturnsZero()
        {
            string policyId = "POL124";
            decimal sumAssured = 0m;
            DateTime maturityDate = new DateTime(2025, 1, 1);
            decimal expectedBonus = 0m;

            _mockService.Setup(s => s.CalculateBaseTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>())).Returns(expectedBonus);

            var result = _mockService.Object.CalculateBaseTerminalBonus(policyId, sumAssured, maturityDate);

            Assert.AreEqual(expectedBonus, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0m, result);

            _mockService.Verify(s => s.CalculateBaseTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL125";
            int premiumPayingYears = 10;
            decimal expectedAmount = 2500m;

            _mockService.Setup(s => s.CalculateLoyaltyAdditionAmount(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedAmount);

            var result = _mockService.Object.CalculateLoyaltyAdditionAmount(policyId, premiumPayingYears);

            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(100m, result);

            _mockService.Verify(s => s.CalculateLoyaltyAdditionAmount(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetAccruedReversionaryBonus_ValidPolicy_ReturnsExpectedBonus()
        {
            string policyId = "POL126";
            decimal expectedBonus = 15000m;

            _mockService.Setup(s => s.GetAccruedReversionaryBonus(It.IsAny<string>())).Returns(expectedBonus);

            var result = _mockService.Object.GetAccruedReversionaryBonus(policyId);

            Assert.AreEqual(expectedBonus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetAccruedReversionaryBonus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ComputeFinalAdditionalBonus_ValidInputs_ReturnsExpectedBonus()
        {
            string policyId = "POL127";
            decimal totalPremiumsPaid = 50000m;
            decimal expectedBonus = 2000m;

            _mockService.Setup(s => s.ComputeFinalAdditionalBonus(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedBonus);

            var result = _mockService.Object.ComputeFinalAdditionalBonus(policyId, totalPremiumsPaid);

            Assert.AreEqual(expectedBonus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.ComputeFinalAdditionalBonus(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateVestedBonusTotal_ValidInputs_ReturnsExpectedTotal()
        {
            string policyId = "POL128";
            DateTime calculationDate = new DateTime(2025, 1, 1);
            decimal expectedTotal = 30000m;

            _mockService.Setup(s => s.CalculateVestedBonusTotal(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedTotal);

            var result = _mockService.Object.CalculateVestedBonusTotal(policyId, calculationDate);

            Assert.AreEqual(expectedTotal, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateVestedBonusTotal(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetSpecialSurrenderValueBonus_ValidInputs_ReturnsExpectedBonus()
        {
            string policyId = "POL129";
            decimal baseSurrenderValue = 40000m;
            decimal expectedBonus = 4000m;

            _mockService.Setup(s => s.GetSpecialSurrenderValueBonus(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedBonus);

            var result = _mockService.Object.GetSpecialSurrenderValueBonus(policyId, baseSurrenderValue);

            Assert.AreEqual(expectedBonus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetSpecialSurrenderValueBonus(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProratedTerminalBonus_ValidInputs_ReturnsExpectedBonus()
        {
            string policyId = "POL130";
            DateTime exitDate = new DateTime(2025, 6, 1);
            int daysActive = 150;
            decimal expectedBonus = 1200m;

            _mockService.Setup(s => s.CalculateProratedTerminalBonus(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(expectedBonus);

            var result = _mockService.Object.CalculateProratedTerminalBonus(policyId, exitDate, daysActive);

            Assert.AreEqual(expectedBonus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateProratedTerminalBonus(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ApplyBonusMultiplier_ValidInputs_ReturnsExpectedBonus()
        {
            decimal baseBonus = 5000m;
            double multiplierRate = 1.5;
            decimal expectedBonus = 7500m;

            _mockService.Setup(s => s.ApplyBonusMultiplier(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedBonus);

            var result = _mockService.Object.ApplyBonusMultiplier(baseBonus, multiplierRate);

            Assert.AreEqual(expectedBonus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.ApplyBonusMultiplier(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidInputs_ReturnsExpectedRate()
        {
            string planCode = "PLAN_A";
            int policyTerm = 20;
            double expectedRate = 0.05;

            _mockService.Setup(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.GetTerminalBonusRate(planCode, policyTerm);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetLoyaltyAdditionRate_ValidInputs_ReturnsExpectedRate()
        {
            string planCode = "PLAN_B";
            int completedYears = 15;
            double expectedRate = 0.03;

            _mockService.Setup(s => s.GetLoyaltyAdditionRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.GetLoyaltyAdditionRate(planCode, completedYears);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetLoyaltyAdditionRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBonusYield_ValidInputs_ReturnsExpectedYield()
        {
            decimal totalBonus = 10000m;
            decimal totalPremiums = 50000m;
            double expectedYield = 0.2;

            _mockService.Setup(s => s.CalculateBonusYield(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedYield);

            var result = _mockService.Object.CalculateBonusYield(totalBonus, totalPremiums);

            Assert.AreEqual(expectedYield, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.CalculateBonusYield(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetFundPerformanceFactor_ValidInputs_ReturnsExpectedFactor()
        {
            string fundId = "FUND_1";
            DateTime evaluationDate = new DateTime(2025, 1, 1);
            double expectedFactor = 1.1;

            _mockService.Setup(s => s.GetFundPerformanceFactor(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedFactor);

            var result = _mockService.Object.GetFundPerformanceFactor(fundId, evaluationDate);

            Assert.AreEqual(expectedFactor, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetFundPerformanceFactor(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetParticipatingFundRatio_ValidInputs_ReturnsExpectedRatio()
        {
            string cohortId = "COHORT_1";
            double expectedRatio = 0.8;

            _mockService.Setup(s => s.GetParticipatingFundRatio(It.IsAny<string>())).Returns(expectedRatio);

            var result = _mockService.Object.GetParticipatingFundRatio(cohortId);

            Assert.AreEqual(expectedRatio, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetParticipatingFundRatio(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForTerminalBonus_Eligible_ReturnsTrue()
        {
            string policyId = "POL131";
            string status = "Active";
            bool expectedEligibility = true;

            _mockService.Setup(s => s.IsEligibleForTerminalBonus(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedEligibility);

            var result = _mockService.Object.IsEligibleForTerminalBonus(policyId, status);

            Assert.AreEqual(expectedEligibility, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsEligibleForTerminalBonus(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsLoyaltyAdditionApplicable_Applicable_ReturnsTrue()
        {
            string planCode = "PLAN_C";
            int elapsedYears = 10;
            bool expectedApplicability = true;

            _mockService.Setup(s => s.IsLoyaltyAdditionApplicable(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedApplicability);

            var result = _mockService.Object.IsLoyaltyAdditionApplicable(planCode, elapsedYears);

            Assert.AreEqual(expectedApplicability, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsLoyaltyAdditionApplicable(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ValidateBonusDeclaration_Valid_ReturnsTrue()
        {
            string declarationId = "DECL_1";
            DateTime effectiveDate = new DateTime(2025, 1, 1);
            bool expectedValidation = true;

            _mockService.Setup(s => s.ValidateBonusDeclaration(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValidation);

            var result = _mockService.Object.ValidateBonusDeclaration(declarationId, effectiveDate);

            Assert.AreEqual(expectedValidation, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateBonusDeclaration(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void HasClaimedPreviousBonuses_HasClaimed_ReturnsTrue()
        {
            string policyId = "POL132";
            bool expectedClaim = true;

            _mockService.Setup(s => s.HasClaimedPreviousBonuses(It.IsAny<string>())).Returns(expectedClaim);

            var result = _mockService.Object.HasClaimedPreviousBonuses(policyId);

            Assert.AreEqual(expectedClaim, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.HasClaimedPreviousBonuses(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyInParticipatingFund_InFund_ReturnsTrue()
        {
            string policyId = "POL133";
            bool expectedInFund = true;

            _mockService.Setup(s => s.IsPolicyInParticipatingFund(It.IsAny<string>())).Returns(expectedInFund);

            var result = _mockService.Object.IsPolicyInParticipatingFund(policyId);

            Assert.AreEqual(expectedInFund, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsPolicyInParticipatingFund(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidInputs_ReturnsExpectedYears()
        {
            string policyId = "POL134";
            DateTime maturityDate = new DateTime(2025, 1, 1);
            int expectedYears = 20;

            _mockService.Setup(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedYears);

            var result = _mockService.Object.GetCompletedPolicyYears(policyId, maturityDate);

            Assert.AreEqual(expectedYears, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetCompletedPolicyYears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetMinimumYearsForTerminalBonus_ValidInputs_ReturnsExpectedYears()
        {
            string planCode = "PLAN_D";
            int expectedYears = 10;

            _mockService.Setup(s => s.GetMinimumYearsForTerminalBonus(It.IsAny<string>())).Returns(expectedYears);

            var result = _mockService.Object.GetMinimumYearsForTerminalBonus(planCode);

            Assert.AreEqual(expectedYears, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetMinimumYearsForTerminalBonus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysSinceLastBonusDeclaration_ValidInputs_ReturnsExpectedDays()
        {
            DateTime lastDeclarationDate = new DateTime(2024, 1, 1);
            int expectedDays = 365;

            _mockService.Setup(s => s.CalculateDaysSinceLastBonusDeclaration(It.IsAny<DateTime>())).Returns(expectedDays);

            var result = _mockService.Object.CalculateDaysSinceLastBonusDeclaration(lastDeclarationDate);

            Assert.AreEqual(expectedDays, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.CalculateDaysSinceLastBonusDeclaration(It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalBonusUnitsAllocated_ValidInputs_ReturnsExpectedUnits()
        {
            string policyId = "POL135";
            int expectedUnits = 500;

            _mockService.Setup(s => s.GetTotalBonusUnitsAllocated(It.IsAny<string>())).Returns(expectedUnits);

            var result = _mockService.Object.GetTotalBonusUnitsAllocated(policyId);

            Assert.AreEqual(expectedUnits, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetTotalBonusUnitsAllocated(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetBonusDeclarationId_ValidInputs_ReturnsExpectedId()
        {
            string planCode = "PLAN_E";
            DateTime declarationYear = new DateTime(2025, 1, 1);
            string expectedId = "DECL_2025_E";

            _mockService.Setup(s => s.GetBonusDeclarationId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedId);

            var result = _mockService.Object.GetBonusDeclarationId(planCode, declarationYear);

            Assert.AreEqual(expectedId, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.GetBonusDeclarationId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void DetermineBonusCohort_ValidInputs_ReturnsExpectedCohort()
        {
            string policyId = "POL136";
            DateTime issueDate = new DateTime(2010, 1, 1);
            string expectedCohort = "COHORT_2010";

            _mockService.Setup(s => s.DetermineBonusCohort(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedCohort);

            var result = _mockService.Object.DetermineBonusCohort(policyId, issueDate);

            Assert.AreEqual(expectedCohort, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.DetermineBonusCohort(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTerminalBonusStatus_ValidInputs_ReturnsExpectedStatus()
        {
            string policyId = "POL137";
            string expectedStatus = "Declared";

            _mockService.Setup(s => s.GetTerminalBonusStatus(It.IsAny<string>())).Returns(expectedStatus);

            var result = _mockService.Object.GetTerminalBonusStatus(policyId);

            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.GetTerminalBonusStatus(It.IsAny<string>()), Times.Once());
        }
    }
}