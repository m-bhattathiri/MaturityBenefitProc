using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.AnnuityProcessing;

namespace MaturityBenefitProc.Tests.Helpers.AnnuityProcessing
{
    [TestClass]
    public class AnnuityCalculationServiceTests
    {
        private IAnnuityCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming AnnuityCalculationService is the implementation of IAnnuityCalculationService
            // For the sake of this test file generation, we instantiate the implementation.
            // If it requires dependencies, mock them. Here we assume a parameterless constructor or simple instantiation.
            // Note: The prompt specifies creating a new AnnuityCalculationService().
            _service = new AnnuityCalculationService();
        }

        [TestMethod]
        public void CalculateMonthlyPayout_ValidInputs_ReturnsExpectedAmount()
        {
            decimal result1 = _service.CalculateMonthlyPayout("POL123", 100000m, 0.06);
            decimal result2 = _service.CalculateMonthlyPayout("POL124", 200000m, 0.05);
            decimal result3 = _service.CalculateMonthlyPayout("POL125", 50000m, 0.08);
            decimal result4 = _service.CalculateMonthlyPayout("POL126", 0m, 0.06);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateAnnualPayout_ValidInputs_ReturnsExpectedAmount()
        {
            decimal result1 = _service.CalculateAnnualPayout("POL123", 100000m, 0.06);
            decimal result2 = _service.CalculateAnnualPayout("POL124", 200000m, 0.05);
            decimal result3 = _service.CalculateAnnualPayout("POL125", 50000m, 0.08);
            decimal result4 = _service.CalculateAnnualPayout("POL126", 0m, 0.06);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateQuarterlyPayout_ValidInputs_ReturnsExpectedAmount()
        {
            decimal result1 = _service.CalculateQuarterlyPayout("POL123", 100000m, 0.06);
            decimal result2 = _service.CalculateQuarterlyPayout("POL124", 200000m, 0.05);
            decimal result3 = _service.CalculateQuarterlyPayout("POL125", 50000m, 0.08);
            decimal result4 = _service.CalculateQuarterlyPayout("POL126", 0m, 0.06);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateSemiAnnualPayout_ValidInputs_ReturnsExpectedAmount()
        {
            decimal result1 = _service.CalculateSemiAnnualPayout("POL123", 100000m, 0.06);
            decimal result2 = _service.CalculateSemiAnnualPayout("POL124", 200000m, 0.05);
            decimal result3 = _service.CalculateSemiAnnualPayout("POL125", 50000m, 0.08);
            decimal result4 = _service.CalculateSemiAnnualPayout("POL126", 0m, 0.06);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetTotalAccumulatedCorpus_ValidInputs_ReturnsExpectedAmount()
        {
            decimal result1 = _service.GetTotalAccumulatedCorpus("POL123", new DateTime(2025, 1, 1));
            decimal result2 = _service.GetTotalAccumulatedCorpus("POL124", new DateTime(2026, 1, 1));
            decimal result3 = _service.GetTotalAccumulatedCorpus("POL125", new DateTime(2024, 1, 1));
            decimal result4 = _service.GetTotalAccumulatedCorpus("INVALID", new DateTime(2025, 1, 1));

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateCommutationAmount_ValidInputs_ReturnsExpectedAmount()
        {
            decimal result1 = _service.CalculateCommutationAmount("POL123", 100000m, 0.33);
            decimal result2 = _service.CalculateCommutationAmount("POL124", 200000m, 0.25);
            decimal result3 = _service.CalculateCommutationAmount("POL125", 50000m, 0.50);
            decimal result4 = _service.CalculateCommutationAmount("POL126", 100000m, 0.0);

            Assert.AreEqual(33000m, result1);
            Assert.AreEqual(50000m, result2);
            Assert.AreEqual(25000m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateResidualCorpus_ValidInputs_ReturnsExpectedAmount()
        {
            decimal result1 = _service.CalculateResidualCorpus(100000m, 33000m);
            decimal result2 = _service.CalculateResidualCorpus(200000m, 50000m);
            decimal result3 = _service.CalculateResidualCorpus(50000m, 25000m);
            decimal result4 = _service.CalculateResidualCorpus(100000m, 0m);

            Assert.AreEqual(67000m, result1);
            Assert.AreEqual(150000m, result2);
            Assert.AreEqual(25000m, result3);
            Assert.AreEqual(100000m, result4);
        }

        [TestMethod]
        public void GetAnnuityFactor_ValidInputs_ReturnsExpectedFactor()
        {
            double result1 = _service.GetAnnuityFactor(60, "OPT1", 0.06);
            double result2 = _service.GetAnnuityFactor(55, "OPT2", 0.05);
            double result3 = _service.GetAnnuityFactor(65, "OPT3", 0.07);
            double result4 = _service.GetAnnuityFactor(70, "OPT1", 0.06);

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreNotEqual(0.0, result4);
        }

        [TestMethod]
        public void GetCurrentInterestRate_ValidInputs_ReturnsExpectedRate()
        {
            double result1 = _service.GetCurrentInterestRate("PROD1", new DateTime(2023, 1, 1));
            double result2 = _service.GetCurrentInterestRate("PROD2", new DateTime(2023, 6, 1));
            double result3 = _service.GetCurrentInterestRate("PROD3", new DateTime(2024, 1, 1));
            double result4 = _service.GetCurrentInterestRate("INVALID", new DateTime(2023, 1, 1));

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void CalculateInternalRateOfReturn_ValidInputs_ReturnsExpectedRate()
        {
            double result1 = _service.CalculateInternalRateOfReturn("POL123", 500000m, 800000m);
            double result2 = _service.CalculateInternalRateOfReturn("POL124", 200000m, 300000m);
            double result3 = _service.CalculateInternalRateOfReturn("POL125", 100000m, 150000m);
            double result4 = _service.CalculateInternalRateOfReturn("POL126", 500000m, 500000m);

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void ComputeMortalityChargeRate_ValidInputs_ReturnsExpectedRate()
        {
            double result1 = _service.ComputeMortalityChargeRate(45, "M");
            double result2 = _service.ComputeMortalityChargeRate(45, "F");
            double result3 = _service.ComputeMortalityChargeRate(60, "M");
            double result4 = _service.ComputeMortalityChargeRate(60, "F");

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreNotEqual(0.0, result4);
        }

        [TestMethod]
        public void CalculateInflationAdjustmentFactor_ValidInputs_ReturnsExpectedFactor()
        {
            double result1 = _service.CalculateInflationAdjustmentFactor(new DateTime(2020, 1, 1), new DateTime(2023, 1, 1), 0.05);
            double result2 = _service.CalculateInflationAdjustmentFactor(new DateTime(2015, 1, 1), new DateTime(2023, 1, 1), 0.04);
            double result3 = _service.CalculateInflationAdjustmentFactor(new DateTime(2022, 1, 1), new DateTime(2023, 1, 1), 0.06);
            double result4 = _service.CalculateInflationAdjustmentFactor(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), 0.05);

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreEqual(1.0, result4);
        }

        [TestMethod]
        public void IsEligibleForCommutation_ValidInputs_ReturnsExpectedResult()
        {
            bool result1 = _service.IsEligibleForCommutation("POL123", 60);
            bool result2 = _service.IsEligibleForCommutation("POL124", 55);
            bool result3 = _service.IsEligibleForCommutation("POL125", 40);
            bool result4 = _service.IsEligibleForCommutation("INVALID", 60);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsPolicyActive_ValidInputs_ReturnsExpectedResult()
        {
            bool result1 = _service.IsPolicyActive("POL123", new DateTime(2023, 1, 1));
            bool result2 = _service.IsPolicyActive("POL124", new DateTime(2023, 1, 1));
            bool result3 = _service.IsPolicyActive("POL125", new DateTime(2023, 1, 1));
            bool result4 = _service.IsPolicyActive("INVALID", new DateTime(2023, 1, 1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void ValidateSpouseDateOfBirth_ValidInputs_ReturnsExpectedResult()
        {
            bool result1 = _service.ValidateSpouseDateOfBirth("POL123", new DateTime(1980, 1, 1));
            bool result2 = _service.ValidateSpouseDateOfBirth("POL124", new DateTime(1975, 1, 1));
            bool result3 = _service.ValidateSpouseDateOfBirth("POL125", new DateTime(2020, 1, 1));
            bool result4 = _service.ValidateSpouseDateOfBirth("INVALID", new DateTime(1980, 1, 1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsJointLifeApplicable_ValidInputs_ReturnsExpectedResult()
        {
            bool result1 = _service.IsJointLifeApplicable("JL1");
            bool result2 = _service.IsJointLifeApplicable("JL2");
            bool result3 = _service.IsJointLifeApplicable("SL1");
            bool result4 = _service.IsJointLifeApplicable("INVALID");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void HasGuaranteedPeriodExpired_ValidInputs_ReturnsExpectedResult()
        {
            bool result1 = _service.HasGuaranteedPeriodExpired("POL123", new DateTime(2030, 1, 1));
            bool result2 = _service.HasGuaranteedPeriodExpired("POL124", new DateTime(2035, 1, 1));
            bool result3 = _service.HasGuaranteedPeriodExpired("POL125", new DateTime(2020, 1, 1));
            bool result4 = _service.HasGuaranteedPeriodExpired("INVALID", new DateTime(2030, 1, 1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void CanDeferPayout_ValidInputs_ReturnsExpectedResult()
        {
            bool result1 = _service.CanDeferPayout("POL123", 12);
            bool result2 = _service.CanDeferPayout("POL124", 24);
            bool result3 = _service.CanDeferPayout("POL125", 120);
            bool result4 = _service.CanDeferPayout("INVALID", 12);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsMinimumCorpusMet_ValidInputs_ReturnsExpectedResult()
        {
            bool result1 = _service.IsMinimumCorpusMet(100000m, "PROD1");
            bool result2 = _service.IsMinimumCorpusMet(200000m, "PROD2");
            bool result3 = _service.IsMinimumCorpusMet(1000m, "PROD1");
            bool result4 = _service.IsMinimumCorpusMet(0m, "PROD1");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void CalculateAgeAtVesting_ValidInputs_ReturnsExpectedAge()
        {
            int result1 = _service.CalculateAgeAtVesting(new DateTime(1960, 1, 1), new DateTime(2020, 1, 1));
            int result2 = _service.CalculateAgeAtVesting(new DateTime(1970, 6, 15), new DateTime(2025, 6, 15));
            int result3 = _service.CalculateAgeAtVesting(new DateTime(1980, 12, 31), new DateTime(2030, 12, 31));
            int result4 = _service.CalculateAgeAtVesting(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1));

            Assert.AreEqual(60, result1);
            Assert.AreEqual(55, result2);
            Assert.AreEqual(50, result3);
            Assert.AreEqual(20, result4);
        }

        [TestMethod]
        public void GetRemainingGuaranteedMonths_ValidInputs_ReturnsExpectedMonths()
        {
            int result1 = _service.GetRemainingGuaranteedMonths("POL123", 10, 24);
            int result2 = _service.GetRemainingGuaranteedMonths("POL124", 15, 60);
            int result3 = _service.GetRemainingGuaranteedMonths("POL125", 5, 60);
            int result4 = _service.GetRemainingGuaranteedMonths("POL126", 10, 120);

            Assert.AreEqual(96, result1);
            Assert.AreEqual(120, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetDefermentPeriodMonths_ValidInputs_ReturnsExpectedMonths()
        {
            int result1 = _service.GetDefermentPeriodMonths(new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            int result2 = _service.GetDefermentPeriodMonths(new DateTime(2020, 1, 1), new DateTime(2022, 1, 1));
            int result3 = _service.GetDefermentPeriodMonths(new DateTime(2020, 1, 1), new DateTime(2020, 7, 1));
            int result4 = _service.GetDefermentPeriodMonths(new DateTime(2020, 1, 1), new DateTime(2020, 1, 1));

            Assert.AreEqual(12, result1);
            Assert.AreEqual(24, result2);
            Assert.AreEqual(6, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetTotalPayoutsMade_ValidInputs_ReturnsExpectedCount()
        {
            int result1 = _service.GetTotalPayoutsMade("POL123");
            int result2 = _service.GetTotalPayoutsMade("POL124");
            int result3 = _service.GetTotalPayoutsMade("POL125");
            int result4 = _service.GetTotalPayoutsMade("INVALID");

            Assert.AreNotEqual(-1, result1);
            Assert.AreNotEqual(-1, result2);
            Assert.AreNotEqual(-1, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void CalculateDaysToNextPayout_ValidInputs_ReturnsExpectedDays()
        {
            int result1 = _service.CalculateDaysToNextPayout(new DateTime(2023, 1, 1), "M");
            int result2 = _service.CalculateDaysToNextPayout(new DateTime(2023, 1, 1), "Q");
            int result3 = _service.CalculateDaysToNextPayout(new DateTime(2023, 1, 1), "H");
            int result4 = _service.CalculateDaysToNextPayout(new DateTime(2023, 1, 1), "A");

            Assert.IsTrue(result1 >= 28 && result1 <= 31);
            Assert.IsTrue(result2 >= 89 && result2 <= 92);
            Assert.IsTrue(result3 >= 181 && result3 <= 184);
            Assert.IsTrue(result4 >= 365 && result4 <= 366);
        }

        [TestMethod]
        public void GetPremiumPaymentTerm_ValidInputs_ReturnsExpectedTerm()
        {
            int result1 = _service.GetPremiumPaymentTerm("POL123");
            int result2 = _service.GetPremiumPaymentTerm("POL124");
            int result3 = _service.GetPremiumPaymentTerm("POL125");
            int result4 = _service.GetPremiumPaymentTerm("INVALID");

            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
            Assert.AreNotEqual(0, result3);
            Assert.AreEqual(0, result4);
        }
    }
}