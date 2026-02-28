using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement;

namespace MaturityBenefitProc.Tests.Helpers.DocumentAndWorkflowManagement
{
    [TestClass]
    public class MaturityDischargeFormServiceMockTests
    {
        private Mock<IMaturityDischargeFormService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IMaturityDischargeFormService>();
        }

        [TestMethod]
        public void GenerateDischargeVoucher_ValidInputs_ReturnsVoucherId()
        {
            string expected = "VCH12345";
            _mockService.Setup(s => s.GenerateDischargeVoucher(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GenerateDischargeVoucher("POL001", new DateTime(2023, 10, 1));

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("VCH00000", result);
            Assert.IsTrue(result.StartsWith("VCH"));
            _mockService.Verify(s => s.GenerateDischargeVoucher(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GenerateDischargeVoucher_MultipleCalls_ReturnsDifferentVouchers()
        {
            string expected1 = "VCH111";
            string expected2 = "VCH222";
            _mockService.SetupSequence(s => s.GenerateDischargeVoucher(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(expected1)
                .Returns(expected2);

            var res1 = _mockService.Object.GenerateDischargeVoucher("P1", DateTime.Now);
            var res2 = _mockService.Object.GenerateDischargeVoucher("P2", DateTime.Now);

            Assert.AreEqual(expected1, res1);
            Assert.AreEqual(expected2, res2);
            Assert.AreNotEqual(res1, res2);
            Assert.IsNotNull(res1);
            Assert.IsNotNull(res2);
            _mockService.Verify(s => s.GenerateDischargeVoucher(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Exactly(2));
        }

        [TestMethod]
        public void ValidateVoucherSignatures_ValidSignatures_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.ValidateVoucherSignatures(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.ValidateVoucherSignatures("VCH123", 2);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateVoucherSignatures(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ValidateVoucherSignatures_InvalidSignatures_ReturnsFalse()
        {
            bool expected = false;
            _mockService.Setup(s => s.ValidateVoucherSignatures(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.ValidateVoucherSignatures("VCH123", 0);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.ValidateVoucherSignatures(It.IsAny<string>(), It.IsAny<int>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void ProcessReturnedVoucher_ValidVoucher_ReturnsStatus()
        {
            string expected = "Processed";
            _mockService.Setup(s => s.ProcessReturnedVoucher(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.ProcessReturnedVoucher("VCH123", DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("Pending", result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.ProcessReturnedVoucher(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateGrossMaturityValue_ValidPolicy_ReturnsAmount()
        {
            decimal expected = 500000m;
            _mockService.Setup(s => s.CalculateGrossMaturityValue(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CalculateGrossMaturityValue("POL123");

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateGrossMaturityValue(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInputs_ReturnsBonus()
        {
            decimal expected = 25000m;
            _mockService.Setup(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateTerminalBonus("POL123", 500000m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateTerminalBonus(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateOutstandingLoanDeduction_WithLoan_ReturnsDeduction()
        {
            decimal expected = 15000m;
            _mockService.Setup(s => s.CalculateOutstandingLoanDeduction(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.CalculateOutstandingLoanDeduction("POL123", DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.CalculateOutstandingLoanDeduction(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CalculateNetPayableAmount_ValidInputs_ReturnsNet()
        {
            decimal expected = 485000m;
            _mockService.Setup(s => s.CalculateNetPayableAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateNetPayableAmount("POL123", 500000m, 15000m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(500000m, result);
            _mockService.Verify(s => s.CalculateNetPayableAmount(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetPenalInterestAmount_Delayed_ReturnsInterest()
        {
            decimal expected = 500m;
            _mockService.Setup(s => s.GetPenalInterestAmount(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetPenalInterestAmount("POL123", 10);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);
            _mockService.Verify(s => s.GetPenalInterestAmount(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetBonusRate_ValidPolicy_ReturnsRate()
        {
            double expected = 5.5;
            _mockService.Setup(s => s.GetBonusRate(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.GetBonusRate("POL123", 10);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetBonusRate(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTaxDeductionPercentage_PanProvided_ReturnsLowRate()
        {
            double expected = 5.0;
            _mockService.Setup(s => s.CalculateTaxDeductionPercentage(It.IsAny<string>(), It.IsAny<bool>())).Returns(expected);

            var result = _mockService.Object.CalculateTaxDeductionPercentage("CUST123", true);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 5.0);
            Assert.AreNotEqual(20.0, result);
            _mockService.Verify(s => s.CalculateTaxDeductionPercentage(It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void GetLoanInterestRate_ValidPolicy_ReturnsRate()
        {
            double expected = 9.5;
            _mockService.Setup(s => s.GetLoanInterestRate(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetLoanInterestRate("POL123");

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);
            _mockService.Verify(s => s.GetLoanInterestRate(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsPolicyEligibleForMaturity_Eligible_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.IsPolicyEligibleForMaturity(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsPolicyEligibleForMaturity("POL123", DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsPolicyEligibleForMaturity(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void VerifyCustomerBankDetails_Valid_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.VerifyCustomerBankDetails(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.VerifyCustomerBankDetails("CUST123", "ACC123");

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.VerifyCustomerBankDetails(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CheckNeftMandateStatus_Active_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.CheckNeftMandateStatus(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CheckNeftMandateStatus("POL123");

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.CheckNeftMandateStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsDischargeFormPrinted_Printed_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.IsDischargeFormPrinted(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.IsDischargeFormPrinted("VCH123");

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsDischargeFormPrinted(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateWitnessDetails_Valid_ReturnsTrue()
        {
            bool expected = true;
            _mockService.Setup(s => s.ValidateWitnessDetails(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.ValidateWitnessDetails("VCH123", "WIT123");

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateWitnessDetails(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetRemainingDaysToMaturity_Valid_ReturnsDays()
        {
            int expected = 30;
            _mockService.Setup(s => s.GetRemainingDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetRemainingDaysToMaturity("POL123", DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetRemainingDaysToMaturity(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CountPendingRequirements_HasPending_ReturnsCount()
        {
            int expected = 2;
            _mockService.Setup(s => s.CountPendingRequirements(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CountPendingRequirements("VCH123");

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.CountPendingRequirements(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPolicyTermInYears_Valid_ReturnsTerm()
        {
            int expected = 15;
            _mockService.Setup(s => s.GetPolicyTermInYears(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetPolicyTermInYears("POL123");

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetPolicyTermInYears(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetDelayedProcessingDays_Delayed_ReturnsDays()
        {
            int expected = 5;
            _mockService.Setup(s => s.GetDelayedProcessingDays(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetDelayedProcessingDays("VCH123", DateTime.Now);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);
            _mockService.Verify(s => s.GetDelayedProcessingDays(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetVoucherStatus_Valid_ReturnsStatus()
        {
            string expected = "Approved";
            _mockService.Setup(s => s.GetVoucherStatus(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetVoucherStatus("VCH123");

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("Pending", result);
            _mockService.Verify(s => s.GetVoucherStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RetrieveDocumentReferenceNumber_Valid_ReturnsRef()
        {
            string expected = "DOC-999";
            _mockService.Setup(s => s.RetrieveDocumentReferenceNumber(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.RetrieveDocumentReferenceNumber("POL123");

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("DOC"));
            Assert.AreNotEqual("DOC-000", result);
            _mockService.Verify(s => s.RetrieveDocumentReferenceNumber(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxExemptionCode_Valid_ReturnsCode()
        {
            string expected = "EX-10D";
            _mockService.Setup(s => s.GetTaxExemptionCode(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetTaxExemptionCode("CUST123");

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            Assert.AreNotEqual("NONE", result);
            _mockService.Verify(s => s.GetTaxExemptionCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void AssignProcessingUser_Valid_ReturnsUserId()
        {
            string expected = "USER-456";
            _mockService.Setup(s => s.AssignProcessingUser(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.AssignProcessingUser("VCH123", 5);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("USER"));
            Assert.AreNotEqual("USER-000", result);
            _mockService.Verify(s => s.AssignProcessingUser(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GenerateNeftReference_Valid_ReturnsRef()
        {
            string expected = "NEFT-789";
            _mockService.Setup(s => s.GenerateNeftReference(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.GenerateNeftReference("POL123", 50000m);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StartsWith("NEFT"));
            Assert.AreNotEqual("NEFT-000", result);
            _mockService.Verify(s => s.GenerateNeftReference(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void UncalledMethod_VerifyNeverCalled()
        {
            _mockService.Setup(s => s.GetVoucherStatus(It.IsAny<string>())).Returns("Status");

            // No call made
            
            _mockService.Verify(s => s.GetVoucherStatus(It.IsAny<string>()), Times.Never());
            _mockService.Verify(s => s.GenerateDischargeVoucher(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never());
            _mockService.Verify(s => s.CalculateGrossMaturityValue(It.IsAny<string>()), Times.Never());
            Assert.IsTrue(true);
        }
    }
}