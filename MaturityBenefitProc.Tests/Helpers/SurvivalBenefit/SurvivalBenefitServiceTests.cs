using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.SurvivalBenefit;

namespace MaturityBenefitProc.Tests.Helpers.SurvivalBenefit
{
    [TestClass]
    public class SurvivalBenefitServiceTests
    {
        private SurvivalBenefitService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new SurvivalBenefitService();
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_ValidInputs_ReturnsSuccess()
        {
            var result = _service.ProcessSurvivalBenefit("POL-SB001", 100000m);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.ReferenceId);
            Assert.AreEqual(100000m, result.Amount);
            Assert.IsTrue(result.ReferenceId.StartsWith("SB-"));
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_NullPolicy_ReturnsFalse()
        {
            var result = _service.ProcessSurvivalBenefit(null, 100000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.AreEqual(string.Empty, result.ReferenceId);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_ZeroAmount_ReturnsFalse()
        {
            var result = _service.ProcessSurvivalBenefit("POL-SB002", 0m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.AreEqual(string.Empty, result.ReferenceId);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_NegativeAmount_ReturnsFalse()
        {
            var result = _service.ProcessSurvivalBenefit("POL-SB003", -5000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(string.Empty, result.ReferenceId);
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_SetsInstallmentNumber()
        {
            var result = _service.ProcessSurvivalBenefit("POL-SB004", 50000m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.InstallmentNumber);
            Assert.IsTrue(result.BenefitPercentage > 0);
            Assert.IsNotNull(result.PlanCode);
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_SecondInstallment_IncrementsNumber()
        {
            _service.ProcessSurvivalBenefit("POL-SB005", 50000m);
            var result = _service.ProcessSurvivalBenefit("POL-SB005", 50000m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(2, result.InstallmentNumber);
            Assert.IsTrue(result.Amount > 0);
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_SetsPlanCode()
        {
            var result = _service.ProcessSurvivalBenefit("POL-SB006", 75000m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("SB-PLAN-001", result.PlanCode);
            Assert.IsNotNull(result.PlanCode);
            Assert.IsTrue(result.PlanCode.Length > 0);
        }

        [TestMethod]
        public void ValidateSurvivalBenefit_ValidPolicy_ReturnsSuccess()
        {
            var result = _service.ValidateSurvivalBenefit("POL-SB007");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.Message.Contains("POL-SB007"));
        }

        [TestMethod]
        public void ValidateSurvivalBenefit_NullPolicy_ReturnsFalse()
        {
            var result = _service.ValidateSurvivalBenefit(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void ValidateSurvivalBenefit_EmptyPolicy_ReturnsFalse()
        {
            var result = _service.ValidateSurvivalBenefit("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_ValidInputs_ReturnsAmount()
        {
            var result = _service.CalculateSurvivalBenefitAmount(500000m, 20m, 1);
            Assert.AreEqual(100000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 500000m);
            Assert.AreEqual(500000m * 0.20m, result);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_ZeroSumAssured_ReturnsZero()
        {
            var result = _service.CalculateSurvivalBenefitAmount(0m, 20m, 1);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_ZeroPercentage_ReturnsZero()
        {
            var result = _service.CalculateSurvivalBenefitAmount(500000m, 0m, 1);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_ZeroInstallment_ReturnsZero()
        {
            var result = _service.CalculateSurvivalBenefitAmount(500000m, 20m, 0);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_SbPlan_FirstInstallment_Returns20()
        {
            var result = _service.GetSurvivalBenefitPercentage("SB-PLAN-001", 1);
            Assert.AreEqual(20m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result <= 100);
            Assert.IsTrue(result >= 20m);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_SbPlan_ThirdInstallment_Returns25()
        {
            var result = _service.GetSurvivalBenefitPercentage("SB-PLAN-001", 3);
            Assert.AreEqual(25m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result > 20m);
            Assert.IsTrue(result <= 100);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_SbPlan_FifthInstallment_Returns30()
        {
            var result = _service.GetSurvivalBenefitPercentage("SB-PLAN-001", 5);
            Assert.AreEqual(30m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result > 25m);
            Assert.IsTrue(result <= 100);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_NullPlanCode_ReturnsZero()
        {
            var result = _service.GetSurvivalBenefitPercentage(null, 1);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_OtherPlan_Returns15()
        {
            var result = _service.GetSurvivalBenefitPercentage("OTHER-PLAN", 1);
            Assert.AreEqual(15m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 20m);
            Assert.IsTrue(result <= 100);
        }

        [TestMethod]
        public void IsSurvivalBenefitDue_FutureDate_ReturnsTrue()
        {
            var result = _service.IsSurvivalBenefitDue("POL-SB008", DateTime.UtcNow);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsFalse(result == false);
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void IsSurvivalBenefitDue_NullPolicy_ReturnsFalse()
        {
            var result = _service.IsSurvivalBenefitDue(null, DateTime.UtcNow);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsTrue(result == false);
            Assert.IsFalse(result == true);
        }

        [TestMethod]
        public void GetNextSurvivalBenefitDue_ValidPolicy_ReturnsNext()
        {
            var result = _service.GetNextSurvivalBenefitDue("POL-SB009");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.InstallmentNumber);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.DueDate > DateTime.UtcNow);
        }

        [TestMethod]
        public void GetNextSurvivalBenefitDue_NullPolicy_ReturnsFalse()
        {
            var result = _service.GetNextSurvivalBenefitDue(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void GetTotalSurvivalBenefitsPaid_WithPayments_ReturnsTotal()
        {
            _service.ProcessSurvivalBenefit("POL-SB010", 50000m);
            _service.ProcessSurvivalBenefit("POL-SB010", 75000m);
            var total = _service.GetTotalSurvivalBenefitsPaid("POL-SB010");
            Assert.AreEqual(125000m, total);
            Assert.IsTrue(total > 0);
            Assert.AreEqual(50000m + 75000m, total);
            Assert.IsTrue(total == 125000m);
        }

        [TestMethod]
        public void GetTotalSurvivalBenefitsPaid_NullPolicy_ReturnsZero()
        {
            var total = _service.GetTotalSurvivalBenefitsPaid(null);
            Assert.AreEqual(0m, total);
            Assert.IsFalse(total > 0);
            Assert.IsFalse(total < 0);
            Assert.IsTrue(total == 0m);
        }

        [TestMethod]
        public void GetRemainingInstallments_NewPolicy_ReturnsFour()
        {
            var remaining = _service.GetRemainingInstallments("POL-SB011");
            Assert.AreEqual(4, remaining);
            Assert.IsTrue(remaining > 0);
            Assert.IsTrue(remaining <= 4);
            Assert.AreEqual(4, remaining);
        }

        [TestMethod]
        public void GetRemainingInstallments_AfterOnePayment_ReturnsThree()
        {
            _service.ProcessSurvivalBenefit("POL-SB012", 50000m);
            var remaining = _service.GetRemainingInstallments("POL-SB012");
            Assert.AreEqual(3, remaining);
            Assert.IsTrue(remaining > 0);
            Assert.IsTrue(remaining < 4);
            Assert.AreEqual(3, remaining);
        }

        [TestMethod]
        public void GetRemainingInstallments_NullPolicy_ReturnsZero()
        {
            var remaining = _service.GetRemainingInstallments(null);
            Assert.AreEqual(0, remaining);
            Assert.IsFalse(remaining > 0);
            Assert.IsFalse(remaining < 0);
            Assert.IsTrue(remaining == 0);
        }
    }
}
