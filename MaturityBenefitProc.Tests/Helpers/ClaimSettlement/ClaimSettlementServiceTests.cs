using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.ClaimSettlement;

namespace MaturityBenefitProc.Tests.Helpers.ClaimSettlement
{
    [TestClass]
    public class ClaimSettlementServiceTests
    {
        private ClaimSettlementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new ClaimSettlementService();
        }

        [TestMethod]
        public void IsClaimSettlementEligible_ValidPolicyAndCif_ReturnsTrue()
        {
            var result = _service.IsClaimSettlementEligible("POL001", "CIF001");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsClaimSettlementEligible_WrongCif_ReturnsFalse()
        {
            var result = _service.IsClaimSettlementEligible("POL001", "CIF999");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsClaimSettlementEligible_UnknownPolicy_ReturnsFalse()
        {
            var result = _service.IsClaimSettlementEligible("POL999", "CIF001");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsClaimSettlementEligible_NullPolicy_ReturnsFalse()
        {
            var result = _service.IsClaimSettlementEligible(null, "CIF001");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsClaimSettlementEligible_NullCif_ReturnsFalse()
        {
            var result = _service.IsClaimSettlementEligible("POL001", null);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void CalculateSettlementAmount_ValidInputs_ReturnsCorrect()
        {
            var result = _service.CalculateSettlementAmount(500000m, 100000m, 50000m);
            Assert.AreEqual(550000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSettlementAmount_ZeroBonus_ReturnsNet()
        {
            var result = _service.CalculateSettlementAmount(500000m, 0m, 50000m);
            Assert.AreEqual(450000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSettlementAmount_ZeroDeductions_ReturnsFull()
        {
            var result = _service.CalculateSettlementAmount(500000m, 100000m, 0m);
            Assert.AreEqual(600000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSettlementAmount_ZeroSum_Returns0()
        {
            var result = _service.CalculateSettlementAmount(0m, 100000m, 50000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSettlementAmount_DeductionsExceedTotal_Returns0()
        {
            var result = _service.CalculateSettlementAmount(100000m, 50000m, 200000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetSettlementCharges_Express_Returns500()
        {
            var result = _service.GetSettlementCharges("Express");
            Assert.AreEqual(500m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetSettlementCharges_Standard_Returns0()
        {
            var result = _service.GetSettlementCharges("Standard");
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetSettlementCharges_Priority_Returns250()
        {
            var result = _service.GetSettlementCharges("Priority");
            Assert.AreEqual(250m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetSettlementCharges_Unknown_Returns0()
        {
            var result = _service.GetSettlementCharges("Other");
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetSettlementCharges_Null_Returns0()
        {
            var result = _service.GetSettlementCharges(null);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void HasDischargeVoucher_ExistingTrue_ReturnsTrue()
        {
            var result = _service.HasDischargeVoucher("CLM001");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void HasDischargeVoucher_ExistingFalse_ReturnsFalse()
        {
            var result = _service.HasDischargeVoucher("CLM002");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void HasDischargeVoucher_Unknown_ReturnsFalse()
        {
            var result = _service.HasDischargeVoucher("CLM999");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void HasDischargeVoucher_Null_ReturnsFalse()
        {
            var result = _service.HasDischargeVoucher(null);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void GetMaximumSettlementAmount_Returns100Million()
        {
            var result = _service.GetMaximumSettlementAmount();
            Assert.AreEqual(100000000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result <= 0);
        }

        [TestMethod]
        public void IsClaimSettlementEligible_POL002_CIF002_ReturnsTrue()
        {
            var result = _service.IsClaimSettlementEligible("POL002", "CIF002");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsClaimSettlementEligible_POL003_CIF003_ReturnsTrue()
        {
            var result = _service.IsClaimSettlementEligible("POL003", "CIF003");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void HasDischargeVoucher_CLM003_ReturnsTrue()
        {
            var result = _service.HasDischargeVoucher("CLM003");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void HasDischargeVoucher_CLM004_ReturnsFalse()
        {
            var result = _service.HasDischargeVoucher("CLM004");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void HasDischargeVoucher_CLM005_ReturnsTrue()
        {
            var result = _service.HasDischargeVoucher("CLM005");
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }
    }
}
