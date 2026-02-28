using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.UndeliveredPayment;

namespace MaturityBenefitProc.Tests.Helpers.UndeliveredPayment
{
    [TestClass]
    public class UndeliveredPaymentServiceTests
    {
        private UndeliveredPaymentService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new UndeliveredPaymentService();
        }

        [TestMethod]
        public void IsEligibleForRedispatch_BelowMax_ReturnsTrue()
        {
            var result = _service.IsEligibleForRedispatch("PAY001", 5);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsEligibleForRedispatch_AtMax_ReturnsFalse()
        {
            var result = _service.IsEligibleForRedispatch("PAY002", 3);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsEligibleForRedispatch_AboveMax_ReturnsFalse()
        {
            var result = _service.IsEligibleForRedispatch("PAY005", 3);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsEligibleForRedispatch_ZeroAttempts_ReturnsTrue()
        {
            var result = _service.IsEligibleForRedispatch("PAY003", 5);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsEligibleForRedispatch_NullReference_ReturnsFalse()
        {
            var result = _service.IsEligibleForRedispatch(null, 5);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsEligibleForRedispatch_UnknownReference_ReturnsTrue()
        {
            var result = _service.IsEligibleForRedispatch("UNKNOWN", 5);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void GetRedispatchAttemptCount_KnownRef_ReturnsCount()
        {
            var result = _service.GetRedispatchAttemptCount("PAY001");
            Assert.AreEqual(1, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchAttemptCount_ThreeAttempts_Returns3()
        {
            var result = _service.GetRedispatchAttemptCount("PAY002");
            Assert.AreEqual(3, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchAttemptCount_ZeroAttempts_Returns0()
        {
            var result = _service.GetRedispatchAttemptCount("PAY003");
            Assert.AreEqual(0, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchAttemptCount_UnknownRef_Returns0()
        {
            var result = _service.GetRedispatchAttemptCount("UNKNOWN");
            Assert.AreEqual(0, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchAttemptCount_NullRef_Returns0()
        {
            var result = _service.GetRedispatchAttemptCount(null);
            Assert.AreEqual(0, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchCharges_Speed_Returns150()
        {
            var result = _service.GetRedispatchCharges("Speed");
            Assert.AreEqual(150m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchCharges_Registered_Returns75()
        {
            var result = _service.GetRedispatchCharges("Registered");
            Assert.AreEqual(75m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchCharges_Courier_Returns200()
        {
            var result = _service.GetRedispatchCharges("Courier");
            Assert.AreEqual(200m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchCharges_Default_Returns100()
        {
            var result = _service.GetRedispatchCharges("Other");
            Assert.AreEqual(100m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchCharges_Null_Returns100()
        {
            var result = _service.GetRedispatchCharges(null);
            Assert.AreEqual(100m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void HasAlternateAddress_ExistingCif_ReturnsTrue()
        {
            var result = _service.HasAlternateAddress("CIF001");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void HasAlternateAddress_NonExistentCif_ReturnsFalse()
        {
            var result = _service.HasAlternateAddress("CIF002");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void HasAlternateAddress_NullCif_ReturnsFalse()
        {
            var result = _service.HasAlternateAddress(null);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void HasAlternateAddress_EmptyCif_ReturnsFalse()
        {
            var result = _service.HasAlternateAddress("");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void GetMaximumRedispatchAmount_ReturnsTenMillion()
        {
            var result = _service.GetMaximumRedispatchAmount();
            Assert.AreEqual(10000000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result <= 0);
        }

        [TestMethod]
        public void HasAlternateAddress_CIF003_ReturnsTrue()
        {
            var result = _service.HasAlternateAddress("CIF003");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void HasAlternateAddress_CIF005_ReturnsTrue()
        {
            var result = _service.HasAlternateAddress("CIF005");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void HasAlternateAddress_CIF007_ReturnsTrue()
        {
            var result = _service.HasAlternateAddress("CIF007");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void GetRedispatchAttemptCount_PAY004_Returns2()
        {
            var result = _service.GetRedispatchAttemptCount("PAY004");
            Assert.AreEqual(2, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetRedispatchAttemptCount_PAY005_Returns4()
        {
            var result = _service.GetRedispatchAttemptCount("PAY005");
            Assert.AreEqual(4, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }
    }
}
