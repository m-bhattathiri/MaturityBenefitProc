using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxCertificate;

namespace MaturityBenefitProc.Tests.Helpers.TaxCertificate
{
    [TestClass]
    public class TaxCertificateServiceTests
    {
        private TaxCertificateService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new TaxCertificateService();
        }

        [TestMethod]
        public void GenerateTaxCertificate_ValidInputs_ReturnsSuccess()
        {
            var result = _service.GenerateTaxCertificate("POL001", "2016-17");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.CertificateNumber);
            Assert.AreEqual("POL001", result.ReferenceId);
            Assert.AreEqual("2016-17", result.FinancialYear);
        }

        [TestMethod]
        public void GenerateTaxCertificate_NullPolicy_ReturnsFalse()
        {
            var result = _service.GenerateTaxCertificate(null, "2016-17");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Policy number"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void GenerateTaxCertificate_EmptyFinancialYear_ReturnsFalse()
        {
            var result = _service.GenerateTaxCertificate("POL001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Financial year"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void GenerateTaxCertificate_WhitespacePolicy_ReturnsFalse()
        {
            var result = _service.GenerateTaxCertificate("   ", "2016-17");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Policy number"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void GenerateTaxCertificate_CertificateNumberStartsWithTAXCERT()
        {
            var result = _service.GenerateTaxCertificate("POL123", "2017-18");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.CertificateNumber.StartsWith("TAXCERT"));
            Assert.IsNotNull(result.ProcessedDate);
            Assert.AreEqual("2017-18", result.FinancialYear);
        }

        [TestMethod]
        public void GenerateTaxCertificate_MultipleCertificates_UniqueCertNumbers()
        {
            var result1 = _service.GenerateTaxCertificate("POL001", "2016-17");
            var result2 = _service.GenerateTaxCertificate("POL002", "2016-17");
            Assert.IsTrue(result1.Success);
            Assert.IsTrue(result2.Success);
            Assert.AreNotEqual(result1.CertificateNumber, result2.CertificateNumber);
            Assert.AreEqual("POL001", result1.ReferenceId);
        }

        [TestMethod]
        public void ValidateTaxCertificate_ExistingCert_ReturnsSuccess()
        {
            var generated = _service.GenerateTaxCertificate("POL100", "2017-18");
            var result = _service.ValidateTaxCertificate(generated.CertificateNumber);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Certificate is valid", result.Message);
            Assert.AreEqual(generated.CertificateNumber, result.CertificateNumber);
            Assert.AreEqual("POL100", result.ReferenceId);
        }

        [TestMethod]
        public void ValidateTaxCertificate_NonExistentCert_ReturnsFalse()
        {
            var result = _service.ValidateTaxCertificate("NONEXIST123");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("not found"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void ValidateTaxCertificate_NullInput_ReturnsFalse()
        {
            var result = _service.ValidateTaxCertificate(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void CalculateTdsAmount_WithPanCard_Calculates2Percent()
        {
            decimal result = _service.CalculateTdsAmount(200000m, true, 100000m);
            Assert.AreEqual(2000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 200000m);
            Assert.AreEqual(Math.Round(result, 2), result);
        }

        [TestMethod]
        public void CalculateTdsAmount_WithoutPanCard_Calculates20Percent()
        {
            decimal result = _service.CalculateTdsAmount(200000m, false, 100000m);
            Assert.AreEqual(20000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 200000m);
            Assert.AreEqual(Math.Round(result, 2), result);
        }

        [TestMethod]
        public void CalculateTdsAmount_BelowExemption_ReturnsZero()
        {
            decimal result = _service.CalculateTdsAmount(50000m, true, 100000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result < 0);
            Assert.AreEqual(Math.Round(result, 2), result);
        }

        [TestMethod]
        public void CalculateTdsAmount_ZeroAmount_ReturnsZero()
        {
            decimal result = _service.CalculateTdsAmount(0m, true, 100000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0m, result);
        }

        [TestMethod]
        public void CalculateTdsAmount_NegativeAmount_ReturnsZero()
        {
            decimal result = _service.CalculateTdsAmount(-5000m, true, 100000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result < 0);
            Assert.AreEqual(Math.Round(result, 2), result);
        }

        [TestMethod]
        public void GetTdsRate_WithPanCard_Returns2()
        {
            decimal rate = _service.GetTdsRate(true, 50000m);
            Assert.AreEqual(2m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate < 100);
            Assert.AreEqual(2m, rate);
        }

        [TestMethod]
        public void GetTdsRate_WithoutPanCard_Returns20()
        {
            decimal rate = _service.GetTdsRate(false, 50000m);
            Assert.AreEqual(20m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate < 100);
            Assert.AreEqual(20m, rate);
        }

        [TestMethod]
        public void GetTdsRate_ZeroPremiumWithPan_Returns2()
        {
            decimal rate = _service.GetTdsRate(true, 0m);
            Assert.AreEqual(2m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate <= 20m);
            Assert.AreEqual(2m, rate);
        }

        [TestMethod]
        public void IsTdsApplicable_GainWithShortTerm_ReturnsTrue()
        {
            bool applicable = _service.IsTdsApplicable(200000m, 100000m, 3);
            Assert.IsTrue(applicable);
            Assert.AreNotEqual(false, applicable);
            Assert.IsTrue(200000m > 100000m);
            Assert.IsTrue(3 < 5);
        }

        [TestMethod]
        public void IsTdsApplicable_NoGainLongTerm_ReturnsFalse()
        {
            bool applicable = _service.IsTdsApplicable(100000m, 100000m, 10);
            Assert.IsFalse(applicable);
            Assert.AreNotEqual(true, applicable);
            Assert.IsTrue(100000m <= 100000m);
            Assert.IsTrue(10 >= 5);
        }

        [TestMethod]
        public void IsTdsApplicable_ZeroAmounts_ReturnsFalse()
        {
            bool applicable = _service.IsTdsApplicable(0m, 0m, 5);
            Assert.IsFalse(applicable);
            Assert.AreNotEqual(true, applicable);
            Assert.AreEqual(0m, 0m);
            Assert.IsFalse(applicable);
        }

        [TestMethod]
        public void GenerateForm10_10D_ValidInput_ReturnsSuccess()
        {
            var result = _service.GenerateForm10_10D("POL200", "2017-18");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.CertificateNumber);
            Assert.IsTrue(result.CertificateNumber.StartsWith("F1010D"));
            Assert.AreEqual("10(10D)", result.TdsSection);
        }

        [TestMethod]
        public void GenerateForm10_10D_NullPolicy_ReturnsFalse()
        {
            var result = _service.GenerateForm10_10D(null, "2017-18");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Policy number"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void GenerateForm16A_ValidInput_ReturnsSuccess()
        {
            var result = _service.GenerateForm16A("POL300", "2016-17");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.CertificateNumber);
            Assert.IsTrue(result.CertificateNumber.StartsWith("F16A"));
            Assert.AreEqual("194DA", result.TdsSection);
        }

        [TestMethod]
        public void GenerateForm16A_EmptyYear_ReturnsFalse()
        {
            var result = _service.GenerateForm16A("POL300", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Financial year"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void GetSection10_10DExemption_LongTermLowPremium_FullExemption()
        {
            decimal exemption = _service.GetSection10_10DExemption(500000m, 300000m, 10);
            Assert.AreEqual(500000m, exemption);
            Assert.IsTrue(exemption > 0);
            Assert.IsTrue(exemption >= 300000m);
            Assert.AreEqual(500000m, exemption);
        }

        [TestMethod]
        public void GetSection10_10DExemption_ShortTerm_ZeroExemption()
        {
            decimal exemption = _service.GetSection10_10DExemption(500000m, 300000m, 3);
            Assert.AreEqual(0m, exemption);
            Assert.IsTrue(exemption >= 0);
            Assert.IsFalse(exemption > 0);
            Assert.AreEqual(0m, exemption);
        }

        [TestMethod]
        public void GetAnnualPremiumLimit_Before2012_Returns20Percent()
        {
            decimal limit = _service.GetAnnualPremiumLimit(2010);
            Assert.AreEqual(0.20m, limit);
            Assert.IsTrue(limit > 0);
            Assert.IsTrue(limit <= 1m);
            Assert.AreEqual(0.20m, limit);
        }

        [TestMethod]
        public void GetAnnualPremiumLimit_After2012_Returns10Percent()
        {
            decimal limit = _service.GetAnnualPremiumLimit(2015);
            Assert.AreEqual(0.10m, limit);
            Assert.IsTrue(limit > 0);
            Assert.IsTrue(limit < 0.20m);
            Assert.AreEqual(0.10m, limit);
        }

        [TestMethod]
        public void ValidatePanForTds_ValidPan_ReturnsTrue()
        {
            bool valid = _service.ValidatePanForTds("ABCDE1234F");
            Assert.IsTrue(valid);
            Assert.AreNotEqual(false, valid);
            Assert.IsTrue("ABCDE1234F".Length == 10);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_InvalidFormat_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("12345ABCDE");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue("12345ABCDE".Length == 10);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_NullPan_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds(null);
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsFalse(valid);
            Assert.AreEqual(false, valid);
        }

        [TestMethod]
        public void CalculateNetTaxableAmount_WithExemption_ReturnsCorrect()
        {
            decimal net = _service.CalculateNetTaxableAmount(500000m, 300000m, 50000m);
            Assert.AreEqual(150000m, net);
            Assert.IsTrue(net > 0);
            Assert.IsTrue(net < 500000m);
            Assert.AreEqual(Math.Round(net, 2), net);
        }

        [TestMethod]
        public void CalculateNetTaxableAmount_NegativeResult_ReturnsZero()
        {
            decimal net = _service.CalculateNetTaxableAmount(100000m, 200000m, 50000m);
            Assert.AreEqual(0m, net);
            Assert.IsTrue(net >= 0);
            Assert.IsFalse(net < 0);
            Assert.AreEqual(0m, net);
        }

        [TestMethod]
        public void GetMaximumTdsAmount_Returns5Million()
        {
            decimal max = _service.GetMaximumTdsAmount();
            Assert.AreEqual(5000000m, max);
            Assert.IsTrue(max > 0);
            Assert.IsTrue(max == 5000000m);
            Assert.AreEqual(5000000m, max);
        }

        [TestMethod]
        public void GetTdsSection_GainPresent_Returns194DA()
        {
            string section = _service.GetTdsSection(200000m, 100000m);
            Assert.AreEqual("194DA", section);
            Assert.IsNotNull(section);
            Assert.IsTrue(section.Length > 0);
            Assert.AreEqual("194DA", section);
        }

        [TestMethod]
        public void GetTdsSection_NoGain_ReturnsExempt()
        {
            string section = _service.GetTdsSection(100000m, 200000m);
            Assert.AreEqual("Exempt", section);
            Assert.IsNotNull(section);
            Assert.IsTrue(section.Length > 0);
            Assert.AreEqual("Exempt", section);
        }

        [TestMethod]
        public void GetTaxCertificateDetails_ExistingCert_ReturnsDetails()
        {
            var generated = _service.GenerateTaxCertificate("POL500", "2016-17");
            var details = _service.GetTaxCertificateDetails(generated.CertificateNumber);
            Assert.IsTrue(details.Success);
            Assert.AreEqual("POL500", details.ReferenceId);
            Assert.AreEqual("2016-17", details.FinancialYear);
            Assert.IsNotNull(details.CertificateNumber);
        }

        [TestMethod]
        public void GetTaxCertificateDetails_NonExistent_ReturnsFalse()
        {
            var details = _service.GetTaxCertificateDetails("INVALID");
            Assert.IsFalse(details.Success);
            Assert.IsNotNull(details.Message);
            Assert.IsTrue(details.Message.Contains("not found"));
            Assert.IsNull(details.CertificateNumber);
        }

        [TestMethod]
        public void GetTaxCertificateHistory_WithRecords_ReturnsList()
        {
            _service.GenerateTaxCertificate("POL600", "2016-17");
            _service.GenerateTaxCertificate("POL600", "2017-18");
            var history = _service.GetTaxCertificateHistory("POL600", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.IsNotNull(history);
            Assert.AreEqual(2, history.Count);
            Assert.IsTrue(history.All(h => h.ReferenceId == "POL600"));
            Assert.IsTrue(history.Count > 0);
        }

        [TestMethod]
        public void GetTaxCertificateHistory_NoRecords_ReturnsEmpty()
        {
            var history = _service.GetTaxCertificateHistory("NOPOL", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.IsNotNull(history);
            Assert.AreEqual(0, history.Count);
            Assert.IsFalse(history.Any());
            Assert.IsTrue(history.Count == 0);
        }
    }
}
