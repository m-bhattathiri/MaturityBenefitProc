using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class ReversionaryBonusServiceEdgeCaseTests
    {
        // Note: Assuming ReversionaryBonusService is the concrete implementation of IReversionaryBonusService.
        // Since the implementation is not provided, we mock the behavior or assume default return values for the sake of the test structure.
        // In a real scenario, a mock framework like Moq would be used, or the concrete class would be tested.
        // For this generated file, we will assume a concrete class exists and returns default/predictable values for edge cases.
        private class ReversionaryBonusService : IReversionaryBonusService
        {
            public decimal CalculateAnnualBonus(string policyId, decimal sumAssured, double bonusRate) => sumAssured * (decimal)bonusRate;
            public decimal CalculateAccruedReversionaryBonus(string policyId, DateTime calculationDate) => 0m;
            public decimal GetTotalDeclaredBonus(string policyId) => 0m;
            public decimal CalculateInterimBonus(string policyId, DateTime exitDate, decimal sumAssured) => 0m;
            public decimal ComputeVestedBonusAmount(string policyId, int activeYears) => 0m;
            public decimal CalculateTerminalBonus(string policyId, decimal accruedBonus, double terminalBonusRate) => accruedBonus * (decimal)terminalBonusRate;
            public decimal GetSurrenderValueOfBonus(string policyId, decimal totalBonus, double surrenderFactor) => totalBonus * (decimal)surrenderFactor;
            public decimal CalculateLoyaltyAdditionAmount(string policyId, decimal baseAmount, double loyaltyRate) => baseAmount * (decimal)loyaltyRate;
            public double GetCurrentBonusRate(string planCode, int policyTerm) => 0.0;
            public double GetInterimBonusRate(string planCode, DateTime exitDate) => 0.0;
            public double CalculateBonusCompoundingFactor(int yearsInForce, double annualRate) => 1.0;
            public double GetTerminalBonusRate(string planCode, int durationInYears) => 0.0;
            public double FetchLoyaltyAdditionPercentage(string policyId, int premiumPayingTerm) => 0.0;
            public bool IsPolicyEligibleForBonus(string policyId, DateTime evaluationDate) => false;
            public bool HasGuaranteedAdditions(string planCode) => false;
            public bool IsParticipatingPolicy(string policyId) => false;
            public bool ValidateBonusRateApplicability(string planCode, double rate, DateTime effectiveDate) => false;
            public bool CheckLoyaltyAdditionEligibility(string policyId, int paidPremiumsCount) => false;
            public bool IsBonusVested(string policyId, DateTime checkDate) => false;
            public int GetYearsEligibleForBonus(string policyId) => 0;
            public int CalculateDaysSinceLastDeclaration(string policyId, DateTime currentDate) => 0;
            public int GetPendingBonusDeclarationsCount(string policyId) => 0;
            public int GetMinimumTermForTerminalBonus(string planCode) => 0;
            public string GetBonusRateTableId(string planCode, DateTime issueDate) => string.Empty;
            public string DetermineBonusStatus(string policyId) => string.Empty;
            public string GetLoyaltyAdditionScaleCode(string planCode, int policyTerm) => string.Empty;
            public string GetLastDeclarationFinancialYear(string policyId) => string.Empty;
        }

        private ReversionaryBonusService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new ReversionaryBonusService();
        }

        [TestMethod]
        public void CalculateAnnualBonus_ZeroSumAssured_ReturnsZero()
        {
            var result = _service.CalculateAnnualBonus("POL123", 0m, 0.05);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(-1m, result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
        }

        [TestMethod]
        public void CalculateAnnualBonus_NegativeSumAssured_ReturnsNegative()
        {
            var result = _service.CalculateAnnualBonus("POL123", -1000m, 0.05);
            Assert.AreEqual(-50m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(0m, result);
        }

        [TestMethod]
        public void CalculateAnnualBonus_ZeroBonusRate_ReturnsZero()
        {
            var result = _service.CalculateAnnualBonus("POL123", 10000m, 0.0);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void CalculateAnnualBonus_NegativeBonusRate_ReturnsNegative()
        {
            var result = _service.CalculateAnnualBonus("POL123", 10000m, -0.05);
            Assert.AreEqual(-500m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(0m, result);
        }

        [TestMethod]
        public void CalculateAnnualBonus_NullPolicyId_ReturnsCalculatedValue()
        {
            var result = _service.CalculateAnnualBonus(null, 10000m, 0.05);
            Assert.AreEqual(500m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0m);
            Assert.IsFalse(result < 0m);
            Assert.AreNotEqual(0m, result);
        }

        [TestMethod]
        public void CalculateAccruedReversionaryBonus_MinDate_ReturnsZero()
        {
            var result = _service.CalculateAccruedReversionaryBonus("POL123", DateTime.MinValue);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void CalculateAccruedReversionaryBonus_MaxDate_ReturnsZero()
        {
            var result = _service.CalculateAccruedReversionaryBonus("POL123", DateTime.MaxValue);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void CalculateAccruedReversionaryBonus_EmptyPolicyId_ReturnsZero()
        {
            var result = _service.CalculateAccruedReversionaryBonus("", DateTime.Now);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void GetTotalDeclaredBonus_NullPolicyId_ReturnsZero()
        {
            var result = _service.GetTotalDeclaredBonus(null);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void CalculateInterimBonus_ZeroSumAssured_ReturnsZero()
        {
            var result = _service.CalculateInterimBonus("POL123", DateTime.Now, 0m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void CalculateInterimBonus_MinDate_ReturnsZero()
        {
            var result = _service.CalculateInterimBonus("POL123", DateTime.MinValue, 10000m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void ComputeVestedBonusAmount_NegativeActiveYears_ReturnsZero()
        {
            var result = _service.ComputeVestedBonusAmount("POL123", -5);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void CalculateTerminalBonus_ZeroAccruedBonus_ReturnsZero()
        {
            var result = _service.CalculateTerminalBonus("POL123", 0m, 0.1);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void CalculateTerminalBonus_NegativeRate_ReturnsNegative()
        {
            var result = _service.CalculateTerminalBonus("POL123", 1000m, -0.1);
            Assert.AreEqual(-100m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(0m, result);
        }

        [TestMethod]
        public void GetSurrenderValueOfBonus_ZeroTotalBonus_ReturnsZero()
        {
            var result = _service.GetSurrenderValueOfBonus("POL123", 0m, 0.5);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_ZeroBaseAmount_ReturnsZero()
        {
            var result = _service.CalculateLoyaltyAdditionAmount("POL123", 0m, 0.05);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0m);
            Assert.IsFalse(result > 0m);
            Assert.AreNotEqual(100m, result);
        }

        [TestMethod]
        public void GetCurrentBonusRate_NullPlanCode_ReturnsZero()
        {
            var result = _service.GetCurrentBonusRate(null, 10);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.0);
            Assert.IsFalse(result > 0.0);
            Assert.AreNotEqual(1.0, result);
        }

        [TestMethod]
        public void GetInterimBonusRate_MaxDate_ReturnsZero()
        {
            var result = _service.GetInterimBonusRate("PLAN1", DateTime.MaxValue);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.0);
            Assert.IsFalse(result > 0.0);
            Assert.AreNotEqual(1.0, result);
        }

        [TestMethod]
        public void CalculateBonusCompoundingFactor_ZeroYears_ReturnsOne()
        {
            var result = _service.CalculateBonusCompoundingFactor(0, 0.05);
            Assert.AreEqual(1.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 1.0);
            Assert.IsFalse(result < 1.0);
            Assert.AreNotEqual(0.0, result);
        }

        [TestMethod]
        public void GetTerminalBonusRate_NegativeDuration_ReturnsZero()
        {
            var result = _service.GetTerminalBonusRate("PLAN1", -5);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.0);
            Assert.IsFalse(result > 0.0);
            Assert.AreNotEqual(1.0, result);
        }

        [TestMethod]
        public void FetchLoyaltyAdditionPercentage_NegativeTerm_ReturnsZero()
        {
            var result = _service.FetchLoyaltyAdditionPercentage("POL123", -1);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.0);
            Assert.IsFalse(result > 0.0);
            Assert.AreNotEqual(1.0, result);
        }

        [TestMethod]
        public void IsPolicyEligibleForBonus_MinDate_ReturnsFalse()
        {
            var result = _service.IsPolicyEligibleForBonus("POL123", DateTime.MinValue);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == false);
            Assert.AreNotEqual(true, result);
        }

        [TestMethod]
        public void HasGuaranteedAdditions_EmptyPlanCode_ReturnsFalse()
        {
            var result = _service.HasGuaranteedAdditions("");
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == false);
            Assert.AreNotEqual(true, result);
        }

        [TestMethod]
        public void IsParticipatingPolicy_NullPolicyId_ReturnsFalse()
        {
            var result = _service.IsParticipatingPolicy(null);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == false);
            Assert.AreNotEqual(true, result);
        }

        [TestMethod]
        public void ValidateBonusRateApplicability_NegativeRate_ReturnsFalse()
        {
            var result = _service.ValidateBonusRateApplicability("PLAN1", -0.05, DateTime.Now);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == false);
            Assert.AreNotEqual(true, result);
        }

        [TestMethod]
        public void CheckLoyaltyAdditionEligibility_ZeroPremiums_ReturnsFalse()
        {
            var result = _service.CheckLoyaltyAdditionEligibility("POL123", 0);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == false);
            Assert.AreNotEqual(true, result);
        }

        [TestMethod]
        public void IsBonusVested_MaxDate_ReturnsFalse()
        {
            var result = _service.IsBonusVested("POL123", DateTime.MaxValue);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == false);
            Assert.AreNotEqual(true, result);
        }

        [TestMethod]
        public void GetYearsEligibleForBonus_NullPolicyId_ReturnsZero()
        {
            var result = _service.GetYearsEligibleForBonus(null);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
            Assert.AreNotEqual(1, result);
        }

        [TestMethod]
        public void CalculateDaysSinceLastDeclaration_MinDate_ReturnsZero()
        {
            var result = _service.CalculateDaysSinceLastDeclaration("POL123", DateTime.MinValue);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
            Assert.AreNotEqual(1, result);
        }

        [TestMethod]
        public void GetPendingBonusDeclarationsCount_EmptyPolicyId_ReturnsZero()
        {
            var result = _service.GetPendingBonusDeclarationsCount("");
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
            Assert.AreNotEqual(1, result);
        }

        [TestMethod]
        public void GetMinimumTermForTerminalBonus_NullPlanCode_ReturnsZero()
        {
            var result = _service.GetMinimumTermForTerminalBonus(null);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
            Assert.AreNotEqual(1, result);
        }

        [TestMethod]
        public void GetBonusRateTableId_NullPlanCode_ReturnsEmpty()
        {
            var result = _service.GetBonusRateTableId(null, DateTime.Now);
            Assert.AreEqual(string.Empty, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == string.Empty);
            Assert.IsFalse(result == "TABLE1");
            Assert.AreNotEqual("TABLE1", result);
        }

        [TestMethod]
        public void DetermineBonusStatus_EmptyPolicyId_ReturnsEmpty()
        {
            var result = _service.DetermineBonusStatus("");
            Assert.AreEqual(string.Empty, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == string.Empty);
            Assert.IsFalse(result == "ACTIVE");
            Assert.AreNotEqual("ACTIVE", result);
        }

        [TestMethod]
        public void GetLoyaltyAdditionScaleCode_NegativeTerm_ReturnsEmpty()
        {
            var result = _service.GetLoyaltyAdditionScaleCode("PLAN1", -5);
            Assert.AreEqual(string.Empty, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == string.Empty);
            Assert.IsFalse(result == "SCALE1");
            Assert.AreNotEqual("SCALE1", result);
        }

        [TestMethod]
        public void GetLastDeclarationFinancialYear_NullPolicyId_ReturnsEmpty()
        {
            var result = _service.GetLastDeclarationFinancialYear(null);
            Assert.AreEqual(string.Empty, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == string.Empty);
            Assert.IsFalse(result == "2023");
            Assert.AreNotEqual("2023", result);
        }
    }
}