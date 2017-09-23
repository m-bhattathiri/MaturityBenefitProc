using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.ChequeDispatch;

namespace MaturityBenefitProc.Tests.Helpers.ChequeDispatch
{
    [TestClass]
    public class ChequeDispatchServiceEdgeCaseTests
    {
        private ChequeDispatchService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new ChequeDispatchService();
        }

        [TestMethod]
        public void ProcessChequeDispatch_MinAmount_Succeeds()
        {
            var result = _service.ProcessChequeDispatch("CLM-E01", "Payee", 0.01m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0.01m, result.Amount);
            Assert.IsNotNull(result.ChequeNumber);
            Assert.IsNotNull(result.AwbNumber);
        }

        [TestMethod]
        public void ProcessChequeDispatch_LargeAmount_Succeeds()
        {
            var result = _service.ProcessChequeDispatch("CLM-E02", "Payee", 9999999m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(9999999m, result.Amount);
            Assert.IsNotNull(result.ChequeNumber);
            Assert.IsTrue(result.ChequeNumber.Length > 0);
        }

        [TestMethod]
        public void ProcessChequeDispatch_NullPayeeName_ReturnsFalse()
        {
            var result = _service.ProcessChequeDispatch("CLM-E03", null, 500000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(string.Empty, result.ReferenceId);
        }

        [TestMethod]
        public void ProcessChequeDispatch_EmptyPayeeName_ReturnsFalse()
        {
            var result = _service.ProcessChequeDispatch("CLM-E04", "", 500000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(string.Empty, result.ReferenceId);
        }

        [TestMethod]
        public void ProcessChequeDispatch_NegativeAmount_ReturnsFalse()
        {
            var result = _service.ProcessChequeDispatch("CLM-E05", "Payee", -1000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(string.Empty, result.ReferenceId);
        }

        [TestMethod]
        public void MarkChequeDelivered_ExistingCheque_UpdatesStatus()
        {
            var dispatched = _service.ProcessChequeDispatch("CLM-E06", "Payee", 300000m);
            var delivered = _service.MarkChequeDelivered(dispatched.ChequeNumber, DateTime.UtcNow);
            Assert.IsTrue(delivered.Success);
            Assert.AreEqual("Delivered", delivered.DeliveryStatus);
            Assert.IsNotNull(delivered.Message);
            Assert.IsTrue(delivered.Amount > 0);
        }

        [TestMethod]
        public void MarkChequeDelivered_NonExistentCheque_StillSucceeds()
        {
            var result = _service.MarkChequeDelivered("NONEXISTENT", DateTime.UtcNow);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Delivered", result.DeliveryStatus);
            Assert.AreEqual("NONEXISTENT", result.ChequeNumber);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void MarkChequeDelivered_NullCheque_ReturnsFalse()
        {
            var result = _service.MarkChequeDelivered(null, DateTime.UtcNow);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void HandleReturnedCheque_ValidInputs_ReturnsSuccess()
        {
            var result = _service.HandleReturnedCheque("MBP-001", "Address not found");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Returned", result.DeliveryStatus);
            Assert.AreEqual("MBP-001", result.ChequeNumber);
            Assert.IsTrue(result.Message.Contains("returned"));
        }

        [TestMethod]
        public void HandleReturnedCheque_NullCheque_ReturnsFalse()
        {
            var result = _service.HandleReturnedCheque(null, "Reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void HandleReturnedCheque_EmptyReason_ReturnsFalse()
        {
            var result = _service.HandleReturnedCheque("MBP-002", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void ReissueCheque_ValidInputs_ReturnsNewCheque()
        {
            var result = _service.ReissueCheque("MBP-ORIG001", "Expired cheque");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.ChequeNumber);
            Assert.IsTrue(result.ChequeNumber.Contains("REISSUE"));
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void ReissueCheque_NullOriginal_ReturnsFalse()
        {
            var result = _service.ReissueCheque(null, "Reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void ReissueCheque_EmptyReason_ReturnsFalse()
        {
            var result = _service.ReissueCheque("MBP-ORIG002", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void GetChequeDetails_ExistingCheque_ReturnsDetails()
        {
            var dispatched = _service.ProcessChequeDispatch("CLM-E07", "Payee Name", 400000m);
            var details = _service.GetChequeDetails(dispatched.ChequeNumber);
            Assert.IsTrue(details.Success);
            Assert.AreEqual(dispatched.ChequeNumber, details.ChequeNumber);
            Assert.AreEqual("Payee Name", details.PayeeName);
            Assert.AreEqual(400000m, details.Amount);
        }

        [TestMethod]
        public void GetChequeDetails_NullCheque_ReturnsFalse()
        {
            var result = _service.GetChequeDetails(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void GetChequeDetails_NonExistent_ReturnsFalse()
        {
            var result = _service.GetChequeDetails("NONEXISTENT-CHQ");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("not found"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void CancelCheque_ValidInputs_ReturnsSuccess()
        {
            var result = _service.CancelCheque("MBP-003", "Customer requested");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Cancelled", result.DeliveryStatus);
            Assert.AreEqual("MBP-003", result.ChequeNumber);
            Assert.IsTrue(result.Message.Contains("cancelled"));
        }

        [TestMethod]
        public void CancelCheque_NullCheque_ReturnsFalse()
        {
            var result = _service.CancelCheque(null, "Reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void CancelCheque_NullReason_ReturnsFalse()
        {
            var result = _service.CancelCheque("MBP-004", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void ValidateChequeAmount_ExactMax_ReturnsTrue()
        {
            var valid = _service.ValidateChequeAmount(10000000m);
            Assert.IsTrue(valid);
            Assert.AreEqual(true, valid);
            Assert.IsFalse(valid == false);
            Assert.IsTrue(valid == true);
        }

        [TestMethod]
        public void ValidateChequeAmount_AboveMax_ReturnsFalse()
        {
            var valid = _service.ValidateChequeAmount(10000001m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }

        [TestMethod]
        public void ValidateChequeAmount_NegativeAmount_ReturnsFalse()
        {
            var valid = _service.ValidateChequeAmount(-1000m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }

        [TestMethod]
        public void GetCourierPartner_EastPincode_ReturnsIndiaPost()
        {
            var partner = _service.GetCourierPartner("700001");
            Assert.AreEqual("IndiaPost", partner);
            Assert.IsNotNull(partner);
            Assert.IsTrue(partner.Length > 0);
            Assert.IsTrue(partner == "IndiaPost");
        }

        [TestMethod]
        public void GetCourierPartner_EmptyPincode_ReturnsEmpty()
        {
            var partner = _service.GetCourierPartner("");
            Assert.AreEqual(string.Empty, partner);
            Assert.IsNotNull(partner);
            Assert.IsTrue(partner.Length == 0);
            Assert.IsFalse(partner.Length > 0);
        }

        [TestMethod]
        public void GetChequeDispatchCharges_UnknownMode_ReturnsDefault()
        {
            var charges = _service.GetChequeDispatchCharges("UNKNOWN");
            Assert.AreEqual(25m, charges);
            Assert.IsTrue(charges > 0);
            Assert.IsTrue(charges == 25m);
            Assert.IsFalse(charges < 0);
        }
    }
}
