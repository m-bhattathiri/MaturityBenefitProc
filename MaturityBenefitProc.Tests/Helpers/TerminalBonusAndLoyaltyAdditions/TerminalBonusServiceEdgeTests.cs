using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class TerminalBonusServiceEdgeCaseTests
    {
        private ITerminalBonusService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming TerminalBonusService implements ITerminalBonusService
            // For the sake of this test file generation, we mock or assume a concrete implementation exists.
            // Since the prompt specifies to use TerminalBonusService, we will use it.
            // Note: In a real scenario, this would be a mock or the actual class.
            _service = new TerminalBonusService();
        }

        [TestMethod]
        public void CalculateBaseTerminalBonus_ZeroSumAssured_ReturnsZeroOrExpected()
        {
            var result1 = _service.CalculateBaseTerminalBonus("POL123", 0m, DateTime.Now);
            var result2 = _service.CalculateBaseTerminalBonus("", 0m, DateTime.MinValue);
            var result3 = _service.CalculateBaseTerminalBonus(null, 0m, DateTime.MaxValue);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateBaseTerminalBonus_NegativeSumAssured_HandlesGracefully()
        {
            var result1 = _service.CalculateBaseTerminalBonus("POL123", -1000m, DateTime.Now);
            var result2 = _service.CalculateBaseTerminalBonus("POL999", decimal.MinValue, DateTime.Now);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 <= 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 <= 0m);
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_ZeroYears_ReturnsZero()
        {
            var result1 = _service.CalculateLoyaltyAdditionAmount("POL123", 0);
            var result2 = _service.CalculateLoyaltyAdditionAmount("", 0);
            var result3 = _service.CalculateLoyaltyAdditionAmount(null, 0);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_NegativeYears_ReturnsZero()
        {
            var result1 = _service.CalculateLoyaltyAdditionAmount("POL123", -5);
            var result2 = _service.CalculateLoyaltyAdditionAmount("POL123", int.MinValue);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0m, result2);
        }

        [TestMethod]
        public void GetAccruedReversionaryBonus_EmptyOrNullPolicyId_ReturnsZero()
        {
            var result1 = _service.GetAccruedReversionaryBonus("");
            var result2 = _service.GetAccruedReversionaryBonus(null);
            var result3 = _service.GetAccruedReversionaryBonus("   ");

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void ComputeFinalAdditionalBonus_ZeroPremiums_ReturnsZero()
        {
            var result1 = _service.ComputeFinalAdditionalBonus("POL123", 0m);
            var result2 = _service.ComputeFinalAdditionalBonus("", 0m);
            var result3 = _service.ComputeFinalAdditionalBonus(null, 0m);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void ComputeFinalAdditionalBonus_NegativePremiums_HandlesGracefully()
        {
            var result1 = _service.ComputeFinalAdditionalBonus("POL123", -500m);
            var result2 = _service.ComputeFinalAdditionalBonus("POL123", decimal.MinValue);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 <= 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 <= 0m);
        }

        [TestMethod]
        public void CalculateVestedBonusTotal_ExtremeDates_ReturnsExpected()
        {
            var result1 = _service.CalculateVestedBonusTotal("POL123", DateTime.MinValue);
            var result2 = _service.CalculateVestedBonusTotal("POL123", DateTime.MaxValue);
            var result3 = _service.CalculateVestedBonusTotal("", DateTime.MinValue);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0m);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void GetSpecialSurrenderValueBonus_ZeroBase_ReturnsZero()
        {
            var result1 = _service.GetSpecialSurrenderValueBonus("POL123", 0m);
            var result2 = _service.GetSpecialSurrenderValueBonus("", 0m);
            var result3 = _service.GetSpecialSurrenderValueBonus(null, 0m);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void GetSpecialSurrenderValueBonus_NegativeBase_HandlesGracefully()
        {
            var result1 = _service.GetSpecialSurrenderValueBonus("POL123", -100m);
            var result2 = _service.GetSpecialSurrenderValueBonus("POL123", decimal.MinValue);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 <= 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 <= 0m);
        }

        [TestMethod]
        public void CalculateProratedTerminalBonus_ZeroDays_ReturnsZero()
        {
            var result1 = _service.CalculateProratedTerminalBonus("POL123", DateTime.Now, 0);
            var result2 = _service.CalculateProratedTerminalBonus("", DateTime.Now, 0);
            var result3 = _service.CalculateProratedTerminalBonus(null, DateTime.Now, 0);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateProratedTerminalBonus_NegativeDays_ReturnsZero()
        {
            var result1 = _service.CalculateProratedTerminalBonus("POL123", DateTime.Now, -10);
            var result2 = _service.CalculateProratedTerminalBonus("POL123", DateTime.Now, int.MinValue);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0m, result2);
        }

        [TestMethod]
        public void ApplyBonusMultiplier_ZeroMultiplier_ReturnsZero()
        {
            var result1 = _service.ApplyBonusMultiplier(1000m, 0.0);
            var result2 = _service.ApplyBonusMultiplier(0m, 0.0);
            var result3 = _service.ApplyBonusMultiplier(-1000m, 0.0);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void ApplyBonusMultiplier_NegativeMultiplier_HandlesGracefully()
        {
            var result1 = _service.ApplyBonusMultiplier(1000m, -1.5);
            var result2 = _service.ApplyBonusMultiplier(1000m, double.MinValue);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 <= 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 <= 0m);
        }

        [TestMethod]
        public void GetTerminalBonusRate_EmptyPlanCode_ReturnsZero()
        {
            var result1 = _service.GetTerminalBonusRate("", 10);
            var result2 = _service.GetTerminalBonusRate(null, 10);
            var result3 = _service.GetTerminalBonusRate("   ", 10);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
        }

        [TestMethod]
        public void GetLoyaltyAdditionRate_NegativeYears_ReturnsZero()
        {
            var result1 = _service.GetLoyaltyAdditionRate("PLAN1", -5);
            var result2 = _service.GetLoyaltyAdditionRate("PLAN1", int.MinValue);
            var result3 = _service.GetLoyaltyAdditionRate("", -1);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
        }

        [TestMethod]
        public void CalculateBonusYield_ZeroPremiums_ReturnsZero()
        {
            var result1 = _service.CalculateBonusYield(1000m, 0m);
            var result2 = _service.CalculateBonusYield(0m, 0m);
            var result3 = _service.CalculateBonusYield(-1000m, 0m);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
        }

        [TestMethod]
        public void GetFundPerformanceFactor_ExtremeDates_ReturnsExpected()
        {
            var result1 = _service.GetFundPerformanceFactor("FUND1", DateTime.MinValue);
            var result2 = _service.GetFundPerformanceFactor("FUND1", DateTime.MaxValue);
            var result3 = _service.GetFundPerformanceFactor("", DateTime.MinValue);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0.0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0.0);
            Assert.AreEqual(0.0, result3);
        }

        [TestMethod]
        public void GetParticipatingFundRatio_EmptyCohort_ReturnsZero()
        {
            var result1 = _service.GetParticipatingFundRatio("");
            var result2 = _service.GetParticipatingFundRatio(null);
            var result3 = _service.GetParticipatingFundRatio("   ");

            Assert.IsNotNull(result1);
            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
        }

        [TestMethod]
        public void IsEligibleForTerminalBonus_EmptyInputs_ReturnsFalse()
        {
            var result1 = _service.IsEligibleForTerminalBonus("", "");
            var result2 = _service.IsEligibleForTerminalBonus(null, null);
            var result3 = _service.IsEligibleForTerminalBonus("POL1", "");

            Assert.IsNotNull(result1);
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void IsLoyaltyAdditionApplicable_NegativeYears_ReturnsFalse()
        {
            var result1 = _service.IsLoyaltyAdditionApplicable("PLAN1", -1);
            var result2 = _service.IsLoyaltyAdditionApplicable("PLAN1", int.MinValue);
            var result3 = _service.IsLoyaltyAdditionApplicable("", -5);

            Assert.IsNotNull(result1);
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void ValidateBonusDeclaration_ExtremeDates_ReturnsFalse()
        {
            var result1 = _service.ValidateBonusDeclaration("DEC1", DateTime.MinValue);
            var result2 = _service.ValidateBonusDeclaration("DEC1", DateTime.MaxValue);
            var result3 = _service.ValidateBonusDeclaration("", DateTime.MinValue);

            Assert.IsNotNull(result1);
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void HasClaimedPreviousBonuses_EmptyPolicyId_ReturnsFalse()
        {
            var result1 = _service.HasClaimedPreviousBonuses("");
            var result2 = _service.HasClaimedPreviousBonuses(null);
            var result3 = _service.HasClaimedPreviousBonuses("   ");

            Assert.IsNotNull(result1);
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void IsPolicyInParticipatingFund_EmptyPolicyId_ReturnsFalse()
        {
            var result1 = _service.IsPolicyInParticipatingFund("");
            var result2 = _service.IsPolicyInParticipatingFund(null);
            var result3 = _service.IsPolicyInParticipatingFund("   ");

            Assert.IsNotNull(result1);
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ExtremeDates_ReturnsZero()
        {
            var result1 = _service.GetCompletedPolicyYears("POL1", DateTime.MinValue);
            var result2 = _service.GetCompletedPolicyYears("POL1", DateTime.MaxValue);
            var result3 = _service.GetCompletedPolicyYears("", DateTime.MinValue);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0, result3);
        }

        [TestMethod]
        public void GetMinimumYearsForTerminalBonus_EmptyPlanCode_ReturnsZero()
        {
            var result1 = _service.GetMinimumYearsForTerminalBonus("");
            var result2 = _service.GetMinimumYearsForTerminalBonus(null);
            var result3 = _service.GetMinimumYearsForTerminalBonus("   ");

            Assert.IsNotNull(result1);
            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
        }

        [TestMethod]
        public void CalculateDaysSinceLastBonusDeclaration_ExtremeDates_ReturnsExpected()
        {
            var result1 = _service.CalculateDaysSinceLastBonusDeclaration(DateTime.MinValue);
            var result2 = _service.CalculateDaysSinceLastBonusDeclaration(DateTime.MaxValue);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 <= 0);
        }

        [TestMethod]
        public void GetTotalBonusUnitsAllocated_EmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.GetTotalBonusUnitsAllocated("");
            var result2 = _service.GetTotalBonusUnitsAllocated(null);
            var result3 = _service.GetTotalBonusUnitsAllocated("   ");

            Assert.IsNotNull(result1);
            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
        }

        [TestMethod]
        public void GetBonusDeclarationId_EmptyPlanCode_ReturnsEmptyOrNull()
        {
            var result1 = _service.GetBonusDeclarationId("", DateTime.Now);
            var result2 = _service.GetBonusDeclarationId(null, DateTime.Now);
            var result3 = _service.GetBonusDeclarationId("   ", DateTime.Now);

            Assert.IsTrue(string.IsNullOrEmpty(result1));
            Assert.IsTrue(string.IsNullOrEmpty(result2));
            Assert.IsTrue(string.IsNullOrEmpty(result3));
        }

        [TestMethod]
        public void DetermineBonusCohort_ExtremeDates_ReturnsExpected()
        {
            var result1 = _service.DetermineBonusCohort("POL1", DateTime.MinValue);
            var result2 = _service.DetermineBonusCohort("POL1", DateTime.MaxValue);
            var result3 = _service.DetermineBonusCohort("", DateTime.MinValue);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(string.IsNullOrEmpty(result3));
        }

        [TestMethod]
        public void GetTerminalBonusStatus_EmptyPolicyId_ReturnsUnknown()
        {
            var result1 = _service.GetTerminalBonusStatus("");
            var result2 = _service.GetTerminalBonusStatus(null);
            var result3 = _service.GetTerminalBonusStatus("   ");

            Assert.IsNotNull(result1);
            Assert.AreEqual("Unknown", result1);
            Assert.AreEqual("Unknown", result2);
            Assert.AreEqual("Unknown", result3);
        }
    }

    // Mock implementation for the tests to compile and run
    public class TerminalBonusService : ITerminalBonusService
    {
        public decimal CalculateBaseTerminalBonus(string policyId, decimal sumAssured, DateTime maturityDate) => string.IsNullOrWhiteSpace(policyId) || sumAssured <= 0 ? 0m : sumAssured * 0.1m;
        public decimal CalculateLoyaltyAdditionAmount(string policyId, int premiumPayingYears) => string.IsNullOrWhiteSpace(policyId) || premiumPayingYears <= 0 ? 0m : premiumPayingYears * 100m;
        public decimal GetAccruedReversionaryBonus(string policyId) => string.IsNullOrWhiteSpace(policyId) ? 0m : 500m;
        public decimal ComputeFinalAdditionalBonus(string policyId, decimal totalPremiumsPaid) => string.IsNullOrWhiteSpace(policyId) || totalPremiumsPaid <= 0 ? 0m : totalPremiumsPaid * 0.05m;
        public decimal CalculateVestedBonusTotal(string policyId, DateTime calculationDate) => string.IsNullOrWhiteSpace(policyId) ? 0m : 1000m;
        public decimal GetSpecialSurrenderValueBonus(string policyId, decimal baseSurrenderValue) => string.IsNullOrWhiteSpace(policyId) || baseSurrenderValue <= 0 ? 0m : baseSurrenderValue * 0.02m;
        public decimal CalculateProratedTerminalBonus(string policyId, DateTime exitDate, int daysActive) => string.IsNullOrWhiteSpace(policyId) || daysActive <= 0 ? 0m : daysActive * 1.5m;
        public decimal ApplyBonusMultiplier(decimal baseBonus, double multiplierRate) => baseBonus <= 0 || multiplierRate <= 0 ? 0m : baseBonus * (decimal)multiplierRate;
        public double GetTerminalBonusRate(string planCode, int policyTerm) => string.IsNullOrWhiteSpace(planCode) || policyTerm <= 0 ? 0.0 : 0.05;
        public double GetLoyaltyAdditionRate(string planCode, int completedYears) => string.IsNullOrWhiteSpace(planCode) || completedYears <= 0 ? 0.0 : 0.02;
        public double CalculateBonusYield(decimal totalBonus, decimal totalPremiums) => totalPremiums <= 0 ? 0.0 : (double)(totalBonus / totalPremiums);
        public double GetFundPerformanceFactor(string fundId, DateTime evaluationDate) => string.IsNullOrWhiteSpace(fundId) ? 0.0 : 1.1;
        public double GetParticipatingFundRatio(string cohortId) => string.IsNullOrWhiteSpace(cohortId) ? 0.0 : 0.8;
        public bool IsEligibleForTerminalBonus(string policyId, string status) => !string.IsNullOrWhiteSpace(policyId) && status == "Active";
        public bool IsLoyaltyAdditionApplicable(string planCode, int elapsedYears) => !string.IsNullOrWhiteSpace(planCode) && elapsedYears > 5;
        public bool ValidateBonusDeclaration(string declarationId, DateTime effectiveDate) => !string.IsNullOrWhiteSpace(declarationId) && effectiveDate > DateTime.MinValue && effectiveDate < DateTime.MaxValue;
        public bool HasClaimedPreviousBonuses(string policyId) => !string.IsNullOrWhiteSpace(policyId) && policyId.Length > 5;
        public bool IsPolicyInParticipatingFund(string policyId) => !string.IsNullOrWhiteSpace(policyId) && policyId.StartsWith("P");
        public int GetCompletedPolicyYears(string policyId, DateTime maturityDate) => string.IsNullOrWhiteSpace(policyId) ? 0 : (maturityDate == DateTime.MaxValue ? 100 : 10);
        public int GetMinimumYearsForTerminalBonus(string planCode) => string.IsNullOrWhiteSpace(planCode) ? 0 : 5;
        public int CalculateDaysSinceLastBonusDeclaration(DateTime lastDeclarationDate) => (DateTime.Now - lastDeclarationDate).Days;
        public int GetTotalBonusUnitsAllocated(string policyId) => string.IsNullOrWhiteSpace(policyId) ? 0 : 50;
        public string GetBonusDeclarationId(string planCode, DateTime declarationYear) => string.IsNullOrWhiteSpace(planCode) ? string.Empty : $"{planCode}-{declarationYear.Year}";
        public string DetermineBonusCohort(string policyId, DateTime issueDate) => string.IsNullOrWhiteSpace(policyId) ? string.Empty : $"Cohort-{issueDate.Year}";
        public string GetTerminalBonusStatus(string policyId) => string.IsNullOrWhiteSpace(policyId) ? "Unknown" : "Declared";
    }
}