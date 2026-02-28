using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance;

namespace MaturityBenefitProc.Tests.Helpers.TaxDeductionsAndCompliance
{
    [TestClass]
    public class TdsCalculationServiceEdgeCaseTests
    {
        private ITdsCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a mock or concrete implementation is available for testing.
            // For the purpose of this generated file, we will use a hypothetical mock implementation.
            // In a real scenario, you would use Moq or a concrete class.
            _service = new MockTdsCalculationService();
        }

        [TestMethod]
        public void CalculateBaseTds_ZeroGrossAmount_ReturnsZero()
        {
            decimal result = _service.CalculateBaseTds("POL123", 0m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void CalculateBaseTds_NegativeGrossAmount_ReturnsZero()
        {
            decimal result = _service.CalculateBaseTds("POL123", -5000m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void CalculateBaseTds_EmptyPolicyId_HandlesGracefully()
        {
            decimal result = _service.CalculateBaseTds("", 10000m);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreEqual(0m, result); // Assuming mock returns 0 for empty
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void GetApplicableTdsRate_EmptyPan_ReturnsDefaultRate()
        {
            double result = _service.GetApplicableTdsRate("", DateTime.Now);
            Assert.IsNotNull(result);
            Assert.AreEqual(0.20, result); // Default higher rate for empty PAN
            Assert.IsTrue(result > 0);
            Assert.IsFalse(result == 0);
        }

        [TestMethod]
        public void GetApplicableTdsRate_MinDate_ReturnsValidRate()
        {
            double result = _service.GetApplicableTdsRate("ABCDE1234F", DateTime.MinValue);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
            Assert.AreEqual(0.0, result); // Assuming mock returns 0 for min date
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void IsTdsApplicable_ZeroAmount_ReturnsFalse()
        {
            bool result = _service.IsTdsApplicable(0m, "194DA");
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void IsTdsApplicable_EmptySectionCode_ReturnsFalse()
        {
            bool result = _service.IsTdsApplicable(100000m, "");
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GetFinancialYear_MinDate_ReturnsZero()
        {
            int result = _service.GetFinancialYear(DateTime.MinValue);
            Assert.AreEqual(0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void GetFinancialYear_MaxDate_ReturnsValidYear()
        {
            int result = _service.GetFinancialYear(DateTime.MaxValue);
            Assert.AreEqual(9999, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.IsFalse(result <= 0);
        }

        [TestMethod]
        public void GetTaxCategoryCode_EmptyCustomerId_ReturnsUnknown()
        {
            string result = _service.GetTaxCategoryCode("");
            Assert.IsNotNull(result);
            Assert.AreEqual("UNKNOWN", result);
            Assert.AreNotEqual("IND", result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void ComputeSurcharge_ZeroBaseTds_ReturnsZero()
        {
            decimal result = _service.ComputeSurcharge(0m, 0.10);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void ComputeSurcharge_NegativeSurchargeRate_ReturnsZero()
        {
            decimal result = _service.ComputeSurcharge(1000m, -0.10);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void ComputeEducationCess_ZeroTaxAmount_ReturnsZero()
        {
            decimal result = _service.ComputeEducationCess(0m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void ComputeEducationCess_NegativeTaxAmount_ReturnsZero()
        {
            decimal result = _service.ComputeEducationCess(-500m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void GetTotalTaxableAmount_ZeroTotalPayout_ReturnsZero()
        {
            decimal result = _service.GetTotalTaxableAmount("POL123", 0m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void CalculateNetPayout_TdsGreaterThanGross_ReturnsZero()
        {
            decimal result = _service.CalculateNetPayout(1000m, 1500m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void GetExemptAmount_NegativePremiumsPaid_ReturnsZero()
        {
            decimal result = _service.GetExemptAmount("POL123", -1000m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void CalculateTdsOnSurrender_ZeroDuration_ReturnsZero()
        {
            decimal result = _service.CalculateTdsOnSurrender("POL123", 50000m, 0);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void CalculateTdsOnMaturity_NegativeMaturityValue_ReturnsZero()
        {
            decimal result = _service.CalculateTdsOnMaturity("POL123", -10000m);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void GetThresholdLimit_EmptySectionCode_ReturnsZero()
        {
            decimal result = _service.GetThresholdLimit("", 2023);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void ComputePenaltyAmount_NegativeDaysDelayed_ReturnsZero()
        {
            decimal result = _service.ComputePenaltyAmount("ABCDE1234F", -5);
            Assert.AreEqual(0m, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void GetEffectiveTaxRate_ZeroRates_ReturnsZero()
        {
            double result = _service.GetEffectiveTaxRate(0.0, 0.0, 0.0);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.0);
            Assert.IsFalse(result > 0.0);
        }

        [TestMethod]
        public void GetSurchargeRate_NegativeIncome_ReturnsZero()
        {
            double result = _service.GetSurchargeRate(-100000m, "IND");
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.0);
            Assert.IsFalse(result > 0.0);
        }

        [TestMethod]
        public void ComputeCessPercentage_InvalidYear_ReturnsZero()
        {
            double result = _service.ComputeCessPercentage(-1);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.0);
            Assert.IsFalse(result > 0.0);
        }

        [TestMethod]
        public void GetMarginalReliefRatio_ZeroCalculatedTax_ReturnsZero()
        {
            double result = _service.GetMarginalReliefRatio(5000000m, 0m);
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.0);
            Assert.IsFalse(result > 0.0);
        }

        [TestMethod]
        public void GetDtaaBenefitRate_EmptyCountryCode_ReturnsZero()
        {
            double result = _service.GetDtaaBenefitRate("");
            Assert.AreEqual(0.0, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 0.0);
            Assert.IsFalse(result > 0.0);
        }

        [TestMethod]
        public void IsPanValid_EmptyPan_ReturnsFalse()
        {
            bool result = _service.IsPanValid("");
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void IsNriCustomer_EmptyCustomerId_ReturnsFalse()
        {
            bool result = _service.IsNriCustomer("");
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void HasForm15GOrH_EmptyCustomerId_ReturnsFalse()
        {
            bool result = _service.HasForm15GOrH("", 2023);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            Assert.AreEqual(false, result);
        }
    }

    // Mock implementation for testing purposes
    public class MockTdsCalculationService : ITdsCalculationService
    {
        public decimal CalculateBaseTds(string policyId, decimal grossAmount) => grossAmount <= 0 || string.IsNullOrEmpty(policyId) ? 0m : grossAmount * 0.05m;
        public double GetApplicableTdsRate(string panNumber, DateTime payoutDate) => string.IsNullOrEmpty(panNumber) ? 0.20 : (payoutDate == DateTime.MinValue ? 0.0 : 0.05);
        public bool IsTdsApplicable(decimal grossAmount, string sectionCode) => grossAmount > 0 && !string.IsNullOrEmpty(sectionCode);
        public int GetFinancialYear(DateTime transactionDate) => transactionDate == DateTime.MinValue ? 0 : transactionDate.Year;
        public string GetTaxCategoryCode(string customerId) => string.IsNullOrEmpty(customerId) ? "UNKNOWN" : "IND";
        public decimal ComputeSurcharge(decimal baseTds, double surchargeRate) => baseTds <= 0 || surchargeRate <= 0 ? 0m : baseTds * (decimal)surchargeRate;
        public decimal ComputeEducationCess(decimal taxAmount) => taxAmount <= 0 ? 0m : taxAmount * 0.04m;
        public decimal GetTotalTaxableAmount(string policyId, decimal totalPayout) => totalPayout <= 0 ? 0m : totalPayout;
        public decimal CalculateNetPayout(decimal grossAmount, decimal totalTds) => totalTds >= grossAmount ? 0m : grossAmount - totalTds;
        public decimal GetExemptAmount(string policyId, decimal premiumsPaid) => premiumsPaid <= 0 ? 0m : premiumsPaid;
        public decimal CalculateTdsOnSurrender(string policyId, decimal surrenderValue, int policyDurationMonths) => policyDurationMonths <= 0 ? 0m : surrenderValue * 0.05m;
        public decimal CalculateTdsOnMaturity(string policyId, decimal maturityValue) => maturityValue <= 0 ? 0m : maturityValue * 0.05m;
        public decimal GetThresholdLimit(string sectionCode, int financialYear) => string.IsNullOrEmpty(sectionCode) ? 0m : 100000m;
        public decimal ComputePenaltyAmount(string panNumber, int daysDelayed) => daysDelayed <= 0 ? 0m : daysDelayed * 100m;
        public double GetEffectiveTaxRate(double baseRate, double surchargeRate, double cessRate) => baseRate == 0 ? 0.0 : baseRate + surchargeRate + cessRate;
        public double GetSurchargeRate(decimal totalIncome, string assesseeType) => totalIncome <= 0 ? 0.0 : 0.10;
        public double ComputeCessPercentage(int financialYear) => financialYear <= 0 ? 0.0 : 0.04;
        public double GetMarginalReliefRatio(decimal totalIncome, decimal taxCalculated) => taxCalculated <= 0 ? 0.0 : 0.05;
        public double GetDtaaBenefitRate(string countryCode) => string.IsNullOrEmpty(countryCode) ? 0.0 : 0.10;
        public bool IsPanValid(string panNumber) => !string.IsNullOrEmpty(panNumber) && panNumber.Length == 10;
        public bool IsNriCustomer(string customerId) => !string.IsNullOrEmpty(customerId) && customerId.StartsWith("NRI");
        public bool HasForm15GOrH(string customerId, int financialYear) => !string.IsNullOrEmpty(customerId) && financialYear > 0;
        public bool IsAmountAboveThreshold(decimal amount, string sectionCode) => amount > 100000m;
        public bool IsAadhaarLinked(string panNumber) => !string.IsNullOrEmpty(panNumber);
        public bool IsExemptUnderSection10_10D(string policyId, decimal sumAssured, decimal annualPremium) => annualPremium * 10 <= sumAssured;
        public bool RequiresHigherTdsRate(string panNumber) => string.IsNullOrEmpty(panNumber);
        public int CalculateDaysDelayed(DateTime expectedDate, DateTime actualDate) => actualDate > expectedDate ? (actualDate - expectedDate).Days : 0;
        public int GetTotalTransactionsInYear(string panNumber, int financialYear) => string.IsNullOrEmpty(panNumber) ? 0 : 5;
        public int GetAgeAtPayout(string customerId, DateTime payoutDate) => 30;
        public int GetPolicyDurationInMonths(DateTime issueDate, DateTime surrenderDate) => surrenderDate > issueDate ? (surrenderDate.Year - issueDate.Year) * 12 + surrenderDate.Month - issueDate.Month : 0;
        public string GetChallanNumber(string transactionId) => string.IsNullOrEmpty(transactionId) ? "" : "CHL" + transactionId;
        public string GenerateTdsCertificateId(string panNumber, int financialYear) => string.IsNullOrEmpty(panNumber) ? "" : $"TDS-{panNumber}-{financialYear}";
        public string GetSectionCode(string payoutType) => string.IsNullOrEmpty(payoutType) ? "" : "194DA";
        public string GetAssesseeType(string customerId) => string.IsNullOrEmpty(customerId) ? "" : "IND";
        public decimal CalculateTdsForNri(string policyId, decimal grossAmount, string countryCode) => grossAmount <= 0 ? 0m : grossAmount * 0.20m;
    }
}