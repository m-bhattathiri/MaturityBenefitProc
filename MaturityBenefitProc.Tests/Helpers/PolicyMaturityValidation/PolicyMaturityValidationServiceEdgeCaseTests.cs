using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.PolicyMaturityValidation;

namespace MaturityBenefitProc.Tests.Helpers.PolicyMaturityValidation
{
    [TestClass]
    public class PolicyMaturityValidationServiceEdgeCaseTests
    {
        private PolicyMaturityValidationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new PolicyMaturityValidationService();
        }

        [TestMethod]
        public void IsPolicyMatured_WithMinDateCheck_ReturnsFalse()
        {
            _service.RegisterPolicy("EDGE001", new DateTime(2017, 6, 15));
            bool result = _service.IsPolicyMatured("EDGE001", DateTime.MinValue);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsPolicyMatured_WithMaxDateCheck_ReturnsTrue()
        {
            _service.RegisterPolicy("EDGE002", new DateTime(2017, 6, 15));
            bool result = _service.IsPolicyMatured("EDGE002", DateTime.MaxValue);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsPolicyMatured_WithNullPolicy_ReturnsFalse()
        {
            bool result = _service.IsPolicyMatured(null, DateTime.UtcNow);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsPolicyMatured_WithEmptyPolicy_ReturnsFalse()
        {
            bool result = _service.IsPolicyMatured("", DateTime.UtcNow);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsKycComplete_WithNullCif_ReturnsFalse()
        {
            bool result = _service.IsKycComplete(null);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsKycComplete_WithEmptyCif_ReturnsFalse()
        {
            bool result = _service.IsKycComplete("");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsKycComplete_WithRejectedStatus_ReturnsFalse()
        {
            _service.SetKycStatus("CIF_REJ", "Rejected");
            bool result = _service.IsKycComplete("CIF_REJ");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsKycComplete_WithPartialStatus_ReturnsFalse()
        {
            _service.SetKycStatus("CIF_PART", "Partial");
            bool result = _service.IsKycComplete("CIF_PART");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void HasAllPremiumsPaid_WithNullPolicy_ReturnsFalse()
        {
            bool result = _service.HasAllPremiumsPaid(null);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void HasAllPremiumsPaid_WithUnknownPolicy_ReturnsFalse()
        {
            bool result = _service.HasAllPremiumsPaid("UNKNOWN_POL");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void HasOutstandingLoan_WithNullPolicy_ReturnsFalse()
        {
            bool result = _service.HasOutstandingLoan(null);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void HasOutstandingLoan_WithUnknownPolicy_ReturnsFalse()
        {
            bool result = _service.HasOutstandingLoan("UNKNOWN_LOAN");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void GetOutstandingLoanAmount_WithNullPolicy_ReturnsZero()
        {
            decimal amount = _service.GetOutstandingLoanAmount(null);
            Assert.AreEqual(0m, amount);
            Assert.IsFalse(amount > 0);
            Assert.IsInstanceOfType(amount, typeof(decimal));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void GetOutstandingLoanAmount_WithEmptyPolicy_ReturnsZero()
        {
            decimal amount = _service.GetOutstandingLoanAmount("");
            Assert.AreEqual(0m, amount);
            Assert.IsFalse(amount > 0);
            Assert.IsInstanceOfType(amount, typeof(decimal));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void GetDaysToMaturity_WithNullPolicy_ReturnsZero()
        {
            int days = _service.GetDaysToMaturity(null, DateTime.UtcNow);
            Assert.AreEqual(0, days);
            Assert.IsFalse(days > 0);
            Assert.IsInstanceOfType(days, typeof(int));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void GetDaysToMaturity_WithExactSameDate_ReturnsZero()
        {
            var matDate = new DateTime(2017, 6, 15);
            _service.RegisterPolicy("EDGE010", matDate);
            int days = _service.GetDaysToMaturity("EDGE010", matDate);
            Assert.AreEqual(0, days);
            Assert.IsFalse(days < 0);
            Assert.IsInstanceOfType(days, typeof(int));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void GetDaysToMaturity_WithOneDayBefore_ReturnsOne()
        {
            _service.RegisterPolicy("EDGE011", new DateTime(2017, 6, 16));
            int days = _service.GetDaysToMaturity("EDGE011", new DateTime(2017, 6, 15));
            Assert.AreEqual(1, days);
            Assert.IsTrue(days > 0);
            Assert.IsInstanceOfType(days, typeof(int));
            Assert.AreNotEqual(0, days);
        }

        [TestMethod]
        public void IsWithinClaimWindow_WithExactMaturityDate_ReturnsTrue()
        {
            var matDate = new DateTime(2017, 6, 15);
            _service.RegisterPolicy("EDGE020", matDate);
            bool result = _service.IsWithinClaimWindow("EDGE020", matDate);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsWithinClaimWindow_WithExactThreeYearsAfter_ReturnsTrue()
        {
            var matDate = new DateTime(2017, 6, 15);
            _service.RegisterPolicy("EDGE021", matDate);
            bool result = _service.IsWithinClaimWindow("EDGE021", matDate.AddYears(3));
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsWithinClaimWindow_WithOneDayOverThreeYears_ReturnsFalse()
        {
            var matDate = new DateTime(2017, 6, 15);
            _service.RegisterPolicy("EDGE022", matDate);
            bool result = _service.IsWithinClaimWindow("EDGE022", matDate.AddYears(3).AddDays(1));
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsWithinClaimWindow_WithBeforeMaturity_ReturnsFalse()
        {
            _service.RegisterPolicy("EDGE023", new DateTime(2017, 6, 15));
            bool result = _service.IsWithinClaimWindow("EDGE023", new DateTime(2017, 6, 14));
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsWithinClaimWindow_WithNullPolicy_ReturnsFalse()
        {
            bool result = _service.IsWithinClaimWindow(null, DateTime.UtcNow);
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void ValidateClaimantIdentity_WithNullCif_ReturnsFalse()
        {
            bool result = _service.ValidateClaimantIdentity(null, "ABCDE1234F", new DateTime(1985, 5, 15));
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void ValidateClaimantIdentity_WithNullPan_ReturnsFalse()
        {
            _service.RegisterIdentity("CIF_EDGE", "ABCDE1234F", new DateTime(1985, 5, 15));
            bool result = _service.ValidateClaimantIdentity("CIF_EDGE", null, new DateTime(1985, 5, 15));
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void ValidateClaimantIdentity_WithUnregisteredCif_ReturnsFalse()
        {
            bool result = _service.ValidateClaimantIdentity("UNKNOWN_CIF", "ABCDE1234F", new DateTime(1985, 5, 15));
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void VerifyDocuments_WithPassport_ReturnsSuccess()
        {
            var result = _service.VerifyDocuments("EDGE030", "Passport", "A1234567");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.DocumentsVerified);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.ReferenceId.Contains("Passport"));
        }

        [TestMethod]
        public void VerifyDocuments_WithShortPassport_ReturnsFailed()
        {
            var result = _service.VerifyDocuments("EDGE031", "Passport", "ABC");
            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.DocumentsVerified);
            Assert.IsTrue(result.Message.Contains("Invalid document number format"));
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void VerifyDocuments_WithEmptyDocNumber_ReturnsFailed()
        {
            var result = _service.VerifyDocuments("EDGE032", "PAN", "");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Document number is required", result.Message);
            Assert.IsFalse(result.DocumentsVerified);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ValidateBankDetails_WithNullCif_ReturnsFailed()
        {
            var result = _service.ValidateBankDetails(null, "1234567890");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("CIF number is required", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateBankDetails_WithNullAccount_ReturnsFailed()
        {
            var result = _service.ValidateBankDetails("CIF001", null);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Account number is required", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateBankDetails_WithTooLongAccount_ReturnsFailed()
        {
            var result = _service.ValidateBankDetails("CIF001", "1234567890123456789");
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Message.Contains("Invalid account number format"));
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateNomineeDetails_WithNullPolicy_ReturnsFailed()
        {
            var result = _service.ValidateNomineeDetails(null);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required for nominee validation", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetValidationSummary_WithNullPolicy_ReturnsFailed()
        {
            var result = _service.GetValidationSummary(null);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required for validation summary", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetValidationSummary_WithUnregisteredPolicy_ReturnsNotFound()
        {
            var result = _service.GetValidationSummary("UNKNOWN_SUMMARY");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy not found", result.Message);
            Assert.AreEqual("NotFound", result.ValidationStatus);
            Assert.IsNotNull(result);
        }
    }
}
