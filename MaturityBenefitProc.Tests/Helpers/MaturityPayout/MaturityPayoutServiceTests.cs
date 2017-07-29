using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.MaturityPayout;

namespace MaturityBenefitProc.Tests.Helpers.MaturityPayout
{
    [TestClass]
    public class MaturityPayoutServiceTests
    {
        private MaturityPayoutService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new MaturityPayoutService();
        }

        [TestMethod]
        public void ProcessMaturityPayout_ValidPolicy_ReturnsSuccess()
        {
            var result = _service.ProcessMaturityPayout("POL-100001", 500000m);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.ReferenceId);
            Assert.AreEqual(500000m, result.Amount);
            Assert.IsTrue(result.ReferenceId.StartsWith("MPO-"));
        }

        [TestMethod]
        public void ProcessMaturityPayout_LargeAmount_ProcessesCorrectly()
        {
            var result = _service.ProcessMaturityPayout("POL-200001", 10000000m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(10000000m, result.GrossAmount);
            Assert.IsTrue(result.TdsDeducted > 0);
            Assert.IsTrue(result.NetPayable < result.GrossAmount);
        }

        [TestMethod]
        public void ProcessMaturityPayout_NullPolicyNumber_ReturnsFalse()
        {
            var result = _service.ProcessMaturityPayout(null, 500000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(string.Empty, result.ReferenceId);
        }

        [TestMethod]
        public void ProcessMaturityPayout_EmptyPolicyNumber_ReturnsFalse()
        {
            var result = _service.ProcessMaturityPayout("", 500000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(string.Empty, result.ReferenceId);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void ProcessMaturityPayout_ZeroAmount_ReturnsFalse()
        {
            var result = _service.ProcessMaturityPayout("POL-100002", 0m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(string.Empty, result.ReferenceId);
        }

        [TestMethod]
        public void ProcessMaturityPayout_NegativeAmount_ReturnsFalse()
        {
            var result = _service.ProcessMaturityPayout("POL-100003", -5000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(string.Empty, result.ReferenceId);
        }

        [TestMethod]
        public void ProcessMaturityPayout_SetsPaymentMode()
        {
            var result = _service.ProcessMaturityPayout("POL-100004", 250000m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("NEFT", result.PaymentMode);
            Assert.IsNotNull(result.PaymentMode);
            Assert.IsTrue(result.PaymentMode.Length > 0);
        }

        [TestMethod]
        public void ProcessMaturityPayout_CalculatesNetPayable()
        {
            var result = _service.ProcessMaturityPayout("POL-100005", 200000m);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.NetPayable > 0);
            Assert.IsTrue(result.NetPayable <= result.GrossAmount);
            Assert.AreEqual(result.GrossAmount - result.TdsDeducted, result.NetPayable);
        }

        [TestMethod]
        public void ValidateMaturityPayout_ValidPolicy_ReturnsSuccess()
        {
            var result = _service.ValidateMaturityPayout("POL-300001");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.Message.Contains("POL-300001"));
        }

        [TestMethod]
        public void ValidateMaturityPayout_NullPolicy_ReturnsFalse()
        {
            var result = _service.ValidateMaturityPayout(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void ValidateMaturityPayout_EmptyPolicy_ReturnsFalse()
        {
            var result = _service.ValidateMaturityPayout("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Length > 0);
            Assert.IsTrue(result.Message.Contains("required"));
        }

        [TestMethod]
        public void CalculateMaturityAmount_AllPositive_ReturnsSum()
        {
            var result = _service.CalculateMaturityAmount(500000m, 100000m, 50000m, 25000m);
            Assert.AreEqual(675000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result >= 500000m);
            Assert.AreEqual(500000m + 100000m + 50000m + 25000m, result);
        }

        [TestMethod]
        public void CalculateMaturityAmount_ZeroBonus_ReturnsSumAssured()
        {
            var result = _service.CalculateMaturityAmount(500000m, 0m, 0m, 0m);
            Assert.AreEqual(500000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result == 500000m);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateMaturityAmount_ZeroSumAssured_ReturnsZero()
        {
            var result = _service.CalculateMaturityAmount(0m, 50000m, 10000m, 5000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateNetPayableAmount_WithDeductions_ReturnsNet()
        {
            var result = _service.CalculateNetPayableAmount(500000m, 25000m, 5000m);
            Assert.AreEqual(470000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 500000m);
            Assert.AreEqual(500000m - 25000m - 5000m, result);
        }

        [TestMethod]
        public void CalculateNetPayableAmount_ZeroGross_ReturnsZero()
        {
            var result = _service.CalculateNetPayableAmount(0m, 1000m, 500m);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsFalse(result < 0);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateNetPayableAmount_DeductionsExceedGross_ReturnsZero()
        {
            var result = _service.CalculateNetPayableAmount(10000m, 8000m, 5000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0m);
            Assert.IsFalse(result < 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void GetPayoutDetails_ExistingPayout_ReturnsDetails()
        {
            var payout = _service.ProcessMaturityPayout("POL-400001", 300000m);
            var result = _service.GetPayoutDetails(payout.ReferenceId);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(payout.ReferenceId, result.ReferenceId);
            Assert.AreEqual(300000m, result.Amount);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void GetPayoutDetails_NonExistent_ReturnsFalse()
        {
            var result = _service.GetPayoutDetails("NONEXISTENT-001");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("not found"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void GetPayoutDetails_NullReference_ReturnsFalse()
        {
            var result = _service.GetPayoutDetails(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void IsPayoutEligible_ValidPolicy_ReturnsTrue()
        {
            var eligible = _service.IsPayoutEligible("POL-500001");
            Assert.IsTrue(eligible);
            Assert.IsTrue(eligible == true);
            Assert.IsFalse(eligible == false);
            Assert.AreEqual(true, eligible);
        }

        [TestMethod]
        public void IsPayoutEligible_NullPolicy_ReturnsFalse()
        {
            var eligible = _service.IsPayoutEligible(null);
            Assert.IsFalse(eligible);
            Assert.AreEqual(false, eligible);
            Assert.IsTrue(eligible == false);
            Assert.IsFalse(eligible == true);
        }

        [TestMethod]
        public void IsPayoutEligible_ShortPolicy_ReturnsFalse()
        {
            var eligible = _service.IsPayoutEligible("POL");
            Assert.IsFalse(eligible);
            Assert.AreEqual(false, eligible);
            Assert.IsTrue(eligible == false);
            Assert.IsFalse(eligible == true);
        }

        [TestMethod]
        public void GetMaximumPayoutAmount_ReturnsExpectedValue()
        {
            var max = _service.GetMaximumPayoutAmount();
            Assert.AreEqual(50000000m, max);
            Assert.IsTrue(max > 0);
            Assert.IsTrue(max > 1000m);
            Assert.IsTrue(max >= 50000000m);
        }

        [TestMethod]
        public void GetMinimumPayoutAmount_ReturnsExpectedValue()
        {
            var min = _service.GetMinimumPayoutAmount();
            Assert.AreEqual(1000m, min);
            Assert.IsTrue(min > 0);
            Assert.IsTrue(min < 50000000m);
            Assert.IsTrue(min <= 1000m);
        }

        [TestMethod]
        public void ValidatePayoutAmount_WithinRange_ReturnsTrue()
        {
            var valid = _service.ValidatePayoutAmount(500000m);
            Assert.IsTrue(valid);
            Assert.AreEqual(true, valid);
            Assert.IsFalse(valid == false);
            Assert.IsTrue(valid == true);
        }

        [TestMethod]
        public void ValidatePayoutAmount_BelowMinimum_ReturnsFalse()
        {
            var valid = _service.ValidatePayoutAmount(500m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }

        [TestMethod]
        public void ValidatePayoutAmount_AboveMaximum_ReturnsFalse()
        {
            var valid = _service.ValidatePayoutAmount(60000000m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }

        [TestMethod]
        public void CalculatePayoutTax_WithPanCard_LowAmount_ReturnsZero()
        {
            var tax = _service.CalculatePayoutTax(50000m, true);
            Assert.AreEqual(0m, tax);
            Assert.IsTrue(tax >= 0m);
            Assert.IsFalse(tax > 0);
            Assert.IsFalse(tax < 0);
        }

        [TestMethod]
        public void CalculatePayoutTax_WithPanCard_HighAmount_ReturnsFivePercent()
        {
            var tax = _service.CalculatePayoutTax(200000m, true);
            Assert.AreEqual(10000m, tax);
            Assert.IsTrue(tax > 0);
            Assert.IsTrue(tax < 200000m);
            Assert.AreEqual(200000m * 0.05m, tax);
        }

        [TestMethod]
        public void CalculatePayoutTax_WithoutPanCard_ReturnsTwentyPercent()
        {
            var tax = _service.CalculatePayoutTax(200000m, false);
            Assert.AreEqual(40000m, tax);
            Assert.IsTrue(tax > 0);
            Assert.IsTrue(tax < 200000m);
            Assert.AreEqual(200000m * 0.20m, tax);
        }

        [TestMethod]
        public void CalculatePayoutTax_ZeroAmount_ReturnsZero()
        {
            var tax = _service.CalculatePayoutTax(0m, true);
            Assert.AreEqual(0m, tax);
            Assert.IsFalse(tax > 0);
            Assert.IsFalse(tax < 0);
            Assert.IsTrue(tax == 0m);
        }
    }
}
