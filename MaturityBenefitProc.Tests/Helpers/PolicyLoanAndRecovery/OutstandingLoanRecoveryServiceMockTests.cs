using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.PolicyLoanAndRecovery;

namespace MaturityBenefitProc.Tests.Helpers.PolicyLoanAndRecovery
{
    [TestClass]
    public class OutstandingLoanRecoveryServiceMockTests
    {
        private Mock<IOutstandingLoanRecoveryService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IOutstandingLoanRecoveryService>();
        }

        [TestMethod]
        public void CalculateTotalOutstandingPrincipal_ValidPolicy_ReturnsExpectedAmount()
        {
            decimal expectedValue = 1500.50m;
            string policyId = "POL123";
            DateTime maturityDate = new DateTime(2023, 1, 1);

            _mockService.Setup(s => s.CalculateTotalOutstandingPrincipal(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalOutstandingPrincipal(policyId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTotalOutstandingPrincipal(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalOutstandingPrincipal_ZeroBalance_ReturnsZero()
        {
            decimal expectedValue = 0m;
            string policyId = "POL999";
            DateTime maturityDate = new DateTime(2023, 12, 31);

            _mockService.Setup(s => s.CalculateTotalOutstandingPrincipal(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalOutstandingPrincipal(policyId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0m, result);

            _mockService.Verify(s => s.CalculateTotalOutstandingPrincipal(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateAccruedInterest_ValidLoan_ReturnsExpectedInterest()
        {
            decimal expectedValue = 250.75m;
            string loanId = "LN456";
            DateTime calcDate = new DateTime(2023, 5, 1);

            _mockService.Setup(s => s.CalculateAccruedInterest(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateAccruedInterest(loanId, calcDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 100m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateAccruedInterest(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetDailyInterestAccrualAmount_ValidParams_ReturnsExpectedAmount()
        {
            decimal expectedValue = 1.25m;
            string loanId = "LN789";
            decimal balance = 5000m;
            double rate = 0.05;

            _mockService.Setup(s => s.GetDailyInterestAccrualAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.GetDailyInterestAccrualAmount(loanId, balance, rate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(10m, result);

            _mockService.Verify(s => s.GetDailyInterestAccrualAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculatePenalties_DaysInArrears_ReturnsExpectedPenalty()
        {
            decimal expectedValue = 50.00m;
            string loanId = "LN111";
            int days = 30;

            _mockService.Setup(s => s.CalculatePenalties(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.CalculatePenalties(loanId, days);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 50.00m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculatePenalties(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalRecoveryAmount_ValidIds_ReturnsTotal()
        {
            decimal expectedValue = 2000.00m;
            string policyId = "POL222";
            string loanId = "LN222";

            _mockService.Setup(s => s.GetTotalRecoveryAmount(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetTotalRecoveryAmount(policyId, loanId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 1000m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetTotalRecoveryAmount(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxOnRecovery_ValidAmount_ReturnsTax()
        {
            decimal expectedValue = 100.00m;
            decimal amount = 1000m;
            string taxCode = "TX1";

            _mockService.Setup(s => s.CalculateTaxOnRecovery(It.IsAny<decimal>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTaxOnRecovery(amount, taxCode);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 100.00m);
            Assert.AreNotEqual(50m, result);

            _mockService.Verify(s => s.CalculateTaxOnRecovery(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingMaturityProceeds_ValidAmounts_ReturnsRemaining()
        {
            decimal expectedValue = 8000.00m;
            decimal totalMaturity = 10000m;
            decimal recovery = 2000m;

            _mockService.Setup(s => s.GetRemainingMaturityProceeds(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.GetRemainingMaturityProceeds(totalMaturity, recovery);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.AreNotEqual(10000m, result);

            _mockService.Verify(s => s.GetRemainingMaturityProceeds(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetCurrentInterestRate_ValidLoan_ReturnsRate()
        {
            double expectedValue = 0.055;
            string loanId = "LN333";

            _mockService.Setup(s => s.GetCurrentInterestRate(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetCurrentInterestRate(loanId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0.0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetCurrentInterestRate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPenaltyRate_ValidParams_ReturnsRate()
        {
            double expectedValue = 0.02;
            string loanId = "LN444";
            string policyType = "LIFE";

            _mockService.Setup(s => s.GetPenaltyRate(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetPenaltyRate(loanId, policyType);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.02);
            Assert.AreNotEqual(0.05, result);

            _mockService.Verify(s => s.GetPenaltyRate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoanToValueRatio_ValidAmounts_ReturnsRatio()
        {
            double expectedValue = 0.5;
            decimal loanAmount = 5000m;
            decimal cashValue = 10000m;

            _mockService.Setup(s => s.CalculateLoanToValueRatio(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateLoanToValueRatio(loanAmount, cashValue);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.5);
            Assert.AreNotEqual(1.0, result);

            _mockService.Verify(s => s.CalculateLoanToValueRatio(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetHistoricalAverageInterestRate_ValidDates_ReturnsAverage()
        {
            double expectedValue = 0.045;
            string loanId = "LN555";
            DateTime start = new DateTime(2020, 1, 1);
            DateTime end = new DateTime(2022, 1, 1);

            _mockService.Setup(s => s.GetHistoricalAverageInterestRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetHistoricalAverageInterestRate(loanId, start, end);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0.0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetHistoricalAverageInterestRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsLoanEligibleForRecovery_Eligible_ReturnsTrue()
        {
            bool expectedValue = true;
            string loanId = "LN666";
            DateTime maturityDate = new DateTime(2023, 1, 1);

            _mockService.Setup(s => s.IsLoanEligibleForRecovery(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.IsLoanEligibleForRecovery(loanId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsLoanEligibleForRecovery(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void HasActiveDefault_NoDefault_ReturnsFalse()
        {
            bool expectedValue = false;
            string policyId = "POL333";

            _mockService.Setup(s => s.HasActiveDefault(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.HasActiveDefault(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.HasActiveDefault(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsRecoveryAmountWithinLimits_WithinLimits_ReturnsTrue()
        {
            bool expectedValue = true;
            decimal recovery = 1000m;
            decimal proceeds = 5000m;

            _mockService.Setup(s => s.IsRecoveryAmountWithinLimits(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.IsRecoveryAmountWithinLimits(recovery, proceeds);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsRecoveryAmountWithinLimits(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void ValidateLoanStatus_ValidStatus_ReturnsTrue()
        {
            bool expectedValue = true;
            string loanId = "LN777";
            string status = "ACTIVE";

            _mockService.Setup(s => s.ValidateLoanStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.ValidateLoanStatus(loanId, status);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateLoanStatus(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RequiresManualReview_HighAmount_ReturnsTrue()
        {
            bool expectedValue = true;
            string policyId = "POL444";
            decimal amount = 50000m;
            int count = 3;

            _mockService.Setup(s => s.RequiresManualReview(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.RequiresManualReview(policyId, amount, count);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.RequiresManualReview(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysInArrears_ValidParams_ReturnsDays()
        {
            int expectedValue = 15;
            string loanId = "LN888";
            DateTime date = new DateTime(2023, 6, 1);

            _mockService.Setup(s => s.GetDaysInArrears(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetDaysInArrears(loanId, date);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetDaysInArrears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetNumberOfActiveLoans_ValidPolicy_ReturnsCount()
        {
            int expectedValue = 2;
            string policyId = "POL555";

            _mockService.Setup(s => s.GetNumberOfActiveLoans(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetNumberOfActiveLoans(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 2);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetNumberOfActiveLoans(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingTermDays_ValidParams_ReturnsDays()
        {
            int expectedValue = 120;
            string loanId = "LN999";
            DateTime date = new DateTime(2023, 12, 31);

            _mockService.Setup(s => s.GetRemainingTermDays(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetRemainingTermDays(loanId, date);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 100);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetRemainingTermDays(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetGracePeriodDays_ValidType_ReturnsDays()
        {
            int expectedValue = 30;
            string policyType = "TERM";

            _mockService.Setup(s => s.GetGracePeriodDays(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetGracePeriodDays(policyType);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 30);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetGracePeriodDays(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPrimaryLoanId_ValidPolicy_ReturnsId()
        {
            string expectedValue = "LN001";
            string policyId = "POL666";

            _mockService.Setup(s => s.GetPrimaryLoanId(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetPrimaryLoanId(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("LN"));
            Assert.AreNotEqual("LN999", result);

            _mockService.Verify(s => s.GetPrimaryLoanId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRecoveryTransactionReference_ValidParams_ReturnsRef()
        {
            string expectedValue = "REC-12345";
            string policyId = "POL777";
            decimal amount = 1500m;

            _mockService.Setup(s => s.GetRecoveryTransactionReference(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.GetRecoveryTransactionReference(policyId, amount);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("REC"));
            Assert.AreNotEqual("", result);

            _mockService.Verify(s => s.GetRecoveryTransactionReference(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetLoanStatusCode_ValidLoan_ReturnsCode()
        {
            string expectedValue = "ACT";
            string loanId = "LN002";

            _mockService.Setup(s => s.GetLoanStatusCode(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetLoanStatusCode(loanId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length == 3);
            Assert.AreNotEqual("CLS", result);

            _mockService.Verify(s => s.GetLoanStatusCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateRecoveryAuditTrailId_ValidParams_ReturnsId()
        {
            string expectedValue = "AUDIT-999";
            string policyId = "POL888";
            string userId = "USR1";
            DateTime time = DateTime.Now;

            _mockService.Setup(s => s.GenerateRecoveryAuditTrailId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GenerateRecoveryAuditTrailId(policyId, userId, time);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("AUDIT"));
            Assert.AreNotEqual("TEST", result);

            _mockService.Verify(s => s.GenerateRecoveryAuditTrailId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }
    }
}