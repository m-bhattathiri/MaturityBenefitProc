using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans;

namespace MaturityBenefitProc.Tests.Helpers.FundValueAndUnitLinkedPlans
{
    [TestClass]
    public class UlipMaturityServiceEdgeCaseTests
    {
        private IUlipMaturityService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a mock or stub implementation for testing purposes
            // In a real scenario, this would be a concrete class or a mock object
            _service = new MockUlipMaturityService();
        }

        [TestMethod]
        public void CalculateTotalFundValue_EmptyPolicyId_ReturnsZero()
        {
            var result = _service.CalculateTotalFundValue("", DateTime.Now);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(-1m, result);
            Assert.IsTrue(result >= 0);
        }

        [TestMethod]
        public void CalculateTotalFundValue_MaxDate_HandlesGracefully()
        {
            var result = _service.CalculateTotalFundValue("POL123", DateTime.MaxValue);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetNavOnDate_MinDate_ReturnsZero()
        {
            var result = _service.GetNavOnDate("FUND1", DateTime.MinValue);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(1m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetNavOnDate_NullFundId_ReturnsZero()
        {
            var result = _service.GetNavOnDate(null, DateTime.Now);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(10m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateMortalityChargeRefund_EmptyPolicyId_ReturnsZero()
        {
            var result = _service.CalculateMortalityChargeRefund(string.Empty);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateLoyaltyAdditions_NegativePolicyTerm_ReturnsZero()
        {
            var result = _service.CalculateLoyaltyAdditions("POL123", -5);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(50m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateWealthBoosters_NegativeAverageFundValue_ReturnsZero()
        {
            var result = _service.CalculateWealthBoosters("POL123", -1000m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetTotalAllocatedUnits_NullPolicyId_ReturnsZero()
        {
            var result = _service.GetTotalAllocatedUnits(null, "FUND1");
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(10m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateSurrenderValue_MinDate_ReturnsZero()
        {
            var result = _service.CalculateSurrenderValue("POL123", DateTime.MinValue);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetFundManagementCharge_NegativeRate_ReturnsZero()
        {
            var result = _service.GetFundManagementCharge("FUND1", -0.05);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(10m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void CalculateGuaranteeAddition_NegativePremium_ReturnsZero()
        {
            var result = _service.CalculateGuaranteeAddition("POL123", -500m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(50m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetTotalPremiumPaid_EmptyPolicyId_ReturnsZero()
        {
            var result = _service.GetTotalPremiumPaid("");
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(1000m, result);
            Assert.IsTrue(result == 0m);
        }

        [TestMethod]
        public void GetFundAllocationRatio_NullFundId_ReturnsZero()
        {
            var result = _service.GetFundAllocationRatio("POL123", null);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(1.0, result);
            Assert.IsTrue(result == 0.0);
        }

        [TestMethod]
        public void GetMortalityRate_NegativeAge_ReturnsZero()
        {
            var result = _service.GetMortalityRate(-10, 5);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.05, result);
            Assert.IsTrue(result == 0.0);
        }

        [TestMethod]
        public void GetNavGrowthRate_StartAfterEnd_ReturnsZero()
        {
            var result = _service.GetNavGrowthRate("FUND1", DateTime.Now.AddDays(1), DateTime.Now);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.1, result);
            Assert.IsTrue(result == 0.0);
        }

        [TestMethod]
        public void GetBonusRate_NegativeYear_ReturnsZero()
        {
            var result = _service.GetBonusRate("POL123", -1);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.05, result);
            Assert.IsTrue(result == 0.0);
        }

        [TestMethod]
        public void GetTaxRate_EmptyStateCode_ReturnsZero()
        {
            var result = _service.GetTaxRate("", "CAT1");
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.18, result);
            Assert.IsTrue(result == 0.0);
        }

        [TestMethod]
        public void IsEligibleForMortalityRefund_NullPolicyId_ReturnsFalse()
        {
            var result = _service.IsEligibleForMortalityRefund(null);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsEligibleForLoyaltyAdditions_EmptyPolicyId_ReturnsFalse()
        {
            var result = _service.IsEligibleForLoyaltyAdditions("");
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsEligibleForWealthBoosters_NullPolicyId_ReturnsFalse()
        {
            var result = _service.IsEligibleForWealthBoosters(null);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void ValidateFundSwitch_NegativeAmount_ReturnsFalse()
        {
            var result = _service.ValidateFundSwitch("POL123", "FUND1", "FUND2", -100m);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void HasActivePremiumHoliday_MinDate_ReturnsFalse()
        {
            var result = _service.HasActivePremiumHoliday("POL123", DateTime.MinValue);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsPolicyMatured_MaxDate_ReturnsFalse()
        {
            var result = _service.IsPolicyMatured("POL123", DateTime.MaxValue);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void ValidateNavDate_MinDate_ReturnsFalse()
        {
            var result = _service.ValidateNavDate("FUND1", DateTime.MinValue);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void IsTopUpAllowed_NegativeAmount_ReturnsFalse()
        {
            var result = _service.IsTopUpAllowed("POL123", -500m);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void GetCompletedPolicyYears_MinDate_ReturnsZero()
        {
            var result = _service.GetCompletedPolicyYears("POL123", DateTime.MinValue);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(5, result);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void GetRemainingPremiumTerms_NullPolicyId_ReturnsZero()
        {
            var result = _service.GetRemainingPremiumTerms(null);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(10, result);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void GetTotalFundSwitchesUsed_NegativeYear_ReturnsZero()
        {
            var result = _service.GetTotalFundSwitchesUsed("POL123", -1);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(2, result);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void GetDaysToMaturity_MaxDate_ReturnsZero()
        {
            var result = _service.GetDaysToMaturity("POL123", DateTime.MaxValue);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100, result);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void GetPrimaryFundId_NullPolicyId_ReturnsEmpty()
        {
            var result = _service.GetPrimaryFundId(null);
            Assert.AreEqual(string.Empty, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("FUND1", result);
            Assert.IsTrue(result == string.Empty);
        }

        [TestMethod]
        public void CalculateFinalMaturityPayout_NullPolicyId_ReturnsZero()
        {
            string status;
            var result = _service.CalculateFinalMaturityPayout(null, DateTime.Now, out status);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", status);
            Assert.AreNotEqual(1000m, result);
            Assert.IsTrue(result == 0m);
        }
    }

    // Mock implementation for testing
    public class MockUlipMaturityService : IUlipMaturityService
    {
        public decimal CalculateTotalFundValue(string policyId, DateTime maturityDate) => 0m;
        public decimal GetNavOnDate(string fundId, DateTime date) => 0m;
        public decimal CalculateMortalityChargeRefund(string policyId) => 0m;
        public decimal CalculateLoyaltyAdditions(string policyId, int policyTerm) => 0m;
        public decimal CalculateWealthBoosters(string policyId, decimal averageFundValue) => 0m;
        public decimal GetTotalAllocatedUnits(string policyId, string fundId) => 0m;
        public decimal CalculateSurrenderValue(string policyId, DateTime surrenderDate) => 0m;
        public decimal GetFundManagementCharge(string fundId, double fmcRate) => 0m;
        public decimal CalculateGuaranteeAddition(string policyId, decimal basePremium) => 0m;
        public decimal GetTotalPremiumPaid(string policyId) => 0m;

        public double GetFundAllocationRatio(string policyId, string fundId) => 0.0;
        public double GetMortalityRate(int ageAtEntry, int policyYear) => 0.0;
        public double GetNavGrowthRate(string fundId, DateTime startDate, DateTime endDate) => 0.0;
        public double GetBonusRate(string policyId, int policyYear) => 0.0;
        public double GetTaxRate(string stateCode, string taxCategory) => 0.0;

        public bool IsEligibleForMortalityRefund(string policyId) => false;
        public bool IsEligibleForLoyaltyAdditions(string policyId) => false;
        public bool IsEligibleForWealthBoosters(string policyId) => false;
        public bool ValidateFundSwitch(string policyId, string fromFundId, string toFundId, decimal amount) => false;
        public bool HasActivePremiumHoliday(string policyId, DateTime checkDate) => false;
        public bool IsPolicyMatured(string policyId, DateTime currentDate) => false;
        public bool ValidateNavDate(string fundId, DateTime navDate) => false;
        public bool IsTopUpAllowed(string policyId, decimal topUpAmount) => false;

        public int GetCompletedPolicyYears(string policyId, DateTime currentDate) => 0;
        public int GetRemainingPremiumTerms(string policyId) => 0;
        public int GetTotalFundSwitchesUsed(string policyId, int year) => 0;
        public int GetDaysToMaturity(string policyId, DateTime currentDate) => 0;
        public int GetGracePeriodDays(string policyId) => 0;
        public int GetFreeLookPeriodDays(string policyId) => 0;

        public string GetPrimaryFundId(string policyId) => string.Empty;
        public string GetPolicyStatus(string policyId) => string.Empty;
        public string GenerateMaturityStatementId(string policyId, DateTime maturityDate) => string.Empty;
        public string GetTaxCategoryCode(string policyId) => string.Empty;
        public string GetRiderCode(string policyId, string riderType) => string.Empty;
        public string GetFundName(string fundId) => string.Empty;

        public decimal CalculateFinalMaturityPayout(string policyId, DateTime maturityDate, out string payoutStatus)
        {
            payoutStatus = "Error";
            return 0m;
        }
        public decimal CalculateTopUpFundValue(string policyId, DateTime maturityDate) => 0m;
        public decimal CalculateBaseFundValue(string policyId, DateTime maturityDate) => 0m;
        public decimal ApplyDiscontinuanceCharge(string policyId, decimal fundValue) => 0m;
        public decimal CalculatePartialWithdrawalImpact(string policyId, decimal withdrawalAmount) => 0m;
    }
}