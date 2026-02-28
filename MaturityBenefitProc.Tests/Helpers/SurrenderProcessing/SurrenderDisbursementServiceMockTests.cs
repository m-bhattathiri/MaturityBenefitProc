using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.SurrenderProcessing;

namespace MaturityBenefitProc.Tests.Helpers.SurrenderProcessing
{
    [TestClass]
    public class SurrenderDisbursementServiceMockTests
    {
        private Mock<ISurrenderDisbursementService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ISurrenderDisbursementService>();
        }

        [TestMethod]
        public void CalculateTotalSurrenderValue_ValidPolicy_ReturnsExpectedValue()
        {
            var policyId = "POL123";
            var effectiveDate = new DateTime(2023, 1, 1);
            var expectedValue = 15000.50m;

            _mockService.Setup(s => s.CalculateTotalSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalSurrenderValue(policyId, effectiveDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTotalSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalSurrenderValue_MultipleCalls_ReturnsExpectedValue()
        {
            var policyId = "POL124";
            var effectiveDate = new DateTime(2023, 2, 1);
            var expectedValue = 25000.75m;

            _mockService.Setup(s => s.CalculateTotalSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result1 = _mockService.Object.CalculateTotalSurrenderValue(policyId, effectiveDate);
            var result2 = _mockService.Object.CalculateTotalSurrenderValue(policyId, effectiveDate);

            Assert.AreEqual(expectedValue, result1);
            Assert.AreEqual(expectedValue, result2);
            Assert.AreEqual(result1, result2);
            Assert.IsNotNull(result1);

            _mockService.Verify(s => s.CalculateTotalSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(2));
        }

        [TestMethod]
        public void CalculatePenalties_ValidBaseValue_ReturnsExpectedPenalty()
        {
            var policyId = "POL125";
            var baseValue = 10000m;
            var expectedPenalty = 500m;

            _mockService.Setup(s => s.CalculatePenalties(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedPenalty);

            var result = _mockService.Object.CalculatePenalties(policyId, baseValue);

            Assert.AreEqual(expectedPenalty, result);
            Assert.IsTrue(result < baseValue);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(baseValue, result);

            _mockService.Verify(s => s.CalculatePenalties(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetOutstandingLoanBalance_HasLoan_ReturnsBalance()
        {
            var policyId = "POL126";
            var expectedBalance = 1200.00m;

            _mockService.Setup(s => s.GetOutstandingLoanBalance(It.IsAny<string>())).Returns(expectedBalance);

            var result = _mockService.Object.GetOutstandingLoanBalance(policyId);

            Assert.AreEqual(expectedBalance, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetOutstandingLoanBalance(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxWithholding_ValidAmount_ReturnsTax()
        {
            var taxableAmount = 5000m;
            var taxRate = 0.20;
            var expectedTax = 1000m;

            _mockService.Setup(s => s.CalculateTaxWithholding(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedTax);

            var result = _mockService.Object.CalculateTaxWithholding(taxableAmount, taxRate);

            Assert.AreEqual(expectedTax, result);
            Assert.IsTrue(result < taxableAmount);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTaxWithholding(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetFinalDisbursementAmount_ValidPolicy_ReturnsAmount()
        {
            var policyId = "POL127";
            var expectedAmount = 13500.50m;

            _mockService.Setup(s => s.GetFinalDisbursementAmount(It.IsAny<string>())).Returns(expectedAmount);

            var result = _mockService.Object.GetFinalDisbursementAmount(policyId);

            Assert.AreEqual(expectedAmount, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetFinalDisbursementAmount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProratedDividends_ValidDate_ReturnsDividends()
        {
            var policyId = "POL128";
            var surrenderDate = new DateTime(2023, 6, 30);
            var expectedDividends = 250.75m;

            _mockService.Setup(s => s.CalculateProratedDividends(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDividends);

            var result = _mockService.Object.CalculateProratedDividends(policyId, surrenderDate);

            Assert.AreEqual(expectedDividends, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateProratedDividends(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_ValidFundValue_ReturnsAdjustment()
        {
            var policyId = "POL129";
            var fundValue = 50000m;
            var expectedAdjustment = -1500m;

            _mockService.Setup(s => s.CalculateMarketValueAdjustment(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedAdjustment);

            var result = _mockService.Object.CalculateMarketValueAdjustment(policyId, fundValue);

            Assert.AreEqual(expectedAdjustment, result);
            Assert.IsTrue(result < 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateMarketValueAdjustment(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetSurrenderChargeRate_ValidYears_ReturnsRate()
        {
            var policyId = "POL130";
            var policyYears = 5;
            var expectedRate = 0.05;

            _mockService.Setup(s => s.GetSurrenderChargeRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedRate);

            var result = _mockService.Object.GetSurrenderChargeRate(policyId, policyYears);

            Assert.AreEqual(expectedRate, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetSurrenderChargeRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableTaxRate_ValidState_ReturnsRate()
        {
            var stateCode = "NY";
            var expectedRate = 0.0882;

            _mockService.Setup(s => s.GetApplicableTaxRate(It.IsAny<string>())).Returns(expectedRate);

            var result = _mockService.Object.GetApplicableTaxRate(stateCode);

            Assert.AreEqual(expectedRate, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetApplicableTaxRate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateVestingPercentage_ValidMonths_ReturnsPercentage()
        {
            var policyId = "POL131";
            var monthsActive = 36;
            var expectedPercentage = 0.60;

            _mockService.Setup(s => s.CalculateVestingPercentage(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedPercentage);

            var result = _mockService.Object.CalculateVestingPercentage(policyId, monthsActive);

            Assert.AreEqual(expectedPercentage, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.CalculateVestingPercentage(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetInterestRateForDelayedPayment_ValidPolicy_ReturnsRate()
        {
            var policyId = "POL132";
            var expectedRate = 0.03;

            _mockService.Setup(s => s.GetInterestRateForDelayedPayment(It.IsAny<string>())).Returns(expectedRate);

            var result = _mockService.Object.GetInterestRateForDelayedPayment(policyId);

            Assert.AreEqual(expectedRate, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetInterestRateForDelayedPayment(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForSurrender_Eligible_ReturnsTrue()
        {
            var policyId = "POL133";
            var expectedResult = true;

            _mockService.Setup(s => s.IsEligibleForSurrender(It.IsAny<string>())).Returns(expectedResult);

            var result = _mockService.Object.IsEligibleForSurrender(policyId);

            Assert.AreEqual(expectedResult, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsEligibleForSurrender(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateBankRoutingNumber_ValidNumber_ReturnsTrue()
        {
            var routingNumber = "123456789";
            var expectedResult = true;

            _mockService.Setup(s => s.ValidateBankRoutingNumber(It.IsAny<string>())).Returns(expectedResult);

            var result = _mockService.Object.ValidateBankRoutingNumber(routingNumber);

            Assert.AreEqual(expectedResult, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateBankRoutingNumber(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RequiresSpousalConsent_Required_ReturnsTrue()
        {
            var policyId = "POL134";
            var stateCode = "CA";
            var expectedResult = true;

            _mockService.Setup(s => s.RequiresSpousalConsent(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedResult);

            var result = _mockService.Object.RequiresSpousalConsent(policyId, stateCode);

            Assert.AreEqual(expectedResult, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.RequiresSpousalConsent(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void HasActiveGarnishments_NoGarnishments_ReturnsFalse()
        {
            var policyId = "POL135";
            var expectedResult = false;

            _mockService.Setup(s => s.HasActiveGarnishments(It.IsAny<string>())).Returns(expectedResult);

            var result = _mockService.Object.HasActiveGarnishments(policyId);

            Assert.AreEqual(expectedResult, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.HasActiveGarnishments(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsDisbursementApproved_Approved_ReturnsTrue()
        {
            var policyId = "POL136";
            var amount = 10000m;
            var expectedResult = true;

            _mockService.Setup(s => s.IsDisbursementApproved(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedResult);

            var result = _mockService.Object.IsDisbursementApproved(policyId, amount);

            Assert.AreEqual(expectedResult, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsDisbursementApproved(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void VerifyBeneficiarySignatures_ValidSignatures_ReturnsTrue()
        {
            var policyId = "POL137";
            var requiredSignatures = 2;
            var expectedResult = true;

            _mockService.Setup(s => s.VerifyBeneficiarySignatures(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedResult);

            var result = _mockService.Object.VerifyBeneficiarySignatures(policyId, requiredSignatures);

            Assert.AreEqual(expectedResult, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.VerifyBeneficiarySignatures(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysUntilProcessingDeadline_ValidDate_ReturnsDays()
        {
            var requestDate = new DateTime(2023, 10, 1);
            var expectedDays = 15;

            _mockService.Setup(s => s.GetDaysUntilProcessingDeadline(It.IsAny<DateTime>())).Returns(expectedDays);

            var result = _mockService.Object.GetDaysUntilProcessingDeadline(requestDate);

            Assert.AreEqual(expectedDays, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetDaysUntilProcessingDeadline(It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetActivePolicyMonths_ValidPolicy_ReturnsMonths()
        {
            var policyId = "POL138";
            var expectedMonths = 48;

            _mockService.Setup(s => s.GetActivePolicyMonths(It.IsAny<string>())).Returns(expectedMonths);

            var result = _mockService.Object.GetActivePolicyMonths(policyId);

            Assert.AreEqual(expectedMonths, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetActivePolicyMonths(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CountPendingDisbursementHolds_ValidPolicy_ReturnsCount()
        {
            var policyId = "POL139";
            var expectedCount = 1;

            _mockService.Setup(s => s.CountPendingDisbursementHolds(It.IsAny<string>())).Returns(expectedCount);

            var result = _mockService.Object.CountPendingDisbursementHolds(policyId);

            Assert.AreEqual(expectedCount, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.CountPendingDisbursementHolds(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetGracePeriodDays_ValidPolicy_ReturnsDays()
        {
            var policyId = "POL140";
            var expectedDays = 30;

            _mockService.Setup(s => s.GetGracePeriodDays(It.IsAny<string>())).Returns(expectedDays);

            var result = _mockService.Object.GetGracePeriodDays(policyId);

            Assert.AreEqual(expectedDays, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetGracePeriodDays(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateDisbursementTransactionId_ValidInputs_ReturnsId()
        {
            var policyId = "POL141";
            var processingDate = new DateTime(2023, 11, 1);
            var expectedId = "TXN-POL141-20231101";

            _mockService.Setup(s => s.GenerateDisbursementTransactionId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedId);

            var result = _mockService.Object.GenerateDisbursementTransactionId(policyId, processingDate);

            Assert.AreEqual(expectedId, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GenerateDisbursementTransactionId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxFormTypeCode_QualifiedPlan_ReturnsFormCode()
        {
            var disbursementAmount = 15000m;
            var isQualifiedPlan = true;
            var expectedForm = "1099-R";

            _mockService.Setup(s => s.GetTaxFormTypeCode(It.IsAny<decimal>(), It.IsAny<bool>())).Returns(expectedForm);

            var result = _mockService.Object.GetTaxFormTypeCode(disbursementAmount, isQualifiedPlan);

            Assert.AreEqual(expectedForm, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GetTaxFormTypeCode(It.IsAny<decimal>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void GetPaymentMethodCode_ValidPolicy_ReturnsMethod()
        {
            var policyId = "POL142";
            var expectedMethod = "EFT";

            _mockService.Setup(s => s.GetPaymentMethodCode(It.IsAny<string>())).Returns(expectedMethod);

            var result = _mockService.Object.GetPaymentMethodCode(policyId);

            Assert.AreEqual(expectedMethod, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GetPaymentMethodCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void DetermineDisbursementStatus_ValidTransaction_ReturnsStatus()
        {
            var transactionId = "TXN12345";
            var expectedStatus = "PROCESSED";

            _mockService.Setup(s => s.DetermineDisbursementStatus(It.IsAny<string>())).Returns(expectedStatus);

            var result = _mockService.Object.DetermineDisbursementStatus(transactionId);

            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.DetermineDisbursementStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalSurrenderValue_NeverCalled_VerifiesNever()
        {
            _mockService.Setup(s => s.CalculateTotalSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(100m);

            _mockService.Verify(s => s.CalculateTotalSurrenderValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never());
            Assert.IsNotNull(_mockService);
            Assert.IsInstanceOfType(_mockService.Object, typeof(ISurrenderDisbursementService));
        }
    }
}