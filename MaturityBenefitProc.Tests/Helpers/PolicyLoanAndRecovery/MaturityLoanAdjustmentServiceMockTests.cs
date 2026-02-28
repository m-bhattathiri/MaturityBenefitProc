using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.PolicyLoanAndRecovery;

namespace MaturityBenefitProc.Tests.Helpers.PolicyLoanAndRecovery
{
    [TestClass]
    public class MaturityLoanAdjustmentServiceMockTests
    {
        private Mock<IMaturityLoanAdjustmentService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IMaturityLoanAdjustmentService>();
        }

        [TestMethod]
        public void CalculateNetMaturityValue_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1001";
            DateTime maturityDate = new DateTime(2025, 12, 31);
            decimal expectedValue = 50000.00m;

            _mockService.Setup(s => s.CalculateNetMaturityValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateNetMaturityValue(policyId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            Assert.AreEqual(50000.00m, result);

            _mockService.Verify(s => s.CalculateNetMaturityValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalOutstandingLoan_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1002";
            DateTime calcDate = new DateTime(2023, 10, 1);
            decimal expectedValue = 15000.50m;

            _mockService.Setup(s => s.CalculateTotalOutstandingLoan(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTotalOutstandingLoan(policyId, calcDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 10000m);
            Assert.AreNotEqual(10000m, result);

            _mockService.Verify(s => s.CalculateTotalOutstandingLoan(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateAccruedInterest_ValidInputs_ReturnsExpectedValue()
        {
            string loanId = "LN-5001";
            double rate = 0.05;
            DateTime toDate = new DateTime(2023, 12, 31);
            decimal expectedValue = 750.25m;

            _mockService.Setup(s => s.CalculateAccruedInterest(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateAccruedInterest(loanId, rate, toDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateAccruedInterest(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForMaturity_EligiblePolicy_ReturnsTrue()
        {
            string policyId = "POL-1003";
            bool expectedValue = true;

            _mockService.Setup(s => s.IsPolicyEligibleForMaturity(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.IsPolicyEligibleForMaturity(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsPolicyEligibleForMaturity(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void HasOutstandingLoans_WithLoans_ReturnsTrue()
        {
            string policyId = "POL-1004";
            bool expectedValue = true;

            _mockService.Setup(s => s.HasOutstandingLoans(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.HasOutstandingLoans(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.HasOutstandingLoans(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableInterestRate_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1005";
            string loanType = "PERSONAL";
            double expectedValue = 0.085;

            _mockService.Setup(s => s.GetApplicableInterestRate(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetApplicableInterestRate(policyId, loanType);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0.05);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetApplicableInterestRate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysInArrears_ValidInputs_ReturnsExpectedValue()
        {
            string loanId = "LN-5002";
            DateTime currentDate = new DateTime(2023, 10, 15);
            int expectedValue = 45;

            _mockService.Setup(s => s.GetDaysInArrears(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetDaysInArrears(loanId, currentDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetDaysInArrears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetLoanStatusCode_ValidInputs_ReturnsExpectedValue()
        {
            string loanId = "LN-5003";
            string expectedValue = "ACTIVE";

            _mockService.Setup(s => s.GetLoanStatusCode(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetLoanStatusCode(loanId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("CLOSED", result);

            _mockService.Verify(s => s.GetLoanStatusCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculatePenalInterest_ValidInputs_ReturnsExpectedValue()
        {
            string loanId = "LN-5004";
            int daysOverdue = 30;
            double penaltyRate = 0.02;
            decimal expectedValue = 150.00m;

            _mockService.Setup(s => s.CalculatePenalInterest(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculatePenalInterest(loanId, daysOverdue, penaltyRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculatePenalInterest(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetGrossMaturityBenefit_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1006";
            decimal expectedValue = 100000.00m;

            _mockService.Setup(s => s.GetGrossMaturityBenefit(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetGrossMaturityBenefit(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 50000m);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetGrossMaturityBenefit(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ApplyLoanRecoveryDeduction_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1007";
            decimal grossAmount = 100000m;
            decimal recoveryAmount = 20000m;
            decimal expectedValue = 80000m;

            _mockService.Setup(s => s.ApplyLoanRecoveryDeduction(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.ApplyLoanRecoveryDeduction(policyId, grossAmount, recoveryAmount);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(grossAmount, result);

            _mockService.Verify(s => s.ApplyLoanRecoveryDeduction(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void ValidateRecoveryAmount_ValidInputs_ReturnsTrue()
        {
            string policyId = "POL-1008";
            decimal recoveryAmount = 5000m;
            bool expectedValue = true;

            _mockService.Setup(s => s.ValidateRecoveryAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.ValidateRecoveryAmount(policyId, recoveryAmount);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateRecoveryAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetActiveLoanCount_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1009";
            int expectedValue = 2;

            _mockService.Setup(s => s.GetActiveLoanCount(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetActiveLoanCount(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetActiveLoanCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateRecoveryTransactionId_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1010";
            DateTime transDate = new DateTime(2023, 10, 20);
            string expectedValue = "TXN-20231020-1010";

            _mockService.Setup(s => s.GenerateRecoveryTransactionId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GenerateRecoveryTransactionId(policyId, transDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("TXN"));
            Assert.AreNotEqual(string.Empty, result);

            _mockService.Verify(s => s.GenerateRecoveryTransactionId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLoanToValueRatio_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1011";
            decimal loanAmount = 25000m;
            decimal policyValue = 100000m;
            double expectedValue = 0.25;

            _mockService.Setup(s => s.CalculateLoanToValueRatio(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateLoanToValueRatio(policyId, loanAmount, policyValue);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.CalculateLoanToValueRatio(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateCapitalizedInterest_ValidInputs_ReturnsExpectedValue()
        {
            string loanId = "LN-5005";
            DateTime lastCapDate = new DateTime(2022, 12, 31);
            decimal expectedValue = 1200.50m;

            _mockService.Setup(s => s.CalculateCapitalizedInterest(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateCapitalizedInterest(loanId, lastCapDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateCapitalizedInterest(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsInterestCapitalizationAllowed_AllowedPolicy_ReturnsTrue()
        {
            string policyId = "POL-1012";
            bool expectedValue = true;

            _mockService.Setup(s => s.IsInterestCapitalizationAllowed(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.IsInterestCapitalizationAllowed(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsInterestCapitalizationAllowed(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetUnpaidPremiumDeduction_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1013";
            DateTime maturityDate = new DateTime(2025, 1, 1);
            decimal expectedValue = 1500.00m;

            _mockService.Setup(s => s.GetUnpaidPremiumDeduction(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetUnpaidPremiumDeduction(policyId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetUnpaidPremiumDeduction(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetMonthsToMaturity_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1014";
            DateTime currentDate = new DateTime(2023, 1, 1);
            int expectedValue = 24;

            _mockService.Setup(s => s.GetMonthsToMaturity(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetMonthsToMaturity(policyId, currentDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetMonthsToMaturity(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void RetrievePolicyCurrencyCode_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1015";
            string expectedValue = "USD";

            _mockService.Setup(s => s.RetrievePolicyCurrencyCode(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.RetrievePolicyCurrencyCode(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length == 3);
            Assert.AreNotEqual("EUR", result);

            _mockService.Verify(s => s.RetrievePolicyCurrencyCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxOnRecovery_ValidInputs_ReturnsExpectedValue()
        {
            decimal recoveryAmount = 10000m;
            double taxRate = 0.05;
            decimal expectedValue = 500m;

            _mockService.Setup(s => s.CalculateTaxOnRecovery(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTaxOnRecovery(recoveryAmount, taxRate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateTaxOnRecovery(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CheckLoanDefaultStatus_DefaultedLoan_ReturnsTrue()
        {
            string loanId = "LN-5006";
            bool expectedValue = true;

            _mockService.Setup(s => s.CheckLoanDefaultStatus(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.CheckLoanDefaultStatus(loanId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.CheckLoanDefaultStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalRecoverableAmount_ValidInputs_ReturnsExpectedValue()
        {
            string policyId = "POL-1016";
            decimal expectedValue = 35000.00m;

            _mockService.Setup(s => s.GetTotalRecoverableAmount(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetTotalRecoverableAmount(policyId);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetTotalRecoverableAmount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void MultipleCalls_VerifyExecutionCounts()
        {
            string policyId = "POL-MULTI";
            _mockService.Setup(s => s.GetActiveLoanCount(It.IsAny<string>())).Returns(1);
            _mockService.Setup(s => s.HasOutstandingLoans(It.IsAny<string>())).Returns(true);

            var count1 = _mockService.Object.GetActiveLoanCount(policyId);
            var count2 = _mockService.Object.GetActiveLoanCount(policyId);
            var hasLoans = _mockService.Object.HasOutstandingLoans(policyId);

            Assert.AreEqual(1, count1);
            Assert.AreEqual(1, count2);
            Assert.IsTrue(hasLoans);
            Assert.IsNotNull(count1);

            _mockService.Verify(s => s.GetActiveLoanCount(It.IsAny<string>()), Times.Exactly(2));
            _mockService.Verify(s => s.HasOutstandingLoans(It.IsAny<string>()), Times.AtLeastOnce());
            _mockService.Verify(s => s.IsPolicyEligibleForMaturity(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void CalculateNetMaturityValue_ZeroMaturity_ReturnsZero()
        {
            string policyId = "POL-ZERO";
            DateTime maturityDate = new DateTime(2025, 12, 31);
            decimal expectedValue = 0m;

            _mockService.Setup(s => s.CalculateNetMaturityValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateNetMaturityValue(policyId, maturityDate);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreNotEqual(100m, result);

            _mockService.Verify(s => s.CalculateNetMaturityValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }
    }
}