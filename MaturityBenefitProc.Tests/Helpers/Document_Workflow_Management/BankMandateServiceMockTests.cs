using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.Document_Workflow_Management;

namespace MaturityBenefitProc.Tests.Helpers.Document_Workflow_Management
{
    [TestClass]
    public class BankMandateServiceMockTests
    {
        private Mock<IBankMandateService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IBankMandateService>();
        }

        [TestMethod]
        public void ValidateNachMandate_ValidInputs_ReturnsTrue()
        {
            // Arrange
            string mandateId = "MAND123";
            string accNum = "123456789";
            _mockService.Setup(s => s.ValidateNachMandate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.ValidateNachMandate(mandateId, accNum);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateNachMandate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateNachMandate_InvalidInputs_ReturnsFalse()
        {
            // Arrange
            string mandateId = "MAND999";
            string accNum = "000000000";
            _mockService.Setup(s => s.ValidateNachMandate(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = _mockService.Object.ValidateNachMandate(mandateId, accNum);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.ValidateNachMandate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void VerifyEMandateStatus_ValidStatus_ReturnsTrue()
        {
            // Arrange
            string eMandateId = "EMAND123";
            DateTime date = DateTime.Now;
            _mockService.Setup(s => s.VerifyEMandateStatus(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(true);

            // Act
            var result = _mockService.Object.VerifyEMandateStatus(eMandateId, date);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.VerifyEMandateStatus(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void VerifyEMandateStatus_ExpiredStatus_ReturnsFalse()
        {
            // Arrange
            string eMandateId = "EMAND999";
            DateTime date = DateTime.Now.AddDays(-10);
            _mockService.Setup(s => s.VerifyEMandateStatus(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(false);

            // Act
            var result = _mockService.Object.VerifyEMandateStatus(eMandateId, date);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.VerifyEMandateStatus(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void RegisterNewMandate_ValidDetails_ReturnsMandateId()
        {
            // Arrange
            string expectedId = "NEW_MAND_001";
            _mockService.Setup(s => s.RegisterNewMandate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedId);

            // Act
            var result = _mockService.Object.RegisterNewMandate("CUST1", "BANK1", "ACC1");

            // Assert
            Assert.AreEqual(expectedId, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("NEW"));
            Assert.AreNotEqual("OLD_MAND_001", result);
            _mockService.Verify(s => s.RegisterNewMandate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RegisterNewMandate_InvalidDetails_ReturnsNull()
        {
            // Arrange
            _mockService.Setup(s => s.RegisterNewMandate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns((string)null);

            // Act
            var result = _mockService.Object.RegisterNewMandate("CUST_INV", "BANK_INV", "ACC_INV");

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual(null, result);
            Assert.AreNotEqual("ANY_ID", result);
            _mockService.Verify(s => s.RegisterNewMandate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateMaxDebitAmount_ValidRequest_ReturnsCalculatedAmount()
        {
            // Arrange
            decimal expected = 5000.00m;
            _mockService.Setup(s => s.CalculateMaxDebitAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateMaxDebitAmount("MAND1", 6000.00m);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateMaxDebitAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateMaxDebitAmount_ZeroRequest_ReturnsZero()
        {
            // Arrange
            decimal expected = 0m;
            _mockService.Setup(s => s.CalculateMaxDebitAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateMaxDebitAmount("MAND1", 0m);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsFalse(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(100m, result);
            _mockService.Verify(s => s.CalculateMaxDebitAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetMandateSuccessRate_ValidRange_ReturnsRate()
        {
            // Arrange
            double expected = 95.5;
            _mockService.Setup(s => s.GetMandateSuccessRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetMandateSuccessRate("BANK1", DateTime.Now.AddDays(-30), DateTime.Now);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(result > 90);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetMandateSuccessRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingValidityDays_ActiveMandate_ReturnsDays()
        {
            // Arrange
            int expected = 365;
            _mockService.Setup(s => s.GetRemainingValidityDays(It.IsAny<string>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetRemainingValidityDays("MAND1");

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetRemainingValidityDays(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingValidityDays_ExpiredMandate_ReturnsZero()
        {
            // Arrange
            int expected = 0;
            _mockService.Setup(s => s.GetRemainingValidityDays(It.IsAny<string>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetRemainingValidityDays("MAND_EXP");

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsFalse(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(365, result);
            _mockService.Verify(s => s.GetRemainingValidityDays(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsAccountEligibleForDirectCredit_Eligible_ReturnsTrue()
        {
            // Arrange
            _mockService.Setup(s => s.IsAccountEligibleForDirectCredit(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.IsAccountEligibleForDirectCredit("ACC1", "IFSC1");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsAccountEligibleForDirectCredit(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsAccountEligibleForDirectCredit_Ineligible_ReturnsFalse()
        {
            // Arrange
            _mockService.Setup(s => s.IsAccountEligibleForDirectCredit(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = _mockService.Object.IsAccountEligibleForDirectCredit("ACC2", "IFSC2");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.IsAccountEligibleForDirectCredit(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void UpdateMandateStatus_ValidUpdate_ReturnsNewStatus()
        {
            // Arrange
            string expected = "ACTIVE";
            _mockService.Setup(s => s.UpdateMandateStatus(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            // Act
            var result = _mockService.Object.UpdateMandateStatus("MAND1", "ACTIVE");

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("INACTIVE", result);
            _mockService.Verify(s => s.UpdateMandateStatus(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CountActiveMandatesForCustomer_HasMandates_ReturnsCount()
        {
            // Arrange
            int expected = 3;
            _mockService.Setup(s => s.CountActiveMandatesForCustomer(It.IsAny<string>())).Returns(expected);

            // Act
            var result = _mockService.Object.CountActiveMandatesForCustomer("CUST1");

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.CountActiveMandatesForCustomer(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalCreditedAmount_ValidMandate_ReturnsAmount()
        {
            // Arrange
            decimal expected = 15000.50m;
            _mockService.Setup(s => s.GetTotalCreditedAmount(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetTotalCreditedAmount("MAND1", DateTime.Now);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(result > 10000m);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetTotalCreditedAmount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CheckMandateLimitExceeded_Exceeded_ReturnsTrue()
        {
            // Arrange
            _mockService.Setup(s => s.CheckMandateLimitExceeded(It.IsAny<string>(), It.IsAny<decimal>())).Returns(true);

            // Act
            var result = _mockService.Object.CheckMandateLimitExceeded("MAND1", 100000m);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.CheckMandateLimitExceeded(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CheckMandateLimitExceeded_NotExceeded_ReturnsFalse()
        {
            // Arrange
            _mockService.Setup(s => s.CheckMandateLimitExceeded(It.IsAny<string>(), It.IsAny<decimal>())).Returns(false);

            // Act
            var result = _mockService.Object.CheckMandateLimitExceeded("MAND1", 100m);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.CheckMandateLimitExceeded(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void RetrieveBankIfscFromMandate_ValidMandate_ReturnsIfsc()
        {
            // Arrange
            string expected = "HDFC0001234";
            _mockService.Setup(s => s.RetrieveBankIfscFromMandate(It.IsAny<string>())).Returns(expected);

            // Act
            var result = _mockService.Object.RetrieveBankIfscFromMandate("MAND1");

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("HDFC"));
            Assert.AreNotEqual("SBIN0001234", result);
            _mockService.Verify(s => s.RetrieveBankIfscFromMandate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateRejectionRatio_ValidInput_ReturnsRatio()
        {
            // Arrange
            double expected = 2.5;
            _mockService.Setup(s => s.CalculateRejectionRatio(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expected);

            // Act
            var result = _mockService.Object.CalculateRejectionRatio("BANK1", 5, 2023);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.CalculateRejectionRatio(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CancelMandate_ValidReason_ReturnsTrue()
        {
            // Arrange
            _mockService.Setup(s => s.CancelMandate(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.CancelMandate("MAND1", "CUST_REQ");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.CancelMandate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CancelMandate_InvalidReason_ReturnsFalse()
        {
            // Arrange
            _mockService.Setup(s => s.CancelMandate(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = _mockService.Object.CancelMandate("MAND1", "UNKNOWN");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.CancelMandate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPendingMandateAuthorizations_HasPending_ReturnsCount()
        {
            // Arrange
            int expected = 15;
            _mockService.Setup(s => s.GetPendingMandateAuthorizations(It.IsAny<string>())).Returns(expected);

            // Act
            var result = _mockService.Object.GetPendingMandateAuthorizations("BRANCH1");

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetPendingMandateAuthorizations(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ComputeMandateProcessingFee_Priority_ReturnsHigherFee()
        {
            // Arrange
            decimal expected = 150.00m;
            _mockService.Setup(s => s.ComputeMandateProcessingFee(It.IsAny<string>(), It.IsAny<bool>())).Returns(expected);

            // Act
            var result = _mockService.Object.ComputeMandateProcessingFee("NACH", true);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsTrue(result > 100m);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(50m, result);
            _mockService.Verify(s => s.ComputeMandateProcessingFee(It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void GenerateMandateReferenceNumber_ValidInput_ReturnsRefNum()
        {
            // Arrange
            string expected = "REF-2023-001";
            _mockService.Setup(s => s.GenerateMandateReferenceNumber(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            // Act
            var result = _mockService.Object.GenerateMandateReferenceNumber("CUST1", DateTime.Now);

            // Assert
            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("REF"));
            Assert.AreNotEqual("REF-000", result);
            _mockService.Verify(s => s.GenerateMandateReferenceNumber(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateCustomerSignature_ValidHash_ReturnsTrue()
        {
            // Arrange
            _mockService.Setup(s => s.ValidateCustomerSignature(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = _mockService.Object.ValidateCustomerSignature("MAND1", "HASH123");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateCustomerSignature(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void MultipleInvocations_VerifyCounts()
        {
            // Arrange
            _mockService.Setup(s => s.CountActiveMandatesForCustomer(It.IsAny<string>())).Returns(2);

            // Act
            _mockService.Object.CountActiveMandatesForCustomer("CUST1");
            _mockService.Object.CountActiveMandatesForCustomer("CUST2");

            // Assert
            _mockService.Verify(s => s.CountActiveMandatesForCustomer(It.IsAny<string>()), Times.Exactly(2));
            _mockService.Verify(s => s.ValidateNachMandate(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            Assert.IsNotNull(_mockService.Object);
        }
    }
}