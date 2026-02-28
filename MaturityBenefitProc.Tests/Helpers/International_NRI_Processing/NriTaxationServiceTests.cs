using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.InternationalAndNriProcessing;

namespace MaturityBenefitProc.Tests.Helpers.InternationalAndNriProcessing
{
    [TestClass]
    public class NriTaxationServiceTests
    {
        private INriTaxationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation named NriTaxationService exists
            _service = new NriTaxationService();
        }

        [TestMethod]
        public void CalculateWithholdingTax_ValidInputs_ReturnsExpectedTax()
        {
            var result1 = _service.CalculateWithholdingTax("POL123", 10000m, "US");
            var result2 = _service.CalculateWithholdingTax("POL456", 50000m, "UK");
            var result3 = _service.CalculateWithholdingTax("POL789", 0m, "CA");

            Assert.IsNotNull(result1);
            Assert.AreEqual(1500m, result1); // Assuming 15% for US
            Assert.AreEqual(5000m, result2); // Assuming 10% for UK
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateWithholdingTax_InvalidCountry_ReturnsDefaultTax()
        {
            var result1 = _service.CalculateWithholdingTax("POL123", 10000m, "XX");
            var result2 = _service.CalculateWithholdingTax("POL456", 50000m, "");
            var result3 = _service.CalculateWithholdingTax("POL789", 1000m, null);

            Assert.AreEqual(3000m, result1); // Assuming 30% default
            Assert.AreEqual(15000m, result2);
            Assert.AreEqual(300m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetDtaaRate_ValidCountry_ReturnsCorrectRate()
        {
            var result1 = _service.GetDtaaRate("US", new DateTime(2023, 1, 1));
            var result2 = _service.GetDtaaRate("UK", new DateTime(2023, 1, 1));
            var result3 = _service.GetDtaaRate("SG", new DateTime(2023, 1, 1));

            Assert.AreEqual(15.0, result1);
            Assert.AreEqual(10.0, result2);
            Assert.AreEqual(12.5, result3);
            Assert.AreNotEqual(0.0, result1);
        }

        [TestMethod]
        public void GetDtaaRate_InvalidCountry_ReturnsZero()
        {
            var result1 = _service.GetDtaaRate("XX", new DateTime(2023, 1, 1));
            var result2 = _service.GetDtaaRate("", new DateTime(2023, 1, 1));
            var result3 = _service.GetDtaaRate(null, new DateTime(2023, 1, 1));

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsEligibleForDtaaBenefits_EligibleCustomer_ReturnsTrue()
        {
            var result1 = _service.IsEligibleForDtaaBenefits("CUST001", "US");
            var result2 = _service.IsEligibleForDtaaBenefits("CUST002", "UK");
            var result3 = _service.IsEligibleForDtaaBenefits("CUST003", "SG");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsEligibleForDtaaBenefits_IneligibleCustomer_ReturnsFalse()
        {
            var result1 = _service.IsEligibleForDtaaBenefits("CUST999", "US");
            var result2 = _service.IsEligibleForDtaaBenefits("CUST001", "XX");
            var result3 = _service.IsEligibleForDtaaBenefits("", "US");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTaxResidencyCertificateStatus_ValidCustomer_ReturnsStatus()
        {
            var result1 = _service.GetTaxResidencyCertificateStatus("CUST001");
            var result2 = _service.GetTaxResidencyCertificateStatus("CUST002");
            var result3 = _service.GetTaxResidencyCertificateStatus("CUST003");

            Assert.AreEqual("Valid", result1);
            Assert.AreEqual("Expired", result2);
            Assert.AreEqual("Pending", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTaxResidencyCertificateStatus_InvalidCustomer_ReturnsNotSubmitted()
        {
            var result1 = _service.GetTaxResidencyCertificateStatus("CUST999");
            var result2 = _service.GetTaxResidencyCertificateStatus("");
            var result3 = _service.GetTaxResidencyCertificateStatus(null);

            Assert.AreEqual("NotSubmitted", result1);
            Assert.AreEqual("NotSubmitted", result2);
            Assert.AreEqual("NotSubmitted", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetDaysPresentInCountry_ValidDates_ReturnsCorrectDays()
        {
            var start = new DateTime(2022, 4, 1);
            var end = new DateTime(2023, 3, 31);
            var result1 = _service.GetDaysPresentInCountry("CUST001", start, end);
            var result2 = _service.GetDaysPresentInCountry("CUST002", start, end);
            var result3 = _service.GetDaysPresentInCountry("CUST003", start, end);

            Assert.AreEqual(150, result1);
            Assert.AreEqual(45, result2);
            Assert.AreEqual(200, result3);
            Assert.AreNotEqual(0, result1);
        }

        [TestMethod]
        public void GetDaysPresentInCountry_InvalidDates_ReturnsZero()
        {
            var start = new DateTime(2023, 4, 1);
            var end = new DateTime(2022, 3, 31);
            var result1 = _service.GetDaysPresentInCountry("CUST001", start, end);
            var result2 = _service.GetDaysPresentInCountry("CUST001", DateTime.MinValue, DateTime.MinValue);
            var result3 = _service.GetDaysPresentInCountry("CUST001", DateTime.MaxValue, DateTime.MaxValue);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ApplySurchargeAndCess_ValidRates_ReturnsTotalTax()
        {
            var result1 = _service.ApplySurchargeAndCess(1000m, 0.10, 0.04);
            var result2 = _service.ApplySurchargeAndCess(5000m, 0.15, 0.04);
            var result3 = _service.ApplySurchargeAndCess(0m, 0.10, 0.04);

            Assert.AreEqual(1144m, result1); // 1000 + 100 (surcharge) = 1100 + 44 (cess)
            Assert.AreEqual(5980m, result2); // 5000 + 750 = 5750 + 230
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ApplySurchargeAndCess_ZeroRates_ReturnsBaseTax()
        {
            var result1 = _service.ApplySurchargeAndCess(1000m, 0.0, 0.0);
            var result2 = _service.ApplySurchargeAndCess(5000m, 0.0, 0.0);
            var result3 = _service.ApplySurchargeAndCess(100m, 0.0, 0.0);

            Assert.AreEqual(1000m, result1);
            Assert.AreEqual(5000m, result2);
            Assert.AreEqual(100m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidatePanStatus_ValidPan_ReturnsTrue()
        {
            var result1 = _service.ValidatePanStatus("ABCDE1234F");
            var result2 = _service.ValidatePanStatus("VWXYZ5678G");
            var result3 = _service.ValidatePanStatus("PQRST9012H");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidatePanStatus_InvalidPan_ReturnsFalse()
        {
            var result1 = _service.ValidatePanStatus("INVALID");
            var result2 = _service.ValidatePanStatus("");
            var result3 = _service.ValidatePanStatus(null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetFatcaDeclarationId_ValidCustomer_ReturnsId()
        {
            var result1 = _service.GetFatcaDeclarationId("CUST001");
            var result2 = _service.GetFatcaDeclarationId("CUST002");
            var result3 = _service.GetFatcaDeclarationId("CUST003");

            Assert.AreEqual("FATCA-001", result1);
            Assert.AreEqual("FATCA-002", result2);
            Assert.AreEqual("FATCA-003", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetFatcaDeclarationId_InvalidCustomer_ReturnsNull()
        {
            var result1 = _service.GetFatcaDeclarationId("CUST999");
            var result2 = _service.GetFatcaDeclarationId("");
            var result3 = _service.GetFatcaDeclarationId(null);

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
            Assert.AreNotEqual("FATCA-999", result1);
        }

        [TestMethod]
        public void CalculateNetMaturityAmount_ValidAmounts_ReturnsDifference()
        {
            var result1 = _service.CalculateNetMaturityAmount(10000m, 1500m);
            var result2 = _service.CalculateNetMaturityAmount(50000m, 5000m);
            var result3 = _service.CalculateNetMaturityAmount(1000m, 0m);

            Assert.AreEqual(8500m, result1);
            Assert.AreEqual(45000m, result2);
            Assert.AreEqual(1000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateNetMaturityAmount_TaxGreaterThanGross_ReturnsZero()
        {
            var result1 = _service.CalculateNetMaturityAmount(1000m, 1500m);
            var result2 = _service.CalculateNetMaturityAmount(5000m, 5000m);
            var result3 = _service.CalculateNetMaturityAmount(0m, 100m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetEffectiveTdsRate_ValidInputs_ReturnsRate()
        {
            var result1 = _service.GetEffectiveTdsRate("CUST001", 10000m);
            var result2 = _service.GetEffectiveTdsRate("CUST002", 50000m);
            var result3 = _service.GetEffectiveTdsRate("CUST003", 1000000m);

            Assert.AreEqual(10.0, result1);
            Assert.AreEqual(15.0, result2);
            Assert.AreEqual(20.0, result3);
            Assert.AreNotEqual(0.0, result1);
        }

        [TestMethod]
        public void GetRemainingValidityDaysForTrc_ValidId_ReturnsDays()
        {
            var result1 = _service.GetRemainingValidityDaysForTrc("TRC001");
            var result2 = _service.GetRemainingValidityDaysForTrc("TRC002");
            var result3 = _service.GetRemainingValidityDaysForTrc("TRC003");

            Assert.AreEqual(120, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(365, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CheckForm10FSubmission_Submitted_ReturnsTrue()
        {
            var result1 = _service.CheckForm10FSubmission("CUST001", new DateTime(2023, 1, 1));
            var result2 = _service.CheckForm10FSubmission("CUST002", new DateTime(2023, 1, 1));
            var result3 = _service.CheckForm10FSubmission("CUST003", new DateTime(2023, 1, 1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ComputeExchangeRateVariance_ValidInputs_ReturnsVariance()
        {
            var result1 = _service.ComputeExchangeRateVariance(1000m, 82.5);
            var result2 = _service.ComputeExchangeRateVariance(5000m, 80.0);
            var result3 = _service.ComputeExchangeRateVariance(0m, 82.5);

            Assert.AreEqual(82500m, result1);
            Assert.AreEqual(400000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ResolveTaxTreatyCode_ValidCountry_ReturnsCode()
        {
            var result1 = _service.ResolveTaxTreatyCode("US");
            var result2 = _service.ResolveTaxTreatyCode("UK");
            var result3 = _service.ResolveTaxTreatyCode("SG");

            Assert.AreEqual("TREATY-US", result1);
            Assert.AreEqual("TREATY-UK", result2);
            Assert.AreEqual("TREATY-SG", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsNriStatusConfirmed_Confirmed_ReturnsTrue()
        {
            var result1 = _service.IsNriStatusConfirmed("CUST001", DateTime.Now);
            var result2 = _service.IsNriStatusConfirmed("CUST002", DateTime.Now);
            var result3 = _service.IsNriStatusConfirmed("CUST003", DateTime.Now);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetMaximumMarginalReliefRate_ValidIncome_ReturnsRate()
        {
            var result1 = _service.GetMaximumMarginalReliefRate(5000000m);
            var result2 = _service.GetMaximumMarginalReliefRate(10000000m);
            var result3 = _service.GetMaximumMarginalReliefRate(0m);

            Assert.AreEqual(10.0, result1);
            Assert.AreEqual(15.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateCapitalGainsTax_ValidInputs_ReturnsTax()
        {
            var result1 = _service.CalculateCapitalGainsTax("POL123", 10000m, 400);
            var result2 = _service.CalculateCapitalGainsTax("POL456", 50000m, 200);
            var result3 = _service.CalculateCapitalGainsTax("POL789", 0m, 400);

            Assert.AreEqual(1000m, result1); // Long term
            Assert.AreEqual(7500m, result2); // Short term
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetHoldingPeriod_ValidDates_ReturnsDays()
        {
            var issue = new DateTime(2020, 1, 1);
            var maturity = new DateTime(2023, 1, 1);
            var result1 = _service.GetHoldingPeriod(issue, maturity);
            var result2 = _service.GetHoldingPeriod(issue, issue.AddDays(100));
            var result3 = _service.GetHoldingPeriod(issue, issue);

            Assert.AreEqual(1096, result1);
            Assert.AreEqual(100, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GenerateTdsCertificateNumber_ValidInputs_ReturnsCertificate()
        {
            var result1 = _service.GenerateTdsCertificateNumber("CUST001", "POL123");
            var result2 = _service.GenerateTdsCertificateNumber("CUST002", "POL456");
            var result3 = _service.GenerateTdsCertificateNumber("CUST003", "POL789");

            Assert.AreEqual("TDS-CUST001-POL123", result1);
            Assert.AreEqual("TDS-CUST002-POL456", result2);
            Assert.AreEqual("TDS-CUST003-POL789", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void VerifyOciCardValidity_ValidCard_ReturnsTrue()
        {
            var result1 = _service.VerifyOciCardValidity("OCI123456");
            var result2 = _service.VerifyOciCardValidity("OCI654321");
            var result3 = _service.VerifyOciCardValidity("OCI987654");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetRepatriableAmount_ValidInputs_ReturnsAmount()
        {
            var result1 = _service.GetRepatriableAmount("POL123", 10000m);
            var result2 = _service.GetRepatriableAmount("POL456", 50000m);
            var result3 = _service.GetRepatriableAmount("POL789", 0m);

            Assert.AreEqual(10000m, result1);
            Assert.AreEqual(50000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetNroAccountTdsRate_ValidBank_ReturnsRate()
        {
            var result1 = _service.GetNroAccountTdsRate("HDFC");
            var result2 = _service.GetNroAccountTdsRate("ICICI");
            var result3 = _service.GetNroAccountTdsRate("SBI");

            Assert.AreEqual(30.0, result1);
            Assert.AreEqual(30.0, result2);
            Assert.AreEqual(30.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CountPreviousTaxFilings_ValidCustomer_ReturnsCount()
        {
            var result1 = _service.CountPreviousTaxFilings("CUST001");
            var result2 = _service.CountPreviousTaxFilings("CUST002");
            var result3 = _service.CountPreviousTaxFilings("CUST003");

            Assert.AreEqual(5, result1);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void DetermineResidentialStatus_ValidDays_ReturnsStatus()
        {
            var result1 = _service.DetermineResidentialStatus(182, 365);
            var result2 = _service.DetermineResidentialStatus(60, 400);
            var result3 = _service.DetermineResidentialStatus(50, 300);

            Assert.AreEqual("Resident", result1);
            Assert.AreEqual("Resident", result2);
            Assert.AreEqual("Non-Resident", result3);
            Assert.IsNotNull(result1);
        }
    }
}