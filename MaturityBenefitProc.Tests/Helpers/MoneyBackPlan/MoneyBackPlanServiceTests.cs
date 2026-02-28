using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.MoneyBackPlan;

namespace MaturityBenefitProc.Tests.Helpers.MoneyBackPlan
{
    [TestClass]
    public class MoneyBackPlanServiceTests
    {
        private MoneyBackPlanService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new MoneyBackPlanService();
            _service.RegisterPolicy("POL-MB-001", "MB20", 500000m, 2010);
            _service.RegisterPolicy("POL-MB-002", "MB25", 1000000m, 2008);
            _service.RegisterPolicy("POL-MB-003", "MB15", 750000m, 2012);
            _service.RegisterPolicy("POL-MB-004", "MB20", 250000m, 2005);
            _service.RegisterPolicy("POL-MB-005", "MB25", 2000000m, 2009);
            _service.RegisterPolicy("POL-MB-006", "MB15", 300000m, 2011);
            _service.RegisterPolicy("POL-MB-007", "MB20", 1500000m, 2013);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_ValidPolicy_ReturnsSuccess()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-001", 1);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Money-back payout processed successfully", result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.Amount > 0);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_MB20FirstInstallment_Returns20Percent()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-001", 1);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(100000m, result.Amount);
            Assert.AreEqual(20m, result.PayoutPercentage);
            Assert.AreEqual(1, result.InstallmentNumber);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_MB20SecondInstallment_Returns20Percent()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-001", 2);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(100000m, result.Amount);
            Assert.AreEqual(20m, result.PayoutPercentage);
            Assert.AreEqual(2, result.InstallmentNumber);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_MB20ThirdInstallment_Returns20Percent()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-001", 3);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(100000m, result.Amount);
            Assert.AreEqual(20m, result.PayoutPercentage);
            Assert.AreEqual(3, result.InstallmentNumber);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_MB20FinalInstallment_Returns40Percent()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-001", 4);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(200000m, result.Amount);
            Assert.AreEqual(40m, result.PayoutPercentage);
            Assert.AreEqual("MB20", result.PlanCode);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_MB25FirstInstallment_Returns15Percent()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-002", 1);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(150000m, result.Amount);
            Assert.AreEqual(15m, result.PayoutPercentage);
            Assert.AreEqual(1, result.InstallmentNumber);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_MB25SecondInstallment_Returns15Percent()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-002", 2);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(150000m, result.Amount);
            Assert.AreEqual(15m, result.PayoutPercentage);
            Assert.AreEqual(2, result.InstallmentNumber);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_MB25FinalInstallment_Returns40Percent()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-002", 5);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(400000m, result.Amount);
            Assert.AreEqual(40m, result.PayoutPercentage);
            Assert.AreEqual("MB25", result.PlanCode);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_MB15FirstInstallment_Returns25Percent()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-003", 1);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(187500m, result.Amount);
            Assert.AreEqual(25m, result.PayoutPercentage);
            Assert.AreEqual(1, result.InstallmentNumber);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_MB15FinalInstallment_Returns50Percent()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-003", 3);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(375000m, result.Amount);
            Assert.AreEqual(50m, result.PayoutPercentage);
            Assert.AreEqual("MB15", result.PlanCode);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_NullPolicy_ReturnsFailure()
        {
            var result = _service.ProcessMoneyBackPayout(null, 1);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_EmptyPolicy_ReturnsFailure()
        {
            var result = _service.ProcessMoneyBackPayout("", 1);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_ZeroInstallment_ReturnsFailure()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-001", 0);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Installment number must be greater than zero", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_NegativeInstallment_ReturnsFailure()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-001", -3);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Installment number must be greater than zero", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_ExceedsMaxInstallment_ReturnsFailure()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-001", 11);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Installment number exceeds maximum allowed installments", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_UnknownPolicy_ReturnsFailure()
        {
            var result = _service.ProcessMoneyBackPayout("POL-UNKNOWN", 1);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy not found in records", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_SetsReferenceIdFormat()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-001", 1);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.ReferenceId.StartsWith("MB-POL-MB-001-1-"));
            Assert.IsNotNull(result.ProcessedDate);
            Assert.AreEqual("MB20", result.PlanCode);
        }

        [TestMethod]
        public void ProcessMoneyBackPayout_LargeSumPolicy_ReturnsCorrectAmount()
        {
            var result = _service.ProcessMoneyBackPayout("POL-MB-005", 1);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(300000m, result.Amount);
            Assert.AreEqual(15m, result.PayoutPercentage);
            Assert.AreEqual("MB25", result.PlanCode);
        }

        [TestMethod]
        public void CalculateMoneyBackAmount_ValidInputs_ReturnsCorrectAmount()
        {
            var amount = _service.CalculateMoneyBackAmount(500000m, 20m);
            Assert.AreEqual(100000m, amount);
            var amount2 = _service.CalculateMoneyBackAmount(1000000m, 15m);
            Assert.AreEqual(150000m, amount2);
            var amount3 = _service.CalculateMoneyBackAmount(750000m, 25m);
            Assert.AreEqual(187500m, amount3);
            var amount4 = _service.CalculateMoneyBackAmount(500000m, 40m);
            Assert.AreEqual(200000m, amount4);
        }

        [TestMethod]
        public void CalculateMoneyBackAmount_ZeroSumAssured_ReturnsZero()
        {
            var amount = _service.CalculateMoneyBackAmount(0m, 20m);
            Assert.AreEqual(0m, amount);
            var amount2 = _service.CalculateMoneyBackAmount(-100m, 20m);
            Assert.AreEqual(0m, amount2);
            var amount3 = _service.CalculateMoneyBackAmount(0m, 0m);
            Assert.AreEqual(0m, amount3);
            var amount4 = _service.CalculateMoneyBackAmount(-500m, 50m);
            Assert.AreEqual(0m, amount4);
        }

        [TestMethod]
        public void CalculateMoneyBackAmount_InvalidPercentage_ReturnsZero()
        {
            var amount = _service.CalculateMoneyBackAmount(500000m, 0m);
            Assert.AreEqual(0m, amount);
            var amount2 = _service.CalculateMoneyBackAmount(500000m, -10m);
            Assert.AreEqual(0m, amount2);
            var amount3 = _service.CalculateMoneyBackAmount(500000m, 101m);
            Assert.AreEqual(0m, amount3);
            var amount4 = _service.CalculateMoneyBackAmount(500000m, 200m);
            Assert.AreEqual(0m, amount4);
        }

        [TestMethod]
        public void CalculateMoneyBackAmount_FullPercentage_ReturnsSumAssured()
        {
            var amount = _service.CalculateMoneyBackAmount(500000m, 100m);
            Assert.AreEqual(500000m, amount);
            var amount2 = _service.CalculateMoneyBackAmount(1000000m, 100m);
            Assert.AreEqual(1000000m, amount2);
            var amount3 = _service.CalculateMoneyBackAmount(250000m, 100m);
            Assert.AreEqual(250000m, amount3);
            var amount4 = _service.CalculateMoneyBackAmount(750000m, 50m);
            Assert.AreEqual(375000m, amount4);
        }

        [TestMethod]
        public void CalculateMoneyBackAmount_SmallValues_ReturnsPreciseAmount()
        {
            var amount = _service.CalculateMoneyBackAmount(100m, 10m);
            Assert.AreEqual(10m, amount);
            var amount2 = _service.CalculateMoneyBackAmount(1000m, 5m);
            Assert.AreEqual(50m, amount2);
            var amount3 = _service.CalculateMoneyBackAmount(333m, 33m);
            Assert.AreEqual(109.89m, amount3);
            var amount4 = _service.CalculateMoneyBackAmount(10000m, 1m);
            Assert.AreEqual(100m, amount4);
        }

        [TestMethod]
        public void GetPayoutPercentage_MB20AllInstallments_ReturnsCorrectPercentages()
        {
            Assert.AreEqual(20m, _service.GetPayoutPercentage("MB20", 1));
            Assert.AreEqual(20m, _service.GetPayoutPercentage("MB20", 2));
            Assert.AreEqual(20m, _service.GetPayoutPercentage("MB20", 3));
            Assert.AreEqual(40m, _service.GetPayoutPercentage("MB20", 4));
        }

        [TestMethod]
        public void GetPayoutPercentage_MB25AllInstallments_ReturnsCorrectPercentages()
        {
            Assert.AreEqual(15m, _service.GetPayoutPercentage("MB25", 1));
            Assert.AreEqual(15m, _service.GetPayoutPercentage("MB25", 2));
            Assert.AreEqual(15m, _service.GetPayoutPercentage("MB25", 3));
            Assert.AreEqual(15m, _service.GetPayoutPercentage("MB25", 4));
            Assert.AreEqual(40m, _service.GetPayoutPercentage("MB25", 5));
        }

        [TestMethod]
        public void GetPayoutPercentage_MB15AllInstallments_ReturnsCorrectPercentages()
        {
            Assert.AreEqual(25m, _service.GetPayoutPercentage("MB15", 1));
            Assert.AreEqual(25m, _service.GetPayoutPercentage("MB15", 2));
            Assert.AreEqual(50m, _service.GetPayoutPercentage("MB15", 3));
            Assert.AreEqual(25m, _service.GetPayoutPercentage("MB15", 4));
        }

        [TestMethod]
        public void GetPayoutPercentage_UnknownPlan_ReturnsDefault20()
        {
            Assert.AreEqual(20m, _service.GetPayoutPercentage("UNKNOWN", 1));
            Assert.AreEqual(20m, _service.GetPayoutPercentage("CUSTOM", 2));
            Assert.AreEqual(20m, _service.GetPayoutPercentage("XYZ", 3));
            Assert.AreEqual(20m, _service.GetPayoutPercentage("SPECIAL", 5));
        }

        [TestMethod]
        public void GetPayoutPercentage_NullPlanCode_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.GetPayoutPercentage(null, 1));
            Assert.AreEqual(0m, _service.GetPayoutPercentage("", 2));
            Assert.AreEqual(0m, _service.GetPayoutPercentage("  ", 3));
            Assert.AreEqual(0m, _service.GetPayoutPercentage(null, 0));
        }

        [TestMethod]
        public void GetPayoutPercentage_CaseInsensitive_ReturnsCorrectPercentages()
        {
            Assert.AreEqual(20m, _service.GetPayoutPercentage("mb20", 1));
            Assert.AreEqual(15m, _service.GetPayoutPercentage("mb25", 2));
            Assert.AreEqual(25m, _service.GetPayoutPercentage("Mb15", 1));
            Assert.AreEqual(40m, _service.GetPayoutPercentage("mB20", 4));
        }

        [TestMethod]
        public void GetPayoutSchedule_ValidPolicy_ReturnsSchedule()
        {
            var result = _service.GetPayoutSchedule("POL-MB-001");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Payout schedule generated successfully", result.Message);
            Assert.IsTrue(result.Amount > 0);
            Assert.AreEqual("MB20", result.PlanCode);
        }

        [TestMethod]
        public void GetPayoutSchedule_MB20Policy_HasFourInstallments()
        {
            var result = _service.GetPayoutSchedule("POL-MB-001");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(4, result.InstallmentNumber);
            Assert.IsTrue(result.Metadata.ContainsKey("Installment_1_Year"));
            Assert.IsTrue(result.Metadata.ContainsKey("Installment_4_Amount"));
        }

        [TestMethod]
        public void GetPayoutSchedule_MB25Policy_HasFiveInstallments()
        {
            var result = _service.GetPayoutSchedule("POL-MB-002");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(5, result.InstallmentNumber);
            Assert.IsTrue(result.Metadata.ContainsKey("Installment_5_Year"));
            Assert.IsTrue(result.Metadata.ContainsKey("Installment_5_Amount"));
        }

        [TestMethod]
        public void GetPayoutSchedule_NullPolicy_ReturnsFailure()
        {
            var result = _service.GetPayoutSchedule(null);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required", result.Message);
            Assert.IsNull(result.PlanCode);
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void IsMoneyBackDue_DueDate_ReturnsTrue()
        {
            Assert.IsTrue(_service.IsMoneyBackDue("POL-MB-001", new DateTime(2016, 6, 1)));
            Assert.IsTrue(_service.IsMoneyBackDue("POL-MB-002", new DateTime(2014, 1, 1)));
            Assert.IsTrue(_service.IsMoneyBackDue("POL-MB-003", new DateTime(2018, 1, 1)));
            Assert.IsTrue(_service.IsMoneyBackDue("POL-MB-004", new DateTime(2011, 1, 1)));
        }

        [TestMethod]
        public void IsMoneyBackDue_BeforeDueDate_ReturnsFalse()
        {
            Assert.IsFalse(_service.IsMoneyBackDue("POL-MB-001", new DateTime(2014, 1, 1)));
            Assert.IsFalse(_service.IsMoneyBackDue("POL-MB-001", new DateTime(2011, 6, 1)));
            Assert.IsFalse(_service.IsMoneyBackDue(null, new DateTime(2020, 1, 1)));
            Assert.IsFalse(_service.IsMoneyBackDue("UNKNOWN", new DateTime(2020, 1, 1)));
        }

        [TestMethod]
        public void GetNextPayoutYear_FirstPayout_ReturnsCorrectYear()
        {
            Assert.AreEqual(2015, _service.GetNextPayoutYear("POL-MB-001"));
            Assert.AreEqual(2013, _service.GetNextPayoutYear("POL-MB-002"));
            Assert.AreEqual(2017, _service.GetNextPayoutYear("POL-MB-003"));
            Assert.AreEqual(2010, _service.GetNextPayoutYear("POL-MB-004"));
        }

        [TestMethod]
        public void GetNextPayoutYear_NullPolicy_ReturnsZero()
        {
            Assert.AreEqual(0, _service.GetNextPayoutYear(null));
            Assert.AreEqual(0, _service.GetNextPayoutYear(""));
            Assert.AreEqual(0, _service.GetNextPayoutYear("  "));
            Assert.AreEqual(0, _service.GetNextPayoutYear("UNKNOWN-POL"));
        }

        [TestMethod]
        public void GetTotalMoneyBackPaid_NoPayout_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.GetTotalMoneyBackPaid("POL-MB-001"));
            Assert.AreEqual(0m, _service.GetTotalMoneyBackPaid(null));
            Assert.AreEqual(0m, _service.GetTotalMoneyBackPaid(""));
            Assert.AreEqual(0m, _service.GetTotalMoneyBackPaid("UNKNOWN"));
        }

        [TestMethod]
        public void GetTotalMoneyBackPaid_AfterPayouts_AccumulatesCorrectly()
        {
            _service.ProcessMoneyBackPayout("POL-MB-001", 1);
            Assert.AreEqual(100000m, _service.GetTotalMoneyBackPaid("POL-MB-001"));
            _service.ProcessMoneyBackPayout("POL-MB-001", 2);
            Assert.AreEqual(200000m, _service.GetTotalMoneyBackPaid("POL-MB-001"));
            _service.ProcessMoneyBackPayout("POL-MB-001", 3);
            Assert.AreEqual(300000m, _service.GetTotalMoneyBackPaid("POL-MB-001"));
        }

        [TestMethod]
        public void GetRemainingMoneyBackPayable_FullPayable_ReturnsScheduleTotal()
        {
            var remaining = _service.GetRemainingMoneyBackPayable("POL-MB-001");
            Assert.IsTrue(remaining > 0);
            Assert.AreEqual(500000m, remaining);
            Assert.IsTrue(remaining <= 500000m);
            Assert.AreNotEqual(0m, remaining);
        }

        [TestMethod]
        public void GetRemainingMoneyBackPayable_AfterPayout_Decreases()
        {
            var before = _service.GetRemainingMoneyBackPayable("POL-MB-001");
            _service.ProcessMoneyBackPayout("POL-MB-001", 1);
            var after = _service.GetRemainingMoneyBackPayable("POL-MB-001");
            Assert.IsTrue(after < before);
            Assert.AreEqual(100000m, before - after);
            Assert.AreEqual(400000m, after);
            Assert.IsTrue(after >= 0);
        }

        [TestMethod]
        public void GetFinalInstallmentAmount_StandardCase_ReturnsCorrectAmount()
        {
            Assert.AreEqual(250000m, _service.GetFinalInstallmentAmount(500000m, 300000m, 50000m));
            Assert.AreEqual(500000m, _service.GetFinalInstallmentAmount(1000000m, 600000m, 100000m));
            Assert.AreEqual(400000m, _service.GetFinalInstallmentAmount(750000m, 375000m, 25000m));
            Assert.IsTrue(_service.GetFinalInstallmentAmount(500000m, 0m, 100000m) > 500000m);
        }

        [TestMethod]
        public void GetFinalInstallmentAmount_ZeroSumAssured_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.GetFinalInstallmentAmount(0m, 0m, 0m));
            Assert.AreEqual(0m, _service.GetFinalInstallmentAmount(-100m, 0m, 0m));
            Assert.AreEqual(0m, _service.GetFinalInstallmentAmount(0m, 100m, 50m));
            Assert.IsFalse(_service.GetFinalInstallmentAmount(-500m, 0m, 0m) > 0);
        }

        [TestMethod]
        public void ValidateMoneyBackAmount_ValidAmount_ReturnsTrue()
        {
            Assert.IsTrue(_service.ValidateMoneyBackAmount(1m));
            Assert.IsTrue(_service.ValidateMoneyBackAmount(100000m));
            Assert.IsTrue(_service.ValidateMoneyBackAmount(50000000m));
            Assert.IsTrue(_service.ValidateMoneyBackAmount(25000000m));
        }

        [TestMethod]
        public void ValidateMoneyBackAmount_InvalidAmount_ReturnsFalse()
        {
            Assert.IsFalse(_service.ValidateMoneyBackAmount(0m));
            Assert.IsFalse(_service.ValidateMoneyBackAmount(-1m));
            Assert.IsFalse(_service.ValidateMoneyBackAmount(50000001m));
            Assert.IsFalse(_service.ValidateMoneyBackAmount(100000000m));
        }

        [TestMethod]
        public void CalculateMoneyBackTax_WithPanCard_Returns2Percent()
        {
            Assert.AreEqual(2000m, _service.CalculateMoneyBackTax(100000m, true));
            Assert.AreEqual(10000m, _service.CalculateMoneyBackTax(500000m, true));
            Assert.AreEqual(20000m, _service.CalculateMoneyBackTax(1000000m, true));
            Assert.AreEqual(5000m, _service.CalculateMoneyBackTax(250000m, true));
        }

        [TestMethod]
        public void CalculateMoneyBackTax_WithoutPanCard_Returns20Percent()
        {
            Assert.AreEqual(20000m, _service.CalculateMoneyBackTax(100000m, false));
            Assert.AreEqual(100000m, _service.CalculateMoneyBackTax(500000m, false));
            Assert.AreEqual(200000m, _service.CalculateMoneyBackTax(1000000m, false));
            Assert.AreEqual(50000m, _service.CalculateMoneyBackTax(250000m, false));
        }

        [TestMethod]
        public void CalculateMoneyBackTax_ZeroAmount_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateMoneyBackTax(0m, true));
            Assert.AreEqual(0m, _service.CalculateMoneyBackTax(0m, false));
            Assert.AreEqual(0m, _service.CalculateMoneyBackTax(-100m, true));
            Assert.AreEqual(0m, _service.CalculateMoneyBackTax(-500m, false));
        }

        [TestMethod]
        public void ValidateMoneyBackPlan_ValidPolicy_ReturnsSuccess()
        {
            var result = _service.ValidateMoneyBackPlan("POL-MB-001");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Money-back plan validation successful", result.Message);
            Assert.AreEqual("MB20", result.PlanCode);
            Assert.AreEqual(500000m, result.Amount);
        }

        [TestMethod]
        public void ValidateMoneyBackPlan_NullPolicy_ReturnsFailure()
        {
            var result = _service.ValidateMoneyBackPlan(null);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required for validation", result.Message);
            Assert.IsNull(result.PlanCode);
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void ApproveMoneyBackPayout_ValidReference_ReturnsSuccess()
        {
            var payout = _service.ProcessMoneyBackPayout("POL-MB-001", 1);
            var approval = _service.ApproveMoneyBackPayout(payout.ReferenceId, "Manager_A");
            Assert.IsTrue(approval.Success);
            Assert.AreEqual("Money-back payout approved successfully", approval.Message);
            Assert.IsNotNull(approval.ReferenceId);
            Assert.IsTrue(approval.Metadata.ContainsKey("ApprovedBy"));
        }

        [TestMethod]
        public void RejectMoneyBackPayout_ValidReference_ReturnsSuccess()
        {
            var payout = _service.ProcessMoneyBackPayout("POL-MB-001", 1);
            var rejection = _service.RejectMoneyBackPayout(payout.ReferenceId, "Incomplete docs");
            Assert.IsTrue(rejection.Success);
            Assert.AreEqual("Money-back payout rejected", rejection.Message);
            Assert.IsNotNull(rejection.ReferenceId);
            Assert.IsTrue(rejection.Metadata.ContainsKey("RejectionReason"));
        }

        [TestMethod]
        public void GetMoneyBackPayoutHistory_NoHistory_ReturnsEmpty()
        {
            var history = _service.GetMoneyBackPayoutHistory("POL-MB-001", DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(history);
            Assert.AreEqual(0, history.Count);
            var history2 = _service.GetMoneyBackPayoutHistory(null, DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(history2);
            Assert.AreEqual(0, history2.Count);
        }

        [TestMethod]
        public void GetMoneyBackPayoutHistory_WithPayouts_ReturnsAll()
        {
            _service.ProcessMoneyBackPayout("POL-MB-001", 1);
            _service.ProcessMoneyBackPayout("POL-MB-001", 2);
            var history = _service.GetMoneyBackPayoutHistory("POL-MB-001", DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));
            Assert.AreEqual(2, history.Count);
            Assert.IsTrue(history.All(h => h.Success));
            Assert.AreEqual(100000m, history[0].Amount);
            Assert.AreEqual(100000m, history[1].Amount);
        }
    }
}
