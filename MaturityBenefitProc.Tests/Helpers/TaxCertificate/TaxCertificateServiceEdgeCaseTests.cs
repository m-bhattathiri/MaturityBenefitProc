using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxCertificate;

namespace MaturityBenefitProc.Tests.Helpers.TaxCertificate
{
    [TestClass]
    public class TaxCertificateServiceEdgeCaseTests
    {
        private TaxCertificateService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new TaxCertificateService();
        }

        [TestMethod]
        public void CalculateTdsAmount_ExactExemptionLimit_ReturnsZero()
        {
            decimal result = _service.CalculateTdsAmount(100000m, true, 100000m);
            Assert.AreEqual(0m, result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result > 0);
            Assert.AreEqual(Math.Round(result, 2), result);
        }

        [TestMethod]
        public void CalculateTdsAmount_OneAboveExemption_ReturnsMinimalTds()
        {
            decimal result = _service.CalculateTdsAmount(100001m, true, 100000m);
            Assert.AreEqual(0.02m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result < 1m);
            Assert.AreEqual(Math.Round(result, 2), result);
        }

        [TestMethod]
        public void CalculateTdsAmount_VeryLargeAmount_CapsAtMaximum()
        {
            decimal result = _service.CalculateTdsAmount(500000000m, true, 0m);
            Assert.AreEqual(5000000m, result);
            Assert.IsTrue(result <= 5000000m);
            Assert.IsTrue(result > 0);
            Assert.AreEqual(Math.Round(result, 2), result);
        }

        [TestMethod]
        public void CalculateTdsAmount_ZeroExemption_TaxesFullAmount()
        {
            decimal result = _service.CalculateTdsAmount(100000m, true, 0m);
            Assert.AreEqual(2000m, result);
            Assert.IsTrue(result > 0);
            Assert.IsTrue(result == 2000m);
            Assert.AreEqual(Math.Round(result, 2), result);
        }

        [TestMethod]
        public void GetTdsRate_NegativePremium_ReturnsRateBasedOnPan()
        {
            decimal rate = _service.GetTdsRate(true, -5000m);
            Assert.AreEqual(2m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate < 100);
            Assert.AreEqual(2m, rate);
        }

        [TestMethod]
        public void GetTdsRate_ZeroPremiumNoPan_Returns20()
        {
            decimal rate = _service.GetTdsRate(false, 0m);
            Assert.AreEqual(20m, rate);
            Assert.IsTrue(rate > 0);
            Assert.IsTrue(rate <= 20);
            Assert.AreEqual(20m, rate);
        }

        [TestMethod]
        public void IsTdsApplicable_ExactlyEqualAmounts_LongTerm_ReturnsFalse()
        {
            bool applicable = _service.IsTdsApplicable(100000m, 100000m, 5);
            Assert.IsFalse(applicable);
            Assert.AreNotEqual(true, applicable);
            Assert.IsTrue(100000m == 100000m);
            Assert.IsFalse(applicable);
        }

        [TestMethod]
        public void IsTdsApplicable_OneRupeeGain_ShortTerm_ReturnsTrue()
        {
            bool applicable = _service.IsTdsApplicable(100001m, 100000m, 4);
            Assert.IsTrue(applicable);
            Assert.AreNotEqual(false, applicable);
            Assert.IsTrue(100001m > 100000m);
            Assert.IsTrue(4 < 5);
        }

        [TestMethod]
        public void IsTdsApplicable_NegativeMaturity_ReturnsFalse()
        {
            bool applicable = _service.IsTdsApplicable(-50000m, 100000m, 10);
            Assert.IsFalse(applicable);
            Assert.AreNotEqual(true, applicable);
            Assert.IsTrue(-50000m < 0);
            Assert.IsFalse(applicable);
        }

        [TestMethod]
        public void IsTdsApplicable_NegativePremium_ReturnsFalse()
        {
            bool applicable = _service.IsTdsApplicable(100000m, -50000m, 10);
            Assert.IsFalse(applicable);
            Assert.AreNotEqual(true, applicable);
            Assert.IsTrue(-50000m < 0);
            Assert.IsFalse(applicable);
        }

        [TestMethod]
        public void IsTdsApplicable_PolicyTermExactly5_WithGain_ReturnsTrue()
        {
            bool applicable = _service.IsTdsApplicable(200000m, 100000m, 5);
            Assert.IsTrue(applicable);
            Assert.AreNotEqual(false, applicable);
            Assert.IsTrue(200000m > 100000m);
            Assert.IsTrue(5 >= 5);
        }

        [TestMethod]
        public void GetSection10_10DExemption_ExactlyZeroGain_ReturnsFullExemption()
        {
            decimal exemption = _service.GetSection10_10DExemption(300000m, 300000m, 10);
            Assert.AreEqual(300000m, exemption);
            Assert.IsTrue(exemption > 0);
            Assert.IsTrue(exemption == 300000m);
            Assert.AreEqual(300000m, exemption);
        }

        [TestMethod]
        public void GetSection10_10DExemption_ZeroMaturity_ReturnsZero()
        {
            decimal exemption = _service.GetSection10_10DExemption(0m, 300000m, 10);
            Assert.AreEqual(0m, exemption);
            Assert.IsTrue(exemption >= 0);
            Assert.IsFalse(exemption > 0);
            Assert.AreEqual(0m, exemption);
        }

        [TestMethod]
        public void GetSection10_10DExemption_PolicyTerm4_ReturnsZero()
        {
            decimal exemption = _service.GetSection10_10DExemption(500000m, 300000m, 4);
            Assert.AreEqual(0m, exemption);
            Assert.IsTrue(exemption >= 0);
            Assert.IsFalse(exemption > 0);
            Assert.AreEqual(0m, exemption);
        }

        [TestMethod]
        public void GetAnnualPremiumLimit_Year2012_Returns20Percent()
        {
            decimal limit = _service.GetAnnualPremiumLimit(2012);
            Assert.AreEqual(0.20m, limit);
            Assert.IsTrue(limit > 0);
            Assert.IsTrue(limit == 0.20m);
            Assert.AreEqual(0.20m, limit);
        }

        [TestMethod]
        public void GetAnnualPremiumLimit_Year2013_Returns10Percent()
        {
            decimal limit = _service.GetAnnualPremiumLimit(2013);
            Assert.AreEqual(0.10m, limit);
            Assert.IsTrue(limit > 0);
            Assert.IsTrue(limit < 0.20m);
            Assert.AreEqual(0.10m, limit);
        }

        [TestMethod]
        public void GetAnnualPremiumLimit_ZeroYear_ReturnsZero()
        {
            decimal limit = _service.GetAnnualPremiumLimit(0);
            Assert.AreEqual(0m, limit);
            Assert.IsTrue(limit >= 0);
            Assert.IsFalse(limit > 0);
            Assert.AreEqual(0m, limit);
        }

        [TestMethod]
        public void GetAnnualPremiumLimit_NegativeYear_ReturnsZero()
        {
            decimal limit = _service.GetAnnualPremiumLimit(-1);
            Assert.AreEqual(0m, limit);
            Assert.IsTrue(limit >= 0);
            Assert.IsFalse(limit > 0);
            Assert.AreEqual(0m, limit);
        }

        [TestMethod]
        public void ValidatePanForTds_LowerCasePan_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("abcde1234f");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue("abcde1234f".Length == 10);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_ShortPan_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("ABCDE");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue("ABCDE".Length < 10);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_EmptyString_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue(string.IsNullOrEmpty(""));
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidatePanForTds_SpecialChars_ReturnsFalse()
        {
            bool valid = _service.ValidatePanForTds("ABC@E1234F");
            Assert.IsFalse(valid);
            Assert.AreNotEqual(true, valid);
            Assert.IsTrue("ABC@E1234F".Contains("@"));
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void CalculateNetTaxableAmount_ZeroExemption_ReturnsGain()
        {
            decimal net = _service.CalculateNetTaxableAmount(500000m, 300000m, 0m);
            Assert.AreEqual(200000m, net);
            Assert.IsTrue(net > 0);
            Assert.IsTrue(net == 200000m);
            Assert.AreEqual(Math.Round(net, 2), net);
        }

        [TestMethod]
        public void CalculateNetTaxableAmount_ZeroMaturity_ReturnsZero()
        {
            decimal net = _service.CalculateNetTaxableAmount(0m, 300000m, 50000m);
            Assert.AreEqual(0m, net);
            Assert.IsTrue(net >= 0);
            Assert.IsFalse(net > 0);
            Assert.AreEqual(0m, net);
        }

        [TestMethod]
        public void CalculateNetTaxableAmount_ExemptionExceedsGain_ReturnsZero()
        {
            decimal net = _service.CalculateNetTaxableAmount(500000m, 300000m, 250000m);
            Assert.AreEqual(0m, net);
            Assert.IsTrue(net >= 0);
            Assert.IsFalse(net < 0);
            Assert.AreEqual(0m, net);
        }

        [TestMethod]
        public void GetTdsSection_ZeroAmounts_ReturnsExempt()
        {
            string section = _service.GetTdsSection(0m, 0m);
            Assert.AreEqual("Exempt", section);
            Assert.IsNotNull(section);
            Assert.AreEqual("Exempt", section);
            Assert.IsTrue(section.Length > 0);
        }

        [TestMethod]
        public void GetTdsSection_NegativeAmounts_ReturnsExempt()
        {
            string section = _service.GetTdsSection(-1000m, -2000m);
            Assert.AreEqual("Exempt", section);
            Assert.IsNotNull(section);
            Assert.AreEqual("Exempt", section);
            Assert.IsTrue(section.Length > 0);
        }

        [TestMethod]
        public void GetTdsSection_EqualAmounts_ReturnsExempt()
        {
            string section = _service.GetTdsSection(100000m, 100000m);
            Assert.AreEqual("Exempt", section);
            Assert.IsNotNull(section);
            Assert.IsTrue(section == "Exempt");
            Assert.IsTrue(section.Length > 0);
        }

        [TestMethod]
        public void GetTaxCertificateHistory_NullPolicy_ReturnsEmpty()
        {
            var history = _service.GetTaxCertificateHistory(null, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
            Assert.IsNotNull(history);
            Assert.AreEqual(0, history.Count);
            Assert.IsFalse(history.Any());
            Assert.IsTrue(history.Count == 0);
        }

        [TestMethod]
        public void GetTaxCertificateHistory_DateRangeOutOfRange_ReturnsEmpty()
        {
            _service.GenerateTaxCertificate("POL700", "2016-17");
            var history = _service.GetTaxCertificateHistory("POL700", DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(20));
            Assert.IsNotNull(history);
            Assert.AreEqual(0, history.Count);
            Assert.IsFalse(history.Any());
            Assert.IsTrue(history.Count == 0);
        }
    }
}
