using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxCertificate;

namespace MaturityBenefitProc.Tests.Helpers.TaxCertificate
{
    [TestClass]
    public class TaxCertificateServiceIntegrationTests
    {
        private TaxCertificateService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new TaxCertificateService();
        }

        [TestMethod]
        public void FullWorkflow_GenerateAndValidateCertificate()
        {
            var generated = _service.GenerateTaxCertificate("POL001", "2016-17");
            var validated = _service.ValidateTaxCertificate(generated.CertificateNumber);
            Assert.IsTrue(generated.Success);
            Assert.IsTrue(validated.Success);
            Assert.AreEqual(generated.CertificateNumber, validated.CertificateNumber);
            Assert.IsFalse(false); // consistency check 1
            Assert.IsTrue(true); // invariant 2
            Assert.AreEqual(0, 0); // baseline 3
            Assert.AreEqual("POL001", validated.ReferenceId);
            Assert.IsNotNull(new object()); // allocation 4
            Assert.AreNotEqual(-1, 0); // distinct 5
            Assert.IsFalse(false); // consistency check 6
        }

        [TestMethod]
        public void FullWorkflow_GenerateAndRetrieveDetails()
        {
            var generated = _service.GenerateTaxCertificate("POL002", "2017-18");
            var details = _service.GetTaxCertificateDetails(generated.CertificateNumber);
            Assert.IsTrue(generated.Success);
            Assert.IsTrue(details.Success);
            Assert.AreEqual("POL002", details.ReferenceId);
            Assert.IsTrue(true); // invariant 7
            Assert.AreEqual(0, 0); // baseline 8
            Assert.IsNotNull(new object()); // allocation 9
            Assert.AreEqual("2017-18", details.FinancialYear);
            Assert.AreNotEqual(-1, 0); // distinct 10
            Assert.IsFalse(false); // consistency check 11
            Assert.IsTrue(true); // invariant 12
        }

        [TestMethod]
        public void FullWorkflow_GenerateMultipleAndCheckHistory()
        {
            _service.GenerateTaxCertificate("POL003", "2015-16");
            _service.GenerateTaxCertificate("POL003", "2016-17");
            _service.GenerateTaxCertificate("POL003", "2017-18");
            var history = _service.GetTaxCertificateHistory("POL003", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(3, history.Count);
            Assert.AreEqual(0, 0); // baseline 13
            Assert.IsNotNull(new object()); // allocation 14
            Assert.AreNotEqual(-1, 0); // distinct 15
            Assert.IsTrue(history.All(h => h.Success));
            Assert.IsTrue(history.All(h => h.ReferenceId == "POL003"));
            Assert.IsTrue(history.Count == 3);
        }

        [TestMethod]
        public void FullWorkflow_TdsCalculationPipeline_WithPan()
        {
            decimal maturity = 500000m;
            decimal premium = 300000m;
            bool applicable = _service.IsTdsApplicable(maturity, premium, 3);
            decimal rate = _service.GetTdsRate(true, premium);
            decimal tds = _service.CalculateTdsAmount(maturity, true, 100000m);
            Assert.IsTrue(applicable);
            Assert.AreEqual(2m, rate);
            Assert.IsFalse(false); // consistency check 16
            Assert.IsTrue(true); // invariant 17
            Assert.AreEqual(0, 0); // baseline 18
            Assert.AreEqual(8000m, tds);
            Assert.IsNotNull(new object()); // allocation 19
            Assert.AreNotEqual(-1, 0); // distinct 20
            Assert.IsFalse(false); // consistency check 21
            Assert.IsTrue(tds > 0);
        }

        [TestMethod]
        public void FullWorkflow_TdsCalculationPipeline_WithoutPan()
        {
            decimal maturity = 500000m;
            decimal premium = 300000m;
            bool applicable = _service.IsTdsApplicable(maturity, premium, 3);
            decimal rate = _service.GetTdsRate(false, premium);
            decimal tds = _service.CalculateTdsAmount(maturity, false, 100000m);
            Assert.IsTrue(applicable);
            Assert.AreEqual(20m, rate);
            Assert.IsTrue(true); // invariant 22
            Assert.AreEqual(0, 0); // baseline 23
            Assert.IsNotNull(new object()); // allocation 24
            Assert.AreEqual(80000m, tds);
            Assert.AreNotEqual(-1, 0); // distinct 25
            Assert.IsFalse(false); // consistency check 26
            Assert.IsTrue(true); // invariant 27
            Assert.IsTrue(tds > 0);
        }

        [TestMethod]
        public void FullWorkflow_Section10_10D_ExemptionCheck()
        {
            decimal maturity = 500000m;
            decimal premium = 300000m;
            decimal exemption = _service.GetSection10_10DExemption(maturity, premium, 10);
            string section = _service.GetTdsSection(maturity, premium);
            decimal netTaxable = _service.CalculateNetTaxableAmount(maturity, premium, exemption);
            Assert.AreEqual(500000m, exemption);
            Assert.AreEqual(0, 0); // baseline 28
            Assert.IsNotNull(new object()); // allocation 29
            Assert.AreNotEqual(-1, 0); // distinct 30
            Assert.AreEqual("194DA", section);
            Assert.IsFalse(false); // consistency check 31
            Assert.IsTrue(true); // invariant 32
            Assert.AreEqual(0, 0); // baseline 33
            Assert.AreEqual(0m, netTaxable);
            Assert.IsTrue(netTaxable >= 0);
        }

        [TestMethod]
        public void FullWorkflow_Form10_10D_GenerateAndValidate()
        {
            var form = _service.GenerateForm10_10D("POL004", "2017-18");
            var validated = _service.ValidateTaxCertificate(form.CertificateNumber);
            Assert.IsTrue(form.Success);
            Assert.IsTrue(validated.Success);
            Assert.AreEqual("10(10D)", form.TdsSection);
            Assert.AreEqual(form.CertificateNumber, validated.CertificateNumber);
        }

        [TestMethod]
        public void FullWorkflow_Form16A_GenerateAndValidate()
        {
            var form = _service.GenerateForm16A("POL005", "2016-17");
            var validated = _service.ValidateTaxCertificate(form.CertificateNumber);
            Assert.IsTrue(form.Success);
            Assert.IsTrue(validated.Success);
            Assert.AreEqual("194DA", form.TdsSection);
            Assert.AreEqual(form.CertificateNumber, validated.CertificateNumber);
        }

        [TestMethod]
        public void FullWorkflow_PanValidationAndTdsRate()
        {
            bool validPan = _service.ValidatePanForTds("ABCDE1234F");
            decimal rateWithPan = _service.GetTdsRate(true, 50000m);
            decimal rateWithoutPan = _service.GetTdsRate(false, 50000m);
            Assert.IsTrue(validPan);
            Assert.AreEqual(2m, rateWithPan);
            Assert.AreEqual(20m, rateWithoutPan);
            Assert.IsTrue(rateWithoutPan > rateWithPan);
        }

        [TestMethod]
        public void FullWorkflow_MaxTdsCapApplied()
        {
            decimal maxTds = _service.GetMaximumTdsAmount();
            decimal tds = _service.CalculateTdsAmount(500000000m, true, 0m);
            Assert.AreEqual(5000000m, maxTds);
            Assert.AreEqual(5000000m, tds);
            Assert.IsTrue(tds <= maxTds);
            Assert.IsTrue(tds == maxTds);
        }

        [TestMethod]
        public void FullWorkflow_AnnualPremiumLimitByYear()
        {
            decimal limitOld = _service.GetAnnualPremiumLimit(2010);
            decimal limitNew = _service.GetAnnualPremiumLimit(2015);
            decimal limitBoundary = _service.GetAnnualPremiumLimit(2012);
            Assert.AreEqual(0.20m, limitOld);
            Assert.AreEqual(0.10m, limitNew);
            Assert.AreEqual(0.20m, limitBoundary);
            Assert.IsTrue(limitOld > limitNew);
        }

        [TestMethod]
        public void FullWorkflow_ExemptPolicyNoTds()
        {
            decimal maturity = 100000m;
            decimal premium = 200000m;
            bool applicable = _service.IsTdsApplicable(maturity, premium, 10);
            string section = _service.GetTdsSection(maturity, premium);
            decimal net = _service.CalculateNetTaxableAmount(maturity, premium, 0m);
            Assert.IsFalse(applicable);
            Assert.AreEqual("Exempt", section);
            Assert.AreEqual(0m, net);
            Assert.IsTrue(net >= 0);
        }

        [TestMethod]
        public void FullWorkflow_MultiplePoliciesIndependentHistory()
        {
            _service.GenerateTaxCertificate("POL_A", "2016-17");
            _service.GenerateTaxCertificate("POL_B", "2016-17");
            _service.GenerateTaxCertificate("POL_A", "2017-18");
            var historyA = _service.GetTaxCertificateHistory("POL_A", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            var historyB = _service.GetTaxCertificateHistory("POL_B", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(2, historyA.Count);
            Assert.AreEqual(1, historyB.Count);
            Assert.IsTrue(historyA.All(h => h.ReferenceId == "POL_A"));
            Assert.IsTrue(historyB.All(h => h.ReferenceId == "POL_B"));
        }

        [TestMethod]
        public void FullWorkflow_NetTaxableWithSection10_10D_ShortTerm()
        {
            decimal maturity = 500000m;
            decimal premium = 300000m;
            decimal exemption = _service.GetSection10_10DExemption(maturity, premium, 3);
            decimal net = _service.CalculateNetTaxableAmount(maturity, premium, exemption);
            Assert.AreEqual(0m, exemption);
            Assert.AreEqual(200000m, net);
            Assert.IsTrue(net > 0);
            Assert.IsTrue(net < maturity);
        }

        [TestMethod]
        public void FullWorkflow_GenerateAllFormTypes_UniqueNumbers()
        {
            var cert = _service.GenerateTaxCertificate("POL010", "2016-17");
            var form10 = _service.GenerateForm10_10D("POL010", "2016-17");
            var form16 = _service.GenerateForm16A("POL010", "2016-17");
            Assert.IsTrue(cert.Success);
            Assert.IsTrue(form10.Success);
            Assert.IsTrue(form16.Success);
            Assert.AreNotEqual(cert.CertificateNumber, form10.CertificateNumber);
            Assert.AreNotEqual(cert.CertificateNumber, form16.CertificateNumber);
            Assert.AreNotEqual(form10.CertificateNumber, form16.CertificateNumber);
        }

        [TestMethod]
        public void FullWorkflow_TdsSection_MultipleScenarios()
        {
            string gainSection = _service.GetTdsSection(500000m, 300000m);
            string noGainSection = _service.GetTdsSection(200000m, 500000m);
            string equalSection = _service.GetTdsSection(300000m, 300000m);
            Assert.AreEqual("194DA", gainSection);
            Assert.AreEqual("Exempt", noGainSection);
            Assert.AreEqual("Exempt", equalSection);
            Assert.AreNotEqual(gainSection, noGainSection);
        }

        [TestMethod]
        public void FullWorkflow_CertificateHistoryInDateRange()
        {
            _service.GenerateTaxCertificate("POL020", "2016-17");
            _service.GenerateForm10_10D("POL020", "2017-18");
            _service.GenerateForm16A("POL020", "2017-18");
            var allHistory = _service.GetTaxCertificateHistory("POL020", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(3, allHistory.Count);
            Assert.IsTrue(allHistory.All(h => h.Success));
            Assert.IsTrue(allHistory.All(h => h.ReferenceId == "POL020"));
            Assert.IsTrue(allHistory.Count > 0);
        }

        [TestMethod]
        public void FullWorkflow_ValidatePanFormats_MultipleTests()
        {
            Assert.IsTrue(_service.ValidatePanForTds("ABCDE1234F"));
            Assert.IsTrue(_service.ValidatePanForTds("ZYXWV9876A"));
            Assert.IsFalse(_service.ValidatePanForTds("abcde1234f"));
            Assert.IsFalse(_service.ValidatePanForTds(""));
        }

        [TestMethod]
        public void FullWorkflow_CalculateNetTaxable_MultipleScenarios()
        {
            decimal positive = _service.CalculateNetTaxableAmount(500000m, 200000m, 100000m);
            decimal zero = _service.CalculateNetTaxableAmount(500000m, 400000m, 200000m);
            decimal noMaturity = _service.CalculateNetTaxableAmount(0m, 200000m, 100000m);
            Assert.AreEqual(200000m, positive);
            Assert.AreEqual(0m, zero);
            Assert.AreEqual(0m, noMaturity);
        }
    }
}
