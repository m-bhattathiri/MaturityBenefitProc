using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.NeftDisbursement;

namespace MaturityBenefitProc.Tests.Helpers.NeftDisbursement
{
    [TestClass]
    public class NeftDisbursementServiceValidationTests
    {
        private NeftDisbursementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new NeftDisbursementService();
        }

        [TestMethod]
        public void ProcessNeftPayment_NullClaimNumber_ReturnsFailed()
        {
            var result = _service.ProcessNeftPayment(null, "123456789", "SBIN0001234", 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessNeftPayment_EmptyClaimNumber_ReturnsFailed()
        {
            var result = _service.ProcessNeftPayment("", "123456789", "SBIN0001234", 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessNeftPayment_WhitespaceClaimNumber_ReturnsFailed()
        {
            var result = _service.ProcessNeftPayment("   ", "123456789", "SBIN0001234", 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessNeftPayment_InvalidAccount_ReturnsFailed()
        {
            var result = _service.ProcessNeftPayment("CLM001", "12345", "SBIN0001234", 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Invalid"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessNeftPayment_InvalidIfscCode_ReturnsFailed()
        {
            var result = _service.ProcessNeftPayment("CLM001", "123456789", "INVALID", 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Invalid"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessNeftPayment_ZeroAmount_ReturnsFailed()
        {
            var result = _service.ProcessNeftPayment("CLM001", "123456789", "SBIN0001234", 0m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Invalid") || result.Message.Contains("amount"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessNeftPayment_NegativeAmount_ReturnsFailed()
        {
            var result = _service.ProcessNeftPayment("CLM001", "123456789", "SBIN0001234", -1000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Invalid") || result.Message.Contains("amount"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessNeftPayment_ExceedsTransferLimit_ReturnsFailed()
        {
            var result = _service.ProcessNeftPayment("CLM001", "123456789", "SBIN0001234", 2000000000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Invalid") || result.Message.Contains("amount"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RetryNeftPayment_NullOriginalUtr_ReturnsFailed()
        {
            var result = _service.RetryNeftPayment(null, "timeout");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RetryNeftPayment_EmptyOriginalUtr_ReturnsFailed()
        {
            var result = _service.RetryNeftPayment("", "timeout");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RetryNeftPayment_NullReason_ReturnsFailed()
        {
            var result = _service.RetryNeftPayment("UTR001", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RetryNeftPayment_EmptyReason_ReturnsFailed()
        {
            var result = _service.RetryNeftPayment("UTR001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RetryNeftPayment_NonExistentUtr_ReturnsFailed()
        {
            var result = _service.RetryNeftPayment("NONEXISTENT", "timeout");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Original payment not found", result.Message);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void CancelNeftPayment_NullUtr_ReturnsFailed()
        {
            var result = _service.CancelNeftPayment(null, "duplicate");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CancelNeftPayment_EmptyUtr_ReturnsFailed()
        {
            var result = _service.CancelNeftPayment("", "duplicate");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CancelNeftPayment_NullReason_ReturnsFailed()
        {
            var result = _service.CancelNeftPayment("UTR001", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CancelNeftPayment_NonExistentUtr_ReturnsFailed()
        {
            var result = _service.CancelNeftPayment("NONEXISTENT", "duplicate");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Payment not found for cancellation", result.Message);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void SuspendNeftPayment_NullUtr_ReturnsFailed()
        {
            var result = _service.SuspendNeftPayment(null, "fraud");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SuspendNeftPayment_EmptyUtr_ReturnsFailed()
        {
            var result = _service.SuspendNeftPayment("", "fraud");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SuspendNeftPayment_NullReason_ReturnsFailed()
        {
            var result = _service.SuspendNeftPayment("UTR001", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SuspendNeftPayment_NonExistentUtr_ReturnsFailed()
        {
            var result = _service.SuspendNeftPayment("NONEXISTENT", "fraud");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Payment not found for suspension", result.Message);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void GetNeftPaymentDetails_NullUtr_ReturnsFailed()
        {
            var result = _service.GetNeftPaymentDetails(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNeftPaymentDetails_EmptyUtr_ReturnsFailed()
        {
            var result = _service.GetNeftPaymentDetails("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNeftPaymentDetails_NonExistentUtr_ReturnsFailed()
        {
            var result = _service.GetNeftPaymentDetails("NONEXISTENT");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Payment details not found", result.Message);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ValidateNeftAmount_MaxDecimalPrecision_ReturnsTrue()
        {
            var result = _service.ValidateNeftAmount(999999999.99m);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void ValidateNeftAmount_SmallFraction_ReturnsTrue()
        {
            var result = _service.ValidateNeftAmount(0.001m);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }
    }
}
