using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.EndowmentPlan;

namespace MaturityBenefitProc.Tests.Helpers.EndowmentPlan
{
    [TestClass]
    public class EndowmentPlanServiceEdgeCaseTests
    {
        private EndowmentPlanService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new EndowmentPlanService();
        }

        [TestMethod]
        public void CalculatePaidUpValue_LargeSumAssured_ReturnsCorrect()
        {
            var result = _service.CalculatePaidUpValue(10000000m, 15, 30);
            Assert.AreEqual(5000000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculatePaidUpValue_NegativePremiums_ReturnsZero()
        {
            var result = _service.CalculatePaidUpValue(500000m, -1, 20);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculatePaidUpValue_NegativeTotalDue_ReturnsZero()
        {
            var result = _service.CalculatePaidUpValue(500000m, 10, -5);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_ZeroPaidUp_ReturnsCorrect()
        {
            var result = _service.CalculateSurrenderValue(0m, 50000m, 8);
            Assert.AreEqual(45000m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_ZeroBonus_ReturnsCorrect()
        {
            var result = _service.CalculateSurrenderValue(250000m, 0m, 8);
            Assert.AreEqual(225000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_OneYear_Returns30Percent()
        {
            var result = _service.CalculateSurrenderValue(100000m, 10000m, 1);
            Assert.AreEqual(33000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_ZeroYears_Returns30Percent()
        {
            var result = _service.CalculateSurrenderValue(100000m, 10000m, 0);
            Assert.AreEqual(33000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_SixYears_Returns70Percent()
        {
            var result = _service.CalculateSurrenderValue(100000m, 10000m, 6);
            Assert.AreEqual(77000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_FourYears_Returns50Percent()
        {
            var result = _service.CalculateSurrenderValue(100000m, 10000m, 4);
            Assert.AreEqual(55000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_TenYears_Returns90Percent()
        {
            var result = _service.CalculateSurrenderValue(100000m, 10000m, 10);
            Assert.AreEqual(99000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetGuaranteedSurrenderValue_NullPolicy_ReturnsZero()
        {
            var result = _service.GetGuaranteedSurrenderValue(null, 5);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetGuaranteedSurrenderValue_LessThan3Years_ReturnsZero()
        {
            var result = _service.GetGuaranteedSurrenderValue("END001", 2);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetGuaranteedSurrenderValue_Exactly3Years_ReturnsValue()
        {
            var result = _service.GetGuaranteedSurrenderValue("END001", 3);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
            Assert.AreEqual(150000m, result);
        }

        [TestMethod]
        public void GetSpecialSurrenderValue_NullPolicy_ReturnsZero()
        {
            var result = _service.GetSpecialSurrenderValue(null, 8);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetSpecialSurrenderValue_LessThan7Years_ReturnsZero()
        {
            var result = _service.GetSpecialSurrenderValue("END001", 6);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetSpecialSurrenderValue_Exactly7Years_ReturnsValue()
        {
            var result = _service.GetSpecialSurrenderValue("END001", 7);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
            Assert.AreEqual(450000m, result);
        }

        [TestMethod]
        public void GetEndowmentMaturityFactor_EmptyPlanCode_Returns1()
        {
            var result = _service.GetEndowmentMaturityFactor("", 20);
            Assert.AreEqual(1.0m, result);
            Assert.IsTrue(result >= 1.0m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetMinimumPremiumsPaidForSurrender_EmptyCode_Returns5()
        {
            var result = _service.GetMinimumPremiumsPaidForSurrender("");
            Assert.AreEqual(5m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void IsEligibleForFullMaturity_EmptyPolicy_ReturnsFalse()
        {
            var result = _service.IsEligibleForFullMaturity("", 20, 20);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsEligibleForFullMaturity_ZeroPaid_ReturnsFalse()
        {
            var result = _service.IsEligibleForFullMaturity("END001", 0, 20);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsWithinGracePeriod_EmptyPolicy_ReturnsFalse()
        {
            var result = _service.IsWithinGracePeriod("", DateTime.UtcNow);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void CalculateReducedPaidUpAmount_ValidInputs_ReturnsCorrect()
        {
            var result = _service.CalculateReducedPaidUpAmount(500000m, 10, 20, 100000m);
            Assert.AreEqual(300000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateReducedPaidUpAmount_ZeroTotalDue_ReturnsZero()
        {
            var result = _service.CalculateReducedPaidUpAmount(500000m, 10, 0, 100000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateReducedPaidUpAmount_ZeroPremiums_ReturnsZero()
        {
            var result = _service.CalculateReducedPaidUpAmount(500000m, 0, 20, 100000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateReducedPaidUpAmount_AllPaid_ReturnsFull()
        {
            var result = _service.CalculateReducedPaidUpAmount(500000m, 20, 20, 100000m);
            Assert.AreEqual(600000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }
    }
}
