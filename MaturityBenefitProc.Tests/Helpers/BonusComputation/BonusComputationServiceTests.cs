using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.BonusComputation;

namespace MaturityBenefitProc.Tests.Helpers.BonusComputation
{
    [TestClass]
    public class BonusComputationServiceTests
    {
        private BonusComputationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new BonusComputationService();
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_ValidEndowment_ReturnsCorrectAmount()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-001", 500000m, 10);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("SimpleReversionary", result.BonusType);
            Assert.AreEqual(225000m, result.Amount);
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_LargeSum_ComputesCorrectly()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-002", 1000000m, 20);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(900000m, result.Amount);
            Assert.AreEqual(45m, result.BonusRate);
            Assert.AreEqual(20, result.PolicyYear);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_SmallSum_ComputesCorrectly()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-003", 100000m, 5);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(22500m, result.Amount);
            Assert.AreEqual(100000m, result.SumAssured);
            Assert.IsTrue(result.Message.Contains("successfully"));
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_OneYear_MinimalBonus()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-004", 200000m, 1);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(9000m, result.Amount);
            Assert.AreEqual(1, result.PolicyYear);
            Assert.IsNotNull(result.ProcessedDate);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_SetsAccruedAmount()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-005", 300000m, 8);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(108000m, result.Amount);
            Assert.AreEqual(108000m, result.AccruedAmount);
            Assert.IsTrue(result.ReferenceId.StartsWith("BNS-SRB-"));
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_TwoYears_CorrectCalculation()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-006", 750000m, 2);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(67500m, result.Amount);
            Assert.AreEqual(2, result.PolicyYear);
            Assert.AreEqual(750000m, result.SumAssured);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_FifteenYears_CorrectBonus()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-007", 400000m, 15);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(270000m, result.Amount);
            Assert.AreEqual(45m, result.BonusRate);
            Assert.AreEqual(15, result.PolicyYear);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_ThirtyYears_HighBonus()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-008", 600000m, 30);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(810000m, result.Amount);
            Assert.IsTrue(result.Amount > result.SumAssured);
            Assert.AreEqual("SimpleReversionary", result.BonusType);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_WithExistingBonus_CompoundsCorrectly()
        {
            var result = _service.ComputeCompoundReversionaryBonus("POL-010", 500000m, 50000m, 10);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("CompoundReversionary", result.BonusType);
            Assert.AreEqual(275000m, result.Amount);
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_NoExistingBonus_UsesOnlySumAssured()
        {
            var result = _service.ComputeCompoundReversionaryBonus("POL-011", 400000m, 0m, 5);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(100000m, result.Amount);
            Assert.AreEqual(50m, result.BonusRate);
            Assert.AreEqual(5, result.PolicyYear);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_LargeExistingBonus_IncludesInBase()
        {
            var result = _service.ComputeCompoundReversionaryBonus("POL-012", 1000000m, 200000m, 15);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(900000m, result.Amount);
            Assert.AreEqual(1000000m, result.SumAssured);
            Assert.IsTrue(result.AccruedAmount > 0);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_NegativeExistingBonus_IgnoresNegative()
        {
            var result = _service.ComputeCompoundReversionaryBonus("POL-013", 500000m, -10000m, 10);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(250000m, result.Amount);
            Assert.IsTrue(result.ReferenceId.StartsWith("BNS-CRB-"));
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_SmallExistingBonus_CompoundsCorrectly()
        {
            var result = _service.ComputeCompoundReversionaryBonus("POL-014", 800000m, 10000m, 3);
            Assert.IsTrue(result.Success);
            var expectedBase = 800000m + 10000m;
            var expected = expectedBase * 50m / 1000m * 3m;
            Assert.AreEqual(expected, result.Amount);
            Assert.AreEqual(50m, result.BonusRate);
        }

        [TestMethod]
        public void ComputeCompoundReversionaryBonus_TwentyYears_LargeBonus()
        {
            var result = _service.ComputeCompoundReversionaryBonus("POL-015", 2000000m, 500000m, 20);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Amount > 2000000m);
            Assert.AreEqual("CompoundReversionary", result.BonusType);
            Assert.AreEqual(20, result.PolicyYear);
        }

        [TestMethod]
        public void ComputeTerminalBonus_Over15Years_Returns5Percent()
        {
            var result = _service.ComputeTerminalBonus("POL-020", 1000000m, 20, "Endowment");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(50000m, result.Amount);
            Assert.AreEqual("Terminal", result.BonusType);
            Assert.AreEqual(20, result.PolicyYear);
        }

        [TestMethod]
        public void ComputeTerminalBonus_Over10Years_Returns3Percent()
        {
            var result = _service.ComputeTerminalBonus("POL-021", 1000000m, 12, "Endowment");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(30000m, result.Amount);
            Assert.AreEqual(30m, result.BonusRate);
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void ComputeTerminalBonus_Over5Years_Returns1Percent()
        {
            var result = _service.ComputeTerminalBonus("POL-022", 1000000m, 7, "MoneyBack");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(10000m, result.Amount);
            Assert.AreEqual(1000000m, result.SumAssured);
            Assert.IsTrue(result.AccruedAmount > 0);
        }

        [TestMethod]
        public void ComputeTerminalBonus_Under5Years_ReturnsZero()
        {
            var result = _service.ComputeTerminalBonus("POL-023", 1000000m, 3, "Endowment");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0m, result.Amount);
            Assert.AreEqual("Terminal", result.BonusType);
            Assert.IsTrue(result.ReferenceId.StartsWith("BNS-TRB-"));
        }

        [TestMethod]
        public void ComputeTerminalBonus_Exactly16Years_Gets5Percent()
        {
            var result = _service.ComputeTerminalBonus("POL-024", 2000000m, 16, "WholeLife");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(100000m, result.Amount);
            Assert.AreEqual("Terminal", result.BonusType);
            Assert.AreEqual(2000000m, result.SumAssured);
        }

        [TestMethod]
        public void ComputeTerminalBonus_Exactly11Years_Gets3Percent()
        {
            var result = _service.ComputeTerminalBonus("POL-025", 500000m, 11, "Endowment");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(15000m, result.Amount);
            Assert.AreEqual(11, result.PolicyYear);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ComputeTerminalBonus_Exactly6Years_Gets1Percent()
        {
            var result = _service.ComputeTerminalBonus("POL-026", 500000m, 6, "MoneyBack");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(5000m, result.Amount);
            Assert.AreEqual(6, result.PolicyYear);
            Assert.AreEqual(500000m, result.SumAssured);
        }

        [TestMethod]
        public void GetBonusRate_Endowment_Returns45()
        {
            var rate = _service.GetBonusRate("Endowment", 10);
            Assert.AreEqual(45m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate <= 100m);
            Assert.AreEqual(45m, _service.GetBonusRate("Endowment", 5));
        }

        [TestMethod]
        public void GetBonusRate_MoneyBack_Returns38()
        {
            var rate = _service.GetBonusRate("MoneyBack", 10);
            Assert.AreEqual(38m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate < 45m);
            Assert.AreEqual(38m, _service.GetBonusRate("MoneyBack", 15));
        }

        [TestMethod]
        public void GetBonusRate_WholeLife_Returns50()
        {
            var rate = _service.GetBonusRate("WholeLife", 10);
            Assert.AreEqual(50m, rate);
            Assert.IsTrue(rate > 45m);
            Assert.IsTrue(rate <= 100m);
            Assert.AreEqual(50m, _service.GetBonusRate("WholeLife", 20));
        }

        [TestMethod]
        public void GetBonusRate_DefaultType_Returns40()
        {
            var rate = _service.GetBonusRate("UnknownPlan", 10);
            Assert.AreEqual(40m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate < 50m);
            Assert.AreEqual(40m, _service.GetBonusRate("CustomPlan", 5));
        }

        [TestMethod]
        public void GetBonusRate_TermPlan_Returns30()
        {
            var rate = _service.GetBonusRate("TermPlan", 10);
            Assert.AreEqual(30m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate < 38m);
            Assert.AreEqual(30m, _service.GetBonusRate("TermPlan", 1));
        }

        [TestMethod]
        public void GetBonusRate_ChildPlan_Returns42()
        {
            var rate = _service.GetBonusRate("ChildPlan", 10);
            Assert.AreEqual(42m, rate);
            Assert.IsTrue(rate > 40m);
            Assert.IsTrue(rate < 45m);
            Assert.AreEqual(42m, _service.GetBonusRate("ChildPlan", 20));
        }

        [TestMethod]
        public void GetBonusRate_PensionPlan_Returns48()
        {
            var rate = _service.GetBonusRate("PensionPlan", 10);
            Assert.AreEqual(48m, rate);
            Assert.IsTrue(rate > 45m);
            Assert.IsTrue(rate < 50m);
            Assert.AreEqual(48m, _service.GetBonusRate("PensionPlan", 15));
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_15PlusYears_Returns5Percent()
        {
            var loyalty = _service.CalculateLoyaltyAddition(1000000m, 15, "Endowment");
            Assert.AreEqual(50000m, loyalty);
            Assert.IsTrue(loyalty > 0);
            Assert.IsTrue(loyalty <= 1000000m);
            Assert.AreEqual(50000m, _service.CalculateLoyaltyAddition(1000000m, 20, "Endowment"));
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_10To14Years_Returns3Percent()
        {
            var loyalty = _service.CalculateLoyaltyAddition(1000000m, 12, "Endowment");
            Assert.AreEqual(30000m, loyalty);
            Assert.IsTrue(loyalty > 0);
            Assert.IsTrue(loyalty < 50000m);
            Assert.AreEqual(30000m, _service.CalculateLoyaltyAddition(1000000m, 10, "WholeLife"));
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_Under10Years_ReturnsZero()
        {
            var loyalty = _service.CalculateLoyaltyAddition(1000000m, 5, "Endowment");
            Assert.AreEqual(0m, loyalty);
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(500000m, 9, "MoneyBack"));
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(200000m, 1, "WholeLife"));
            Assert.AreEqual(0m, _service.CalculateLoyaltyAddition(800000m, 8, "TermPlan"));
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_VeryLargeSum_CalculatesCorrectly()
        {
            var loyalty = _service.CalculateLoyaltyAddition(50000000m, 15, "Endowment");
            Assert.AreEqual(2500000m, loyalty);
            Assert.IsTrue(loyalty > 0);
            Assert.IsTrue(loyalty < 50000000m);
            Assert.AreEqual(2500000m, _service.CalculateLoyaltyAddition(50000000m, 20, "WholeLife"));
        }

        [TestMethod]
        public void IsBonusEligible_MinimumYears3OrMore_ReturnsTrue()
        {
            Assert.IsTrue(_service.IsBonusEligible("POL-030", 3));
            Assert.IsTrue(_service.IsBonusEligible("POL-031", 5));
            Assert.IsTrue(_service.IsBonusEligible("POL-032", 10));
            Assert.IsTrue(_service.IsBonusEligible("POL-033", 20));
        }

        [TestMethod]
        public void IsBonusEligible_Under3Years_ReturnsFalse()
        {
            Assert.IsFalse(_service.IsBonusEligible("POL-034", 2));
            Assert.IsFalse(_service.IsBonusEligible("POL-035", 1));
            Assert.IsFalse(_service.IsBonusEligible("POL-036", 0));
            Assert.IsFalse(_service.IsBonusEligible("", 5));
        }

        [TestMethod]
        public void IsBonusEligible_LargeYears_StillReturnsTrue()
        {
            Assert.IsTrue(_service.IsBonusEligible("POL-037", 50));
            Assert.IsTrue(_service.IsBonusEligible("POL-038", 100));
            Assert.IsTrue(_service.IsBonusEligible("POL-039", 30));
            Assert.IsTrue(_service.IsBonusEligible("POL-040", 25));
        }

        [TestMethod]
        public void CalculateInterimBonus_ValidInputs_ReturnsCorrectAmount()
        {
            var interim = _service.CalculateInterimBonus(500000m, 45m, 180);
            Assert.IsTrue(interim > 0);
            var expected = Math.Round(500000m * 45m / 1000m * 180m / 365m, 2);
            Assert.AreEqual(expected, interim);
            Assert.IsTrue(interim < 500000m);
            Assert.IsTrue(interim > 10000m);
        }

        [TestMethod]
        public void CalculateInterimBonus_FullYear_EqualsAnnualRate()
        {
            var interim = _service.CalculateInterimBonus(1000000m, 45m, 365);
            Assert.AreEqual(45000m, interim);
            Assert.IsTrue(interim > 0);
            Assert.AreEqual(1000000m * 45m / 1000m, interim);
            Assert.IsTrue(interim <= 1000000m);
        }

        [TestMethod]
        public void CalculateInterimBonus_QuarterYear_ReturnsQuarterBonus()
        {
            var interim = _service.CalculateInterimBonus(1000000m, 40m, 91);
            Assert.IsTrue(interim > 0);
            var expected = Math.Round(1000000m * 40m / 1000m * 91m / 365m, 2);
            Assert.AreEqual(expected, interim);
            Assert.IsTrue(interim < 11000m);
            Assert.IsTrue(interim > 9000m);
        }

        [TestMethod]
        public void GetBonusBreakdown_AfterComputation_ReturnsBreakdown()
        {
            _service.ComputeSimpleReversionaryBonus("POL-050", 500000m, 10);
            var breakdown = _service.GetBonusBreakdown("POL-050");
            Assert.IsTrue(breakdown.Success);
            Assert.AreEqual("Breakdown", breakdown.BonusType);
            Assert.IsTrue(breakdown.Amount > 0);
            Assert.IsNotNull(breakdown.Metadata);
        }

        [TestMethod]
        public void GetBonusBreakdown_NoHistory_ReturnsFailed()
        {
            var breakdown = _service.GetBonusBreakdown("POL-NONE");
            Assert.IsFalse(breakdown.Success);
            Assert.IsTrue(breakdown.Message.Contains("No bonus records"));
            Assert.AreEqual(0m, breakdown.Amount);
            Assert.IsNull(breakdown.ReferenceId);
        }

        [TestMethod]
        public void GetMaximumBonusRate_VariousTypes_ReturnsCorrectMax()
        {
            Assert.AreEqual(58m, _service.GetMaximumBonusRate("Endowment"));
            Assert.AreEqual(48m, _service.GetMaximumBonusRate("MoneyBack"));
            Assert.AreEqual(65m, _service.GetMaximumBonusRate("WholeLife"));
            Assert.AreEqual(52m, _service.GetMaximumBonusRate("SomeOther"));
        }

        [TestMethod]
        public void GetMaximumBonusRate_ChildAndPension_ReturnsCorrect()
        {
            Assert.AreEqual(55m, _service.GetMaximumBonusRate("ChildPlan"));
            Assert.AreEqual(60m, _service.GetMaximumBonusRate("PensionPlan"));
            Assert.AreEqual(38m, _service.GetMaximumBonusRate("TermPlan"));
            Assert.IsTrue(_service.GetMaximumBonusRate("PensionPlan") > _service.GetMaximumBonusRate("ChildPlan"));
        }

        [TestMethod]
        public void GetMinimumBonusRate_VariousTypes_ReturnsCorrectMin()
        {
            Assert.AreEqual(32m, _service.GetMinimumBonusRate("Endowment"));
            Assert.AreEqual(28m, _service.GetMinimumBonusRate("MoneyBack"));
            Assert.AreEqual(35m, _service.GetMinimumBonusRate("WholeLife"));
            Assert.AreEqual(25m, _service.GetMinimumBonusRate("UnknownType"));
        }

        [TestMethod]
        public void GetMinimumBonusRate_ChildAndPension_ReturnsCorrect()
        {
            Assert.AreEqual(30m, _service.GetMinimumBonusRate("ChildPlan"));
            Assert.AreEqual(34m, _service.GetMinimumBonusRate("PensionPlan"));
            Assert.AreEqual(20m, _service.GetMinimumBonusRate("TermPlan"));
            Assert.IsTrue(_service.GetMinimumBonusRate("PensionPlan") > _service.GetMinimumBonusRate("ChildPlan"));
        }

        [TestMethod]
        public void ValidateBonusComputation_PositiveAmount_ReturnsTrue()
        {
            Assert.IsTrue(_service.ValidateBonusComputation("POL-060", 1000m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-061", 50000000m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-062", 0.01m));
            Assert.IsTrue(_service.ValidateBonusComputation("POL-063", 25000000m));
        }

        [TestMethod]
        public void ValidateBonusComputation_ZeroOrNegative_ReturnsFalse()
        {
            Assert.IsFalse(_service.ValidateBonusComputation("POL-064", 0m));
            Assert.IsFalse(_service.ValidateBonusComputation("POL-065", -1000m));
            Assert.IsFalse(_service.ValidateBonusComputation("", 5000m));
            Assert.IsFalse(_service.ValidateBonusComputation("POL-066", 50000001m));
        }

        [TestMethod]
        public void GetBonusHistory_WithRecords_ReturnsFiltered()
        {
            _service.ComputeSimpleReversionaryBonus("POL-070", 500000m, 10);
            var history = _service.GetBonusHistory("POL-070", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.IsTrue(history.Count > 0);
            Assert.IsTrue(history[0].Success);
            Assert.AreEqual("SimpleReversionary", history[0].BonusType);
            Assert.IsNotNull(history[0].ReferenceId);
        }

        [TestMethod]
        public void GetBonusHistory_NoRecords_ReturnsEmpty()
        {
            var history = _service.GetBonusHistory("POL-EMPTY", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(0, history.Count);
            Assert.IsNotNull(history);
            Assert.IsFalse(history.Any());
            Assert.IsInstanceOfType(history, typeof(List<BonusComputationResult>));
        }

        [TestMethod]
        public void GetBonusHistory_MultipleRecords_ReturnsAll()
        {
            _service.ComputeSimpleReversionaryBonus("POL-071", 100000m, 5);
            _service.ComputeCompoundReversionaryBonus("POL-071", 100000m, 10000m, 5);
            _service.ComputeTerminalBonus("POL-071", 100000m, 20, "Endowment");
            var history = _service.GetBonusHistory("POL-071", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(3, history.Count);
            Assert.IsTrue(history.All(h => h.Success));
            Assert.AreEqual("SimpleReversionary", history[0].BonusType);
            Assert.AreEqual("CompoundReversionary", history[1].BonusType);
        }

        [TestMethod]
        public void GetBonusDeclarationYear_AfterComputation_ReturnsCurrentYear()
        {
            _service.ComputeSimpleReversionaryBonus("POL-080", 500000m, 10);
            var year = _service.GetBonusDeclarationYear("POL-080");
            Assert.AreEqual(DateTime.UtcNow.Year, year);
            Assert.IsTrue(year > 2000);
            Assert.IsTrue(year <= 2030);
            Assert.AreNotEqual(0, year);
        }

        [TestMethod]
        public void GetBonusDeclarationYear_NoComputation_ReturnsZero()
        {
            var year = _service.GetBonusDeclarationYear("POL-NONE");
            Assert.AreEqual(0, year);
            Assert.IsFalse(year > 0);
            Assert.AreEqual(0, _service.GetBonusDeclarationYear(""));
            Assert.AreEqual(0, _service.GetBonusDeclarationYear(null));
        }

        [TestMethod]
        public void CalculateTotalAccruedBonus_ValidInputs_ReturnsCorrect()
        {
            var total = _service.CalculateTotalAccruedBonus(500000m, 45m, 10);
            Assert.AreEqual(225000m, total);
            Assert.IsTrue(total > 0);
            Assert.IsTrue(total < 500000m);
            Assert.AreEqual(225000m, Math.Round(total, 2));
        }

        [TestMethod]
        public void CalculateTotalAccruedBonus_LargeSumAndYears_ReturnsCorrect()
        {
            var total = _service.CalculateTotalAccruedBonus(2000000m, 50m, 20);
            Assert.AreEqual(2000000m, total);
            Assert.IsTrue(total > 0);
            var expected = 2000000m * 50m / 1000m * 20m;
            Assert.AreEqual(expected, total);
        }

        [TestMethod]
        public void CalculateTotalAccruedBonus_SmallValues_ReturnsCorrect()
        {
            var total = _service.CalculateTotalAccruedBonus(100000m, 30m, 1);
            Assert.AreEqual(3000m, total);
            Assert.IsTrue(total > 0);
            Assert.IsTrue(total < 100000m);
            Assert.AreEqual(3000m, Math.Round(total, 2));
        }

        [TestMethod]
        public void GetAccruedBonusAmount_AfterMultipleComputations_SumsCorrectly()
        {
            _service.ComputeSimpleReversionaryBonus("POL-090", 100000m, 5);
            _service.ComputeSimpleReversionaryBonus("POL-090", 100000m, 5);
            var accrued = _service.GetAccruedBonusAmount("POL-090");
            Assert.AreEqual(45000m, accrued);
            Assert.IsTrue(accrued > 0);
            Assert.IsTrue(accrued > 22500m);
            Assert.AreEqual(45000m, Math.Round(accrued, 2));
        }

        [TestMethod]
        public void GetAccruedBonusAmount_NoPriorComputation_ReturnsZero()
        {
            var accrued = _service.GetAccruedBonusAmount("POL-NONE");
            Assert.AreEqual(0m, accrued);
            Assert.IsFalse(accrued > 0);
            Assert.AreEqual(0m, _service.GetAccruedBonusAmount(""));
            Assert.AreEqual(0m, _service.GetAccruedBonusAmount(null));
        }

        [TestMethod]
        public void GetAccruedBonusAmount_SingleComputation_MatchesResult()
        {
            var result = _service.ComputeSimpleReversionaryBonus("POL-091", 800000m, 12);
            var accrued = _service.GetAccruedBonusAmount("POL-091");
            Assert.AreEqual(result.Amount, accrued);
            Assert.IsTrue(accrued > 0);
            Assert.AreEqual(432000m, accrued);
            Assert.AreEqual(result.AccruedAmount, accrued);
        }

        [TestMethod]
        public void ComputeSimpleReversionaryBonus_ReturnsUniqueReferenceIds()
        {
            var r1 = _service.ComputeSimpleReversionaryBonus("POL-100A", 100000m, 5);
            var r2 = _service.ComputeSimpleReversionaryBonus("POL-100B", 200000m, 10);
            Assert.AreNotEqual(r1.ReferenceId, r2.ReferenceId);
            Assert.IsTrue(r1.ReferenceId.StartsWith("BNS-SRB-"));
            Assert.IsTrue(r2.ReferenceId.StartsWith("BNS-SRB-"));
            Assert.IsNotNull(r1.ReferenceId);
        }

        [TestMethod]
        public void ComputeTerminalBonus_VeryLargeSum_CorrectPercentage()
        {
            var result = _service.ComputeTerminalBonus("POL-110", 50000000m, 20, "Endowment");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(2500000m, result.Amount);
            Assert.AreEqual("Terminal", result.BonusType);
            Assert.AreEqual(50000000m, result.SumAssured);
        }
    }
}
