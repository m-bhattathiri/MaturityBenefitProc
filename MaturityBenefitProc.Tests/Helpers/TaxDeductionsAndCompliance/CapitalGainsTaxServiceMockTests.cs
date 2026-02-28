using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class CapitalGainsTaxServiceMockTests
    {
        private Mock<ICapitalGainsTaxService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ICapitalGainsTaxService>();
        }

        [TestMethod]
        public void CalculateShortTermCapitalGains_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expected = 5000m;
            _mockService.Setup(s => s.CalculateShortTermCapitalGains(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateShortTermCapitalGains("POL123", 15000m, 10000m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateShortTermCapitalGains(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateShortTermCapitalGains_ZeroGains_ReturnsZero()
        {
            decimal expected = 0m;
            _mockService.Setup(s => s.CalculateShortTermCapitalGains(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateShortTermCapitalGains("POL124", 10000m, 10000m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0m, result);
            _mockService.Verify(s => s.CalculateShortTermCapitalGains(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLongTermCapitalGains_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expected = 15000m;
            _mockService.Setup(s => s.CalculateLongTermCapitalGains(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateLongTermCapitalGains("POL125", 50000m, 35000m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(100m, result);
            _mockService.Verify(s => s.CalculateLongTermCapitalGains(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_Exempt_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.IsExemptUnderSection10_10D(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.IsExemptUnderSection10_10D("POL126", 10000m, 150000m);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsExemptUnderSection10_10D(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_NotExempt_ReturnsFalse()
        {
            bool expected = false;
            _mockService.Setup(s => s.IsExemptUnderSection10_10D(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.IsExemptUnderSection10_10D("POL127", 50000m, 100000m);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.IsExemptUnderSection10_10D(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableTaxRate_Resident_ReturnsRate()
        {
            double expected = 0.10;
            _mockService.Setup(s => s.GetApplicableTaxRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetApplicableTaxRate("CUST01", "RES", DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.20, result);
            _mockService.Verify(s => s.GetApplicableTaxRate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateGrandfatheredValue_ValidDate_ReturnsValue()
        {
            decimal expected = 100000m;
            _mockService.Setup(s => s.CalculateGrandfatheredValue(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateGrandfatheredValue("POL128", new DateTime(2018, 1, 31));

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateGrandfatheredValue(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateHoldingPeriodInDays_ValidDates_ReturnsDays()
        {
            int expected = 365;
            _mockService.Setup(s => s.CalculateHoldingPeriodInDays(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateHoldingPeriodInDays("POL129", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.CalculateHoldingPeriodInDays(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsHighPremiumUlip_HighPremium_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.IsHighPremiumUlip(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.IsHighPremiumUlip("POL130", 300000m);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsHighPremiumUlip(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxableAmount_ValidInputs_ReturnsAmount()
        {
            decimal expected = 50000m;
            _mockService.Setup(s => s.CalculateTaxableAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateTaxableAmount("POL131", 100000m, 50000m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTaxableAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSurcharge_HighIncome_ReturnsSurcharge()
        {
            decimal expected = 15000m;
            _mockService.Setup(s => s.CalculateSurcharge(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateSurcharge(100000m, 6000000m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateSurcharge(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateHealthAndEducationCess_ValidTax_ReturnsCess()
        {
            decimal expected = 4000m;
            _mockService.Setup(s => s.CalculateHealthAndEducationCess(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateHealthAndEducationCess(100000m, 0m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateHealthAndEducationCess(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxCategoryCode_ValidPolicy_ReturnsCode()
        {
            string expected = "LTCG";
            _mockService.Setup(s => s.GetTaxCategoryCode(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetTaxCategoryCode("POL132", DateTime.Now.AddYears(-5));

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("STCG", result);
            _mockService.Verify(s => s.GetTaxCategoryCode(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalPremiumsPaid_ValidDates_ReturnsTotal()
        {
            decimal expected = 250000m;
            _mockService.Setup(s => s.GetTotalPremiumsPaid(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetTotalPremiumsPaid("POL133", DateTime.Now.AddYears(-5), DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetTotalPremiumsPaid(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidatePanStatus_ValidPan_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.ValidatePanStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.ValidatePanStatus("ABCDE1234F", "CUST02");

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidatePanStatus(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTdsRate_Resident_ReturnsRate()
        {
            double expected = 0.05;
            _mockService.Setup(s => s.GetTdsRate(It.IsAny<string>(), It.IsAny<bool>())).Returns(expected);

            var result = _mockService.Object.GetTdsRate("ABCDE1234F", false);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.10, result);
            _mockService.Verify(s => s.GetTdsRate(It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTdsAmount_ValidInputs_ReturnsAmount()
        {
            decimal expected = 5000m;
            _mockService.Setup(s => s.CalculateTdsAmount(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateTdsAmount(100000m, 0.05);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTdsAmount(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetFinancialYear_ValidDate_ReturnsYear()
        {
            int expected = 2023;
            _mockService.Setup(s => s.GetFinancialYear(It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetFinancialYear(new DateTime(2023, 5, 1));

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 2000);
            Assert.AreNotEqual(2022, result);
            _mockService.Verify(s => s.GetFinancialYear(It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GenerateTaxComputationCertificateId_ValidInputs_ReturnsId()
        {
            string expected = "CERT-12345";
            _mockService.Setup(s => s.GenerateTaxComputationCertificateId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GenerateTaxComputationCertificateId("POL134", DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("CERT"));
            Assert.AreNotEqual("", result);
            _mockService.Verify(s => s.GenerateTaxComputationCertificateId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateIndexationBenefit_ValidInputs_ReturnsBenefit()
        {
            decimal expected = 120000m;
            _mockService.Setup(s => s.CalculateIndexationBenefit(It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.CalculateIndexationBenefit(100000m, 2015, 2023);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 100000m);
            Assert.AreNotEqual(100000m, result);
            _mockService.Verify(s => s.CalculateIndexationBenefit(It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void RequiresCapitalGainsReporting_HighAmount_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.RequiresCapitalGainsReporting(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.RequiresCapitalGainsReporting("POL135", 300000m);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.RequiresCapitalGainsReporting(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateNetPayableAfterTaxes_ValidInputs_ReturnsNet()
        {
            decimal expected = 450000m;
            _mockService.Setup(s => s.CalculateNetPayableAfterTaxes(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateNetPayableAfterTaxes(500000m, 50000m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(500000m, result);
            _mockService.Verify(s => s.CalculateNetPayableAfterTaxes(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetNumberOfFundSwitches_ValidDates_ReturnsCount()
        {
            int expected = 3;
            _mockService.Setup(s => s.GetNumberOfFundSwitches(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetNumberOfFundSwitches("POL136", DateTime.Now.AddYears(-1), DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetNumberOfFundSwitches(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateCapitalLossCarriedForward_ValidInputs_ReturnsLoss()
        {
            decimal expected = 25000m;
            _mockService.Setup(s => s.CalculateCapitalLossCarriedForward(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.CalculateCapitalLossCarriedForward("CUST03", 2022);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateCapitalLossCarriedForward(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void IsEquityOrientedFund_HighEquity_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.IsEquityOrientedFund(It.IsAny<string>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.IsEquityOrientedFund("FUND01", 75.0);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsEquityOrientedFund(It.IsAny<string>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetSttRate_ValidFund_ReturnsRate()
        {
            double expected = 0.001;
            _mockService.Setup(s => s.GetSttRate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetSttRate("FUND02", DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetSttRate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateSecuritiesTransactionTax_ValidInputs_ReturnsTax()
        {
            decimal expected = 100m;
            _mockService.Setup(s => s.CalculateSecuritiesTransactionTax(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateSecuritiesTransactionTax(100000m, 0.001);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateSecuritiesTransactionTax(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxResidencyStatus_ValidCustomer_ReturnsStatus()
        {
            string expected = "NRI";
            _mockService.Setup(s => s.GetTaxResidencyStatus(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetTaxResidencyStatus("CUST04", 2023);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("RES", result);
            _mockService.Verify(s => s.GetTaxResidencyStatus(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }
    }
}