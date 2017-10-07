using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.PolicyMaturityValidation;

namespace MaturityBenefitProc.Tests.Helpers.PolicyMaturityValidation
{
    [TestClass]
    public class PolicyMaturityValidationServiceTests
    {
        private PolicyMaturityValidationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new PolicyMaturityValidationService();
        }

        [TestMethod]
        public void ValidatePolicyForMaturity_WithValidPolicy_AllPremiumsPaid_ReturnsSuccess()
        {
            _service.RegisterPolicy("POL001", new DateTime(2017, 6, 15));
            _service.SetPremiumStatus("POL001", true, 20, 20);
            _service.SetLoanStatus("POL001", false, 0m);
            _service.RegisterNominee("POL001", "Ramesh Kumar");

            var result = _service.ValidatePolicyForMaturity("POL001");

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Policy is eligible for maturity benefit", result.Message);
            Assert.AreEqual("Approved", result.ValidationStatus);
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void ValidatePolicyForMaturity_WithNullPolicyNumber_ReturnsFailed()
        {
            var result = _service.ValidatePolicyForMaturity(null);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required", result.Message);
            Assert.AreEqual("Failed", result.ValidationStatus);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ValidatePolicyForMaturity_WithEmptyPolicyNumber_ReturnsFailed()
        {
            var result = _service.ValidatePolicyForMaturity("");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required", result.Message);
            Assert.AreEqual("Failed", result.ValidationStatus);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ValidatePolicyForMaturity_WithWhitespacePolicyNumber_ReturnsFailed()
        {
            var result = _service.ValidatePolicyForMaturity("   ");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required", result.Message);
            Assert.AreEqual("Failed", result.ValidationStatus);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ValidatePolicyForMaturity_WithUnregisteredPolicy_ReturnsNotFound()
        {
            var result = _service.ValidatePolicyForMaturity("UNKNOWN123");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy not found in system", result.Message);
            Assert.AreEqual("NotFound", result.ValidationStatus);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void ValidatePolicyForMaturity_WithPendingPremiums_ReturnsPremiumsPending()
        {
            _service.RegisterPolicy("POL002", new DateTime(2018, 1, 1));
            _service.SetPremiumStatus("POL002", false, 15, 20);

            var result = _service.ValidatePolicyForMaturity("POL002");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not all premiums have been paid", result.Message);
            Assert.AreEqual("PremiumsPending", result.ValidationStatus);
            Assert.AreEqual("Incomplete", result.PremiumStatus);
        }

        [TestMethod]
        public void ValidatePolicyForMaturity_WithOutstandingLoan_ReturnsLoanOutstanding()
        {
            _service.RegisterPolicy("POL003", new DateTime(2017, 12, 31));
            _service.SetPremiumStatus("POL003", true, 20, 20);
            _service.SetLoanStatus("POL003", true, 50000m);

            var result = _service.ValidatePolicyForMaturity("POL003");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Outstanding loan exists against policy", result.Message);
            Assert.AreEqual("LoanOutstanding", result.ValidationStatus);
            Assert.AreEqual(50000m, result.OutstandingLoan);
        }

        [TestMethod]
        public void VerifyDocuments_WithValidPan_ReturnsSuccess()
        {
            var result = _service.VerifyDocuments("POL001", "PAN", "ABCDE1234F");

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Document verified successfully", result.Message);
            Assert.IsTrue(result.DocumentsVerified);
            Assert.IsNotNull(result.ReferenceId);
        }

        [TestMethod]
        public void VerifyDocuments_WithValidAadhaar_ReturnsSuccess()
        {
            var result = _service.VerifyDocuments("POL001", "Aadhaar", "123456789012");

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Document verified successfully", result.Message);
            Assert.IsTrue(result.DocumentsVerified);
            Assert.IsTrue(result.ReferenceId.Contains("Aadhaar"));
        }

        [TestMethod]
        public void VerifyDocuments_WithInvalidPanLength_ReturnsFailed()
        {
            var result = _service.VerifyDocuments("POL001", "PAN", "ABC");

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Message.Contains("Invalid document number format"));
            Assert.IsFalse(result.DocumentsVerified);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void VerifyDocuments_WithInvalidAadhaarLength_ReturnsFailed()
        {
            var result = _service.VerifyDocuments("POL001", "Aadhaar", "12345");

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Message.Contains("Invalid document number format"));
            Assert.IsFalse(result.DocumentsVerified);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void VerifyDocuments_WithInvalidDocType_ReturnsFailed()
        {
            var result = _service.VerifyDocuments("POL001", "RationCard", "XYZ123");

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Message.Contains("Invalid document type"));
            Assert.IsFalse(result.DocumentsVerified);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void VerifyDocuments_WithNullPolicyNumber_ReturnsFailed()
        {
            var result = _service.VerifyDocuments(null, "PAN", "ABCDE1234F");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required for document verification", result.Message);
            Assert.IsFalse(result.DocumentsVerified);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void VerifyDocuments_WithNullDocType_ReturnsFailed()
        {
            var result = _service.VerifyDocuments("POL001", null, "ABCDE1234F");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Document type is required", result.Message);
            Assert.IsFalse(result.DocumentsVerified);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void IsPolicyMatured_WithMaturedPolicy_ReturnsTrue()
        {
            _service.RegisterPolicy("POL010", new DateTime(2017, 1, 1));
            bool result = _service.IsPolicyMatured("POL010", new DateTime(2017, 6, 1));
            Assert.IsTrue(result);
            Assert.IsNotNull(_service);
            Assert.AreNotEqual(0, 1);
            Assert.IsTrue(new DateTime(2017, 6, 1) > new DateTime(2017, 1, 1));
        }

        [TestMethod]
        public void IsPolicyMatured_WithFutureMaturity_ReturnsFalse()
        {
            _service.RegisterPolicy("POL011", new DateTime(2020, 1, 1));
            bool result = _service.IsPolicyMatured("POL011", new DateTime(2017, 6, 1));
            Assert.IsFalse(result);
            Assert.IsNotNull(_service);
            Assert.AreNotEqual(true, result);
            Assert.IsTrue(new DateTime(2020, 1, 1) > new DateTime(2017, 6, 1));
        }

        [TestMethod]
        public void IsPolicyMatured_WithExactMaturityDate_ReturnsTrue()
        {
            _service.RegisterPolicy("POL012", new DateTime(2017, 6, 15));
            bool result = _service.IsPolicyMatured("POL012", new DateTime(2017, 6, 15));
            Assert.IsTrue(result);
            Assert.IsNotNull(_service);
            Assert.AreNotEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
        }

        [TestMethod]
        public void IsPolicyMatured_WithUnknownPolicy_ReturnsFalse()
        {
            bool result = _service.IsPolicyMatured("UNKNOWN", new DateTime(2017, 6, 1));
            Assert.IsFalse(result);
            Assert.IsNotNull(_service);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
        }

        [TestMethod]
        public void IsKycComplete_WithVerifiedStatus_ReturnsTrue()
        {
            _service.SetKycStatus("CIF001", "Verified");
            bool result = _service.IsKycComplete("CIF001");
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsKycComplete_WithPendingStatus_ReturnsFalse()
        {
            _service.SetKycStatus("CIF002", "Pending");
            bool result = _service.IsKycComplete("CIF002");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsKycComplete_WithUnknownCif_ReturnsFalse()
        {
            bool result = _service.IsKycComplete("UNKNOWN");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void CheckPremiumStatus_WithAllPaid_ReturnsSuccess()
        {
            _service.SetPremiumStatus("POL020", true, 20, 20);
            var result = _service.CheckPremiumStatus("POL020");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("All premiums paid", result.Message);
            Assert.AreEqual("AllPaid", result.PremiumStatus);
            Assert.IsNotNull(result.Metadata);
        }

        [TestMethod]
        public void CheckPremiumStatus_WithPartialPaid_ReturnsPartial()
        {
            _service.SetPremiumStatus("POL021", false, 10, 20);
            var result = _service.CheckPremiumStatus("POL021");
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Message.Contains("Premiums outstanding"));
            Assert.AreEqual("Partial", result.PremiumStatus);
            Assert.IsNotNull(result.Metadata);
        }

        [TestMethod]
        public void CheckPremiumStatus_WithNullPolicy_ReturnsFailed()
        {
            var result = _service.CheckPremiumStatus(null);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy number is required", result.Message);
            Assert.AreEqual("Unknown", result.PremiumStatus);
            Assert.IsNull(result.ReferenceId);
        }

        [TestMethod]
        public void HasAllPremiumsPaid_WhenAllPaid_ReturnsTrue()
        {
            _service.SetPremiumStatus("POL030", true, 20, 20);
            bool result = _service.HasAllPremiumsPaid("POL030");
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void HasAllPremiumsPaid_WhenNotAllPaid_ReturnsFalse()
        {
            _service.SetPremiumStatus("POL031", false, 10, 20);
            bool result = _service.HasAllPremiumsPaid("POL031");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void HasOutstandingLoan_WithLoan_ReturnsTrue()
        {
            _service.SetLoanStatus("POL040", true, 25000m);
            bool result = _service.HasOutstandingLoan("POL040");
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void HasOutstandingLoan_WithoutLoan_ReturnsFalse()
        {
            _service.SetLoanStatus("POL041", false, 0m);
            bool result = _service.HasOutstandingLoan("POL041");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void GetOutstandingLoanAmount_WithLoan_ReturnsAmount()
        {
            _service.SetLoanStatus("POL050", true, 75000m);
            decimal amount = _service.GetOutstandingLoanAmount("POL050");
            Assert.AreEqual(75000m, amount);
            Assert.IsTrue(amount > 0);
            Assert.IsInstanceOfType(amount, typeof(decimal));
            Assert.AreNotEqual(0m, amount);
        }

        [TestMethod]
        public void GetOutstandingLoanAmount_WithoutLoan_ReturnsZero()
        {
            decimal amount = _service.GetOutstandingLoanAmount("UNKNOWN");
            Assert.AreEqual(0m, amount);
            Assert.IsFalse(amount > 0);
            Assert.IsInstanceOfType(amount, typeof(decimal));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void ValidateNomineeDetails_WithNominee_ReturnsSuccess()
        {
            _service.RegisterNominee("POL060", "Suresh Sharma");
            var result = _service.ValidateNomineeDetails("POL060");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Message.Contains("Suresh Sharma"));
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsNotNull(result.ProcessedDate);
        }

        [TestMethod]
        public void ValidateNomineeDetails_WithoutNominee_ReturnsFailed()
        {
            var result = _service.ValidateNomineeDetails("UNKNOWN");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("No nominee registered for policy", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateBankDetails_WithValidAccount_ReturnsSuccess()
        {
            var result = _service.ValidateBankDetails("CIF010", "1234567890123");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Bank details validated successfully", result.Message);
            Assert.IsNotNull(result.ReferenceId);
            Assert.IsNotNull(result.Metadata);
        }

        [TestMethod]
        public void ValidateBankDetails_WithShortAccount_ReturnsFailed()
        {
            var result = _service.ValidateBankDetails("CIF010", "12345");
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Message.Contains("Invalid account number format"));
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IsWithinClaimWindow_WithinThreeYears_ReturnsTrue()
        {
            _service.RegisterPolicy("POL070", new DateTime(2017, 1, 1));
            bool result = _service.IsWithinClaimWindow("POL070", new DateTime(2018, 6, 1));
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void IsWithinClaimWindow_AfterThreeYears_ReturnsFalse()
        {
            _service.RegisterPolicy("POL071", new DateTime(2017, 1, 1));
            bool result = _service.IsWithinClaimWindow("POL071", new DateTime(2021, 1, 1));
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void GetDaysToMaturity_WithFutureMaturity_ReturnsPositiveDays()
        {
            _service.RegisterPolicy("POL080", new DateTime(2018, 1, 1));
            int days = _service.GetDaysToMaturity("POL080", new DateTime(2017, 1, 1));
            Assert.AreEqual(365, days);
            Assert.IsTrue(days > 0);
            Assert.IsInstanceOfType(days, typeof(int));
            Assert.AreNotEqual(0, days);
        }

        [TestMethod]
        public void GetDaysToMaturity_WithPastMaturity_ReturnsZero()
        {
            _service.RegisterPolicy("POL081", new DateTime(2016, 1, 1));
            int days = _service.GetDaysToMaturity("POL081", new DateTime(2017, 1, 1));
            Assert.AreEqual(0, days);
            Assert.IsFalse(days > 0);
            Assert.IsInstanceOfType(days, typeof(int));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void ValidateClaimantIdentity_WithMatchingDetails_ReturnsTrue()
        {
            _service.RegisterIdentity("CIF020", "ABCDE1234F", new DateTime(1985, 5, 15));
            bool result = _service.ValidateClaimantIdentity("CIF020", "ABCDE1234F", new DateTime(1985, 5, 15));
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void ValidateClaimantIdentity_WithWrongPan_ReturnsFalse()
        {
            _service.RegisterIdentity("CIF021", "ABCDE1234F", new DateTime(1985, 5, 15));
            bool result = _service.ValidateClaimantIdentity("CIF021", "WRONG12345", new DateTime(1985, 5, 15));
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void ValidateClaimantIdentity_WithWrongDob_ReturnsFalse()
        {
            _service.RegisterIdentity("CIF022", "ABCDE1234F", new DateTime(1985, 5, 15));
            bool result = _service.ValidateClaimantIdentity("CIF022", "ABCDE1234F", new DateTime(1990, 1, 1));
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsNotNull(_service);
        }

        [TestMethod]
        public void GetValidationSummary_WithAllPassingPolicy_ReturnsAllPassed()
        {
            _service.RegisterPolicy("POL090", new DateTime(2020, 1, 1));
            _service.SetPremiumStatus("POL090", true, 20, 20);
            _service.SetLoanStatus("POL090", false, 0m);
            _service.RegisterNominee("POL090", "Test Nominee");
            var result = _service.GetValidationSummary("POL090");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("All validations passed", result.Message);
            Assert.AreEqual("AllPassed", result.ValidationStatus);
            Assert.IsNotNull(result.Metadata);
        }

        [TestMethod]
        public void GetValidationSummary_WithFailingPolicy_ReturnsHasIssues()
        {
            _service.RegisterPolicy("POL091", new DateTime(2020, 1, 1));
            _service.SetPremiumStatus("POL091", false, 10, 20);
            var result = _service.GetValidationSummary("POL091");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("One or more validations failed", result.Message);
            Assert.AreEqual("HasIssues", result.ValidationStatus);
            Assert.IsNotNull(result.Metadata);
        }

        [TestMethod]
        public void GetValidationHistory_WithEmptyHistory_ReturnsEmptyList()
        {
            var result = _service.GetValidationHistory("POL100", new DateTime(2017, 1, 1), new DateTime(2017, 12, 31));
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsInstanceOfType(result, typeof(List<PolicyMaturityValidationResult>));
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetValidationHistory_WithNullPolicy_ReturnsEmptyList()
        {
            var result = _service.GetValidationHistory(null, new DateTime(2017, 1, 1), new DateTime(2017, 12, 31));
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsInstanceOfType(result, typeof(List<PolicyMaturityValidationResult>));
            Assert.IsFalse(result.Any());
        }
    }
}
