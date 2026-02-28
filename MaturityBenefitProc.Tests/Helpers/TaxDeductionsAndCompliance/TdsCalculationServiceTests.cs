using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class TdsCalculationServiceTests
    {
        private ITdsCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming TdsCalculationService is the concrete implementation
            // In a real scenario, this would be instantiated directly or via a factory/mock
            // For the purpose of this test file structure, we assume a mock or concrete class is available.
            // Since the prompt specifies testing the FIXED implementation, we'll assume TdsCalculationService exists.
            _service = new TdsCalculationService();
        }

        [TestMethod]
        public void CalculateBaseTds_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateBaseTds("POL123", 100000m);
            var result2 = _service.CalculateBaseTds("POL124", 50000m);
            var result3 = _service.CalculateBaseTds("POL125", 0m);
            var result4 = _service.CalculateBaseTds("POL126", 200000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(5000m, result1); // Assuming 5% rate for testing
            Assert.AreEqual(2500m, result2);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void GetApplicableTdsRate_ValidPan_ReturnsCorrectRate()
        {
            var date = new DateTime(2023, 5, 1);
            var result1 = _service.GetApplicableTdsRate("ABCDE1234F", date);
            var result2 = _service.GetApplicableTdsRate("INVALID", date);
            var result3 = _service.GetApplicableTdsRate("", date);
            var result4 = _service.GetApplicableTdsRate(null, date);

            Assert.IsTrue(result1 > 0);
            Assert.AreEqual(0.05, result1); // Assuming 5% for valid PAN
            Assert.AreEqual(0.20, result2); // Assuming 20% for invalid PAN
            Assert.AreEqual(0.20, result3);
            Assert.AreEqual(0.20, result4);
        }

        [TestMethod]
        public void IsTdsApplicable_VariousAmounts_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsTdsApplicable(150000m, "194DA");
            var result2 = _service.IsTdsApplicable(50000m, "194DA");
            var result3 = _service.IsTdsApplicable(99999m, "194DA");
            var result4 = _service.IsTdsApplicable(100000m, "194DA");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsTrue(result4); // Assuming threshold is 100,000
        }

        [TestMethod]
        public void GetFinancialYear_VariousDates_ReturnsCorrectYear()
        {
            var result1 = _service.GetFinancialYear(new DateTime(2023, 5, 1));
            var result2 = _service.GetFinancialYear(new DateTime(2023, 3, 31));
            var result3 = _service.GetFinancialYear(new DateTime(2024, 1, 1));
            var result4 = _service.GetFinancialYear(new DateTime(2022, 12, 31));

            Assert.AreEqual(2023, result1);
            Assert.AreEqual(2022, result2);
            Assert.AreEqual(2023, result3);
            Assert.AreEqual(2022, result4);
        }

        [TestMethod]
        public void GetTaxCategoryCode_ValidCustomers_ReturnsCorrectCode()
        {
            var result1 = _service.GetTaxCategoryCode("CUST001");
            var result2 = _service.GetTaxCategoryCode("CUST002");
            var result3 = _service.GetTaxCategoryCode("NRI001");
            var result4 = _service.GetTaxCategoryCode("");

            Assert.IsNotNull(result1);
            Assert.AreEqual("IND", result1); // Assuming IND for individual
            Assert.AreEqual("IND", result2);
            Assert.AreEqual("NRI", result3);
            Assert.AreEqual("UNKNOWN", result4);
        }

        [TestMethod]
        public void ComputeSurcharge_ValidInputs_ReturnsCorrectSurcharge()
        {
            var result1 = _service.ComputeSurcharge(10000m, 0.10);
            var result2 = _service.ComputeSurcharge(50000m, 0.15);
            var result3 = _service.ComputeSurcharge(0m, 0.10);
            var result4 = _service.ComputeSurcharge(10000m, 0.0);

            Assert.AreEqual(1000m, result1);
            Assert.AreEqual(7500m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void ComputeEducationCess_ValidTaxAmount_ReturnsCorrectCess()
        {
            var result1 = _service.ComputeEducationCess(10000m);
            var result2 = _service.ComputeEducationCess(50000m);
            var result3 = _service.ComputeEducationCess(0m);
            var result4 = _service.ComputeEducationCess(100m);

            Assert.AreEqual(400m, result1); // Assuming 4% cess
            Assert.AreEqual(2000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(4m, result4);
        }

        [TestMethod]
        public void GetTotalTaxableAmount_ValidInputs_ReturnsCorrectAmount()
        {
            var result1 = _service.GetTotalTaxableAmount("POL123", 500000m);
            var result2 = _service.GetTotalTaxableAmount("POL124", 100000m);
            var result3 = _service.GetTotalTaxableAmount("POL125", 0m);
            var result4 = _service.GetTotalTaxableAmount("POL126", -5000m);

            Assert.IsTrue(result1 >= 0);
            Assert.AreEqual(400000m, result1); // Assuming 100k exempt
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateNetPayout_ValidInputs_ReturnsCorrectNet()
        {
            var result1 = _service.CalculateNetPayout(100000m, 5000m);
            var result2 = _service.CalculateNetPayout(50000m, 0m);
            var result3 = _service.CalculateNetPayout(200000m, 10000m);
            var result4 = _service.CalculateNetPayout(0m, 0m);

            Assert.AreEqual(95000m, result1);
            Assert.AreEqual(50000m, result2);
            Assert.AreEqual(190000m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetExemptAmount_ValidInputs_ReturnsCorrectExemptAmount()
        {
            var result1 = _service.GetExemptAmount("POL123", 100000m);
            var result2 = _service.GetExemptAmount("POL124", 50000m);
            var result3 = _service.GetExemptAmount("POL125", 0m);
            var result4 = _service.GetExemptAmount("POL126", 200000m);

            Assert.IsTrue(result1 >= 0);
            Assert.AreEqual(100000m, result1); // Assuming fully exempt up to premiums paid
            Assert.AreEqual(50000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(200000m, result4);
        }

        [TestMethod]
        public void CalculateTdsOnSurrender_ValidInputs_ReturnsCorrectTds()
        {
            var result1 = _service.CalculateTdsOnSurrender("POL123", 150000m, 24);
            var result2 = _service.CalculateTdsOnSurrender("POL124", 50000m, 60);
            var result3 = _service.CalculateTdsOnSurrender("POL125", 200000m, 12);
            var result4 = _service.CalculateTdsOnSurrender("POL126", 0m, 36);

            Assert.IsTrue(result1 >= 0);
            Assert.AreEqual(7500m, result1); // Assuming 5% on surrender value
            Assert.AreEqual(0m, result2); // Assuming no TDS for > 3 years or < threshold
            Assert.AreEqual(10000m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateTdsOnMaturity_ValidInputs_ReturnsCorrectTds()
        {
            var result1 = _service.CalculateTdsOnMaturity("POL123", 200000m);
            var result2 = _service.CalculateTdsOnMaturity("POL124", 50000m);
            var result3 = _service.CalculateTdsOnMaturity("POL125", 0m);
            var result4 = _service.CalculateTdsOnMaturity("POL126", 150000m);

            Assert.IsTrue(result1 >= 0);
            Assert.AreEqual(10000m, result1); // Assuming 5%
            Assert.AreEqual(0m, result2); // Below threshold
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(7500m, result4);
        }

        [TestMethod]
        public void GetThresholdLimit_ValidInputs_ReturnsCorrectLimit()
        {
            var result1 = _service.GetThresholdLimit("194DA", 2023);
            var result2 = _service.GetThresholdLimit("194DA", 2022);
            var result3 = _service.GetThresholdLimit("INVALID", 2023);
            var result4 = _service.GetThresholdLimit("194DA", 2010);

            Assert.IsTrue(result1 > 0);
            Assert.AreEqual(100000m, result1);
            Assert.AreEqual(100000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(100000m, result4);
        }

        [TestMethod]
        public void ComputePenaltyAmount_ValidInputs_ReturnsCorrectPenalty()
        {
            var result1 = _service.ComputePenaltyAmount("ABCDE1234F", 10);
            var result2 = _service.ComputePenaltyAmount("ABCDE1234F", 0);
            var result3 = _service.ComputePenaltyAmount("ABCDE1234F", 30);
            var result4 = _service.ComputePenaltyAmount("ABCDE1234F", -5);

            Assert.IsTrue(result1 >= 0);
            Assert.AreEqual(2000m, result1); // Assuming 200 per day
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(6000m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetEffectiveTaxRate_ValidInputs_ReturnsCorrectRate()
        {
            var result1 = _service.GetEffectiveTaxRate(0.05, 0.10, 0.04);
            var result2 = _service.GetEffectiveTaxRate(0.20, 0.0, 0.04);
            var result3 = _service.GetEffectiveTaxRate(0.0, 0.0, 0.0);
            var result4 = _service.GetEffectiveTaxRate(0.30, 0.15, 0.04);

            Assert.IsTrue(result1 > 0);
            Assert.AreEqual(0.0572, Math.Round(result1, 4)); // 0.05 * 1.10 * 1.04
            Assert.AreEqual(0.208, Math.Round(result2, 4));
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.3588, Math.Round(result4, 4));
        }

        [TestMethod]
        public void GetSurchargeRate_ValidInputs_ReturnsCorrectRate()
        {
            var result1 = _service.GetSurchargeRate(6000000m, "IND");
            var result2 = _service.GetSurchargeRate(4000000m, "IND");
            var result3 = _service.GetSurchargeRate(15000000m, "IND");
            var result4 = _service.GetSurchargeRate(0m, "IND");

            Assert.IsTrue(result1 >= 0);
            Assert.AreEqual(0.10, result1); // > 50L
            Assert.AreEqual(0.0, result2); // < 50L
            Assert.AreEqual(0.15, result3); // > 1Cr
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void ComputeCessPercentage_ValidInputs_ReturnsCorrectPercentage()
        {
            var result1 = _service.ComputeCessPercentage(2023);
            var result2 = _service.ComputeCessPercentage(2017);
            var result3 = _service.ComputeCessPercentage(2018);
            var result4 = _service.ComputeCessPercentage(2024);

            Assert.IsTrue(result1 > 0);
            Assert.AreEqual(0.04, result1);
            Assert.AreEqual(0.03, result2); // Assuming 3% before 2018
            Assert.AreEqual(0.04, result3);
            Assert.AreEqual(0.04, result4);
        }

        [TestMethod]
        public void GetMarginalReliefRatio_ValidInputs_ReturnsCorrectRatio()
        {
            var result1 = _service.GetMarginalReliefRatio(5100000m, 1500000m);
            var result2 = _service.GetMarginalReliefRatio(4000000m, 1000000m);
            var result3 = _service.GetMarginalReliefRatio(10100000m, 3000000m);
            var result4 = _service.GetMarginalReliefRatio(0m, 0m);

            Assert.IsTrue(result1 >= 0);
            Assert.AreEqual(0.05, result1); // Mocked values
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.10, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetDtaaBenefitRate_ValidInputs_ReturnsCorrectRate()
        {
            var result1 = _service.GetDtaaBenefitRate("US");
            var result2 = _service.GetDtaaBenefitRate("UK");
            var result3 = _service.GetDtaaBenefitRate("AE");
            var result4 = _service.GetDtaaBenefitRate("XX");

            Assert.IsTrue(result1 >= 0);
            Assert.AreEqual(0.15, result1); // Mocked values
            Assert.AreEqual(0.10, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void IsPanValid_VariousPans_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsPanValid("ABCDE1234F");
            var result2 = _service.IsPanValid("INVALIDPAN");
            var result3 = _service.IsPanValid("12345ABCDE");
            var result4 = _service.IsPanValid("");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsNriCustomer_VariousCustomers_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsNriCustomer("NRI001");
            var result2 = _service.IsNriCustomer("CUST001");
            var result3 = _service.IsNriCustomer("NRI002");
            var result4 = _service.IsNriCustomer("");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void HasForm15GOrH_VariousInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.HasForm15GOrH("CUST001", 2023);
            var result2 = _service.HasForm15GOrH("CUST002", 2023);
            var result3 = _service.HasForm15GOrH("CUST001", 2022);
            var result4 = _service.HasForm15GOrH("", 2023);

            Assert.IsTrue(result1); // Mocked to true for CUST001 in 2023
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsAmountAboveThreshold_VariousAmounts_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsAmountAboveThreshold(150000m, "194DA");
            var result2 = _service.IsAmountAboveThreshold(50000m, "194DA");
            var result3 = _service.IsAmountAboveThreshold(100000m, "194DA");
            var result4 = _service.IsAmountAboveThreshold(0m, "194DA");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3); // Assuming strictly greater than
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsAadhaarLinked_VariousPans_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsAadhaarLinked("ABCDE1234F");
            var result2 = _service.IsAadhaarLinked("FGHIJ5678K");
            var result3 = _service.IsAadhaarLinked("UNLINKED12");
            var result4 = _service.IsAadhaarLinked("");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_VariousInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsExemptUnderSection10_10D("POL123", 500000m, 40000m);
            var result2 = _service.IsExemptUnderSection10_10D("POL124", 500000m, 60000m);
            var result3 = _service.IsExemptUnderSection10_10D("POL125", 100000m, 5000m);
            var result4 = _service.IsExemptUnderSection10_10D("POL126", 100000m, 15000m);

            Assert.IsTrue(result1); // Premium < 10% of SA
            Assert.IsFalse(result2); // Premium > 10% of SA
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void RequiresHigherTdsRate_VariousPans_ReturnsExpectedBoolean()
        {
            var result1 = _service.RequiresHigherTdsRate("INVALIDPAN");
            var result2 = _service.RequiresHigherTdsRate("ABCDE1234F");
            var result3 = _service.RequiresHigherTdsRate("UNLINKED12");
            var result4 = _service.RequiresHigherTdsRate("");

            Assert.IsTrue(result1); // Invalid PAN requires higher rate
            Assert.IsFalse(result2); // Valid and linked
            Assert.IsTrue(result3); // Unlinked requires higher rate
            Assert.IsTrue(result4);
        }

        [TestMethod]
        public void CalculateDaysDelayed_VariousDates_ReturnsCorrectDays()
        {
            var expected = new DateTime(2023, 5, 1);
            var result1 = _service.CalculateDaysDelayed(expected, new DateTime(2023, 5, 11));
            var result2 = _service.CalculateDaysDelayed(expected, new DateTime(2023, 5, 1));
            var result3 = _service.CalculateDaysDelayed(expected, new DateTime(2023, 4, 30));
            var result4 = _service.CalculateDaysDelayed(expected, new DateTime(2023, 6, 1));

            Assert.IsTrue(result1 >= 0);
            Assert.AreEqual(10, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3); // No delay if earlier
            Assert.AreEqual(31, result4);
        }
    }
}