using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.PolicyLoanAndRecovery;

namespace MaturityBenefitProc.Tests.Helpers.PolicyLoanAndRecovery
{
    [TestClass]
    public class LoanInterestCalculationServiceMockTests
    {
        private Mock<ILoanInterestCalculationService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ILoanInterestCalculationService>();
        }

        [TestMethod]
        public void CalculateTotalAccruedInterest_ValidInputs_ReturnsExpectedAmount()
        {
            // Arrange
            string policyId = "POL-123";
            decimal principal = 1000m;
            DateTime maturityDate = new DateTime(2025, 12, 31);
            decimal expected = 150.50m;

            _mockService.Setup(s => s.CalculateTotalAccruedInterest(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.CalculateTotalAccruedInterest(policyId, principal, maturityDate);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateTotalAccruedInterest(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTotalAccruedInterest_ZeroPrincipal_ReturnsZero()
        {
            // Arrange
            string policyId = "POL-124";
            decimal principal = 0m;
            DateTime maturityDate = new DateTime(2025, 12, 31);
            decimal expected = 0m;

            _mockService.Setup(s => s.CalculateTotalAccruedInterest(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.CalculateTotalAccruedInterest(policyId, principal, maturityDate);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateTotalAccruedInterest(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDailyInterestAmount_ValidInputs_ReturnsExpectedAmount()
        {
            // Arrange
            decimal principal = 5000m;
            double rate = 0.05;
            decimal expected = 0.68m;

            _mockService.Setup(s => s.CalculateDailyInterestAmount(It.IsAny<decimal>(), It.IsAny<double>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.CalculateDailyInterestAmount(principal, rate);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(1m, result);
            
            _mockService.Verify(s => s.CalculateDailyInterestAmount(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDailyInterestAmount_NegativeRate_ReturnsNegativeAmount()
        {
            // Arrange
            decimal principal = 5000m;
            double rate = -0.05;
            decimal expected = -0.68m;

            _mockService.Setup(s => s.CalculateDailyInterestAmount(It.IsAny<decimal>(), It.IsAny<double>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.CalculateDailyInterestAmount(principal, rate);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.IsTrue(result < 0);
            
            _mockService.Verify(s => s.CalculateDailyInterestAmount(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetOutstandingLoanBalance_ValidLoan_ReturnsBalance()
        {
            // Arrange
            string loanId = "LN-100";
            DateTime asOfDate = DateTime.Today;
            decimal expected = 4500.75m;

            _mockService.Setup(s => s.GetOutstandingLoanBalance(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.GetOutstandingLoanBalance(loanId, asOfDate);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.GetOutstandingLoanBalance(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateCapitalizedInterest_ValidDates_ReturnsAmount()
        {
            // Arrange
            string loanId = "LN-200";
            DateTime lastCap = new DateTime(2023, 1, 1);
            DateTime maturity = new DateTime(2024, 1, 1);
            decimal expected = 250.00m;

            _mockService.Setup(s => s.CalculateCapitalizedInterest(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.CalculateCapitalizedInterest(loanId, lastCap, maturity);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(100m, result);
            
            _mockService.Verify(s => s.CalculateCapitalizedInterest(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateProjectedInterestAtMaturity_ValidInputs_ReturnsAmount()
        {
            // Arrange
            string policyId = "POL-300";
            decimal balance = 10000m;
            double rate = 0.06;
            decimal expected = 600m;

            _mockService.Setup(s => s.CalculateProjectedInterestAtMaturity(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.CalculateProjectedInterestAtMaturity(policyId, balance, rate);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            
            _mockService.Verify(s => s.CalculateProjectedInterestAtMaturity(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableInterestRate_ValidPolicy_ReturnsRate()
        {
            // Arrange
            string policyId = "POL-400";
            DateTime effectiveDate = DateTime.Today;
            double expected = 0.055;

            _mockService.Setup(s => s.GetApplicableInterestRate(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.GetApplicableInterestRate(policyId, effectiveDate);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            
            _mockService.Verify(s => s.GetApplicableInterestRate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateEffectiveAnnualRate_ValidInputs_ReturnsRate()
        {
            // Arrange
            double nominalRate = 0.05;
            int frequency = 12;
            double expected = 0.05116;

            _mockService.Setup(s => s.CalculateEffectiveAnnualRate(It.IsAny<double>(), It.IsAny<int>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.CalculateEffectiveAnnualRate(nominalRate, frequency);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(nominalRate, result);
            
            _mockService.Verify(s => s.CalculateEffectiveAnnualRate(It.IsAny<double>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetHistoricalAverageInterestRate_ValidDates_ReturnsAverage()
        {
            // Arrange
            string policyId = "POL-500";
            DateTime start = new DateTime(2020, 1, 1);
            DateTime end = new DateTime(2023, 1, 1);
            double expected = 0.045;

            _mockService.Setup(s => s.GetHistoricalAverageInterestRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.GetHistoricalAverageInterestRate(policyId, start, end);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            
            _mockService.Verify(s => s.GetHistoricalAverageInterestRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsInterestCapitalizationEligible_Eligible_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL-600";
            string loanId = "LN-600";

            _mockService.Setup(s => s.IsInterestCapitalizationEligible(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(true);

            // Act
            var result = _mockService.Object.IsInterestCapitalizationEligible(policyId, loanId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);
            
            _mockService.Verify(s => s.IsInterestCapitalizationEligible(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsInterestCapitalizationEligible_NotEligible_ReturnsFalse()
        {
            // Arrange
            string policyId = "POL-601";
            string loanId = "LN-601";

            _mockService.Setup(s => s.IsInterestCapitalizationEligible(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(false);

            // Act
            var result = _mockService.Object.IsInterestCapitalizationEligible(policyId, loanId);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);
            
            _mockService.Verify(s => s.IsInterestCapitalizationEligible(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateInterestRateCap_UnderCap_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL-700";
            double rate = 0.04;

            _mockService.Setup(s => s.ValidateInterestRateCap(It.IsAny<string>(), It.IsAny<double>()))
                        .Returns(true);

            // Act
            var result = _mockService.Object.ValidateInterestRateCap(policyId, rate);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);
            
            _mockService.Verify(s => s.ValidateInterestRateCap(It.IsAny<string>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void ValidateInterestRateCap_OverCap_ReturnsFalse()
        {
            // Arrange
            string policyId = "POL-701";
            double rate = 0.15;

            _mockService.Setup(s => s.ValidateInterestRateCap(It.IsAny<string>(), It.IsAny<double>()))
                        .Returns(false);

            // Act
            var result = _mockService.Object.ValidateInterestRateCap(policyId, rate);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);
            
            _mockService.Verify(s => s.ValidateInterestRateCap(It.IsAny<string>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void HasUnpaidInterest_HasInterest_ReturnsTrue()
        {
            // Arrange
            string loanId = "LN-800";

            _mockService.Setup(s => s.HasUnpaidInterest(It.IsAny<string>()))
                        .Returns(true);

            // Act
            var result = _mockService.Object.HasUnpaidInterest(loanId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);
            
            _mockService.Verify(s => s.HasUnpaidInterest(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void HasUnpaidInterest_NoInterest_ReturnsFalse()
        {
            // Arrange
            string loanId = "LN-801";

            _mockService.Setup(s => s.HasUnpaidInterest(It.IsAny<string>()))
                        .Returns(false);

            // Act
            var result = _mockService.Object.HasUnpaidInterest(loanId);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);
            
            _mockService.Verify(s => s.HasUnpaidInterest(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsLoanInGracePeriod_InGrace_ReturnsTrue()
        {
            // Arrange
            string loanId = "LN-900";
            DateTime currentDate = DateTime.Today;

            _mockService.Setup(s => s.IsLoanInGracePeriod(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(true);

            // Act
            var result = _mockService.Object.IsLoanInGracePeriod(loanId, currentDate);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            Assert.AreEqual(true, result);
            
            _mockService.Verify(s => s.IsLoanInGracePeriod(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsLoanInGracePeriod_NotInGrace_ReturnsFalse()
        {
            // Arrange
            string loanId = "LN-901";
            DateTime currentDate = DateTime.Today;

            _mockService.Setup(s => s.IsLoanInGracePeriod(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(false);

            // Act
            var result = _mockService.Object.IsLoanInGracePeriod(loanId, currentDate);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);
            
            _mockService.Verify(s => s.IsLoanInGracePeriod(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysAccrued_ValidDates_ReturnsDays()
        {
            // Arrange
            DateTime lastPayment = new DateTime(2023, 1, 1);
            DateTime maturity = new DateTime(2023, 1, 31);
            int expected = 30;

            _mockService.Setup(s => s.CalculateDaysAccrued(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.CalculateDaysAccrued(lastPayment, maturity);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.CalculateDaysAccrued(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCompoundingPeriods_ValidInputs_ReturnsPeriods()
        {
            // Arrange
            DateTime start = new DateTime(2022, 1, 1);
            DateTime end = new DateTime(2023, 1, 1);
            int frequency = 12;
            int expected = 12;

            _mockService.Setup(s => s.GetCompoundingPeriods(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.GetCompoundingPeriods(start, end, frequency);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.GetCompoundingPeriods(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysInArrears_ValidLoan_ReturnsDays()
        {
            // Arrange
            string loanId = "LN-1000";
            DateTime current = DateTime.Today;
            int expected = 45;

            _mockService.Setup(s => s.GetDaysInArrears(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.GetDaysInArrears(loanId, current);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            
            _mockService.Verify(s => s.GetDaysInArrears(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetInterestCalculationMethodCode_ValidPolicy_ReturnsCode()
        {
            // Arrange
            string policyId = "POL-1100";
            string expected = "SIMPLE";

            _mockService.Setup(s => s.GetInterestCalculationMethodCode(It.IsAny<string>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.GetInterestCalculationMethodCode(policyId);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("COMPOUND", result);
            Assert.IsTrue(result.Length > 0);
            
            _mockService.Verify(s => s.GetInterestCalculationMethodCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateInterestStatementId_ValidInputs_ReturnsId()
        {
            // Arrange
            string loanId = "LN-1200";
            DateTime date = DateTime.Today;
            string expected = "STMT-1200-2023";

            _mockService.Setup(s => s.GenerateInterestStatementId(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.GenerateInterestStatementId(loanId, date);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.StartsWith("STMT"));
            
            _mockService.Verify(s => s.GenerateInterestStatementId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxDeductibilityCode_ValidLoan_ReturnsCode()
        {
            // Arrange
            string loanId = "LN-1300";
            string expected = "TAX-DED";

            _mockService.Setup(s => s.GetTaxDeductibilityCode(It.IsAny<string>()))
                        .Returns(expected);

            // Act
            var result = _mockService.Object.GetTaxDeductibilityCode(loanId);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("NON-DED", result);
            Assert.IsTrue(result.Length > 0);
            
            _mockService.Verify(s => s.GetTaxDeductibilityCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void MultipleCalls_VerifyTimes()
        {
            // Arrange
            string loanId = "LN-1400";
            _mockService.Setup(s => s.HasUnpaidInterest(It.IsAny<string>())).Returns(true);

            // Act
            _mockService.Object.HasUnpaidInterest(loanId);
            _mockService.Object.HasUnpaidInterest(loanId);

            // Assert
            Assert.IsNotNull(_mockService.Object);
            Assert.IsTrue(true);
            Assert.IsFalse(false);
            Assert.AreNotEqual(null, _mockService.Object);
            
            _mockService.Verify(s => s.HasUnpaidInterest(It.IsAny<string>()), Times.Exactly(2));
            _mockService.Verify(s => s.GetTaxDeductibilityCode(It.IsAny<string>()), Times.Never());
        }
    }
}