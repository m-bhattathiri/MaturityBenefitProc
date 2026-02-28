using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class LoyaltyAdditionServiceTests
    {
        private ILoyaltyAdditionService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation exists for testing
            _service = new LoyaltyAdditionService();
        }

        [TestMethod]
        public void CalculateBaseLoyaltyAddition_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateBaseLoyaltyAddition("POL123", 100000m, 10);
            var result2 = _service.CalculateBaseLoyaltyAddition("POL456", 50000m, 5);
            var result3 = _service.CalculateBaseLoyaltyAddition("POL789", 200000m, 20);
            var result4 = _service.CalculateBaseLoyaltyAddition("POL000", 0m, 10);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateBaseLoyaltyAddition_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.CalculateBaseLoyaltyAddition("", 100000m, 10);
            var result2 = _service.CalculateBaseLoyaltyAddition(null, 100000m, 10);
            var result3 = _service.CalculateBaseLoyaltyAddition("POL123", -100m, 10);
            var result4 = _service.CalculateBaseLoyaltyAddition("POL123", 100000m, -5);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetLoyaltyAdditionRate_ValidInputs_ReturnsRate()
        {
            var result1 = _service.GetLoyaltyAdditionRate("PROD1", 10);
            var result2 = _service.GetLoyaltyAdditionRate("PROD2", 15);
            var result3 = _service.GetLoyaltyAdditionRate("PROD3", 5);
            var result4 = _service.GetLoyaltyAdditionRate("PROD1", 20);

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreNotEqual(0.0, result4);
        }

        [TestMethod]
        public void GetLoyaltyAdditionRate_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.GetLoyaltyAdditionRate("", 10);
            var result2 = _service.GetLoyaltyAdditionRate(null, 10);
            var result3 = _service.GetLoyaltyAdditionRate("PROD1", -1);
            var result4 = _service.GetLoyaltyAdditionRate("PROD1", 0);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void IsEligibleForLoyaltyAddition_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.IsEligibleForLoyaltyAddition("POL123", DateTime.Now.AddDays(-1));
            var result2 = _service.IsEligibleForLoyaltyAddition("POL456", DateTime.Now.AddYears(-5));
            var result3 = _service.IsEligibleForLoyaltyAddition("POL789", DateTime.Now.AddDays(-10));
            var result4 = _service.IsEligibleForLoyaltyAddition("POL999", DateTime.Now.AddMonths(-1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
        }

        [TestMethod]
        public void IsEligibleForLoyaltyAddition_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.IsEligibleForLoyaltyAddition("", DateTime.Now);
            var result2 = _service.IsEligibleForLoyaltyAddition(null, DateTime.Now);
            var result3 = _service.IsEligibleForLoyaltyAddition("POL123", DateTime.Now.AddDays(1));
            var result4 = _service.IsEligibleForLoyaltyAddition("POL123", DateTime.MaxValue);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void GetCompletedPremiumYears_ValidInputs_ReturnsYears()
        {
            var result1 = _service.GetCompletedPremiumYears("POL123", DateTime.Now.AddYears(-10));
            var result2 = _service.GetCompletedPremiumYears("POL456", DateTime.Now.AddYears(-5));
            var result3 = _service.GetCompletedPremiumYears("POL789", DateTime.Now.AddYears(-1));
            var result4 = _service.GetCompletedPremiumYears("POL999", DateTime.Now.AddYears(-20));

            Assert.IsTrue(result1 >= 10);
            Assert.IsTrue(result2 >= 5);
            Assert.IsTrue(result3 >= 1);
            Assert.IsTrue(result4 >= 20);
        }

        [TestMethod]
        public void GetCompletedPremiumYears_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.GetCompletedPremiumYears("", DateTime.Now.AddYears(-10));
            var result2 = _service.GetCompletedPremiumYears(null, DateTime.Now.AddYears(-10));
            var result3 = _service.GetCompletedPremiumYears("POL123", DateTime.Now.AddDays(1));
            var result4 = _service.GetCompletedPremiumYears("POL123", DateTime.MaxValue);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GenerateLoyaltyTransactionId_ValidInputs_ReturnsId()
        {
            var result1 = _service.GenerateLoyaltyTransactionId("POL123", DateTime.Now);
            var result2 = _service.GenerateLoyaltyTransactionId("POL456", DateTime.Now.AddDays(-1));
            var result3 = _service.GenerateLoyaltyTransactionId("POL789", DateTime.Now.AddMonths(-1));
            var result4 = _service.GenerateLoyaltyTransactionId("POL999", DateTime.Now.AddYears(-1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GenerateLoyaltyTransactionId_InvalidInputs_ReturnsNullOrEmpty()
        {
            var result1 = _service.GenerateLoyaltyTransactionId("", DateTime.Now);
            var result2 = _service.GenerateLoyaltyTransactionId(null, DateTime.Now);
            var result3 = _service.GenerateLoyaltyTransactionId("POL123", DateTime.MinValue);
            var result4 = _service.GenerateLoyaltyTransactionId("POL123", DateTime.MaxValue);

            Assert.IsTrue(string.IsNullOrEmpty(result1));
            Assert.IsTrue(string.IsNullOrEmpty(result2));
            Assert.IsTrue(string.IsNullOrEmpty(result3));
            Assert.IsTrue(string.IsNullOrEmpty(result4));
        }

        [TestMethod]
        public void ComputeFinalLoyaltyAmount_ValidInputs_ReturnsAmount()
        {
            var result1 = _service.ComputeFinalLoyaltyAmount("POL123", 1000m, 1.5);
            var result2 = _service.ComputeFinalLoyaltyAmount("POL456", 500m, 1.2);
            var result3 = _service.ComputeFinalLoyaltyAmount("POL789", 2000m, 2.0);
            var result4 = _service.ComputeFinalLoyaltyAmount("POL999", 100m, 1.1);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
        }

        [TestMethod]
        public void ComputeFinalLoyaltyAmount_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.ComputeFinalLoyaltyAmount("", 1000m, 1.5);
            var result2 = _service.ComputeFinalLoyaltyAmount(null, 1000m, 1.5);
            var result3 = _service.ComputeFinalLoyaltyAmount("POL123", -100m, 1.5);
            var result4 = _service.ComputeFinalLoyaltyAmount("POL123", 1000m, -1.0);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void HasCompletedPremiumTerm_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.HasCompletedPremiumTerm("POL123", 10);
            var result2 = _service.HasCompletedPremiumTerm("POL456", 5);
            var result3 = _service.HasCompletedPremiumTerm("POL789", 20);
            var result4 = _service.HasCompletedPremiumTerm("POL999", 15);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
        }

        [TestMethod]
        public void HasCompletedPremiumTerm_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.HasCompletedPremiumTerm("", 10);
            var result2 = _service.HasCompletedPremiumTerm(null, 10);
            var result3 = _service.HasCompletedPremiumTerm("POL123", -1);
            var result4 = _service.HasCompletedPremiumTerm("POL123", 0);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void CalculateLoyaltyMultiplier_ValidInputs_ReturnsMultiplier()
        {
            var result1 = _service.CalculateLoyaltyMultiplier(90, 1.5);
            var result2 = _service.CalculateLoyaltyMultiplier(80, 1.2);
            var result3 = _service.CalculateLoyaltyMultiplier(100, 2.0);
            var result4 = _service.CalculateLoyaltyMultiplier(70, 1.1);

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreNotEqual(0.0, result4);
        }

        [TestMethod]
        public void CalculateLoyaltyMultiplier_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.CalculateLoyaltyMultiplier(-10, 1.5);
            var result2 = _service.CalculateLoyaltyMultiplier(110, 1.5);
            var result3 = _service.CalculateLoyaltyMultiplier(90, -1.0);
            var result4 = _service.CalculateLoyaltyMultiplier(90, 0.0);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetAccruedLoyaltyValue_ValidInputs_ReturnsValue()
        {
            var result1 = _service.GetAccruedLoyaltyValue("POL123", DateTime.Now);
            var result2 = _service.GetAccruedLoyaltyValue("POL456", DateTime.Now.AddDays(-1));
            var result3 = _service.GetAccruedLoyaltyValue("POL789", DateTime.Now.AddMonths(-1));
            var result4 = _service.GetAccruedLoyaltyValue("POL999", DateTime.Now.AddYears(-1));

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
        }

        [TestMethod]
        public void GetAccruedLoyaltyValue_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.GetAccruedLoyaltyValue("", DateTime.Now);
            var result2 = _service.GetAccruedLoyaltyValue(null, DateTime.Now);
            var result3 = _service.GetAccruedLoyaltyValue("POL123", DateTime.MinValue);
            var result4 = _service.GetAccruedLoyaltyValue("POL123", DateTime.MaxValue);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateDaysSinceLastAddition_ValidInputs_ReturnsDays()
        {
            var result1 = _service.CalculateDaysSinceLastAddition("POL123", DateTime.Now);
            var result2 = _service.CalculateDaysSinceLastAddition("POL456", DateTime.Now.AddDays(10));
            var result3 = _service.CalculateDaysSinceLastAddition("POL789", DateTime.Now.AddMonths(1));
            var result4 = _service.CalculateDaysSinceLastAddition("POL999", DateTime.Now.AddYears(1));

            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
            Assert.AreNotEqual(0, result3);
            Assert.AreNotEqual(0, result4);
        }

        [TestMethod]
        public void CalculateDaysSinceLastAddition_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.CalculateDaysSinceLastAddition("", DateTime.Now);
            var result2 = _service.CalculateDaysSinceLastAddition(null, DateTime.Now);
            var result3 = _service.CalculateDaysSinceLastAddition("POL123", DateTime.MinValue);
            var result4 = _service.CalculateDaysSinceLastAddition("POL123", DateTime.Now.AddDays(-1));

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetLoyaltyFundCode_ValidInputs_ReturnsCode()
        {
            var result1 = _service.GetLoyaltyFundCode("CAT1", 2010);
            var result2 = _service.GetLoyaltyFundCode("CAT2", 2015);
            var result3 = _service.GetLoyaltyFundCode("CAT3", 2020);
            var result4 = _service.GetLoyaltyFundCode("CAT1", 2005);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetLoyaltyFundCode_InvalidInputs_ReturnsNullOrEmpty()
        {
            var result1 = _service.GetLoyaltyFundCode("", 2010);
            var result2 = _service.GetLoyaltyFundCode(null, 2010);
            var result3 = _service.GetLoyaltyFundCode("CAT1", -1);
            var result4 = _service.GetLoyaltyFundCode("CAT1", 0);

            Assert.IsTrue(string.IsNullOrEmpty(result1));
            Assert.IsTrue(string.IsNullOrEmpty(result2));
            Assert.IsTrue(string.IsNullOrEmpty(result3));
            Assert.IsTrue(string.IsNullOrEmpty(result4));
        }

        [TestMethod]
        public void ValidateLoyaltyAdditionRules_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.ValidateLoyaltyAdditionRules("POL123", 1000m);
            var result2 = _service.ValidateLoyaltyAdditionRules("POL456", 500m);
            var result3 = _service.ValidateLoyaltyAdditionRules("POL789", 2000m);
            var result4 = _service.ValidateLoyaltyAdditionRules("POL999", 100m);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
        }

        [TestMethod]
        public void ValidateLoyaltyAdditionRules_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.ValidateLoyaltyAdditionRules("", 1000m);
            var result2 = _service.ValidateLoyaltyAdditionRules(null, 1000m);
            var result3 = _service.ValidateLoyaltyAdditionRules("POL123", -100m);
            var result4 = _service.ValidateLoyaltyAdditionRules("POL123", 0m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void CalculateProratedAddition_ValidInputs_ReturnsAmount()
        {
            var result1 = _service.CalculateProratedAddition("POL123", 1000m, 180);
            var result2 = _service.CalculateProratedAddition("POL456", 500m, 90);
            var result3 = _service.CalculateProratedAddition("POL789", 2000m, 365);
            var result4 = _service.CalculateProratedAddition("POL999", 100m, 30);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateProratedAddition_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.CalculateProratedAddition("", 1000m, 180);
            var result2 = _service.CalculateProratedAddition(null, 1000m, 180);
            var result3 = _service.CalculateProratedAddition("POL123", -100m, 180);
            var result4 = _service.CalculateProratedAddition("POL123", 1000m, -10);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetBonusInterestRate_ValidInputs_ReturnsRate()
        {
            var result1 = _service.GetBonusInterestRate("FUND1", DateTime.Now);
            var result2 = _service.GetBonusInterestRate("FUND2", DateTime.Now.AddDays(-1));
            var result3 = _service.GetBonusInterestRate("FUND3", DateTime.Now.AddMonths(-1));
            var result4 = _service.GetBonusInterestRate("FUND1", DateTime.Now.AddYears(-1));

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreNotEqual(0.0, result4);
        }

        [TestMethod]
        public void GetBonusInterestRate_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.GetBonusInterestRate("", DateTime.Now);
            var result2 = _service.GetBonusInterestRate(null, DateTime.Now);
            var result3 = _service.GetBonusInterestRate("FUND1", DateTime.MinValue);
            var result4 = _service.GetBonusInterestRate("FUND1", DateTime.MaxValue);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetMissedPremiumCount_ValidInputs_ReturnsCount()
        {
            var result1 = _service.GetMissedPremiumCount("POL123");
            var result2 = _service.GetMissedPremiumCount("POL456");
            var result3 = _service.GetMissedPremiumCount("POL789");
            var result4 = _service.GetMissedPremiumCount("POL999");

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result3 >= 0);
            Assert.IsTrue(result4 >= 0);
        }

        [TestMethod]
        public void GetMissedPremiumCount_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.GetMissedPremiumCount("");
            var result2 = _service.GetMissedPremiumCount(null);
            var result3 = _service.GetMissedPremiumCount("   ");
            var result4 = _service.GetMissedPremiumCount(string.Empty);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void DetermineLoyaltyCategory_ValidInputs_ReturnsCategory()
        {
            var result1 = _service.DetermineLoyaltyCategory(10, 100000m);
            var result2 = _service.DetermineLoyaltyCategory(15, 500000m);
            var result3 = _service.DetermineLoyaltyCategory(20, 1000000m);
            var result4 = _service.DetermineLoyaltyCategory(5, 50000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void DetermineLoyaltyCategory_InvalidInputs_ReturnsNullOrEmpty()
        {
            var result1 = _service.DetermineLoyaltyCategory(-1, 100000m);
            var result2 = _service.DetermineLoyaltyCategory(0, 100000m);
            var result3 = _service.DetermineLoyaltyCategory(10, -100m);
            var result4 = _service.DetermineLoyaltyCategory(10, 0m);

            Assert.IsTrue(string.IsNullOrEmpty(result1));
            Assert.IsTrue(string.IsNullOrEmpty(result2));
            Assert.IsTrue(string.IsNullOrEmpty(result3));
            Assert.IsTrue(string.IsNullOrEmpty(result4));
        }

        [TestMethod]
        public void GetSurrenderLoyaltyValue_ValidInputs_ReturnsValue()
        {
            var result1 = _service.GetSurrenderLoyaltyValue("POL123", 100000m, 0.5);
            var result2 = _service.GetSurrenderLoyaltyValue("POL456", 50000m, 0.3);
            var result3 = _service.GetSurrenderLoyaltyValue("POL789", 200000m, 0.8);
            var result4 = _service.GetSurrenderLoyaltyValue("POL999", 10000m, 0.1);

            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.AreNotEqual(0m, result4);
        }

        [TestMethod]
        public void GetSurrenderLoyaltyValue_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.GetSurrenderLoyaltyValue("", 100000m, 0.5);
            var result2 = _service.GetSurrenderLoyaltyValue(null, 100000m, 0.5);
            var result3 = _service.GetSurrenderLoyaltyValue("POL123", -100m, 0.5);
            var result4 = _service.GetSurrenderLoyaltyValue("POL123", 100000m, -0.1);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void IsPolicyInForce_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.IsPolicyInForce("POL123", DateTime.Now);
            var result2 = _service.IsPolicyInForce("POL456", DateTime.Now.AddDays(-1));
            var result3 = _service.IsPolicyInForce("POL789", DateTime.Now.AddMonths(-1));
            var result4 = _service.IsPolicyInForce("POL999", DateTime.Now.AddYears(-1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
        }

        [TestMethod]
        public void IsPolicyInForce_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.IsPolicyInForce("", DateTime.Now);
            var result2 = _service.IsPolicyInForce(null, DateTime.Now);
            var result3 = _service.IsPolicyInForce("POL123", DateTime.MinValue);
            var result4 = _service.IsPolicyInForce("POL123", DateTime.MaxValue);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void ComputePersistencyRatio_ValidInputs_ReturnsRatio()
        {
            var result1 = _service.ComputePersistencyRatio("POL123", 120, 120);
            var result2 = _service.ComputePersistencyRatio("POL456", 60, 50);
            var result3 = _service.ComputePersistencyRatio("POL789", 240, 200);
            var result4 = _service.ComputePersistencyRatio("POL999", 12, 10);

            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
            Assert.AreNotEqual(0.0, result3);
            Assert.AreNotEqual(0.0, result4);
        }

        [TestMethod]
        public void ComputePersistencyRatio_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.ComputePersistencyRatio("", 120, 120);
            var result2 = _service.ComputePersistencyRatio(null, 120, 120);
            var result3 = _service.ComputePersistencyRatio("POL123", -10, 10);
            var result4 = _service.ComputePersistencyRatio("POL123", 120, -10);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }
    }
}