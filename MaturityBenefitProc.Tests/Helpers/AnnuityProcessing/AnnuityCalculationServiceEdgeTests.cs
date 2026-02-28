using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.AnnuityProcessing;

namespace MaturityBenefitProc.Tests.Helpers.AnnuityProcessing
{
    [TestClass]
    public class AnnuityCalculationServiceEdgeCaseTests
    {
        private IAnnuityCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming AnnuityCalculationService is the concrete implementation
            _service = new AnnuityCalculationService();
        }

        [TestMethod]
        public void CalculateMonthlyPayout_ZeroAndNegativeCorpus_ReturnsZero()
        {
            decimal resultZero = _service.CalculateMonthlyPayout("POL123", 0m, 0.05);
            decimal resultNeg = _service.CalculateMonthlyPayout("POL123", -1000m, 0.05);
            decimal resultZeroInt = _service.CalculateMonthlyPayout("POL123", 10000m, 0.0);
            decimal resultNegInt = _service.CalculateMonthlyPayout("POL123", 10000m, -0.05);

            Assert.AreEqual(0m, resultZero, "Zero corpus should yield zero payout.");
            Assert.AreEqual(0m, resultNeg, "Negative corpus should yield zero payout.");
            Assert.AreEqual(0m, resultZeroInt, "Zero interest should yield zero payout.");
            Assert.AreEqual(0m, resultNegInt, "Negative interest should yield zero payout.");
        }

        [TestMethod]
        public void CalculateAnnualPayout_MaxValues_HandlesLargeNumbers()
        {
            decimal resultLargeCorpus = _service.CalculateAnnualPayout("POL123", decimal.MaxValue, 0.05);
            decimal resultLargeInt = _service.CalculateAnnualPayout("POL123", 10000m, double.MaxValue);
            decimal resultBothLarge = _service.CalculateAnnualPayout("POL123", decimal.MaxValue, 1.0);

            Assert.IsNotNull(resultLargeCorpus);
            Assert.IsNotNull(resultLargeInt);
            Assert.IsNotNull(resultBothLarge);
            Assert.IsTrue(resultLargeCorpus >= 0m);
        }

        [TestMethod]
        public void CalculateQuarterlyPayout_NullAndEmptyPolicyId_ReturnsZero()
        {
            decimal resultNull = _service.CalculateQuarterlyPayout(null, 10000m, 0.05);
            decimal resultEmpty = _service.CalculateQuarterlyPayout(string.Empty, 10000m, 0.05);
            decimal resultWhitespace = _service.CalculateQuarterlyPayout("   ", 10000m, 0.05);

            Assert.AreEqual(0m, resultNull);
            Assert.AreEqual(0m, resultEmpty);
            Assert.AreEqual(0m, resultWhitespace);
            Assert.AreNotEqual(100m, resultNull);
        }

        [TestMethod]
        public void CalculateSemiAnnualPayout_BoundaryInterestRates_ReturnsExpected()
        {
            decimal resultZero = _service.CalculateSemiAnnualPayout("P1", 10000m, 0.0);
            decimal resultOne = _service.CalculateSemiAnnualPayout("P1", 10000m, 1.0); // 100%
            decimal resultOverOne = _service.CalculateSemiAnnualPayout("P1", 10000m, 1.5); // 150%

            Assert.AreEqual(0m, resultZero);
            Assert.IsTrue(resultOne > 0m);
            Assert.IsTrue(resultOverOne > resultOne);
            Assert.IsNotNull(resultZero);
        }

        [TestMethod]
        public void GetTotalAccumulatedCorpus_ExtremeDates_ReturnsZero()
        {
            decimal resultMinDate = _service.GetTotalAccumulatedCorpus("P1", DateTime.MinValue);
            decimal resultMaxDate = _service.GetTotalAccumulatedCorpus("P1", DateTime.MaxValue);
            decimal resultNullPolicy = _service.GetTotalAccumulatedCorpus(null, DateTime.Now);

            Assert.AreEqual(0m, resultMinDate);
            Assert.AreEqual(0m, resultMaxDate);
            Assert.AreEqual(0m, resultNullPolicy);
            Assert.IsNotNull(resultMinDate);
        }

        [TestMethod]
        public void CalculateCommutationAmount_InvalidPercentages_ReturnsZeroOrMax()
        {
            decimal resultNeg = _service.CalculateCommutationAmount("P1", 10000m, -0.1);
            decimal resultOver100 = _service.CalculateCommutationAmount("P1", 10000m, 1.5);
            decimal resultZero = _service.CalculateCommutationAmount("P1", 10000m, 0.0);
            decimal resultNegCorpus = _service.CalculateCommutationAmount("P1", -10000m, 0.3);

            Assert.AreEqual(0m, resultNeg);
            Assert.AreEqual(10000m, resultOver100, "Should cap at 100% of corpus.");
            Assert.AreEqual(0m, resultZero);
            Assert.AreEqual(0m, resultNegCorpus);
        }

        [TestMethod]
        public void CalculateResidualCorpus_CommutedGreaterThanTotal_ReturnsZero()
        {
            decimal resultGreater = _service.CalculateResidualCorpus(10000m, 15000m);
            decimal resultEqual = _service.CalculateResidualCorpus(10000m, 10000m);
            decimal resultNegCommuted = _service.CalculateResidualCorpus(10000m, -5000m);
            decimal resultNegTotal = _service.CalculateResidualCorpus(-10000m, 5000m);

            Assert.AreEqual(0m, resultGreater);
            Assert.AreEqual(0m, resultEqual);
            Assert.AreEqual(10000m, resultNegCommuted, "Negative commuted should be ignored or treated as 0.");
            Assert.AreEqual(0m, resultNegTotal);
        }

        [TestMethod]
        public void GetAnnuityFactor_InvalidAges_ReturnsZero()
        {
            double resultNegAge = _service.GetAnnuityFactor(-5, "OPT1", 0.05);
            double resultHighAge = _service.GetAnnuityFactor(150, "OPT1", 0.05);
            double resultNullOpt = _service.GetAnnuityFactor(60, null, 0.05);
            double resultNegRate = _service.GetAnnuityFactor(60, "OPT1", -0.05);

            Assert.AreEqual(0.0, resultNegAge);
            Assert.AreEqual(0.0, resultHighAge);
            Assert.AreEqual(0.0, resultNullOpt);
            Assert.AreEqual(0.0, resultNegRate);
        }

        [TestMethod]
        public void GetCurrentInterestRate_InvalidInputs_ReturnsZero()
        {
            double resultNullCode = _service.GetCurrentInterestRate(null, DateTime.Now);
            double resultMinDate = _service.GetCurrentInterestRate("PROD1", DateTime.MinValue);
            double resultMaxDate = _service.GetCurrentInterestRate("PROD1", DateTime.MaxValue);

            Assert.AreEqual(0.0, resultNullCode);
            Assert.AreEqual(0.0, resultMinDate);
            Assert.AreEqual(0.0, resultMaxDate);
            Assert.IsNotNull(resultNullCode);
        }

        [TestMethod]
        public void CalculateInternalRateOfReturn_ZeroOrNegativeInputs_ReturnsZero()
        {
            double resultZeroPrem = _service.CalculateInternalRateOfReturn("P1", 0m, 10000m);
            double resultNegPrem = _service.CalculateInternalRateOfReturn("P1", -5000m, 10000m);
            double resultZeroPayout = _service.CalculateInternalRateOfReturn("P1", 5000m, 0m);
            double resultNegPayout = _service.CalculateInternalRateOfReturn("P1", 5000m, -10000m);

            Assert.AreEqual(0.0, resultZeroPrem);
            Assert.AreEqual(0.0, resultNegPrem);
            Assert.AreEqual(0.0, resultZeroPayout);
            Assert.AreEqual(0.0, resultNegPayout);
        }

        [TestMethod]
        public void ComputeMortalityChargeRate_InvalidAgeOrGender_ReturnsFallback()
        {
            double resultNegAge = _service.ComputeMortalityChargeRate(-10, "M");
            double resultHighAge = _service.ComputeMortalityChargeRate(200, "F");
            double resultNullGender = _service.ComputeMortalityChargeRate(50, null);
            double resultInvalidGender = _service.ComputeMortalityChargeRate(50, "X");

            Assert.AreEqual(0.0, resultNegAge);
            Assert.AreEqual(0.0, resultHighAge);
            Assert.AreEqual(0.0, resultNullGender);
            Assert.AreEqual(0.0, resultInvalidGender);
        }

        [TestMethod]
        public void CalculateInflationAdjustmentFactor_InvalidDates_ReturnsOne()
        {
            double resultBaseAfterCurrent = _service.CalculateInflationAdjustmentFactor(DateTime.Now.AddDays(1), DateTime.Now, 0.05);
            double resultNegInflation = _service.CalculateInflationAdjustmentFactor(DateTime.Now.AddYears(-1), DateTime.Now, -0.05);
            double resultZeroInflation = _service.CalculateInflationAdjustmentFactor(DateTime.Now.AddYears(-1), DateTime.Now, 0.0);

            Assert.AreEqual(1.0, resultBaseAfterCurrent);
            Assert.AreEqual(1.0, resultNegInflation);
            Assert.AreEqual(1.0, resultZeroInflation);
            Assert.IsNotNull(resultBaseAfterCurrent);
        }

        [TestMethod]
        public void IsEligibleForCommutation_InvalidInputs_ReturnsFalse()
        {
            bool resultNullPolicy = _service.IsEligibleForCommutation(null, 60);
            bool resultNegAge = _service.IsEligibleForCommutation("P1", -5);
            bool resultZeroAge = _service.IsEligibleForCommutation("P1", 0);
            bool resultHighAge = _service.IsEligibleForCommutation("P1", 150);

            Assert.IsFalse(resultNullPolicy);
            Assert.IsFalse(resultNegAge);
            Assert.IsFalse(resultZeroAge);
            Assert.IsFalse(resultHighAge);
        }

        [TestMethod]
        public void IsPolicyActive_ExtremeDates_ReturnsFalse()
        {
            bool resultMinDate = _service.IsPolicyActive("P1", DateTime.MinValue);
            bool resultMaxDate = _service.IsPolicyActive("P1", DateTime.MaxValue);
            bool resultNullPolicy = _service.IsPolicyActive(null, DateTime.Now);

            Assert.IsFalse(resultMinDate);
            Assert.IsFalse(resultMaxDate);
            Assert.IsFalse(resultNullPolicy);
            Assert.IsNotNull(resultMinDate);
        }

        [TestMethod]
        public void ValidateSpouseDateOfBirth_FutureOrMinDate_ReturnsFalse()
        {
            bool resultFuture = _service.ValidateSpouseDateOfBirth("P1", DateTime.Now.AddDays(1));
            bool resultMinDate = _service.ValidateSpouseDateOfBirth("P1", DateTime.MinValue);
            bool resultNullPolicy = _service.ValidateSpouseDateOfBirth(null, DateTime.Now.AddYears(-30));

            Assert.IsFalse(resultFuture);
            Assert.IsFalse(resultMinDate);
            Assert.IsFalse(resultNullPolicy);
            Assert.IsNotNull(resultFuture);
        }

        [TestMethod]
        public void IsJointLifeApplicable_NullOrEmptyCode_ReturnsFalse()
        {
            bool resultNull = _service.IsJointLifeApplicable(null);
            bool resultEmpty = _service.IsJointLifeApplicable(string.Empty);
            bool resultWhitespace = _service.IsJointLifeApplicable("   ");

            Assert.IsFalse(resultNull);
            Assert.IsFalse(resultEmpty);
            Assert.IsFalse(resultWhitespace);
            Assert.IsNotNull(resultNull);
        }

        [TestMethod]
        public void HasGuaranteedPeriodExpired_ExtremeDates_ReturnsExpected()
        {
            bool resultMinDate = _service.HasGuaranteedPeriodExpired("P1", DateTime.MinValue);
            bool resultMaxDate = _service.HasGuaranteedPeriodExpired("P1", DateTime.MaxValue);
            bool resultNullPolicy = _service.HasGuaranteedPeriodExpired(null, DateTime.Now);

            Assert.IsFalse(resultMinDate); // Min date means it hasn't expired
            Assert.IsTrue(resultMaxDate);  // Max date means it definitely expired
            Assert.IsFalse(resultNullPolicy);
            Assert.IsNotNull(resultMinDate);
        }

        [TestMethod]
        public void CanDeferPayout_InvalidMonths_ReturnsFalse()
        {
            bool resultNegMonths = _service.CanDeferPayout("P1", -1);
            bool resultZeroMonths = _service.CanDeferPayout("P1", 0);
            bool resultHighMonths = _service.CanDeferPayout("P1", 1200); // 100 years
            bool resultNullPolicy = _service.CanDeferPayout(null, 12);

            Assert.IsFalse(resultNegMonths);
            Assert.IsFalse(resultZeroMonths);
            Assert.IsFalse(resultHighMonths);
            Assert.IsFalse(resultNullPolicy);
        }

        [TestMethod]
        public void IsMinimumCorpusMet_NegativeCorpusOrNullProduct_ReturnsFalse()
        {
            bool resultNegCorpus = _service.IsMinimumCorpusMet(-1000m, "PROD1");
            bool resultZeroCorpus = _service.IsMinimumCorpusMet(0m, "PROD1");
            bool resultNullProduct = _service.IsMinimumCorpusMet(10000m, null);

            Assert.IsFalse(resultNegCorpus);
            Assert.IsFalse(resultZeroCorpus);
            Assert.IsFalse(resultNullProduct);
            Assert.IsNotNull(resultNegCorpus);
        }

        [TestMethod]
        public void CalculateAgeAtVesting_DobAfterVesting_ReturnsZero()
        {
            int resultDobAfter = _service.CalculateAgeAtVesting(DateTime.Now.AddDays(1), DateTime.Now);
            int resultSameDay = _service.CalculateAgeAtVesting(DateTime.Now, DateTime.Now);
            int resultExtreme = _service.CalculateAgeAtVesting(DateTime.MaxValue, DateTime.MinValue);

            Assert.AreEqual(0, resultDobAfter);
            Assert.AreEqual(0, resultSameDay);
            Assert.AreEqual(0, resultExtreme);
            Assert.IsNotNull(resultDobAfter);
        }

        [TestMethod]
        public void GetRemainingGuaranteedMonths_InvalidInputs_ReturnsZero()
        {
            int resultNegYears = _service.GetRemainingGuaranteedMonths("P1", -5, 10);
            int resultNegPayouts = _service.GetRemainingGuaranteedMonths("P1", 10, -5);
            int resultPayoutsExceed = _service.GetRemainingGuaranteedMonths("P1", 5, 100);

            Assert.AreEqual(0, resultNegYears);
            Assert.AreEqual(120, resultNegPayouts); // 10 years * 12 = 120, ignore negative payouts
            Assert.AreEqual(0, resultPayoutsExceed);
            Assert.IsNotNull(resultNegYears);
        }

        [TestMethod]
        public void GetDefermentPeriodMonths_VestingAfterStart_ReturnsZero()
        {
            int resultAfter = _service.GetDefermentPeriodMonths(DateTime.Now.AddDays(1), DateTime.Now);
            int resultSame = _service.GetDefermentPeriodMonths(DateTime.Now, DateTime.Now);
            int resultExtreme = _service.GetDefermentPeriodMonths(DateTime.MaxValue, DateTime.MinValue);

            Assert.AreEqual(0, resultAfter);
            Assert.AreEqual(0, resultSame);
            Assert.AreEqual(0, resultExtreme);
            Assert.IsNotNull(resultAfter);
        }

        [TestMethod]
        public void CalculateDaysToNextPayout_InvalidFrequency_ReturnsZero()
        {
            int resultNullFreq = _service.CalculateDaysToNextPayout(DateTime.Now, null);
            int resultEmptyFreq = _service.CalculateDaysToNextPayout(DateTime.Now, string.Empty);
            int resultInvalidFreq = _service.CalculateDaysToNextPayout(DateTime.Now, "INVALID");
            int resultMinDate = _service.CalculateDaysToNextPayout(DateTime.MinValue, "M");

            Assert.AreEqual(0, resultNullFreq);
            Assert.AreEqual(0, resultEmptyFreq);
            Assert.AreEqual(0, resultInvalidFreq);
            Assert.IsTrue(resultMinDate >= 0);
        }

        [TestMethod]
        public void GetTaxSlabCode_NegativeIncomeOrAge_ReturnsDefault()
        {
            string resultNegIncome = _service.GetTaxSlabCode(-50000m, 30);
            string resultNegAge = _service.GetTaxSlabCode(50000m, -5);
            string resultZeroIncome = _service.GetTaxSlabCode(0m, 30);

            Assert.IsNotNull(resultNegIncome);
            Assert.IsNotNull(resultNegAge);
            Assert.IsNotNull(resultZeroIncome);
            Assert.AreEqual("EXEMPT", resultNegIncome); // Assuming default fallback
        }

        [TestMethod]
        public void CalculateDeathBenefit_NegativeCorpus_ReturnsZero()
        {
            decimal resultNegCorpus = _service.CalculateDeathBenefit("P1", -1000m, DateTime.Now);
            decimal resultNullPolicy = _service.CalculateDeathBenefit(null, 10000m, DateTime.Now);
            decimal resultMinDate = _service.CalculateDeathBenefit("P1", 10000m, DateTime.MinValue);

            Assert.AreEqual(0m, resultNegCorpus);
            Assert.AreEqual(0m, resultNullPolicy);
            Assert.IsTrue(resultMinDate >= 0m);
            Assert.IsNotNull(resultNegCorpus);
        }

        [TestMethod]
        public void ComputeTaxablePortion_InvalidInputs_ReturnsZero()
        {
            decimal resultNegPayout = _service.ComputeTaxablePortion(-5000m, 0.2);
            decimal resultNegTax = _service.ComputeTaxablePortion(5000m, -0.2);
            decimal resultOver100Tax = _service.ComputeTaxablePortion(5000m, 1.5);

            Assert.AreEqual(0m, resultNegPayout);
            Assert.AreEqual(0m, resultNegTax);
            Assert.AreEqual(5000m, resultOver100Tax); // Capped at 100%
            Assert.IsNotNull(resultNegPayout);
        }

        [TestMethod]
        public void CalculateLumpSumPayout_NegativeCorpus_ReturnsZero()
        {
            decimal resultNegCorpus = _service.CalculateLumpSumPayout("P1", -10000m, true);
            decimal resultZeroCorpus = _service.CalculateLumpSumPayout("P1", 0m, false);
            decimal resultNullPolicy = _service.CalculateLumpSumPayout(null, 10000m, true);

            Assert.AreEqual(0m, resultNegCorpus);
            Assert.AreEqual(0m, resultZeroCorpus);
            Assert.AreEqual(0m, resultNullPolicy);
            Assert.IsNotNull(resultNegCorpus);
        }
    }
}