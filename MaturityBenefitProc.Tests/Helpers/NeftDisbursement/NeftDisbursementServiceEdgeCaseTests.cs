using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.NeftDisbursement;

namespace MaturityBenefitProc.Tests.Helpers.NeftDisbursement
{
    [TestClass]
    public class NeftDisbursementServiceEdgeCaseTests
    {
        private NeftDisbursementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new NeftDisbursementService();
        }

        [TestMethod]
        public void ValidateIfscCode_LowercaseLetters_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("sbin0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_MixedCaseLetters_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("SbIn0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_MissingZeroAtFifth_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("SBIN1001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_TooShort_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("SBIN012");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_TooLong_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("SBIN000123456");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_SpecialChars_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("SBI@0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_WhitespaceOnly_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("   ");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_ExactlyNineDigits_ReturnsTrue()
        {
            var result = _service.ValidateBankAccount("123456789", "SBIN0001234");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_ExactlyEighteenDigits_ReturnsTrue()
        {
            var result = _service.ValidateBankAccount("123456789012345678", "SBIN0001234");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_WhitespaceOnly_ReturnsFalse()
        {
            var result = _service.ValidateBankAccount("   ", "SBIN0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_MixedAlphaNumeric_ReturnsFalse()
        {
            var result = _service.ValidateBankAccount("12345ABCD", "SBIN0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void GetNeftCharges_ExactlyAtBoundary10001_Returns5()
        {
            var result = _service.GetNeftCharges(10001m);
            Assert.AreEqual(5m, result);
            Assert.IsTrue(result > 2.50m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result <= 0);
        }

        [TestMethod]
        public void GetNeftCharges_ExactlyAtBoundary100001_Returns15()
        {
            var result = _service.GetNeftCharges(100001m);
            Assert.AreEqual(15m, result);
            Assert.IsTrue(result > 5m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result <= 0);
        }

        [TestMethod]
        public void GetNeftCharges_ExactlyAtBoundary200001_Returns25()
        {
            var result = _service.GetNeftCharges(200001m);
            Assert.AreEqual(25m, result);
            Assert.IsTrue(result > 15m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result <= 0);
        }

        [TestMethod]
        public void GetNeftCharges_OneRupee_Returns250Paise()
        {
            var result = _service.GetNeftCharges(1m);
            Assert.AreEqual(2.50m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void IsWithinNeftWindow_ExactlyAt8AM_ReturnsTrue()
        {
            var time = new DateTime(2017, 6, 15, 8, 0, 0);
            var result = _service.IsWithinNeftWindow(time);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsWithinNeftWindow_ExactlyAt7PM_ReturnsFalse()
        {
            var time = new DateTime(2017, 6, 15, 19, 0, 0);
            var result = _service.IsWithinNeftWindow(time);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsWithinNeftWindow_JustBefore7PM_ReturnsTrue()
        {
            var time = new DateTime(2017, 6, 15, 18, 59, 59);
            var result = _service.IsWithinNeftWindow(time);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsWithinNeftWindow_Midnight_ReturnsFalse()
        {
            var time = new DateTime(2017, 6, 15, 0, 0, 0);
            var result = _service.IsWithinNeftWindow(time);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateNeftAmount_OneRupee_ReturnsTrue()
        {
            var result = _service.ValidateNeftAmount(1m);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void ValidateNeftAmount_VerySmallDecimal_ReturnsTrue()
        {
            var result = _service.ValidateNeftAmount(0.01m);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void ValidateNeftAmount_JustOverLimit_ReturnsFalse()
        {
            var result = _service.ValidateNeftAmount(1000000000.01m);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void GenerateUtrNumber_EmptyBankCode_ReturnsEmpty()
        {
            var result = _service.GenerateUtrNumber("", DateTime.UtcNow);
            Assert.AreEqual(string.Empty, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length == 0);
            Assert.IsFalse(result.Contains("UTR"));
        }

        [TestMethod]
        public void GenerateUtrNumber_WhitespaceBankCode_ReturnsEmpty()
        {
            var result = _service.GenerateUtrNumber("   ", DateTime.UtcNow);
            Assert.AreEqual(string.Empty, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length == 0);
            Assert.IsFalse(result.StartsWith("UTR"));
        }

        [TestMethod]
        public void GenerateUtrNumber_DifferentDates_DifferentUtrs()
        {
            var utr1 = _service.GenerateUtrNumber("SBIN", new DateTime(2017, 1, 1));
            var utr2 = _service.GenerateUtrNumber("SBIN", new DateTime(2017, 12, 31));
            Assert.AreNotEqual(utr1, utr2);
            Assert.IsTrue(utr1.Contains("20170101"));
            Assert.IsTrue(utr2.Contains("20171231"));
            Assert.IsNotNull(utr1);
        }

        [TestMethod]
        public void GetNeftPaymentStatus_NullUtr_Fails()
        {
            var result = _service.GetNeftPaymentStatus(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNeftPaymentStatus_EmptyUtr_Fails()
        {
            var result = _service.GetNeftPaymentStatus("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNeftPaymentStatus_NonExistentUtr_Fails()
        {
            var result = _service.GetNeftPaymentStatus("NONEXISTENT");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Payment not found", result.Message);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void GetNeftPaymentHistory_EmptyClaimNumber_ReturnsEmptyList()
        {
            var result = _service.GetNeftPaymentHistory("", DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<NeftDisbursementResult>);
        }

        [TestMethod]
        public void GetNeftPaymentHistory_NullClaimNumber_ReturnsEmptyList()
        {
            var result = _service.GetNeftPaymentHistory(null, DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<NeftDisbursementResult>);
        }
    }
}
