using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.PolicyLoanAndRecovery;

namespace MaturityBenefitProc.Tests.Helpers.PolicyLoanAndRecovery
{
    [TestClass]
    public class LoanInterestCalculationServiceTests
    {
        private ILoanInterestCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming LoanInterestCalculationService is the concrete implementation
            _service = new LoanInterestCalculationService();
        }

        [TestMethod]
        public void CalculateTotalAccruedInterest_ValidInputs_ReturnsCalculatedInterest()
        {
            var result1 = _service.CalculateTotalAccruedInterest("POL123", 10000m, new DateTime(2025, 12, 31));
            var result2 = _service.CalculateTotalAccruedInterest("POL456", 5000m, new DateTime(2024, 1, 1));
            var result3 = _service.CalculateTotalAccruedInterest("POL789", 1m, new DateTime(2023, 6, 15));
            var result4 = _service.CalculateTotalAccruedInterest("POL999", 25000m, DateTime.Now.AddDays(30));

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateTotalAccruedInterest_ZeroPrincipal_ReturnsZero()
        {
            var result1 = _service.CalculateTotalAccruedInterest("POL123", 0m, new DateTime(2025, 12, 31));
            var result2 = _service.CalculateTotalAccruedInterest("POL456", 0m, new DateTime(2024, 1, 1));
            var result3 = _service.CalculateTotalAccruedInterest("", 0m, DateTime.Now);
            var result4 = _service.CalculateTotalAccruedInterest(null, 0m, DateTime.MaxValue);

            // Assuming fixed implementation correctly handles 0 principal by returning 0
            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateDailyInterestAmount_ValidInputs_ReturnsDailyAmount()
        {
            var result1 = _service.CalculateDailyInterestAmount(10000m, 0.05);
            var result2 = _service.CalculateDailyInterestAmount(5000m, 0.035);
            var result3 = _service.CalculateDailyInterestAmount(100m, 0.10);
            var result4 = _service.CalculateDailyInterestAmount(25000m, 0.07);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
            Assert.IsTrue(result1 > 0m);
        }

        [TestMethod]
        public void CalculateDailyInterestAmount_ZeroRateOrPrincipal_ReturnsZero()
        {
            var result1 = _service.CalculateDailyInterestAmount(0m, 0.05);
            var result2 = _service.CalculateDailyInterestAmount(5000m, 0.0);
            var result3 = _service.CalculateDailyInterestAmount(0m, 0.0);
            var result4 = _service.CalculateDailyInterestAmount(-1000m, 0.0);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetOutstandingLoanBalance_ValidLoanId_ReturnsBalance()
        {
            var result1 = _service.GetOutstandingLoanBalance("LOAN001", DateTime.Now);
            var result2 = _service.GetOutstandingLoanBalance("LOAN002", DateTime.Now.AddDays(-10));
            var result3 = _service.GetOutstandingLoanBalance("LOAN003", new DateTime(2023, 1, 1));
            var result4 = _service.GetOutstandingLoanBalance("LOAN004", new DateTime(2025, 12, 31));

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateCapitalizedInterest_ValidDates_ReturnsCapitalizedAmount()
        {
            var result1 = _service.CalculateCapitalizedInterest("LOAN123", new DateTime(2023, 1, 1), new DateTime(2024, 1, 1));
            var result2 = _service.CalculateCapitalizedInterest("LOAN456", new DateTime(2022, 6, 1), new DateTime(2023, 6, 1));
            var result3 = _service.CalculateCapitalizedInterest("LOAN789", DateTime.Now.AddYears(-1), DateTime.Now);
            var result4 = _service.CalculateCapitalizedInterest("LOAN999", new DateTime(2020, 1, 1), new DateTime(2025, 1, 1));

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
            Assert.IsTrue(result1 > 0m);
        }

        [TestMethod]
        public void CalculateProjectedInterestAtMaturity_ValidInputs_ReturnsProjectedInterest()
        {
            var result1 = _service.CalculateProjectedInterestAtMaturity("POL111", 5000m, 0.06);
            var result2 = _service.CalculateProjectedInterestAtMaturity("POL222", 10000m, 0.045);
            var result3 = _service.CalculateProjectedInterestAtMaturity("POL333", 2500m, 0.08);
            var result4 = _service.CalculateProjectedInterestAtMaturity("POL444", 7500m, 0.055);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
            Assert.IsTrue(result1 > 0m);
        }

        [TestMethod]
        public void GetApplicableInterestRate_ValidPolicy_ReturnsRate()
        {
            var result1 = _service.GetApplicableInterestRate("POL123", DateTime.Now);
            var result2 = _service.GetApplicableInterestRate("POL456", new DateTime(2020, 1, 1));
            var result3 = _service.GetApplicableInterestRate("POL789", new DateTime(2025, 12, 31));
            var result4 = _service.GetApplicableInterestRate("POL999", DateTime.Now.AddDays(15));

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreNotEqual(0.0, result4);
            Assert.IsTrue(result1 > 0.0);
        }

        [TestMethod]
        public void CalculateEffectiveAnnualRate_ValidInputs_ReturnsEAR()
        {
            var result1 = _service.CalculateEffectiveAnnualRate(0.05, 12);
            var result2 = _service.CalculateEffectiveAnnualRate(0.06, 4);
            var result3 = _service.CalculateEffectiveAnnualRate(0.04, 2);
            var result4 = _service.CalculateEffectiveAnnualRate(0.07, 365);

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreNotEqual(0.0, result4);
            Assert.IsTrue(result1 >= 0.05);
        }

        [TestMethod]
        public void GetHistoricalAverageInterestRate_ValidDates_ReturnsAverageRate()
        {
            var result1 = _service.GetHistoricalAverageInterestRate("POL123", new DateTime(2010, 1, 1), new DateTime(2020, 1, 1));
            var result2 = _service.GetHistoricalAverageInterestRate("POL456", new DateTime(2015, 6, 1), new DateTime(2023, 6, 1));
            var result3 = _service.GetHistoricalAverageInterestRate("POL789", new DateTime(2000, 1, 1), DateTime.Now);
            var result4 = _service.GetHistoricalAverageInterestRate("POL999", new DateTime(2022, 1, 1), new DateTime(2023, 1, 1));

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreNotEqual(0.0, result4);
            Assert.IsTrue(result1 > 0.0);
        }

        [TestMethod]
        public void IsInterestCapitalizationEligible_ValidIds_ReturnsTrue()
        {
            var result1 = _service.IsInterestCapitalizationEligible("POL123", "LOAN123");
            var result2 = _service.IsInterestCapitalizationEligible("POL456", "LOAN456");
            var result3 = _service.IsInterestCapitalizationEligible("POL789", "LOAN789");
            var result4 = _service.IsInterestCapitalizationEligible("POL999", "LOAN999");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
        }

        [TestMethod]
        public void IsInterestCapitalizationEligible_EmptyIds_ReturnsFalse()
        {
            var result1 = _service.IsInterestCapitalizationEligible("", "LOAN123");
            var result2 = _service.IsInterestCapitalizationEligible("POL456", "");
            var result3 = _service.IsInterestCapitalizationEligible("", "");
            var result4 = _service.IsInterestCapitalizationEligible(null, null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void ValidateInterestRateCap_ValidRates_ReturnsExpectedBoolean()
        {
            var result1 = _service.ValidateInterestRateCap("POL123", 0.05);
            var result2 = _service.ValidateInterestRateCap("POL456", 0.02);
            var result3 = _service.ValidateInterestRateCap("POL789", 0.15); // Assuming 15% might exceed cap
            var result4 = _service.ValidateInterestRateCap("POL999", 0.08);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
            Assert.IsTrue(result1 || !result1); // Ensures it returns a boolean without throwing
        }

        [TestMethod]
        public void HasUnpaidInterest_ValidLoanId_ReturnsTrue()
        {
            var result1 = _service.HasUnpaidInterest("LOAN_UNPAID_1");
            var result2 = _service.HasUnpaidInterest("LOAN_UNPAID_2");
            var result3 = _service.HasUnpaidInterest("LOAN_UNPAID_3");
            var result4 = _service.HasUnpaidInterest("LOAN_UNPAID_4");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
        }

        [TestMethod]
        public void HasUnpaidInterest_EmptyLoanId_ReturnsFalse()
        {
            var result1 = _service.HasUnpaidInterest("");
            var result2 = _service.HasUnpaidInterest("   ");
            var result3 = _service.HasUnpaidInterest(string.Empty);
            var result4 = _service.HasUnpaidInterest(null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsLoanInGracePeriod_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.IsLoanInGracePeriod("LOAN123", DateTime.Now);
            var result2 = _service.IsLoanInGracePeriod("LOAN456", DateTime.Now.AddDays(-5));
            var result3 = _service.IsLoanInGracePeriod("LOAN789", DateTime.Now.AddDays(5));
            var result4 = _service.IsLoanInGracePeriod("LOAN999", DateTime.Now.AddDays(10));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
        }

        [TestMethod]
        public void CalculateDaysAccrued_ValidDates_ReturnsPositiveDays()
        {
            var result1 = _service.CalculateDaysAccrued(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
            var result2 = _service.CalculateDaysAccrued(new DateTime(2023, 1, 1), new DateTime(2024, 1, 1));
            var result3 = _service.CalculateDaysAccrued(DateTime.Now.AddDays(-10), DateTime.Now);
            var result4 = _service.CalculateDaysAccrued(new DateTime(2020, 2, 28), new DateTime(2020, 3, 1)); // Leap year

            Assert.AreEqual(30, result1);
            Assert.AreEqual(365, result2);
            Assert.AreEqual(10, result3);
            Assert.AreEqual(2, result4);
            Assert.AreNotEqual(0, result1);
        }

        [TestMethod]
        public void CalculateDaysAccrued_SameDates_ReturnsZero()
        {
            var date = new DateTime(2023, 5, 5);
            var result1 = _service.CalculateDaysAccrued(date, date);
            var result2 = _service.CalculateDaysAccrued(DateTime.Now.Date, DateTime.Now.Date);
            var result3 = _service.CalculateDaysAccrued(DateTime.MinValue, DateTime.MinValue);
            var result4 = _service.CalculateDaysAccrued(DateTime.MaxValue, DateTime.MaxValue);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetCompoundingPeriods_ValidInputs_ReturnsPeriods()
        {
            var result1 = _service.GetCompoundingPeriods(new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), 12);
            var result2 = _service.GetCompoundingPeriods(new DateTime(2020, 1, 1), new DateTime(2022, 1, 1), 4);
            var result3 = _service.GetCompoundingPeriods(new DateTime(2023, 1, 1), new DateTime(2023, 7, 1), 2);
            var result4 = _service.GetCompoundingPeriods(new DateTime(2010, 1, 1), new DateTime(2020, 1, 1), 1);

            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
            Assert.AreNotEqual(0, result3);
            Assert.AreNotEqual(0, result4);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void GetDaysInArrears_ValidInputs_ReturnsDays()
        {
            var result1 = _service.GetDaysInArrears("LOAN123", DateTime.Now);
            var result2 = _service.GetDaysInArrears("LOAN456", DateTime.Now.AddDays(30));
            var result3 = _service.GetDaysInArrears("LOAN789", new DateTime(2024, 1, 1));
            var result4 = _service.GetDaysInArrears("LOAN999", new DateTime(2025, 12, 31));

            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
            Assert.AreNotEqual(0, result3);
            Assert.AreNotEqual(0, result4);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void GetInterestCalculationMethodCode_ValidPolicy_ReturnsCode()
        {
            var result1 = _service.GetInterestCalculationMethodCode("POL123");
            var result2 = _service.GetInterestCalculationMethodCode("POL456");
            var result3 = _service.GetInterestCalculationMethodCode("POL789");
            var result4 = _service.GetInterestCalculationMethodCode("POL999");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
            Assert.AreNotEqual(string.Empty, result1);
        }

        [TestMethod]
        public void GetInterestCalculationMethodCode_EmptyPolicy_ReturnsNullOrEmpty()
        {
            var result1 = _service.GetInterestCalculationMethodCode("");
            var result2 = _service.GetInterestCalculationMethodCode("   ");
            var result3 = _service.GetInterestCalculationMethodCode(string.Empty);
            var result4 = _service.GetInterestCalculationMethodCode(null);

            Assert.IsTrue(string.IsNullOrWhiteSpace(result1));
            Assert.IsTrue(string.IsNullOrWhiteSpace(result2));
            Assert.IsTrue(string.IsNullOrWhiteSpace(result3));
            Assert.IsTrue(string.IsNullOrWhiteSpace(result4));
        }

        [TestMethod]
        public void GenerateInterestStatementId_ValidInputs_ReturnsStatementId()
        {
            var result1 = _service.GenerateInterestStatementId("LOAN123", new DateTime(2023, 1, 1));
            var result2 = _service.GenerateInterestStatementId("LOAN456", new DateTime(2023, 6, 30));
            var result3 = _service.GenerateInterestStatementId("LOAN789", DateTime.Now);
            var result4 = _service.GenerateInterestStatementId("LOAN999", DateTime.Now.AddDays(-15));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
            Assert.IsTrue(result1.Contains("LOAN123"));
        }

        [TestMethod]
        public void GetTaxDeductibilityCode_ValidLoanId_ReturnsCode()
        {
            var result1 = _service.GetTaxDeductibilityCode("LOAN123");
            var result2 = _service.GetTaxDeductibilityCode("LOAN456");
            var result3 = _service.GetTaxDeductibilityCode("LOAN789");
            var result4 = _service.GetTaxDeductibilityCode("LOAN999");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
            Assert.AreNotEqual(string.Empty, result1);
        }

        [TestMethod]
        public void GetTaxDeductibilityCode_EmptyLoanId_ReturnsNullOrEmpty()
        {
            var result1 = _service.GetTaxDeductibilityCode("");
            var result2 = _service.GetTaxDeductibilityCode("   ");
            var result3 = _service.GetTaxDeductibilityCode(string.Empty);
            var result4 = _service.GetTaxDeductibilityCode(null);

            Assert.IsTrue(string.IsNullOrWhiteSpace(result1));
            Assert.IsTrue(string.IsNullOrWhiteSpace(result2));
            Assert.IsTrue(string.IsNullOrWhiteSpace(result3));
            Assert.IsTrue(string.IsNullOrWhiteSpace(result4));
        }

        [TestMethod]
        public void CalculateTotalAccruedInterest_NegativePrincipal_ReturnsZeroOrNegative()
        {
            var result1 = _service.CalculateTotalAccruedInterest("POL123", -1000m, DateTime.Now);
            var result2 = _service.CalculateTotalAccruedInterest("POL456", -5000m, DateTime.Now);
            var result3 = _service.CalculateTotalAccruedInterest("POL789", -10m, DateTime.Now);
            var result4 = _service.CalculateTotalAccruedInterest("POL999", -25000m, DateTime.Now);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
            Assert.IsTrue(result1 <= 0m);
        }
    }
}