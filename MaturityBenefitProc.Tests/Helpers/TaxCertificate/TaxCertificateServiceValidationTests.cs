using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxCertificate;

namespace MaturityBenefitProc.Tests.Helpers.TaxCertificate
{
    [TestClass]
    public class TaxCertificateServiceValidationTests
    {
        private TaxCertificateService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new TaxCertificateService();
        }

        [TestMethod]
        public void ValidatePanForTds_ValidPanABCPK1234A_ReturnsTrue()
        {
            bool valid = _service.ValidatePanForTds("ABCPK1234A");
            Assert.IsTrue(valid);
            Assert.AreNotEqual(false, valid);
            Assert.IsTrue("ABCPK1234A".Length == 10);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_ValidPanZYXWV9876Z_ReturnsTrue()
        {
            bool valid = _service.ValidatePanForTds("ZYXWV9876Z");
            Assert.IsTrue(valid);
            Assert.AreNotEqual(false, valid);
            Assert.IsTrue("ZYXWV9876Z".Length == 10);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_ValidPanDJKPM5678N_ReturnsTrue()
        {
            bool valid = _service.ValidatePanForTds("DJKPM5678N");
            Assert.IsTrue(valid);
            Assert.AreNotEqual(false, valid);
            Assert.IsTrue("DJKPM5678N".Length == 10);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_TooLong_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("ABCDE12345F");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue("ABCDE12345F".Length > 10);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_NumbersFirst_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("1234ABCDEF");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue("1234ABCDEF".Length == 10);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_AllNumbers_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("1234567890");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue("1234567890".Length == 10);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_AllAlpha_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("ABCDEFGHIJ");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue("ABCDEFGHIJ".Length == 10);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_SpacesInPan_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("ABC E1234F");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue("ABC E1234F".Contains(" "));
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void GenerateTaxCertificate_PolicyWithSpecialChars_Succeeds()
        {
            var result = _service.GenerateTaxCertificate("POL-001/2017", "2016-17");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.CertificateNumber);
            Assert.AreEqual("POL-001/2017", result.ReferenceId);
            Assert.AreEqual("2016-17", result.FinancialYear);
        }

        [TestMethod]
        public void GenerateTaxCertificate_VeryLongPolicyNumber_Succeeds()
        {
            string longPolicy = new string('P', 100);
            var result = _service.GenerateTaxCertificate(longPolicy, "2016-17");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.CertificateNumber);
            Assert.AreEqual(longPolicy, result.ReferenceId);
            Assert.IsTrue(result.ReferenceId.Length == 100);
        }

        [TestMethod]
        public void ValidateTaxCertificate_EmptyString_ReturnsFalse()
        {
            var result = _service.ValidateTaxCertificate("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void ValidateTaxCertificate_WhitespaceString_ReturnsFalse()
        {
            var result = _service.ValidateTaxCertificate("   ");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void CalculateTdsAmount_DecimalPrecision_RoundsCorrectly()
        {
            decimal result = _service.CalculateTdsAmount(100003m, true, 100000m);
            Assert.AreEqual(0.06m, result);
            Assert.IsTrue(result > 0);
            Assert.AreEqual(Math.Round(result, 2), result);
            Assert.IsTrue(result < 1m);
        }

        [TestMethod]
        public void CalculateTdsAmount_LargeExemption_ReturnsZero()
        {
            decimal result = _service.CalculateTdsAmount(100000m, true, 999999m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(0m, result);
        }

        [TestMethod]
        public void GetTdsRate_LargePremiumWithPan_Returns2()
        {
            decimal rate = _service.GetTdsRate(true, 10000000m);
            Assert.AreEqual(2m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate == 2m);
            Assert.AreEqual(2m, rate);
        }

        [TestMethod]
        public void GetTdsRate_LargePremiumWithoutPan_Returns20()
        {
            decimal rate = _service.GetTdsRate(false, 10000000m);
            Assert.AreEqual(20m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate == 20m);
            Assert.AreEqual(20m, rate);
        }

        [TestMethod]
        public void IsTdsApplicable_LargeGainLongTerm_ReturnsTrue()
        {
            bool applicable = _service.IsTdsApplicable(10000000m, 100000m, 20);
            Assert.IsTrue(applicable);
            Assert.AreNotEqual(false, applicable);
            Assert.IsTrue(10000000m > 100000m);
            Assert.IsTrue(applicable);
        }

        [TestMethod]
        public void IsTdsApplicable_PolicyTermZero_WithGain_ReturnsTrue()
        {
            bool applicable = _service.IsTdsApplicable(200000m, 100000m, 0);
            Assert.IsTrue(applicable);
            Assert.AreNotEqual(false, applicable);
            Assert.IsTrue(0 < 5);
            Assert.IsTrue(applicable);
        }

        [TestMethod]
        public void GenerateForm10_10D_EmptyYear_ReturnsFalse()
        {
            var result = _service.GenerateForm10_10D("POL001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Financial year"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void GenerateForm10_10D_WhitespacePolicy_ReturnsFalse()
        {
            var result = _service.GenerateForm10_10D("   ", "2017-18");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Policy number"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void GenerateForm16A_NullPolicy_ReturnsFalse()
        {
            var result = _service.GenerateForm16A(null, "2016-17");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Policy number"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void GenerateForm16A_WhitespaceYear_ReturnsFalse()
        {
            var result = _service.GenerateForm16A("POL001", "   ");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Financial year"));
            Assert.IsNull(result.CertificateNumber);
        }

        [TestMethod]
        public void GetSection10_10DExemption_NegativeMaturity_ReturnsZero()
        {
            decimal exemption = _service.GetSection10_10DExemption(-500000m, 300000m, 10);
            Assert.AreEqual(0m, exemption);
            Assert.IsTrue(exemption >= 0);
            Assert.IsFalse(exemption > 0);
            Assert.AreEqual(0m, exemption);
        }

        [TestMethod]
        public void GetSection10_10DExemption_NegativePremium_ReturnsZero()
        {
            decimal exemption = _service.GetSection10_10DExemption(500000m, -300000m, 10);
            Assert.AreEqual(0m, exemption);
            Assert.IsTrue(exemption >= 0);
            Assert.IsFalse(exemption > 0);
            Assert.AreEqual(0m, exemption);
        }

        [TestMethod]
        public void CalculateNetTaxableAmount_NegativeMaturity_ReturnsZero()
        {
            decimal net = _service.CalculateNetTaxableAmount(-100000m, 50000m, 10000m);
            Assert.AreEqual(0m, net);
            Assert.IsTrue(net >= 0);
            Assert.IsFalse(net < 0);
            Assert.AreEqual(0m, net);
        }

        [TestMethod]
        public void CalculateNetTaxableAmount_AllZero_ReturnsZero()
        {
            decimal net = _service.CalculateNetTaxableAmount(0m, 0m, 0m);
            Assert.AreEqual(0m, net);
            Assert.IsTrue(net >= 0);
            Assert.IsFalse(net > 0);
            Assert.AreEqual(0m, net);
        }

        [TestMethod]
        public void GetTaxCertificateDetails_EmptyString_ReturnsFalse()
        {
            var details = _service.GetTaxCertificateDetails("");
            Assert.IsFalse(details.Success);
            Assert.IsNotNull(details.Message);
            Assert.IsTrue(details.Message.Contains("required"));
            Assert.IsNull(details.CertificateNumber);
        }

        [TestMethod]
        public void GetTaxCertificateDetails_NullInput_ReturnsFalse()
        {
            var details = _service.GetTaxCertificateDetails(null);
            Assert.IsFalse(details.Success);
            Assert.IsNotNull(details.Message);
            Assert.IsTrue(details.Message.Contains("required"));
            Assert.IsNull(details.CertificateNumber);
        }
    }
}
