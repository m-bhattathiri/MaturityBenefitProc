using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.Reinsurance_Pool_Management;

namespace MaturityBenefitProc.Tests.Helpers.Reinsurance_Pool_Management
{
    [TestClass]
    public class MaturityReinsuranceRecoveryServiceMockTests
    {
        private Mock<IMaturityReinsuranceRecoveryService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IMaturityReinsuranceRecoveryService>();
        }

        [TestMethod]
        public void CalculateTotalRecoveryAmount_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-123";
            decimal totalMaturityBenefit = 10000m;
            decimal expectedValue = 5000m;

            _mockService.Setup(s => s.CalculateTotalRecoveryAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalRecoveryAmount(policyId, totalMaturityBenefit);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTotalRecoveryAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateQuotaShareRecovery_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-124";
            decimal maturityAmount = 10000m;
            double quotaSharePercentage = 0.4;
            decimal expectedValue = 4000m;

            _mockService.Setup(s => s.CalculateQuotaShareRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateQuotaShareRecovery(policyId, maturityAmount, quotaSharePercentage);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 4000m);
            Assert.AreNotEqual(10000m, result);
            _mockService.Verify(s => s.CalculateQuotaShareRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSurplusTreatyRecovery_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-125";
            decimal maturityAmount = 15000m;
            decimal retentionLimit = 5000m;
            decimal expectedValue = 10000m;

            _mockService.Setup(s => s.CalculateSurplusTreatyRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateSurplusTreatyRecovery(policyId, maturityAmount, retentionLimit);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > retentionLimit);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateSurplusTreatyRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateExcessOfLossRecovery_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-126";
            decimal maturityAmount = 20000m;
            decimal attachmentPoint = 10000m;
            decimal expectedValue = 10000m;

            _mockService.Setup(s => s.CalculateExcessOfLossRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateExcessOfLossRecovery(policyId, maturityAmount, attachmentPoint);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 10000m);
            Assert.AreNotEqual(20000m, result);
            _mockService.Verify(s => s.CalculateExcessOfLossRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetReinsurancePercentage_ValidInputs_ReturnsExpectedPercentage()
        {
            string policyId = "POL-127";
            DateTime maturityDate = new DateTime(2023, 1, 1);
            double expectedValue = 0.5;

            _mockService.Setup(s => s.GetReinsurancePercentage(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetReinsurancePercentage(policyId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(1.0, result);
            _mockService.Verify(s => s.GetReinsurancePercentage(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPoolAllocationRatio_ValidInputs_ReturnsExpectedRatio()
        {
            string poolId = "POOL-1";
            string reinsurerId = "RE-1";
            double expectedValue = 0.25;

            _mockService.Setup(s => s.GetPoolAllocationRatio(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetPoolAllocationRatio(poolId, reinsurerId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < 1.0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetPoolAllocationRatio(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyReinsured_ReinsuredPolicy_ReturnsTrue()
        {
            string policyId = "POL-128";
            bool expectedValue = true;

            _mockService.Setup(s => s.IsPolicyReinsured(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.IsPolicyReinsured(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsPolicyReinsured(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsReinsurerActive_ActiveReinsurer_ReturnsTrue()
        {
            string reinsurerId = "RE-2";
            DateTime checkDate = DateTime.Now;
            bool expectedValue = true;

            _mockService.Setup(s => s.IsReinsurerActive(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.IsReinsurerActive(reinsurerId, checkDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsReinsurerActive(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateTreatyLimits_WithinLimits_ReturnsTrue()
        {
            string treatyId = "TR-1";
            decimal recoveryAmount = 5000m;
            bool expectedValue = true;

            _mockService.Setup(s => s.ValidateTreatyLimits(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.ValidateTreatyLimits(treatyId, recoveryAmount);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateTreatyLimits(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CheckFacultativeEligibility_Eligible_ReturnsTrue()
        {
            string policyId = "POL-129";
            decimal maturityAmount = 100000m;
            bool expectedValue = true;

            _mockService.Setup(s => s.CheckFacultativeEligibility(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CheckFacultativeEligibility(policyId, maturityAmount);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.CheckFacultativeEligibility(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysUntilRecoveryDue_ValidInputs_ReturnsDays()
        {
            string reinsurerId = "RE-3";
            DateTime claimDate = DateTime.Now;
            int expectedValue = 30;

            _mockService.Setup(s => s.GetDaysUntilRecoveryDue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetDaysUntilRecoveryDue(reinsurerId, claimDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 30);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetDaysUntilRecoveryDue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetReinsurerCountForPolicy_ValidPolicy_ReturnsCount()
        {
            string policyId = "POL-130";
            int expectedValue = 2;

            _mockService.Setup(s => s.GetReinsurerCountForPolicy(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetReinsurerCountForPolicy(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetReinsurerCountForPolicy(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetActiveTreatiesCount_ValidInputs_ReturnsCount()
        {
            string reinsurerId = "RE-4";
            DateTime asOfDate = DateTime.Now;
            int expectedValue = 5;

            _mockService.Setup(s => s.GetActiveTreatiesCount(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetActiveTreatiesCount(reinsurerId, asOfDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 5);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetActiveTreatiesCount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPrimaryReinsurerId_ValidPolicy_ReturnsId()
        {
            string policyId = "POL-131";
            string expectedValue = "RE-1";

            _mockService.Setup(s => s.GetPrimaryReinsurerId(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetPrimaryReinsurerId(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("RE"));
            Assert.AreNotEqual(string.Empty, result);
            _mockService.Verify(s => s.GetPrimaryReinsurerId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTreatyCode_ValidInputs_ReturnsCode()
        {
            string policyId = "POL-132";
            DateTime maturityDate = DateTime.Now;
            string expectedValue = "TR-2023";

            _mockService.Setup(s => s.GetTreatyCode(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetTreatyCode(policyId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("TR-2022", result);
            _mockService.Verify(s => s.GetTreatyCode(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GenerateRecoveryClaimReference_ValidInputs_ReturnsReference()
        {
            string policyId = "POL-133";
            string reinsurerId = "RE-5";
            string expectedValue = "REF-12345";

            _mockService.Setup(s => s.GenerateRecoveryClaimReference(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GenerateRecoveryClaimReference(policyId, reinsurerId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("REF"));
            Assert.AreNotEqual(string.Empty, result);
            _mockService.Verify(s => s.GenerateRecoveryClaimReference(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProportionalRecovery_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-134";
            decimal amount = 10000m;
            double proportion = 0.3;
            decimal expectedValue = 3000m;

            _mockService.Setup(s => s.CalculateProportionalRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateProportionalRecovery(policyId, amount, proportion);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 3000m);
            Assert.AreNotEqual(10000m, result);
            _mockService.Verify(s => s.CalculateProportionalRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateNonProportionalRecovery_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-135";
            decimal amount = 25000m;
            decimal deductible = 10000m;
            decimal expectedValue = 15000m;

            _mockService.Setup(s => s.CalculateNonProportionalRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateNonProportionalRecovery(policyId, amount, deductible);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateNonProportionalRecovery(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetFacultativeReinsuranceRate_ValidPolicy_ReturnsRate()
        {
            string policyId = "POL-136";
            double expectedValue = 0.15;

            _mockService.Setup(s => s.GetFacultativeReinsuranceRate(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetFacultativeReinsuranceRate(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetFacultativeReinsuranceRate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsPoolArrangementValid_ValidPool_ReturnsTrue()
        {
            string poolId = "POOL-2";
            DateTime maturityDate = DateTime.Now;
            bool expectedValue = true;

            _mockService.Setup(s => s.IsPoolArrangementValid(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.IsPoolArrangementValid(poolId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsPoolArrangementValid(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPoolMemberCount_ValidPool_ReturnsCount()
        {
            string poolId = "POOL-3";
            int expectedValue = 4;

            _mockService.Setup(s => s.GetPoolMemberCount(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetPoolMemberCount(poolId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 4);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetPoolMemberCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPoolAdministratorId_ValidPool_ReturnsId()
        {
            string poolId = "POOL-4";
            string expectedValue = "ADMIN-1";

            _mockService.Setup(s => s.GetPoolAdministratorId(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetPoolAdministratorId(poolId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("ADMIN"));
            Assert.AreNotEqual(string.Empty, result);
            _mockService.Verify(s => s.GetPoolAdministratorId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculatePoolMemberShare_ValidInputs_ReturnsShare()
        {
            string poolId = "POOL-5";
            string memberId = "MEM-1";
            decimal totalRecovery = 10000m;
            decimal expectedValue = 2500m;

            _mockService.Setup(s => s.CalculatePoolMemberShare(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculatePoolMemberShare(poolId, memberId, totalRecovery);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 2500m);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculatePoolMemberShare(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void ApplyCurrencyConversion_ValidInputs_ReturnsConvertedAmount()
        {
            decimal amount = 1000m;
            string fromCurrency = "USD";
            string toCurrency = "EUR";
            DateTime conversionDate = DateTime.Now;
            decimal expectedValue = 850m;

            _mockService.Setup(s => s.ApplyCurrencyConversion(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.ApplyCurrencyConversion(amount, fromCurrency, toCurrency, conversionDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(amount, result);
            _mockService.Verify(s => s.ApplyCurrencyConversion(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCurrencyExchangeRate_ValidInputs_ReturnsRate()
        {
            string fromCurrency = "USD";
            string toCurrency = "GBP";
            DateTime date = DateTime.Now;
            double expectedValue = 0.75;

            _mockService.Setup(s => s.GetCurrencyExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetCurrencyExchangeRate(fromCurrency, toCurrency, date);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(1.0, result);
            _mockService.Verify(s => s.GetCurrencyExchangeRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateCurrencyCode_ValidCode_ReturnsTrue()
        {
            string currencyCode = "USD";
            bool expectedValue = true;

            _mockService.Setup(s => s.ValidateCurrencyCode(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.ValidateCurrencyCode(currencyCode);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateCurrencyCode(It.IsAny<string>()), Times.Once());
        }
    }
}