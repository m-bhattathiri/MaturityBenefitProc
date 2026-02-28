using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class ReversionaryBonusServiceMockTests
    {
        private Mock<IReversionaryBonusService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IReversionaryBonusService>();
        }

        [TestMethod]
        public void CalculateAnnualBonus_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal sumAssured = 100000m;
            double bonusRate = 0.05;
            decimal expected = 5000m;

            _mockService.Setup(s => s.CalculateAnnualBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateAnnualBonus(policyId, sumAssured, bonusRate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            
            _mockService.Verify(s => s.CalculateAnnualBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateAccruedReversionaryBonus_ValidDate_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            DateTime calcDate = new DateTime(2023, 1, 1);
            decimal expected = 15000m;

            _mockService.Setup(s => s.CalculateAccruedReversionaryBonus(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateAccruedReversionaryBonus(policyId, calcDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 15000m);
            Assert.AreNotEqual(-1m, result);

            _mockService.Verify(s => s.CalculateAccruedReversionaryBonus(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalDeclaredBonus_ValidPolicy_ReturnsTotal()
        {
            string policyId = "POL123";
            decimal expected = 25000m;

            _mockService.Setup(s => s.GetTotalDeclaredBonus(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetTotalDeclaredBonus(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 10000m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetTotalDeclaredBonus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateInterimBonus_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            DateTime exitDate = new DateTime(2023, 6, 30);
            decimal sumAssured = 100000m;
            decimal expected = 2500m;

            _mockService.Setup(s => s.CalculateInterimBonus(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateInterimBonus(policyId, exitDate, sumAssured);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 2500m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateInterimBonus(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void ComputeVestedBonusAmount_ValidYears_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            int activeYears = 5;
            decimal expected = 10000m;

            _mockService.Setup(s => s.ComputeVestedBonusAmount(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.ComputeVestedBonusAmount(policyId, activeYears);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(1m, result);

            _mockService.Verify(s => s.ComputeVestedBonusAmount(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal accruedBonus = 50000m;
            double terminalBonusRate = 0.1;
            decimal expected = 5000m;

            _mockService.Setup(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateTerminalBonus(policyId, accruedBonus, terminalBonusRate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 5000m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetSurrenderValueOfBonus_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal totalBonus = 20000m;
            double surrenderFactor = 0.5;
            decimal expected = 10000m;

            _mockService.Setup(s => s.GetSurrenderValueOfBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.GetSurrenderValueOfBonus(policyId, totalBonus, surrenderFactor);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 10000m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetSurrenderValueOfBonus(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal baseAmount = 100000m;
            double loyaltyRate = 0.02;
            decimal expected = 2000m;

            _mockService.Setup(s => s.CalculateLoyaltyAdditionAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateLoyaltyAdditionAmount(policyId, baseAmount, loyaltyRate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 2000m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateLoyaltyAdditionAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetCurrentBonusRate_ValidPlan_ReturnsRate()
        {
            string planCode = "PLAN1";
            int policyTerm = 10;
            double expected = 0.05;

            _mockService.Setup(s => s.GetCurrentBonusRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetCurrentBonusRate(planCode, policyTerm);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetCurrentBonusRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetInterimBonusRate_ValidPlan_ReturnsRate()
        {
            string planCode = "PLAN1";
            DateTime exitDate = new DateTime(2023, 6, 30);
            double expected = 0.04;

            _mockService.Setup(s => s.GetInterimBonusRate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetInterimBonusRate(planCode, exitDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.04);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetInterimBonusRate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBonusCompoundingFactor_ValidInputs_ReturnsFactor()
        {
            int yearsInForce = 5;
            double annualRate = 0.05;
            double expected = 1.276;

            _mockService.Setup(s => s.CalculateBonusCompoundingFactor(It.IsAny<int>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateBonusCompoundingFactor(yearsInForce, annualRate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 1.0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.CalculateBonusCompoundingFactor(It.IsAny<int>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidPlan_ReturnsRate()
        {
            string planCode = "PLAN1";
            int durationInYears = 15;
            double expected = 0.15;

            _mockService.Setup(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetTerminalBonusRate(planCode, durationInYears);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.15);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetTerminalBonusRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void FetchLoyaltyAdditionPercentage_ValidPolicy_ReturnsPercentage()
        {
            string policyId = "POL123";
            int premiumPayingTerm = 10;
            double expected = 0.02;

            _mockService.Setup(s => s.FetchLoyaltyAdditionPercentage(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.FetchLoyaltyAdditionPercentage(policyId, premiumPayingTerm);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.02);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.FetchLoyaltyAdditionPercentage(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForBonus_EligiblePolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime evalDate = new DateTime(2023, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.IsPolicyEligibleForBonus(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsPolicyEligibleForBonus(policyId, evalDate);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsPolicyEligibleForBonus(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void HasGuaranteedAdditions_PlanHasAdditions_ReturnsTrue()
        {
            string planCode = "PLAN1";
            bool expected = true;

            _mockService.Setup(s => s.HasGuaranteedAdditions(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.HasGuaranteedAdditions(planCode);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.HasGuaranteedAdditions(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsParticipatingPolicy_ParticipatingPolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            bool expected = true;

            _mockService.Setup(s => s.IsParticipatingPolicy(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.IsParticipatingPolicy(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsParticipatingPolicy(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateBonusRateApplicability_ValidRate_ReturnsTrue()
        {
            string planCode = "PLAN1";
            double rate = 0.05;
            DateTime effectiveDate = new DateTime(2023, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.ValidateBonusRateApplicability(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.ValidateBonusRateApplicability(planCode, rate, effectiveDate);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateBonusRateApplicability(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CheckLoyaltyAdditionEligibility_Eligible_ReturnsTrue()
        {
            string policyId = "POL123";
            int paidPremiumsCount = 10;
            bool expected = true;

            _mockService.Setup(s => s.CheckLoyaltyAdditionEligibility(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.CheckLoyaltyAdditionEligibility(policyId, paidPremiumsCount);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.CheckLoyaltyAdditionEligibility(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void IsBonusVested_VestedBonus_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime checkDate = new DateTime(2023, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.IsBonusVested(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsBonusVested(policyId, checkDate);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsBonusVested(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetYearsEligibleForBonus_ValidPolicy_ReturnsYears()
        {
            string policyId = "POL123";
            int expected = 5;

            _mockService.Setup(s => s.GetYearsEligibleForBonus(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetYearsEligibleForBonus(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 5);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetYearsEligibleForBonus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysSinceLastDeclaration_ValidPolicy_ReturnsDays()
        {
            string policyId = "POL123";
            DateTime currentDate = new DateTime(2023, 1, 1);
            int expected = 100;

            _mockService.Setup(s => s.CalculateDaysSinceLastDeclaration(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateDaysSinceLastDeclaration(policyId, currentDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 100);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.CalculateDaysSinceLastDeclaration(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPendingBonusDeclarationsCount_ValidPolicy_ReturnsCount()
        {
            string policyId = "POL123";
            int expected = 2;

            _mockService.Setup(s => s.GetPendingBonusDeclarationsCount(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetPendingBonusDeclarationsCount(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 2);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetPendingBonusDeclarationsCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetMinimumTermForTerminalBonus_ValidPlan_ReturnsTerm()
        {
            string planCode = "PLAN1";
            int expected = 10;

            _mockService.Setup(s => s.GetMinimumTermForTerminalBonus(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetMinimumTermForTerminalBonus(planCode);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 10);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetMinimumTermForTerminalBonus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetBonusRateTableId_ValidInputs_ReturnsTableId()
        {
            string planCode = "PLAN1";
            DateTime issueDate = new DateTime(2020, 1, 1);
            string expected = "TABLE_A";

            _mockService.Setup(s => s.GetBonusRateTableId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetBonusRateTableId(planCode, issueDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("TABLE_B", result);

            _mockService.Verify(s => s.GetBonusRateTableId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void DetermineBonusStatus_ValidPolicy_ReturnsStatus()
        {
            string policyId = "POL123";
            string expected = "ACTIVE";

            _mockService.Setup(s => s.DetermineBonusStatus(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.DetermineBonusStatus(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == "ACTIVE");
            Assert.AreNotEqual("INACTIVE", result);

            _mockService.Verify(s => s.DetermineBonusStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetLoyaltyAdditionScaleCode_ValidInputs_ReturnsCode()
        {
            string planCode = "PLAN1";
            int policyTerm = 15;
            string expected = "SCALE_1";

            _mockService.Setup(s => s.GetLoyaltyAdditionScaleCode(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetLoyaltyAdditionScaleCode(planCode, policyTerm);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == "SCALE_1");
            Assert.AreNotEqual("SCALE_2", result);

            _mockService.Verify(s => s.GetLoyaltyAdditionScaleCode(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetLastDeclarationFinancialYear_ValidPolicy_ReturnsYear()
        {
            string policyId = "POL123";
            string expected = "2022-2023";

            _mockService.Setup(s => s.GetLastDeclarationFinancialYear(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetLastDeclarationFinancialYear(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("2022"));
            Assert.AreNotEqual("2021-2022", result);

            _mockService.Verify(s => s.GetLastDeclarationFinancialYear(It.IsAny<string>()), Times.Once());
        }
    }
}