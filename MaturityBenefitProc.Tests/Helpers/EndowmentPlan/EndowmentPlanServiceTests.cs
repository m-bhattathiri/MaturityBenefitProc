using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.EndowmentPlan;

namespace MaturityBenefitProc.Tests.Helpers.EndowmentPlan
{
    [TestClass]
    public class EndowmentPlanServiceTests
    {
        private EndowmentPlanService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new EndowmentPlanService();
        }

        [TestMethod]
        public void CalculatePaidUpValue_ValidInputs_ReturnsCorrect()
        {
            var result = _service.CalculatePaidUpValue(500000m, 10, 20);
            Assert.AreEqual(250000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculatePaidUpValue_AllPremiumsPaid_ReturnsFull()
        {
            var result = _service.CalculatePaidUpValue(500000m, 20, 20);
            Assert.AreEqual(500000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculatePaidUpValue_OnePremium_ReturnsSmall()
        {
            var result = _service.CalculatePaidUpValue(500000m, 1, 20);
            Assert.AreEqual(25000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculatePaidUpValue_ZeroTotalDue_ReturnsZero()
        {
            var result = _service.CalculatePaidUpValue(500000m, 10, 0);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculatePaidUpValue_ZeroPremiumsPaid_ReturnsZero()
        {
            var result = _service.CalculatePaidUpValue(500000m, 0, 20);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_SevenPlusYears_Returns90Percent()
        {
            var result = _service.CalculateSurrenderValue(250000m, 50000m, 8);
            Assert.AreEqual(270000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_FiveToSixYears_Returns70Percent()
        {
            var result = _service.CalculateSurrenderValue(250000m, 50000m, 5);
            Assert.AreEqual(210000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_ThreeToFourYears_Returns50Percent()
        {
            var result = _service.CalculateSurrenderValue(250000m, 50000m, 3);
            Assert.AreEqual(150000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_LessThanThreeYears_Returns30Percent()
        {
            var result = _service.CalculateSurrenderValue(250000m, 50000m, 2);
            Assert.AreEqual(90000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_ExactlySeven_Returns90()
        {
            var result = _service.CalculateSurrenderValue(100000m, 20000m, 7);
            Assert.AreEqual(108000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetEndowmentMaturityFactor_END20_Returns1()
        {
            var result = _service.GetEndowmentMaturityFactor("END20", 20);
            Assert.AreEqual(1.0m, result);
            Assert.IsTrue(result >= 1.0m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetEndowmentMaturityFactor_END25_Returns105()
        {
            var result = _service.GetEndowmentMaturityFactor("END25", 25);
            Assert.AreEqual(1.05m, result);
            Assert.IsTrue(result > 1.0m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetEndowmentMaturityFactor_END30_Returns110()
        {
            var result = _service.GetEndowmentMaturityFactor("END30", 30);
            Assert.AreEqual(1.10m, result);
            Assert.IsTrue(result > 1.0m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetEndowmentMaturityFactor_Unknown_Returns1()
        {
            var result = _service.GetEndowmentMaturityFactor("UNKNOWN", 20);
            Assert.AreEqual(1.0m, result);
            Assert.IsTrue(result >= 1.0m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetEndowmentMaturityFactor_Null_Returns1()
        {
            var result = _service.GetEndowmentMaturityFactor(null, 20);
            Assert.AreEqual(1.0m, result);
            Assert.IsTrue(result >= 1.0m);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetMinimumPremiumsPaidForSurrender_END20_Returns6()
        {
            var result = _service.GetMinimumPremiumsPaidForSurrender("END20");
            Assert.AreEqual(6m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetMinimumPremiumsPaidForSurrender_END25_Returns7()
        {
            var result = _service.GetMinimumPremiumsPaidForSurrender("END25");
            Assert.AreEqual(7m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetMinimumPremiumsPaidForSurrender_Default_Returns5()
        {
            var result = _service.GetMinimumPremiumsPaidForSurrender("OTHER");
            Assert.AreEqual(5m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetMinimumPremiumsPaidForSurrender_Null_Returns5()
        {
            var result = _service.GetMinimumPremiumsPaidForSurrender(null);
            Assert.AreEqual(5m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void IsEligibleForFullMaturity_AllPaid_ReturnsTrue()
        {
            var result = _service.IsEligibleForFullMaturity("END001", 20, 20);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsEligibleForFullMaturity_NotAllPaid_ReturnsFalse()
        {
            var result = _service.IsEligibleForFullMaturity("END002", 15, 25);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsEligibleForFullMaturity_NullPolicy_ReturnsFalse()
        {
            var result = _service.IsEligibleForFullMaturity(null, 20, 20);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsEligibleForFullMaturity_OverPaid_ReturnsTrue()
        {
            var result = _service.IsEligibleForFullMaturity("END001", 25, 20);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsWithinGracePeriod_RecentPremium_ReturnsTrue()
        {
            var result = _service.IsWithinGracePeriod("END001", DateTime.UtcNow);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsWithinGracePeriod_NullPolicy_ReturnsFalse()
        {
            var result = _service.IsWithinGracePeriod(null, DateTime.UtcNow);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void IsWithinGracePeriod_UnknownPolicy_ReturnsFalse()
        {
            var result = _service.IsWithinGracePeriod("UNKNOWN", DateTime.UtcNow);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }
    }
}
