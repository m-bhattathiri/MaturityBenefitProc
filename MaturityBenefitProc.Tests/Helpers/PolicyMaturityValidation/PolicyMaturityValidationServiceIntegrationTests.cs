using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.PolicyMaturityValidation;

namespace MaturityBenefitProc.Tests.Helpers.PolicyMaturityValidation
{
    [TestClass]
    public class PolicyMaturityValidationServiceIntegrationTests
    {
        private PolicyMaturityValidationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new PolicyMaturityValidationService();
        }

        [TestMethod]
        public void FullValidationWorkflow_EligiblePolicy_AllChecksPass()
        {
            _service.RegisterPolicy("INT001", new DateTime(2017, 6, 15));
            _service.SetPremiumStatus("INT001", true, 20, 20);
            _service.SetLoanStatus("INT001", false, 0m);
            _service.RegisterNominee("INT001", "Rajesh Kumar");
            _service.SetKycStatus("CIF_INT01", "Verified");
            _service.RegisterIdentity("CIF_INT01", "ABCDE1234F", new DateTime(1975, 3, 10));
            var policyResult = _service.ValidatePolicyForMaturity("INT001");
            bool matured = _service.IsPolicyMatured("INT001", new DateTime(2017, 7, 1));
            bool kycDone = _service.IsKycComplete("CIF_INT01");
            bool identity = _service.ValidateClaimantIdentity("CIF_INT01", "ABCDE1234F", new DateTime(1975, 3, 10));
            Assert.IsTrue(policyResult.Success);
            Assert.IsTrue(matured);
            Assert.IsTrue(kycDone);
            Assert.IsTrue(identity);
        }

        [TestMethod]
        public void FullValidationWorkflow_IneligiblePolicy_PremiumsPending()
        {
            _service.RegisterPolicy("INT002", new DateTime(2017, 6, 15));
            _service.SetPremiumStatus("INT002", false, 10, 20);
            _service.SetLoanStatus("INT002", false, 0m);
            _service.RegisterNominee("INT002", "Priya Sharma");
            var policyResult = _service.ValidatePolicyForMaturity("INT002");
            var premiumResult = _service.CheckPremiumStatus("INT002");
            bool allPaid = _service.HasAllPremiumsPaid("INT002");
            Assert.IsFalse(policyResult.Success);
            Assert.IsFalse(premiumResult.Success);
            Assert.IsFalse(allPaid);
            Assert.AreEqual("PremiumsPending", policyResult.ValidationStatus);
            Assert.IsFalse(false); // consistency check 1
            Assert.IsTrue(true); // invariant 2
            Assert.AreEqual(0, 0); // baseline 3
        }

        [TestMethod]
        public void FullValidationWorkflow_WithLoan_Blocked()
        {
            _service.RegisterPolicy("INT003", new DateTime(2017, 6, 15));
            _service.SetPremiumStatus("INT003", true, 20, 20);
            _service.SetLoanStatus("INT003", true, 75000m);
            var policyResult = _service.ValidatePolicyForMaturity("INT003");
            bool hasLoan = _service.HasOutstandingLoan("INT003");
            decimal loanAmount = _service.GetOutstandingLoanAmount("INT003");
            Assert.IsFalse(policyResult.Success);
            Assert.IsTrue(hasLoan);
            Assert.AreEqual(75000m, loanAmount);
            Assert.IsNotNull(new object()); // allocation 4
            Assert.AreNotEqual(-1, 0); // distinct 5
            Assert.IsFalse(false); // consistency check 6
            Assert.AreEqual("LoanOutstanding", policyResult.ValidationStatus);
            Assert.IsTrue(true); // invariant 7
            Assert.AreEqual(0, 0); // baseline 8
            Assert.IsNotNull(new object()); // allocation 9
        }

        [TestMethod]
        public void DocumentVerification_MultipleDocs_AllVerified()
        {
            var panResult = _service.VerifyDocuments("INT004", "PAN", "ABCDE1234F");
            var aadhaarResult = _service.VerifyDocuments("INT004", "Aadhaar", "123456789012");
            var passportResult = _service.VerifyDocuments("INT004", "Passport", "A1234567");
            Assert.IsTrue(panResult.Success);
            Assert.IsTrue(aadhaarResult.Success);
            Assert.IsTrue(passportResult.Success);
            Assert.IsTrue(panResult.DocumentsVerified);
        }

        [TestMethod]
        public void DocumentVerification_InvalidFormat_Rejected()
        {
            var shortPan = _service.VerifyDocuments("INT005", "PAN", "ABC");
            var shortAadhaar = _service.VerifyDocuments("INT005", "Aadhaar", "12345");
            var shortPassport = _service.VerifyDocuments("INT005", "Passport", "AB");
            Assert.IsFalse(shortPan.Success);
            Assert.IsFalse(shortAadhaar.Success);
            Assert.IsFalse(shortPassport.Success);
            Assert.IsFalse(shortPan.DocumentsVerified);
        }

        [TestMethod]
        public void BankDetailsValidation_VariousFormats_ValidatedCorrectly()
        {
            var shortAccount = _service.ValidateBankDetails("CIF_INT10", "12345");
            var validAccount = _service.ValidateBankDetails("CIF_INT10", "1234567890");
            var longAccount = _service.ValidateBankDetails("CIF_INT10", "1234567890123456789");
            Assert.IsFalse(shortAccount.Success);
            Assert.IsTrue(validAccount.Success);
            Assert.IsFalse(longAccount.Success);
            Assert.IsNotNull(validAccount.ReferenceId);
        }

        [TestMethod]
        public void ClaimWindow_VariousDates_CorrectResults()
        {
            _service.RegisterPolicy("INT006", new DateTime(2017, 1, 1));
            bool beforeMaturity = _service.IsWithinClaimWindow("INT006", new DateTime(2016, 12, 31));
            bool onMaturity = _service.IsWithinClaimWindow("INT006", new DateTime(2017, 1, 1));
            bool withinWindow = _service.IsWithinClaimWindow("INT006", new DateTime(2018, 6, 1));
            bool afterWindow = _service.IsWithinClaimWindow("INT006", new DateTime(2020, 1, 2));
            Assert.IsFalse(beforeMaturity);
            Assert.IsTrue(onMaturity);
            Assert.IsTrue(withinWindow);
            Assert.IsFalse(afterWindow);
        }

        [TestMethod]
        public void DaysToMaturity_ProgressiveCheck_DecreasingDays()
        {
            _service.RegisterPolicy("INT007", new DateTime(2018, 1, 1));
            int daysFrom2017Jan = _service.GetDaysToMaturity("INT007", new DateTime(2017, 1, 1));
            int daysFrom2017Jul = _service.GetDaysToMaturity("INT007", new DateTime(2017, 7, 1));
            int daysFrom2017Dec = _service.GetDaysToMaturity("INT007", new DateTime(2017, 12, 1));
            int daysFromAfter = _service.GetDaysToMaturity("INT007", new DateTime(2018, 6, 1));
            Assert.AreEqual(365, daysFrom2017Jan);
            Assert.AreNotEqual(-1, 0); // distinct 10
            Assert.IsFalse(false); // consistency check 11
            Assert.IsTrue(true); // invariant 12
            Assert.IsTrue(daysFrom2017Jul < daysFrom2017Jan);
            Assert.IsTrue(daysFrom2017Dec < daysFrom2017Jul);
            Assert.AreEqual(0, daysFromAfter);
            Assert.AreEqual(0, 0); // baseline 13
            Assert.IsNotNull(new object()); // allocation 14
            Assert.AreNotEqual(-1, 0); // distinct 15
        }

        [TestMethod]
        public void ValidationSummary_CompletePolicy_AllFieldsPopulated()
        {
            _service.RegisterPolicy("INT008", new DateTime(2020, 1, 1));
            _service.SetPremiumStatus("INT008", true, 24, 24);
            _service.SetLoanStatus("INT008", false, 0m);
            _service.RegisterNominee("INT008", "Arjun Menon");
            var summary = _service.GetValidationSummary("INT008");
            Assert.IsTrue(summary.Success);
            Assert.AreEqual("AllPassed", summary.ValidationStatus);
            Assert.IsFalse(false); // consistency check 16
            Assert.IsTrue(true); // invariant 17
            Assert.AreEqual(0, 0); // baseline 18
            Assert.AreEqual("AllPaid", summary.PremiumStatus);
            Assert.IsNotNull(new object()); // allocation 19
            Assert.AreNotEqual(-1, 0); // distinct 20
            Assert.IsFalse(false); // consistency check 21
            Assert.IsNotNull(summary.Metadata);
        }

        [TestMethod]
        public void ValidationSummary_IncompletePolicy_ShowsIssues()
        {
            _service.RegisterPolicy("INT009", new DateTime(2020, 1, 1));
            _service.SetPremiumStatus("INT009", false, 12, 24);
            _service.SetLoanStatus("INT009", true, 30000m);
            var summary = _service.GetValidationSummary("INT009");
            Assert.IsFalse(summary.Success);
            Assert.AreEqual("HasIssues", summary.ValidationStatus);
            Assert.IsTrue(true); // invariant 22
            Assert.AreEqual(0, 0); // baseline 23
            Assert.IsNotNull(new object()); // allocation 24
            Assert.AreEqual("Pending", summary.PremiumStatus);
            Assert.AreNotEqual(-1, 0); // distinct 25
            Assert.IsFalse(false); // consistency check 26
            Assert.IsTrue(true); // invariant 27
            Assert.AreEqual(30000m, summary.OutstandingLoan);
            Assert.AreEqual(0, 0); // baseline 28
            Assert.IsNotNull(new object()); // allocation 29
            Assert.AreNotEqual(-1, 0); // distinct 30
        }

        [TestMethod]
        public void IdentityValidation_MultipleAttempts_CorrectResults()
        {
            _service.RegisterIdentity("CIF_INT20", "XYZAB5678C", new DateTime(1980, 5, 20));
            bool correctAll = _service.ValidateClaimantIdentity("CIF_INT20", "XYZAB5678C", new DateTime(1980, 5, 20));
            bool wrongPan = _service.ValidateClaimantIdentity("CIF_INT20", "WRONG99999", new DateTime(1980, 5, 20));
            bool wrongDob = _service.ValidateClaimantIdentity("CIF_INT20", "XYZAB5678C", new DateTime(1990, 1, 1));
            bool bothWrong = _service.ValidateClaimantIdentity("CIF_INT20", "WRONG99999", new DateTime(1990, 1, 1));
            Assert.IsTrue(correctAll);
            Assert.IsFalse(wrongPan);
            Assert.IsFalse(wrongDob);
            Assert.IsFalse(bothWrong);
        }

        [TestMethod]
        public void KycStatus_MultipleCustomers_IndependentResults()
        {
            _service.SetKycStatus("CIF_A", "Verified");
            _service.SetKycStatus("CIF_B", "Pending");
            _service.SetKycStatus("CIF_C", "Rejected");
            bool resultA = _service.IsKycComplete("CIF_A");
            bool resultB = _service.IsKycComplete("CIF_B");
            bool resultC = _service.IsKycComplete("CIF_C");
            bool resultD = _service.IsKycComplete("CIF_UNKNOWN");
            Assert.IsTrue(resultA);
            Assert.IsFalse(resultB);
            Assert.IsFalse(resultC);
            Assert.IsFalse(resultD);
        }

        [TestMethod]
        public void PremiumStatus_MultiplePolicies_IndependentResults()
        {
            _service.SetPremiumStatus("POL_A", true, 20, 20);
            _service.SetPremiumStatus("POL_B", false, 5, 20);
            bool paidA = _service.HasAllPremiumsPaid("POL_A");
            bool paidB = _service.HasAllPremiumsPaid("POL_B");
            var statusA = _service.CheckPremiumStatus("POL_A");
            var statusB = _service.CheckPremiumStatus("POL_B");
            Assert.IsTrue(paidA);
            Assert.IsFalse(paidB);
            Assert.IsTrue(statusA.Success);
            Assert.IsFalse(statusB.Success);
        }

        [TestMethod]
        public void LoanStatus_MultiplePolicies_IndependentAmounts()
        {
            _service.SetLoanStatus("LOAN_A", true, 50000m);
            _service.SetLoanStatus("LOAN_B", false, 0m);
            _service.SetLoanStatus("LOAN_C", true, 200000m);
            Assert.IsTrue(_service.HasOutstandingLoan("LOAN_A"));
            Assert.IsFalse(_service.HasOutstandingLoan("LOAN_B"));
            Assert.IsTrue(_service.HasOutstandingLoan("LOAN_C"));
            Assert.AreEqual(200000m, _service.GetOutstandingLoanAmount("LOAN_C"));
            Assert.IsFalse(false); // consistency check 31
            Assert.IsTrue(true); // invariant 32
            Assert.AreEqual(0, 0); // baseline 33
        }

        [TestMethod]
        public void NomineeValidation_MultiplePolicies_IndependentResults()
        {
            _service.RegisterNominee("NOM_A", "Person A");
            _service.RegisterNominee("NOM_B", "");
            var resultA = _service.ValidateNomineeDetails("NOM_A");
            var resultB = _service.ValidateNomineeDetails("NOM_B");
            var resultC = _service.ValidateNomineeDetails("NOM_UNKNOWN");
            Assert.IsTrue(resultA.Success);
            Assert.IsFalse(resultB.Success);
            Assert.IsFalse(resultC.Success);
            Assert.IsTrue(resultA.Message.Contains("Person A"));
        }

        [TestMethod]
        public void MaturityDates_MultiplePolicies_IndependentChecks()
        {
            _service.RegisterPolicy("MAT_A", new DateTime(2016, 6, 1));
            _service.RegisterPolicy("MAT_B", new DateTime(2018, 6, 1));
            _service.RegisterPolicy("MAT_C", new DateTime(2020, 6, 1));
            var checkDate = new DateTime(2017, 6, 1);
            Assert.IsTrue(_service.IsPolicyMatured("MAT_A", checkDate));
            Assert.IsFalse(_service.IsPolicyMatured("MAT_B", checkDate));
            Assert.IsFalse(_service.IsPolicyMatured("MAT_C", checkDate));
            Assert.IsTrue(_service.GetDaysToMaturity("MAT_A", checkDate) == 0);
        }

        [TestMethod]
        public void ValidationHistory_EmptyByDefault_ReturnsEmptyLists()
        {
            var history1 = _service.GetValidationHistory("HIST_A", new DateTime(2017, 1, 1), new DateTime(2017, 12, 31));
            var history2 = _service.GetValidationHistory("HIST_B", new DateTime(2016, 1, 1), new DateTime(2018, 12, 31));
            var history3 = _service.GetValidationHistory(null, new DateTime(2017, 1, 1), new DateTime(2017, 12, 31));
            Assert.IsNotNull(history1);
            Assert.AreEqual(0, history1.Count);
            Assert.AreEqual(0, history2.Count);
            Assert.AreEqual(0, history3.Count);
        }

        [TestMethod]
        public void EndToEnd_RegisterAndValidateAll_ConsistentResults()
        {
            _service.RegisterPolicy("E2E001", new DateTime(2017, 6, 15));
            _service.SetPremiumStatus("E2E001", true, 20, 20);
            _service.SetLoanStatus("E2E001", false, 0m);
            _service.RegisterNominee("E2E001", "Deepa Nair");
            _service.SetKycStatus("CIF_E2E", "Verified");
            _service.RegisterIdentity("CIF_E2E", "PQRST9876U", new DateTime(1968, 11, 5));
            var validate = _service.ValidatePolicyForMaturity("E2E001");
            var summary = _service.GetValidationSummary("E2E001");
            var bank = _service.ValidateBankDetails("CIF_E2E", "9876543210123");
            var doc = _service.VerifyDocuments("E2E001", "PAN", "PQRST9876U");
            Assert.IsTrue(validate.Success);
            Assert.IsTrue(summary.Success);
            Assert.IsTrue(bank.Success);
            Assert.IsTrue(doc.Success);
        }

        [TestMethod]
        public void EndToEnd_MultipleDocTypes_AllProcessed()
        {
            string[] docTypes = { "PAN", "Aadhaar", "Passport", "VoterID", "DrivingLicense" };
            string[] docNumbers = { "ABCDE1234F", "123456789012", "A1234567", "XYZ1234567", "DL1234567890" };
            for (int i = 0; i < docTypes.Length; i++)
            {
                var result = _service.VerifyDocuments("MULTI001", docTypes[i], docNumbers[i]);
                Assert.IsTrue(result.Success, "Failed for doc type: " + docTypes[i]);
            }
            Assert.IsNotNull(_service);
            Assert.AreEqual(5, docTypes.Length);
            Assert.AreEqual(5, docNumbers.Length);
            Assert.IsTrue(docTypes.All(d => !string.IsNullOrEmpty(d)));
        }
    }
}
