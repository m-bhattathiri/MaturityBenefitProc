using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.ReinsuranceAndPoolManagement;

namespace MaturityBenefitProc.Tests.Helpers.ReinsuranceAndPoolManagement
{
    [TestClass]
    public class PoolAllocationServiceMockTests
    {
        private Mock<IPoolAllocationService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IPoolAllocationService>();
        }

        [TestMethod]
        public void CalculateTotalPoolLiability_ValidInputs_ReturnsExpectedAmount()
        {
            string poolId = "POOL-100";
            DateTime maturityDate = new DateTime(2023, 12, 31);
            decimal expectedValue = 1500000.50m;

            _mockService.Setup(s => s.CalculateTotalPoolLiability(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalPoolLiability(poolId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTotalPoolLiability(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
            _mockService.Verify(s => s.CalculateTotalPoolLiability(poolId, maturityDate), Times.AtLeastOnce());
        }

        [TestMethod]
        public void AllocateCedentShare_ValidInputs_ReturnsExpectedShare()
        {
            string policyId = "POL-999";
            decimal totalValue = 50000m;
            decimal expectedShare = 10000m;

            _mockService.Setup(s => s.AllocateCedentShare(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedShare);

            var result = _mockService.Object.AllocateCedentShare(policyId, totalValue);

            Assert.AreEqual(expectedShare, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result <= totalValue);
            Assert.AreNotEqual(totalValue, result);

            _mockService.Verify(s => s.AllocateCedentShare(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateReinsurerQuotaShare_ValidInputs_ReturnsExpectedValue()
        {
            string reinsurerId = "RE-001";
            decimal grossLiability = 100000m;
            double quotaShare = 0.25;
            decimal expectedValue = 25000m;

            _mockService.Setup(s => s.CalculateReinsurerQuotaShare(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateReinsurerQuotaShare(reinsurerId, grossLiability, quotaShare);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateReinsurerQuotaShare(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalAllocatedAmount_ValidPolicy_ReturnsAmount()
        {
            string policyId = "POL-123";
            decimal expectedAmount = 45000m;

            _mockService.Setup(s => s.GetTotalAllocatedAmount(It.IsAny<string>())).Returns(expectedAmount);

            var result = _mockService.Object.GetTotalAllocatedAmount(policyId);

            Assert.AreEqual(expectedAmount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(-1m, result);

            _mockService.Verify(s => s.GetTotalAllocatedAmount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ComputeSurplusTreatyAllocation_ValidInputs_ReturnsExpectedValue()
        {
            string treatyId = "TR-500";
            decimal maturityAmount = 150000m;
            decimal retentionLimit = 50000m;
            decimal expectedValue = 100000m;

            _mockService.Setup(s => s.ComputeSurplusTreatyAllocation(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.ComputeSurplusTreatyAllocation(treatyId, maturityAmount, retentionLimit);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.ComputeSurplusTreatyAllocation(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculatePoolAdministrativeFee_ValidInputs_ReturnsFee()
        {
            string poolId = "POOL-A";
            decimal totalAllocated = 1000000m;
            decimal expectedFee = 5000m;

            _mockService.Setup(s => s.CalculatePoolAdministrativeFee(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedFee);

            var result = _mockService.Object.CalculatePoolAdministrativeFee(poolId, totalAllocated);

            Assert.AreEqual(expectedFee, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < totalAllocated);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculatePoolAdministrativeFee(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetReinsurerParticipationRate_ValidInputs_ReturnsRate()
        {
            string reinsurerId = "RE-002";
            string poolId = "POOL-B";
            double expectedRate = 0.15;

            _mockService.Setup(s => s.GetReinsurerParticipationRate(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedRate);

            var result = _mockService.Object.GetReinsurerParticipationRate(reinsurerId, poolId);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0.0 && result <= 1.0);
            Assert.AreNotEqual(1.5, result);

            _mockService.Verify(s => s.GetReinsurerParticipationRate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateEffectiveTaxRate_ValidInputs_ReturnsRate()
        {
            string poolId = "POOL-C";
            string jurisdictionCode = "US-NY";
            double expectedRate = 0.085;

            _mockService.Setup(s => s.CalculateEffectiveTaxRate(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedRate);

            var result = _mockService.Object.CalculateEffectiveTaxRate(poolId, jurisdictionCode);

            Assert.AreEqual(expectedRate, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0.0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.CalculateEffectiveTaxRate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPoolUtilizationRatio_ValidInputs_ReturnsRatio()
        {
            string poolId = "POOL-D";
            DateTime start = new DateTime(2023, 1, 1);
            DateTime end = new DateTime(2023, 12, 31);
            double expectedRatio = 0.75;

            _mockService.Setup(s => s.GetPoolUtilizationRatio(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedRatio);

            var result = _mockService.Object.GetPoolUtilizationRatio(poolId, start, end);

            Assert.AreEqual(expectedRatio, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0.0);
            Assert.AreNotEqual(-1.0, result);

            _mockService.Verify(s => s.GetPoolUtilizationRatio(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateRiskAdjustmentFactor_ValidInputs_ReturnsFactor()
        {
            string policyId = "POL-456";
            int riskScore = 80;
            double expectedFactor = 1.2;

            _mockService.Setup(s => s.CalculateRiskAdjustmentFactor(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedFactor);

            var result = _mockService.Object.CalculateRiskAdjustmentFactor(policyId, riskScore);

            Assert.AreEqual(expectedFactor, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0.0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.CalculateRiskAdjustmentFactor(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForPool_EligiblePolicy_ReturnsTrue()
        {
            string policyId = "POL-789";
            string poolId = "POOL-E";
            bool expected = true;

            _mockService.Setup(s => s.IsPolicyEligibleForPool(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.IsPolicyEligibleForPool(policyId, poolId);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsPolicyEligibleForPool(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateAllocationTotals_ValidTotals_ReturnsTrue()
        {
            string policyId = "POL-111";
            decimal totalAmount = 100000m;
            bool expected = true;

            _mockService.Setup(s => s.ValidateAllocationTotals(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.ValidateAllocationTotals(policyId, totalAmount);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateAllocationTotals(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CheckReinsurerCapacity_SufficientCapacity_ReturnsTrue()
        {
            string reinsurerId = "RE-003";
            decimal requested = 500000m;
            bool expected = true;

            _mockService.Setup(s => s.CheckReinsurerCapacity(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CheckReinsurerCapacity(reinsurerId, requested);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.CheckReinsurerCapacity(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsTreatyActive_ActiveTreaty_ReturnsTrue()
        {
            string treatyId = "TR-100";
            DateTime date = new DateTime(2023, 6, 15);
            bool expected = true;

            _mockService.Setup(s => s.IsTreatyActive(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsTreatyActive(treatyId, date);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsTreatyActive(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void VerifyPoolSolvency_SolventPool_ReturnsTrue()
        {
            string poolId = "POOL-F";
            bool expected = true;

            _mockService.Setup(s => s.VerifyPoolSolvency(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.VerifyPoolSolvency(poolId);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.VerifyPoolSolvency(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RequiresManualUnderwriterReview_HighAmount_ReturnsTrue()
        {
            string policyId = "POL-222";
            decimal amount = 5000000m;
            bool expected = true;

            _mockService.Setup(s => s.RequiresManualUnderwriterReview(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.RequiresManualUnderwriterReview(policyId, amount);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.RequiresManualUnderwriterReview(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetActiveCoInsurersCount_ValidPool_ReturnsCount()
        {
            string poolId = "POOL-G";
            int expectedCount = 5;

            _mockService.Setup(s => s.GetActiveCoInsurersCount(It.IsAny<string>())).Returns(expectedCount);

            var result = _mockService.Object.GetActiveCoInsurersCount(poolId);

            Assert.AreEqual(expectedCount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetActiveCoInsurersCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysToMaturity_ValidInputs_ReturnsDays()
        {
            string policyId = "POL-333";
            DateTime current = new DateTime(2023, 1, 1);
            int expectedDays = 365;

            _mockService.Setup(s => s.CalculateDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDays);

            var result = _mockService.Object.CalculateDaysToMaturity(policyId, current);

            Assert.AreEqual(expectedDays, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.CalculateDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetAllocationRevisionCount_ValidPolicy_ReturnsCount()
        {
            string policyId = "POL-444";
            int expectedCount = 2;

            _mockService.Setup(s => s.GetAllocationRevisionCount(It.IsAny<string>())).Returns(expectedCount);

            var result = _mockService.Object.GetAllocationRevisionCount(policyId);

            Assert.AreEqual(expectedCount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(-1, result);

            _mockService.Verify(s => s.GetAllocationRevisionCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTreatyDurationInMonths_ValidTreaty_ReturnsMonths()
        {
            string treatyId = "TR-200";
            int expectedMonths = 12;

            _mockService.Setup(s => s.GetTreatyDurationInMonths(It.IsAny<string>())).Returns(expectedMonths);

            var result = _mockService.Object.GetTreatyDurationInMonths(treatyId);

            Assert.AreEqual(expectedMonths, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetTreatyDurationInMonths(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CountEligiblePoliciesInPool_ValidInputs_ReturnsCount()
        {
            string poolId = "POOL-H";
            DateTime month = new DateTime(2023, 10, 1);
            int expectedCount = 150;

            _mockService.Setup(s => s.CountEligiblePoliciesInPool(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedCount);

            var result = _mockService.Object.CountEligiblePoliciesInPool(poolId, month);

            Assert.AreEqual(expectedCount, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(-1, result);

            _mockService.Verify(s => s.CountEligiblePoliciesInPool(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void DeterminePrimaryPool_ValidInputs_ReturnsPoolId()
        {
            string policyId = "POL-555";
            decimal value = 250000m;
            string expectedPool = "POOL-PRIMARY";

            _mockService.Setup(s => s.DeterminePrimaryPool(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedPool);

            var result = _mockService.Object.DeterminePrimaryPool(policyId, value);

            Assert.AreEqual(expectedPool, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.StartsWith("POOL"));

            _mockService.Verify(s => s.DeterminePrimaryPool(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GenerateAllocationReferenceNumber_ValidInputs_ReturnsRef()
        {
            string policyId = "POL-666";
            string poolId = "POOL-I";
            string expectedRef = "REF-12345";

            _mockService.Setup(s => s.GenerateAllocationReferenceNumber(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedRef);

            var result = _mockService.Object.GenerateAllocationReferenceNumber(policyId, poolId);

            Assert.AreEqual(expectedRef, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.StartsWith("REF"));

            _mockService.Verify(s => s.GenerateAllocationReferenceNumber(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetReinsurerStatus_ValidId_ReturnsStatus()
        {
            string reinsurerId = "RE-004";
            string expectedStatus = "Active";

            _mockService.Setup(s => s.GetReinsurerStatus(It.IsAny<string>())).Returns(expectedStatus);

            var result = _mockService.Object.GetReinsurerStatus(reinsurerId);

            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("Inactive", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.GetReinsurerStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RetrieveTreatyCode_ValidInputs_ReturnsCode()
        {
            string poolId = "POOL-J";
            DateTime date = new DateTime(2023, 1, 1);
            string expectedCode = "TC-999";

            _mockService.Setup(s => s.RetrieveTreatyCode(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedCode);

            var result = _mockService.Object.RetrieveTreatyCode(poolId, date);

            Assert.AreEqual(expectedCode, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.StartsWith("TC"));

            _mockService.Verify(s => s.RetrieveTreatyCode(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetAllocationCurrency_ValidPool_ReturnsCurrency()
        {
            string poolId = "POOL-K";
            string expectedCurrency = "USD";

            _mockService.Setup(s => s.GetAllocationCurrency(It.IsAny<string>())).Returns(expectedCurrency);

            var result = _mockService.Object.GetAllocationCurrency(poolId);

            Assert.AreEqual(expectedCurrency, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("EUR", result);
            Assert.IsTrue(result.Length == 3);

            _mockService.Verify(s => s.GetAllocationCurrency(It.IsAny<string>()), Times.Once());
        }
    }
}