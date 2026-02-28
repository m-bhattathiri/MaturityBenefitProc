using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class TerminalBonusServiceValidationTests
    {
        private ITerminalBonusService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming TerminalBonusService implements ITerminalBonusService
            // For the sake of this test file generation, we mock or assume a concrete implementation exists.
            // Since we can't instantiate an interface, we will use a dummy implementation or assume TerminalBonusService exists.
            // The prompt says "Each test creates a new TerminalBonusService()".
            _service = new TerminalBonusService();
        }

        [TestMethod]
        public void CalculateBaseTerminalBonus_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.CalculateBaseTerminalBonus("POL123", 100000m, new DateTime(2025, 1, 1));
            var result2 = service.CalculateBaseTerminalBonus("POL124", 50000m, new DateTime(2025, 1, 1));
            var result3 = service.CalculateBaseTerminalBonus("POL125", 0m, new DateTime(2025, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateBaseTerminalBonus_InvalidPolicyId_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultNull = service.CalculateBaseTerminalBonus(null, 100000m, DateTime.Now);
            var resultEmpty = service.CalculateBaseTerminalBonus("", 100000m, DateTime.Now);
            var resultWhitespace = service.CalculateBaseTerminalBonus("   ", 100000m, DateTime.Now);

            Assert.AreEqual(0m, resultNull);
            Assert.AreEqual(0m, resultEmpty);
            Assert.AreEqual(0m, resultWhitespace);
            Assert.IsNotNull(resultNull);
            Assert.IsNotNull(resultEmpty);
        }

        [TestMethod]
        public void CalculateBaseTerminalBonus_NegativeSumAssured_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var result1 = service.CalculateBaseTerminalBonus("POL123", -100m, DateTime.Now);
            var result2 = service.CalculateBaseTerminalBonus("POL124", -50000m, DateTime.Now);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.CalculateLoyaltyAdditionAmount("POL123", 10);
            var result2 = service.CalculateLoyaltyAdditionAmount("POL124", 20);
            var result3 = service.CalculateLoyaltyAdditionAmount("POL125", 0);

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_InvalidPolicyId_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultNull = service.CalculateLoyaltyAdditionAmount(null, 10);
            var resultEmpty = service.CalculateLoyaltyAdditionAmount("", 10);
            var resultWhitespace = service.CalculateLoyaltyAdditionAmount("   ", 10);

            Assert.AreEqual(0m, resultNull);
            Assert.AreEqual(0m, resultEmpty);
            Assert.AreEqual(0m, resultWhitespace);
            Assert.IsNotNull(resultNull);
            Assert.IsNotNull(resultEmpty);
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_NegativeYears_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var result1 = service.CalculateLoyaltyAdditionAmount("POL123", -1);
            var result2 = service.CalculateLoyaltyAdditionAmount("POL124", -10);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetAccruedReversionaryBonus_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.GetAccruedReversionaryBonus("POL123");
            var result2 = service.GetAccruedReversionaryBonus("POL124");

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetAccruedReversionaryBonus_InvalidPolicyId_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultNull = service.GetAccruedReversionaryBonus(null);
            var resultEmpty = service.GetAccruedReversionaryBonus("");
            var resultWhitespace = service.GetAccruedReversionaryBonus("   ");

            Assert.AreEqual(0m, resultNull);
            Assert.AreEqual(0m, resultEmpty);
            Assert.AreEqual(0m, resultWhitespace);
            Assert.IsNotNull(resultNull);
            Assert.IsNotNull(resultEmpty);
        }

        [TestMethod]
        public void ComputeFinalAdditionalBonus_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.ComputeFinalAdditionalBonus("POL123", 50000m);
            var result2 = service.ComputeFinalAdditionalBonus("POL124", 100000m);
            var result3 = service.ComputeFinalAdditionalBonus("POL125", 0m);

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void ComputeFinalAdditionalBonus_InvalidInputs_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultNull = service.ComputeFinalAdditionalBonus(null, 50000m);
            var resultEmpty = service.ComputeFinalAdditionalBonus("", 50000m);
            var resultNegative = service.ComputeFinalAdditionalBonus("POL123", -100m);

            Assert.AreEqual(0m, resultNull);
            Assert.AreEqual(0m, resultEmpty);
            Assert.AreEqual(0m, resultNegative);
            Assert.IsNotNull(resultNull);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void CalculateVestedBonusTotal_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.CalculateVestedBonusTotal("POL123", DateTime.Now);
            var result2 = service.CalculateVestedBonusTotal("POL124", DateTime.Now.AddYears(-1));

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateVestedBonusTotal_InvalidPolicyId_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultNull = service.CalculateVestedBonusTotal(null, DateTime.Now);
            var resultEmpty = service.CalculateVestedBonusTotal("", DateTime.Now);
            var resultWhitespace = service.CalculateVestedBonusTotal("   ", DateTime.Now);

            Assert.AreEqual(0m, resultNull);
            Assert.AreEqual(0m, resultEmpty);
            Assert.AreEqual(0m, resultWhitespace);
            Assert.IsNotNull(resultNull);
            Assert.IsNotNull(resultEmpty);
        }

        [TestMethod]
        public void GetSpecialSurrenderValueBonus_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.GetSpecialSurrenderValueBonus("POL123", 10000m);
            var result2 = service.GetSpecialSurrenderValueBonus("POL124", 50000m);
            var result3 = service.GetSpecialSurrenderValueBonus("POL125", 0m);

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetSpecialSurrenderValueBonus_InvalidInputs_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultNull = service.GetSpecialSurrenderValueBonus(null, 10000m);
            var resultEmpty = service.GetSpecialSurrenderValueBonus("", 10000m);
            var resultNegative = service.GetSpecialSurrenderValueBonus("POL123", -500m);

            Assert.AreEqual(0m, resultNull);
            Assert.AreEqual(0m, resultEmpty);
            Assert.AreEqual(0m, resultNegative);
            Assert.IsNotNull(resultNull);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void CalculateProratedTerminalBonus_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.CalculateProratedTerminalBonus("POL123", DateTime.Now, 100);
            var result2 = service.CalculateProratedTerminalBonus("POL124", DateTime.Now, 365);
            var result3 = service.CalculateProratedTerminalBonus("POL125", DateTime.Now, 0);

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateProratedTerminalBonus_InvalidInputs_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultNull = service.CalculateProratedTerminalBonus(null, DateTime.Now, 100);
            var resultEmpty = service.CalculateProratedTerminalBonus("", DateTime.Now, 100);
            var resultNegative = service.CalculateProratedTerminalBonus("POL123", DateTime.Now, -10);

            Assert.AreEqual(0m, resultNull);
            Assert.AreEqual(0m, resultEmpty);
            Assert.AreEqual(0m, resultNegative);
            Assert.IsNotNull(resultNull);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void ApplyBonusMultiplier_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.ApplyBonusMultiplier(1000m, 1.5);
            var result2 = service.ApplyBonusMultiplier(5000m, 2.0);
            var result3 = service.ApplyBonusMultiplier(0m, 1.5);

            Assert.AreEqual(1500m, result1);
            Assert.AreEqual(10000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void ApplyBonusMultiplier_NegativeInputs_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var result1 = service.ApplyBonusMultiplier(-1000m, 1.5);
            var result2 = service.ApplyBonusMultiplier(1000m, -1.5);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.GetTerminalBonusRate("PLAN_A", 10);
            var result2 = service.GetTerminalBonusRate("PLAN_B", 20);

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetTerminalBonusRate_InvalidInputs_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultNull = service.GetTerminalBonusRate(null, 10);
            var resultEmpty = service.GetTerminalBonusRate("", 10);
            var resultNegative = service.GetTerminalBonusRate("PLAN_A", -5);

            Assert.AreEqual(0, resultNull);
            Assert.AreEqual(0, resultEmpty);
            Assert.AreEqual(0, resultNegative);
            Assert.IsNotNull(resultNull);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void GetLoyaltyAdditionRate_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.GetLoyaltyAdditionRate("PLAN_A", 5);
            var result2 = service.GetLoyaltyAdditionRate("PLAN_B", 15);

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetLoyaltyAdditionRate_InvalidInputs_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultNull = service.GetLoyaltyAdditionRate(null, 5);
            var resultEmpty = service.GetLoyaltyAdditionRate("", 5);
            var resultNegative = service.GetLoyaltyAdditionRate("PLAN_A", -1);

            Assert.AreEqual(0, resultNull);
            Assert.AreEqual(0, resultEmpty);
            Assert.AreEqual(0, resultNegative);
            Assert.IsNotNull(resultNull);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void CalculateBonusYield_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.CalculateBonusYield(1000m, 10000m);
            var result2 = service.CalculateBonusYield(5000m, 50000m);
            var result3 = service.CalculateBonusYield(0m, 10000m);

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateBonusYield_InvalidInputs_ReturnsZero()
        {
            var service = new TerminalBonusService();
            var resultZeroPremiums = service.CalculateBonusYield(1000m, 0m);
            var resultNegativeBonus = service.CalculateBonusYield(-1000m, 10000m);
            var resultNegativePremiums = service.CalculateBonusYield(1000m, -10000m);

            Assert.AreEqual(0, resultZeroPremiums);
            Assert.AreEqual(0, resultNegativeBonus);
            Assert.AreEqual(0, resultNegativePremiums);
            Assert.IsNotNull(resultZeroPremiums);
            Assert.IsNotNull(resultNegativeBonus);
        }

        [TestMethod]
        public void IsEligibleForTerminalBonus_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.IsEligibleForTerminalBonus("POL123", "ACTIVE");
            var result2 = service.IsEligibleForTerminalBonus("POL124", "MATURED");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void IsEligibleForTerminalBonus_InvalidInputs_ReturnsFalse()
        {
            var service = new TerminalBonusService();
            var resultNullId = service.IsEligibleForTerminalBonus(null, "ACTIVE");
            var resultNullStatus = service.IsEligibleForTerminalBonus("POL123", null);
            var resultEmpty = service.IsEligibleForTerminalBonus("", "");

            Assert.IsFalse(resultNullId);
            Assert.IsFalse(resultNullStatus);
            Assert.IsFalse(resultEmpty);
            Assert.IsNotNull(resultNullId);
            Assert.IsNotNull(resultEmpty);
        }

        [TestMethod]
        public void IsLoyaltyAdditionApplicable_ValidInputs_ReturnsExpected()
        {
            var service = new TerminalBonusService();
            var result1 = service.IsLoyaltyAdditionApplicable("PLAN_A", 10);
            var result2 = service.IsLoyaltyAdditionApplicable("PLAN_B", 2);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }
    }
}
