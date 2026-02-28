using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.UndeliveredPayment;

namespace MaturityBenefitProc.Tests.Helpers.UndeliveredPayment
{
    [TestClass]
    public class UndeliveredPaymentServiceValidationTests
    {
        private UndeliveredPaymentService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new UndeliveredPaymentService();
        }

        [TestMethod]
        public void InitiateRedispatch_NullRef_Fails()
        {
            var result = _service.InitiateRedispatch(null, "Speed");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InitiateRedispatch_EmptyRef_Fails()
        {
            var result = _service.InitiateRedispatch("", "Speed");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InitiateRedispatch_NullMode_Fails()
        {
            var result = _service.InitiateRedispatch("PAY001", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InitiateRedispatch_EmptyMode_Fails()
        {
            var result = _service.InitiateRedispatch("PAY001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConvertToNeft_NullRef_Fails()
        {
            var result = _service.ConvertToNeft(null, "123456789", "SBIN0001234");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConvertToNeft_NullAccount_Fails()
        {
            var result = _service.ConvertToNeft("PAY001", null, "SBIN0001234");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConvertToNeft_NullIfsc_Fails()
        {
            var result = _service.ConvertToNeft("PAY001", "123456789", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConvertToNeft_EmptyRef_Fails()
        {
            var result = _service.ConvertToNeft("", "123456789", "SBIN0001234");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConvertToNeft_EmptyAccount_Fails()
        {
            var result = _service.ConvertToNeft("PAY001", "", "SBIN0001234");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConvertToNeft_EmptyIfsc_Fails()
        {
            var result = _service.ConvertToNeft("PAY001", "123456789", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MarkAddressVerified_NullRef_Fails()
        {
            var result = _service.MarkAddressVerified(null, "Agent1");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MarkAddressVerified_NullVerifier_Fails()
        {
            var result = _service.MarkAddressVerified("PAY001", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MarkAddressVerified_EmptyRef_Fails()
        {
            var result = _service.MarkAddressVerified("", "Agent1");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MarkAddressVerified_EmptyVerifier_Fails()
        {
            var result = _service.MarkAddressVerified("PAY001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EscalateUndelivered_NullRef_Fails()
        {
            var result = _service.EscalateUndelivered(null, "Manager");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EscalateUndelivered_NullLevel_Fails()
        {
            var result = _service.EscalateUndelivered("PAY001", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EscalateUndelivered_EmptyRef_Fails()
        {
            var result = _service.EscalateUndelivered("", "Manager");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EscalateUndelivered_EmptyLevel_Fails()
        {
            var result = _service.EscalateUndelivered("PAY001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CancelRedispatch_NullRef_Fails()
        {
            var result = _service.CancelRedispatch(null, "reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CancelRedispatch_NullReason_Fails()
        {
            var result = _service.CancelRedispatch("PAY001", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CancelRedispatch_EmptyRef_Fails()
        {
            var result = _service.CancelRedispatch("", "reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CancelRedispatch_EmptyReason_Fails()
        {
            var result = _service.CancelRedispatch("PAY001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUndeliveredPaymentHistory_NullCif_ReturnsEmpty()
        {
            var result = _service.GetUndeliveredPaymentHistory(null, DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<UndeliveredPaymentResult>);
        }

        [TestMethod]
        public void GetUndeliveredPaymentHistory_EmptyCif_ReturnsEmpty()
        {
            var result = _service.GetUndeliveredPaymentHistory("", DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<UndeliveredPaymentResult>);
        }

        [TestMethod]
        public void GetUndeliveredPaymentHistory_NonExistentCif_ReturnsEmpty()
        {
            var result = _service.GetUndeliveredPaymentHistory("NONEXISTENT", DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<UndeliveredPaymentResult>);
        }
    }
}
