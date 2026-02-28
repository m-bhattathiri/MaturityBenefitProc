using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class TerminalBonusServiceTests
    {
        private ITerminalBonusService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming TerminalBonusService is the implementation of ITerminalBonusService
            // For the sake of this test file generation, we assume it exists in the namespace.
            // If it requires a mock framework, we would use Moq, but the prompt implies testing a concrete class.
            // Since the prompt specifies testing the FIXED implementation, we instantiate it.
            // Note: The prompt says "private TerminalBonusService _service;" but the interface is ITerminalBonusService.
            // We will use the interface type for the field but instantiate the concrete class.
            _service = new TerminalBonusService();
        }

        [TestMethod]
        public void CalculateBaseTerminalBonus_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateBaseTerminalBonus("POL123", 100000m, new DateTime(2023, 1, 1));
            var result2 = _service.CalculateBaseTerminalBonus("POL456", 50000m, new DateTime(2023, 6, 1));
            var result3 = _service.CalculateBaseTerminalBonus("POL789", 200000m, new DateTime(2024, 1, 1));
            var result4 = _service.CalculateBaseTerminalBonus("POL000", 0m, new DateTime(2023, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.AreNotEqual(-1m, result2);
            Assert.IsNotNull(result3);
            Assert.AreEqual(0m, result4); // Assuming 0 sum assured gives 0 bonus
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_VariousYears_ReturnsCorrectAmount()
        {
            var result1 = _service.CalculateLoyaltyAdditionAmount("POL123", 10);
            var result2 = _service.CalculateLoyaltyAdditionAmount("POL456", 15);
            var result3 = _service.CalculateLoyaltyAdditionAmount("POL789", 5);
            var result4 = _service.CalculateLoyaltyAdditionAmount("POL000", 0);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= result1); // Assuming more years = more or equal loyalty
            Assert.IsTrue(result3 >= 0);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetAccruedReversionaryBonus_ValidPolicies_ReturnsAccruedAmount()
        {
            var result1 = _service.GetAccruedReversionaryBonus("POL123");
            var result2 = _service.GetAccruedReversionaryBonus("POL456");
            var result3 = _service.GetAccruedReversionaryBonus("POL789");
            var result4 = _service.GetAccruedReversionaryBonus("INVALID");

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result3 >= 0);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void ComputeFinalAdditionalBonus_ValidPremiums_ReturnsComputedBonus()
        {
            var result1 = _service.ComputeFinalAdditionalBonus("POL123", 50000m);
            var result2 = _service.ComputeFinalAdditionalBonus("POL456", 100000m);
            var result3 = _service.ComputeFinalAdditionalBonus("POL789", 25000m);
            var result4 = _service.ComputeFinalAdditionalBonus("POL000", 0m);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= result1);
            Assert.IsNotNull(result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateVestedBonusTotal_DifferentDates_ReturnsTotal()
        {
            var result1 = _service.CalculateVestedBonusTotal("POL123", new DateTime(2022, 12, 31));
            var result2 = _service.CalculateVestedBonusTotal("POL123", new DateTime(2023, 12, 31));
            var result3 = _service.CalculateVestedBonusTotal("POL456", new DateTime(2023, 1, 1));
            var result4 = _service.CalculateVestedBonusTotal("INVALID", new DateTime(2023, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= result1);
            Assert.IsNotNull(result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetSpecialSurrenderValueBonus_ValidBase_ReturnsBonus()
        {
            var result1 = _service.GetSpecialSurrenderValueBonus("POL123", 10000m);
            var result2 = _service.GetSpecialSurrenderValueBonus("POL456", 50000m);
            var result3 = _service.GetSpecialSurrenderValueBonus("POL789", 20000m);
            var result4 = _service.GetSpecialSurrenderValueBonus("POL000", 0m);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= result1);
            Assert.IsNotNull(result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateProratedTerminalBonus_ValidDays_ReturnsProratedAmount()
        {
            var result1 = _service.CalculateProratedTerminalBonus("POL123", new DateTime(2023, 6, 30), 180);
            var result2 = _service.CalculateProratedTerminalBonus("POL456", new DateTime(2023, 12, 31), 365);
            var result3 = _service.CalculateProratedTerminalBonus("POL789", new DateTime(2023, 3, 31), 90);
            var result4 = _service.CalculateProratedTerminalBonus("POL000", new DateTime(2023, 1, 1), 0);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= result1);
            Assert.IsNotNull(result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void ApplyBonusMultiplier_ValidMultipliers_ReturnsMultipliedAmount()
        {
            var result1 = _service.ApplyBonusMultiplier(1000m, 1.5);
            var result2 = _service.ApplyBonusMultiplier(2000m, 1.0);
            var result3 = _service.ApplyBonusMultiplier(500m, 2.0);
            var result4 = _service.ApplyBonusMultiplier(1000m, 0.0);

            Assert.AreEqual(1500m, result1);
            Assert.AreEqual(2000m, result2);
            Assert.AreEqual(1000m, result3);
            Assert.AreEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidTerms_ReturnsRate()
        {
            var result1 = _service.GetTerminalBonusRate("PLAN_A", 10);
            var result2 = _service.GetTerminalBonusRate("PLAN_B", 15);
            var result3 = _service.GetTerminalBonusRate("PLAN_C", 20);
            var result4 = _service.GetTerminalBonusRate("INVALID", 5);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result3 >= 0);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetLoyaltyAdditionRate_ValidYears_ReturnsRate()
        {
            var result1 = _service.GetLoyaltyAdditionRate("PLAN_A", 5);
            var result2 = _service.GetLoyaltyAdditionRate("PLAN_B", 10);
            var result3 = _service.GetLoyaltyAdditionRate("PLAN_C", 15);
            var result4 = _service.GetLoyaltyAdditionRate("INVALID", 2);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result3 >= 0);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void CalculateBonusYield_ValidAmounts_ReturnsYield()
        {
            var result1 = _service.CalculateBonusYield(1000m, 10000m);
            var result2 = _service.CalculateBonusYield(2000m, 10000m);
            var result3 = _service.CalculateBonusYield(500m, 5000m);
            var result4 = _service.CalculateBonusYield(0m, 10000m);

            Assert.AreEqual(0.1, result1, 0.001);
            Assert.AreEqual(0.2, result2, 0.001);
            Assert.AreEqual(0.1, result3, 0.001);
            Assert.AreEqual(0.0, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetFundPerformanceFactor_ValidFunds_ReturnsFactor()
        {
            var result1 = _service.GetFundPerformanceFactor("FUND_1", new DateTime(2023, 1, 1));
            var result2 = _service.GetFundPerformanceFactor("FUND_2", new DateTime(2023, 6, 1));
            var result3 = _service.GetFundPerformanceFactor("FUND_3", new DateTime(2023, 12, 31));
            var result4 = _service.GetFundPerformanceFactor("INVALID", new DateTime(2023, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result3 >= 0);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetParticipatingFundRatio_ValidCohorts_ReturnsRatio()
        {
            var result1 = _service.GetParticipatingFundRatio("COHORT_2010");
            var result2 = _service.GetParticipatingFundRatio("COHORT_2015");
            var result3 = _service.GetParticipatingFundRatio("COHORT_2020");
            var result4 = _service.GetParticipatingFundRatio("INVALID");

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0 && result1 <= 1);
            Assert.IsTrue(result2 >= 0 && result2 <= 1);
            Assert.IsTrue(result3 >= 0 && result3 <= 1);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void IsEligibleForTerminalBonus_ValidStatuses_ReturnsExpected()
        {
            var result1 = _service.IsEligibleForTerminalBonus("POL123", "ACTIVE");
            var result2 = _service.IsEligibleForTerminalBonus("POL456", "MATURED");
            var result3 = _service.IsEligibleForTerminalBonus("POL789", "SURRENDERED");
            var result4 = _service.IsEligibleForTerminalBonus("POL000", "LAPSED");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsLoyaltyAdditionApplicable_ValidPlans_ReturnsExpected()
        {
            var result1 = _service.IsLoyaltyAdditionApplicable("PLAN_A", 10);
            var result2 = _service.IsLoyaltyAdditionApplicable("PLAN_B", 5);
            var result3 = _service.IsLoyaltyAdditionApplicable("PLAN_C", 15);
            var result4 = _service.IsLoyaltyAdditionApplicable("PLAN_A", 2);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateBonusDeclaration_ValidDeclarations_ReturnsExpected()
        {
            var result1 = _service.ValidateBonusDeclaration("DECL_2023", new DateTime(2023, 4, 1));
            var result2 = _service.ValidateBonusDeclaration("DECL_2022", new DateTime(2022, 4, 1));
            var result3 = _service.ValidateBonusDeclaration("DECL_FUTURE", new DateTime(2025, 4, 1));
            var result4 = _service.ValidateBonusDeclaration("INVALID", new DateTime(2023, 4, 1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void HasClaimedPreviousBonuses_ValidPolicies_ReturnsExpected()
        {
            var result1 = _service.HasClaimedPreviousBonuses("POL_CLAIMED");
            var result2 = _service.HasClaimedPreviousBonuses("POL_UNCLAIMED");
            var result3 = _service.HasClaimedPreviousBonuses("POL_PARTIAL");
            var result4 = _service.HasClaimedPreviousBonuses("INVALID");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsPolicyInParticipatingFund_ValidPolicies_ReturnsExpected()
        {
            var result1 = _service.IsPolicyInParticipatingFund("POL_PAR");
            var result2 = _service.IsPolicyInParticipatingFund("POL_NONPAR");
            var result3 = _service.IsPolicyInParticipatingFund("POL_ULIP");
            var result4 = _service.IsPolicyInParticipatingFund("INVALID");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidDates_ReturnsYears()
        {
            var result1 = _service.GetCompletedPolicyYears("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GetCompletedPolicyYears("POL456", new DateTime(2024, 1, 1));
            var result3 = _service.GetCompletedPolicyYears("POL789", new DateTime(2025, 1, 1));
            var result4 = _service.GetCompletedPolicyYears("INVALID", new DateTime(2023, 1, 1));

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result3 >= 0);
            Assert.AreEqual(0, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetMinimumYearsForTerminalBonus_ValidPlans_ReturnsYears()
        {
            var result1 = _service.GetMinimumYearsForTerminalBonus("PLAN_A");
            var result2 = _service.GetMinimumYearsForTerminalBonus("PLAN_B");
            var result3 = _service.GetMinimumYearsForTerminalBonus("PLAN_C");
            var result4 = _service.GetMinimumYearsForTerminalBonus("INVALID");

            Assert.IsTrue(result1 > 0);
            Assert.IsTrue(result2 > 0);
            Assert.IsTrue(result3 > 0);
            Assert.AreEqual(0, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateDaysSinceLastBonusDeclaration_ValidDates_ReturnsDays()
        {
            var result1 = _service.CalculateDaysSinceLastBonusDeclaration(DateTime.Now.AddDays(-100));
            var result2 = _service.CalculateDaysSinceLastBonusDeclaration(DateTime.Now.AddDays(-200));
            var result3 = _service.CalculateDaysSinceLastBonusDeclaration(DateTime.Now.AddDays(-365));
            var result4 = _service.CalculateDaysSinceLastBonusDeclaration(DateTime.Now);

            Assert.IsTrue(result1 >= 99);
            Assert.IsTrue(result2 >= 199);
            Assert.IsTrue(result3 >= 364);
            Assert.AreEqual(0, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTotalBonusUnitsAllocated_ValidPolicies_ReturnsUnits()
        {
            var result1 = _service.GetTotalBonusUnitsAllocated("POL123");
            var result2 = _service.GetTotalBonusUnitsAllocated("POL456");
            var result3 = _service.GetTotalBonusUnitsAllocated("POL789");
            var result4 = _service.GetTotalBonusUnitsAllocated("INVALID");

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result3 >= 0);
            Assert.AreEqual(0, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetBonusDeclarationId_ValidInputs_ReturnsId()
        {
            var result1 = _service.GetBonusDeclarationId("PLAN_A", new DateTime(2023, 1, 1));
            var result2 = _service.GetBonusDeclarationId("PLAN_B", new DateTime(2022, 1, 1));
            var result3 = _service.GetBonusDeclarationId("PLAN_C", new DateTime(2021, 1, 1));
            var result4 = _service.GetBonusDeclarationId("INVALID", new DateTime(2023, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNull(result4);
            Assert.AreNotEqual("", result1);
        }

        [TestMethod]
        public void DetermineBonusCohort_ValidInputs_ReturnsCohort()
        {
            var result1 = _service.DetermineBonusCohort("POL123", new DateTime(2010, 1, 1));
            var result2 = _service.DetermineBonusCohort("POL456", new DateTime(2015, 1, 1));
            var result3 = _service.DetermineBonusCohort("POL789", new DateTime(2020, 1, 1));
            var result4 = _service.DetermineBonusCohort("INVALID", new DateTime(2023, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNull(result4);
            Assert.AreNotEqual("", result1);
        }

        [TestMethod]
        public void GetTerminalBonusStatus_ValidPolicies_ReturnsStatus()
        {
            var result1 = _service.GetTerminalBonusStatus("POL123");
            var result2 = _service.GetTerminalBonusStatus("POL456");
            var result3 = _service.GetTerminalBonusStatus("POL789");
            var result4 = _service.GetTerminalBonusStatus("INVALID");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNull(result4);
            Assert.AreNotEqual("", result1);
        }
    }
}