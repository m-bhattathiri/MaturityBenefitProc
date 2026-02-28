using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.SurvivalBenefit;

namespace MaturityBenefitProc.Tests.Helpers.SurvivalBenefit
{
    [TestClass]
    public class SurvivalBenefitServiceValidationTests
    {
        private SurvivalBenefitService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new SurvivalBenefitService();
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_SpecialCharsInPolicy_Processes()
        {
            var result = _service.ProcessSurvivalBenefit("POL/2017-SB#001", 80000m);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.ReferenceId);
            Assert.AreEqual(80000m, result.Amount);
            Assert.IsTrue(result.InstallmentNumber >= 1);
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_LongPolicyNumber_Processes()
        {
            var longPolicy = "POL-" + new string('A', 150);
            var result = _service.ProcessSurvivalBenefit(longPolicy, 60000m);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.ReferenceId);
            Assert.AreEqual(60000m, result.Amount);
            Assert.IsTrue(result.InstallmentNumber >= 1);
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_DecimalPrecision_Maintained()
        {
            var result = _service.ProcessSurvivalBenefit("POL-PREC01", 12345.67m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(12345.67m, result.Amount);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.Amount > 12345m);
        }

        [TestMethod]
        public void ValidateSurvivalBenefit_NumericOnlyPolicy_ReturnsSuccess()
        {
            var result = _service.ValidateSurvivalBenefit("9876543210");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.Message.Contains("9876543210"));
        }

        [TestMethod]
        public void ValidateSurvivalBenefit_WhitespacePolicy_ReturnsSuccess()
        {
            var result = _service.ValidateSurvivalBenefit("   ");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.ReferenceId.Length > 0);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_SmallDecimals_CalculatesCorrectly()
        {
            var result = _service.CalculateSurvivalBenefitAmount(100.50m, 10.5m, 1);
            Assert.IsTrue(result > 0);
            Assert.AreEqual(Math.Round(100.50m * (10.5m / 100m), 2), result);
            Assert.IsTrue(result < 100.50m);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_LargeValues_CalculatesCorrectly()
        {
            var result = _service.CalculateSurvivalBenefitAmount(50000000m, 30m, 5);
            Assert.AreEqual(15000000m, result);
            Assert.IsTrue(result > 0);
            Assert.AreEqual(50000000m * 0.30m, result);
            Assert.IsTrue(result < 50000000m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_NegativeInstallment_ReturnsZero()
        {
            var result = _service.CalculateSurvivalBenefitAmount(500000m, 20m, -1);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_NegativePercentage_ReturnsZero()
        {
            var result = _service.CalculateSurvivalBenefitAmount(500000m, -20m, 1);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_SecondInstallment_Returns20()
        {
            var result = _service.GetSurvivalBenefitPercentage("SB-PLAN-001", 2);
            Assert.AreEqual(20m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result <= 100);
            Assert.AreEqual(20m, result);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_NegativeInstallment_ReturnsZero()
        {
            var result = _service.GetSurvivalBenefitPercentage("SB-PLAN-001", -1);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void IsSurvivalBenefitDue_ExactNow_ReturnsTrue()
        {
            var result = _service.IsSurvivalBenefitDue("POL-VAL01", DateTime.UtcNow);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsFalse(result == false);
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void IsSurvivalBenefitDue_FarFuture_ReturnsFalse()
        {
            var result = _service.IsSurvivalBenefitDue("POL-VAL02", DateTime.UtcNow.AddYears(10));
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsTrue(result == false);
            Assert.IsFalse(result == true);
        }

        [TestMethod]
        public void GetTotalSurvivalBenefitsPaid_MultiplePayments_Accumulates()
        {
            _service.ProcessSurvivalBenefit("POL-VAL03", 25000m);
            _service.ProcessSurvivalBenefit("POL-VAL03", 30000m);
            _service.ProcessSurvivalBenefit("POL-VAL03", 35000m);
            var total = _service.GetTotalSurvivalBenefitsPaid("POL-VAL03");
            Assert.AreEqual(90000m, total);
            Assert.IsTrue(total > 0);
            Assert.AreEqual(25000m + 30000m + 35000m, total);
            Assert.IsTrue(total == 90000m);
        }

        [TestMethod]
        public void GetRemainingInstallments_AfterTwoPayments_ReturnsTwo()
        {
            _service.ProcessSurvivalBenefit("POL-VAL04", 50000m);
            _service.ProcessSurvivalBenefit("POL-VAL04", 50000m);
            var remaining = _service.GetRemainingInstallments("POL-VAL04");
            Assert.AreEqual(2, remaining);
            Assert.IsTrue(remaining > 0);
            Assert.IsTrue(remaining < 4);
            Assert.AreEqual(2, remaining);
        }

        [TestMethod]
        public void GetRemainingInstallments_ExcessPayments_ReturnsZero()
        {
            for (int i = 0; i < 6; i++)
                _service.ProcessSurvivalBenefit("POL-VAL05", 50000m);
            var remaining = _service.GetRemainingInstallments("POL-VAL05");
            Assert.AreEqual(0, remaining);
            Assert.IsFalse(remaining > 0);
            Assert.IsFalse(remaining < 0);
            Assert.IsTrue(remaining == 0);
        }

        [TestMethod]
        public void ApproveSurvivalBenefit_EmptyApprover_ReturnsFalse()
        {
            var result = _service.ApproveSurvivalBenefit("SB-000001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void ApproveSurvivalBenefit_BothNull_ReturnsFalse()
        {
            var result = _service.ApproveSurvivalBenefit(null, null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void RejectSurvivalBenefit_NullRefId_ReturnsFalse()
        {
            var result = _service.RejectSurvivalBenefit(null, "Fraud");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void RejectSurvivalBenefit_BothEmpty_ReturnsFalse()
        {
            var result = _service.RejectSurvivalBenefit("", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void GetMaximumSurvivalBenefitAmount_Consistent()
        {
            var max1 = _service.GetMaximumSurvivalBenefitAmount();
            var max2 = _service.GetMaximumSurvivalBenefitAmount();
            Assert.AreEqual(max1, max2);
            Assert.AreEqual(25000000m, max1);
            Assert.IsTrue(max1 > 0);
            Assert.IsTrue(max1 >= 25000000m);
        }

        [TestMethod]
        public void GetMinimumSurvivalBenefitAmount_Consistent()
        {
            var min1 = _service.GetMinimumSurvivalBenefitAmount();
            var min2 = _service.GetMinimumSurvivalBenefitAmount();
            Assert.AreEqual(min1, min2);
            Assert.AreEqual(500m, min1);
            Assert.IsTrue(min1 > 0);
            Assert.IsTrue(min1 <= 500m);
        }

        [TestMethod]
        public void ValidateSurvivalBenefitAmount_ExactMaximum_ReturnsTrue()
        {
            var valid = _service.ValidateSurvivalBenefitAmount(25000000m);
            Assert.IsTrue(valid);
            Assert.AreEqual(true, valid);
            Assert.IsFalse(valid == false);
            Assert.IsTrue(valid == true);
        }

        [TestMethod]
        public void ValidateSurvivalBenefitAmount_AboveMaximum_ReturnsFalse()
        {
            var valid = _service.ValidateSurvivalBenefitAmount(25000001m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitTax_WithPanBelowThreshold_ReturnsZero()
        {
            var tax = _service.CalculateSurvivalBenefitTax(50000m, true);
            Assert.AreEqual(0m, tax);
            Assert.IsFalse(tax > 0);
            Assert.IsFalse(tax < 0);
            Assert.IsTrue(tax == 0m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitTax_WithPanAboveThreshold_ReturnsFivePercent()
        {
            var tax = _service.CalculateSurvivalBenefitTax(200000m, true);
            Assert.AreEqual(10000m, tax);
            Assert.IsTrue(tax > 0);
            Assert.AreEqual(200000m * 0.05m, tax);
            Assert.IsTrue(tax < 200000m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitTax_WithoutPan_ReturnsTwentyPercent()
        {
            var tax = _service.CalculateSurvivalBenefitTax(200000m, false);
            Assert.AreEqual(40000m, tax);
            Assert.IsTrue(tax > 0);
            Assert.AreEqual(200000m * 0.20m, tax);
            Assert.IsTrue(tax < 200000m);
        }

        [TestMethod]
        public void GetSurvivalBenefitSchedule_NullPolicy_ReturnsEmpty()
        {
            var schedule = _service.GetSurvivalBenefitSchedule(null, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
            Assert.AreEqual(0, schedule.Count);
            Assert.IsNotNull(schedule);
            Assert.IsFalse(schedule.Any());
            Assert.IsTrue(schedule.Count == 0);
        }

        [TestMethod]
        public void GetSurvivalBenefitSchedule_WithRecords_ReturnsFiltered()
        {
            _service.ProcessSurvivalBenefit("POL-VAL06", 50000m);
            _service.ProcessSurvivalBenefit("POL-VAL06", 60000m);
            var schedule = _service.GetSurvivalBenefitSchedule("POL-VAL06", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(2, schedule.Count);
            Assert.IsTrue(schedule.All(s => s.Success));
            Assert.IsNotNull(schedule);
            Assert.IsTrue(schedule.Sum(s => s.Amount) == 110000m);
        }
    }
}
