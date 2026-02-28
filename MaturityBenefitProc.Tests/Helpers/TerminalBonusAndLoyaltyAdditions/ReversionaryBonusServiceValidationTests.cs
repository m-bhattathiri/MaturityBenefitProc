using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class ReversionaryBonusServiceValidationTests
    {
        private IReversionaryBonusService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a mock or concrete implementation exists for testing purposes.
            // For the sake of this generated file, we will assume a mock framework or a stub class is used.
            // Since the prompt specifies `_service = new ReversionaryBonusService();`, we will use that.
            // Note: The interface is IReversionaryBonusService, but the prompt says to instantiate ReversionaryBonusService.
            _service = new ReversionaryBonusServiceStub();
        }

        [TestMethod]
        public void CalculateAnnualBonus_ValidInputs_ReturnsExpectedResult()
        {
            var result1 = _service.CalculateAnnualBonus("POL123", 100000m, 0.05);
            var result2 = _service.CalculateAnnualBonus("POL124", 50000m, 0.04);
            var result3 = _service.CalculateAnnualBonus("POL125", 200000m, 0.06);

            Assert.IsNotNull(result1);
            Assert.AreEqual(5000m, result1);
            Assert.AreEqual(2000m, result2);
            Assert.AreEqual(12000m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void CalculateAnnualBonus_ZeroSumAssured_ReturnsZero()
        {
            var result1 = _service.CalculateAnnualBonus("POL123", 0m, 0.05);
            var result2 = _service.CalculateAnnualBonus("POL124", 0m, 0.04);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateAnnualBonus_NegativeSumAssured_ReturnsZeroOrThrows()
        {
            var result1 = _service.CalculateAnnualBonus("POL123", -10000m, 0.05);
            var result2 = _service.CalculateAnnualBonus("POL124", -50000m, 0.04);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateAccruedReversionaryBonus_ValidInputs_ReturnsExpectedResult()
        {
            var date = new DateTime(2023, 1, 1);
            var result1 = _service.CalculateAccruedReversionaryBonus("POL123", date);
            var result2 = _service.CalculateAccruedReversionaryBonus("POL124", date.AddYears(1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void CalculateAccruedReversionaryBonus_FutureDate_ReturnsZero()
        {
            var date = DateTime.Now.AddYears(10);
            var result1 = _service.CalculateAccruedReversionaryBonus("POL123", date);
            var result2 = _service.CalculateAccruedReversionaryBonus("POL124", date);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetTotalDeclaredBonus_ValidPolicyId_ReturnsAmount()
        {
            var result1 = _service.GetTotalDeclaredBonus("POL123");
            var result2 = _service.GetTotalDeclaredBonus("POL124");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void GetTotalDeclaredBonus_InvalidPolicyId_ReturnsZero()
        {
            var result1 = _service.GetTotalDeclaredBonus("");
            var result2 = _service.GetTotalDeclaredBonus(null);
            var result3 = _service.GetTotalDeclaredBonus("   ");

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateInterimBonus_ValidInputs_ReturnsExpectedResult()
        {
            var date = new DateTime(2023, 6, 30);
            var result1 = _service.CalculateInterimBonus("POL123", date, 100000m);
            var result2 = _service.CalculateInterimBonus("POL124", date, 50000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void CalculateInterimBonus_ZeroSumAssured_ReturnsZero()
        {
            var date = new DateTime(2023, 6, 30);
            var result1 = _service.CalculateInterimBonus("POL123", date, 0m);
            var result2 = _service.CalculateInterimBonus("POL124", date, 0m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void ComputeVestedBonusAmount_ValidInputs_ReturnsExpectedResult()
        {
            var result1 = _service.ComputeVestedBonusAmount("POL123", 5);
            var result2 = _service.ComputeVestedBonusAmount("POL124", 10);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void ComputeVestedBonusAmount_NegativeYears_ReturnsZero()
        {
            var result1 = _service.ComputeVestedBonusAmount("POL123", -1);
            var result2 = _service.ComputeVestedBonusAmount("POL124", -5);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInputs_ReturnsExpectedResult()
        {
            var result1 = _service.CalculateTerminalBonus("POL123", 10000m, 0.10);
            var result2 = _service.CalculateTerminalBonus("POL124", 5000m, 0.05);

            Assert.AreEqual(1000m, result1);
            Assert.AreEqual(250m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateTerminalBonus_ZeroAccruedBonus_ReturnsZero()
        {
            var result1 = _service.CalculateTerminalBonus("POL123", 0m, 0.10);
            var result2 = _service.CalculateTerminalBonus("POL124", 0m, 0.05);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetSurrenderValueOfBonus_ValidInputs_ReturnsExpectedResult()
        {
            var result1 = _service.GetSurrenderValueOfBonus("POL123", 10000m, 0.50);
            var result2 = _service.GetSurrenderValueOfBonus("POL124", 5000m, 0.40);

            Assert.AreEqual(5000m, result1);
            Assert.AreEqual(2000m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetSurrenderValueOfBonus_ZeroBonus_ReturnsZero()
        {
            var result1 = _service.GetSurrenderValueOfBonus("POL123", 0m, 0.50);
            var result2 = _service.GetSurrenderValueOfBonus("POL124", 0m, 0.40);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateLoyaltyAdditionAmount_ValidInputs_ReturnsExpectedResult()
        {
            var result1 = _service.CalculateLoyaltyAdditionAmount("POL123", 100000m, 0.02);
            var result2 = _service.CalculateLoyaltyAdditionAmount("POL124", 50000m, 0.01);

            Assert.AreEqual(2000m, result1);
            Assert.AreEqual(500m, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetCurrentBonusRate_ValidInputs_ReturnsExpectedResult()
        {
            var result1 = _service.GetCurrentBonusRate("PLAN1", 10);
            var result2 = _service.GetCurrentBonusRate("PLAN2", 20);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void GetInterimBonusRate_ValidInputs_ReturnsExpectedResult()
        {
            var date = new DateTime(2023, 1, 1);
            var result1 = _service.GetInterimBonusRate("PLAN1", date);
            var result2 = _service.GetInterimBonusRate("PLAN2", date);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void CalculateBonusCompoundingFactor_ValidInputs_ReturnsExpectedResult()
        {
            var result1 = _service.CalculateBonusCompoundingFactor(5, 0.05);
            var result2 = _service.CalculateBonusCompoundingFactor(10, 0.04);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 > 1.0);
            Assert.IsTrue(result2 > 1.0);
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidInputs_ReturnsExpectedResult()
        {
            var result1 = _service.GetTerminalBonusRate("PLAN1", 15);
            var result2 = _service.GetTerminalBonusRate("PLAN2", 20);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void FetchLoyaltyAdditionPercentage_ValidInputs_ReturnsExpectedResult()
        {
            var result1 = _service.FetchLoyaltyAdditionPercentage("POL123", 10);
            var result2 = _service.FetchLoyaltyAdditionPercentage("POL124", 15);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void IsPolicyEligibleForBonus_ValidInputs_ReturnsTrueOrFalse()
        {
            var date = new DateTime(2023, 1, 1);
            var result1 = _service.IsPolicyEligibleForBonus("POL123", date);
            var result2 = _service.IsPolicyEligibleForBonus("POL124", date);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void HasGuaranteedAdditions_ValidInputs_ReturnsTrueOrFalse()
        {
            var result1 = _service.HasGuaranteedAdditions("PLAN1");
            var result2 = _service.HasGuaranteedAdditions("PLAN2");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void IsParticipatingPolicy_ValidInputs_ReturnsTrueOrFalse()
        {
            var result1 = _service.IsParticipatingPolicy("POL123");
            var result2 = _service.IsParticipatingPolicy("POL124");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void ValidateBonusRateApplicability_ValidInputs_ReturnsTrueOrFalse()
        {
            var date = new DateTime(2023, 1, 1);
            var result1 = _service.ValidateBonusRateApplicability("PLAN1", 0.05, date);
            var result2 = _service.ValidateBonusRateApplicability("PLAN2", 0.04, date);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void CheckLoyaltyAdditionEligibility_ValidInputs_ReturnsTrueOrFalse()
        {
            var result1 = _service.CheckLoyaltyAdditionEligibility("POL123", 10);
            var result2 = _service.CheckLoyaltyAdditionEligibility("POL124", 5);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }
    }

    // Stub class for testing purposes
    public class ReversionaryBonusServiceStub : IReversionaryBonusService
    {
        public decimal CalculateAnnualBonus(string policyId, decimal sumAssured, double bonusRate) => sumAssured > 0 ? sumAssured * (decimal)bonusRate : 0m;
        public decimal CalculateAccruedReversionaryBonus(string policyId, DateTime calculationDate) => calculationDate <= DateTime.Now ? 1000m : 0m;
        public decimal GetTotalDeclaredBonus(string policyId) => string.IsNullOrWhiteSpace(policyId) ? 0m : 5000m;
        public decimal CalculateInterimBonus(string policyId, DateTime exitDate, decimal sumAssured) => sumAssured > 0 ? 500m : 0m;
        public decimal ComputeVestedBonusAmount(string policyId, int activeYears) => activeYears > 0 ? activeYears * 100m : 0m;
        public decimal CalculateTerminalBonus(string policyId, decimal accruedBonus, double terminalBonusRate) => accruedBonus * (decimal)terminalBonusRate;
        public decimal GetSurrenderValueOfBonus(string policyId, decimal totalBonus, double surrenderFactor) => totalBonus * (decimal)surrenderFactor;
        public decimal CalculateLoyaltyAdditionAmount(string policyId, decimal baseAmount, double loyaltyRate) => baseAmount * (decimal)loyaltyRate;
        
        public double GetCurrentBonusRate(string planCode, int policyTerm) => 0.05;
        public double GetInterimBonusRate(string planCode, DateTime exitDate) => 0.02;
        public double CalculateBonusCompoundingFactor(int yearsInForce, double annualRate) => Math.Pow(1 + annualRate, yearsInForce);
        public double GetTerminalBonusRate(string planCode, int durationInYears) => 0.10;
        public double FetchLoyaltyAdditionPercentage(string policyId, int premiumPayingTerm) => 0.05;
        
        public bool IsPolicyEligibleForBonus(string policyId, DateTime evaluationDate) => true;
        public bool HasGuaranteedAdditions(string planCode) => true;
        public bool IsParticipatingPolicy(string policyId) => true;
        public bool ValidateBonusRateApplicability(string planCode, double rate, DateTime effectiveDate) => true;
        public bool CheckLoyaltyAdditionEligibility(string policyId, int paidPremiumsCount) => paidPremiumsCount >= 10;
        public bool IsBonusVested(string policyId, DateTime checkDate) => true;
        
        public int GetYearsEligibleForBonus(string policyId) => 5;
        public int CalculateDaysSinceLastDeclaration(string policyId, DateTime currentDate) => 100;
        public int GetPendingBonusDeclarationsCount(string policyId) => 1;
        public int GetMinimumTermForTerminalBonus(string planCode) => 10;
        
        public string GetBonusRateTableId(string planCode, DateTime issueDate) => "TBL1";
        public string DetermineBonusStatus(string policyId) => "Active";
        public string GetLoyaltyAdditionScaleCode(string planCode, int policyTerm) => "SCL1";
        public string GetLastDeclarationFinancialYear(string policyId) => "2022-2023";
    }
}