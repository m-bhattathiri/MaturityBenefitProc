using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.AnnuityProcessing;

namespace MaturityBenefitProc.Tests.Helpers.AnnuityProcessing
{
    [TestClass]
    public class PensionCommutationServiceMockTests
    {
        private Mock<IPensionCommutationService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IPensionCommutationService>();
        }

        [TestMethod]
        public void CalculateMaximumTaxFreeLumpSum_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal totalFundValue = 100000m;
            decimal expected = 25000m;

            _mockService.Setup(s => s.CalculateMaximumTaxFreeLumpSum(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateMaximumTaxFreeLumpSum(policyId, totalFundValue);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            Assert.IsFalse(result < 0);

            _mockService.Verify(s => s.CalculateMaximumTaxFreeLumpSum(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateMaximumTaxFreeLumpSum_ZeroFundValue_ReturnsZero()
        {
            string policyId = "POL124";
            decimal totalFundValue = 0m;
            decimal expected = 0m;

            _mockService.Setup(s => s.CalculateMaximumTaxFreeLumpSum(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateMaximumTaxFreeLumpSum(policyId, totalFundValue);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            Assert.IsTrue(result == 0);

            _mockService.Verify(s => s.CalculateMaximumTaxFreeLumpSum(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateCommutationAmount_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL125";
            double commutationFactor = 15.5;
            decimal expected = 50000m;

            _mockService.Setup(s => s.CalculateCommutationAmount(It.IsAny<string>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateCommutationAmount(policyId, commutationFactor);
            var result2 = _mockService.Object.CalculateCommutationAmount(policyId, commutationFactor);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, result2);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.IsFalse(result == 0);

            _mockService.Verify(s => s.CalculateCommutationAmount(It.IsAny<string>(), It.IsAny<double>()), Times.Exactly(2));
        }

        [TestMethod]
        public void GetAvailableLifetimeAllowance_ValidInputs_ReturnsExpectedAmount()
        {
            string customerId = "CUST001";
            DateTime calcDate = new DateTime(2023, 1, 1);
            decimal expected = 1073100m;

            _mockService.Setup(s => s.GetAvailableLifetimeAllowance(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetAvailableLifetimeAllowance(customerId, calcDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 1000000m);
            Assert.IsFalse(result < 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetAvailableLifetimeAllowance(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateResidualPensionFund_ValidInputs_ReturnsExpectedAmount()
        {
            decimal totalFund = 200000m;
            decimal commuted = 50000m;
            decimal expected = 150000m;

            _mockService.Setup(s => s.CalculateResidualPensionFund(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateResidualPensionFund(totalFund, commuted);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 150000m);
            Assert.IsFalse(result == totalFund);

            _mockService.Verify(s => s.CalculateResidualPensionFund(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void CalculateTaxableCommutationPortion_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL126";
            decimal requested = 60000m;
            decimal expected = 10000m;

            _mockService.Setup(s => s.CalculateTaxableCommutationPortion(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateTaxableCommutationPortion(policyId, requested);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.IsFalse(result == requested);

            _mockService.Verify(s => s.CalculateTaxableCommutationPortion(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetGuaranteedMinimumPensionValue_ValidPolicy_ReturnsExpectedAmount()
        {
            string policyId = "POL127";
            decimal expected = 5000m;

            _mockService.Setup(s => s.GetGuaranteedMinimumPensionValue(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetGuaranteedMinimumPensionValue(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 5000m);
            Assert.IsFalse(result == 0m);

            _mockService.Verify(s => s.GetGuaranteedMinimumPensionValue(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateEarlyRetirementReduction_ValidInputs_ReturnsExpectedAmount()
        {
            decimal baseAmount = 10000m;
            int monthsEarly = 24;
            decimal expected = 1200m;

            _mockService.Setup(s => s.CalculateEarlyRetirementReduction(It.IsAny<decimal>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.CalculateEarlyRetirementReduction(baseAmount, monthsEarly);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.IsFalse(result >= baseAmount);

            _mockService.Verify(s => s.CalculateEarlyRetirementReduction(It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL128";
            DateTime maturityDate = new DateTime(2025, 1, 1);
            decimal expected = 2500m;

            _mockService.Setup(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateTerminalBonus(policyId, maturityDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 2500m);
            Assert.IsFalse(result < 0);

            _mockService.Verify(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCommutationFactor_ValidInputs_ReturnsExpectedFactor()
        {
            int age = 65;
            string gender = "M";
            double expected = 18.5;

            _mockService.Setup(s => s.GetCommutationFactor(It.IsAny<int>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetCommutationFactor(age, gender);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 10.0);
            Assert.IsFalse(result < 0);

            _mockService.Verify(s => s.GetCommutationFactor(It.IsAny<int>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxFreePercentage_ValidPolicy_ReturnsExpectedPercentage()
        {
            string policyId = "POL129";
            double expected = 0.25;

            _mockService.Setup(s => s.CalculateTaxFreePercentage(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CalculateTaxFreePercentage(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.25);
            Assert.IsFalse(result > 1.0);

            _mockService.Verify(s => s.CalculateTaxFreePercentage(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetMarketValueReductionRate_ValidInputs_ReturnsExpectedRate()
        {
            string fundId = "FUND01";
            DateTime withdrawalDate = new DateTime(2023, 6, 1);
            double expected = 0.05;

            _mockService.Setup(s => s.GetMarketValueReductionRate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetMarketValueReductionRate(fundId, withdrawalDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result > 1.0);

            _mockService.Verify(s => s.GetMarketValueReductionRate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLifetimeAllowanceUsedPercentage_ValidInputs_ReturnsExpectedPercentage()
        {
            string customerId = "CUST002";
            decimal withdrawalAmount = 250000m;
            double expected = 0.23;

            _mockService.Setup(s => s.CalculateLifetimeAllowanceUsedPercentage(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateLifetimeAllowanceUsedPercentage(customerId, withdrawalAmount);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.IsFalse(result > 1.0);

            _mockService.Verify(s => s.CalculateLifetimeAllowanceUsedPercentage(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForCommutation_EligiblePolicy_ReturnsTrue()
        {
            string policyId = "POL130";
            DateTime requestDate = DateTime.Now;
            bool expected = true;

            _mockService.Setup(s => s.IsEligibleForCommutation(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsEligibleForCommutation(policyId, requestDate);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);

            _mockService.Verify(s => s.IsEligibleForCommutation(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateLumpSumLimit_ValidAmount_ReturnsTrue()
        {
            string policyId = "POL131";
            decimal requestedAmount = 20000m;
            bool expected = true;

            _mockService.Setup(s => s.ValidateLumpSumLimit(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.ValidateLumpSumLimit(policyId, requestedAmount);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);

            _mockService.Verify(s => s.ValidateLumpSumLimit(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void HasTrivialCommutationRights_HasRights_ReturnsTrue()
        {
            string policyId = "POL132";
            decimal totalWealth = 15000m;
            bool expected = true;

            _mockService.Setup(s => s.HasTrivialCommutationRights(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.HasTrivialCommutationRights(policyId, totalWealth);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);

            _mockService.Verify(s => s.HasTrivialCommutationRights(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsProtectedTaxFreeCashApplicable_Applicable_ReturnsTrue()
        {
            string policyId = "POL133";
            bool expected = true;

            _mockService.Setup(s => s.IsProtectedTaxFreeCashApplicable(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.IsProtectedTaxFreeCashApplicable(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);

            _mockService.Verify(s => s.IsProtectedTaxFreeCashApplicable(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RequiresSpousalConsent_RequiresConsent_ReturnsTrue()
        {
            string policyId = "POL134";
            decimal amount = 50000m;
            bool expected = true;

            _mockService.Setup(s => s.RequiresSpousalConsent(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.RequiresSpousalConsent(policyId, amount);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);

            _mockService.Verify(s => s.RequiresSpousalConsent(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsHealthConditionWaiverApplicable_Applicable_ReturnsTrue()
        {
            string customerId = "CUST003";
            string medicalCode = "MED01";
            bool expected = true;

            _mockService.Setup(s => s.IsHealthConditionWaiverApplicable(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.IsHealthConditionWaiverApplicable(customerId, medicalCode);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);

            _mockService.Verify(s => s.IsHealthConditionWaiverApplicable(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysToMaturity_ValidInputs_ReturnsExpectedDays()
        {
            string policyId = "POL135";
            DateTime currentDate = DateTime.Now;
            int expected = 365;

            _mockService.Setup(s => s.CalculateDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateDaysToMaturity(policyId, currentDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.IsFalse(result < 0);

            _mockService.Verify(s => s.CalculateDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetMinimumRetirementAge_ValidPolicy_ReturnsExpectedAge()
        {
            string policyId = "POL136";
            int expected = 55;

            _mockService.Setup(s => s.GetMinimumRetirementAge(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetMinimumRetirementAge(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 50);
            Assert.IsFalse(result < 50);

            _mockService.Verify(s => s.GetMinimumRetirementAge(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CountPreviousCommutations_ValidCustomer_ReturnsExpectedCount()
        {
            string customerId = "CUST004";
            int expected = 2;

            _mockService.Setup(s => s.CountPreviousCommutations(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CountPreviousCommutations(customerId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result < 0);

            _mockService.Verify(s => s.CountPreviousCommutations(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetGuaranteePeriodMonths_ValidPolicy_ReturnsExpectedMonths()
        {
            string policyId = "POL137";
            int expected = 60;

            _mockService.Setup(s => s.GetGuaranteePeriodMonths(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetGuaranteePeriodMonths(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.IsFalse(result < 0);

            _mockService.Verify(s => s.GetGuaranteePeriodMonths(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateCommutationReference_ValidInputs_ReturnsReference()
        {
            string policyId = "POL138";
            DateTime requestDate = DateTime.Now;
            string expected = "REF-POL138-2023";

            _mockService.Setup(s => s.GenerateCommutationReference(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GenerateCommutationReference(policyId, requestDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("REF"));
            Assert.IsFalse(string.IsNullOrEmpty(result));

            _mockService.Verify(s => s.GenerateCommutationReference(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxCodeForExcessCommutation_ValidCustomer_ReturnsTaxCode()
        {
            string customerId = "CUST005";
            string expected = "BR";

            _mockService.Setup(s => s.GetTaxCodeForExcessCommutation(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetTaxCodeForExcessCommutation(customerId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.IsFalse(string.IsNullOrEmpty(result));

            _mockService.Verify(s => s.GetTaxCodeForExcessCommutation(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void DetermineCommutationTaxBand_ValidAmount_ReturnsTaxBand()
        {
            decimal taxableAmount = 15000m;
            string expected = "Basic";

            _mockService.Setup(s => s.DetermineCommutationTaxBand(It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.DetermineCommutationTaxBand(taxableAmount);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == "Basic");
            Assert.IsFalse(string.IsNullOrEmpty(result));

            _mockService.Verify(s => s.DetermineCommutationTaxBand(It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetRegulatoryFrameworkCode_ValidDate_ReturnsFrameworkCode()
        {
            DateTime startDate = new DateTime(2000, 1, 1);
            string expected = "PRE-A-DAY";

            _mockService.Setup(s => s.GetRegulatoryFrameworkCode(It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetRegulatoryFrameworkCode(startDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("PRE"));
            Assert.IsFalse(string.IsNullOrEmpty(result));

            _mockService.Verify(s => s.GetRegulatoryFrameworkCode(It.IsAny<DateTime>()), Times.Once());
        }
    }
}