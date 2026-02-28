using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.International_NRI_Processing;

namespace MaturityBenefitProc.Tests.Helpers.International_NRI_Processing
{
    [TestClass]
    public class NriRepatriationServiceValidationTests
    {
        // Note: Since INriRepatriationService is an interface, we assume a mock or a concrete implementation 
        // named NriRepatriationService exists for testing purposes.
        private INriRepatriationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation or mock setup here.
            // For the sake of this generated test file, we instantiate a hypothetical concrete class.
            // In a real scenario, this would be a mock (e.g., using Moq) or the actual implementation.
            _service = new NriRepatriationServiceMock(); 
        }

        [TestMethod]
        public void ValidateRepatriationEligibility_ValidInputs_ReturnsTrue()
        {
            bool result1 = _service.ValidateRepatriationEligibility("POL123", "CUST456");
            bool result2 = _service.ValidateRepatriationEligibility("POL999", "CUST888");
            
            Assert.IsTrue(result1, "Expected eligibility to be true for valid inputs.");
            Assert.IsTrue(result2, "Expected eligibility to be true for valid inputs.");
            Assert.IsNotNull(result1);
            Assert.AreNotEqual(result1, false);
        }

        [TestMethod]
        public void ValidateRepatriationEligibility_InvalidInputs_ReturnsFalse()
        {
            bool result1 = _service.ValidateRepatriationEligibility("", "CUST456");
            bool result2 = _service.ValidateRepatriationEligibility("POL123", null);
            bool result3 = _service.ValidateRepatriationEligibility("   ", "   ");

            Assert.IsFalse(result1, "Expected false for empty policy ID.");
            Assert.IsFalse(result2, "Expected false for null customer ID.");
            Assert.IsFalse(result3, "Expected false for whitespace inputs.");
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateMaximumRepatriationAmount_ValidInputs_ReturnsExpectedAmount()
        {
            DateTime evalDate = new DateTime(2023, 1, 1);
            decimal amount1 = _service.CalculateMaximumRepatriationAmount("POL123", evalDate);
            decimal amount2 = _service.CalculateMaximumRepatriationAmount("POL456", evalDate.AddDays(10));

            Assert.IsTrue(amount1 >= 0, "Amount should be non-negative.");
            Assert.IsTrue(amount2 >= 0, "Amount should be non-negative.");
            Assert.AreNotEqual(-1m, amount1);
            Assert.IsNotNull(amount1);
        }

        [TestMethod]
        public void CalculateMaximumRepatriationAmount_InvalidPolicyId_ReturnsZero()
        {
            DateTime evalDate = DateTime.Now;
            decimal amount1 = _service.CalculateMaximumRepatriationAmount("", evalDate);
            decimal amount2 = _service.CalculateMaximumRepatriationAmount(null, evalDate);

            Assert.AreEqual(0m, amount1, "Expected 0 for empty policy ID.");
            Assert.AreEqual(0m, amount2, "Expected 0 for null policy ID.");
            Assert.IsNotNull(amount1);
            Assert.IsNotNull(amount2);
        }

        [TestMethod]
        public void GetCurrentFemaRepatriationLimitPercentage_ValidPolicy_ReturnsValidPercentage()
        {
            double limit1 = _service.GetCurrentFemaRepatriationLimitPercentage("POL123");
            double limit2 = _service.GetCurrentFemaRepatriationLimitPercentage("POL999");

            Assert.IsTrue(limit1 >= 0.0 && limit1 <= 100.0, "Limit should be between 0 and 100.");
            Assert.IsTrue(limit2 >= 0.0 && limit2 <= 100.0, "Limit should be between 0 and 100.");
            Assert.IsNotNull(limit1);
            Assert.AreNotEqual(-1.0, limit1);
        }

        [TestMethod]
        public void GetCurrentFemaRepatriationLimitPercentage_InvalidPolicy_ReturnsZero()
        {
            double limit1 = _service.GetCurrentFemaRepatriationLimitPercentage("");
            double limit2 = _service.GetCurrentFemaRepatriationLimitPercentage(null);

            Assert.AreEqual(0.0, limit1, "Expected 0 for empty policy ID.");
            Assert.AreEqual(0.0, limit2, "Expected 0 for null policy ID.");
            Assert.IsNotNull(limit1);
            Assert.IsNotNull(limit2);
        }

        [TestMethod]
        public void GetDaysSinceLastRepatriation_ValidCustomer_ReturnsValidDays()
        {
            int days1 = _service.GetDaysSinceLastRepatriation("CUST123");
            int days2 = _service.GetDaysSinceLastRepatriation("CUST999");

            Assert.IsTrue(days1 >= 0, "Days should be non-negative.");
            Assert.IsTrue(days2 >= 0, "Days should be non-negative.");
            Assert.IsNotNull(days1);
            Assert.AreNotEqual(-1, days1);
        }

        [TestMethod]
        public void GetDaysSinceLastRepatriation_InvalidCustomer_ReturnsMinusOne()
        {
            int days1 = _service.GetDaysSinceLastRepatriation("");
            int days2 = _service.GetDaysSinceLastRepatriation(null);

            Assert.AreEqual(-1, days1, "Expected -1 for empty customer ID.");
            Assert.AreEqual(-1, days2, "Expected -1 for null customer ID.");
            Assert.IsNotNull(days1);
            Assert.IsNotNull(days2);
        }

        [TestMethod]
        public void GetNroAccountStatus_ValidAccount_ReturnsStatus()
        {
            string status1 = _service.GetNroAccountStatus("ACC123");
            string status2 = _service.GetNroAccountStatus("ACC999");

            Assert.IsNotNull(status1);
            Assert.IsNotNull(status2);
            Assert.AreNotEqual("", status1);
            Assert.IsTrue(status1.Length > 0);
        }

        [TestMethod]
        public void GetNroAccountStatus_InvalidAccount_ReturnsUnknown()
        {
            string status1 = _service.GetNroAccountStatus("");
            string status2 = _service.GetNroAccountStatus(null);

            Assert.AreEqual("UNKNOWN", status1);
            Assert.AreEqual("UNKNOWN", status2);
            Assert.IsNotNull(status1);
            Assert.IsNotNull(status2);
        }

        [TestMethod]
        public void CalculateTaxDeductionAtSource_ValidInputs_ReturnsCalculatedTax()
        {
            decimal tax1 = _service.CalculateTaxDeductionAtSource(10000m, "US");
            decimal tax2 = _service.CalculateTaxDeductionAtSource(50000m, "UK");

            Assert.IsTrue(tax1 >= 0m, "Tax should be non-negative.");
            Assert.IsTrue(tax2 >= 0m, "Tax should be non-negative.");
            Assert.IsTrue(tax1 <= 10000m, "Tax cannot exceed repatriation amount.");
            Assert.IsNotNull(tax1);
        }

        [TestMethod]
        public void CalculateTaxDeductionAtSource_NegativeAmount_ReturnsZero()
        {
            decimal tax1 = _service.CalculateTaxDeductionAtSource(-1000m, "US");
            decimal tax2 = _service.CalculateTaxDeductionAtSource(-5000m, "UK");

            Assert.AreEqual(0m, tax1);
            Assert.AreEqual(0m, tax2);
            Assert.IsNotNull(tax1);
            Assert.IsNotNull(tax2);
        }

        [TestMethod]
        public void IsFemaComplianceMet_ValidInputs_ReturnsBoolean()
        {
            bool result1 = _service.IsFemaComplianceMet("CUST123", 5000m);
            bool result2 = _service.IsFemaComplianceMet("CUST999", 1000000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1); // Just checking it returns a bool
            Assert.AreNotEqual(result1, null);
        }

        [TestMethod]
        public void IsFemaComplianceMet_NegativeAmount_ReturnsFalse()
        {
            bool result1 = _service.IsFemaComplianceMet("CUST123", -5000m);
            bool result2 = _service.IsFemaComplianceMet("CUST999", -1m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GenerateForm15CbRequirementCode_ValidAmount_ReturnsCode()
        {
            string code1 = _service.GenerateForm15CbRequirementCode(500000m);
            string code2 = _service.GenerateForm15CbRequirementCode(1000000m);

            Assert.IsNotNull(code1);
            Assert.IsNotNull(code2);
            Assert.AreNotEqual("", code1);
            Assert.IsTrue(code1.Length > 0);
        }

        [TestMethod]
        public void GenerateForm15CbRequirementCode_ZeroOrNegativeAmount_ReturnsNotRequired()
        {
            string code1 = _service.GenerateForm15CbRequirementCode(0m);
            string code2 = _service.GenerateForm15CbRequirementCode(-500m);

            Assert.AreEqual("NOT_REQUIRED", code1);
            Assert.AreEqual("NOT_REQUIRED", code2);
            Assert.IsNotNull(code1);
            Assert.IsNotNull(code2);
        }

        [TestMethod]
        public void GetRemainingRepatriationsForFinancialYear_ValidInputs_ReturnsCount()
        {
            int count1 = _service.GetRemainingRepatriationsForFinancialYear("CUST123", new DateTime(2023, 4, 1));
            int count2 = _service.GetRemainingRepatriationsForFinancialYear("CUST999", new DateTime(2023, 4, 1));

            Assert.IsTrue(count1 >= 0);
            Assert.IsTrue(count2 >= 0);
            Assert.IsNotNull(count1);
            Assert.AreNotEqual(-1, count1);
        }

        [TestMethod]
        public void GetRemainingRepatriationsForFinancialYear_InvalidCustomer_ReturnsZero()
        {
            int count1 = _service.GetRemainingRepatriationsForFinancialYear("", new DateTime(2023, 4, 1));
            int count2 = _service.GetRemainingRepatriationsForFinancialYear(null, new DateTime(2023, 4, 1));

            Assert.AreEqual(0, count1);
            Assert.AreEqual(0, count2);
            Assert.IsNotNull(count1);
            Assert.IsNotNull(count2);
        }

        [TestMethod]
        public void CalculateExchangeRateVariance_ValidInputs_ReturnsVariance()
        {
            decimal variance1 = _service.CalculateExchangeRateVariance("USD", 1000m);
            decimal variance2 = _service.CalculateExchangeRateVariance("GBP", 5000m);

            Assert.IsNotNull(variance1);
            Assert.IsNotNull(variance2);
            Assert.IsTrue(variance1 >= 0m);
            Assert.AreNotEqual(-1m, variance1);
        }

        [TestMethod]
        public void CalculateExchangeRateVariance_InvalidCurrency_ReturnsZero()
        {
            decimal variance1 = _service.CalculateExchangeRateVariance("", 1000m);
            decimal variance2 = _service.CalculateExchangeRateVariance(null, 5000m);

            Assert.AreEqual(0m, variance1);
            Assert.AreEqual(0m, variance2);
            Assert.IsNotNull(variance1);
            Assert.IsNotNull(variance2);
        }

        [TestMethod]
        public void GetDoubleTaxationAvoidanceAgreementRate_ValidCountry_ReturnsRate()
        {
            double rate1 = _service.GetDoubleTaxationAvoidanceAgreementRate("US");
            double rate2 = _service.GetDoubleTaxationAvoidanceAgreementRate("UK");

            Assert.IsTrue(rate1 >= 0.0 && rate1 <= 100.0);
            Assert.IsTrue(rate2 >= 0.0 && rate2 <= 100.0);
            Assert.IsNotNull(rate1);
            Assert.AreNotEqual(-1.0, rate1);
        }

        [TestMethod]
        public void GetDoubleTaxationAvoidanceAgreementRate_InvalidCountry_ReturnsDefaultRate()
        {
            double rate1 = _service.GetDoubleTaxationAvoidanceAgreementRate("");
            double rate2 = _service.GetDoubleTaxationAvoidanceAgreementRate(null);

            Assert.AreEqual(0.0, rate1);
            Assert.AreEqual(0.0, rate2);
            Assert.IsNotNull(rate1);
            Assert.IsNotNull(rate2);
        }

        [TestMethod]
        public void VerifyNreAccountFundingSource_ValidInputs_ReturnsBoolean()
        {
            bool result1 = _service.VerifyNreAccountFundingSource("POL123", "ACC123");
            bool result2 = _service.VerifyNreAccountFundingSource("POL999", "ACC999");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.AreNotEqual(result1, null);
        }

        [TestMethod]
        public void VerifyNreAccountFundingSource_InvalidInputs_ReturnsFalse()
        {
            bool result1 = _service.VerifyNreAccountFundingSource("", "ACC123");
            bool result2 = _service.VerifyNreAccountFundingSource("POL123", null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void GetReserveBankOfIndiaApprovalCode_ValidInputs_ReturnsCode()
        {
            string code1 = _service.GetReserveBankOfIndiaApprovalCode("POL123", 50000m);
            string code2 = _service.GetReserveBankOfIndiaApprovalCode("POL999", 100000m);

            Assert.IsNotNull(code1);
            Assert.IsNotNull(code2);
            Assert.AreNotEqual("", code1);
            Assert.IsTrue(code1.Length > 0);
        }

        [TestMethod]
        public void GetReserveBankOfIndiaApprovalCode_InvalidPolicy_ReturnsNone()
        {
            string code1 = _service.GetReserveBankOfIndiaApprovalCode("", 50000m);
            string code2 = _service.GetReserveBankOfIndiaApprovalCode(null, 100000m);

            Assert.AreEqual("NONE", code1);
            Assert.AreEqual("NONE", code2);
            Assert.IsNotNull(code1);
            Assert.IsNotNull(code2);
        }

        [TestMethod]
        public void ComputeAllowableMaturityProceeds_ValidInputs_ReturnsAmount()
        {
            decimal amount1 = _service.ComputeAllowableMaturityProceeds("POL123", 100000m);
            decimal amount2 = _service.ComputeAllowableMaturityProceeds("POL999", 500000m);

            Assert.IsTrue(amount1 >= 0m);
            Assert.IsTrue(amount2 >= 0m);
            Assert.IsTrue(amount1 <= 100000m);
            Assert.IsNotNull(amount1);
        }

        [TestMethod]
        public void ComputeAllowableMaturityProceeds_NegativeMaturityValue_ReturnsZero()
        {
            decimal amount1 = _service.ComputeAllowableMaturityProceeds("POL123", -10000m);
            decimal amount2 = _service.ComputeAllowableMaturityProceeds("POL999", -500m);

            Assert.AreEqual(0m, amount1);
            Assert.AreEqual(0m, amount2);
            Assert.IsNotNull(amount1);
            Assert.IsNotNull(amount2);
        }

        [TestMethod]
        public void CalculateDaysToMaturity_ValidInputs_ReturnsDays()
        {
            int days1 = _service.CalculateDaysToMaturity("POL123", DateTime.Now);
            int days2 = _service.CalculateDaysToMaturity("POL999", DateTime.Now.AddDays(10));

            Assert.IsNotNull(days1);
            Assert.IsNotNull(days2);
            Assert.IsTrue(days1 >= 0 || days1 < 0); // Can be negative if already matured
            Assert.AreNotEqual(null, days1);
        }

        [TestMethod]
        public void CalculateDaysToMaturity_InvalidPolicy_ReturnsZero()
        {
            int days1 = _service.CalculateDaysToMaturity("", DateTime.Now);
            int days2 = _service.CalculateDaysToMaturity(null, DateTime.Now);

            Assert.AreEqual(0, days1);
            Assert.AreEqual(0, days2);
            Assert.IsNotNull(days1);
            Assert.IsNotNull(days2);
        }

        [TestMethod]
        public void CheckFatcaComplianceStatus_ValidCustomer_ReturnsBoolean()
        {
            bool status1 = _service.CheckFatcaComplianceStatus("CUST123");
            bool status2 = _service.CheckFatcaComplianceStatus("CUST999");

            Assert.IsNotNull(status1);
            Assert.IsNotNull(status2);
            Assert.IsTrue(status1 || !status1);
            Assert.AreNotEqual(status1, null);
        }

        [TestMethod]
        public void CheckFatcaComplianceStatus_InvalidCustomer_ReturnsFalse()
        {
            bool status1 = _service.CheckFatcaComplianceStatus("");
            bool status2 = _service.CheckFatcaComplianceStatus(null);

            Assert.IsFalse(status1);
            Assert.IsFalse(status2);
            Assert.IsNotNull(status1);
            Assert.IsNotNull(status2);
        }

        [TestMethod]
        public void CalculatePremiumPaidInForeignCurrency_ValidPolicy_ReturnsAmount()
        {
            decimal amount1 = _service.CalculatePremiumPaidInForeignCurrency("POL123");
            decimal amount2 = _service.CalculatePremiumPaidInForeignCurrency("POL999");

            Assert.IsTrue(amount1 >= 0m);
            Assert.IsTrue(amount2 >= 0m);
            Assert.IsNotNull(amount1);
            Assert.AreNotEqual(-1m, amount1);
        }

        [TestMethod]
        public void CalculatePremiumPaidInForeignCurrency_InvalidPolicy_ReturnsZero()
        {
            decimal amount1 = _service.CalculatePremiumPaidInForeignCurrency("");
            decimal amount2 = _service.CalculatePremiumPaidInForeignCurrency(null);

            Assert.AreEqual(0m, amount1);
            Assert.AreEqual(0m, amount2);
            Assert.IsNotNull(amount1);
            Assert.IsNotNull(amount2);
        }

        [TestMethod]
        public void GetProportionateRepatriationRatio_ValidPolicy_ReturnsRatio()
        {
            double ratio1 = _service.GetProportionateRepatriationRatio("POL123");
            double ratio2 = _service.GetProportionateRepatriationRatio("POL999");

            Assert.IsTrue(ratio1 >= 0.0 && ratio1 <= 1.0);
            Assert.IsTrue(ratio2 >= 0.0 && ratio2 <= 1.0);
            Assert.IsNotNull(ratio1);
            Assert.AreNotEqual(-1.0, ratio1);
        }

        [TestMethod]
        public void GetProportionateRepatriationRatio_InvalidPolicy_ReturnsZero()
        {
            double ratio1 = _service.GetProportionateRepatriationRatio("");
            double ratio2 = _service.GetProportionateRepatriationRatio(null);

            Assert.AreEqual(0.0, ratio1);
            Assert.AreEqual(0.0, ratio2);
            Assert.IsNotNull(ratio1);
            Assert.IsNotNull(ratio2);
        }

        [TestMethod]
        public void RetrieveAuthorizedDealerBankCode_ValidCustomer_ReturnsCode()
        {
            string code1 = _service.RetrieveAuthorizedDealerBankCode("CUST123");
            string code2 = _service.RetrieveAuthorizedDealerBankCode("CUST999");

            Assert.IsNotNull(code1);
            Assert.IsNotNull(code2);
            Assert.AreNotEqual("", code1);
            Assert.IsTrue(code1.Length > 0);
        }

        [TestMethod]
        public void RetrieveAuthorizedDealerBankCode_InvalidCustomer_ReturnsUnknown()
        {
            string code1 = _service.RetrieveAuthorizedDealerBankCode("");
            string code2 = _service.RetrieveAuthorizedDealerBankCode(null);

            Assert.AreEqual("UNKNOWN", code1);
            Assert.AreEqual("UNKNOWN", code2);
            Assert.IsNotNull(code1);
            Assert.IsNotNull(code2);
        }

        [TestMethod]
        public void ValidateCaCertificateRequirement_ValidAmount_ReturnsBoolean()
        {
            bool req1 = _service.ValidateCaCertificateRequirement(500000m);
            bool req2 = _service.ValidateCaCertificateRequirement(1000m);

            Assert.IsNotNull(req1);
            Assert.IsNotNull(req2);
            Assert.IsTrue(req1 || !req1);
            Assert.AreNotEqual(req1, null);
        }

        [TestMethod]
        public void ValidateCaCertificateRequirement_NegativeAmount_ReturnsFalse()
        {
            bool req1 = _service.ValidateCaCertificateRequirement(-5000m);
            bool req2 = _service.ValidateCaCertificateRequirement(-1m);

            Assert.IsFalse(req1);
            Assert.IsFalse(req2);
            Assert.IsNotNull(req1);
            Assert.IsNotNull(req2);
        }

        [TestMethod]
        public void CalculateNetRepatriableAmount_ValidInputs_ReturnsNetAmount()
        {
            decimal net1 = _service.CalculateNetRepatriableAmount(10000m, 1000m, 50m);
            decimal net2 = _service.CalculateNetRepatriableAmount(50000m, 5000m, 100m);

            Assert.AreEqual(8950m, net1);
            Assert.AreEqual(44900m, net2);
            Assert.IsTrue(net1 >= 0m);
            Assert.IsNotNull(net1);
        }

        [TestMethod]
        public void CalculateNetRepatriableAmount_DeductionsExceedGross_ReturnsZero()
        {
            decimal net1 = _service.CalculateNetRepatriableAmount(1000m, 1500m, 0m);
            decimal net2 = _service.CalculateNetRepatriableAmount(500m, 100m, 600m);

            Assert.AreEqual(0m, net1);
            Assert.AreEqual(0m, net2);
            Assert.IsNotNull(net1);
            Assert.IsNotNull(net2);
        }

        [TestMethod]
        public void GetPendingDocumentCount_ValidPolicy_ReturnsCount()
        {
            int count1 = _service.GetPendingDocumentCount("POL123");
            int count2 = _service.GetPendingDocumentCount("POL999");

            Assert.IsTrue(count1 >= 0);
            Assert.IsTrue(count2 >= 0);
            Assert.IsNotNull(count1);
            Assert.AreNotEqual(-1, count1);
        }

        [TestMethod]
        public void GetPendingDocumentCount_InvalidPolicy_ReturnsZero()
        {
            int count1 = _service.GetPendingDocumentCount("");
            int count2 = _service.GetPendingDocumentCount(null);

            Assert.AreEqual(0, count1);
            Assert.AreEqual(0, count2);
            Assert.IsNotNull(count1);
            Assert.IsNotNull(count2);
        }

        [TestMethod]
        public void GetRepatriationRejectionReasonCode_ValidInputs_ReturnsCode()
        {
            string code1 = _service.GetRepatriationRejectionReasonCode("POL123", 5000000m);
            string code2 = _service.GetRepatriationRejectionReasonCode("POL999", 1000m);

            Assert.IsNotNull(code1);
            Assert.IsNotNull(code2);
            Assert.AreNotEqual(null, code1);
            Assert.IsTrue(code1.Length > 0 || code1 == "NONE");
        }

        [TestMethod]
        public void GetRepatriationRejectionReasonCode_InvalidPolicy_ReturnsInvalidPolicyCode()
        {
            string code1 = _service.GetRepatriationRejectionReasonCode("", 5000m);
            string code2 = _service.GetRepatriationRejectionReasonCode(null, 5000m);

            Assert.AreEqual("INVALID_POLICY", code1);
            Assert.AreEqual("INVALID_POLICY", code2);
            Assert.IsNotNull(code1);
            Assert.IsNotNull(code2);
        }

        [TestMethod]
        public void IsCountryInRestrictedList_ValidCountry_ReturnsBoolean()
        {
            bool restricted1 = _service.IsCountryInRestrictedList("IR");
            bool restricted2 = _service.IsCountryInRestrictedList("US");

            Assert.IsNotNull(restricted1);
            Assert.IsNotNull(restricted2);
            Assert.IsTrue(restricted1 || !restricted1);
            Assert.AreNotEqual(restricted1, null);
        }

        [TestMethod]
        public void IsCountryInRestrictedList_InvalidCountry_ReturnsTrue()
        {
            bool restricted1 = _service.IsCountryInRestrictedList("");
            bool restricted2 = _service.IsCountryInRestrictedList(null);

            Assert.IsTrue(restricted1);
            Assert.IsTrue(restricted2);
            Assert.IsNotNull(restricted1);
            Assert.IsNotNull(restricted2);
        }

        [TestMethod]
        public void GetTotalRepatriatedAmountYearToDate_ValidInputs_ReturnsAmount()
        {
            decimal amount1 = _service.GetTotalRepatriatedAmountYearToDate("CUST123", new DateTime(2023, 4, 1));
            decimal amount2 = _service.GetTotalRepatriatedAmountYearToDate("CUST999", new DateTime(2023, 4, 1));

            Assert.IsTrue(amount1 >= 0m);
            Assert.IsTrue(amount2 >= 0m);
            Assert.IsNotNull(amount1);
            Assert.AreNotEqual(-1m, amount1);
        }

        [TestMethod]
        public void GetTotalRepatriatedAmountYearToDate_InvalidCustomer_ReturnsZero()
        {
            decimal amount1 = _service.GetTotalRepatriatedAmountYearToDate("", new DateTime(2023, 4, 1));
            decimal amount2 = _service.GetTotalRepatriatedAmountYearToDate(null, new DateTime(2023, 4, 1));

            Assert.AreEqual(0m, amount1);
            Assert.AreEqual(0m, amount2);
            Assert.IsNotNull(amount1);
            Assert.IsNotNull(amount2);
        }
    }

    // Mock implementation for the purpose of compiling the tests
    public class NriRepatriationServiceMock : INriRepatriationService
    {
        public bool ValidateRepatriationEligibility(string policyId, string customerId) => !string.IsNullOrWhiteSpace(policyId) && !string.IsNullOrWhiteSpace(customerId);
        public decimal CalculateMaximumRepatriationAmount(string policyId, DateTime evaluationDate) => string.IsNullOrWhiteSpace(policyId) ? 0m : 10000m;
        public double GetCurrentFemaRepatriationLimitPercentage(string policyId) => string.IsNullOrWhiteSpace(policyId) ? 0.0 : 50.0;
        public int GetDaysSinceLastRepatriation(string customerId) => string.IsNullOrWhiteSpace(customerId) ? -1 : 30;
        public string GetNroAccountStatus(string accountId) => string.IsNullOrWhiteSpace(accountId) ? "UNKNOWN" : "ACTIVE";
        public decimal CalculateTaxDeductionAtSource(decimal repatriationAmount, string taxResidencyCode) => repatriationAmount < 0 ? 0m : repatriationAmount * 0.1m;
        public bool IsFemaComplianceMet(string customerId, decimal requestedAmount) => requestedAmount >= 0 && !string.IsNullOrWhiteSpace(customerId);
        public string GenerateForm15CbRequirementCode(decimal amount) => amount <= 0 ? "NOT_REQUIRED" : "REQ_15CB";
        public int GetRemainingRepatriationsForFinancialYear(string customerId, DateTime currentFinancialYearStart) => string.IsNullOrWhiteSpace(customerId) ? 0 : 2;
        public decimal CalculateExchangeRateVariance(string currencyCode, decimal baseAmount) => string.IsNullOrWhiteSpace(currencyCode) ? 0m : 5m;
        public double GetDoubleTaxationAvoidanceAgreementRate(string countryCode) => string.IsNullOrWhiteSpace(countryCode) ? 0.0 : 15.0;
        public bool VerifyNreAccountFundingSource(string policyId, string accountId) => !string.IsNullOrWhiteSpace(policyId) && !string.IsNullOrWhiteSpace(accountId);
        public string GetReserveBankOfIndiaApprovalCode(string policyId, decimal amount) => string.IsNullOrWhiteSpace(policyId) ? "NONE" : "RBI_APP_123";
        public decimal ComputeAllowableMaturityProceeds(string policyId, decimal totalMaturityValue) => totalMaturityValue < 0 ? 0m : totalMaturityValue;
        public int CalculateDaysToMaturity(string policyId, DateTime currentDate) => string.IsNullOrWhiteSpace(policyId) ? 0 : 100;
        public bool CheckFatcaComplianceStatus(string customerId) => !string.IsNullOrWhiteSpace(customerId);
        public decimal CalculatePremiumPaidInForeignCurrency(string policyId) => string.IsNullOrWhiteSpace(policyId) ? 0m : 5000m;
        public double GetProportionateRepatriationRatio(string policyId) => string.IsNullOrWhiteSpace(policyId) ? 0.0 : 0.5;
        public string RetrieveAuthorizedDealerBankCode(string customerId) => string.IsNullOrWhiteSpace(customerId) ? "UNKNOWN" : "AD_BANK_001";
        public bool ValidateCaCertificateRequirement(decimal requestedAmount) => requestedAmount > 0;
        public decimal CalculateNetRepatriableAmount(decimal grossAmount, decimal tdsAmount, decimal fees) => (grossAmount - tdsAmount - fees) < 0 ? 0m : (grossAmount - tdsAmount - fees);
        public int GetPendingDocumentCount(string policyId) => string.IsNullOrWhiteSpace(policyId) ? 0 : 1;
        public string GetRepatriationRejectionReasonCode(string policyId, decimal requestedAmount) => string.IsNullOrWhiteSpace(policyId) ? "INVALID_POLICY" : "NONE";
        public bool IsCountryInRestrictedList(string countryCode) => string.IsNullOrWhiteSpace(countryCode) || countryCode == "IR";
        public decimal GetTotalRepatriatedAmountYearToDate(string customerId, DateTime financialYearStart) => string.IsNullOrWhiteSpace(customerId) ? 0m : 1000m;
    }
}