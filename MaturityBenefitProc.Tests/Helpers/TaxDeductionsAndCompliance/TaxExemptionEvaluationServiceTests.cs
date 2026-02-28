using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class TaxExemptionEvaluationServiceTests
    {
        private ITaxExemptionEvaluationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation named TaxExemptionEvaluationService exists
            _service = new TaxExemptionEvaluationService();
        }

        [TestMethod]
        public void IsEligibleForSection1010D_ValidPolicy_ReturnsTrue()
        {
            var result1 = _service.IsEligibleForSection1010D("POL123", new DateTime(2015, 1, 1));
            var result2 = _service.IsEligibleForSection1010D("POL999", new DateTime(2010, 5, 10));
            var result3 = _service.IsEligibleForSection1010D("POL000", new DateTime(2020, 12, 31));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsEligibleForSection1010D_InvalidPolicy_ReturnsFalse()
        {
            var result1 = _service.IsEligibleForSection1010D("", new DateTime(2015, 1, 1));
            var result2 = _service.IsEligibleForSection1010D(null, new DateTime(2010, 5, 10));
            var result3 = _service.IsEligibleForSection1010D("INVALID", DateTime.MinValue);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateTaxableMaturityAmount_ValidInputs_ReturnsCorrectAmount()
        {
            var result1 = _service.CalculateTaxableMaturityAmount("POL123", 50000m, 100000m);
            var result2 = _service.CalculateTaxableMaturityAmount("POL456", 10000m, 15000m);
            var result3 = _service.CalculateTaxableMaturityAmount("POL789", 0m, 50000m);

            Assert.AreEqual(50000m, result1);
            Assert.AreEqual(5000m, result2);
            Assert.AreEqual(50000m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void CalculateTaxableMaturityAmount_Loss_ReturnsZero()
        {
            var result1 = _service.CalculateTaxableMaturityAmount("POL123", 100000m, 50000m);
            var result2 = _service.CalculateTaxableMaturityAmount("POL456", 15000m, 10000m);
            var result3 = _service.CalculateTaxableMaturityAmount("POL789", 50000m, 50000m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetApplicableTdsRate_ValidPan_ReturnsStandardRate()
        {
            var result1 = _service.GetApplicableTdsRate("ABCDE1234F", true);
            var result2 = _service.GetApplicableTdsRate("XYZAB5678C", true);
            var result3 = _service.GetApplicableTdsRate("PAN1234567", true);

            Assert.AreEqual(5.0, result1);
            Assert.AreEqual(5.0, result2);
            Assert.AreEqual(5.0, result3);
            Assert.AreNotEqual(20.0, result1);
        }

        [TestMethod]
        public void GetApplicableTdsRate_InvalidPan_ReturnsHighRate()
        {
            var result1 = _service.GetApplicableTdsRate("ABCDE1234F", false);
            var result2 = _service.GetApplicableTdsRate("", false);
            var result3 = _service.GetApplicableTdsRate(null, false);

            Assert.AreEqual(20.0, result1);
            Assert.AreEqual(20.0, result2);
            Assert.AreEqual(20.0, result3);
            Assert.AreNotEqual(5.0, result1);
        }

        [TestMethod]
        public void CalculateTdsAmount_ValidInputs_ReturnsCorrectAmount()
        {
            var result1 = _service.CalculateTdsAmount(100000m, 5.0);
            var result2 = _service.CalculateTdsAmount(50000m, 20.0);
            var result3 = _service.CalculateTdsAmount(0m, 5.0);

            Assert.AreEqual(5000m, result1);
            Assert.AreEqual(10000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreNotEqual(-1m, result1);
        }

        [TestMethod]
        public void CalculateTdsAmount_NegativeTaxableAmount_ReturnsZero()
        {
            var result1 = _service.CalculateTdsAmount(-10000m, 5.0);
            var result2 = _service.CalculateTdsAmount(-50000m, 20.0);
            var result3 = _service.CalculateTdsAmount(-1m, 10.0);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPolicyTermInYears_ValidPolicy_ReturnsTerm()
        {
            var result1 = _service.GetPolicyTermInYears("POL10");
            var result2 = _service.GetPolicyTermInYears("POL20");
            var result3 = _service.GetPolicyTermInYears("POL15");

            Assert.AreEqual(10, result1);
            Assert.AreEqual(20, result2);
            Assert.AreEqual(15, result3);
            Assert.AreNotEqual(0, result1);
        }

        [TestMethod]
        public void GetPolicyTermInYears_InvalidPolicy_ReturnsZero()
        {
            var result1 = _service.GetPolicyTermInYears("");
            var result2 = _service.GetPolicyTermInYears(null);
            var result3 = _service.GetPolicyTermInYears("INVALID");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidatePremiumToSumAssuredRatio_ValidRatio_ReturnsTrue()
        {
            var result1 = _service.ValidatePremiumToSumAssuredRatio(10000m, 150000m, new DateTime(2015, 1, 1));
            var result2 = _service.ValidatePremiumToSumAssuredRatio(5000m, 100000m, new DateTime(2010, 1, 1));
            var result3 = _service.ValidatePremiumToSumAssuredRatio(1000m, 20000m, new DateTime(2020, 1, 1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidatePremiumToSumAssuredRatio_InvalidRatio_ReturnsFalse()
        {
            var result1 = _service.ValidatePremiumToSumAssuredRatio(20000m, 100000m, new DateTime(2015, 1, 1));
            var result2 = _service.ValidatePremiumToSumAssuredRatio(30000m, 100000m, new DateTime(2010, 1, 1));
            var result3 = _service.ValidatePremiumToSumAssuredRatio(5000m, 10000m, new DateTime(2020, 1, 1));

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetExemptionRejectionReasonCode_ValidPolicy_ReturnsCode()
        {
            var result1 = _service.GetExemptionRejectionReasonCode("POL_REJ_1");
            var result2 = _service.GetExemptionRejectionReasonCode("POL_REJ_2");
            var result3 = _service.GetExemptionRejectionReasonCode("POL_REJ_3");

            Assert.AreEqual("EXCESS_PREMIUM", result1);
            Assert.AreEqual("INVALID_PAN", result2);
            Assert.AreEqual("TERM_TOO_SHORT", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetExemptionRejectionReasonCode_ValidExemptPolicy_ReturnsNone()
        {
            var result1 = _service.GetExemptionRejectionReasonCode("POL_EXEMPT");
            var result2 = _service.GetExemptionRejectionReasonCode("POL_OK");
            var result3 = _service.GetExemptionRejectionReasonCode("POL123");

            Assert.AreEqual("NONE", result1);
            Assert.AreEqual("NONE", result2);
            Assert.AreEqual("NONE", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTotalPremiumsPaid_ValidDates_ReturnsAmount()
        {
            var result1 = _service.GetTotalPremiumsPaid("POL123", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            var result2 = _service.GetTotalPremiumsPaid("POL456", new DateTime(2015, 1, 1), new DateTime(2020, 1, 1));
            var result3 = _service.GetTotalPremiumsPaid("POL789", new DateTime(2010, 1, 1), new DateTime(2011, 1, 1));

            Assert.AreEqual(10000m, result1);
            Assert.AreEqual(50000m, result2);
            Assert.AreEqual(12000m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void GetTotalPremiumsPaid_InvalidDates_ReturnsZero()
        {
            var result1 = _service.GetTotalPremiumsPaid("POL123", new DateTime(2021, 1, 1), new DateTime(2020, 1, 1));
            var result2 = _service.GetTotalPremiumsPaid("POL456", DateTime.MaxValue, DateTime.MinValue);
            var result3 = _service.GetTotalPremiumsPaid("", new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculatePremiumToSumAssuredPercentage_ValidInputs_ReturnsPercentage()
        {
            var result1 = _service.CalculatePremiumToSumAssuredPercentage(10000m, 100000m);
            var result2 = _service.CalculatePremiumToSumAssuredPercentage(5000m, 200000m);
            var result3 = _service.CalculatePremiumToSumAssuredPercentage(20000m, 100000m);

            Assert.AreEqual(10.0, result1);
            Assert.AreEqual(2.5, result2);
            Assert.AreEqual(20.0, result3);
            Assert.AreNotEqual(0.0, result1);
        }

        [TestMethod]
        public void CalculatePremiumToSumAssuredPercentage_ZeroSumAssured_ReturnsZero()
        {
            var result1 = _service.CalculatePremiumToSumAssuredPercentage(10000m, 0m);
            var result2 = _service.CalculatePremiumToSumAssuredPercentage(5000m, 0m);
            var result3 = _service.CalculatePremiumToSumAssuredPercentage(0m, 0m);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetDaysUntilTaxFilingDeadline_BeforeDeadline_ReturnsDays()
        {
            var result1 = _service.GetDaysUntilTaxFilingDeadline(new DateTime(2023, 7, 1));
            var result2 = _service.GetDaysUntilTaxFilingDeadline(new DateTime(2023, 7, 15));
            var result3 = _service.GetDaysUntilTaxFilingDeadline(new DateTime(2023, 7, 30));

            Assert.AreEqual(30, result1);
            Assert.AreEqual(16, result2);
            Assert.AreEqual(1, result3);
            Assert.AreNotEqual(0, result1);
        }

        [TestMethod]
        public void GetDaysUntilTaxFilingDeadline_AfterDeadline_ReturnsZero()
        {
            var result1 = _service.GetDaysUntilTaxFilingDeadline(new DateTime(2023, 8, 1));
            var result2 = _service.GetDaysUntilTaxFilingDeadline(new DateTime(2023, 12, 31));
            var result3 = _service.GetDaysUntilTaxFilingDeadline(new DateTime(2023, 7, 31));

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CheckIfPolicyIsUlip_UlipPolicy_ReturnsTrue()
        {
            var result1 = _service.CheckIfPolicyIsUlip("ULIP123");
            var result2 = _service.CheckIfPolicyIsUlip("ULIP456");
            var result3 = _service.CheckIfPolicyIsUlip("ULIP789");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CheckIfPolicyIsUlip_NonUlipPolicy_ReturnsFalse()
        {
            var result1 = _service.CheckIfPolicyIsUlip("TRAD123");
            var result2 = _service.CheckIfPolicyIsUlip("TERM456");
            var result3 = _service.CheckIfPolicyIsUlip("");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateUlipExemptionLimit_ValidInputs_ReturnsLimit()
        {
            var result1 = _service.CalculateUlipExemptionLimit(300000m, new DateTime(2021, 4, 1));
            var result2 = _service.CalculateUlipExemptionLimit(200000m, new DateTime(2021, 4, 1));
            var result3 = _service.CalculateUlipExemptionLimit(500000m, new DateTime(2021, 4, 1));

            Assert.AreEqual(250000m, result1);
            Assert.AreEqual(200000m, result2);
            Assert.AreEqual(250000m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void RetrieveCustomerPanStatus_ValidCustomer_ReturnsStatus()
        {
            var result1 = _service.RetrieveCustomerPanStatus("CUST123");
            var result2 = _service.RetrieveCustomerPanStatus("CUST456");
            var result3 = _service.RetrieveCustomerPanStatus("CUST789");

            Assert.AreEqual("VALID", result1);
            Assert.AreEqual("INVALID", result2);
            Assert.AreEqual("NOT_PROVIDED", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsDeathBenefitExempt_ValidCause_ReturnsTrue()
        {
            var result1 = _service.IsDeathBenefitExempt("POL123", "NATURAL");
            var result2 = _service.IsDeathBenefitExempt("POL456", "ACCIDENT");
            var result3 = _service.IsDeathBenefitExempt("POL789", "ILLNESS");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ComputeNetPayableAfterTaxes_ValidInputs_ReturnsNet()
        {
            var result1 = _service.ComputeNetPayableAfterTaxes(100000m, 5000m, 500m);
            var result2 = _service.ComputeNetPayableAfterTaxes(50000m, 1000m, 0m);
            var result3 = _service.ComputeNetPayableAfterTaxes(200000m, 20000m, 2000m);

            Assert.AreEqual(94500m, result1);
            Assert.AreEqual(49000m, result2);
            Assert.AreEqual(178000m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void CountPoliciesExceedingPremiumLimit_ValidCustomer_ReturnsCount()
        {
            var result1 = _service.CountPoliciesExceedingPremiumLimit("CUST123", 100000m);
            var result2 = _service.CountPoliciesExceedingPremiumLimit("CUST456", 50000m);
            var result3 = _service.CountPoliciesExceedingPremiumLimit("CUST789", 250000m);

            Assert.AreEqual(2, result1);
            Assert.AreEqual(3, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetSurchargeRate_ValidIncome_ReturnsRate()
        {
            var result1 = _service.GetSurchargeRate(6000000m);
            var result2 = _service.GetSurchargeRate(15000000m);
            var result3 = _service.GetSurchargeRate(3000000m);

            Assert.AreEqual(10.0, result1);
            Assert.AreEqual(15.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void EvaluateNriTaxCompliance_ValidNri_ReturnsTrue()
        {
            var result1 = _service.EvaluateNriTaxCompliance("CUST123", "US");
            var result2 = _service.EvaluateNriTaxCompliance("CUST456", "UK");
            var result3 = _service.EvaluateNriTaxCompliance("CUST789", "AE");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }
    }
}