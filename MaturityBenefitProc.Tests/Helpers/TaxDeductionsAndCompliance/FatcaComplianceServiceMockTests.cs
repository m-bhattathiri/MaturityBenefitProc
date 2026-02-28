using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class FatcaComplianceServiceMockTests
    {
        private Mock<IFatcaComplianceService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IFatcaComplianceService>();
        }

        [TestMethod]
        public void ValidateFatcaStatus_ValidStatus_ReturnsTrue()
        {
            _mockService.Setup(s => s.ValidateFatcaStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.ValidateFatcaStatus("POL123", "CUST456");

            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateFatcaStatus(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateFatcaStatus_InvalidStatus_ReturnsFalse()
        {
            _mockService.Setup(s => s.ValidateFatcaStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var result = _mockService.Object.ValidateFatcaStatus("POL999", "CUST888");

            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.ValidateFatcaStatus(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsCrsDeclarationRequired_Required_ReturnsTrue()
        {
            _mockService.Setup(s => s.IsCrsDeclarationRequired(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            var result = _mockService.Object.IsCrsDeclarationRequired("CUST123", 50000m);

            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsCrsDeclarationRequired(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsCrsDeclarationRequired_NotRequired_ReturnsFalse()
        {
            _mockService.Setup(s => s.IsCrsDeclarationRequired(It.IsAny<string>(), It.IsAny<decimal>())).Returns(false);

            var result = _mockService.Object.IsCrsDeclarationRequired("CUST123", 100m);

            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.IsCrsDeclarationRequired(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxResidencyCountryCode_ValidCustomer_ReturnsCode()
        {
            _mockService.Setup(s => s.GetTaxResidencyCountryCode(It.IsAny<string>())).Returns("US");

            var result = _mockService.Object.GetTaxResidencyCountryCode("CUST123");

            Assert.AreEqual("US", result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("UK", result);
            Assert.AreNotEqual("", result);

            _mockService.Verify(s => s.GetTaxResidencyCountryCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxResidencyCountryCode_UnknownCustomer_ReturnsNull()
        {
            _mockService.Setup(s => s.GetTaxResidencyCountryCode(It.IsAny<string>())).Returns((string)null);

            var result = _mockService.Object.GetTaxResidencyCountryCode("UNKNOWN");

            Assert.IsNull(result);
            Assert.AreNotEqual("US", result);
            Assert.AreNotEqual("", result);

            _mockService.Verify(s => s.GetTaxResidencyCountryCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateWithholdingTax_ValidInputs_ReturnsCalculatedAmount()
        {
            _mockService.Setup(s => s.CalculateWithholdingTax(It.IsAny<decimal>(), It.IsAny<double>())).Returns(150m);

            var result = _mockService.Object.CalculateWithholdingTax(1000m, 0.15);

            Assert.AreEqual(150m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.AreNotEqual(100m, result);

            _mockService.Verify(s => s.CalculateWithholdingTax(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateWithholdingTax_ZeroRate_ReturnsZero()
        {
            _mockService.Setup(s => s.CalculateWithholdingTax(It.IsAny<decimal>(), It.IsAny<double>())).Returns(0m);

            var result = _mockService.Object.CalculateWithholdingTax(1000m, 0.0);

            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(150m, result);
            Assert.AreNotEqual(-1m, result);

            _mockService.Verify(s => s.CalculateWithholdingTax(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableWithholdingRate_ValidW8Ben_ReturnsLowerRate()
        {
            _mockService.Setup(s => s.GetApplicableWithholdingRate(It.IsAny<string>(), It.IsAny<bool>())).Returns(0.15);

            var result = _mockService.Object.GetApplicableWithholdingRate("US", true);

            Assert.AreEqual(0.15, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.30, result);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetApplicableWithholdingRate(It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableWithholdingRate_NoW8Ben_ReturnsHigherRate()
        {
            _mockService.Setup(s => s.GetApplicableWithholdingRate(It.IsAny<string>(), It.IsAny<bool>())).Returns(0.30);

            var result = _mockService.Object.GetApplicableWithholdingRate("US", false);

            Assert.AreEqual(0.30, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.15, result);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetApplicableWithholdingRate(It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysUntilDeclarationExpiry_ValidDate_ReturnsDays()
        {
            _mockService.Setup(s => s.GetDaysUntilDeclarationExpiry(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(30);

            var result = _mockService.Object.GetDaysUntilDeclarationExpiry("CUST123", DateTime.Now);

            Assert.AreEqual(30, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.AreNotEqual(-1, result);

            _mockService.Verify(s => s.GetDaysUntilDeclarationExpiry(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysUntilDeclarationExpiry_Expired_ReturnsNegative()
        {
            _mockService.Setup(s => s.GetDaysUntilDeclarationExpiry(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(-5);

            var result = _mockService.Object.GetDaysUntilDeclarationExpiry("CUST123", DateTime.Now);

            Assert.AreEqual(-5, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.AreNotEqual(30, result);

            _mockService.Verify(s => s.GetDaysUntilDeclarationExpiry(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void VerifyTinFormat_ValidFormat_ReturnsTrue()
        {
            _mockService.Setup(s => s.VerifyTinFormat(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.VerifyTinFormat("123-45-678", "US");

            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.VerifyTinFormat(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void VerifyTinFormat_InvalidFormat_ReturnsFalse()
        {
            _mockService.Setup(s => s.VerifyTinFormat(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var result = _mockService.Object.VerifyTinFormat("INVALID", "US");

            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.VerifyTinFormat(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateComplianceReportId_ValidInputs_ReturnsId()
        {
            _mockService.Setup(s => s.GenerateComplianceReportId(It.IsAny<string>(), It.IsAny<DateTime>())).Returns("REP-123");

            var result = _mockService.Object.GenerateComplianceReportId("CUST123", DateTime.Now);

            Assert.AreEqual("REP-123", result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("REP-000", result);
            Assert.AreNotEqual("", result);

            _mockService.Verify(s => s.GenerateComplianceReportId(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CountMissingComplianceDocuments_HasMissing_ReturnsCount()
        {
            _mockService.Setup(s => s.CountMissingComplianceDocuments(It.IsAny<string>())).Returns(2);

            var result = _mockService.Object.CountMissingComplianceDocuments("CUST123");

            Assert.AreEqual(2, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.AreNotEqual(-1, result);

            _mockService.Verify(s => s.CountMissingComplianceDocuments(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CountMissingComplianceDocuments_NoMissing_ReturnsZero()
        {
            _mockService.Setup(s => s.CountMissingComplianceDocuments(It.IsAny<string>())).Returns(0);

            var result = _mockService.Object.CountMissingComplianceDocuments("CUST123");

            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(2, result);
            Assert.AreNotEqual(-1, result);

            _mockService.Verify(s => s.CountMissingComplianceDocuments(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetThresholdAmountForReporting_ValidCountry_ReturnsThreshold()
        {
            _mockService.Setup(s => s.GetThresholdAmountForReporting(It.IsAny<string>())).Returns(50000m);

            var result = _mockService.Object.GetThresholdAmountForReporting("US");

            Assert.AreEqual(50000m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.AreNotEqual(10000m, result);

            _mockService.Verify(s => s.GetThresholdAmountForReporting(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CheckHighValueDisbursementEligibility_Eligible_ReturnsTrue()
        {
            _mockService.Setup(s => s.CheckHighValueDisbursementEligibility(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            var result = _mockService.Object.CheckHighValueDisbursementEligibility("POL123", 100000m);

            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.CheckHighValueDisbursementEligibility(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CheckHighValueDisbursementEligibility_NotEligible_ReturnsFalse()
        {
            _mockService.Setup(s => s.CheckHighValueDisbursementEligibility(It.IsAny<string>(), It.IsAny<decimal>())).Returns(false);

            var result = _mockService.Object.CheckHighValueDisbursementEligibility("POL123", 100000m);

            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);

            _mockService.Verify(s => s.CheckHighValueDisbursementEligibility(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void RetrieveFatcaClassificationCode_ValidCustomer_ReturnsCode()
        {
            _mockService.Setup(s => s.RetrieveFatcaClassificationCode(It.IsAny<string>())).Returns("NFFE");

            var result = _mockService.Object.RetrieveFatcaClassificationCode("CUST123");

            Assert.AreEqual("NFFE", result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("FFE", result);
            Assert.AreNotEqual("", result);

            _mockService.Verify(s => s.RetrieveFatcaClassificationCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateCrsRiskScore_ValidInputs_ReturnsScore()
        {
            _mockService.Setup(s => s.CalculateCrsRiskScore(It.IsAny<string>(), It.IsAny<int>())).Returns(8.5);

            var result = _mockService.Object.CalculateCrsRiskScore("CUST123", 3);

            Assert.AreEqual(8.5, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.AreNotEqual(10.0, result);

            _mockService.Verify(s => s.CalculateCrsRiskScore(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void HasActiveIndicia_HasIndicia_ReturnsTrue()
        {
            _mockService.Setup(s => s.HasActiveIndicia(It.IsAny<string>())).Returns(true);

            var result = _mockService.Object.HasActiveIndicia("CUST123");

            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.HasActiveIndicia(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetGracePeriodDays_ValidInputs_ReturnsDays()
        {
            _mockService.Setup(s => s.GetGracePeriodDays(It.IsAny<string>(), It.IsAny<string>())).Returns(90);

            var result = _mockService.Object.GetGracePeriodDays("CUST123", "NFFE");

            Assert.AreEqual(90, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.AreNotEqual(30, result);

            _mockService.Verify(s => s.GetGracePeriodDays(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ComputeNetDisbursementAmount_ValidInputs_ReturnsNet()
        {
            _mockService.Setup(s => s.ComputeNetDisbursementAmount(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(850m);

            var result = _mockService.Object.ComputeNetDisbursementAmount(1000m, 150m);

            Assert.AreEqual(850m, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(1000m, result);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.ComputeNetDisbursementAmount(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void SubmitCrsReport_ValidInputs_ReturnsReportId()
        {
            _mockService.Setup(s => s.SubmitCrsReport(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>())).Returns("CRS-999");

            var result = _mockService.Object.SubmitCrsReport("CUST123", 50000m, DateTime.Now);

            Assert.AreEqual("CRS-999", result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("CRS-000", result);
            Assert.AreNotEqual("", result);

            _mockService.Verify(s => s.SubmitCrsReport(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()), Times.Once());
        }
    }
}