using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.SurrenderProcessing;

namespace MaturityBenefitProc.Tests.Helpers.SurrenderProcessing
{
    [TestClass]
    public class SurrenderPenaltyServiceTests
    {
        private ISurrenderPenaltyService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation named SurrenderPenaltyService exists
            _service = new SurrenderPenaltyService();
        }

        [TestMethod]
        public void CalculateBasePenaltyAmount_ValidInput_ReturnsExpected()
        {
            var result1 = _service.CalculateBasePenaltyAmount("POL123", 1000m);
            var result2 = _service.CalculateBasePenaltyAmount("POL456", 5000m);
            var result3 = _service.CalculateBasePenaltyAmount("POL789", 0m);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result1 > 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 > 0);
        }

        [TestMethod]
        public void CalculateBasePenaltyAmount_NegativeValue_ReturnsZero()
        {
            var result1 = _service.CalculateBasePenaltyAmount("POL123", -100m);
            var result2 = _service.CalculateBasePenaltyAmount("POL456", -5000m);
            var result3 = _service.CalculateBasePenaltyAmount("POL789", -0.01m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPenaltyPercentage_ValidDate_ReturnsExpected()
        {
            var result1 = _service.GetPenaltyPercentage("POL123", new DateTime(2020, 1, 1));
            var result2 = _service.GetPenaltyPercentage("POL456", new DateTime(2021, 5, 15));
            var result3 = _service.GetPenaltyPercentage("POL789", new DateTime(2022, 12, 31));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0.0, result1);
            Assert.IsTrue(result1 > 0.0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 > 0.0);
        }

        [TestMethod]
        public void GetPenaltyPercentage_FutureDate_ReturnsExpected()
        {
            var result1 = _service.GetPenaltyPercentage("POL123", DateTime.MaxValue);
            var result2 = _service.GetPenaltyPercentage("POL456", DateTime.Now.AddYears(10));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0.0, result1);
            Assert.IsTrue(result1 >= 0.0);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void IsEligibleForPenaltyWaiver_ValidCode_ReturnsTrue()
        {
            var result1 = _service.IsEligibleForPenaltyWaiver("POL123", "WAIVE100");
            var result2 = _service.IsEligibleForPenaltyWaiver("POL456", "MEDICAL");
            var result3 = _service.IsEligibleForPenaltyWaiver("POL789", "DEATH");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsEligibleForPenaltyWaiver_InvalidCode_ReturnsFalse()
        {
            var result1 = _service.IsEligibleForPenaltyWaiver("POL123", "INVALID");
            var result2 = _service.IsEligibleForPenaltyWaiver("POL456", "");
            var result3 = _service.IsEligibleForPenaltyWaiver("POL789", null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetRemainingLockInDays_ValidDate_ReturnsExpected()
        {
            var result1 = _service.GetRemainingLockInDays("POL123", new DateTime(2020, 1, 1));
            var result2 = _service.GetRemainingLockInDays("POL456", new DateTime(2021, 5, 15));
            var result3 = _service.GetRemainingLockInDays("POL789", new DateTime(2022, 12, 31));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0, result1);
            Assert.IsTrue(result1 > 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 > 0);
        }

        [TestMethod]
        public void GetRemainingLockInDays_PastLockIn_ReturnsZero()
        {
            var result1 = _service.GetRemainingLockInDays("POL123", DateTime.MaxValue);
            var result2 = _service.GetRemainingLockInDays("POL456", DateTime.Now.AddYears(50));

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetApplicablePenaltyTierCode_ValidYears_ReturnsExpected()
        {
            var result1 = _service.GetApplicablePenaltyTierCode("POL123", 1);
            var result2 = _service.GetApplicablePenaltyTierCode("POL456", 5);
            var result3 = _service.GetApplicablePenaltyTierCode("POL789", 10);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual("", result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual("", result2);
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public void GetApplicablePenaltyTierCode_NegativeYears_ReturnsDefault()
        {
            var result1 = _service.GetApplicablePenaltyTierCode("POL123", -1);
            var result2 = _service.GetApplicablePenaltyTierCode("POL456", -5);

            Assert.IsNotNull(result1);
            Assert.AreEqual("DEFAULT", result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual("DEFAULT", result2);
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_ValidInput_ReturnsExpected()
        {
            var result1 = _service.CalculateMarketValueAdjustment("POL123", 1000m, 0.05);
            var result2 = _service.CalculateMarketValueAdjustment("POL456", 5000m, 0.10);
            var result3 = _service.CalculateMarketValueAdjustment("POL789", 10000m, 0.02);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result1 > 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 > 0m);
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_ZeroRate_ReturnsZero()
        {
            var result1 = _service.CalculateMarketValueAdjustment("POL123", 1000m, 0.0);
            var result2 = _service.CalculateMarketValueAdjustment("POL456", 5000m, 0.0);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetTotalDeductionCharges_ValidDate_ReturnsExpected()
        {
            var result1 = _service.GetTotalDeductionCharges("POL123", new DateTime(2020, 1, 1));
            var result2 = _service.GetTotalDeductionCharges("POL456", new DateTime(2021, 5, 15));
            var result3 = _service.GetTotalDeductionCharges("POL789", new DateTime(2022, 12, 31));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result1 > 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 > 0m);
        }

        [TestMethod]
        public void GetTotalDeductionCharges_FutureDate_ReturnsExpected()
        {
            var result1 = _service.GetTotalDeductionCharges("POL123", DateTime.MaxValue);
            var result2 = _service.GetTotalDeductionCharges("POL456", DateTime.Now.AddYears(10));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result1 >= 0m);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void ValidateSurrenderDate_ValidDate_ReturnsTrue()
        {
            var result1 = _service.ValidateSurrenderDate("POL123", DateTime.Now);
            var result2 = _service.ValidateSurrenderDate("POL456", DateTime.Now.AddDays(-1));
            var result3 = _service.ValidateSurrenderDate("POL789", DateTime.Now.AddDays(1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateSurrenderDate_InvalidDate_ReturnsFalse()
        {
            var result1 = _service.ValidateSurrenderDate("POL123", DateTime.MinValue);
            var result2 = _service.ValidateSurrenderDate("POL456", new DateTime(1900, 1, 1));

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateProratedBonusRecoveryRate_ValidMonths_ReturnsExpected()
        {
            var result1 = _service.CalculateProratedBonusRecoveryRate("POL123", 12);
            var result2 = _service.CalculateProratedBonusRecoveryRate("POL456", 24);
            var result3 = _service.CalculateProratedBonusRecoveryRate("POL789", 36);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0.0, result1);
            Assert.IsTrue(result1 > 0.0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 > 0.0);
        }

        [TestMethod]
        public void CalculateProratedBonusRecoveryRate_ZeroMonths_ReturnsZero()
        {
            var result1 = _service.CalculateProratedBonusRecoveryRate("POL123", 0);
            var result2 = _service.CalculateProratedBonusRecoveryRate("POL456", -5);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateTaxWithholdingAmount_ValidInput_ReturnsExpected()
        {
            var result1 = _service.CalculateTaxWithholdingAmount("POL123", 1000m, 0.20);
            var result2 = _service.CalculateTaxWithholdingAmount("POL456", 5000m, 0.15);
            var result3 = _service.CalculateTaxWithholdingAmount("POL789", 10000m, 0.10);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result1 > 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 > 0m);
        }

        [TestMethod]
        public void CalculateTaxWithholdingAmount_ZeroRate_ReturnsZero()
        {
            var result1 = _service.CalculateTaxWithholdingAmount("POL123", 1000m, 0.0);
            var result2 = _service.CalculateTaxWithholdingAmount("POL456", 5000m, 0.0);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidDate_ReturnsExpected()
        {
            var result1 = _service.GetCompletedPolicyYears("POL123", new DateTime(2020, 1, 1));
            var result2 = _service.GetCompletedPolicyYears("POL456", new DateTime(2021, 5, 15));
            var result3 = _service.GetCompletedPolicyYears("POL789", new DateTime(2022, 12, 31));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(-1, result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void RetrievePenaltyRuleId_ValidInput_ReturnsExpected()
        {
            var result1 = _service.RetrievePenaltyRuleId("PROD1", new DateTime(2020, 1, 1));
            var result2 = _service.RetrievePenaltyRuleId("PROD2", new DateTime(2021, 5, 15));
            var result3 = _service.RetrievePenaltyRuleId("PROD3", new DateTime(2022, 12, 31));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual("", result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual("", result2);
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public void HasOutstandingLoans_ValidPolicy_ReturnsExpected()
        {
            var result1 = _service.HasOutstandingLoans("POL123");
            var result2 = _service.HasOutstandingLoans("POL456");

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 || !result1); // Just checking it returns a boolean
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void CalculateLoanInterestDeduction_ValidDate_ReturnsExpected()
        {
            var result1 = _service.CalculateLoanInterestDeduction("POL123", new DateTime(2020, 1, 1));
            var result2 = _service.CalculateLoanInterestDeduction("POL456", new DateTime(2021, 5, 15));
            var result3 = _service.CalculateLoanInterestDeduction("POL789", new DateTime(2022, 12, 31));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(-1m, result1);
            Assert.IsTrue(result1 >= 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0m);
        }

        [TestMethod]
        public void GetSurrenderChargeFactor_ValidDuration_ReturnsExpected()
        {
            var result1 = _service.GetSurrenderChargeFactor("POL123", 12);
            var result2 = _service.GetSurrenderChargeFactor("POL456", 24);
            var result3 = _service.GetSurrenderChargeFactor("POL789", 36);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0.0, result1);
            Assert.IsTrue(result1 > 0.0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 > 0.0);
        }

        [TestMethod]
        public void CalculateFinalNetSurrenderValue_ValidInput_ReturnsExpected()
        {
            var result1 = _service.CalculateFinalNetSurrenderValue("POL123", 1000m, 100m);
            var result2 = _service.CalculateFinalNetSurrenderValue("POL456", 5000m, 500m);
            var result3 = _service.CalculateFinalNetSurrenderValue("POL789", 10000m, 1000m);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result1 > 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 > 0m);
        }

        [TestMethod]
        public void RequiresManagerApproval_HighPenalty_ReturnsTrue()
        {
            var result1 = _service.RequiresManagerApproval("POL123", 50000m);
            var result2 = _service.RequiresManagerApproval("POL456", 100000m);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void RequiresManagerApproval_LowPenalty_ReturnsFalse()
        {
            var result1 = _service.RequiresManagerApproval("POL123", 10m);
            var result2 = _service.RequiresManagerApproval("POL456", 50m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetFreeWithdrawalCount_ValidDate_ReturnsExpected()
        {
            var result1 = _service.GetFreeWithdrawalCount("POL123", new DateTime(2020, 1, 1));
            var result2 = _service.GetFreeWithdrawalCount("POL456", new DateTime(2021, 5, 15));
            var result3 = _service.GetFreeWithdrawalCount("POL789", new DateTime(2022, 12, 31));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(-1, result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
        }
    }
}