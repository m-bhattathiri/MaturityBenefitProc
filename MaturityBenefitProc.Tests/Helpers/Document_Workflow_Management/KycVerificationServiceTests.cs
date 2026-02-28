using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement;

namespace MaturityBenefitProc.Tests.Helpers.DocumentAndWorkflowManagement
{
    [TestClass]
    public class KycVerificationServiceTests
    {
        private IKycVerificationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation exists for testing
            _service = new KycVerificationService();
        }

        [TestMethod]
        public void VerifyPanFormat_ValidAndInvalidInputs_ReturnsExpected()
        {
            Assert.IsTrue(_service.VerifyPanFormat("ABCDE1234F"));
            Assert.IsFalse(_service.VerifyPanFormat("ABCDE1234"));
            Assert.IsFalse(_service.VerifyPanFormat("12345ABCDE"));
            Assert.IsFalse(_service.VerifyPanFormat(""));
            Assert.IsFalse(_service.VerifyPanFormat(null));
        }

        [TestMethod]
        public void VerifyAadharChecksum_ValidAndInvalidInputs_ReturnsExpected()
        {
            Assert.IsTrue(_service.VerifyAadharChecksum("123456789012"));
            Assert.IsFalse(_service.VerifyAadharChecksum("12345678901"));
            Assert.IsFalse(_service.VerifyAadharChecksum("ABCDEFGH1234"));
            Assert.IsFalse(_service.VerifyAadharChecksum(""));
            Assert.IsFalse(_service.VerifyAadharChecksum(null));
        }

        [TestMethod]
        public void IsKycCompliant_VariousCustomerIds_ReturnsExpected()
        {
            Assert.IsTrue(_service.IsKycCompliant("CUST-1001"));
            Assert.IsFalse(_service.IsKycCompliant("CUST-9999"));
            Assert.IsFalse(_service.IsKycCompliant(""));
            Assert.IsFalse(_service.IsKycCompliant(null));
            Assert.IsFalse(_service.IsKycCompliant("   "));
        }

        [TestMethod]
        public void CheckNameMatch_VariousNames_ReturnsExpected()
        {
            Assert.IsTrue(_service.CheckNameMatch("John Doe", "John Doe"));
            Assert.IsTrue(_service.CheckNameMatch("JOHN DOE", "John Doe"));
            Assert.IsFalse(_service.CheckNameMatch("John Doe", "Jane Doe"));
            Assert.IsFalse(_service.CheckNameMatch("", "John Doe"));
            Assert.IsFalse(_service.CheckNameMatch(null, null));
        }

        [TestMethod]
        public void IsAddressProofValid_VariousDates_ReturnsExpected()
        {
            Assert.IsTrue(_service.IsAddressProofValid("DOC-123", DateTime.Now.AddMonths(-1)));
            Assert.IsFalse(_service.IsAddressProofValid("DOC-123", DateTime.Now.AddYears(-2)));
            Assert.IsFalse(_service.IsAddressProofValid("", DateTime.Now));
            Assert.IsFalse(_service.IsAddressProofValid(null, DateTime.Now));
            Assert.IsTrue(_service.IsAddressProofValid("DOC-999", DateTime.Now.AddDays(-10)));
        }

        [TestMethod]
        public void ValidateSignature_VariousHashes_ReturnsExpected()
        {
            Assert.IsTrue(_service.ValidateSignature("CUST-1", "hash123"));
            Assert.IsFalse(_service.ValidateSignature("CUST-1", "wronghash"));
            Assert.IsFalse(_service.ValidateSignature("", "hash123"));
            Assert.IsFalse(_service.ValidateSignature(null, null));
            Assert.IsFalse(_service.ValidateSignature("CUST-2", ""));
        }

        [TestMethod]
        public void IsCustomerMinor_VariousDatesOfBirth_ReturnsExpected()
        {
            Assert.IsTrue(_service.IsCustomerMinor("CUST-1", DateTime.Now.AddYears(-10)));
            Assert.IsFalse(_service.IsCustomerMinor("CUST-2", DateTime.Now.AddYears(-25)));
            Assert.IsTrue(_service.IsCustomerMinor("CUST-3", DateTime.Now.AddYears(-17)));
            Assert.IsFalse(_service.IsCustomerMinor("CUST-4", DateTime.Now.AddYears(-18)));
            Assert.IsFalse(_service.IsCustomerMinor(null, DateTime.Now.AddYears(-30)));
        }

        [TestMethod]
        public void CheckPepStatus_VariousCustomerIds_ReturnsExpected()
        {
            Assert.IsTrue(_service.CheckPepStatus("PEP-123"));
            Assert.IsFalse(_service.CheckPepStatus("CUST-123"));
            Assert.IsFalse(_service.CheckPepStatus(""));
            Assert.IsFalse(_service.CheckPepStatus(null));
            Assert.IsFalse(_service.CheckPepStatus("   "));
        }

        [TestMethod]
        public void IsFatcaCompliant_VariousCustomerIds_ReturnsExpected()
        {
            Assert.IsTrue(_service.IsFatcaCompliant("FATCA-1"));
            Assert.IsFalse(_service.IsFatcaCompliant("NON-FATCA-1"));
            Assert.IsFalse(_service.IsFatcaCompliant(""));
            Assert.IsFalse(_service.IsFatcaCompliant(null));
            Assert.IsTrue(_service.IsFatcaCompliant("FATCA-2"));
        }

        [TestMethod]
        public void VerifyBankDetails_VariousAccounts_ReturnsExpected()
        {
            Assert.IsTrue(_service.VerifyBankDetails("1234567890", "SBIN0001234"));
            Assert.IsFalse(_service.VerifyBankDetails("123", "SBIN0001234"));
            Assert.IsFalse(_service.VerifyBankDetails("1234567890", "INVALID"));
            Assert.IsFalse(_service.VerifyBankDetails("", ""));
            Assert.IsFalse(_service.VerifyBankDetails(null, null));
        }

        [TestMethod]
        public void RequiresEnhancedDueDiligence_VariousAmounts_ReturnsExpected()
        {
            Assert.IsTrue(_service.RequiresEnhancedDueDiligence("CUST-1", 2000000m));
            Assert.IsFalse(_service.RequiresEnhancedDueDiligence("CUST-2", 50000m));
            Assert.IsTrue(_service.RequiresEnhancedDueDiligence("PEP-1", 1000m));
            Assert.IsFalse(_service.RequiresEnhancedDueDiligence("", 0m));
            Assert.IsFalse(_service.RequiresEnhancedDueDiligence(null, -100m));
        }

        [TestMethod]
        public void CalculateTotalMaturityAmount_VariousInputs_ReturnsExpected()
        {
            Assert.AreEqual(11000m, _service.CalculateTotalMaturityAmount("POL-1", 10000m));
            Assert.AreEqual(0m, _service.CalculateTotalMaturityAmount("POL-2", 0m));
            Assert.AreEqual(0m, _service.CalculateTotalMaturityAmount("", 5000m));
            Assert.AreEqual(0m, _service.CalculateTotalMaturityAmount(null, 1000m));
            Assert.AreEqual(5500m, _service.CalculateTotalMaturityAmount("POL-3", 5000m));
        }

        [TestMethod]
        public void GetTdsDeductionAmount_VariousInputs_ReturnsExpected()
        {
            Assert.AreEqual(1000m, _service.GetTdsDeductionAmount("ABCDE1234F", 10000m));
            Assert.AreEqual(2000m, _service.GetTdsDeductionAmount("INVALID", 10000m));
            Assert.AreEqual(0m, _service.GetTdsDeductionAmount("ABCDE1234F", 0m));
            Assert.AreEqual(0m, _service.GetTdsDeductionAmount("", 10000m));
            Assert.AreEqual(0m, _service.GetTdsDeductionAmount(null, 5000m));
        }

        [TestMethod]
        public void CalculatePenalInterest_VariousDays_ReturnsExpected()
        {
            Assert.AreEqual(100m, _service.CalculatePenalInterest("POL-1", 10));
            Assert.AreEqual(0m, _service.CalculatePenalInterest("POL-2", 0));
            Assert.AreEqual(0m, _service.CalculatePenalInterest("POL-3", -5));
            Assert.AreEqual(0m, _service.CalculatePenalInterest("", 10));
            Assert.AreEqual(0m, _service.CalculatePenalInterest(null, 20));
        }

        [TestMethod]
        public void GetSurrenderValue_VariousDates_ReturnsExpected()
        {
            Assert.AreEqual(5000m, _service.GetSurrenderValue("POL-1", DateTime.Now));
            Assert.AreEqual(0m, _service.GetSurrenderValue("INVALID", DateTime.Now));
            Assert.AreEqual(0m, _service.GetSurrenderValue("", DateTime.Now));
            Assert.AreEqual(0m, _service.GetSurrenderValue(null, DateTime.Now));
            Assert.AreEqual(5000m, _service.GetSurrenderValue("POL-2", DateTime.Now.AddDays(-1)));
        }

        [TestMethod]
        public void CalculateBonusAccrued_VariousPolicies_ReturnsExpected()
        {
            Assert.AreEqual(1500m, _service.CalculateBonusAccrued("POL-1"));
            Assert.AreEqual(0m, _service.CalculateBonusAccrued("INVALID"));
            Assert.AreEqual(0m, _service.CalculateBonusAccrued(""));
            Assert.AreEqual(0m, _service.CalculateBonusAccrued(null));
            Assert.AreEqual(1500m, _service.CalculateBonusAccrued("POL-2"));
        }

        [TestMethod]
        public void GetMaximumAllowableCashPayout_VariousCustomers_ReturnsExpected()
        {
            Assert.AreEqual(10000m, _service.GetMaximumAllowableCashPayout("CUST-1"));
            Assert.AreEqual(0m, _service.GetMaximumAllowableCashPayout("INVALID"));
            Assert.AreEqual(0m, _service.GetMaximumAllowableCashPayout(""));
            Assert.AreEqual(0m, _service.GetMaximumAllowableCashPayout(null));
            Assert.AreEqual(10000m, _service.GetMaximumAllowableCashPayout("CUST-2"));
        }

        [TestMethod]
        public void ComputeNetSettlementAmount_VariousAmounts_ReturnsExpected()
        {
            Assert.AreEqual(9000m, _service.ComputeNetSettlementAmount(10000m, 1000m));
            Assert.AreEqual(0m, _service.ComputeNetSettlementAmount(1000m, 1000m));
            Assert.AreEqual(0m, _service.ComputeNetSettlementAmount(500m, 1000m));
            Assert.AreEqual(5000m, _service.ComputeNetSettlementAmount(5000m, 0m));
            Assert.AreEqual(0m, _service.ComputeNetSettlementAmount(0m, 0m));
        }

        [TestMethod]
        public void GetTdsRate_VariousPans_ReturnsExpected()
        {
            Assert.AreEqual(0.10, _service.GetTdsRate("ABCDE1234F", true));
            Assert.AreEqual(0.20, _service.GetTdsRate("ABCDE1234F", false));
            Assert.AreEqual(0.20, _service.GetTdsRate("", false));
            Assert.AreEqual(0.20, _service.GetTdsRate(null, false));
            Assert.AreEqual(0.10, _service.GetTdsRate("XYZDE1234F", true));
        }

        [TestMethod]
        public void CalculateNameMatchConfidence_VariousNames_ReturnsExpected()
        {
            Assert.AreEqual(1.0, _service.CalculateNameMatchConfidence("John Doe", "John Doe"));
            Assert.AreEqual(0.0, _service.CalculateNameMatchConfidence("John Doe", "Jane Doe"));
            Assert.AreEqual(0.0, _service.CalculateNameMatchConfidence("", "John Doe"));
            Assert.AreEqual(0.0, _service.CalculateNameMatchConfidence(null, null));
            Assert.AreEqual(1.0, _service.CalculateNameMatchConfidence("JOHN DOE", "John Doe"));
        }

        [TestMethod]
        public void GetRiskScore_VariousCustomers_ReturnsExpected()
        {
            Assert.AreEqual(0.5, _service.GetRiskScore("CUST-1"));
            Assert.AreEqual(0.9, _service.GetRiskScore("PEP-1"));
            Assert.AreEqual(0.0, _service.GetRiskScore(""));
            Assert.AreEqual(0.0, _service.GetRiskScore(null));
            Assert.AreEqual(0.5, _service.GetRiskScore("CUST-2"));
        }

        [TestMethod]
        public void GetFaceMatchPercentage_VariousPhotos_ReturnsExpected()
        {
            Assert.AreEqual(95.5, _service.GetFaceMatchPercentage("PHOTO-1", "PHOTO-2"));
            Assert.AreEqual(0.0, _service.GetFaceMatchPercentage("PHOTO-1", "INVALID"));
            Assert.AreEqual(0.0, _service.GetFaceMatchPercentage("", "PHOTO-2"));
            Assert.AreEqual(0.0, _service.GetFaceMatchPercentage(null, null));
            Assert.AreEqual(95.5, _service.GetFaceMatchPercentage("PHOTO-3", "PHOTO-4"));
        }

        [TestMethod]
        public void GetDocumentClarityScore_VariousDocs_ReturnsExpected()
        {
            Assert.AreEqual(8.5, _service.GetDocumentClarityScore("DOC-1"));
            Assert.AreEqual(0.0, _service.GetDocumentClarityScore("INVALID"));
            Assert.AreEqual(0.0, _service.GetDocumentClarityScore(""));
            Assert.AreEqual(0.0, _service.GetDocumentClarityScore(null));
            Assert.AreEqual(8.5, _service.GetDocumentClarityScore("DOC-2"));
        }

        [TestMethod]
        public void GetCkycMatchProbability_VariousInputs_ReturnsExpected()
        {
            Assert.AreEqual(0.85, _service.GetCkycMatchProbability("CUST-1", "CKYC-1"));
            Assert.AreEqual(0.0, _service.GetCkycMatchProbability("CUST-1", "INVALID"));
            Assert.AreEqual(0.0, _service.GetCkycMatchProbability("", "CKYC-1"));
            Assert.AreEqual(0.0, _service.GetCkycMatchProbability(null, null));
            Assert.AreEqual(0.85, _service.GetCkycMatchProbability("CUST-2", "CKYC-2"));
        }

        [TestMethod]
        public void GetPendingKycDocumentCount_VariousCustomers_ReturnsExpected()
        {
            Assert.AreEqual(2, _service.GetPendingKycDocumentCount("CUST-1"));
            Assert.AreEqual(0, _service.GetPendingKycDocumentCount("COMPLIANT-1"));
            Assert.AreEqual(0, _service.GetPendingKycDocumentCount(""));
            Assert.AreEqual(0, _service.GetPendingKycDocumentCount(null));
            Assert.AreEqual(2, _service.GetPendingKycDocumentCount("CUST-2"));
        }

        [TestMethod]
        public void GetDaysUntilMaturity_VariousDates_ReturnsExpected()
        {
            Assert.AreEqual(30, _service.GetDaysUntilMaturity("POL-1", DateTime.Now.AddDays(-30)));
            Assert.AreEqual(0, _service.GetDaysUntilMaturity("INVALID", DateTime.Now));
            Assert.AreEqual(0, _service.GetDaysUntilMaturity("", DateTime.Now));
            Assert.AreEqual(0, _service.GetDaysUntilMaturity(null, DateTime.Now));
            Assert.AreEqual(10, _service.GetDaysUntilMaturity("POL-2", DateTime.Now.AddDays(-10)));
        }

        [TestMethod]
        public void GetAadharLinkageStatusCode_VariousCustomers_ReturnsExpected()
        {
            Assert.AreEqual(1, _service.GetAadharLinkageStatusCode("CUST-1"));
            Assert.AreEqual(0, _service.GetAadharLinkageStatusCode("UNLINKED-1"));
            Assert.AreEqual(-1, _service.GetAadharLinkageStatusCode(""));
            Assert.AreEqual(-1, _service.GetAadharLinkageStatusCode(null));
            Assert.AreEqual(1, _service.GetAadharLinkageStatusCode("CUST-2"));
        }

        [TestMethod]
        public void CountFailedVerificationAttempts_VariousCustomers_ReturnsExpected()
        {
            Assert.AreEqual(3, _service.CountFailedVerificationAttempts("CUST-1"));
            Assert.AreEqual(0, _service.CountFailedVerificationAttempts("NEW-1"));
            Assert.AreEqual(0, _service.CountFailedVerificationAttempts(""));
            Assert.AreEqual(0, _service.CountFailedVerificationAttempts(null));
            Assert.AreEqual(3, _service.CountFailedVerificationAttempts("CUST-2"));
        }

        [TestMethod]
        public void GetPolicyDurationInMonths_VariousPolicies_ReturnsExpected()
        {
            Assert.AreEqual(120, _service.GetPolicyDurationInMonths("POL-1"));
            Assert.AreEqual(0, _service.GetPolicyDurationInMonths("INVALID"));
            Assert.AreEqual(0, _service.GetPolicyDurationInMonths(""));
            Assert.AreEqual(0, _service.GetPolicyDurationInMonths(null));
            Assert.AreEqual(120, _service.GetPolicyDurationInMonths("POL-2"));
        }

        [TestMethod]
        public void GetAgeAtMaturity_VariousDates_ReturnsExpected()
        {
            Assert.AreEqual(60, _service.GetAgeAtMaturity("CUST-1", DateTime.Now.AddYears(30)));
            Assert.AreEqual(0, _service.GetAgeAtMaturity("INVALID", DateTime.Now));
            Assert.AreEqual(0, _service.GetAgeAtMaturity("", DateTime.Now));
            Assert.AreEqual(0, _service.GetAgeAtMaturity(null, DateTime.Now));
            Assert.AreEqual(60, _service.GetAgeAtMaturity("CUST-2", DateTime.Now.AddYears(30)));
        }

        [TestMethod]
        public void GetDocumentExpiryDays_VariousDates_ReturnsExpected()
        {
            Assert.AreEqual(90, _service.GetDocumentExpiryDays("DOC-1", DateTime.Now.AddDays(-90)));
            Assert.AreEqual(0, _service.GetDocumentExpiryDays("INVALID", DateTime.Now));
            Assert.AreEqual(0, _service.GetDocumentExpiryDays("", DateTime.Now));
            Assert.AreEqual(0, _service.GetDocumentExpiryDays(null, DateTime.Now));
            Assert.AreEqual(30, _service.GetDocumentExpiryDays("DOC-2", DateTime.Now.AddDays(-30)));
        }

        [TestMethod]
        public void GenerateKycReferenceNumber_VariousCustomers_ReturnsExpected()
        {
            Assert.IsNotNull(_service.GenerateKycReferenceNumber("CUST-1"));
            Assert.IsTrue(_service.GenerateKycReferenceNumber("CUST-1").StartsWith("KYC-"));
            Assert.IsNull(_service.GenerateKycReferenceNumber(""));
            Assert.IsNull(_service.GenerateKycReferenceNumber(null));
            Assert.IsNotNull(_service.GenerateKycReferenceNumber("CUST-2"));
        }

        [TestMethod]
        public void GetCkycNumber_VariousPans_ReturnsExpected()
        {
            Assert.AreEqual("CKYC-12345", _service.GetCkycNumber("ABCDE1234F"));
            Assert.IsNull(_service.GetCkycNumber("INVALID"));
            Assert.IsNull(_service.GetCkycNumber(""));
            Assert.IsNull(_service.GetCkycNumber(null));
            Assert.AreEqual("CKYC-12345", _service.GetCkycNumber("XYZDE1234F"));
        }

        [TestMethod]
        public void RetrieveAadharVaultReference_VariousAadhars_ReturnsExpected()
        {
            Assert.AreEqual("VAULT-9876", _service.RetrieveAadharVaultReference("123456789012"));
            Assert.IsNull(_service.RetrieveAadharVaultReference("INVALID"));
            Assert.IsNull(_service.RetrieveAadharVaultReference(""));
            Assert.IsNull(_service.RetrieveAadharVaultReference(null));
            Assert.AreEqual("VAULT-9876", _service.RetrieveAadharVaultReference("987654321098"));
        }

        [TestMethod]
        public void GetVerificationStatusDescription_VariousCodes_ReturnsExpected()
        {
            Assert.AreEqual("Verified", _service.GetVerificationStatusDescription(1));
            Assert.AreEqual("Pending", _service.GetVerificationStatusDescription(0));
            Assert.AreEqual("Rejected", _service.GetVerificationStatusDescription(-1));
            Assert.AreEqual("Unknown", _service.GetVerificationStatusDescription(99));
            Assert.AreEqual("Unknown", _service.GetVerificationStatusDescription(-99));
        }

        [TestMethod]
        public void FetchPrimaryBankIfsc_VariousCustomers_ReturnsExpected()
        {
            Assert.AreEqual("SBIN0001234", _service.FetchPrimaryBankIfsc("CUST-1"));
            Assert.IsNull(_service.FetchPrimaryBankIfsc("INVALID"));
            Assert.IsNull(_service.FetchPrimaryBankIfsc(""));
            Assert.IsNull(_service.FetchPrimaryBankIfsc(null));
            Assert.AreEqual("SBIN0001234", _service.FetchPrimaryBankIfsc("CUST-2"));
        }

        [TestMethod]
        public void GetRejectionReasonCode_VariousDocs_ReturnsExpected()
        {
            Assert.AreEqual("REJ-01", _service.GetRejectionReasonCode("DOC-1"));
            Assert.IsNull(_service.GetRejectionReasonCode("VALID-DOC"));
            Assert.IsNull(_service.GetRejectionReasonCode(""));
            Assert.IsNull(_service.GetRejectionReasonCode(null));
            Assert.AreEqual("REJ-01", _service.GetRejectionReasonCode("DOC-2"));
        }

        [TestMethod]
        public void GenerateSettlementTransactionId_VariousInputs_ReturnsExpected()
        {
            Assert.IsNotNull(_service.GenerateSettlementTransactionId("POL-1", 10000m));
            Assert.IsTrue(_service.GenerateSettlementTransactionId("POL-1", 10000m).StartsWith("TXN-"));
            Assert.IsNull(_service.GenerateSettlementTransactionId("", 10000m));
            Assert.IsNull(_service.GenerateSettlementTransactionId(null, 10000m));
            Assert.IsNull(_service.GenerateSettlementTransactionId("POL-1", 0m));
        }

        [TestMethod]
        public void GetTaxResidencyCountry_VariousCustomers_ReturnsExpected()
        {
            Assert.AreEqual("IN", _service.GetTaxResidencyCountry("CUST-1"));
            Assert.AreEqual("US", _service.GetTaxResidencyCountry("FATCA-1"));
            Assert.IsNull(_service.GetTaxResidencyCountry(""));
            Assert.IsNull(_service.GetTaxResidencyCountry(null));
            Assert.AreEqual("IN", _service.GetTaxResidencyCountry("CUST-2"));
        }

        [TestMethod]
        public void FetchDigiLockerDocumentUri_VariousInputs_ReturnsExpected()
        {
            Assert.AreEqual("uri://digilocker/PAN/123", _service.FetchDigiLockerDocumentUri("PAN", "123"));
            Assert.IsNull(_service.FetchDigiLockerDocumentUri("", "123"));
            Assert.IsNull(_service.FetchDigiLockerDocumentUri("PAN", ""));
            Assert.IsNull(_service.FetchDigiLockerDocumentUri(null, null));
            Assert.AreEqual("uri://digilocker/AADHAR/456", _service.FetchDigiLockerDocumentUri("AADHAR", "456"));
        }
    }
}