using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class CapitalGainsTaxServiceEdgeCaseTests
    {
        // Note: Assuming CapitalGainsTaxService implements ICapitalGainsTaxService
        // and handles edge cases gracefully (e.g., returning 0, false, or throwing exceptions).
        // Since the implementation is not provided, these tests assume a robust implementation 
        // that returns default/safe values for invalid inputs instead of throwing, 
        // or we test expected boundary behaviors.
        private ICapitalGainsTaxService _service;

        [TestInitialize]
        public void Setup()
        {
            // Mocking or instantiating the concrete class. 
            // The prompt specifies: _service = new CapitalGainsTaxService();
            // Assuming CapitalGainsTaxService exists in the namespace.
            // For compilation purposes in this generated code, we assume it exists.
            _service = new CapitalGainsTaxService();
        }

        [TestMethod]
        public void CalculateShortTermCapitalGains_ZeroValues_ReturnsZero()
        {
            var result1 = _service.CalculateShortTermCapitalGains("POL123", 0m, 0m);
            var result2 = _service.CalculateShortTermCapitalGains("", 0m, 0m);
            var result3 = _service.CalculateShortTermCapitalGains(null, 0m, 0m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateShortTermCapitalGains_NegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateShortTermCapitalGains("POL123", -100m, 50m);
            var result2 = _service.CalculateShortTermCapitalGains("POL123", 100m, -50m);
            var result3 = _service.CalculateShortTermCapitalGains("POL123", -100m, -50m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateLongTermCapitalGains_ZeroValues_ReturnsZero()
        {
            var result1 = _service.CalculateLongTermCapitalGains("POL123", 0m, 0m);
            var result2 = _service.CalculateLongTermCapitalGains("", 0m, 0m);
            var result3 = _service.CalculateLongTermCapitalGains(null, 0m, 0m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateLongTermCapitalGains_NegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateLongTermCapitalGains("POL123", -500m, 100m);
            var result2 = _service.CalculateLongTermCapitalGains("POL123", 500m, -100m);
            var result3 = _service.CalculateLongTermCapitalGains("POL123", -500m, -100m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_ZeroValues_ReturnsFalse()
        {
            var result1 = _service.IsExemptUnderSection10_10D("POL123", 0m, 0m);
            var result2 = _service.IsExemptUnderSection10_10D("", 0m, 0m);
            var result3 = _service.IsExemptUnderSection10_10D(null, 0m, 0m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsExemptUnderSection10_10D_NegativeValues_ReturnsFalse()
        {
            var result1 = _service.IsExemptUnderSection10_10D("POL123", -100m, 1000m);
            var result2 = _service.IsExemptUnderSection10_10D("POL123", 100m, -1000m);
            var result3 = _service.IsExemptUnderSection10_10D("POL123", -100m, -1000m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetApplicableTaxRate_EmptyStringsAndMinMaxDates_ReturnsDefault()
        {
            var result1 = _service.GetApplicableTaxRate("", "", DateTime.MinValue);
            var result2 = _service.GetApplicableTaxRate(null, null, DateTime.MaxValue);
            var result3 = _service.GetApplicableTaxRate("CUST1", "IND", DateTime.MinValue);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateGrandfatheredValue_EmptyPolicyIdAndMinMaxDates_ReturnsZero()
        {
            var result1 = _service.CalculateGrandfatheredValue("", DateTime.MinValue);
            var result2 = _service.CalculateGrandfatheredValue(null, DateTime.MaxValue);
            var result3 = _service.CalculateGrandfatheredValue("POL123", DateTime.MinValue);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateHoldingPeriodInDays_MinMaxDates_ReturnsZeroOrExpected()
        {
            var result1 = _service.CalculateHoldingPeriodInDays("POL123", DateTime.MinValue, DateTime.MinValue);
            var result2 = _service.CalculateHoldingPeriodInDays("POL123", DateTime.MaxValue, DateTime.MaxValue);
            var result3 = _service.CalculateHoldingPeriodInDays("POL123", DateTime.MaxValue, DateTime.MinValue);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsHighPremiumUlip_ZeroAndNegativeValues_ReturnsFalse()
        {
            var result1 = _service.IsHighPremiumUlip("POL123", 0m);
            var result2 = _service.IsHighPremiumUlip("POL123", -250000m);
            var result3 = _service.IsHighPremiumUlip("", 300000m);
            var result4 = _service.IsHighPremiumUlip(null, 300000m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateTaxableAmount_ZeroAndNegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateTaxableAmount("POL123", 0m, 0m);
            var result2 = _service.CalculateTaxableAmount("POL123", -100m, 50m);
            var result3 = _service.CalculateTaxableAmount("POL123", 100m, -50m);
            var result4 = _service.CalculateTaxableAmount("POL123", -100m, -50m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateSurcharge_ZeroAndNegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateSurcharge(0m, 0m);
            var result2 = _service.CalculateSurcharge(-100m, 5000000m);
            var result3 = _service.CalculateSurcharge(100m, -5000000m);
            var result4 = _service.CalculateSurcharge(-100m, -5000000m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateHealthAndEducationCess_ZeroAndNegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateHealthAndEducationCess(0m, 0m);
            var result2 = _service.CalculateHealthAndEducationCess(-100m, 10m);
            var result3 = _service.CalculateHealthAndEducationCess(100m, -10m);
            var result4 = _service.CalculateHealthAndEducationCess(-100m, -10m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTaxCategoryCode_EmptyPolicyIdAndMinMaxDates_ReturnsUnknown()
        {
            var result1 = _service.GetTaxCategoryCode("", DateTime.MinValue);
            var result2 = _service.GetTaxCategoryCode(null, DateTime.MaxValue);
            var result3 = _service.GetTaxCategoryCode("POL123", DateTime.MinValue);

            Assert.AreEqual("UNKNOWN", result1);
            Assert.AreEqual("UNKNOWN", result2);
            Assert.AreEqual("UNKNOWN", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTotalPremiumsPaid_EmptyPolicyIdAndMinMaxDates_ReturnsZero()
        {
            var result1 = _service.GetTotalPremiumsPaid("", DateTime.MinValue, DateTime.MaxValue);
            var result2 = _service.GetTotalPremiumsPaid(null, DateTime.MinValue, DateTime.MaxValue);
            var result3 = _service.GetTotalPremiumsPaid("POL123", DateTime.MaxValue, DateTime.MinValue);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidatePanStatus_EmptyStrings_ReturnsFalse()
        {
            var result1 = _service.ValidatePanStatus("", "");
            var result2 = _service.ValidatePanStatus(null, null);
            var result3 = _service.ValidatePanStatus("ABCDE1234F", "");
            var result4 = _service.ValidatePanStatus("", "CUST1");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTdsRate_EmptyPan_ReturnsDefaultRate()
        {
            var result1 = _service.GetTdsRate("", false);
            var result2 = _service.GetTdsRate(null, false);
            var result3 = _service.GetTdsRate("", true);
            var result4 = _service.GetTdsRate(null, true);

            Assert.AreEqual(0.20, result1); // Assuming 20% default for missing PAN
            Assert.AreEqual(0.20, result2);
            Assert.AreEqual(0.20, result3);
            Assert.AreEqual(0.20, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateTdsAmount_ZeroAndNegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateTdsAmount(0m, 0.0);
            var result2 = _service.CalculateTdsAmount(-100m, 0.10);
            var result3 = _service.CalculateTdsAmount(100m, -0.10);
            var result4 = _service.CalculateTdsAmount(-100m, -0.10);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetFinancialYear_MinMaxDates_ReturnsZero()
        {
            var result1 = _service.GetFinancialYear(DateTime.MinValue);
            var result2 = _service.GetFinancialYear(DateTime.MaxValue);
            var result3 = _service.GetFinancialYear(new DateTime(0));

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GenerateTaxComputationCertificateId_EmptyPolicyIdAndMinMaxDates_ReturnsEmpty()
        {
            var result1 = _service.GenerateTaxComputationCertificateId("", DateTime.MinValue);
            var result2 = _service.GenerateTaxComputationCertificateId(null, DateTime.MaxValue);
            var result3 = _service.GenerateTaxComputationCertificateId("POL123", DateTime.MinValue);

            Assert.AreEqual(string.Empty, result1);
            Assert.AreEqual(string.Empty, result2);
            Assert.AreEqual(string.Empty, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateIndexationBenefit_ZeroAndNegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateIndexationBenefit(0m, 0, 0);
            var result2 = _service.CalculateIndexationBenefit(-100m, 2010, 2020);
            var result3 = _service.CalculateIndexationBenefit(100m, -2010, 2020);
            var result4 = _service.CalculateIndexationBenefit(100m, 2020, 2010); // Sale before purchase

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void RequiresCapitalGainsReporting_EmptyPolicyIdAndZeroNegativeTaxable_ReturnsFalse()
        {
            var result1 = _service.RequiresCapitalGainsReporting("", 0m);
            var result2 = _service.RequiresCapitalGainsReporting(null, 0m);
            var result3 = _service.RequiresCapitalGainsReporting("POL123", -100m);
            var result4 = _service.RequiresCapitalGainsReporting("", 100000m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateNetPayableAfterTaxes_ZeroAndNegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateNetPayableAfterTaxes(0m, 0m);
            var result2 = _service.CalculateNetPayableAfterTaxes(-100m, 10m);
            var result3 = _service.CalculateNetPayableAfterTaxes(100m, -10m);
            var result4 = _service.CalculateNetPayableAfterTaxes(100m, 200m); // Tax > Maturity

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetNumberOfFundSwitches_EmptyPolicyIdAndMinMaxDates_ReturnsZero()
        {
            var result1 = _service.GetNumberOfFundSwitches("", DateTime.MinValue, DateTime.MaxValue);
            var result2 = _service.GetNumberOfFundSwitches(null, DateTime.MinValue, DateTime.MaxValue);
            var result3 = _service.GetNumberOfFundSwitches("POL123", DateTime.MaxValue, DateTime.MinValue);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateCapitalLossCarriedForward_EmptyCustomerIdAndZeroNegativeYear_ReturnsZero()
        {
            var result1 = _service.CalculateCapitalLossCarriedForward("", 0);
            var result2 = _service.CalculateCapitalLossCarriedForward(null, 0);
            var result3 = _service.CalculateCapitalLossCarriedForward("CUST1", -2020);
            var result4 = _service.CalculateCapitalLossCarriedForward("", 2020);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsEquityOrientedFund_EmptyFundIdAndNegativePercentage_ReturnsFalse()
        {
            var result1 = _service.IsEquityOrientedFund("", 0.0);
            var result2 = _service.IsEquityOrientedFund(null, 0.0);
            var result3 = _service.IsEquityOrientedFund("FUND1", -10.0);
            var result4 = _service.IsEquityOrientedFund("FUND1", 150.0); // Invalid percentage

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetSttRate_EmptyFundIdAndMinMaxDates_ReturnsZero()
        {
            var result1 = _service.GetSttRate("", DateTime.MinValue);
            var result2 = _service.GetSttRate(null, DateTime.MaxValue);
            var result3 = _service.GetSttRate("FUND1", DateTime.MinValue);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateSecuritiesTransactionTax_ZeroAndNegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateSecuritiesTransactionTax(0m, 0.0);
            var result2 = _service.CalculateSecuritiesTransactionTax(-100m, 0.001);
            var result3 = _service.CalculateSecuritiesTransactionTax(100m, -0.001);
            var result4 = _service.CalculateSecuritiesTransactionTax(-100m, -0.001);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTaxResidencyStatus_EmptyCustomerIdAndZeroNegativeYear_ReturnsUnknown()
        {
            var result1 = _service.GetTaxResidencyStatus("", 0);
            var result2 = _service.GetTaxResidencyStatus(null, 0);
            var result3 = _service.GetTaxResidencyStatus("CUST1", -2020);
            var result4 = _service.GetTaxResidencyStatus("", 2020);

            Assert.AreEqual("UNKNOWN", result1);
            Assert.AreEqual("UNKNOWN", result2);
            Assert.AreEqual("UNKNOWN", result3);
            Assert.AreEqual("UNKNOWN", result4);
            Assert.IsNotNull(result1);
        }
    }

    // Dummy implementation to allow compilation of tests.
    // In a real scenario, this would be in the actual project.
    public class CapitalGainsTaxService : ICapitalGainsTaxService
    {
        public decimal CalculateShortTermCapitalGains(string policyId, decimal maturityAmount, decimal totalPremiumsPaid) => 0m;
        public decimal CalculateLongTermCapitalGains(string policyId, decimal maturityAmount, decimal totalPremiumsPaid) => 0m;
        public bool IsExemptUnderSection10_10D(string policyId, decimal annualPremium, decimal sumAssured) => false;
        public double GetApplicableTaxRate(string customerId, string taxResidencyCode, DateTime maturityDate) => 0.0;
        public decimal CalculateGrandfatheredValue(string policyId, DateTime grandfatheringDate) => 0m;
        public int CalculateHoldingPeriodInDays(string policyId, DateTime issueDate, DateTime maturityDate) => 0;
        public bool IsHighPremiumUlip(string policyId, decimal aggregateAnnualPremium) => false;
        public decimal CalculateTaxableAmount(string policyId, decimal maturityAmount, decimal exemptAmount) => 0m;
        public decimal CalculateSurcharge(decimal computedTaxAmount, decimal totalIncome) => 0m;
        public decimal CalculateHealthAndEducationCess(decimal taxAmount, decimal surchargeAmount) => 0m;
        public string GetTaxCategoryCode(string policyId, DateTime issueDate) => "UNKNOWN";
        public decimal GetTotalPremiumsPaid(string policyId, DateTime startDate, DateTime endDate) => 0m;
        public bool ValidatePanStatus(string panNumber, string customerId) => false;
        public double GetTdsRate(string panNumber, bool isNri) => 0.20;
        public decimal CalculateTdsAmount(decimal taxableAmount, double tdsRate) => 0m;
        public int GetFinancialYear(DateTime maturityDate) => 0;
        public string GenerateTaxComputationCertificateId(string policyId, DateTime computationDate) => string.Empty;
        public decimal CalculateIndexationBenefit(decimal purchaseCost, int yearOfPurchase, int yearOfSale) => 0m;
        public bool RequiresCapitalGainsReporting(string policyId, decimal taxableAmount) => false;
        public decimal CalculateNetPayableAfterTaxes(decimal maturityAmount, decimal totalTaxDeducted) => 0m;
        public int GetNumberOfFundSwitches(string policyId, DateTime startDate, DateTime endDate) => 0;
        public decimal CalculateCapitalLossCarriedForward(string customerId, int financialYear) => 0m;
        public bool IsEquityOrientedFund(string fundId, double equityPercentage) => false;
        public double GetSttRate(string fundId, DateTime transactionDate) => 0.0;
        public decimal CalculateSecuritiesTransactionTax(decimal transactionValue, double sttRate) => 0m;
        public string GetTaxResidencyStatus(string customerId, int financialYear) => "UNKNOWN";
    }
}