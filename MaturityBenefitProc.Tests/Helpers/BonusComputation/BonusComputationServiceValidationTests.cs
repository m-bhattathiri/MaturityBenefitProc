using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.BonusComputation;

namespace MaturityBenefitProc.Tests.Helpers.BonusComputation
{
    [TestClass]
    public class BonusComputationServiceValidationTests
    {
        private BonusComputationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new BonusComputationService();
        }

        [TestMethod]
        public void ValidateBonusComputation_SmallPositiveAmount_ReturnsTrue()
        {
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V01", 0.01m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V02", 1m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V03", 100m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V04", 999m));
        }

        [TestMethod]
        public void ValidateBonusComputation_MediumAmount_ReturnsTrue()
        {
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V05", 10000m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V06", 100000m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V07", 500000m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V08", 1000000m));
        }

        [TestMethod]
        public void ValidateBonusComputation_LargeAmount_ReturnsTrue()
        {
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V09", 5000000m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V10", 10000000m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V11", 25000000m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-V12", 50000000m));
        }

        [TestMethod]
        public void ValidateBonusComputation_NullPolicy_ReturnsFalse()
        {
            Assert.IsFalse(_service.ValidateBonusComputation(null, 1000m));
            Assert.IsFalse(_service.ValidateBonusComputation("", 1000m));
            Assert.IsFalse(_service.ValidateBonusComputation("  ", 1000m));
            Assert.IsFalse(_service.ValidateBonusComputation(null, 50000000m));
        }

        [TestMethod]
        public void IsBonusEligible_BoundaryAt3Years_ReturnsTrue()
        {
            Assert.IsTrue(_service.IsBonusEligible("POL-E01", 3));
            Assert.IsTrue(_service.IsBonusEligible("POL-E02", 4));
            Assert.IsTrue(_service.IsBonusEligible("POL-E03", 5));
            Assert.IsTrue(_service.IsBonusEligible("POL-E04", 100));
        }

        [TestMethod]
        public void IsBonusEligible_BelowBoundary_ReturnsFalse()
        {
            Assert.IsFalse(_service.IsBonusEligible("POL-E05", 2));
            Assert.IsFalse(_service.IsBonusEligible("POL-E06", 1));
            Assert.IsFalse(_service.IsBonusEligible("POL-E07", 0));
            Assert.IsFalse(_service.IsBonusEligible("POL-E08", -1));
        }

        [TestMethod]
        public void GetBonusRate_AllKnownTypes_ReturnsPositive()
        {
            Assert.IsTrue(_service.GetBonusRate("Endowment", 1) > 0);
            Assert.IsTrue(_service.GetBonusRate("MoneyBack", 1) > 0);
            Assert.IsTrue(_service.GetBonusRate("WholeLife", 1) > 0);
            Assert.IsTrue(_service.GetBonusRate("TermPlan", 1) > 0);
        }

        [TestMethod]
        public void GetBonusRate_AllTypes_BelowMaximum()
        {
            Assert.IsTrue(_service.GetBonusRate("Endowment", 1) <= _service.GetMaximumBonusRate("Endowment"));
            Assert.IsTrue(_service.GetBonusRate("MoneyBack", 1) <= _service.GetMaximumBonusRate("MoneyBack"));
            Assert.IsTrue(_service.GetBonusRate("WholeLife", 1) <= _service.GetMaximumBonusRate("WholeLife"));
            Assert.IsTrue(_service.GetBonusRate("TermPlan", 1) <= _service.GetMaximumBonusRate("TermPlan"));
        }

        [TestMethod]
        public void GetBonusRate_AllTypes_AboveMinimum()
        {
            Assert.IsTrue(_service.GetBonusRate("Endowment", 1) >= _service.GetMinimumBonusRate("Endowment"));
            Assert.IsTrue(_service.GetBonusRate("MoneyBack", 1) >= _service.GetMinimumBonusRate("MoneyBack"));
            Assert.IsTrue(_service.GetBonusRate("WholeLife", 1) >= _service.GetMinimumBonusRate("WholeLife"));
            Assert.IsTrue(_service.GetBonusRate("TermPlan", 1) >= _service.GetMinimumBonusRate("TermPlan"));
        }

        [TestMethod]
        public void GetMaximumBonusRate_AlwaysGreaterThanMinimum()
        {
            Assert.IsTrue(_service.GetMaximumBonusRate("Endowment") > _service.GetMinimumBonusRate("Endowment"));
            Assert.IsTrue(_service.GetMaximumBonusRate("MoneyBack") > _service.GetMinimumBonusRate("MoneyBack"));
            Assert.IsTrue(_service.GetMaximumBonusRate("WholeLife") > _service.GetMinimumBonusRate("WholeLife"));
            Assert.IsTrue(_service.GetMaximumBonusRate("TermPlan") > _service.GetMinimumBonusRate("TermPlan"));
        }

        [TestMethod]
        public void GetMaximumBonusRate_NullType_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.GetMaximumBonusRate(null));
            Assert.AreEqual(0m, _service.GetMaximumBonusRate(""));
            Assert.AreEqual(0m, _service.GetMaximumBonusRate("  "));
            Assert.AreEqual(0m, _service.GetMaximumBonusRate(null));
        }

        [TestMethod]
        public void GetMinimumBonusRate_NullType_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.GetMinimumBonusRate(null));
            Assert.AreEqual(0m, _service.GetMinimumBonusRate(""));
            Assert.AreEqual(0m, _service.GetMinimumBonusRate("  "));
            Assert.AreEqual(0m, _service.GetMinimumBonusRate(null));
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_BoundaryAt10Years_Returns3Percent()
        {
            var loyalty = _service.CalculateLoyaltyAddition(1000000m, 10, "Endowment");
            Assert.AreEqual(30000m, loyalty);
            Assert.IsTrue(loyalty > 0);
            Assert.IsTrue(loyalty < 50000m);
            Assert.AreEqual(30000m, _service.CalculateLoyaltyAddition(1000000m, 14, "Endowment"));
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_BoundaryAt15Years_Returns5Percent()
        {
            var loyalty = _service.CalculateLoyaltyAddition(1000000m, 15, "Endowment");
            Assert.AreEqual(50000m, loyalty);
            Assert.IsTrue(loyalty > 30000m);
            Assert.AreEqual(50000m, _service.CalculateLoyaltyAddition(1000000m, 25, "Endowment"));
            Assert.AreEqual(100000m, _service.CalculateLoyaltyAddition(2000000m, 15, "Endowment"));
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_Below10Years_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(1000000m, 9, "Endowment"));
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(1000000m, 5, "MoneyBack"));
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(1000000m, 1, "Endowment"));
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(1000000m, 0, "Endowment"));
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_LargeSum_ComputesCorrectly()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-LARGE", 10000000m, 1);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(450000m, result.Amount);
            Assert.IsTrue(result.Amount > 0);
            Assert.IsTrue(result.Amount <= 10000000m);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_LargeExisting_HandlesCorrectly()
        {
            var result = _service.ComputeCompoundReversionaryBonus("POL-LRG2", 1000000m, 5000000m, 1);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Amount > 0);
            Assert.AreEqual(270000m, result.Amount);
            Assert.IsTrue(result.AccruedAmount > 5000000m);
        }

        [TestMethod]
        public void CalculateInterimBonus_SingleDay_ReturnsSmallAmount()
        {
            var interim = _service.CalculateInterimBonus(1000000m, 45m, 1);
            Assert.IsTrue(interim > 0);
            var expected = Math.Round(1000000m * 45m / 1000m * 1m / 365m, 2);
            Assert.AreEqual(expected, interim);
            Assert.IsTrue(interim < 200m);
            Assert.IsTrue(interim > 100m);
        }

        [TestMethod]
        public void CalculateInterimBonus_HalfYear_ReturnsCorrect()
        {
            var interim = _service.CalculateInterimBonus(1000000m, 45m, 182);
            Assert.IsTrue(interim > 0);
            Assert.IsTrue(interim < 45000m);
            Assert.IsTrue(interim > 20000m);
            var expected = Math.Round(1000000m * 45m / 1000m * 182m / 365m, 2);
            Assert.AreEqual(expected, interim);
        }

        [TestMethod]
        public void GetBonusHistory_FutureDateRange_ReturnsEmpty()
        {
            _service.ComputeSimpleReversionaryBonus("POL-HIST", 500000m, 10);
            var history = _service.GetBonusHistory("POL-HIST", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(2));
            Assert.AreEqual(0, history.Count);
            Assert.IsNotNull(history);
            Assert.IsFalse(history.Any());
            Assert.IsInstanceOfType(history, typeof(List<BonusComputationResult>));
        }

        [TestMethod]
        public void CalculateTotalAccruedBonus_NegativeInputs_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateTotalAccruedBonus(-500000m, 45m, 10));
            Assert.AreEqual(0m, _service.CalculateTotalAccruedBonus(500000m, -45m, 10));
            Assert.AreEqual(0m, _service.CalculateTotalAccruedBonus(500000m, 45m, -10));
            Assert.AreEqual(0m, _service.CalculateTotalAccruedBonus(-1m, -1m, -1));
        }

        [TestMethod]
        public void GetAccruedBonusAmount_NullPolicy_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.GetAccruedBonusAmount(null));
            Assert.AreEqual(0m, _service.GetAccruedBonusAmount(""));
            Assert.AreEqual(0m, _service.GetAccruedBonusAmount("   "));
            Assert.AreEqual(0m, _service.GetAccruedBonusAmount("NONEXISTENT"));
        }

        [TestMethod]
        public void GetBonusDeclarationYear_NullPolicy_ReturnsZero()
        {
            Assert.AreEqual(0, _service.GetBonusDeclarationYear(null));
            Assert.AreEqual(0, _service.GetBonusDeclarationYear(""));
            Assert.AreEqual(0, _service.GetBonusDeclarationYear("   "));
            Assert.AreEqual(0, _service.GetBonusDeclarationYear("NONEXISTENT"));
        }

        [TestMethod]
        public void ValidateBonusComputation_ZeroAmount_ReturnsFalse()
        {
            Assert.IsFalse(_service.ValidateBonusComputation("POL-Z01", 0m));
            Assert.IsFalse(_service.ValidateBonusComputation("POL-Z02", -1m));
            Assert.IsFalse(_service.ValidateBonusComputation("POL-Z03", -100m));
            Assert.IsFalse(_service.ValidateBonusComputation("POL-Z04", -999999m));
        }

        [TestMethod]
        public void ValidateBonusComputation_ExceedsMax_ReturnsFalse()
        {
            Assert.IsFalse(_service.ValidateBonusComputation("POL-M01", 50000001m));
            Assert.IsFalse(_service.ValidateBonusComputation("POL-M02", 100000000m));
            Assert.IsFalse(_service.ValidateBonusComputation("POL-M03", 999999999m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-M04", 50000000m));
        }

        [TestMethod]
        public void CalculateInterimBonus_ZeroDays_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateInterimBonus(500000m, 45m, 0));
            Assert.AreEqual(0m, _service.CalculateInterimBonus(1000000m, 50m, 0));
            Assert.AreEqual(0m, _service.CalculateInterimBonus(500000m, 45m, -10));
            Assert.AreEqual(0m, _service.CalculateInterimBonus(1000000m, 50m, -5));
        }
    }
}
