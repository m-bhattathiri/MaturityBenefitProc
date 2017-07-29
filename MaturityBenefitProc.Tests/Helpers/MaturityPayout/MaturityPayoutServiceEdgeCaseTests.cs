using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.MaturityPayout;

namespace MaturityBenefitProc.Tests.Helpers.MaturityPayout
{
    [TestClass]
    public class MaturityPayoutServiceEdgeCaseTests
    {
        private MaturityPayoutService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new MaturityPayoutService();
        }

        [TestMethod]
        public void ProcessMaturityPayout_MinimumAmount_ReturnsSuccess()
        {
            var result = _service.ProcessMaturityPayout("POL-EDGE01", 0.01m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0.01m, result.Amount);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.NetPayable >= 0);
        }

        [TestMethod]
        public void ProcessMaturityPayout_VeryLargeAmount_ReturnsSuccess()
        {
            var result = _service.ProcessMaturityPayout("POL-EDGE02", 99999999.99m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(99999999.99m, result.Amount);
            Assert.IsTrue(result.TdsDeducted > 0);
            Assert.IsTrue(result.NetPayable > 0);
        }

        [TestMethod]
        public void ProcessMaturityPayout_MultipleCalls_UniqueRefs()
        {
            var r1 = _service.ProcessMaturityPayout("POL-EDGE03", 100000m);
            var r2 = _service.ProcessMaturityPayout("POL-EDGE04", 200000m);
            Assert.AreNotEqual(r1.ReferenceId, r2.ReferenceId);
            Assert.IsTrue(r1.Success);
            Assert.IsTrue(r2.Success);
            Assert.AreEqual(100000m, r1.Amount);
        }

        [TestMethod]
        public void ProcessMaturityPayout_SamePolicyTwice_BothSucceed()
        {
            var r1 = _service.ProcessMaturityPayout("POL-EDGE05", 100000m);
            var r2 = _service.ProcessMaturityPayout("POL-EDGE05", 200000m);
            Assert.IsTrue(r1.Success);
            Assert.IsTrue(r2.Success);
            Assert.AreNotEqual(r1.ReferenceId, r2.ReferenceId);
            Assert.AreNotEqual(r1.Amount, r2.Amount);
        }

        [TestMethod]
        public void CalculateMaturityAmount_AllZeros_ReturnsZero()
        {
            var result = _service.CalculateMaturityAmount(0m, 0m, 0m, 0m);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateMaturityAmount_OnlySumAssured_ReturnsSumAssured()
        {
            var result = _service.CalculateMaturityAmount(1000000m, 0m, 0m, 0m);
            Assert.AreEqual(1000000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result == 1000000m);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateMaturityAmount_LargeValues_HandlesCorrectly()
        {
            var result = _service.CalculateMaturityAmount(50000000m, 10000000m, 5000000m, 2000000m);
            Assert.AreEqual(67000000m, result);
            Assert.IsTrue(result > 50000000m);
            Assert.IsTrue(result > 0);
            Assert.AreEqual(50000000m + 10000000m + 5000000m + 2000000m, result);
        }

        [TestMethod]
        public void CalculateNetPayableAmount_NoDeductions_ReturnsGross()
        {
            var result = _service.CalculateNetPayableAmount(500000m, 0m, 0m);
            Assert.AreEqual(500000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result == 500000m);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateNetPayableAmount_NegativeGross_ReturnsZero()
        {
            var result = _service.CalculateNetPayableAmount(-100m, 0m, 0m);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void CalculateNetPayableAmount_EqualDeductions_ReturnsZero()
        {
            var result = _service.CalculateNetPayableAmount(10000m, 5000m, 5000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result < 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void GetTotalDeductions_ValidPolicyAndAmount_ReturnsPositive()
        {
            var result = _service.GetTotalDeductions("POL-EDGE06", 500000m);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 500000m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetTotalDeductions_NullPolicy_ReturnsZero()
        {
            var result = _service.GetTotalDeductions(null, 500000m);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetTotalDeductions_ZeroGrossAmount_ReturnsZero()
        {
            var result = _service.GetTotalDeductions("POL-EDGE07", 0m);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetTotalDeductions_NegativeAmount_ReturnsZero()
        {
            var result = _service.GetTotalDeductions("POL-EDGE08", -5000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void ApproveMaturityPayout_ExistingPayout_SetsApprover()
        {
            var payout = _service.ProcessMaturityPayout("POL-EDGE09", 300000m);
            var result = _service.ApproveMaturityPayout(payout.ReferenceId, "Supervisor1");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Supervisor1", result.ApprovedBy);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ApproveMaturityPayout_NonExistentPayout_StillApproves()
        {
            var result = _service.ApproveMaturityPayout("MPO-NONEXIST", "Supervisor2");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Supervisor2", result.ApprovedBy);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual("MPO-NONEXIST", result.ReferenceId);
        }

        [TestMethod]
        public void ApproveMaturityPayout_NullRefId_ReturnsFalse()
        {
            var result = _service.ApproveMaturityPayout(null, "Supervisor3");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void ApproveMaturityPayout_NullApprover_ReturnsFalse()
        {
            var result = _service.ApproveMaturityPayout("MPO-000001", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void RejectMaturityPayout_ValidInputs_ReturnsRejected()
        {
            var result = _service.RejectMaturityPayout("MPO-000002", "Fraudulent claim");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("REJECTED", result.PaymentMode);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual("MPO-000002", result.ReferenceId);
        }

        [TestMethod]
        public void RejectMaturityPayout_NullRefId_ReturnsFalse()
        {
            var result = _service.RejectMaturityPayout(null, "Some reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void RejectMaturityPayout_NullReason_ReturnsFalse()
        {
            var result = _service.RejectMaturityPayout("MPO-000003", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void SuspendPayout_ValidInputs_ReturnsSuspended()
        {
            var result = _service.SuspendPayout("MPO-000004", "Pending investigation");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("SUSPENDED", result.PaymentMode);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual("MPO-000004", result.ReferenceId);
        }

        [TestMethod]
        public void SuspendPayout_NullRefId_ReturnsFalse()
        {
            var result = _service.SuspendPayout(null, "Some reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void SuspendPayout_EmptyReason_ReturnsFalse()
        {
            var result = _service.SuspendPayout("MPO-000005", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void GetPayoutHistory_ValidPolicyWithHistory_ReturnsRecords()
        {
            _service.ProcessMaturityPayout("POL-HIST01", 100000m);
            _service.ProcessMaturityPayout("POL-HIST01", 200000m);
            var history = _service.GetPayoutHistory("POL-HIST01", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.IsTrue(history.Count >= 2);
            Assert.IsTrue(history.All(h => h.Success));
            Assert.IsNotNull(history);
            Assert.IsTrue(history.Count > 0);
        }

        [TestMethod]
        public void GetPayoutHistory_NoPriorPayouts_ReturnsEmpty()
        {
            var history = _service.GetPayoutHistory("POL-NOHIST", DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
            Assert.AreEqual(0, history.Count);
            Assert.IsNotNull(history);
            Assert.IsFalse(history.Any());
            Assert.IsTrue(history.Count == 0);
        }

        [TestMethod]
        public void GetPayoutHistory_NullPolicy_ReturnsEmpty()
        {
            var history = _service.GetPayoutHistory(null, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
            Assert.AreEqual(0, history.Count);
            Assert.IsNotNull(history);
            Assert.IsFalse(history.Any());
            Assert.IsTrue(history.Count == 0);
        }

        [TestMethod]
        public void CalculatePayoutTax_NegativeAmount_ReturnsZero()
        {
            var tax = _service.CalculatePayoutTax(-5000m, true);
            Assert.AreEqual(0m, tax);
            Assert.IsFalse(tax > 0);
            Assert.IsFalse(tax < 0);
            Assert.IsTrue(tax == 0m);
        }

        [TestMethod]
        public void CalculatePayoutTax_ExactThreshold_ReturnsZero()
        {
            var tax = _service.CalculatePayoutTax(100000m, true);
            Assert.AreEqual(0m, tax);
            Assert.IsFalse(tax > 0);
            Assert.IsFalse(tax < 0);
            Assert.IsTrue(tax == 0m);
        }

        [TestMethod]
        public void CalculatePayoutTax_JustAboveThreshold_ReturnsTax()
        {
            var tax = _service.CalculatePayoutTax(100001m, true);
            Assert.IsTrue(tax > 0);
            Assert.AreEqual(Math.Round(100001m * 0.05m, 2), tax);
            Assert.IsTrue(tax < 100001m);
            Assert.IsFalse(tax < 0);
        }

        [TestMethod]
        public void ValidatePayoutAmount_ExactMinimum_ReturnsTrue()
        {
            var valid = _service.ValidatePayoutAmount(1000m);
            Assert.IsTrue(valid);
            Assert.AreEqual(true, valid);
            Assert.IsFalse(valid == false);
            Assert.IsTrue(valid == true);
        }

        [TestMethod]
        public void ValidatePayoutAmount_ExactMaximum_ReturnsTrue()
        {
            var valid = _service.ValidatePayoutAmount(50000000m);
            Assert.IsTrue(valid);
            Assert.AreEqual(true, valid);
            Assert.IsFalse(valid == false);
            Assert.IsTrue(valid == true);
        }

        [TestMethod]
        public void ValidatePayoutAmount_JustBelowMinimum_ReturnsFalse()
        {
            var valid = _service.ValidatePayoutAmount(999m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }
    }
}
