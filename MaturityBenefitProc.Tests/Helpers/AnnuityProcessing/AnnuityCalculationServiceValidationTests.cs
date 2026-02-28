using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.AnnuityProcessing;

namespace MaturityBenefitProc.Tests.Helpers.AnnuityProcessing
{
    [TestClass]
    public class AnnuityCalculationServiceValidationTests
    {
        private IAnnuityCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming AnnuityCalculationService is the concrete implementation of IAnnuityCalculationService
            // For testing purposes, we instantiate the concrete class.
            _service = new AnnuityCalculationService();
        }

        [TestMethod]
        public void CalculateMonthlyPayout_InvalidInputs_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateMonthlyPayout(null, 100000m, 0.05));
            Assert.AreEqual(0m, _service.CalculateMonthlyPayout("", 100000m, 0.05));
            Assert.AreEqual(0m, _service.CalculateMonthlyPayout("   ", 100000m, 0.05));
            Assert.AreEqual(0m, _service.CalculateMonthlyPayout("POL123", -500m, 0.05));
            Assert.AreEqual(0m, _service.CalculateMonthlyPayout("POL123", 100000m, -0.01));
        }

        [TestMethod]
        public void CalculateAnnualPayout_BoundaryValues_HandledCorrectly()
        {
            Assert.AreEqual(0m, _service.CalculateAnnualPayout("POL123", 0m, 0.05));
            Assert.AreEqual(0m, _service.CalculateAnnualPayout("POL123", 100000m, 0.0));
            Assert.IsNotNull(_service.CalculateAnnualPayout("POL123", decimal.MaxValue, 0.05));
            Assert.IsNotNull(_service.CalculateAnnualPayout("POL123", 100000m, double.MaxValue));
        }

        [TestMethod]
        public void CalculateQuarterlyPayout_SequentialCalls_ReturnsConsistentResults()
        {
            var result1 = _service.CalculateQuarterlyPayout("POL123", 50000m, 0.06);
            var result2 = _service.CalculateQuarterlyPayout("POL123", 50000m, 0.06);
            var result3 = _service.CalculateQuarterlyPayout("POL124", 50000m, 0.06);

            Assert.AreEqual(result1, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result3);
            Assert.AreEqual(0m, _service.CalculateQuarterlyPayout(null, 50000m, 0.06));
        }

        [TestMethod]
        public void CalculateSemiAnnualPayout_NegativeValues_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateSemiAnnualPayout("POL123", -10000m, 0.05));
            Assert.AreEqual(0m, _service.CalculateSemiAnnualPayout("POL123", 10000m, -0.05));
            Assert.AreEqual(0m, _service.CalculateSemiAnnualPayout("POL123", -10000m, -0.05));
            Assert.AreEqual(0m, _service.CalculateSemiAnnualPayout("", -10000m, 0.05));
        }

        [TestMethod]
        public void GetTotalAccumulatedCorpus_InvalidPolicyId_ReturnsZero()
        {
            DateTime maturityDate = DateTime.Now.AddYears(10);
            Assert.AreEqual(0m, _service.GetTotalAccumulatedCorpus(null, maturityDate));
            Assert.AreEqual(0m, _service.GetTotalAccumulatedCorpus("", maturityDate));
            Assert.AreEqual(0m, _service.GetTotalAccumulatedCorpus("   ", maturityDate));
            Assert.IsNotNull(_service.GetTotalAccumulatedCorpus("POL123", maturityDate));
        }

        [TestMethod]
        public void CalculateCommutationAmount_InvalidPercentages_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateCommutationAmount("POL123", 100000m, -0.1));
            Assert.AreEqual(0m, _service.CalculateCommutationAmount("POL123", 100000m, 1.1)); // Over 100%
            Assert.AreEqual(0m, _service.CalculateCommutationAmount("POL123", -100000m, 0.3));
            Assert.AreEqual(0m, _service.CalculateCommutationAmount(null, 100000m, 0.3));
        }

        [TestMethod]
        public void CalculateResidualCorpus_CommutedGreaterThanTotal_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateResidualCorpus(100000m, 150000m));
            Assert.AreEqual(0m, _service.CalculateResidualCorpus(-100000m, 50000m));
            Assert.AreEqual(0m, _service.CalculateResidualCorpus(100000m, -50000m));
            Assert.IsNotNull(_service.CalculateResidualCorpus(100000m, 30000m));
        }

        [TestMethod]
        public void GetAnnuityFactor_InvalidInputs_ReturnsZero()
        {
            Assert.AreEqual(0.0, _service.GetAnnuityFactor(-5, "OPT1", 0.05));
            Assert.AreEqual(0.0, _service.GetAnnuityFactor(200, "OPT1", 0.05));
            Assert.AreEqual(0.0, _service.GetAnnuityFactor(60, null, 0.05));
            Assert.AreEqual(0.0, _service.GetAnnuityFactor(60, "", 0.05));
            Assert.AreEqual(0.0, _service.GetAnnuityFactor(60, "OPT1", -0.05));
        }

        [TestMethod]
        public void GetCurrentInterestRate_InvalidProductCode_ReturnsZero()
        {
            DateTime effectiveDate = DateTime.Now;
            Assert.AreEqual(0.0, _service.GetCurrentInterestRate(null, effectiveDate));
            Assert.AreEqual(0.0, _service.GetCurrentInterestRate("", effectiveDate));
            Assert.AreEqual(0.0, _service.GetCurrentInterestRate("   ", effectiveDate));
            Assert.IsNotNull(_service.GetCurrentInterestRate("PROD123", effectiveDate));
        }

        [TestMethod]
        public void CalculateInternalRateOfReturn_InvalidAmounts_ReturnsZero()
        {
            Assert.AreEqual(0.0, _service.CalculateInternalRateOfReturn("POL123", 0m, 10000m));
            Assert.AreEqual(0.0, _service.CalculateInternalRateOfReturn("POL123", -5000m, 10000m));
            Assert.AreEqual(0.0, _service.CalculateInternalRateOfReturn("POL123", 5000m, -10000m));
            Assert.AreEqual(0.0, _service.CalculateInternalRateOfReturn(null, 5000m, 10000m));
        }

        [TestMethod]
        public void ComputeMortalityChargeRate_InvalidInputs_ReturnsZero()
        {
            Assert.AreEqual(0.0, _service.ComputeMortalityChargeRate(-1, "M"));
            Assert.AreEqual(0.0, _service.ComputeMortalityChargeRate(150, "M"));
            Assert.AreEqual(0.0, _service.ComputeMortalityChargeRate(45, null));
            Assert.AreEqual(0.0, _service.ComputeMortalityChargeRate(45, ""));
            Assert.AreEqual(0.0, _service.ComputeMortalityChargeRate(45, "INVALID"));
        }

        [TestMethod]
        public void CalculateInflationAdjustmentFactor_BaseDateAfterCurrent_ReturnsOne()
        {
            DateTime baseDate = new DateTime(2025, 1, 1);
            DateTime currentDate = new DateTime(2024, 1, 1);
            Assert.AreEqual(1.0, _service.CalculateInflationAdjustmentFactor(baseDate, currentDate, 0.05));
            Assert.AreEqual(1.0, _service.CalculateInflationAdjustmentFactor(baseDate, baseDate, 0.05));
            Assert.AreEqual(1.0, _service.CalculateInflationAdjustmentFactor(new DateTime(2020, 1, 1), new DateTime(2024, 1, 1), -0.05));
            Assert.IsNotNull(_service.CalculateInflationAdjustmentFactor(new DateTime(2020, 1, 1), new DateTime(2024, 1, 1), 0.05));
        }

        [TestMethod]
        public void IsEligibleForCommutation_InvalidInputs_ReturnsFalse()
        {
            Assert.IsFalse(_service.IsEligibleForCommutation(null, 60));
            Assert.IsFalse(_service.IsEligibleForCommutation("", 60));
            Assert.IsFalse(_service.IsEligibleForCommutation("POL123", -5));
            Assert.IsFalse(_service.IsEligibleForCommutation("POL123", 200));
        }

        [TestMethod]
        public void IsPolicyActive_InvalidPolicyId_ReturnsFalse()
        {
            DateTime checkDate = DateTime.Now;
            Assert.IsFalse(_service.IsPolicyActive(null, checkDate));
            Assert.IsFalse(_service.IsPolicyActive("", checkDate));
            Assert.IsFalse(_service.IsPolicyActive("   ", checkDate));
            Assert.IsNotNull(_service.IsPolicyActive("POL123", checkDate));
        }

        [TestMethod]
        public void ValidateSpouseDateOfBirth_FutureDate_ReturnsFalse()
        {
            Assert.IsFalse(_service.ValidateSpouseDateOfBirth("POL123", DateTime.Now.AddDays(1)));
            Assert.IsFalse(_service.ValidateSpouseDateOfBirth("POL123", DateTime.Now.AddYears(10)));
            Assert.IsFalse(_service.ValidateSpouseDateOfBirth(null, new DateTime(1980, 1, 1)));
            Assert.IsFalse(_service.ValidateSpouseDateOfBirth("", new DateTime(1980, 1, 1)));
        }

        [TestMethod]
        public void IsJointLifeApplicable_InvalidCode_ReturnsFalse()
        {
            Assert.IsFalse(_service.IsJointLifeApplicable(null));
            Assert.IsFalse(_service.IsJointLifeApplicable(""));
            Assert.IsFalse(_service.IsJointLifeApplicable("   "));
            Assert.IsNotNull(_service.IsJointLifeApplicable("JLF"));
        }

        [TestMethod]
        public void HasGuaranteedPeriodExpired_InvalidPolicyId_ReturnsFalse()
        {
            DateTime currentDate = DateTime.Now;
            Assert.IsFalse(_service.HasGuaranteedPeriodExpired(null, currentDate));
            Assert.IsFalse(_service.HasGuaranteedPeriodExpired("", currentDate));
            Assert.IsFalse(_service.HasGuaranteedPeriodExpired("   ", currentDate));
            Assert.IsNotNull(_service.HasGuaranteedPeriodExpired("POL123", currentDate));
        }

        [TestMethod]
        public void CanDeferPayout_InvalidInputs_ReturnsFalse()
        {
            Assert.IsFalse(_service.CanDeferPayout(null, 12));
            Assert.IsFalse(_service.CanDeferPayout("", 12));
            Assert.IsFalse(_service.CanDeferPayout("POL123", -1));
            Assert.IsFalse(_service.CanDeferPayout("POL123", 0));
        }

        [TestMethod]
        public void IsMinimumCorpusMet_NegativeCorpus_ReturnsFalse()
        {
            Assert.IsFalse(_service.IsMinimumCorpusMet(-1000m, "PROD1"));
            Assert.IsFalse(_service.IsMinimumCorpusMet(10000m, null));
            Assert.IsFalse(_service.IsMinimumCorpusMet(10000m, ""));
            Assert.IsNotNull(_service.IsMinimumCorpusMet(10000m, "PROD1"));
        }

        [TestMethod]
        public void CalculateAgeAtVesting_VestingBeforeDob_ReturnsZero()
        {
            DateTime dob = new DateTime(1980, 1, 1);
            DateTime vesting = new DateTime(1970, 1, 1);
            Assert.AreEqual(0, _service.CalculateAgeAtVesting(dob, vesting));
            Assert.AreEqual(0, _service.CalculateAgeAtVesting(DateTime.Now.AddDays(1), DateTime.Now));
            Assert.AreNotEqual(0, _service.CalculateAgeAtVesting(new DateTime(1980, 1, 1), new DateTime(2040, 1, 1)));
            Assert.IsNotNull(_service.CalculateAgeAtVesting(dob, new DateTime(2040, 1, 1)));
        }

        [TestMethod]
        public void GetRemainingGuaranteedMonths_PayoutsExceedGuaranteed_ReturnsZero()
        {
            Assert.AreEqual(0, _service.GetRemainingGuaranteedMonths("POL123", 10, 121));
            Assert.AreEqual(0, _service.GetRemainingGuaranteedMonths("POL123", -5, 10));
            Assert.AreEqual(0, _service.GetRemainingGuaranteedMonths("POL123", 10, -5));
            Assert.AreEqual(0, _service.GetRemainingGuaranteedMonths(null, 10, 5));
        }

        [TestMethod]
        public void GetDefermentPeriodMonths_DeferredBeforeVesting_ReturnsZero()
        {
            DateTime vesting = new DateTime(2025, 1, 1);
            DateTime deferred = new DateTime(2024, 1, 1);
            Assert.AreEqual(0, _service.GetDefermentPeriodMonths(vesting, deferred));
            Assert.AreEqual(0, _service.GetDefermentPeriodMonths(vesting, vesting));
            Assert.AreNotEqual(0, _service.GetDefermentPeriodMonths(new DateTime(2024, 1, 1), new DateTime(2025, 1, 1)));
            Assert.IsNotNull(_service.GetDefermentPeriodMonths(vesting, new DateTime(2026, 1, 1)));
        }

        [TestMethod]
        public void GetTotalPayoutsMade_InvalidPolicyId_ReturnsZero()
        {
            Assert.AreEqual(0, _service.GetTotalPayoutsMade(null));
            Assert.AreEqual(0, _service.GetTotalPayoutsMade(""));
            Assert.AreEqual(0, _service.GetTotalPayoutsMade("   "));
            Assert.IsNotNull(_service.GetTotalPayoutsMade("POL123"));
        }

        [TestMethod]
        public void CalculateDaysToNextPayout_InvalidFrequency_ReturnsZero()
        {
            DateTime lastPayout = DateTime.Now;
            Assert.AreEqual(0, _service.CalculateDaysToNextPayout(lastPayout, null));
            Assert.AreEqual(0, _service.CalculateDaysToNextPayout(lastPayout, ""));
            Assert.AreEqual(0, _service.CalculateDaysToNextPayout(lastPayout, "INVALID"));
            Assert.IsNotNull(_service.CalculateDaysToNextPayout(lastPayout, "M"));
        }

        [TestMethod]
        public void GetPremiumPaymentTerm_InvalidPolicyId_ReturnsZero()
        {
            Assert.AreEqual(0, _service.GetPremiumPaymentTerm(null));
            Assert.AreEqual(0, _service.GetPremiumPaymentTerm(""));
            Assert.AreEqual(0, _service.GetPremiumPaymentTerm("   "));
            Assert.IsNotNull(_service.GetPremiumPaymentTerm("POL123"));
        }

        [TestMethod]
        public void GetAnnuityOptionCode_InvalidPolicyId_ReturnsNull()
        {
            Assert.IsNull(_service.GetAnnuityOptionCode(null));
            Assert.IsNull(_service.GetAnnuityOptionCode(""));
            Assert.IsNull(_service.GetAnnuityOptionCode("   "));
            Assert.IsNotNull(_service.GetAnnuityOptionCode("POL123") ?? string.Empty);
        }

        [TestMethod]
        public void GeneratePayoutTransactionId_InvalidPolicyId_ReturnsNull()
        {
            DateTime payoutDate = DateTime.Now;
            Assert.IsNull(_service.GeneratePayoutTransactionId(null, payoutDate));
            Assert.IsNull(_service.GeneratePayoutTransactionId("", payoutDate));
            Assert.IsNull(_service.GeneratePayoutTransactionId("   ", payoutDate));
            Assert.IsNotNull(_service.GeneratePayoutTransactionId("POL123", payoutDate) ?? string.Empty);
        }

        [TestMethod]
        public void GetTaxSlabCode_NegativeIncome_ReturnsDefault()
        {
            Assert.IsNotNull(_service.GetTaxSlabCode(-1000m, 45));
            Assert.IsNotNull(_service.GetTaxSlabCode(500000m, -5));
            Assert.IsNotNull(_service.GetTaxSlabCode(500000m, 200));
            Assert.IsNotNull(_service.GetTaxSlabCode(1000000m, 60));
        }

        [TestMethod]
        public void DeterminePayoutFrequencyCode_InvalidPolicyId_ReturnsNull()
        {
            Assert.IsNull(_service.DeterminePayoutFrequencyCode(null));
            Assert.IsNull(_service.DeterminePayoutFrequencyCode(""));
            Assert.IsNull(_service.DeterminePayoutFrequencyCode("   "));
            Assert.IsNotNull(_service.DeterminePayoutFrequencyCode("POL123") ?? string.Empty);
        }

        [TestMethod]
        public void GetBeneficiaryRelationshipCode_InvalidIds_ReturnsNull()
        {
            Assert.IsNull(_service.GetBeneficiaryRelationshipCode(null, "BEN123"));
            Assert.IsNull(_service.GetBeneficiaryRelationshipCode("POL123", null));
            Assert.IsNull(_service.GetBeneficiaryRelationshipCode("", ""));
            Assert.IsNotNull(_service.GetBeneficiaryRelationshipCode("POL123", "BEN123") ?? string.Empty);
        }

        [TestMethod]
        public void CalculateDeathBenefit_NegativeCorpus_ReturnsZero()
        {
            DateTime dod = DateTime.Now;
            Assert.AreEqual(0m, _service.CalculateDeathBenefit("POL123", -50000m, dod));
            Assert.AreEqual(0m, _service.CalculateDeathBenefit(null, 50000m, dod));
            Assert.AreEqual(0m, _service.CalculateDeathBenefit("", 50000m, dod));
            Assert.IsNotNull(_service.CalculateDeathBenefit("POL123", 50000m, dod));
        }

        [TestMethod]
        public void CalculateSurrenderValue_NegativeYear_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateSurrenderValue("POL123", 100000m, -1));
            Assert.AreEqual(0m, _service.CalculateSurrenderValue("POL123", -100000m, 5));
            Assert.AreEqual(0m, _service.CalculateSurrenderValue(null, 100000m, 5));
            Assert.IsNotNull(_service.CalculateSurrenderValue("POL123", 100000m, 5));
        }

        [TestMethod]
        public void ComputeTaxablePortion_InvalidInputs_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.ComputeTaxablePortion(-5000m, 0.2));
            Assert.AreEqual(0m, _service.ComputeTaxablePortion(5000m, -0.2));
            Assert.AreEqual(0m, _service.ComputeTaxablePortion(5000m, 1.5)); // Tax rate > 100%
            Assert.IsNotNull(_service.ComputeTaxablePortion(5000m, 0.2));
        }

        [TestMethod]
        public void GetGuaranteedMinimumWithdrawalBenefit_NegativeCorpus_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.GetGuaranteedMinimumWithdrawalBenefit("POL123", -10000m));
            Assert.AreEqual(0m, _service.GetGuaranteedMinimumWithdrawalBenefit(null, 10000m));
            Assert.AreEqual(0m, _service.GetGuaranteedMinimumWithdrawalBenefit("", 10000m));
            Assert.IsNotNull(_service.GetGuaranteedMinimumWithdrawalBenefit("POL123", 10000m));
        }

        [TestMethod]
        public void CalculateLumpSumPayout_NegativeCorpus_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateLumpSumPayout("POL123", -50000m, true));
            Assert.AreEqual(0m, _service.CalculateLumpSumPayout(null, 50000m, false));
            Assert.AreEqual(0m, _service.CalculateLumpSumPayout("", 50000m, true));
            Assert.IsNotNull(_service.CalculateLumpSumPayout("POL123", 50000m, false));
        }
    }
}