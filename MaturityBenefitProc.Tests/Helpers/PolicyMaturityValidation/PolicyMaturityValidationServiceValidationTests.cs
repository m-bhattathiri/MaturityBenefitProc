using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.PolicyMaturityValidation;

namespace MaturityBenefitProc.Tests.Helpers.PolicyMaturityValidation
{
    [TestClass]
    public class PolicyMaturityValidationServiceValidationTests
    {
        private PolicyMaturityValidationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new PolicyMaturityValidationService();
        }

        [TestMethod]
        public void RegisterPolicy_ThenValidate_PolicyExists()
        {
            _service.RegisterPolicy("VAL001", new DateTime(2017, 12, 31));
            bool matured = _service.IsPolicyMatured("VAL001", new DateTime(2018, 1, 1));
            Assert.IsTrue(matured);
            Assert.AreNotEqual(false, matured);
            Assert.IsInstanceOfType(matured, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void RegisterPolicy_WithFarFutureDate_NotMaturedNow()
        {
            _service.RegisterPolicy("VAL002", new DateTime(2050, 1, 1));
            bool matured = _service.IsPolicyMatured("VAL002", new DateTime(2017, 6, 1));
            Assert.IsFalse(matured);
            Assert.AreEqual(false, matured);
            Assert.IsInstanceOfType(matured, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void SetPremiumStatus_ThenCheckAllPaid_ReturnsCorrectly()
        {
            _service.SetPremiumStatus("VAL003", true, 24, 24);
            bool allPaid = _service.HasAllPremiumsPaid("VAL003");
            Assert.IsTrue(allPaid);
            Assert.AreNotEqual(false, allPaid);
            Assert.IsInstanceOfType(allPaid, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void SetPremiumStatus_Partial_ReturnsNotAllPaid()
        {
            _service.SetPremiumStatus("VAL004", false, 12, 24);
            bool allPaid = _service.HasAllPremiumsPaid("VAL004");
            Assert.IsFalse(allPaid);
            Assert.AreEqual(false, allPaid);
            Assert.IsInstanceOfType(allPaid, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void SetLoanStatus_WithLoan_HasOutstandingReturnsTrue()
        {
            _service.SetLoanStatus("VAL005", true, 100000m);
            bool hasLoan = _service.HasOutstandingLoan("VAL005");
            decimal amount = _service.GetOutstandingLoanAmount("VAL005");
            Assert.IsTrue(hasLoan);
            Assert.AreEqual(100000m, amount);
            Assert.IsTrue(amount > 0);
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void SetLoanStatus_WithoutLoan_HasOutstandingReturnsFalse()
        {
            _service.SetLoanStatus("VAL006", false, 0m);
            bool hasLoan = _service.HasOutstandingLoan("VAL006");
            decimal amount = _service.GetOutstandingLoanAmount("VAL006");
            Assert.IsFalse(hasLoan);
            Assert.AreEqual(0m, amount);
            Assert.IsFalse(amount > 0);
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void SetKycStatus_Verified_ReturnsComplete()
        {
            _service.SetKycStatus("CIF_VAL01", "Verified");
            bool complete = _service.IsKycComplete("CIF_VAL01");
            Assert.IsTrue(complete);
            Assert.AreNotEqual(false, complete);
            Assert.IsInstanceOfType(complete, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void SetKycStatus_InProgress_ReturnsNotComplete()
        {
            _service.SetKycStatus("CIF_VAL02", "InProgress");
            bool complete = _service.IsKycComplete("CIF_VAL02");
            Assert.IsFalse(complete);
            Assert.AreEqual(false, complete);
            Assert.IsInstanceOfType(complete, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void RegisterNominee_ThenValidate_ReturnsSuccess()
        {
            _service.RegisterNominee("VAL007", "Anita Desai");
            var result = _service.ValidateNomineeDetails("VAL007");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Message.Contains("Anita Desai"));
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsNotNull(result.ProcessedDate);
        }

        [TestMethod]
        public void RegisterNominee_WithEmptyName_ReturnsFailure()
        {
            _service.RegisterNominee("VAL008", "");
            var result = _service.ValidateNomineeDetails("VAL008");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Nominee name is empty", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RegisterIdentity_ThenValidate_WithCorrectDetails_ReturnsTrue()
        {
            _service.RegisterIdentity("CIF_VAL03", "XYZAB5678C", new DateTime(1970, 8, 20));
            bool valid = _service.ValidateClaimantIdentity("CIF_VAL03", "XYZAB5678C", new DateTime(1970, 8, 20));
            Assert.IsTrue(valid);
            Assert.AreNotEqual(false, valid);
            Assert.IsInstanceOfType(valid, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void RegisterIdentity_ThenValidate_WithWrongPan_ReturnsFalse()
        {
            _service.RegisterIdentity("CIF_VAL04", "XYZAB5678C", new DateTime(1970, 8, 20));
            bool valid = _service.ValidateClaimantIdentity("CIF_VAL04", "WRONG99999", new DateTime(1970, 8, 20));
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsInstanceOfType(valid, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void RegisterIdentity_ThenValidate_WithWrongDob_ReturnsFalse()
        {
            _service.RegisterIdentity("CIF_VAL05", "XYZAB5678C", new DateTime(1970, 8, 20));
            bool valid = _service.ValidateClaimantIdentity("CIF_VAL05", "XYZAB5678C", new DateTime(1980, 1, 1));
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
            Assert.IsInstanceOfType(valid, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void VerifyDocuments_VoterID_ReturnsSuccess()
        {
            var result = _service.VerifyDocuments("VAL010", "VoterID", "ABC1234567");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.DocumentsVerified);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.ReferenceId.Contains("VoterID"));
        }

        [TestMethod]
        public void VerifyDocuments_DrivingLicense_ReturnsSuccess()
        {
            var result = _service.VerifyDocuments("VAL011", "DrivingLicense", "DL1234567890");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.DocumentsVerified);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsTrue(result.ReferenceId.Contains("DrivingLicense"));
        }

        [TestMethod]
        public void ValidateBankDetails_WithMinLength_ReturnsSuccess()
        {
            var result = _service.ValidateBankDetails("CIF_VAL10", "123456789");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Bank details validated successfully", result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsNotNull(result.Metadata);
        }

        [TestMethod]
        public void ValidateBankDetails_WithMaxLength_ReturnsSuccess()
        {
            var result = _service.ValidateBankDetails("CIF_VAL11", "123456789012345678");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Bank details validated successfully", result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsNotNull(result.Metadata);
        }

        [TestMethod]
        public void ValidateBankDetails_WithEightDigits_ReturnsFailed()
        {
            var result = _service.ValidateBankDetails("CIF_VAL12", "12345678");
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Message.Contains("Invalid account number format"));
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CheckPremiumStatus_WithUnregisteredPolicy_ReturnsNotFound()
        {
            var result = _service.CheckPremiumStatus("UNKNOWN_PREMIUM");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Premium records not found for policy", result.Message);
            Assert.AreEqual("NotFound", result.PremiumStatus);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CheckPremiumStatus_WithEmptyPolicy_ReturnsFailed()
        {
            var result = _service.CheckPremiumStatus("");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required", result.Message);
            Assert.AreEqual("Unknown", result.PremiumStatus);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CheckPremiumStatus_AllPaid_MetadataContainsCounts()
        {
            _service.SetPremiumStatus("VAL020", true, 20, 20);
            var result = _service.CheckPremiumStatus("VAL020");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Metadata);
            Assert.IsTrue(result.Metadata.ContainsKey("PremiumsPaid"));
            Assert.AreEqual("20", result.Metadata["PremiumsPaid"]);
        }

        [TestMethod]
        public void CheckPremiumStatus_Partial_MetadataContainsCounts()
        {
            _service.SetPremiumStatus("VAL021", false, 8, 20);
            var result = _service.CheckPremiumStatus("VAL021");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Metadata);
            Assert.AreEqual("8", result.Metadata["PremiumsPaid"]);
            Assert.AreEqual("20", result.Metadata["PremiumsDue"]);
        }

        [TestMethod]
        public void GetValidationSummary_WithLoanAndNoPremiums_ReturnsHasIssues()
        {
            _service.RegisterPolicy("VAL030", new DateTime(2020, 1, 1));
            _service.SetPremiumStatus("VAL030", false, 5, 20);
            _service.SetLoanStatus("VAL030", true, 50000m);
            var result = _service.GetValidationSummary("VAL030");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("HasIssues", result.ValidationStatus);
            Assert.AreEqual("Pending", result.PremiumStatus);
            Assert.AreEqual(50000m, result.OutstandingLoan);
        }

        [TestMethod]
        public void GetValidationSummary_PremiumsPaidNoLoanNoNominee_ReturnsHasIssues()
        {
            _service.RegisterPolicy("VAL031", new DateTime(2020, 1, 1));
            _service.SetPremiumStatus("VAL031", true, 20, 20);
            _service.SetLoanStatus("VAL031", false, 0m);
            var result = _service.GetValidationSummary("VAL031");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("HasIssues", result.ValidationStatus);
            Assert.AreEqual("AllPaid", result.PremiumStatus);
            Assert.IsNotNull(result.Metadata);
        }

        [TestMethod]
        public void GetValidationSummary_AllClear_MetadataComplete()
        {
            _service.RegisterPolicy("VAL032", new DateTime(2020, 1, 1));
            _service.SetPremiumStatus("VAL032", true, 20, 20);
            _service.SetLoanStatus("VAL032", false, 0m);
            _service.RegisterNominee("VAL032", "Nominee Name");
            var result = _service.GetValidationSummary("VAL032");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Metadata);
            Assert.AreEqual("True", result.Metadata["PremiumsPaid"]);
            Assert.AreEqual("False", result.Metadata["HasLoan"]);
        }
    }
}
