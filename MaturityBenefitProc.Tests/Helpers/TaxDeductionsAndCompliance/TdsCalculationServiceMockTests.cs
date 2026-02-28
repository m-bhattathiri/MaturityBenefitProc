using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class TdsCalculationServiceMockTests
    {
        private Mock<ITdsCalculationService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ITdsCalculationService>();
        }

        [TestMethod]
        public void CalculateBaseTds_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 1500.50m;
            _mockService.Setup(s => s.CalculateBaseTds(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);
            
            var result = _mockService.Object.CalculateBaseTds("POL123", 15000m);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateBaseTds(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableTdsRate_ValidPan_ReturnsExpectedRate()
        {
            double expectedValue = 5.0;
            _mockService.Setup(s => s.GetApplicableTdsRate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetApplicableTdsRate("ABCDE1234F", DateTime.Now);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(10.0, result);
            _mockService.Verify(s => s.GetApplicableTdsRate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsTdsApplicable_AmountAboveThreshold_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsTdsApplicable(It.IsAny<decimal>(), It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.IsTdsApplicable(150000m, "194DA");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsTdsApplicable(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetFinancialYear_ValidDate_ReturnsExpectedYear()
        {
            int expectedValue = 2023;
            _mockService.Setup(s => s.GetFinancialYear(It.IsAny<DateTime>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetFinancialYear(new DateTime(2023, 5, 1));
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 2000);
            Assert.AreNotEqual(2022, result);
            _mockService.Verify(s => s.GetFinancialYear(It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxCategoryCode_ValidCustomer_ReturnsExpectedCode()
        {
            string expectedValue = "IND";
            _mockService.Setup(s => s.GetTaxCategoryCode(It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetTaxCategoryCode("CUST123");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("HUF", result);
            _mockService.Verify(s => s.GetTaxCategoryCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ComputeSurcharge_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 500m;
            _mockService.Setup(s => s.ComputeSurcharge(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);
            
            var result = _mockService.Object.ComputeSurcharge(5000m, 10.0);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.ComputeSurcharge(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void ComputeEducationCess_ValidTax_ReturnsExpectedAmount()
        {
            decimal expectedValue = 200m;
            _mockService.Setup(s => s.ComputeEducationCess(It.IsAny<decimal>())).Returns(expectedValue);
            
            var result = _mockService.Object.ComputeEducationCess(5000m);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.ComputeEducationCess(It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalTaxableAmount_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 100000m;
            _mockService.Setup(s => s.GetTotalTaxableAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetTotalTaxableAmount("POL123", 150000m);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetTotalTaxableAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateNetPayout_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 145000m;
            _mockService.Setup(s => s.CalculateNetPayout(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);
            
            var result = _mockService.Object.CalculateNetPayout(150000m, 5000m);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateNetPayout(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetExemptAmount_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 50000m;
            _mockService.Setup(s => s.GetExemptAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetExemptAmount("POL123", 50000m);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetExemptAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTdsOnSurrender_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 2500m;
            _mockService.Setup(s => s.CalculateTdsOnSurrender(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(expectedValue);
            
            var result = _mockService.Object.CalculateTdsOnSurrender("POL123", 50000m, 36);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTdsOnSurrender(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTdsOnMaturity_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 5000m;
            _mockService.Setup(s => s.CalculateTdsOnMaturity(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);
            
            var result = _mockService.Object.CalculateTdsOnMaturity("POL123", 100000m);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTdsOnMaturity(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetThresholdLimit_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 100000m;
            _mockService.Setup(s => s.GetThresholdLimit(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetThresholdLimit("194DA", 2023);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetThresholdLimit(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ComputePenaltyAmount_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 200m;
            _mockService.Setup(s => s.ComputePenaltyAmount(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);
            
            var result = _mockService.Object.ComputePenaltyAmount("ABCDE1234F", 10);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.ComputePenaltyAmount(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetEffectiveTaxRate_ValidInputs_ReturnsExpectedRate()
        {
            double expectedValue = 5.2;
            _mockService.Setup(s => s.GetEffectiveTaxRate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetEffectiveTaxRate(5.0, 0.0, 4.0);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetEffectiveTaxRate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetSurchargeRate_ValidInputs_ReturnsExpectedRate()
        {
            double expectedValue = 10.0;
            _mockService.Setup(s => s.GetSurchargeRate(It.IsAny<decimal>(), It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetSurchargeRate(6000000m, "IND");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetSurchargeRate(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ComputeCessPercentage_ValidYear_ReturnsExpectedPercentage()
        {
            double expectedValue = 4.0;
            _mockService.Setup(s => s.ComputeCessPercentage(It.IsAny<int>())).Returns(expectedValue);
            
            var result = _mockService.Object.ComputeCessPercentage(2023);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.ComputeCessPercentage(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetMarginalReliefRatio_ValidInputs_ReturnsExpectedRatio()
        {
            double expectedValue = 0.5;
            _mockService.Setup(s => s.GetMarginalReliefRatio(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetMarginalReliefRatio(5100000m, 1500000m);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetMarginalReliefRatio(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetDtaaBenefitRate_ValidCountry_ReturnsExpectedRate()
        {
            double expectedValue = 10.0;
            _mockService.Setup(s => s.GetDtaaBenefitRate(It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetDtaaBenefitRate("US");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetDtaaBenefitRate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsPanValid_ValidPan_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsPanValid(It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.IsPanValid("ABCDE1234F");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsPanValid(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsNriCustomer_ValidCustomer_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsNriCustomer(It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.IsNriCustomer("CUST123");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsNriCustomer(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void HasForm15GOrH_ValidCustomer_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.HasForm15GOrH(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);
            
            var result = _mockService.Object.HasForm15GOrH("CUST123", 2023);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.HasForm15GOrH(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void IsAmountAboveThreshold_ValidInputs_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsAmountAboveThreshold(It.IsAny<decimal>(), It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.IsAmountAboveThreshold(150000m, "194DA");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsAmountAboveThreshold(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsAadhaarLinked_ValidPan_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsAadhaarLinked(It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.IsAadhaarLinked("ABCDE1234F");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsAadhaarLinked(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_ValidInputs_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.IsExemptUnderSection10_10D(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedValue);
            
            var result = _mockService.Object.IsExemptUnderSection10_10D("POL123", 1000000m, 50000m);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsExemptUnderSection10_10D(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void RequiresHigherTdsRate_ValidPan_ReturnsTrue()
        {
            bool expectedValue = true;
            _mockService.Setup(s => s.RequiresHigherTdsRate(It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.RequiresHigherTdsRate("ABCDE1234F");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.RequiresHigherTdsRate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysDelayed_ValidDates_ReturnsExpectedDays()
        {
            int expectedValue = 5;
            _mockService.Setup(s => s.CalculateDaysDelayed(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);
            
            var result = _mockService.Object.CalculateDaysDelayed(DateTime.Now.AddDays(-5), DateTime.Now);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.CalculateDaysDelayed(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalTransactionsInYear_ValidInputs_ReturnsExpectedCount()
        {
            int expectedValue = 3;
            _mockService.Setup(s => s.GetTotalTransactionsInYear(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetTotalTransactionsInYear("ABCDE1234F", 2023);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetTotalTransactionsInYear(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetAgeAtPayout_ValidInputs_ReturnsExpectedAge()
        {
            int expectedValue = 45;
            _mockService.Setup(s => s.GetAgeAtPayout(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetAgeAtPayout("CUST123", DateTime.Now);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetAgeAtPayout(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPolicyDurationInMonths_ValidDates_ReturnsExpectedMonths()
        {
            int expectedValue = 60;
            _mockService.Setup(s => s.GetPolicyDurationInMonths(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetPolicyDurationInMonths(DateTime.Now.AddYears(-5), DateTime.Now);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetPolicyDurationInMonths(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetChallanNumber_ValidTransaction_ReturnsExpectedNumber()
        {
            string expectedValue = "CHL12345";
            _mockService.Setup(s => s.GetChallanNumber(It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetChallanNumber("TXN123");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("CHL00000", result);
            _mockService.Verify(s => s.GetChallanNumber(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateTdsCertificateId_ValidInputs_ReturnsExpectedId()
        {
            string expectedValue = "CERT2023ABCDE1234F";
            _mockService.Setup(s => s.GenerateTdsCertificateId(It.IsAny<string>(), It.IsAny<int>())).Returns(expectedValue);
            
            var result = _mockService.Object.GenerateTdsCertificateId("ABCDE1234F", 2023);
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("CERT0000", result);
            _mockService.Verify(s => s.GenerateTdsCertificateId(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetSectionCode_ValidPayoutType_ReturnsExpectedCode()
        {
            string expectedValue = "194DA";
            _mockService.Setup(s => s.GetSectionCode(It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetSectionCode("MATURITY");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("194A", result);
            _mockService.Verify(s => s.GetSectionCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetAssesseeType_ValidCustomer_ReturnsExpectedType()
        {
            string expectedValue = "INDIVIDUAL";
            _mockService.Setup(s => s.GetAssesseeType(It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.GetAssesseeType("CUST123");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("COMPANY", result);
            _mockService.Verify(s => s.GetAssesseeType(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTdsForNri_ValidInputs_ReturnsExpectedAmount()
        {
            decimal expectedValue = 3000m;
            _mockService.Setup(s => s.CalculateTdsForNri(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>())).Returns(expectedValue);
            
            var result = _mockService.Object.CalculateTdsForNri("POL123", 30000m, "US");
            
            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTdsForNri(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Once());
        }
    }
}