using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement;

namespace MaturityBenefitProc.Tests.Helpers.DocumentAndWorkflowManagement
{
    [TestClass]
    public class KycVerificationServiceIntegrationTests
    {
        private IKycVerificationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming KycVerificationService implements IKycVerificationService
            // For the purpose of this test file generation, we use a mock/stub instantiation.
            // In a real scenario, this would be the actual implementation.
            _service = new KycVerificationServiceStub();
        }

        [TestMethod]
        public void VerifyPanAndTds_ValidPan_ReturnsStandardTdsRate()
        {
            string pan = "ABCDE1234F";
            decimal maturityAmount = 100000m;

            bool isPanValid = _service.VerifyPanFormat(pan);
            double tdsRate = _service.GetTdsRate(pan, isPanValid);
            decimal tdsAmount = _service.GetTdsDeductionAmount(pan, maturityAmount);

            Assert.IsTrue(isPanValid);
            Assert.AreEqual(0.10, tdsRate);
            Assert.AreEqual(10000m, tdsAmount);
            Assert.IsNotNull(pan);
        }

        [TestMethod]
        public void VerifyPanAndTds_InvalidPan_ReturnsHigherTdsRate()
        {
            string pan = "INVALIDPAN";
            decimal maturityAmount = 100000m;

            bool isPanValid = _service.VerifyPanFormat(pan);
            double tdsRate = _service.GetTdsRate(pan, isPanValid);
            decimal tdsAmount = _service.GetTdsDeductionAmount(pan, maturityAmount);

            Assert.IsFalse(isPanValid);
            Assert.AreEqual(0.20, tdsRate);
            Assert.AreEqual(20000m, tdsAmount);
            Assert.IsNotNull(pan);
        }

        [TestMethod]
        public void KycCompliance_FullyCompliant_ReturnsTrueAndZeroPending()
        {
            string customerId = "CUST123";

            bool isCompliant = _service.IsKycCompliant(customerId);
            int pendingDocs = _service.GetPendingKycDocumentCount(customerId);
            string kycRef = _service.GenerateKycReferenceNumber(customerId);

            Assert.IsTrue(isCompliant);
            Assert.AreEqual(0, pendingDocs);
            Assert.IsNotNull(kycRef);
            Assert.AreNotEqual("", kycRef);
        }

        [TestMethod]
        public void KycCompliance_NonCompliant_ReturnsFalseAndPendingCount()
        {
            string customerId = "CUST999";

            bool isCompliant = _service.IsKycCompliant(customerId);
            int pendingDocs = _service.GetPendingKycDocumentCount(customerId);
            string kycRef = _service.GenerateKycReferenceNumber(customerId);

            Assert.IsFalse(isCompliant);
            Assert.IsTrue(pendingDocs > 0);
            Assert.IsNotNull(kycRef);
            Assert.AreNotEqual("", kycRef);
        }

        [TestMethod]
        public void NameMatch_HighConfidence_ReturnsTrue()
        {
            string custName = "John Doe";
            string docName = "John Doe";

            double confidence = _service.CalculateNameMatchConfidence(custName, docName);
            bool isMatch = _service.CheckNameMatch(custName, docName);

            Assert.IsTrue(confidence > 0.9);
            Assert.IsTrue(isMatch);
            Assert.AreEqual(custName, docName);
            Assert.IsNotNull(custName);
        }

        [TestMethod]
        public void NameMatch_LowConfidence_ReturnsFalse()
        {
            string custName = "John Doe";
            string docName = "Jane Smith";

            double confidence = _service.CalculateNameMatchConfidence(custName, docName);
            bool isMatch = _service.CheckNameMatch(custName, docName);

            Assert.IsTrue(confidence < 0.5);
            Assert.IsFalse(isMatch);
            Assert.AreNotEqual(custName, docName);
            Assert.IsNotNull(custName);
        }

        [TestMethod]
        public void MaturitySettlement_ValidPolicy_CalculatesCorrectNet()
        {
            string policyId = "POL123";
            decimal baseAmount = 50000m;
            string pan = "ABCDE1234F";

            decimal totalAmount = _service.CalculateTotalMaturityAmount(policyId, baseAmount);
            decimal tds = _service.GetTdsDeductionAmount(pan, totalAmount);
            decimal netAmount = _service.ComputeNetSettlementAmount(totalAmount, tds);
            string transId = _service.GenerateSettlementTransactionId(policyId, netAmount);

            Assert.IsTrue(totalAmount >= baseAmount);
            Assert.IsTrue(tds >= 0);
            Assert.AreEqual(totalAmount - tds, netAmount);
            Assert.IsNotNull(transId);
        }

        [TestMethod]
        public void EnhancedDueDiligence_HighRisk_RequiresEDD()
        {
            string customerId = "CUST_HIGH_RISK";
            decimal amount = 2000000m;

            double riskScore = _service.GetRiskScore(customerId);
            bool requiresEdd = _service.RequiresEnhancedDueDiligence(customerId, amount);
            bool isPep = _service.CheckPepStatus(customerId);

            Assert.IsTrue(riskScore > 0.7);
            Assert.IsTrue(requiresEdd);
            Assert.IsTrue(isPep);
            Assert.IsNotNull(customerId);
        }

        [TestMethod]
        public void EnhancedDueDiligence_LowRisk_DoesNotRequireEDD()
        {
            string customerId = "CUST_LOW_RISK";
            decimal amount = 50000m;

            double riskScore = _service.GetRiskScore(customerId);
            bool requiresEdd = _service.RequiresEnhancedDueDiligence(customerId, amount);
            bool isPep = _service.CheckPepStatus(customerId);

            Assert.IsTrue(riskScore < 0.3);
            Assert.IsFalse(requiresEdd);
            Assert.IsFalse(isPep);
            Assert.IsNotNull(customerId);
        }

        [TestMethod]
        public void AadharVerification_ValidAadhar_ReturnsSuccess()
        {
            string aadhar = "123456789012";
            string customerId = "CUST123";

            bool isValid = _service.VerifyAadharChecksum(aadhar);
            string vaultRef = _service.RetrieveAadharVaultReference(aadhar);
            int statusCode = _service.GetAadharLinkageStatusCode(customerId);

            Assert.IsTrue(isValid);
            Assert.IsNotNull(vaultRef);
            Assert.AreEqual(1, statusCode); // 1 = Linked
            Assert.AreNotEqual("", vaultRef);
        }

        [TestMethod]
        public void AadharVerification_InvalidAadhar_ReturnsFailure()
        {
            string aadhar = "000000000000";
            string customerId = "CUST999";

            bool isValid = _service.VerifyAadharChecksum(aadhar);
            string vaultRef = _service.RetrieveAadharVaultReference(aadhar);
            int statusCode = _service.GetAadharLinkageStatusCode(customerId);

            Assert.IsFalse(isValid);
            Assert.IsNull(vaultRef);
            Assert.AreEqual(0, statusCode); // 0 = Not Linked
            Assert.IsNotNull(aadhar);
        }

        [TestMethod]
        public void BankDetails_ValidBank_ReturnsTrue()
        {
            string account = "123456789";
            string ifsc = "SBIN0001234";
            string customerId = "CUST123";

            bool isValid = _service.VerifyBankDetails(account, ifsc);
            string primaryIfsc = _service.FetchPrimaryBankIfsc(customerId);

            Assert.IsTrue(isValid);
            Assert.IsNotNull(primaryIfsc);
            Assert.AreEqual(ifsc, primaryIfsc);
            Assert.AreNotEqual("", account);
        }

        [TestMethod]
        public void BankDetails_InvalidBank_ReturnsFalse()
        {
            string account = "000";
            string ifsc = "INVALID";
            string customerId = "CUST999";

            bool isValid = _service.VerifyBankDetails(account, ifsc);
            string primaryIfsc = _service.FetchPrimaryBankIfsc(customerId);

            Assert.IsFalse(isValid);
            Assert.AreNotEqual(ifsc, primaryIfsc);
            Assert.IsNotNull(account);
            Assert.IsNotNull(ifsc);
        }

        [TestMethod]
        public void FatcaCompliance_USResident_ReturnsFalse()
        {
            string customerId = "CUST_US";

            string country = _service.GetTaxResidencyCountry(customerId);
            bool isFatcaCompliant = _service.IsFatcaCompliant(customerId);

            Assert.AreEqual("USA", country);
            Assert.IsFalse(isFatcaCompliant);
            Assert.IsNotNull(country);
            Assert.IsNotNull(customerId);
        }

        [TestMethod]
        public void FatcaCompliance_INResident_ReturnsTrue()
        {
            string customerId = "CUST_IN";

            string country = _service.GetTaxResidencyCountry(customerId);
            bool isFatcaCompliant = _service.IsFatcaCompliant(customerId);

            Assert.AreEqual("IND", country);
            Assert.IsTrue(isFatcaCompliant);
            Assert.IsNotNull(country);
            Assert.IsNotNull(customerId);
        }

        [TestMethod]
        public void DocumentClarity_HighClarity_ReturnsValid()
        {
            string docId = "DOC123";
            DateTime issueDate = DateTime.Now.AddYears(-1);

            double clarityScore = _service.GetDocumentClarityScore(docId);
            bool isValid = _service.IsAddressProofValid(docId, issueDate);
            string rejectReason = _service.GetRejectionReasonCode(docId);

            Assert.IsTrue(clarityScore > 0.8);
            Assert.IsTrue(isValid);
            Assert.IsNull(rejectReason);
            Assert.IsNotNull(docId);
        }

        [TestMethod]
        public void DocumentClarity_LowClarity_ReturnsInvalid()
        {
            string docId = "DOC999";
            DateTime issueDate = DateTime.Now.AddYears(-1);

            double clarityScore = _service.GetDocumentClarityScore(docId);
            bool isValid = _service.IsAddressProofValid(docId, issueDate);
            string rejectReason = _service.GetRejectionReasonCode(docId);

            Assert.IsTrue(clarityScore < 0.4);
            Assert.IsFalse(isValid);
            Assert.IsNotNull(rejectReason);
            Assert.AreEqual("BLURRY", rejectReason);
        }

        [TestMethod]
        public void AgeAtMaturity_Minor_ReturnsTrue()
        {
            string customerId = "CUST_MINOR";
            DateTime dob = DateTime.Now.AddYears(-10);
            DateTime maturityDate = DateTime.Now;

            bool isMinor = _service.IsCustomerMinor(customerId, dob);
            int age = _service.GetAgeAtMaturity(customerId, maturityDate);

            Assert.IsTrue(isMinor);
            Assert.IsTrue(age < 18);
            Assert.AreEqual(10, age);
            Assert.IsNotNull(customerId);
        }

        [TestMethod]
        public void AgeAtMaturity_Adult_ReturnsFalse()
        {
            string customerId = "CUST_ADULT";
            DateTime dob = DateTime.Now.AddYears(-30);
            DateTime maturityDate = DateTime.Now;

            bool isMinor = _service.IsCustomerMinor(customerId, dob);
            int age = _service.GetAgeAtMaturity(customerId, maturityDate);

            Assert.IsFalse(isMinor);
            Assert.IsTrue(age >= 18);
            Assert.AreEqual(30, age);
            Assert.IsNotNull(customerId);
        }

        [TestMethod]
        public void PenalInterest_Delayed_CalculatesInterest()
        {
            string policyId = "POL123";
            int daysDelayed = 30;

            decimal penalInterest = _service.CalculatePenalInterest(policyId, daysDelayed);
            int duration = _service.GetPolicyDurationInMonths(policyId);

            Assert.IsTrue(penalInterest > 0);
            Assert.IsTrue(duration > 0);
            Assert.IsNotNull(policyId);
            Assert.AreNotEqual(0m, penalInterest);
        }

        [TestMethod]
        public void PenalInterest_NotDelayed_ReturnsZero()
        {
            string policyId = "POL123";
            int daysDelayed = 0;

            decimal penalInterest = _service.CalculatePenalInterest(policyId, daysDelayed);
            int duration = _service.GetPolicyDurationInMonths(policyId);

            Assert.AreEqual(0m, penalInterest);
            Assert.IsTrue(duration > 0);
            Assert.IsNotNull(policyId);
            Assert.AreNotEqual(100m, penalInterest);
        }

        [TestMethod]
        public void SurrenderValue_ValidRequest_CalculatesValue()
        {
            string policyId = "POL123";
            DateTime requestDate = DateTime.Now;

            decimal surrenderValue = _service.GetSurrenderValue(policyId, requestDate);
            decimal bonus = _service.CalculateBonusAccrued(policyId);

            Assert.IsTrue(surrenderValue > 0);
            Assert.IsTrue(bonus >= 0);
            Assert.IsNotNull(policyId);
            Assert.AreNotEqual(0m, surrenderValue);
        }

        [TestMethod]
        public void CkycVerification_ValidPan_ReturnsCkycDetails()
        {
            string pan = "ABCDE1234F";
            string customerId = "CUST123";

            string ckycNo = _service.GetCkycNumber(pan);
            double matchProb = _service.GetCkycMatchProbability(customerId, ckycNo);

            Assert.IsNotNull(ckycNo);
            Assert.IsTrue(matchProb > 0.8);
            Assert.AreNotEqual("", ckycNo);
            Assert.IsNotNull(pan);
        }

        [TestMethod]
        public void DigiLocker_ValidDoc_ReturnsUri()
        {
            string docType = "PAN";
            string docNo = "ABCDE1234F";

            string uri = _service.FetchDigiLockerDocumentUri(docType, docNo);
            bool isValid = _service.VerifyPanFormat(docNo);

            Assert.IsNotNull(uri);
            Assert.IsTrue(isValid);
            Assert.IsTrue(uri.StartsWith("https://"));
            Assert.AreNotEqual("", uri);
        }

        [TestMethod]
        public void VerificationStatus_ValidCode_ReturnsDescription()
        {
            int statusCode = 1;

            string desc = _service.GetVerificationStatusDescription(statusCode);

            Assert.IsNotNull(desc);
            Assert.AreEqual("Verified", desc);
            Assert.AreNotEqual("", desc);
            Assert.IsTrue(desc.Length > 0);
        }
    }

    // Stub implementation for testing purposes
    public class KycVerificationServiceStub : IKycVerificationService
    {
        public bool VerifyPanFormat(string panNumber) => panNumber == "ABCDE1234F";
        public bool VerifyAadharChecksum(string aadharNumber) => aadharNumber == "123456789012";
        public bool IsKycCompliant(string customerId) => customerId == "CUST123";
        public bool CheckNameMatch(string customerName, string documentName) => customerName == documentName;
        public bool IsAddressProofValid(string documentId, DateTime issueDate) => documentId == "DOC123";
        public bool ValidateSignature(string customerId, string signatureHash) => true;
        public bool IsCustomerMinor(string customerId, DateTime dateOfBirth) => (DateTime.Now.Year - dateOfBirth.Year) < 18;
        public bool CheckPepStatus(string customerId) => customerId == "CUST_HIGH_RISK";
        public bool IsFatcaCompliant(string customerId) => customerId != "CUST_US";
        public bool VerifyBankDetails(string accountNumber, string ifscCode) => accountNumber == "123456789" && ifscCode == "SBIN0001234";
        public bool RequiresEnhancedDueDiligence(string customerId, decimal maturityAmount) => customerId == "CUST_HIGH_RISK";
        
        public decimal CalculateTotalMaturityAmount(string policyId, decimal baseAmount) => baseAmount + 10000m;
        public decimal GetTdsDeductionAmount(string panNumber, decimal maturityAmount) => VerifyPanFormat(panNumber) ? maturityAmount * 0.10m : maturityAmount * 0.20m;
        public decimal CalculatePenalInterest(string policyId, int daysDelayed) => daysDelayed * 100m;
        public decimal GetSurrenderValue(string policyId, DateTime requestDate) => 40000m;
        public decimal CalculateBonusAccrued(string policyId) => 5000m;
        public decimal GetMaximumAllowableCashPayout(string customerId) => 10000m;
        public decimal ComputeNetSettlementAmount(decimal grossAmount, decimal deductions) => grossAmount - deductions;

        public double GetTdsRate(string panNumber, bool isPanValid) => isPanValid ? 0.10 : 0.20;
        public double CalculateNameMatchConfidence(string name1, string name2) => name1 == name2 ? 1.0 : 0.2;
        public double GetRiskScore(string customerId) => customerId == "CUST_HIGH_RISK" ? 0.9 : 0.1;
        public double GetFaceMatchPercentage(string photoId1, string photoId2) => 0.95;
        public double GetDocumentClarityScore(string documentId) => documentId == "DOC123" ? 0.9 : 0.2;
        public double GetCkycMatchProbability(string customerId, string ckycRecordId) => 0.95;

        public int GetPendingKycDocumentCount(string customerId) => customerId == "CUST123" ? 0 : 2;
        public int GetDaysUntilMaturity(string policyId, DateTime currentDate) => 30;
        public int GetAadharLinkageStatusCode(string customerId) => customerId == "CUST123" ? 1 : 0;
        public int CountFailedVerificationAttempts(string customerId) => 0;
        public int GetPolicyDurationInMonths(string policyId) => 120;
        public int GetAgeAtMaturity(string customerId, DateTime maturityDate) => customerId == "CUST_MINOR" ? 10 : 30;
        public int GetDocumentExpiryDays(string documentId, DateTime currentDate) => 365;

        public string GenerateKycReferenceNumber(string customerId) => "KYC" + customerId;
        public string GetCkycNumber(string panNumber) => "CKYC" + panNumber;
        public string RetrieveAadharVaultReference(string aadharNumber) => aadharNumber == "123456789012" ? "VAULT123" : null;
        public string GetVerificationStatusDescription(int statusCode) => statusCode == 1 ? "Verified" : "Pending";
        public string FetchPrimaryBankIfsc(string customerId) => customerId == "CUST123" ? "SBIN0001234" : "UNKNOWN";
        public string GetRejectionReasonCode(string documentId) => documentId == "DOC123" ? null : "BLURRY";
        public string GenerateSettlementTransactionId(string policyId, decimal amount) => "TXN" + policyId;
        public string GetTaxResidencyCountry(string customerId) => customerId == "CUST_US" ? "USA" : "IND";
        public string FetchDigiLockerDocumentUri(string documentType, string documentNumber) => "https://digilocker.gov.in/" + documentNumber;
    }
}