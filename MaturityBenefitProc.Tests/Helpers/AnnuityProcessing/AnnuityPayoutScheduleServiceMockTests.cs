using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.AnnuityProcessing;

namespace MaturityBenefitProc.Tests.Helpers.AnnuityProcessing
{
    [TestClass]
    public class AnnuityPayoutScheduleServiceMockTests
    {
        private Mock<IAnnuityPayoutScheduleService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IAnnuityPayoutScheduleService>();
        }

        [TestMethod]
        public void CalculateMonthlyPayout_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-123";
            decimal principalAmount = 100000m;
            double interestRate = 0.05;
            decimal expectedValue = 500m;

            _mockService.Setup(s => s.CalculateMonthlyPayout(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateMonthlyPayout(policyId, principalAmount, interestRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateMonthlyPayout(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateQuarterlyPayout_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-124";
            decimal principalAmount = 100000m;
            double interestRate = 0.05;
            decimal expectedValue = 1500m;

            _mockService.Setup(s => s.CalculateQuarterlyPayout(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateQuarterlyPayout(policyId, principalAmount, interestRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateQuarterlyPayout(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateAnnualPayout_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-125";
            decimal principalAmount = 100000m;
            double interestRate = 0.05;
            decimal expectedValue = 6000m;

            _mockService.Setup(s => s.CalculateAnnualPayout(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateAnnualPayout(policyId, principalAmount, interestRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateAnnualPayout(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalProjectedPayout_ValidSchedule_ReturnsExpectedAmount()
        {
            string scheduleId = "SCH-001";
            decimal expectedValue = 150000m;

            _mockService.Setup(s => s.GetTotalProjectedPayout(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetTotalProjectedPayout(scheduleId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.GetTotalProjectedPayout(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxWithholding_ValidInputs_ReturnsExpectedAmount()
        {
            decimal payoutAmount = 1000m;
            double taxRate = 0.20;
            decimal expectedValue = 200m;

            _mockService.Setup(s => s.CalculateTaxWithholding(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTaxWithholding(payoutAmount, taxRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateTaxWithholding(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingPrincipal_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-126";
            DateTime asOfDate = new DateTime(2023, 1, 1);
            decimal expectedValue = 85000m;

            _mockService.Setup(s => s.GetRemainingPrincipal(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetRemainingPrincipal(policyId, asOfDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.GetRemainingPrincipal(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculatePenaltyForEarlyWithdrawal_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL-127";
            decimal withdrawalAmount = 5000m;
            decimal expectedValue = 500m;

            _mockService.Setup(s => s.CalculatePenaltyForEarlyWithdrawal(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculatePenaltyForEarlyWithdrawal(policyId, withdrawalAmount);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculatePenaltyForEarlyWithdrawal(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetCurrentInterestRate_ValidPolicy_ReturnsExpectedRate()
        {
            string policyId = "POL-128";
            double expectedValue = 0.045;

            _mockService.Setup(s => s.GetCurrentInterestRate(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetCurrentInterestRate(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            
            _mockService.Verify(s => s.GetCurrentInterestRate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateCostOfLivingAdjustment_ValidInputs_ReturnsExpectedAdjustment()
        {
            string scheduleId = "SCH-002";
            int year = 2024;
            double expectedValue = 0.02;

            _mockService.Setup(s => s.CalculateCostOfLivingAdjustment(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateCostOfLivingAdjustment(scheduleId, year);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            
            _mockService.Verify(s => s.CalculateCostOfLivingAdjustment(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetSurvivorBenefitRatio_ValidPolicy_ReturnsExpectedRatio()
        {
            string policyId = "POL-129";
            double expectedValue = 0.50;

            _mockService.Setup(s => s.GetSurvivorBenefitRatio(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetSurvivorBenefitRatio(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            
            _mockService.Verify(s => s.GetSurvivorBenefitRatio(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsEligibleForPayout_Eligible_ReturnsTrue()
        {
            string policyId = "POL-130";
            DateTime requestedDate = new DateTime(2023, 5, 1);
            bool expectedValue = true;

            _mockService.Setup(s => s.IsEligibleForPayout(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.IsEligibleForPayout(policyId, requestedDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            
            _mockService.Verify(s => s.IsEligibleForPayout(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateScheduleParameters_ValidParams_ReturnsTrue()
        {
            string policyId = "POL-131";
            int payoutFrequency = 12;
            decimal amount = 1000m;
            bool expectedValue = true;

            _mockService.Setup(s => s.ValidateScheduleParameters(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.ValidateScheduleParameters(policyId, payoutFrequency, amount);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            
            _mockService.Verify(s => s.ValidateScheduleParameters(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void HasActiveSchedule_Active_ReturnsTrue()
        {
            string policyId = "POL-132";
            bool expectedValue = true;

            _mockService.Setup(s => s.HasActiveSchedule(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.HasActiveSchedule(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            
            _mockService.Verify(s => s.HasActiveSchedule(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void SuspendPayoutSchedule_ValidSchedule_ReturnsTrue()
        {
            string scheduleId = "SCH-003";
            string reasonCode = "R01";
            bool expectedValue = true;

            _mockService.Setup(s => s.SuspendPayoutSchedule(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.SuspendPayoutSchedule(scheduleId, reasonCode);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            
            _mockService.Verify(s => s.SuspendPayoutSchedule(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ResumePayoutSchedule_ValidSchedule_ReturnsTrue()
        {
            string scheduleId = "SCH-004";
            bool expectedValue = true;

            _mockService.Setup(s => s.ResumePayoutSchedule(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.ResumePayoutSchedule(scheduleId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            
            _mockService.Verify(s => s.ResumePayoutSchedule(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ApproveScheduleModifications_ValidApproval_ReturnsTrue()
        {
            string scheduleId = "SCH-005";
            string approverId = "APP-001";
            bool expectedValue = true;

            _mockService.Setup(s => s.ApproveScheduleModifications(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.ApproveScheduleModifications(scheduleId, approverId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            
            _mockService.Verify(s => s.ApproveScheduleModifications(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingPayoutCount_ValidSchedule_ReturnsExpectedCount()
        {
            string scheduleId = "SCH-006";
            int expectedValue = 120;

            _mockService.Setup(s => s.GetRemainingPayoutCount(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetRemainingPayoutCount(scheduleId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.GetRemainingPayoutCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysUntilNextPayout_ValidSchedule_ReturnsExpectedDays()
        {
            string scheduleId = "SCH-007";
            DateTime currentDate = new DateTime(2023, 6, 15);
            int expectedValue = 15;

            _mockService.Setup(s => s.GetDaysUntilNextPayout(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetDaysUntilNextPayout(scheduleId, currentDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.GetDaysUntilNextPayout(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalInstallments_ValidDates_ReturnsExpectedCount()
        {
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2033, 1, 1);
            int frequencyCode = 12;
            int expectedValue = 120;

            _mockService.Setup(s => s.CalculateTotalInstallments(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalInstallments(startDate, endDate, frequencyCode);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.CalculateTotalInstallments(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetGracePeriodDays_ValidPolicy_ReturnsExpectedDays()
        {
            string policyId = "POL-133";
            int expectedValue = 30;

            _mockService.Setup(s => s.GetGracePeriodDays(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetGracePeriodDays(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.GetGracePeriodDays(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateScheduleId_ValidInputs_ReturnsExpectedId()
        {
            string policyId = "POL-134";
            DateTime creationDate = new DateTime(2023, 1, 1);
            string expectedValue = "SCH-POL-134-2023";

            _mockService.Setup(s => s.GenerateScheduleId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GenerateScheduleId(policyId, creationDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);
            
            _mockService.Verify(s => s.GenerateScheduleId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPayoutStatusCode_ValidSchedule_ReturnsExpectedCode()
        {
            string scheduleId = "SCH-008";
            string expectedValue = "ACTIVE";

            _mockService.Setup(s => s.GetPayoutStatusCode(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetPayoutStatusCode(scheduleId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);
            
            _mockService.Verify(s => s.GetPayoutStatusCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void DetermineTaxFormType_ValidInputs_ReturnsExpectedForm()
        {
            string policyId = "POL-135";
            decimal annualPayoutTotal = 25000m;
            string expectedValue = "1099-R";

            _mockService.Setup(s => s.DetermineTaxFormType(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.DetermineTaxFormType(policyId, annualPayoutTotal);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);
            
            _mockService.Verify(s => s.DetermineTaxFormType(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void UpdateBeneficiaryDetails_ValidInputs_ReturnsExpectedStatus()
        {
            string scheduleId = "SCH-009";
            string beneficiaryId = "BEN-001";
            string expectedValue = "SUCCESS";

            _mockService.Setup(s => s.UpdateBeneficiaryDetails(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.UpdateBeneficiaryDetails(scheduleId, beneficiaryId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);
            
            _mockService.Verify(s => s.UpdateBeneficiaryDetails(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetNextProcessingBatchId_ValidDate_ReturnsExpectedBatchId()
        {
            DateTime processingDate = new DateTime(2023, 7, 1);
            string expectedValue = "BATCH-20230701";

            _mockService.Setup(s => s.GetNextProcessingBatchId(It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetNextProcessingBatchId(processingDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual(string.Empty, result);
            
            _mockService.Verify(s => s.GetNextProcessingBatchId(It.IsAny<DateTime>()), Times.Once());
        }
    }
}