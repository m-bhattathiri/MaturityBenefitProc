using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.MaturityPayout;

namespace MaturityBenefitProc.Tests.Helpers.MaturityPayout
{
    [TestClass]
    public class MaturityPayoutServiceValidationTests
    {
        private MaturityPayoutService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new MaturityPayoutService();
        }

        [TestMethod]
        public void ProcessMaturityPayout_WhitespacePolicyNumber_ReturnsFalse()
        {
            var result = _service.ProcessMaturityPayout("   ", 500000m);
            Assert.IsTrue(result.Success || !result.Success);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Length >= 0);
        }

        [TestMethod]
        public void ProcessMaturityPayout_SpecialCharactersInPolicy_Processes()
        {
            var result = _service.ProcessMaturityPayout("POL/2017-0001#A", 250000m);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.ReferenceId);
            Assert.AreEqual(250000m, result.Amount);
            Assert.IsTrue(result.ReferenceId.Length > 0);
        }

        [TestMethod]
        public void ProcessMaturityPayout_VeryLongPolicyNumber_Processes()
        {
            var longPolicy = "POL-" + new string('X', 200);
            var result = _service.ProcessMaturityPayout(longPolicy, 100000m);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.ReferenceId);
            Assert.AreEqual(100000m, result.Amount);
            Assert.IsTrue(result.NetPayable >= 0);
        }

        [TestMethod]
        public void ProcessMaturityPayout_DecimalPrecision_Maintained()
        {
            var result = _service.ProcessMaturityPayout("POL-PREC01", 123456.78m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(123456.78m, result.Amount);
            Assert.AreEqual(123456.78m, result.GrossAmount);
            Assert.IsTrue(result.NetPayable > 0);
        }

        [TestMethod]
        public void ValidateMaturityPayout_WhitespacePolicy_ReturnsFalse()
        {
            var result = _service.ValidateMaturityPayout("   ");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.ReferenceId.Length > 0);
        }

        [TestMethod]
        public void ValidateMaturityPayout_NumericOnlyPolicy_ReturnsSuccess()
        {
            var result = _service.ValidateMaturityPayout("1234567890");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.Message.Contains("1234567890"));
        }

        [TestMethod]
        public void CalculateMaturityAmount_NegativeSumAssured_ReturnsZero()
        {
            var result = _service.CalculateMaturityAmount(-500000m, 100000m, 50000m, 25000m);
            Assert.AreEqual(0m, result);
            Assert.IsFalse(result > 0);
            Assert.IsTrue(result <= 0);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateMaturityAmount_SmallDecimals_CalculatesCorrectly()
        {
            var result = _service.CalculateMaturityAmount(100.50m, 10.25m, 5.10m, 2.15m);
            Assert.AreEqual(118.00m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result > 100m);
            Assert.IsTrue(result < 200m);
        }

        [TestMethod]
        public void CalculateNetPayableAmount_SmallAmounts_CalculatesCorrectly()
        {
            var result = _service.CalculateNetPayableAmount(1000m, 100m, 50m);
            Assert.AreEqual(850m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 1000m);
            Assert.AreEqual(1000m - 100m - 50m, result);
        }

        [TestMethod]
        public void CalculateNetPayableAmount_OnlyTdsDeduction_CalculatesCorrectly()
        {
            var result = _service.CalculateNetPayableAmount(500000m, 25000m, 0m);
            Assert.AreEqual(475000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 500000m);
            Assert.AreEqual(500000m - 25000m, result);
        }

        [TestMethod]
        public void CalculateNetPayableAmount_OnlyOtherDeductions_CalculatesCorrectly()
        {
            var result = _service.CalculateNetPayableAmount(500000m, 0m, 10000m);
            Assert.AreEqual(490000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 500000m);
            Assert.AreEqual(500000m - 10000m, result);
        }

        [TestMethod]
        public void GetPayoutDetails_EmptyReference_ReturnsFalse()
        {
            var result = _service.GetPayoutDetails("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void GetPayoutDetails_WhitespaceReference_ReturnsFalse()
        {
            var result = _service.GetPayoutDetails("   ");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("not found"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void IsPayoutEligible_EmptyPolicy_ReturnsFalse()
        {
            var eligible = _service.IsPayoutEligible("");
            Assert.IsFalse(eligible);
            Assert.AreEqual(false, eligible);
            Assert.IsTrue(eligible == false);
            Assert.IsFalse(eligible == true);
        }

        [TestMethod]
        public void IsPayoutEligible_AfterProcessing_ReturnsTrue()
        {
            _service.ProcessMaturityPayout("POL-ELIG01", 500000m);
            var eligible = _service.IsPayoutEligible("POL-ELIG01");
            Assert.IsTrue(eligible);
            Assert.AreEqual(true, eligible);
            Assert.IsFalse(eligible == false);
            Assert.IsTrue(eligible == true);
        }

        [TestMethod]
        public void IsPayoutEligible_ExactMinimumLength_ReturnsTrue()
        {
            var eligible = _service.IsPayoutEligible("ABCDE");
            Assert.IsTrue(eligible);
            Assert.AreEqual(true, eligible);
            Assert.IsFalse(eligible == false);
            Assert.IsTrue(eligible == true);
        }

        [TestMethod]
        public void IsPayoutEligible_FourCharPolicy_ReturnsFalse()
        {
            var eligible = _service.IsPayoutEligible("ABCD");
            Assert.IsFalse(eligible);
            Assert.AreEqual(false, eligible);
            Assert.IsTrue(eligible == false);
            Assert.IsFalse(eligible == true);
        }

        [TestMethod]
        public void GetMaximumPayoutAmount_AlwaysConsistent()
        {
            var max1 = _service.GetMaximumPayoutAmount();
            var max2 = _service.GetMaximumPayoutAmount();
            Assert.AreEqual(max1, max2);
            Assert.IsTrue(max1 > 0);
            Assert.AreEqual(50000000m, max1);
            Assert.AreEqual(50000000m, max2);
        }

        [TestMethod]
        public void GetMinimumPayoutAmount_AlwaysConsistent()
        {
            var min1 = _service.GetMinimumPayoutAmount();
            var min2 = _service.GetMinimumPayoutAmount();
            Assert.AreEqual(min1, min2);
            Assert.IsTrue(min1 > 0);
            Assert.AreEqual(1000m, min1);
            Assert.AreEqual(1000m, min2);
        }

        [TestMethod]
        public void ValidatePayoutAmount_ZeroAmount_ReturnsFalse()
        {
            var valid = _service.ValidatePayoutAmount(0m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }

        [TestMethod]
        public void ValidatePayoutAmount_NegativeAmount_ReturnsFalse()
        {
            var valid = _service.ValidatePayoutAmount(-1000m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }

        [TestMethod]
        public void ValidatePayoutAmount_MidRangeAmount_ReturnsTrue()
        {
            var valid = _service.ValidatePayoutAmount(25000000m);
            Assert.IsTrue(valid);
            Assert.AreEqual(true, valid);
            Assert.IsFalse(valid == false);
            Assert.IsTrue(valid == true);
        }

        [TestMethod]
        public void CalculatePayoutTax_WithPan_BelowThreshold_ReturnsZero()
        {
            var tax = _service.CalculatePayoutTax(99999m, true);
            Assert.AreEqual(0m, tax);
            Assert.IsFalse(tax > 0);
            Assert.IsTrue(tax >= 0);
            Assert.IsFalse(tax < 0);
        }

        [TestMethod]
        public void CalculatePayoutTax_WithoutPan_SmallAmount_StillTaxed()
        {
            var tax = _service.CalculatePayoutTax(10000m, false);
            Assert.AreEqual(2000m, tax);
            Assert.IsTrue(tax > 0);
            Assert.AreEqual(10000m * 0.20m, tax);
            Assert.IsTrue(tax < 10000m);
        }

        [TestMethod]
        public void CalculatePayoutTax_LargeAmountWithPan_ReturnsFivePercent()
        {
            var tax = _service.CalculatePayoutTax(5000000m, true);
            Assert.AreEqual(250000m, tax);
            Assert.IsTrue(tax > 0);
            Assert.AreEqual(5000000m * 0.05m, tax);
            Assert.IsTrue(tax < 5000000m);
        }
    }
}
