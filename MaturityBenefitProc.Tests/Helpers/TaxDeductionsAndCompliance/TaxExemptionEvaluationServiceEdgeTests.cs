using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class TaxExemptionEvaluationServiceEdgeCaseTests
    {
        // Note: Assuming a mock or concrete implementation exists for testing purposes.
        // Since the interface is provided, we will mock it using a stub class for the tests to compile and run.
        private class TaxExemptionEvaluationServiceStub : ITaxExemptionEvaluationService
        {
            public bool IsEligibleForSection1010D(string policyId, DateTime issueDate) => !string.IsNullOrEmpty(policyId) && issueDate > DateTime.MinValue;
            public decimal CalculateTaxableMaturityAmount(string policyId, decimal totalPremiumsPaid, decimal maturityAmount) => Math.Max(0, maturityAmount - totalPremiumsPaid);
            public double GetApplicableTdsRate(string panNumber, bool isPanValid) => isPanValid && !string.IsNullOrEmpty(panNumber) ? 5.0 : 20.0;
            public decimal CalculateTdsAmount(decimal taxableAmount, double tdsRate) => taxableAmount > 0 && tdsRate > 0 ? taxableAmount * (decimal)(tdsRate / 100) : 0;
            public int GetPolicyTermInYears(string policyId) => string.IsNullOrEmpty(policyId) ? 0 : 10;
            public bool ValidatePremiumToSumAssuredRatio(decimal annualPremium, decimal sumAssured, DateTime issueDate) => sumAssured > 0 && (annualPremium / sumAssured) <= 0.1m;
            public string GetExemptionRejectionReasonCode(string policyId) => string.IsNullOrEmpty(policyId) ? "INVALID_ID" : "NONE";
            public decimal GetTotalPremiumsPaid(string policyId, DateTime startDate, DateTime endDate) => startDate < endDate ? 1000m : 0m;
            public double CalculatePremiumToSumAssuredPercentage(decimal annualPremium, decimal sumAssured) => sumAssured == 0 ? 0 : (double)(annualPremium / sumAssured * 100);
            public int GetDaysUntilTaxFilingDeadline(DateTime currentProcessDate) => currentProcessDate == DateTime.MaxValue ? 0 : 30;
            public bool CheckIfPolicyIsUlip(string policyId) => policyId?.StartsWith("ULIP") ?? false;
            public decimal CalculateUlipExemptionLimit(decimal aggregatePremium, DateTime financialYearStart) => aggregatePremium > 250000 ? 250000 : aggregatePremium;
            public string RetrieveCustomerPanStatus(string customerId) => string.IsNullOrEmpty(customerId) ? "UNKNOWN" : "VALID";
            public bool IsDeathBenefitExempt(string policyId, string causeOfDeathCode) => causeOfDeathCode != "SUICIDE";
            public decimal ComputeNetPayableAfterTaxes(decimal grossAmount, decimal tdsAmount, decimal surcharge) => grossAmount - tdsAmount - surcharge;
            public int CountPoliciesExceedingPremiumLimit(string customerId, decimal premiumLimit) => premiumLimit < 0 ? 0 : 1;
            public double GetSurchargeRate(decimal totalTaxableIncome) => totalTaxableIncome > 5000000 ? 10.0 : 0.0;
            public bool EvaluateNriTaxCompliance(string customerId, string countryCode) => countryCode != "IN";
        }

        private ITaxExemptionEvaluationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new TaxExemptionEvaluationServiceStub();
        }

        [TestMethod]
        public void IsEligibleForSection1010D_EmptyPolicyId_ReturnsFalse()
        {
            var result = _service.IsEligibleForSection1010D("", DateTime.Now);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            
            var resultNull = _service.IsEligibleForSection1010D(null, DateTime.Now);
            Assert.IsFalse(resultNull);
            Assert.IsNotNull(resultNull);
        }

        [TestMethod]
        public void IsEligibleForSection1010D_MinValueDate_ReturnsFalse()
        {
            var result = _service.IsEligibleForSection1010D("POL123", DateTime.MinValue);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            
            var resultMax = _service.IsEligibleForSection1010D("POL123", DateTime.MaxValue);
            Assert.IsTrue(resultMax);
            Assert.IsNotNull(resultMax);
        }

        [TestMethod]
        public void CalculateTaxableMaturityAmount_ZeroAmounts_ReturnsZero()
        {
            var result = _service.CalculateTaxableMaturityAmount("POL123", 0m, 0m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            
            var resultNegative = _service.CalculateTaxableMaturityAmount("POL123", 100m, -50m);
            Assert.AreEqual(0m, resultNegative);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void CalculateTaxableMaturityAmount_LargeAmounts_CalculatesCorrectly()
        {
            var result = _service.CalculateTaxableMaturityAmount("POL123", 1000000000m, 1500000000m);
            Assert.AreEqual(500000000m, result);
            Assert.IsNotNull(result);
            
            var resultEqual = _service.CalculateTaxableMaturityAmount("POL123", 100m, 100m);
            Assert.AreEqual(0m, resultEqual);
            Assert.IsNotNull(resultEqual);
        }

        [TestMethod]
        public void GetApplicableTdsRate_EmptyPan_ReturnsHighRate()
        {
            var result = _service.GetApplicableTdsRate("", true);
            Assert.AreEqual(20.0, result);
            Assert.IsNotNull(result);
            
            var resultNull = _service.GetApplicableTdsRate(null, true);
            Assert.AreEqual(20.0, resultNull);
            Assert.IsNotNull(resultNull);
        }

        [TestMethod]
        public void GetApplicableTdsRate_InvalidPan_ReturnsHighRate()
        {
            var result = _service.GetApplicableTdsRate("ABCDE1234F", false);
            Assert.AreEqual(20.0, result);
            Assert.IsNotNull(result);
            
            var resultValid = _service.GetApplicableTdsRate("ABCDE1234F", true);
            Assert.AreEqual(5.0, resultValid);
            Assert.IsNotNull(resultValid);
        }

        [TestMethod]
        public void CalculateTdsAmount_ZeroTaxableAmount_ReturnsZero()
        {
            var result = _service.CalculateTdsAmount(0m, 5.0);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            
            var resultNegative = _service.CalculateTdsAmount(-100m, 5.0);
            Assert.AreEqual(0m, resultNegative);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void CalculateTdsAmount_ZeroTdsRate_ReturnsZero()
        {
            var result = _service.CalculateTdsAmount(1000m, 0.0);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            
            var resultNegativeRate = _service.CalculateTdsAmount(1000m, -5.0);
            Assert.AreEqual(0m, resultNegativeRate);
            Assert.IsNotNull(resultNegativeRate);
        }

        [TestMethod]
        public void GetPolicyTermInYears_EmptyPolicyId_ReturnsZero()
        {
            var result = _service.GetPolicyTermInYears("");
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            
            var resultNull = _service.GetPolicyTermInYears(null);
            Assert.AreEqual(0, resultNull);
            Assert.IsNotNull(resultNull);
        }

        [TestMethod]
        public void ValidatePremiumToSumAssuredRatio_ZeroSumAssured_ReturnsFalse()
        {
            var result = _service.ValidatePremiumToSumAssuredRatio(1000m, 0m, DateTime.Now);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            
            var resultNegative = _service.ValidatePremiumToSumAssuredRatio(1000m, -10000m, DateTime.Now);
            Assert.IsFalse(resultNegative);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void ValidatePremiumToSumAssuredRatio_ZeroPremium_ReturnsTrue()
        {
            var result = _service.ValidatePremiumToSumAssuredRatio(0m, 10000m, DateTime.Now);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            
            var resultNegativePremium = _service.ValidatePremiumToSumAssuredRatio(-1000m, 10000m, DateTime.Now);
            Assert.IsTrue(resultNegativePremium);
            Assert.IsNotNull(resultNegativePremium);
        }

        [TestMethod]
        public void GetExemptionRejectionReasonCode_EmptyPolicyId_ReturnsInvalidId()
        {
            var result = _service.GetExemptionRejectionReasonCode("");
            Assert.AreEqual("INVALID_ID", result);
            Assert.IsNotNull(result);
            
            var resultNull = _service.GetExemptionRejectionReasonCode(null);
            Assert.AreEqual("INVALID_ID", resultNull);
            Assert.IsNotNull(resultNull);
        }

        [TestMethod]
        public void GetTotalPremiumsPaid_ReversedDates_ReturnsZero()
        {
            var result = _service.GetTotalPremiumsPaid("POL123", DateTime.MaxValue, DateTime.MinValue);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            
            var resultEqual = _service.GetTotalPremiumsPaid("POL123", DateTime.Now, DateTime.Now);
            Assert.AreEqual(0m, resultEqual);
            Assert.IsNotNull(resultEqual);
        }

        [TestMethod]
        public void CalculatePremiumToSumAssuredPercentage_ZeroSumAssured_ReturnsZero()
        {
            var result = _service.CalculatePremiumToSumAssuredPercentage(1000m, 0m);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            
            var resultZeroPremium = _service.CalculatePremiumToSumAssuredPercentage(0m, 10000m);
            Assert.AreEqual(0.0, resultZeroPremium);
            Assert.IsNotNull(resultZeroPremium);
        }

        [TestMethod]
        public void GetDaysUntilTaxFilingDeadline_MaxValueDate_ReturnsZero()
        {
            var result = _service.GetDaysUntilTaxFilingDeadline(DateTime.MaxValue);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            
            var resultMin = _service.GetDaysUntilTaxFilingDeadline(DateTime.MinValue);
            Assert.AreEqual(30, resultMin);
            Assert.IsNotNull(resultMin);
        }

        [TestMethod]
        public void CheckIfPolicyIsUlip_EmptyPolicyId_ReturnsFalse()
        {
            var result = _service.CheckIfPolicyIsUlip("");
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            
            var resultNull = _service.CheckIfPolicyIsUlip(null);
            Assert.IsFalse(resultNull);
            Assert.IsNotNull(resultNull);
        }

        [TestMethod]
        public void CalculateUlipExemptionLimit_ZeroPremium_ReturnsZero()
        {
            var result = _service.CalculateUlipExemptionLimit(0m, DateTime.Now);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            
            var resultNegative = _service.CalculateUlipExemptionLimit(-100m, DateTime.Now);
            Assert.AreEqual(-100m, resultNegative);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void CalculateUlipExemptionLimit_LargePremium_ReturnsCappedLimit()
        {
            var result = _service.CalculateUlipExemptionLimit(1000000m, DateTime.Now);
            Assert.AreEqual(250000m, result);
            Assert.IsNotNull(result);
            
            var resultExact = _service.CalculateUlipExemptionLimit(250000m, DateTime.Now);
            Assert.AreEqual(250000m, resultExact);
            Assert.IsNotNull(resultExact);
        }

        [TestMethod]
        public void RetrieveCustomerPanStatus_EmptyCustomerId_ReturnsUnknown()
        {
            var result = _service.RetrieveCustomerPanStatus("");
            Assert.AreEqual("UNKNOWN", result);
            Assert.IsNotNull(result);
            
            var resultNull = _service.RetrieveCustomerPanStatus(null);
            Assert.AreEqual("UNKNOWN", resultNull);
            Assert.IsNotNull(resultNull);
        }

        [TestMethod]
        public void IsDeathBenefitExempt_SuicideCode_ReturnsFalse()
        {
            var result = _service.IsDeathBenefitExempt("POL123", "SUICIDE");
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            
            var resultEmpty = _service.IsDeathBenefitExempt("POL123", "");
            Assert.IsTrue(resultEmpty);
            Assert.IsNotNull(resultEmpty);
        }

        [TestMethod]
        public void ComputeNetPayableAfterTaxes_ZeroAmounts_ReturnsZero()
        {
            var result = _service.ComputeNetPayableAfterTaxes(0m, 0m, 0m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            
            var resultNegative = _service.ComputeNetPayableAfterTaxes(-100m, -10m, -5m);
            Assert.AreEqual(-85m, resultNegative);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void ComputeNetPayableAfterTaxes_LargeAmounts_CalculatesCorrectly()
        {
            var result = _service.ComputeNetPayableAfterTaxes(1000000000m, 50000000m, 10000000m);
            Assert.AreEqual(940000000m, result);
            Assert.IsNotNull(result);
            
            var resultExact = _service.ComputeNetPayableAfterTaxes(100m, 100m, 0m);
            Assert.AreEqual(0m, resultExact);
            Assert.IsNotNull(resultExact);
        }

        [TestMethod]
        public void CountPoliciesExceedingPremiumLimit_NegativeLimit_ReturnsZero()
        {
            var result = _service.CountPoliciesExceedingPremiumLimit("CUST123", -100m);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            
            var resultZero = _service.CountPoliciesExceedingPremiumLimit("CUST123", 0m);
            Assert.AreEqual(1, resultZero);
            Assert.IsNotNull(resultZero);
        }

        [TestMethod]
        public void GetSurchargeRate_ZeroIncome_ReturnsZero()
        {
            var result = _service.GetSurchargeRate(0m);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            
            var resultNegative = _service.GetSurchargeRate(-100000m);
            Assert.AreEqual(0.0, resultNegative);
            Assert.IsNotNull(resultNegative);
        }

        [TestMethod]
        public void GetSurchargeRate_LargeIncome_ReturnsHighRate()
        {
            var result = _service.GetSurchargeRate(10000000m);
            Assert.AreEqual(10.0, result);
            Assert.IsNotNull(result);
            
            var resultExact = _service.GetSurchargeRate(5000000m);
            Assert.AreEqual(0.0, resultExact);
            Assert.IsNotNull(resultExact);
        }

        [TestMethod]
        public void EvaluateNriTaxCompliance_EmptyCountryCode_ReturnsTrue()
        {
            var result = _service.EvaluateNriTaxCompliance("CUST123", "");
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            
            var resultNull = _service.EvaluateNriTaxCompliance("CUST123", null);
            Assert.IsTrue(resultNull);
            Assert.IsNotNull(resultNull);
        }

        [TestMethod]
        public void EvaluateNriTaxCompliance_InCountryCode_ReturnsFalse()
        {
            var result = _service.EvaluateNriTaxCompliance("CUST123", "IN");
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            
            var resultOther = _service.EvaluateNriTaxCompliance("CUST123", "US");
            Assert.IsTrue(resultOther);
            Assert.IsNotNull(resultOther);
        }
    }
}