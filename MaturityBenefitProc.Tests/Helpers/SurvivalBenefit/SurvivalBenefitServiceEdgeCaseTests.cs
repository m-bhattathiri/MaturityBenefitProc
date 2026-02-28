using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.SurvivalBenefit;

namespace MaturityBenefitProc.Tests.Helpers.SurvivalBenefit
{
    [TestClass]
    public class SurvivalBenefitServiceEdgeCaseTests
    {
        private SurvivalBenefitService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new SurvivalBenefitService();
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_MinimalAmount_ReturnsSuccess()
        {
            var result = _service.ProcessSurvivalBenefit("POL-EDGE01", 0.01m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0.01m, result.Amount);
            Assert.IsNotNull(result.ReferenceId);
            Assert.AreEqual(1, result.InstallmentNumber);
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_VeryLargeAmount_Processes()
        {
            var result = _service.ProcessSurvivalBenefit("POL-EDGE02", 99999999m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(99999999m, result.Amount);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.ReferenceId.Length > 0);
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_FourInstallments_AllSucceed()
        {
            for (int i = 1; i <= 4; i++)
            {
                var result = _service.ProcessSurvivalBenefit("POL-EDGE03", 50000m);
                Assert.IsTrue(result.Success);
                Assert.AreEqual(i, result.InstallmentNumber);
                Assert.IsTrue(result.Amount > 0);
                Assert.IsNotNull(result.ReferenceId);
            }
        }

        [TestMethod]
        public void ProcessSurvivalBenefit_UniqueReferenceIds_AcrossCalls()
        {
            var r1 = _service.ProcessSurvivalBenefit("POL-EDGE04A", 10000m);
            var r2 = _service.ProcessSurvivalBenefit("POL-EDGE04B", 20000m);
            Assert.AreNotEqual(r1.ReferenceId, r2.ReferenceId);
            Assert.IsTrue(r1.Success);
            Assert.IsTrue(r2.Success);
            Assert.AreEqual(10000m, r1.Amount);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_HighPercentage_ReturnsCorrect()
        {
            var result = _service.CalculateSurvivalBenefitAmount(1000000m, 50m, 1);
            Assert.AreEqual(500000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result == 1000000m * 0.50m);
            Assert.IsTrue(result < 1000000m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_HundredPercent_ReturnsSumAssured()
        {
            var result = _service.CalculateSurvivalBenefitAmount(500000m, 100m, 1);
            Assert.AreEqual(500000m, result);
            Assert.IsTrue(result > 0);
            Assert.AreEqual(500000m, result);
            Assert.IsTrue(result == 500000m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_FractionalPercentage_Rounds()
        {
            var result = _service.CalculateSurvivalBenefitAmount(333333m, 33.33m, 1);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 333333m);
            Assert.AreEqual(Math.Round(333333m * (33.33m / 100m), 2), result);
            Assert.IsTrue(result > 100000m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitAmount_NegativeSumAssured_ReturnsZero()
        {
            var result = _service.CalculateSurvivalBenefitAmount(-500000m, 20m, 1);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_JeevanPlan_FirstInstallment()
        {
            var result = _service.GetSurvivalBenefitPercentage("JEEVAN-ANAND", 1);
            Assert.AreEqual(20m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result <= 100);
            Assert.IsTrue(result >= 20m);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_JeevanPlan_FourthInstallment()
        {
            var result = _service.GetSurvivalBenefitPercentage("JEEVAN-ANAND", 4);
            Assert.AreEqual(25m, result);
            Assert.IsTrue(result > 20m);
            Assert.IsTrue(result <= 100);
            Assert.IsTrue(result >= 25m);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_EmptyPlanCode_ReturnsZero()
        {
            var result = _service.GetSurvivalBenefitPercentage("", 1);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetSurvivalBenefitPercentage_ZeroInstallment_ReturnsZero()
        {
            var result = _service.GetSurvivalBenefitPercentage("SB-PLAN-001", 0);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void IsSurvivalBenefitDue_PastDate_ReturnsTrue()
        {
            var result = _service.IsSurvivalBenefitDue("POL-EDGE05", DateTime.UtcNow.AddMonths(-1));
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsFalse(result == false);
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void IsSurvivalBenefitDue_EmptyPolicy_ReturnsFalse()
        {
            var result = _service.IsSurvivalBenefitDue("", DateTime.UtcNow);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsTrue(result == false);
            Assert.IsFalse(result == true);
        }

        [TestMethod]
        public void GetNextSurvivalBenefitDue_AfterProcessing_IncrementsInstallment()
        {
            _service.ProcessSurvivalBenefit("POL-EDGE06", 50000m);
            var next = _service.GetNextSurvivalBenefitDue("POL-EDGE06");
            Assert.IsTrue(next.Success);
            Assert.AreEqual(2, next.InstallmentNumber);
            Assert.IsNotNull(next.ReferenceId);
            Assert.IsTrue(next.DueDate > DateTime.UtcNow);
        }

        [TestMethod]
        public void GetNextSurvivalBenefitDue_EmptyPolicy_ReturnsFalse()
        {
            var result = _service.GetNextSurvivalBenefitDue("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void GetTotalSurvivalBenefitsPaid_NoPriorPayments_ReturnsZero()
        {
            var total = _service.GetTotalSurvivalBenefitsPaid("POL-NOHISTORY");
            Assert.AreEqual(0m, total);
            Assert.IsFalse(total > 0);
            Assert.IsFalse(total < 0);
            Assert.IsTrue(total == 0m);
        }

        [TestMethod]
        public void GetTotalSurvivalBenefitsPaid_EmptyPolicy_ReturnsZero()
        {
            var total = _service.GetTotalSurvivalBenefitsPaid("");
            Assert.AreEqual(0m, total);
            Assert.IsFalse(total > 0);
            Assert.IsFalse(total < 0);
            Assert.IsTrue(total == 0m);
        }

        [TestMethod]
        public void GetRemainingInstallments_AfterAllPaid_ReturnsZero()
        {
            for (int i = 0; i < 4; i++)
                _service.ProcessSurvivalBenefit("POL-EDGE07", 50000m);
            var remaining = _service.GetRemainingInstallments("POL-EDGE07");
            Assert.AreEqual(0, remaining);
            Assert.IsFalse(remaining > 0);
            Assert.IsFalse(remaining < 0);
            Assert.IsTrue(remaining == 0);
        }

        [TestMethod]
        public void GetRemainingInstallments_EmptyPolicy_ReturnsZero()
        {
            var remaining = _service.GetRemainingInstallments("");
            Assert.AreEqual(0, remaining);
            Assert.IsFalse(remaining > 0);
            Assert.IsFalse(remaining < 0);
            Assert.IsTrue(remaining == 0);
        }

        [TestMethod]
        public void ApproveSurvivalBenefit_ValidInputs_ReturnsSuccess()
        {
            var result = _service.ApproveSurvivalBenefit("SB-000001", "Manager");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("SB-000001", result.ReferenceId);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("approved"));
        }

        [TestMethod]
        public void ApproveSurvivalBenefit_NullRefId_ReturnsFalse()
        {
            var result = _service.ApproveSurvivalBenefit(null, "Manager");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void RejectSurvivalBenefit_ValidInputs_ReturnsSuccess()
        {
            var result = _service.RejectSurvivalBenefit("SB-000002", "Incomplete documents");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("SB-000002", result.ReferenceId);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("rejected"));
        }

        [TestMethod]
        public void RejectSurvivalBenefit_EmptyReason_ReturnsFalse()
        {
            var result = _service.RejectSurvivalBenefit("SB-000003", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void ValidateSurvivalBenefitAmount_ExactMinimum_ReturnsTrue()
        {
            var valid = _service.ValidateSurvivalBenefitAmount(500m);
            Assert.IsTrue(valid);
            Assert.AreEqual(true, valid);
            Assert.IsFalse(valid == false);
            Assert.IsTrue(valid == true);
        }

        [TestMethod]
        public void ValidateSurvivalBenefitAmount_BelowMinimum_ReturnsFalse()
        {
            var valid = _service.ValidateSurvivalBenefitAmount(499m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitTax_ZeroAmount_ReturnsZero()
        {
            var tax = _service.CalculateSurvivalBenefitTax(0m, true);
            Assert.AreEqual(0m, tax);
            Assert.IsFalse(tax > 0);
            Assert.IsFalse(tax < 0);
            Assert.IsTrue(tax == 0m);
        }

        [TestMethod]
        public void CalculateSurvivalBenefitTax_NegativeAmount_ReturnsZero()
        {
            var tax = _service.CalculateSurvivalBenefitTax(-5000m, true);
            Assert.AreEqual(0m, tax);
            Assert.IsFalse(tax > 0);
            Assert.IsFalse(tax < 0);
            Assert.IsTrue(tax == 0m);
        }
    }
}
