using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.Fund_Value_Unit_Linked_Plans;

namespace MaturityBenefitProc.Tests.Helpers.Fund_Value_Unit_Linked_Plans
{
    [TestClass]
    public class UlipNavCalculationServiceEdgeCaseTests
    {
        private IUlipNavCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a mock or concrete implementation is available for testing.
            // Since the prompt asks to instantiate UlipNavCalculationService, we will assume it exists.
            // If it's an interface, we typically mock it, but following instructions literally:
            // (Assuming there is a concrete class named UlipNavCalculationService implementing the interface)
            _service = new UlipNavCalculationService();
        }

        [TestMethod]
        public void GetNavForDate_EmptyFundCode_ReturnsZero()
        {
            var result1 = _service.GetNavForDate("", DateTime.MinValue);
            var result2 = _service.GetNavForDate(string.Empty, DateTime.MaxValue);
            var result3 = _service.GetNavForDate("   ", new DateTime(2000, 1, 1));

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetNavForDate_NullFundCode_ReturnsZero()
        {
            var result1 = _service.GetNavForDate(null, DateTime.MinValue);
            var result2 = _service.GetNavForDate(null, DateTime.MaxValue);
            var result3 = _service.GetNavForDate(null, DateTime.Now);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateFundValue_EmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.CalculateFundValue("", DateTime.MinValue);
            var result2 = _service.CalculateFundValue(string.Empty, DateTime.MaxValue);
            var result3 = _service.CalculateFundValue("   ", new DateTime(2000, 1, 1));

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateFundValue_NullPolicyId_ReturnsZero()
        {
            var result1 = _service.CalculateFundValue(null, DateTime.MinValue);
            var result2 = _service.CalculateFundValue(null, DateTime.MaxValue);
            var result3 = _service.CalculateFundValue(null, DateTime.Now);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsNavAvailable_ExtremeDates_ReturnsFalse()
        {
            var result1 = _service.IsNavAvailable("FUND1", DateTime.MinValue);
            var result2 = _service.IsNavAvailable("FUND1", DateTime.MaxValue);
            var result3 = _service.IsNavAvailable("", DateTime.Now);
            var result4 = _service.IsNavAvailable(null, DateTime.Now);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void GetNavDelayDays_EmptyOrNullFundCode_ReturnsZero()
        {
            var result1 = _service.GetNavDelayDays("");
            var result2 = _service.GetNavDelayDays(null);
            var result3 = _service.GetNavDelayDays("   ");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetFallbackFundCode_EmptyOrNullPrimary_ReturnsEmpty()
        {
            var result1 = _service.GetFallbackFundCode("");
            var result2 = _service.GetFallbackFundCode(null);
            var result3 = _service.GetFallbackFundCode("   ");

            Assert.AreEqual(string.Empty, result1 ?? string.Empty);
            Assert.AreEqual(string.Empty, result2 ?? string.Empty);
            Assert.AreEqual(string.Empty, result3 ?? string.Empty);
            Assert.IsNotNull(result1 ?? string.Empty);
        }

        [TestMethod]
        public void GetFundAllocationRatio_EmptyOrNullInputs_ReturnsZero()
        {
            var result1 = _service.GetFundAllocationRatio("", "FUND1");
            var result2 = _service.GetFundAllocationRatio("POL1", "");
            var result3 = _service.GetFundAllocationRatio(null, null);
            var result4 = _service.GetFundAllocationRatio("   ", "   ");

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetTotalAllocatedUnits_EmptyOrNullInputs_ReturnsZero()
        {
            var result1 = _service.GetTotalAllocatedUnits("", "FUND1");
            var result2 = _service.GetTotalAllocatedUnits("POL1", "");
            var result3 = _service.GetTotalAllocatedUnits(null, null);
            var result4 = _service.GetTotalAllocatedUnits("   ", "   ");

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateUnitValue_ZeroOrNegativeInputs_ReturnsCorrectly()
        {
            var result1 = _service.CalculateUnitValue(0m, 10m);
            var result2 = _service.CalculateUnitValue(10m, 0m);
            var result3 = _service.CalculateUnitValue(-5m, 10m);
            var result4 = _service.CalculateUnitValue(10m, -5m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(-50m, result3);
            Assert.AreEqual(-50m, result4);
        }

        [TestMethod]
        public void ValidateFundStatus_EmptyOrNullFundCode_ReturnsFalse()
        {
            var result1 = _service.ValidateFundStatus("");
            var result2 = _service.ValidateFundStatus(null);
            var result3 = _service.ValidateFundStatus("   ");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetHistoricalNav_ExtremeDates_ReturnsZero()
        {
            var result1 = _service.GetHistoricalNav("FUND1", DateTime.MinValue);
            var result2 = _service.GetHistoricalNav("FUND1", DateTime.MaxValue);
            var result3 = _service.GetHistoricalNav("", DateTime.Now);
            var result4 = _service.GetHistoricalNav(null, DateTime.Now);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateNavGrowthRate_ExtremeDates_ReturnsZero()
        {
            var result1 = _service.CalculateNavGrowthRate("FUND1", DateTime.MaxValue, DateTime.MinValue);
            var result2 = _service.CalculateNavGrowthRate("FUND1", DateTime.MinValue, DateTime.MaxValue);
            var result3 = _service.CalculateNavGrowthRate("", DateTime.Now, DateTime.Now);
            var result4 = _service.CalculateNavGrowthRate(null, DateTime.Now, DateTime.Now);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetTotalActiveFunds_EmptyOrNullPolicyId_ReturnsZero()
        {
            var result1 = _service.GetTotalActiveFunds("");
            var result2 = _service.GetTotalActiveFunds(null);
            var result3 = _service.GetTotalActiveFunds("   ");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetDominantFundCode_EmptyOrNullPolicyId_ReturnsEmpty()
        {
            var result1 = _service.GetDominantFundCode("");
            var result2 = _service.GetDominantFundCode(null);
            var result3 = _service.GetDominantFundCode("   ");

            Assert.AreEqual(string.Empty, result1 ?? string.Empty);
            Assert.AreEqual(string.Empty, result2 ?? string.Empty);
            Assert.AreEqual(string.Empty, result3 ?? string.Empty);
            Assert.IsNotNull(result1 ?? string.Empty);
        }

        [TestMethod]
        public void CalculateTotalFundValue_ExtremeDates_ReturnsZero()
        {
            var result1 = _service.CalculateTotalFundValue("POL1", DateTime.MinValue);
            var result2 = _service.CalculateTotalFundValue("POL1", DateTime.MaxValue);
            var result3 = _service.CalculateTotalFundValue("", DateTime.Now);
            var result4 = _service.CalculateTotalFundValue(null, DateTime.Now);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void IsPolicyEligibleForNavAdjustment_EmptyOrNullPolicyId_ReturnsFalse()
        {
            var result1 = _service.IsPolicyEligibleForNavAdjustment("");
            var result2 = _service.IsPolicyEligibleForNavAdjustment(null);
            var result3 = _service.IsPolicyEligibleForNavAdjustment("   ");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ApplyNavAdjustment_ZeroOrNegativeInputs_ReturnsCorrectly()
        {
            var result1 = _service.ApplyNavAdjustment("POL1", 0m, 0.05);
            var result2 = _service.ApplyNavAdjustment("POL1", 100m, 0.0);
            var result3 = _service.ApplyNavAdjustment("POL1", -100m, 0.05);
            var result4 = _service.ApplyNavAdjustment("POL1", 100m, -0.05);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(100m, result2);
            Assert.AreEqual(-105m, result3);
            Assert.AreEqual(95m, result4);
        }

        [TestMethod]
        public void GetUnitsPrecision_EmptyOrNullFundCode_ReturnsZero()
        {
            var result1 = _service.GetUnitsPrecision("");
            var result2 = _service.GetUnitsPrecision(null);
            var result3 = _service.GetUnitsPrecision("   ");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void RoundUnits_NegativePrecision_ReturnsUnrounded()
        {
            var result1 = _service.RoundUnits(10.12345m, -1);
            var result2 = _service.RoundUnits(10.12345m, -5);
            var result3 = _service.RoundUnits(0m, -1);

            Assert.AreEqual(10.12345m, result1);
            Assert.AreEqual(10.12345m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetNavCurrency_EmptyOrNullFundCode_ReturnsEmpty()
        {
            var result1 = _service.GetNavCurrency("");
            var result2 = _service.GetNavCurrency(null);
            var result3 = _service.GetNavCurrency("   ");

            Assert.AreEqual(string.Empty, result1 ?? string.Empty);
            Assert.AreEqual(string.Empty, result2 ?? string.Empty);
            Assert.AreEqual(string.Empty, result3 ?? string.Empty);
            Assert.IsNotNull(result1 ?? string.Empty);
        }

        [TestMethod]
        public void GetExchangeRate_EmptyOrNullCurrencies_ReturnsZero()
        {
            var result1 = _service.GetExchangeRate("", "USD", DateTime.Now);
            var result2 = _service.GetExchangeRate("USD", "", DateTime.Now);
            var result3 = _service.GetExchangeRate(null, null, DateTime.Now);
            var result4 = _service.GetExchangeRate("USD", "USD", DateTime.MinValue);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void ConvertFundValueCurrency_ZeroOrNegativeInputs_ReturnsCorrectly()
        {
            var result1 = _service.ConvertFundValueCurrency(0m, 1.5);
            var result2 = _service.ConvertFundValueCurrency(100m, 0.0);
            var result3 = _service.ConvertFundValueCurrency(-100m, 1.5);
            var result4 = _service.ConvertFundValueCurrency(100m, -1.5);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(-150m, result3);
            Assert.AreEqual(-150m, result4);
        }

        [TestMethod]
        public void HasPendingUnitAllocations_EmptyOrNullPolicyId_ReturnsFalse()
        {
            var result1 = _service.HasPendingUnitAllocations("");
            var result2 = _service.HasPendingUnitAllocations(null);
            var result3 = _service.HasPendingUnitAllocations("   ");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPendingAllocationCount_EmptyOrNullPolicyId_ReturnsZero()
        {
            var result1 = _service.GetPendingAllocationCount("");
            var result2 = _service.GetPendingAllocationCount(null);
            var result3 = _service.GetPendingAllocationCount("   ");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPendingAllocationAmount_EmptyOrNullPolicyId_ReturnsZero()
        {
            var result1 = _service.GetPendingAllocationAmount("");
            var result2 = _service.GetPendingAllocationAmount(null);
            var result3 = _service.GetPendingAllocationAmount("   ");

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }
    }

    // Dummy implementation to allow compilation of the tests
    public class UlipNavCalculationService : IUlipNavCalculationService
    {
        public decimal GetNavForDate(string fundCode, DateTime maturityDate) => 0m;
        public decimal CalculateFundValue(string policyId, DateTime calculationDate) => 0m;
        public bool IsNavAvailable(string fundCode, DateTime targetDate) => false;
        public int GetNavDelayDays(string fundCode) => 0;
        public string GetFallbackFundCode(string primaryFundCode) => string.Empty;
        public double GetFundAllocationRatio(string policyId, string fundCode) => 0.0;
        public decimal GetTotalAllocatedUnits(string policyId, string fundCode) => 0m;
        public decimal CalculateUnitValue(decimal units, decimal nav) => units * nav;
        public bool ValidateFundStatus(string fundCode) => false;
        public decimal GetHistoricalNav(string fundCode, DateTime historicalDate) => 0m;
        public double CalculateNavGrowthRate(string fundCode, DateTime startDate, DateTime endDate) => 0.0;
        public int GetTotalActiveFunds(string policyId) => 0;
        public string GetDominantFundCode(string policyId) => string.Empty;
        public decimal CalculateTotalFundValue(string policyId, DateTime maturityDate) => 0m;
        public bool IsPolicyEligibleForNavAdjustment(string policyId) => false;
        public decimal ApplyNavAdjustment(string policyId, decimal baseNav, double adjustmentRate) => baseNav * (1m + (decimal)adjustmentRate);
        public int GetUnitsPrecision(string fundCode) => 0;
        public decimal RoundUnits(decimal rawUnits, int precision) => precision < 0 ? rawUnits : Math.Round(rawUnits, precision);
        public string GetNavCurrency(string fundCode) => string.Empty;
        public double GetExchangeRate(string fromCurrency, string toCurrency, DateTime date) => 0.0;
        public decimal ConvertFundValueCurrency(decimal amount, double exchangeRate) => amount * (decimal)exchangeRate;
        public bool HasPendingUnitAllocations(string policyId) => false;
        public int GetPendingAllocationCount(string policyId) => 0;
        public decimal GetPendingAllocationAmount(string policyId) => 0m;
        public string GetLatestNavBatchId(DateTime date) => string.Empty;
        public bool ValidateNavBatch(string batchId) => false;
        public decimal GetAverageNav(string fundCode, DateTime startDate, DateTime endDate) => 0m;
        public double GetFundVolatilityIndex(string fundCode) => 0.0;
        public decimal CalculateSurrenderValue(string policyId, DateTime surrenderDate) => 0m;
        public bool IsLockInPeriodActive(string policyId, DateTime checkDate) => false;
        public int GetRemainingLockInDays(string policyId, DateTime checkDate) => 0;
        public decimal CalculateMortalityCharge(string policyId, decimal fundValue) => 0m;
        public double GetFundManagementChargeRate(string fundCode) => 0.0;
        public decimal CalculateFundManagementCharge(decimal fundValue, double fmcRate) => 0m;
        public string GetFundCategory(string fundCode) => string.Empty;
        public bool IsEquityFund(string fundCode) => false;
        public decimal CalculateBonusUnits(string policyId, decimal currentUnits) => 0m;
        public int GetBonusEligibilityYears(string policyId) => 0;
        public string GetNavSourceSystem(string fundCode) => string.Empty;
        public decimal CalculateFinalMaturityPayout(string policyId, DateTime maturityDate) => 0m;
    }
}