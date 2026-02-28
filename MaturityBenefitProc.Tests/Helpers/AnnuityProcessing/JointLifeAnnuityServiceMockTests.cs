using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.AnnuityProcessing;

namespace MaturityBenefitProc.Tests.Helpers.AnnuityProcessing
{
    [TestClass]
    public class JointLifeAnnuityServiceMockTests
    {
        private Mock<IJointLifeAnnuityService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IJointLifeAnnuityService>();
        }

        [TestMethod]
        public void CalculateSurvivorBenefitAmount_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal baseAmount = 1000m;
            double percentage = 0.5;
            decimal expected = 500m;

            _mockService.Setup(s => s.CalculateSurvivorBenefitAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateSurvivorBenefitAmount(policyId, baseAmount, percentage);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CalculateSurvivorBenefitAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSecondaryAnnuitantPayout_ValidDate_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            DateTime deathDate = new DateTime(2023, 1, 1);
            decimal expected = 750m;

            _mockService.Setup(s => s.CalculateSecondaryAnnuitantPayout(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateSecondaryAnnuitantPayout(policyId, deathDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CalculateSecondaryAnnuitantPayout(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalPaidToPrimaryAnnuitant_ValidPolicy_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal expected = 50000m;

            _mockService.Setup(s => s.GetTotalPaidToPrimaryAnnuitant(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetTotalPaidToPrimaryAnnuitant(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetTotalPaidToPrimaryAnnuitant(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProRataPayment_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            DateTime deathDate = new DateTime(2023, 1, 15);
            decimal monthlyBenefit = 1000m;
            decimal expected = 500m;

            _mockService.Setup(s => s.CalculateProRataPayment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateProRataPayment(policyId, deathDate, monthlyBenefit);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CalculateProRataPayment(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void ComputeLumpSumDeathBenefit_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal accountValue = 100000m;
            decimal totalPaid = 40000m;
            decimal expected = 60000m;

            _mockService.Setup(s => s.ComputeLumpSumDeathBenefit(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.ComputeLumpSumDeathBenefit(policyId, accountValue, totalPaid);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.ComputeLumpSumDeathBenefit(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxablePortion_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal payoutAmount = 1000m;
            decimal expected = 200m;

            _mockService.Setup(s => s.CalculateTaxablePortion(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateTaxablePortion(policyId, payoutAmount);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CalculateTaxablePortion(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetGuaranteedMinimumDeathBenefit_ValidPolicy_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal expected = 100000m;

            _mockService.Setup(s => s.GetGuaranteedMinimumDeathBenefit(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetGuaranteedMinimumDeathBenefit(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetGuaranteedMinimumDeathBenefit(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetSurvivorReductionRate_ValidPolicy_ReturnsExpectedRate()
        {
            string policyId = "POL123";
            double expected = 0.5;

            _mockService.Setup(s => s.GetSurvivorReductionRate(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetSurvivorReductionRate(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetSurvivorReductionRate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateActuarialAdjustmentFactor_ValidAges_ReturnsExpectedFactor()
        {
            int primaryAge = 65;
            int secondaryAge = 60;
            double expected = 0.85;

            _mockService.Setup(s => s.CalculateActuarialAdjustmentFactor(It.IsAny<int>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.CalculateActuarialAdjustmentFactor(primaryAge, secondaryAge);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CalculateActuarialAdjustmentFactor(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetCostOfLivingAdjustmentRate_ValidInputs_ReturnsExpectedRate()
        {
            string policyId = "POL123";
            DateTime adjustmentDate = new DateTime(2023, 1, 1);
            double expected = 0.03;

            _mockService.Setup(s => s.GetCostOfLivingAdjustmentRate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetCostOfLivingAdjustmentRate(policyId, adjustmentDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetCostOfLivingAdjustmentRate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ComputeJointLifeExpectancyFactor_ValidInputs_ReturnsExpectedFactor()
        {
            int primaryAge = 65;
            int secondaryAge = 60;
            string tableId = "TBL1";
            double expected = 25.5;

            _mockService.Setup(s => s.ComputeJointLifeExpectancyFactor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.ComputeJointLifeExpectancyFactor(primaryAge, secondaryAge, tableId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.ComputeJointLifeExpectancyFactor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsSecondaryAnnuitantEligible_ValidInputs_ReturnsTrue()
        {
            string policyId = "POL123";
            string annuitantId = "ANN456";
            bool expected = true;

            _mockService.Setup(s => s.IsSecondaryAnnuitantEligible(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.IsSecondaryAnnuitantEligible(policyId, annuitantId);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsSecondaryAnnuitantEligible(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateSpousalContinuation_ValidInputs_ReturnsTrue()
        {
            string policyId = "POL123";
            DateTime deathDate = new DateTime(2023, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.ValidateSpousalContinuation(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.ValidateSpousalContinuation(policyId, deathDate);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateSpousalContinuation(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void HasGuaranteedPeriodExpired_ValidInputs_ReturnsFalse()
        {
            string policyId = "POL123";
            DateTime evalDate = new DateTime(2023, 1, 1);
            bool expected = false;

            _mockService.Setup(s => s.HasGuaranteedPeriodExpired(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.HasGuaranteedPeriodExpired(policyId, evalDate);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.HasGuaranteedPeriodExpired(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsJointLifePolicyActive_ValidPolicy_ReturnsTrue()
        {
            string policyId = "POL123";
            bool expected = true;

            _mockService.Setup(s => s.IsJointLifePolicyActive(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.IsJointLifePolicyActive(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsJointLifePolicyActive(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RequiresMedallionSignatureGuarantee_HighAmount_ReturnsTrue()
        {
            decimal payoutAmount = 150000m;
            bool expected = true;

            _mockService.Setup(s => s.RequiresMedallionSignatureGuarantee(It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.RequiresMedallionSignatureGuarantee(payoutAmount);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.RequiresMedallionSignatureGuarantee(It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CheckBeneficiaryOverrideExists_ValidInputs_ReturnsFalse()
        {
            string policyId = "POL123";
            string annuitantId = "ANN456";
            bool expected = false;

            _mockService.Setup(s => s.CheckBeneficiaryOverrideExists(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CheckBeneficiaryOverrideExists(policyId, annuitantId);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.CheckBeneficiaryOverrideExists(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingGuaranteedPayments_ValidInputs_ReturnsExpectedCount()
        {
            string policyId = "POL123";
            DateTime deathDate = new DateTime(2023, 1, 1);
            int expected = 60;

            _mockService.Setup(s => s.GetRemainingGuaranteedPayments(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetRemainingGuaranteedPayments(policyId, deathDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetRemainingGuaranteedPayments(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysBetweenPayments_ValidDates_ReturnsExpectedDays()
        {
            DateTime lastDate = new DateTime(2023, 1, 1);
            DateTime nextDate = new DateTime(2023, 2, 1);
            int expected = 31;

            _mockService.Setup(s => s.CalculateDaysBetweenPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateDaysBetweenPayments(lastDate, nextDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CalculateDaysBetweenPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetSecondaryAnnuitantAge_ValidInputs_ReturnsExpectedAge()
        {
            string annuitantId = "ANN456";
            DateTime evalDate = new DateTime(2023, 1, 1);
            int expected = 60;

            _mockService.Setup(s => s.GetSecondaryAnnuitantAge(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetSecondaryAnnuitantAge(annuitantId, evalDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetSecondaryAnnuitantAge(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CountProcessedSurvivorClaims_ValidPolicy_ReturnsExpectedCount()
        {
            string policyId = "POL123";
            int expected = 2;

            _mockService.Setup(s => s.CountProcessedSurvivorClaims(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CountProcessedSurvivorClaims(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CountProcessedSurvivorClaims(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetSecondaryAnnuitantId_ValidPolicy_ReturnsExpectedId()
        {
            string policyId = "POL123";
            string expected = "ANN456";

            _mockService.Setup(s => s.GetSecondaryAnnuitantId(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetSecondaryAnnuitantId(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.GetSecondaryAnnuitantId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void DeterminePayoutTransitionStatus_ValidPolicy_ReturnsExpectedStatus()
        {
            string policyId = "POL123";
            string expected = "Pending";

            _mockService.Setup(s => s.DeterminePayoutTransitionStatus(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.DeterminePayoutTransitionStatus(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.DeterminePayoutTransitionStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateSurvivorClaimReference_ValidInputs_ReturnsExpectedRef()
        {
            string policyId = "POL123";
            string annuitantId = "ANN456";
            string expected = "REF-POL123-ANN456";

            _mockService.Setup(s => s.GenerateSurvivorClaimReference(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GenerateSurvivorClaimReference(policyId, annuitantId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.GenerateSurvivorClaimReference(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableMortalityTableCode_ValidInputs_ReturnsExpectedCode()
        {
            string policyId = "POL123";
            DateTime issueDate = new DateTime(2010, 1, 1);
            string expected = "2000GAM";

            _mockService.Setup(s => s.GetApplicableMortalityTableCode(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetApplicableMortalityTableCode(policyId, issueDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.GetApplicableMortalityTableCode(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }
    }
}