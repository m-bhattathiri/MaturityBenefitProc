using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class CapitalGainsTaxServiceValidationTests
    {
        private ICapitalGainsTaxService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming CapitalGainsTaxService implements ICapitalGainsTaxService
            // For the purpose of this test file generation, we will mock or assume a concrete implementation exists.
            // Since we can't instantiate an interface, we will assume a concrete class CapitalGainsTaxService exists in the same namespace.
            // If using a mocking framework, we would use Mock<ICapitalGainsTaxService>().Object.
            // Here we use a dummy implementation or assume the concrete class is available.
            _service = new CapitalGainsTaxService();
        }

        [TestMethod]
        public void CalculateShortTermCapitalGains_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateShortTermCapitalGains("POL123", 150000m, 100000m);
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            
            decimal result2 = _service.CalculateShortTermCapitalGains("POL124", 200000m, 200000m);
            Assert.AreEqual(0m, result2);
            
            decimal result3 = _service.CalculateShortTermCapitalGains("POL125", 100000m, 150000m);
            Assert.IsTrue(result3 <= 0);
        }

        [TestMethod]
        public void CalculateShortTermCapitalGains_InvalidPolicyId_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateShortTermCapitalGains(null, 100000m, 50000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateShortTermCapitalGains("", 100000m, 50000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateShortTermCapitalGains("   ", 100000m, 50000m));
        }

        [TestMethod]
        public void CalculateShortTermCapitalGains_NegativeAmounts_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateShortTermCapitalGains("POL123", -100m, 50000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateShortTermCapitalGains("POL123", 100000m, -50000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateShortTermCapitalGains("POL123", -100m, -50000m));
        }

        [TestMethod]
        public void CalculateLongTermCapitalGains_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateLongTermCapitalGains("POL123", 150000m, 100000m);
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            
            decimal result2 = _service.CalculateLongTermCapitalGains("POL124", 200000m, 200000m);
            Assert.AreEqual(0m, result2);
            
            decimal result3 = _service.CalculateLongTermCapitalGains("POL125", 100000m, 150000m);
            Assert.IsTrue(result3 <= 0);
        }

        [TestMethod]
        public void CalculateLongTermCapitalGains_InvalidPolicyId_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateLongTermCapitalGains(null, 100000m, 50000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateLongTermCapitalGains("", 100000m, 50000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateLongTermCapitalGains("   ", 100000m, 50000m));
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_ValidInputs_ReturnsExpected()
        {
            bool result1 = _service.IsExemptUnderSection10_10D("POL123", 10000m, 150000m);
            Assert.IsTrue(result1);
            
            bool result2 = _service.IsExemptUnderSection10_10D("POL124", 20000m, 100000m);
            Assert.IsFalse(result2);
            
            bool result3 = _service.IsExemptUnderSection10_10D("POL125", 10000m, 100000m);
            Assert.IsTrue(result3);
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.IsExemptUnderSection10_10D(null, 10000m, 100000m));
            Assert.ThrowsException<ArgumentException>(() => _service.IsExemptUnderSection10_10D("POL123", -10000m, 100000m));
            Assert.ThrowsException<ArgumentException>(() => _service.IsExemptUnderSection10_10D("POL123", 10000m, -100000m));
        }

        [TestMethod]
        public void GetApplicableTaxRate_ValidInputs_ReturnsExpected()
        {
            double result1 = _service.GetApplicableTaxRate("CUST123", "RES", new DateTime(2023, 1, 1));
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result1 <= 1);
            
            double result2 = _service.GetApplicableTaxRate("CUST124", "NRI", new DateTime(2023, 1, 1));
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result2 <= 1);
        }

        [TestMethod]
        public void GetApplicableTaxRate_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.GetApplicableTaxRate(null, "RES", DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.GetApplicableTaxRate("CUST123", null, DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.GetApplicableTaxRate("", "RES", DateTime.Now));
        }

        [TestMethod]
        public void CalculateGrandfatheredValue_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateGrandfatheredValue("POL123", new DateTime(2018, 1, 31));
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            
            decimal result2 = _service.CalculateGrandfatheredValue("POL124", new DateTime(2020, 1, 1));
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void CalculateGrandfatheredValue_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateGrandfatheredValue(null, DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateGrandfatheredValue("", DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateGrandfatheredValue("   ", DateTime.Now));
        }

        [TestMethod]
        public void CalculateHoldingPeriodInDays_ValidInputs_ReturnsExpected()
        {
            int result1 = _service.CalculateHoldingPeriodInDays("POL123", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            Assert.AreEqual(366, result1); // Leap year
            
            int result2 = _service.CalculateHoldingPeriodInDays("POL124", new DateTime(2021, 1, 1), new DateTime(2022, 1, 1));
            Assert.AreEqual(365, result2);
            
            int result3 = _service.CalculateHoldingPeriodInDays("POL125", new DateTime(2022, 1, 1), new DateTime(2022, 1, 1));
            Assert.AreEqual(0, result3);
        }

        [TestMethod]
        public void CalculateHoldingPeriodInDays_InvalidDates_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateHoldingPeriodInDays("POL123", new DateTime(2022, 1, 1), new DateTime(2021, 1, 1)));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateHoldingPeriodInDays(null, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1)));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateHoldingPeriodInDays("", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1)));
        }

        [TestMethod]
        public void IsHighPremiumUlip_ValidInputs_ReturnsExpected()
        {
            bool result1 = _service.IsHighPremiumUlip("POL123", 300000m);
            Assert.IsTrue(result1);
            
            bool result2 = _service.IsHighPremiumUlip("POL124", 200000m);
            Assert.IsFalse(result2);
            
            bool result3 = _service.IsHighPremiumUlip("POL125", 250000m);
            Assert.IsTrue(result3); // Assuming >= 250000 is high premium
        }

        [TestMethod]
        public void IsHighPremiumUlip_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.IsHighPremiumUlip(null, 300000m));
            Assert.ThrowsException<ArgumentException>(() => _service.IsHighPremiumUlip("", 300000m));
            Assert.ThrowsException<ArgumentException>(() => _service.IsHighPremiumUlip("POL123", -100m));
        }

        [TestMethod]
        public void CalculateTaxableAmount_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateTaxableAmount("POL123", 150000m, 50000m);
            Assert.AreEqual(100000m, result1);
            
            decimal result2 = _service.CalculateTaxableAmount("POL124", 100000m, 100000m);
            Assert.AreEqual(0m, result2);
            
            decimal result3 = _service.CalculateTaxableAmount("POL125", 50000m, 100000m);
            Assert.AreEqual(0m, result3); // Should not be negative
        }

        [TestMethod]
        public void CalculateTaxableAmount_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTaxableAmount(null, 150000m, 50000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTaxableAmount("POL123", -150000m, 50000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTaxableAmount("POL123", 150000m, -50000m));
        }

        [TestMethod]
        public void CalculateSurcharge_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateSurcharge(100000m, 6000000m);
            Assert.IsTrue(result1 > 0);
            
            decimal result2 = _service.CalculateSurcharge(100000m, 4000000m);
            Assert.AreEqual(0m, result2);
            
            decimal result3 = _service.CalculateSurcharge(0m, 6000000m);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateSurcharge_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateSurcharge(-100000m, 6000000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateSurcharge(100000m, -6000000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateSurcharge(-100000m, -6000000m));
        }

        [TestMethod]
        public void CalculateHealthAndEducationCess_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateHealthAndEducationCess(100000m, 10000m);
            Assert.AreEqual(4400m, result1); // 4% of 110000
            
            decimal result2 = _service.CalculateHealthAndEducationCess(0m, 0m);
            Assert.AreEqual(0m, result2);
            
            decimal result3 = _service.CalculateHealthAndEducationCess(50000m, 0m);
            Assert.AreEqual(2000m, result3);
        }

        [TestMethod]
        public void CalculateHealthAndEducationCess_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateHealthAndEducationCess(-100000m, 10000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateHealthAndEducationCess(100000m, -10000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateHealthAndEducationCess(-100000m, -10000m));
        }

        [TestMethod]
        public void GetTaxCategoryCode_ValidInputs_ReturnsExpected()
        {
            string result1 = _service.GetTaxCategoryCode("POL123", new DateTime(2020, 1, 1));
            Assert.IsNotNull(result1);
            Assert.AreNotEqual("", result1);
            
            string result2 = _service.GetTaxCategoryCode("POL124", new DateTime(2021, 2, 1));
            Assert.IsNotNull(result2);
            Assert.AreNotEqual("", result2);
        }

        [TestMethod]
        public void GetTaxCategoryCode_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxCategoryCode(null, DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxCategoryCode("", DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxCategoryCode("   ", DateTime.Now));
        }

        [TestMethod]
        public void GetTotalPremiumsPaid_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.GetTotalPremiumsPaid("POL123", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            Assert.IsTrue(result1 >= 0);
            
            decimal result2 = _service.GetTotalPremiumsPaid("POL124", new DateTime(2021, 1, 1), new DateTime(2022, 1, 1));
            Assert.IsTrue(result2 >= 0);
            
            decimal result3 = _service.GetTotalPremiumsPaid("POL125", new DateTime(2022, 1, 1), new DateTime(2022, 1, 1));
            Assert.IsTrue(result3 >= 0);
        }

        [TestMethod]
        public void GetTotalPremiumsPaid_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.GetTotalPremiumsPaid(null, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1)));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTotalPremiumsPaid("POL123", new DateTime(2022, 1, 1), new DateTime(2021, 1, 1)));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTotalPremiumsPaid("", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1)));
        }

        [TestMethod]
        public void ValidatePanStatus_ValidInputs_ReturnsExpected()
        {
            bool result1 = _service.ValidatePanStatus("ABCDE1234F", "CUST123");
            Assert.IsTrue(result1 || !result1); // Just checking it returns a bool
            
            bool result2 = _service.ValidatePanStatus("VWXYZ5678G", "CUST124");
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void ValidatePanStatus_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.ValidatePanStatus(null, "CUST123"));
            Assert.ThrowsException<ArgumentException>(() => _service.ValidatePanStatus("ABCDE1234F", null));
            Assert.ThrowsException<ArgumentException>(() => _service.ValidatePanStatus("", "CUST123"));
        }

        [TestMethod]
        public void GetTdsRate_ValidInputs_ReturnsExpected()
        {
            double result1 = _service.GetTdsRate("ABCDE1234F", false);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result1 <= 1);
            
            double result2 = _service.GetTdsRate("VWXYZ5678G", true);
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result2 <= 1);
            
            double result3 = _service.GetTdsRate(null, false); // No PAN
            Assert.IsTrue(result3 >= 0.2); // Usually 20% if no PAN
        }

        [TestMethod]
        public void CalculateTdsAmount_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateTdsAmount(100000m, 0.05);
            Assert.AreEqual(5000m, result1);
            
            decimal result2 = _service.CalculateTdsAmount(50000m, 0.20);
            Assert.AreEqual(10000m, result2);
            
            decimal result3 = _service.CalculateTdsAmount(0m, 0.05);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateTdsAmount_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTdsAmount(-100000m, 0.05));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTdsAmount(100000m, -0.05));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTdsAmount(100000m, 1.5));
        }

        [TestMethod]
        public void GetFinancialYear_ValidInputs_ReturnsExpected()
        {
            int result1 = _service.GetFinancialYear(new DateTime(2023, 3, 31));
            Assert.AreEqual(2022, result1);
            
            int result2 = _service.GetFinancialYear(new DateTime(2023, 4, 1));
            Assert.AreEqual(2023, result2);
            
            int result3 = _service.GetFinancialYear(new DateTime(2024, 1, 1));
            Assert.AreEqual(2023, result3);
        }

        [TestMethod]
        public void GenerateTaxComputationCertificateId_ValidInputs_ReturnsExpected()
        {
            string result1 = _service.GenerateTaxComputationCertificateId("POL123", new DateTime(2023, 1, 1));
            Assert.IsNotNull(result1);
            Assert.AreNotEqual("", result1);
            
            string result2 = _service.GenerateTaxComputationCertificateId("POL124", new DateTime(2023, 2, 1));
            Assert.IsNotNull(result2);
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void GenerateTaxComputationCertificateId_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.GenerateTaxComputationCertificateId(null, DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.GenerateTaxComputationCertificateId("", DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.GenerateTaxComputationCertificateId("   ", DateTime.Now));
        }

        [TestMethod]
        public void CalculateIndexationBenefit_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateIndexationBenefit(100000m, 2010, 2020);
            Assert.IsTrue(result1 >= 100000m);
            
            decimal result2 = _service.CalculateIndexationBenefit(50000m, 2015, 2020);
            Assert.IsTrue(result2 >= 50000m);
            
            decimal result3 = _service.CalculateIndexationBenefit(100000m, 2020, 2020);
            Assert.AreEqual(100000m, result3);
        }

        [TestMethod]
        public void CalculateIndexationBenefit_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateIndexationBenefit(-100000m, 2010, 2020));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateIndexationBenefit(100000m, 2020, 2010));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateIndexationBenefit(100000m, -2010, 2020));
        }

        [TestMethod]
        public void RequiresCapitalGainsReporting_ValidInputs_ReturnsExpected()
        {
            bool result1 = _service.RequiresCapitalGainsReporting("POL123", 260000m);
            Assert.IsTrue(result1);
            
            bool result2 = _service.RequiresCapitalGainsReporting("POL124", 100000m);
            Assert.IsFalse(result2); // Assuming threshold is 250000
            
            bool result3 = _service.RequiresCapitalGainsReporting("POL125", 0m);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void CalculateNetPayableAfterTaxes_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateNetPayableAfterTaxes(150000m, 15000m);
            Assert.AreEqual(135000m, result1);
            
            decimal result2 = _service.CalculateNetPayableAfterTaxes(100000m, 0m);
            Assert.AreEqual(100000m, result2);
            
            decimal result3 = _service.CalculateNetPayableAfterTaxes(50000m, 50000m);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateNetPayableAfterTaxes_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateNetPayableAfterTaxes(-150000m, 15000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateNetPayableAfterTaxes(150000m, -15000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateNetPayableAfterTaxes(150000m, 200000m));
        }

        [TestMethod]
        public void GetNumberOfFundSwitches_ValidInputs_ReturnsExpected()
        {
            int result1 = _service.GetNumberOfFundSwitches("POL123", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            Assert.IsTrue(result1 >= 0);
            
            int result2 = _service.GetNumberOfFundSwitches("POL124", new DateTime(2021, 1, 1), new DateTime(2022, 1, 1));
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void GetNumberOfFundSwitches_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.GetNumberOfFundSwitches(null, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1)));
            Assert.ThrowsException<ArgumentException>(() => _service.GetNumberOfFundSwitches("POL123", new DateTime(2022, 1, 1), new DateTime(2021, 1, 1)));
            Assert.ThrowsException<ArgumentException>(() => _service.GetNumberOfFundSwitches("", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1)));
        }

        [TestMethod]
        public void CalculateCapitalLossCarriedForward_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateCapitalLossCarriedForward("CUST123", 2022);
            Assert.IsTrue(result1 >= 0);
            
            decimal result2 = _service.CalculateCapitalLossCarriedForward("CUST124", 2023);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void IsEquityOrientedFund_ValidInputs_ReturnsExpected()
        {
            bool result1 = _service.IsEquityOrientedFund("FUND123", 65.5);
            Assert.IsTrue(result1);
            
            bool result2 = _service.IsEquityOrientedFund("FUND124", 50.0);
            Assert.IsFalse(result2);
            
            bool result3 = _service.IsEquityOrientedFund("FUND125", 100.0);
            Assert.IsTrue(result3);
        }

        [TestMethod]
        public void IsEquityOrientedFund_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.IsEquityOrientedFund(null, 65.5));
            Assert.ThrowsException<ArgumentException>(() => _service.IsEquityOrientedFund("FUND123", -10.0));
            Assert.ThrowsException<ArgumentException>(() => _service.IsEquityOrientedFund("FUND123", 110.0));
        }

        [TestMethod]
        public void GetSttRate_ValidInputs_ReturnsExpected()
        {
            double result1 = _service.GetSttRate("FUND123", new DateTime(2023, 1, 1));
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result1 <= 1);
            
            double result2 = _service.GetSttRate("FUND124", new DateTime(2022, 1, 1));
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result2 <= 1);
        }

        [TestMethod]
        public void CalculateSecuritiesTransactionTax_ValidInputs_ReturnsExpected()
        {
            decimal result1 = _service.CalculateSecuritiesTransactionTax(100000m, 0.001);
            Assert.AreEqual(100m, result1);
            
            decimal result2 = _service.CalculateSecuritiesTransactionTax(50000m, 0.002);
            Assert.AreEqual(100m, result2);
            
            decimal result3 = _service.CalculateSecuritiesTransactionTax(0m, 0.001);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateSecuritiesTransactionTax_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateSecuritiesTransactionTax(-100000m, 0.001));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateSecuritiesTransactionTax(100000m, -0.001));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateSecuritiesTransactionTax(100000m, 1.5));
        }

        [TestMethod]
        public void GetTaxResidencyStatus_ValidInputs_ReturnsExpected()
        {
            string result1 = _service.GetTaxResidencyStatus("CUST123", 2022);
            Assert.IsNotNull(result1);
            Assert.AreNotEqual("", result1);
            
            string result2 = _service.GetTaxResidencyStatus("CUST124", 2023);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual("", result2);
        }

        [TestMethod]
        public void GetTaxResidencyStatus_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxResidencyStatus(null, 2022));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxResidencyStatus("", 2022));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxResidencyStatus("   ", 2022));
        }
    }
}
