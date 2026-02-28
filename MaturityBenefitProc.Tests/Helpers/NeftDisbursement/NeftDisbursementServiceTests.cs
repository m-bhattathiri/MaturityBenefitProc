using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.NeftDisbursement;

namespace MaturityBenefitProc.Tests.Helpers.NeftDisbursement
{
    [TestClass]
    public class NeftDisbursementServiceTests
    {
        private NeftDisbursementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new NeftDisbursementService();
        }

        [TestMethod]
        public void ValidateIfscCode_ValidCode_ReturnsTrue()
        {
            var result = _service.ValidateIfscCode("SBIN0001234");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_AnotherValidCode_ReturnsTrue()
        {
            var result = _service.ValidateIfscCode("HDFC0BRANCH");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_NullInput_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode(null);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_EmptyString_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_InvalidFormat_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("INVALID");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_ValidAccount_ReturnsTrue()
        {
            var result = _service.ValidateBankAccount("123456789", "SBIN0001234");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_TooShort_ReturnsFalse()
        {
            var result = _service.ValidateBankAccount("12345678", "SBIN0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_TooLong_ReturnsFalse()
        {
            var result = _service.ValidateBankAccount("1234567890123456789", "SBIN0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_NonDigits_ReturnsFalse()
        {
            var result = _service.ValidateBankAccount("12345ABC9", "SBIN0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_NullAccount_ReturnsFalse()
        {
            var result = _service.ValidateBankAccount(null, "SBIN0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void GetNeftTransferLimit_ReturnsOneBillion()
        {
            var result = _service.GetNeftTransferLimit();
            Assert.AreEqual(1000000000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreEqual(1000000000m, result);
        }

        [TestMethod]
        public void GetNeftCharges_SmallAmount_Returns250Paise()
        {
            var result = _service.GetNeftCharges(5000m);
            Assert.AreEqual(2.50m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 5m);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNeftCharges_MediumAmount_Returns5()
        {
            var result = _service.GetNeftCharges(50000m);
            Assert.AreEqual(5m, result);
            Assert.IsTrue(result > 2.50m);
            Assert.IsTrue(result <= 5m);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNeftCharges_LargeAmount_Returns15()
        {
            var result = _service.GetNeftCharges(150000m);
            Assert.AreEqual(15m, result);
            Assert.IsTrue(result > 5m);
            Assert.IsTrue(result <= 15m);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNeftCharges_VeryLargeAmount_Returns25()
        {
            var result = _service.GetNeftCharges(500000m);
            Assert.AreEqual(25m, result);
            Assert.IsTrue(result > 15m);
            Assert.IsTrue(result <= 25m);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNeftCharges_Boundary10000_Returns250()
        {
            var result = _service.GetNeftCharges(10000m);
            Assert.AreEqual(2.50m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetNeftCharges_Boundary100000_Returns5()
        {
            var result = _service.GetNeftCharges(100000m);
            Assert.AreEqual(5m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetNeftCharges_Boundary200000_Returns15()
        {
            var result = _service.GetNeftCharges(200000m);
            Assert.AreEqual(15m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void ValidateNeftAmount_ValidAmount_ReturnsTrue()
        {
            var result = _service.ValidateNeftAmount(50000m);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void ValidateNeftAmount_ZeroAmount_ReturnsFalse()
        {
            var result = _service.ValidateNeftAmount(0m);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateNeftAmount_NegativeAmount_ReturnsFalse()
        {
            var result = _service.ValidateNeftAmount(-100m);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateNeftAmount_ExceedsLimit_ReturnsFalse()
        {
            var result = _service.ValidateNeftAmount(1000000001m);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateNeftAmount_AtLimit_ReturnsTrue()
        {
            var result = _service.ValidateNeftAmount(1000000000m);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsWithinNeftWindow_DuringWindow_ReturnsTrue()
        {
            var time = new DateTime(2017, 6, 15, 10, 30, 0);
            var result = _service.IsWithinNeftWindow(time);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsWithinNeftWindow_BeforeWindow_ReturnsFalse()
        {
            var time = new DateTime(2017, 6, 15, 7, 59, 0);
            var result = _service.IsWithinNeftWindow(time);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsWithinNeftWindow_AfterWindow_ReturnsFalse()
        {
            var time = new DateTime(2017, 6, 15, 19, 0, 0);
            var result = _service.IsWithinNeftWindow(time);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void GenerateUtrNumber_ValidInputs_ReturnsUtr()
        {
            var date = new DateTime(2017, 6, 15);
            var result = _service.GenerateUtrNumber("SBIN", date);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("UTR"));
            Assert.IsTrue(result.Contains("SBIN"));
            Assert.IsTrue(result.Contains("20170615"));
        }

        [TestMethod]
        public void GenerateUtrNumber_NullBankCode_ReturnsEmpty()
        {
            var result = _service.GenerateUtrNumber(null, DateTime.UtcNow);
            Assert.AreEqual(string.Empty, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length == 0);
            Assert.IsFalse(result.StartsWith("UTR"));
        }

        [TestMethod]
        public void GenerateUtrNumber_SequentialCalls_DifferentUtrs()
        {
            var date = new DateTime(2017, 6, 15);
            var utr1 = _service.GenerateUtrNumber("SBIN", date);
            var utr2 = _service.GenerateUtrNumber("SBIN", date);
            Assert.AreNotEqual(utr1, utr2);
            Assert.IsNotNull(utr1);
            Assert.IsNotNull(utr2);
            Assert.IsTrue(utr1.StartsWith("UTR"));
        }

        [TestMethod]
        public void ValidateNeftPayment_NullClaimNumber_Fails()
        {
            var result = _service.ValidateNeftPayment(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual("Claim number is required", result.Message);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateNeftPayment_EmptyClaim_Fails()
        {
            var result = _service.ValidateNeftPayment("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateNeftPayment_ValidClaimNoPayments_Success()
        {
            var result = _service.ValidateNeftPayment("CLM001");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual("CLM001", result.ReferenceId);
            Assert.IsNotNull(result);
        }
    }
}
