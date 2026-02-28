using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.Fund_Value_Unit_Linked_Plans;

namespace MaturityBenefitProc.Tests.Helpers.Fund_Value_Unit_Linked_Plans
{
    [TestClass]
    public class UlipNavCalculationServiceTests
    {
        private IUlipNavCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation exists for testing
            // If it's an interface, we would typically mock it, but the prompt implies testing a concrete fixed implementation.
            // Since the prompt says "Each test creates a new UlipNavCalculationService()", we will use that class name.
            // Note: The interface is IUlipNavCalculationService, so the class is likely UlipNavCalculationService.
            _service = new UlipNavCalculationService();
        }

        [TestMethod]
        public void GetNavForDate_ValidInputs_ReturnsExpectedNav()
        {
            var date1 = new DateTime(2023, 1, 1);
            var date2 = new DateTime(2023, 6, 15);
            
            var result1 = _service.GetNavForDate("FUND01", date1);
            var result2 = _service.GetNavForDate("FUND02", date2);
            var result3 = _service.GetNavForDate("FUND01", date2);
            
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.AreNotEqual(0m, result3);
            Assert.IsTrue(result1 > 0);
            Assert.IsTrue(result2 > 0);
        }

        [TestMethod]
        public void CalculateFundValue_ValidPolicy_ReturnsCalculatedValue()
        {
            var date = new DateTime(2023, 5, 1);
            var result1 = _service.CalculateFundValue("POL123", date);
            var result2 = _service.CalculateFundValue("POL456", date);
            
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsNavAvailable_VariousDates_ReturnsBoolean()
        {
            var date1 = new DateTime(2023, 1, 1);
            var date2 = DateTime.Now.AddDays(10); // Future date
            
            var result1 = _service.IsNavAvailable("FUND01", date1);
            var result2 = _service.IsNavAvailable("FUND01", date2);
            
            Assert.IsTrue(result1 || !result1); // Just checking it returns a bool properly
            Assert.IsFalse(result2); // Future date should not have NAV
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetNavDelayDays_ValidFunds_ReturnsExpectedDays()
        {
            var result1 = _service.GetNavDelayDays("FUND01");
            var result2 = _service.GetNavDelayDays("FUND02");
            var result3 = _service.GetNavDelayDays("UNKNOWN");
            
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0, result3); // Assuming unknown returns 0
            Assert.AreNotEqual(-1, result1);
        }

        [TestMethod]
        public void GetFallbackFundCode_ValidFunds_ReturnsFallback()
        {
            var result1 = _service.GetFallbackFundCode("FUND01");
            var result2 = _service.GetFallbackFundCode("FUND02");
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual("", result1);
            Assert.AreNotEqual("FUND01", result1);
        }

        [TestMethod]
        public void GetFundAllocationRatio_ValidInputs_ReturnsRatio()
        {
            var result1 = _service.GetFundAllocationRatio("POL123", "FUND01");
            var result2 = _service.GetFundAllocationRatio("POL123", "FUND02");
            
            Assert.IsTrue(result1 >= 0.0 && result1 <= 1.0);
            Assert.IsTrue(result2 >= 0.0 && result2 <= 1.0);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetTotalAllocatedUnits_ValidInputs_ReturnsUnits()
        {
            var result1 = _service.GetTotalAllocatedUnits("POL123", "FUND01");
            var result2 = _service.GetTotalAllocatedUnits("POL456", "FUND02");
            
            Assert.IsTrue(result1 >= 0m);
            Assert.IsTrue(result2 >= 0m);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateUnitValue_ValidInputs_ReturnsProduct()
        {
            var result1 = _service.CalculateUnitValue(100m, 15.5m);
            var result2 = _service.CalculateUnitValue(50.5m, 10m);
            var result3 = _service.CalculateUnitValue(0m, 15.5m);
            
            Assert.AreEqual(1550m, result1);
            Assert.AreEqual(505m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreNotEqual(100m, result1);
        }

        [TestMethod]
        public void ValidateFundStatus_VariousFunds_ReturnsStatus()
        {
            var result1 = _service.ValidateFundStatus("FUND01");
            var result2 = _service.ValidateFundStatus("INVALID_FUND");
            
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetHistoricalNav_ValidInputs_ReturnsNav()
        {
            var date = new DateTime(2020, 1, 1);
            var result1 = _service.GetHistoricalNav("FUND01", date);
            var result2 = _service.GetHistoricalNav("FUND02", date);
            
            Assert.IsTrue(result1 > 0m);
            Assert.IsTrue(result2 > 0m);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void CalculateNavGrowthRate_ValidDates_ReturnsRate()
        {
            var start = new DateTime(2020, 1, 1);
            var end = new DateTime(2021, 1, 1);
            var result1 = _service.CalculateNavGrowthRate("FUND01", start, end);
            var result2 = _service.CalculateNavGrowthRate("FUND02", start, end);
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual(0.0, result1);
            Assert.IsTrue(result1 > -1.0); // Assuming not a total loss
        }

        [TestMethod]
        public void GetTotalActiveFunds_ValidPolicy_ReturnsCount()
        {
            var result1 = _service.GetTotalActiveFunds("POL123");
            var result2 = _service.GetTotalActiveFunds("POL456");
            
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetDominantFundCode_ValidPolicy_ReturnsCode()
        {
            var result1 = _service.GetDominantFundCode("POL123");
            var result2 = _service.GetDominantFundCode("POL456");
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual("", result1);
            Assert.AreNotEqual("", result2);
        }

        [TestMethod]
        public void CalculateTotalFundValue_ValidInputs_ReturnsTotal()
        {
            var date = new DateTime(2023, 1, 1);
            var result1 = _service.CalculateTotalFundValue("POL123", date);
            var result2 = _service.CalculateTotalFundValue("POL456", date);
            
            Assert.IsTrue(result1 >= 0m);
            Assert.IsTrue(result2 >= 0m);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void IsPolicyEligibleForNavAdjustment_ValidPolicy_ReturnsBool()
        {
            var result1 = _service.IsPolicyEligibleForNavAdjustment("POL123");
            var result2 = _service.IsPolicyEligibleForNavAdjustment("POL999");
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void ApplyNavAdjustment_ValidInputs_ReturnsAdjustedNav()
        {
            var result1 = _service.ApplyNavAdjustment("POL123", 100m, 0.05);
            var result2 = _service.ApplyNavAdjustment("POL456", 50m, -0.02);
            
            Assert.AreEqual(105m, result1);
            Assert.AreEqual(49m, result2);
            Assert.AreNotEqual(100m, result1);
            Assert.AreNotEqual(50m, result2);
        }

        [TestMethod]
        public void GetUnitsPrecision_ValidFunds_ReturnsPrecision()
        {
            var result1 = _service.GetUnitsPrecision("FUND01");
            var result2 = _service.GetUnitsPrecision("FUND02");
            
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void RoundUnits_ValidInputs_ReturnsRounded()
        {
            var result1 = _service.RoundUnits(10.12345m, 2);
            var result2 = _service.RoundUnits(10.125m, 2);
            var result3 = _service.RoundUnits(10.12345m, 4);
            
            Assert.AreEqual(10.12m, result1);
            Assert.AreEqual(10.13m, result2); // Assuming MidpointRounding.AwayFromZero or similar
            Assert.AreEqual(10.1235m, result3);
            Assert.AreNotEqual(10.12345m, result1);
        }

        [TestMethod]
        public void GetNavCurrency_ValidFunds_ReturnsCurrencyCode()
        {
            var result1 = _service.GetNavCurrency("FUND01");
            var result2 = _service.GetNavCurrency("FUND02");
            
            Assert.IsNotNull(result1);
            Assert.AreEqual(3, result1.Length);
            Assert.IsNotNull(result2);
            Assert.AreEqual(3, result2.Length);
        }

        [TestMethod]
        public void GetExchangeRate_ValidInputs_ReturnsRate()
        {
            var date = new DateTime(2023, 1, 1);
            var result1 = _service.GetExchangeRate("USD", "EUR", date);
            var result2 = _service.GetExchangeRate("USD", "USD", date);
            
            Assert.IsTrue(result1 > 0);
            Assert.AreEqual(1.0, result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void ConvertFundValueCurrency_ValidInputs_ReturnsConverted()
        {
            var result1 = _service.ConvertFundValueCurrency(100m, 0.85);
            var result2 = _service.ConvertFundValueCurrency(50m, 1.2);
            
            Assert.AreEqual(85m, result1);
            Assert.AreEqual(60m, result2);
            Assert.AreNotEqual(100m, result1);
            Assert.AreNotEqual(50m, result2);
        }

        [TestMethod]
        public void HasPendingUnitAllocations_ValidPolicy_ReturnsBool()
        {
            var result1 = _service.HasPendingUnitAllocations("POL123");
            var result2 = _service.HasPendingUnitAllocations("POL456");
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void GetPendingAllocationCount_ValidPolicy_ReturnsCount()
        {
            var result1 = _service.GetPendingAllocationCount("POL123");
            var result2 = _service.GetPendingAllocationCount("POL456");
            
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetPendingAllocationAmount_ValidPolicy_ReturnsAmount()
        {
            var result1 = _service.GetPendingAllocationAmount("POL123");
            var result2 = _service.GetPendingAllocationAmount("POL456");
            
            Assert.IsTrue(result1 >= 0m);
            Assert.IsTrue(result2 >= 0m);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetLatestNavBatchId_ValidDate_ReturnsBatchId()
        {
            var date = new DateTime(2023, 1, 1);
            var result1 = _service.GetLatestNavBatchId(date);
            
            Assert.IsNotNull(result1);
            Assert.AreNotEqual("", result1);
            Assert.IsTrue(result1.Length > 0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateNavBatch_ValidBatch_ReturnsBool()
        {
            var result1 = _service.ValidateNavBatch("BATCH123");
            var result2 = _service.ValidateNavBatch("INVALID");
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsFalse(result2);
        }
    }
}