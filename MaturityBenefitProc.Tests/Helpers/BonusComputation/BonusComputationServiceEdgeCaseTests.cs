using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.BonusComputation;

namespace MaturityBenefitProc.Tests.Helpers.BonusComputation
{
    [TestClass]
    public class BonusComputationServiceEdgeCaseTests
    {
        private BonusComputationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new BonusComputationService();
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_NullPolicyNumber_ReturnsFailed()
        {
            var result = _service.ComputeSimpleReversionaryBonus(null, 500000m, 10);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Invalid"));
            Assert.AreEqual(0m, result.Amount);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_EmptyPolicyNumber_ReturnsFailed()
        {
            var result = _service.ComputeSimpleReversionaryBonus("", 500000m, 10);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_WhitespacePolicyNumber_ReturnsFailed()
        {
            var result = _service.ComputeSimpleReversionaryBonus("   ", 500000m, 10);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_ZeroSumAssured_ReturnsFailed()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-001", 0m, 10);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_NegativeSumAssured_ReturnsFailed()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-002", -100000m, 10);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_ZeroYears_ReturnsFailed()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-003", 500000m, 0);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_NegativeYears_ReturnsFailed()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-004", 500000m, -5);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_NullPolicy_ReturnsFailed()
        {
            var result = _service.ComputeCompoundReversionaryBonus(null, 500000m, 50000m, 10);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_ZeroSum_ReturnsFailed()
        {
            var result = _service.ComputeCompoundReversionaryBonus("POL-010", 0m, 50000m, 10);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_NegativeSum_ReturnsFailed()
        {
            var result = _service.ComputeCompoundReversionaryBonus("POL-011", -500000m, 0m, 10);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeTerminalBonus_NullPolicy_ReturnsFailed()
        {
            var result = _service.ComputeTerminalBonus(null, 1000000m, 20, "Endowment");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeTerminalBonus_ZeroSumAssured_ReturnsFailed()
        {
            var result = _service.ComputeTerminalBonus("POL-020", 0m, 20, "Endowment");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeTerminalBonus_ZeroCompletedYears_ReturnsFailed()
        {
            var result = _service.ComputeTerminalBonus("POL-021", 1000000m, 0, "Endowment");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(0m, result.Amount);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeTerminalBonus_Exactly5Years_ReturnsZeroBonus()
        {
            var result = _service.ComputeTerminalBonus("POL-022", 1000000m, 5, "Endowment");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.AreEqual("Terminal", result.BonusType);
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeTerminalBonus_Exactly10Years_Returns1Percent()
        {
            var result = _service.ComputeTerminalBonus("POL-023", 1000000m, 10, "MoneyBack");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(10000m, result.Amount);
            Assert.AreEqual("Terminal", result.BonusType);
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeTerminalBonus_Exactly15Years_Returns3Percent()
        {
            var result = _service.ComputeTerminalBonus("POL-024", 1000000m, 15, "WholeLife");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(30000m, result.Amount);
            Assert.AreEqual("Terminal", result.BonusType);
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void GetBonusRate_NullType_ReturnsZero()
        {
            var rate = _service.GetBonusRate(null, 10);
            Assert.AreEqual(0m, rate);
            Assert.IsFalse(rate > 0);
            Assert.AreEqual(0m, _service.GetBonusRate(null, 5));
            Assert.AreEqual(0m, _service.GetBonusRate(null, 1));
        }

        [TestMethod]
        public void GetBonusRate_EmptyType_ReturnsZero()
        {
            var rate = _service.GetBonusRate("", 10);
            Assert.AreEqual(0m, rate);
            Assert.IsFalse(rate > 0);
            Assert.AreEqual(0m, _service.GetBonusRate("", 5));
            Assert.AreEqual(0m, _service.GetBonusRate("  ", 10));
        }

        [TestMethod]
        public void GetBonusRate_ZeroYear_ReturnsZero()
        {
            var rate = _service.GetBonusRate("Endowment", 0);
            Assert.AreEqual(0m, rate);
            Assert.IsFalse(rate > 0);
            Assert.AreEqual(0m, _service.GetBonusRate("MoneyBack", 0));
            Assert.AreEqual(0m, _service.GetBonusRate("WholeLife", -1));
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_ZeroSumAssured_ReturnsZero()
        {
            var loyalty = _service.CalculateLoyaltyAddition(0m, 15, "Endowment");
            Assert.AreEqual(0m, loyalty);
            Assert.IsFalse(loyalty > 0);
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(-1000m, 15, "Endowment"));
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(0m, 20, "WholeLife"));
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_NullPolicyType_ReturnsZero()
        {
            var loyalty = _service.CalculateLoyaltyAddition(1000000m, 15, null);
            Assert.AreEqual(0m, loyalty);
            Assert.IsFalse(loyalty > 0);
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(1000000m, 15, ""));
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(1000000m, 15, "  "));
        }

        [TestMethod]
        public void CalculateInterimBonus_ZeroSumAssured_ReturnsZero()
        {
            var interim = _service.CalculateInterimBonus(0m, 45m, 180);
            Assert.AreEqual(0m, interim);
            Assert.IsFalse(interim > 0);
            Assert.AreEqual(0m, _service.CalculateInterimBonus(-100m, 45m, 180));
            Assert.AreEqual(0m, _service.CalculateInterimBonus(0m, 0m, 180));
        }

        [TestMethod]
        public void CalculateInterimBonus_ZeroBonusRate_ReturnsZero()
        {
            var interim = _service.CalculateInterimBonus(500000m, 0m, 180);
            Assert.AreEqual(0m, interim);
            Assert.IsFalse(interim > 0);
            Assert.AreEqual(0m, _service.CalculateInterimBonus(500000m, -10m, 180));
            Assert.AreEqual(0m, _service.CalculateInterimBonus(500000m, 0m, 365));
        }

        [TestMethod]
        public void CalculateInterimBonus_ZeroDays_ReturnsZero()
        {
            var interim = _service.CalculateInterimBonus(500000m, 45m, 0);
            Assert.AreEqual(0m, interim);
            Assert.IsFalse(interim > 0);
            Assert.AreEqual(0m, _service.CalculateInterimBonus(500000m, 45m, -10));
            Assert.AreEqual(0m, _service.CalculateInterimBonus(1000000m, 50m, 0));
        }

        [TestMethod]
        public void GetBonusBreakdown_NullPolicy_ReturnsFailed()
        {
            var breakdown = _service.GetBonusBreakdown(null);
            Assert.IsFalse(breakdown.Success);
            Assert.IsNotNull(breakdown.Message);
            Assert.AreEqual(0m, breakdown.Amount);
            Assert.IsNull(breakdown.ReferenceId);
        }

        [TestMethod]
        public void GetBonusBreakdown_EmptyPolicy_ReturnsFailed()
        {
            var breakdown = _service.GetBonusBreakdown("");
            Assert.IsFalse(breakdown.Success);
            Assert.IsNotNull(breakdown.Message);
            Assert.AreEqual(0m, breakdown.Amount);
            Assert.IsNull(breakdown.ReferenceId);
        }

        [TestMethod]
        public void ValidateBonusComputation_ExceedsMaximum_ReturnsFalse()
        {
            Assert.IsFalse(_service.ValidateBonusComputation("POL-080", 50000001m));
            Assert.IsFalse(_service.ValidateBonusComputation("POL-081", 100000000m));
            Assert.IsFalse(_service.ValidateBonusComputation("POL-082", decimal.MaxValue));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-083", 50000000m));
        }

        [TestMethod]
        public void CalculateTotalAccruedBonus_ZeroInputs_ReturnsZero()
        {
            Assert.AreEqual(0m, _service.CalculateTotalAccruedBonus(0m, 45m, 10));
            Assert.AreEqual(0m, _service.CalculateTotalAccruedBonus(500000m, 0m, 10));
            Assert.AreEqual(0m, _service.CalculateTotalAccruedBonus(500000m, 45m, 0));
            Assert.AreEqual(0m, _service.CalculateTotalAccruedBonus(-1m, 45m, 10));
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
    }
}
