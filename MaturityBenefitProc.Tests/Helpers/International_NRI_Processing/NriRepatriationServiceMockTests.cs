using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.International_NRI_Processing;

namespace MaturityBenefitProc.Tests.Helpers.International_NRI_Processing
{
    [TestClass]
    public class NriRepatriationServiceMockTests
    {
        private Mock<INriRepatriationService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<INriRepatriationService>();
        }

        [TestMethod]
        public void ValidateRepatriationEligibility_Eligible_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL123";
            string customerId = "CUST456";
            _mockService.Setup(s => s.ValidateRepatriationEligibility(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.ValidateRepatriationEligibility(policyId, customerId);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateRepatriationEligibility(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateRepatriationEligibility_NotEligible_ReturnsFalse()
        {
            // Arrange
            string policyId = "POL999";
            string customerId = "CUST888";
            _mockService.Setup(s => s.ValidateRepatriationEligibility(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = _mockService.Object.ValidateRepatriationEligibility(policyId, customerId);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.ValidateRepatriationEligibility(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateMaximumRepatriationAmount_ValidInput_ReturnsAmount()
        {
            // Arrange
            string policyId = "POL123";
            DateTime evalDate = new DateTime(2023, 1, 1);
            decimal expectedAmount = 50000.00m;
            _mockService.Setup(s => s.CalculateMaximumRepatriationAmount(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedAmount);

            // Act
            var result = _mockService.Object.CalculateMaximumRepatriationAmount(policyId, evalDate);

            // Assert
            Assert.AreEqual(expectedAmount, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateMaximumRepatriationAmount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCurrentFemaRepatriationLimitPercentage_ValidPolicy_ReturnsPercentage()
        {
            // Arrange
            string policyId = "POL123";
            double expectedPercentage = 100.0;
            _mockService.Setup(s => s.GetCurrentFemaRepatriationLimitPercentage(It.IsAny<string>())).Returns(expectedPercentage);

            // Act
            var result = _mockService.Object.GetCurrentFemaRepatriationLimitPercentage(policyId);

            // Assert
            Assert.AreEqual(expectedPercentage, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetCurrentFemaRepatriationLimitPercentage(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysSinceLastRepatriation_ValidCustomer_ReturnsDays()
        {
            // Arrange
            string customerId = "CUST123";
            int expectedDays = 45;
            _mockService.Setup(s => s.GetDaysSinceLastRepatriation(It.IsAny<string>())).Returns(expectedDays);

            // Act
            var result = _mockService.Object.GetDaysSinceLastRepatriation(customerId);

            // Assert
            Assert.AreEqual(expectedDays, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetDaysSinceLastRepatriation(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetNroAccountStatus_ActiveAccount_ReturnsActive()
        {
            // Arrange
            string accountId = "ACC123";
            string expectedStatus = "ACTIVE";
            _mockService.Setup(s => s.GetNroAccountStatus(It.IsAny<string>())).Returns(expectedStatus);

            // Act
            var result = _mockService.Object.GetNroAccountStatus(accountId);

            // Assert
            Assert.AreEqual(expectedStatus, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("INACTIVE", result);
            _mockService.Verify(s => s.GetNroAccountStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxDeductionAtSource_ValidInput_ReturnsTds()
        {
            // Arrange
            decimal amount = 10000m;
            string taxCode = "US";
            decimal expectedTds = 1500m;
            _mockService.Setup(s => s.CalculateTaxDeductionAtSource(It.IsAny<decimal>(), It.IsAny<string>())).Returns(expectedTds);

            // Act
            var result = _mockService.Object.CalculateTaxDeductionAtSource(amount, taxCode);

            // Assert
            Assert.AreEqual(expectedTds, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTaxDeductionAtSource(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsFemaComplianceMet_Compliant_ReturnsTrue()
        {
            // Arrange
            string customerId = "CUST123";
            decimal amount = 5000m;
            _mockService.Setup(s => s.IsFemaComplianceMet(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            // Act
            var result = _mockService.Object.IsFemaComplianceMet(customerId, amount);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsFemaComplianceMet(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GenerateForm15CbRequirementCode_ValidAmount_ReturnsCode()
        {
            // Arrange
            decimal amount = 600000m;
            string expectedCode = "REQ-15CB";
            _mockService.Setup(s => s.GenerateForm15CbRequirementCode(It.IsAny<decimal>())).Returns(expectedCode);

            // Act
            var result = _mockService.Object.GenerateForm15CbRequirementCode(amount);

            // Assert
            Assert.AreEqual(expectedCode, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("REQ"));
            Assert.AreNotEqual("", result);
            _mockService.Verify(s => s.GenerateForm15CbRequirementCode(It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingRepatriationsForFinancialYear_ValidInput_ReturnsCount()
        {
            // Arrange
            string customerId = "CUST123";
            DateTime start = new DateTime(2023, 4, 1);
            int expectedCount = 3;
            _mockService.Setup(s => s.GetRemainingRepatriationsForFinancialYear(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedCount);

            // Act
            var result = _mockService.Object.GetRemainingRepatriationsForFinancialYear(customerId, start);

            // Assert
            Assert.AreEqual(expectedCount, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(-1, result);
            _mockService.Verify(s => s.GetRemainingRepatriationsForFinancialYear(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateExchangeRateVariance_ValidInput_ReturnsVariance()
        {
            // Arrange
            string currency = "USD";
            decimal amount = 1000m;
            decimal expectedVariance = 25.50m;
            _mockService.Setup(s => s.CalculateExchangeRateVariance(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedVariance);

            // Act
            var result = _mockService.Object.CalculateExchangeRateVariance(currency, amount);

            // Assert
            Assert.AreEqual(expectedVariance, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateExchangeRateVariance(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetDoubleTaxationAvoidanceAgreementRate_ValidCountry_ReturnsRate()
        {
            // Arrange
            string countryCode = "US";
            double expectedRate = 15.0;
            _mockService.Setup(s => s.GetDoubleTaxationAvoidanceAgreementRate(It.IsAny<string>())).Returns(expectedRate);

            // Act
            var result = _mockService.Object.GetDoubleTaxationAvoidanceAgreementRate(countryCode);

            // Assert
            Assert.AreEqual(expectedRate, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetDoubleTaxationAvoidanceAgreementRate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void VerifyNreAccountFundingSource_ValidSource_ReturnsTrue()
        {
            // Arrange
            string policyId = "POL123";
            string accountId = "ACC123";
            _mockService.Setup(s => s.VerifyNreAccountFundingSource(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.VerifyNreAccountFundingSource(policyId, accountId);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.VerifyNreAccountFundingSource(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetReserveBankOfIndiaApprovalCode_ValidInput_ReturnsCode()
        {
            // Arrange
            string policyId = "POL123";
            decimal amount = 2000000m;
            string expectedCode = "RBI-APP-123";
            _mockService.Setup(s => s.GetReserveBankOfIndiaApprovalCode(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedCode);

            // Act
            var result = _mockService.Object.GetReserveBankOfIndiaApprovalCode(policyId, amount);

            // Assert
            Assert.AreEqual(expectedCode, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("RBI"));
            Assert.AreNotEqual("", result);
            _mockService.Verify(s => s.GetReserveBankOfIndiaApprovalCode(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void ComputeAllowableMaturityProceeds_ValidInput_ReturnsProceeds()
        {
            // Arrange
            string policyId = "POL123";
            decimal totalValue = 100000m;
            decimal expectedProceeds = 80000m;
            _mockService.Setup(s => s.ComputeAllowableMaturityProceeds(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedProceeds);

            // Act
            var result = _mockService.Object.ComputeAllowableMaturityProceeds(policyId, totalValue);

            // Assert
            Assert.AreEqual(expectedProceeds, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.ComputeAllowableMaturityProceeds(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateDaysToMaturity_ValidInput_ReturnsDays()
        {
            // Arrange
            string policyId = "POL123";
            DateTime current = new DateTime(2023, 1, 1);
            int expectedDays = 365;
            _mockService.Setup(s => s.CalculateDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedDays);

            // Act
            var result = _mockService.Object.CalculateDaysToMaturity(policyId, current);

            // Assert
            Assert.AreEqual(expectedDays, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.CalculateDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CheckFatcaComplianceStatus_Compliant_ReturnsTrue()
        {
            // Arrange
            string customerId = "CUST123";
            _mockService.Setup(s => s.CheckFatcaComplianceStatus(It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.CheckFatcaComplianceStatus(customerId);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.CheckFatcaComplianceStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculatePremiumPaidInForeignCurrency_ValidPolicy_ReturnsAmount()
        {
            // Arrange
            string policyId = "POL123";
            decimal expectedAmount = 15000m;
            _mockService.Setup(s => s.CalculatePremiumPaidInForeignCurrency(It.IsAny<string>())).Returns(expectedAmount);

            // Act
            var result = _mockService.Object.CalculatePremiumPaidInForeignCurrency(policyId);

            // Assert
            Assert.AreEqual(expectedAmount, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculatePremiumPaidInForeignCurrency(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetProportionateRepatriationRatio_ValidPolicy_ReturnsRatio()
        {
            // Arrange
            string policyId = "POL123";
            double expectedRatio = 0.75;
            _mockService.Setup(s => s.GetProportionateRepatriationRatio(It.IsAny<string>())).Returns(expectedRatio);

            // Act
            var result = _mockService.Object.GetProportionateRepatriationRatio(policyId);

            // Assert
            Assert.AreEqual(expectedRatio, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetProportionateRepatriationRatio(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RetrieveAuthorizedDealerBankCode_ValidCustomer_ReturnsCode()
        {
            // Arrange
            string customerId = "CUST123";
            string expectedCode = "AD-BANK-001";
            _mockService.Setup(s => s.RetrieveAuthorizedDealerBankCode(It.IsAny<string>())).Returns(expectedCode);

            // Act
            var result = _mockService.Object.RetrieveAuthorizedDealerBankCode(customerId);

            // Assert
            Assert.AreEqual(expectedCode, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("AD"));
            Assert.AreNotEqual("", result);
            _mockService.Verify(s => s.RetrieveAuthorizedDealerBankCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateCaCertificateRequirement_Required_ReturnsTrue()
        {
            // Arrange
            decimal amount = 600000m;
            _mockService.Setup(s => s.ValidateCaCertificateRequirement(It.IsAny<decimal>())).Returns(true);

            // Act
            var result = _mockService.Object.ValidateCaCertificateRequirement(amount);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateCaCertificateRequirement(It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateNetRepatriableAmount_ValidInput_ReturnsNet()
        {
            // Arrange
            decimal gross = 10000m;
            decimal tds = 1000m;
            decimal fees = 50m;
            decimal expectedNet = 8950m;
            _mockService.Setup(s => s.CalculateNetRepatriableAmount(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expectedNet);

            // Act
            var result = _mockService.Object.CalculateNetRepatriableAmount(gross, tds, fees);

            // Assert
            Assert.AreEqual(expectedNet, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateNetRepatriableAmount(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetPendingDocumentCount_ValidPolicy_ReturnsCount()
        {
            // Arrange
            string policyId = "POL123";
            int expectedCount = 2;
            _mockService.Setup(s => s.GetPendingDocumentCount(It.IsAny<string>())).Returns(expectedCount);

            // Act
            var result = _mockService.Object.GetPendingDocumentCount(policyId);

            // Assert
            Assert.AreEqual(expectedCount, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(-1, result);
            _mockService.Verify(s => s.GetPendingDocumentCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRepatriationRejectionReasonCode_Rejected_ReturnsReason()
        {
            // Arrange
            string policyId = "POL123";
            decimal amount = 5000000m;
            string expectedReason = "LIMIT_EXCEEDED";
            _mockService.Setup(s => s.GetRepatriationRejectionReasonCode(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedReason);

            // Act
            var result = _mockService.Object.GetRepatriationRejectionReasonCode(policyId, amount);

            // Assert
            Assert.AreEqual(expectedReason, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("", result);
            _mockService.Verify(s => s.GetRepatriationRejectionReasonCode(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void IsCountryInRestrictedList_Restricted_ReturnsTrue()
        {
            // Arrange
            string countryCode = "IR";
            _mockService.Setup(s => s.IsCountryInRestrictedList(It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.IsCountryInRestrictedList(countryCode);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsCountryInRestrictedList(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalRepatriatedAmountYearToDate_ValidInput_ReturnsTotal()
        {
            // Arrange
            string customerId = "CUST123";
            DateTime start = new DateTime(2023, 4, 1);
            decimal expectedTotal = 250000m;
            _mockService.Setup(s => s.GetTotalRepatriatedAmountYearToDate(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedTotal);

            // Act
            var result = _mockService.Object.GetTotalRepatriatedAmountYearToDate(customerId, start);

            // Assert
            Assert.AreEqual(expectedTotal, result);
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(-1m, result);
            _mockService.Verify(s => s.GetTotalRepatriatedAmountYearToDate(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }
    }
}