using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.ChequeDispatch;

namespace MaturityBenefitProc.Tests.Helpers.ChequeDispatch
{
    [TestClass]
    public class ChequeDispatchServiceTests
    {
        private ChequeDispatchService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new ChequeDispatchService();
        }

        [TestMethod]
        public void ProcessChequeDispatch_ValidInputs_ReturnsSuccess()
        {
            var result = _service.ProcessChequeDispatch("CLM-001", "Ramesh Kumar", 500000m);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.ChequeNumber);
            Assert.AreEqual(500000m, result.Amount);
            Assert.AreEqual("Ramesh Kumar", result.PayeeName);
        }

        [TestMethod]
        public void ProcessChequeDispatch_NullClaimNumber_ReturnsFalse()
        {
            var result = _service.ProcessChequeDispatch(null, "Payee", 500000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.AreEqual(string.Empty, result.ReferenceId);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ProcessChequeDispatch_ZeroAmount_ReturnsFalse()
        {
            var result = _service.ProcessChequeDispatch("CLM-002", "Payee", 0m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(string.Empty, result.ReferenceId);
        }

        [TestMethod]
        public void ProcessChequeDispatch_SetsAwbNumber()
        {
            var result = _service.ProcessChequeDispatch("CLM-003", "Payee", 250000m);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.AwbNumber);
            Assert.IsTrue(result.AwbNumber.StartsWith("AWB"));
            Assert.IsTrue(result.AwbNumber.Length > 3);
        }

        [TestMethod]
        public void ProcessChequeDispatch_SetsDeliveryStatus()
        {
            var result = _service.ProcessChequeDispatch("CLM-004", "Payee", 100000m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Dispatched", result.DeliveryStatus);
            Assert.IsNotNull(result.DeliveryStatus);
            Assert.IsTrue(result.DeliveryStatus.Length > 0);
        }

        [TestMethod]
        public void ProcessChequeDispatch_SetsCourierPartner()
        {
            var result = _service.ProcessChequeDispatch("CLM-005", "Payee", 300000m);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("BlueDart", result.CourierPartner);
            Assert.IsNotNull(result.CourierPartner);
            Assert.IsTrue(result.CourierPartner.Length > 0);
        }

        [TestMethod]
        public void ValidateChequeDispatch_ValidClaim_ReturnsSuccess()
        {
            var result = _service.ValidateChequeDispatch("CLM-006");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.Message.Contains("CLM-006"));
        }

        [TestMethod]
        public void ValidateChequeDispatch_NullClaim_ReturnsFalse()
        {
            var result = _service.ValidateChequeDispatch(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void GenerateChequeNumber_ValidInputs_ReturnsChequeNumber()
        {
            var chequeNum = _service.GenerateChequeNumber("MBP", 1);
            Assert.IsNotNull(chequeNum);
            Assert.IsTrue(chequeNum.StartsWith("MBP-"));
            Assert.IsTrue(chequeNum.Length > 4);
            Assert.IsTrue(chequeNum.Contains("-"));
        }

        [TestMethod]
        public void GenerateChequeNumber_NullBranch_ReturnsEmpty()
        {
            var chequeNum = _service.GenerateChequeNumber(null, 1);
            Assert.AreEqual(string.Empty, chequeNum);
            Assert.IsNotNull(chequeNum);
            Assert.IsTrue(chequeNum.Length == 0);
            Assert.IsFalse(chequeNum.Length > 0);
        }

        [TestMethod]
        public void GenerateChequeNumber_ZeroSequence_ReturnsEmpty()
        {
            var chequeNum = _service.GenerateChequeNumber("MBP", 0);
            Assert.AreEqual(string.Empty, chequeNum);
            Assert.IsNotNull(chequeNum);
            Assert.IsTrue(chequeNum.Length == 0);
            Assert.IsFalse(chequeNum.Length > 0);
        }

        [TestMethod]
        public void TrackCourierStatus_ValidAwb_ReturnsStatus()
        {
            var result = _service.TrackCourierStatus("AWB00000001");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("InTransit", result.DeliveryStatus);
            Assert.AreEqual("AWB00000001", result.AwbNumber);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void TrackCourierStatus_NullAwb_ReturnsFalse()
        {
            var result = _service.TrackCourierStatus(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsTrue(result.Message.Length > 0);
        }

        [TestMethod]
        public void IsChequeExpired_RecentCheque_ReturnsFalse()
        {
            var expired = _service.IsChequeExpired(DateTime.UtcNow.AddDays(-10), 90);
            Assert.IsFalse(expired);
            Assert.AreEqual(false, expired);
            Assert.IsTrue(expired == false);
            Assert.IsFalse(expired == true);
        }

        [TestMethod]
        public void IsChequeExpired_OldCheque_ReturnsTrue()
        {
            var expired = _service.IsChequeExpired(DateTime.UtcNow.AddDays(-100), 90);
            Assert.IsTrue(expired);
            Assert.AreEqual(true, expired);
            Assert.IsFalse(expired == false);
            Assert.IsTrue(expired == true);
        }

        [TestMethod]
        public void IsChequeExpired_ZeroValidity_ReturnsTrue()
        {
            var expired = _service.IsChequeExpired(DateTime.UtcNow, 0);
            Assert.IsTrue(expired);
            Assert.AreEqual(true, expired);
            Assert.IsFalse(expired == false);
            Assert.IsTrue(expired == true);
        }

        [TestMethod]
        public void GetChequeDispatchCharges_Courier_ReturnsHundred()
        {
            var charges = _service.GetChequeDispatchCharges("COURIER");
            Assert.AreEqual(100m, charges);
            Assert.IsTrue(charges > 0);
            Assert.IsTrue(charges == 100m);
            Assert.IsFalse(charges < 0);
        }

        [TestMethod]
        public void GetChequeDispatchCharges_SpeedPost_ReturnsFifty()
        {
            var charges = _service.GetChequeDispatchCharges("SPEED_POST");
            Assert.AreEqual(50m, charges);
            Assert.IsTrue(charges > 0);
            Assert.IsTrue(charges == 50m);
            Assert.IsFalse(charges < 0);
        }

        [TestMethod]
        public void GetChequeDispatchCharges_RegisteredPost_ReturnsThirty()
        {
            var charges = _service.GetChequeDispatchCharges("REGISTERED_POST");
            Assert.AreEqual(30m, charges);
            Assert.IsTrue(charges > 0);
            Assert.IsTrue(charges == 30m);
            Assert.IsFalse(charges < 0);
        }

        [TestMethod]
        public void GetChequeDispatchCharges_NullMode_ReturnsZero()
        {
            var charges = _service.GetChequeDispatchCharges(null);
            Assert.AreEqual(0m, charges);
            Assert.IsFalse(charges > 0);
            Assert.IsFalse(charges < 0);
            Assert.IsTrue(charges == 0m);
        }

        [TestMethod]
        public void ValidateChequeAmount_ValidAmount_ReturnsTrue()
        {
            var valid = _service.ValidateChequeAmount(500000m);
            Assert.IsTrue(valid);
            Assert.AreEqual(true, valid);
            Assert.IsFalse(valid == false);
            Assert.IsTrue(valid == true);
        }

        [TestMethod]
        public void ValidateChequeAmount_ZeroAmount_ReturnsFalse()
        {
            var valid = _service.ValidateChequeAmount(0m);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsTrue(valid == false);
            Assert.IsFalse(valid == true);
        }

        [TestMethod]
        public void GetCourierPartner_NorthPincode_ReturnsBlueDart()
        {
            var partner = _service.GetCourierPartner("110001");
            Assert.AreEqual("BlueDart", partner);
            Assert.IsNotNull(partner);
            Assert.IsTrue(partner.Length > 0);
            Assert.IsTrue(partner == "BlueDart");
        }

        [TestMethod]
        public void GetCourierPartner_WestPincode_ReturnsDTDC()
        {
            var partner = _service.GetCourierPartner("400001");
            Assert.AreEqual("DTDC", partner);
            Assert.IsNotNull(partner);
            Assert.IsTrue(partner.Length > 0);
            Assert.IsTrue(partner == "DTDC");
        }

        [TestMethod]
        public void GetCourierPartner_SouthPincode_ReturnsFedEx()
        {
            var partner = _service.GetCourierPartner("560001");
            Assert.AreEqual("FedEx", partner);
            Assert.IsNotNull(partner);
            Assert.IsTrue(partner.Length > 0);
            Assert.IsTrue(partner == "FedEx");
        }

        [TestMethod]
        public void GetCourierPartner_NullPincode_ReturnsEmpty()
        {
            var partner = _service.GetCourierPartner(null);
            Assert.AreEqual(string.Empty, partner);
            Assert.IsNotNull(partner);
            Assert.IsTrue(partner.Length == 0);
            Assert.IsFalse(partner.Length > 0);
        }

        [TestMethod]
        public void GetMaximumChequeAmount_ReturnsExpected()
        {
            var max = _service.GetMaximumChequeAmount();
            Assert.AreEqual(10000000m, max);
            Assert.IsTrue(max > 0);
            Assert.IsTrue(max >= 10000000m);
            Assert.IsTrue(max > 1000m);
        }
    }
}
