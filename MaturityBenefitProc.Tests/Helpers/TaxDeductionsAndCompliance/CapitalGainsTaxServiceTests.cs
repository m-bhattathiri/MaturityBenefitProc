using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class CapitalGainsTaxServiceTests
    {
        private ICapitalGainsTaxService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation named CapitalGainsTaxService exists
            _service = new CapitalGainsTaxService();
        }

        [TestMethod]
        public void CalculateShortTermCapitalGains_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateShortTermCapitalGains("POL123", 150000m, 100000m);
            var result2 = _service.CalculateShortTermCapitalGains("POL124", 200000m, 250000m);
            var result3 = _service.CalculateShortTermCapitalGains("POL125", 100000m, 100000m);

            Assert.AreEqual(50000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateShortTermCapitalGains_NegativeInputs_ReturnsZero()
        {
            var result1 = _service.CalculateShortTermCapitalGains("POL123", -150000m, 100000m);
            var result2 = _service.CalculateShortTermCapitalGains("POL124", 200000m, -250000m);
            var result3 = _service.CalculateShortTermCapitalGains("POL125", -100000m, -100000m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateLongTermCapitalGains_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateLongTermCapitalGains("POL123", 150000m, 100000m);
            var result2 = _service.CalculateLongTermCapitalGains("POL124", 200000m, 250000m);
            var result3 = _service.CalculateLongTermCapitalGains("POL125", 100000m, 100000m);

            Assert.AreEqual(50000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateLongTermCapitalGains_NegativeInputs_ReturnsZero()
        {
            var result1 = _service.CalculateLongTermCapitalGains("POL123", -150000m, 100000m);
            var result2 = _service.CalculateLongTermCapitalGains("POL124", 200000m, -250000m);
            var result3 = _service.CalculateLongTermCapitalGains("POL125", -100000m, -100000m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.IsExemptUnderSection10_10D("POL123", 10000m, 150000m);
            var result2 = _service.IsExemptUnderSection10_10D("POL124", 20000m, 150000m);
            var result3 = _service.IsExemptUnderSection10_10D("POL125", 15000m, 150000m);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_NegativeInputs_ReturnsFalse()
        {
            var result1 = _service.IsExemptUnderSection10_10D("POL123", -10000m, 150000m);
            var result2 = _service.IsExemptUnderSection10_10D("POL124", 20000m, -150000m);
            var result3 = _service.IsExemptUnderSection10_10D("POL125", -15000m, -150000m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetApplicableTaxRate_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetApplicableTaxRate("CUST1", "RES", new DateTime(2023, 1, 1));
            var result2 = _service.GetApplicableTaxRate("CUST2", "NRI", new DateTime(2023, 1, 1));
            var result3 = _service.GetApplicableTaxRate("CUST3", "UNKNOWN", new DateTime(2023, 1, 1));

            Assert.AreEqual(0.10, result1);
            Assert.AreEqual(0.125, result2);
            Assert.AreEqual(0.10, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateGrandfatheredValue_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateGrandfatheredValue("POL123", new DateTime(2018, 1, 31));
            var result2 = _service.CalculateGrandfatheredValue("POL124", new DateTime(2020, 1, 31));
            var result3 = _service.CalculateGrandfatheredValue("POL125", new DateTime(2017, 1, 31));

            Assert.AreEqual(100000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(100000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateHoldingPeriodInDays_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateHoldingPeriodInDays("POL123", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            var result2 = _service.CalculateHoldingPeriodInDays("POL124", new DateTime(2020, 1, 1), new DateTime(2020, 1, 1));
            var result3 = _service.CalculateHoldingPeriodInDays("POL125", new DateTime(2021, 1, 1), new DateTime(2020, 1, 1));

            Assert.AreEqual(366, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsHighPremiumUlip_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.IsHighPremiumUlip("POL123", 200000m);
            var result2 = _service.IsHighPremiumUlip("POL124", 300000m);
            var result3 = _service.IsHighPremiumUlip("POL125", 250000m);

            Assert.IsFalse(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateTaxableAmount_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateTaxableAmount("POL123", 150000m, 50000m);
            var result2 = _service.CalculateTaxableAmount("POL124", 100000m, 150000m);
            var result3 = _service.CalculateTaxableAmount("POL125", 100000m, 100000m);

            Assert.AreEqual(100000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateSurcharge_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateSurcharge(10000m, 6000000m);
            var result2 = _service.CalculateSurcharge(10000m, 4000000m);
            var result3 = _service.CalculateSurcharge(10000m, 15000000m);

            Assert.AreEqual(1000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(1500m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateHealthAndEducationCess_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateHealthAndEducationCess(10000m, 1000m);
            var result2 = _service.CalculateHealthAndEducationCess(5000m, 0m);
            var result3 = _service.CalculateHealthAndEducationCess(0m, 0m);

            Assert.AreEqual(440m, result1);
            Assert.AreEqual(200m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTaxCategoryCode_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetTaxCategoryCode("POL123", new DateTime(2021, 2, 1));
            var result2 = _service.GetTaxCategoryCode("POL124", new DateTime(2020, 1, 1));
            var result3 = _service.GetTaxCategoryCode("POL125", new DateTime(2021, 1, 31));

            Assert.AreEqual("POST_BUDGET_2021", result1);
            Assert.AreEqual("PRE_BUDGET_2021", result2);
            Assert.AreEqual("PRE_BUDGET_2021", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTotalPremiumsPaid_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetTotalPremiumsPaid("POL123", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            var result2 = _service.GetTotalPremiumsPaid("POL124", new DateTime(2021, 1, 1), new DateTime(2020, 1, 1));
            var result3 = _service.GetTotalPremiumsPaid("POL125", new DateTime(2020, 1, 1), new DateTime(2020, 1, 1));

            Assert.AreEqual(50000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidatePanStatus_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.ValidatePanStatus("ABCDE1234F", "CUST1");
            var result2 = _service.ValidatePanStatus("", "CUST2");
            var result3 = _service.ValidatePanStatus("INVALID", "CUST3");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTdsRate_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetTdsRate("ABCDE1234F", false);
            var result2 = _service.GetTdsRate("", false);
            var result3 = _service.GetTdsRate("ABCDE1234F", true);

            Assert.AreEqual(0.05, result1);
            Assert.AreEqual(0.20, result2);
            Assert.AreEqual(0.125, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateTdsAmount_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateTdsAmount(100000m, 0.05);
            var result2 = _service.CalculateTdsAmount(100000m, 0.20);
            var result3 = _service.CalculateTdsAmount(0m, 0.05);

            Assert.AreEqual(5000m, result1);
            Assert.AreEqual(20000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetFinancialYear_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetFinancialYear(new DateTime(2023, 3, 31));
            var result2 = _service.GetFinancialYear(new DateTime(2023, 4, 1));
            var result3 = _service.GetFinancialYear(new DateTime(2024, 1, 1));

            Assert.AreEqual(2022, result1);
            Assert.AreEqual(2023, result2);
            Assert.AreEqual(2023, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GenerateTaxComputationCertificateId_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GenerateTaxComputationCertificateId("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GenerateTaxComputationCertificateId("POL124", new DateTime(2023, 1, 1));
            var result3 = _service.GenerateTaxComputationCertificateId("POL125", new DateTime(2023, 1, 1));

            Assert.AreEqual("TCC-POL123-20230101", result1);
            Assert.AreEqual("TCC-POL124-20230101", result2);
            Assert.AreEqual("TCC-POL125-20230101", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateIndexationBenefit_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateIndexationBenefit(100000m, 2010, 2020);
            var result2 = _service.CalculateIndexationBenefit(100000m, 2020, 2010);
            var result3 = _service.CalculateIndexationBenefit(100000m, 2020, 2020);

            Assert.AreEqual(150000m, result1);
            Assert.AreEqual(100000m, result2);
            Assert.AreEqual(100000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void RequiresCapitalGainsReporting_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.RequiresCapitalGainsReporting("POL123", 300000m);
            var result2 = _service.RequiresCapitalGainsReporting("POL124", 200000m);
            var result3 = _service.RequiresCapitalGainsReporting("POL125", 250000m);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateNetPayableAfterTaxes_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateNetPayableAfterTaxes(150000m, 10000m);
            var result2 = _service.CalculateNetPayableAfterTaxes(100000m, 150000m);
            var result3 = _service.CalculateNetPayableAfterTaxes(100000m, 0m);

            Assert.AreEqual(140000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(100000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetNumberOfFundSwitches_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetNumberOfFundSwitches("POL123", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            var result2 = _service.GetNumberOfFundSwitches("POL124", new DateTime(2021, 1, 1), new DateTime(2020, 1, 1));
            var result3 = _service.GetNumberOfFundSwitches("POL125", new DateTime(2020, 1, 1), new DateTime(2020, 1, 1));

            Assert.AreEqual(2, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateCapitalLossCarriedForward_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateCapitalLossCarriedForward("CUST1", 2022);
            var result2 = _service.CalculateCapitalLossCarriedForward("CUST2", 2023);
            var result3 = _service.CalculateCapitalLossCarriedForward("CUST3", 2021);

            Assert.AreEqual(50000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(25000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsEquityOrientedFund_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.IsEquityOrientedFund("FUND1", 70.0);
            var result2 = _service.IsEquityOrientedFund("FUND2", 60.0);
            var result3 = _service.IsEquityOrientedFund("FUND3", 65.0);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetSttRate_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetSttRate("FUND1", new DateTime(2023, 1, 1));
            var result2 = _service.GetSttRate("FUND2", new DateTime(2023, 1, 1));
            var result3 = _service.GetSttRate("FUND3", new DateTime(2023, 1, 1));

            Assert.AreEqual(0.001, result1);
            Assert.AreEqual(0.001, result2);
            Assert.AreEqual(0.001, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateSecuritiesTransactionTax_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateSecuritiesTransactionTax(100000m, 0.001);
            var result2 = _service.CalculateSecuritiesTransactionTax(200000m, 0.001);
            var result3 = _service.CalculateSecuritiesTransactionTax(0m, 0.001);

            Assert.AreEqual(100m, result1);
            Assert.AreEqual(200m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTaxResidencyStatus_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetTaxResidencyStatus("CUST1", 2022);
            var result2 = _service.GetTaxResidencyStatus("CUST2", 2023);
            var result3 = _service.GetTaxResidencyStatus("CUST3", 2021);

            Assert.AreEqual("RES", result1);
            Assert.AreEqual("NRI", result2);
            Assert.AreEqual("RES", result3);
            Assert.IsNotNull(result1);
        }
    }
}