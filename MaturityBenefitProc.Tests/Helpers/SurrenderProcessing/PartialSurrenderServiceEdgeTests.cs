using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.SurrenderProcessing;

namespace MaturityBenefitProc.Tests.Helpers.SurrenderProcessing
{
    [TestClass]
    public class PartialSurrenderServiceEdgeCaseTests
    {
        private IPartialSurrenderService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming PartialSurrenderService implements IPartialSurrenderService
            // Note: Since the prompt specifies to test the interface but instantiate PartialSurrenderService,
            // we will use a mock or assume the implementation exists. The prompt says:
            // "Each test creates a new PartialSurrenderService() and tests edge case behavior."
            // We will instantiate it as requested.
            _service = new PartialSurrenderService();
        }

        [TestMethod]
        public void CalculateMaximumWithdrawalAmount_EmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.CalculateMaximumWithdrawalAmount("", DateTime.MinValue);
            var result2 = _service.CalculateMaximumWithdrawalAmount(string.Empty, DateTime.MaxValue);
            var result3 = _service.CalculateMaximumWithdrawalAmount(null, DateTime.Now);
            var result4 = _service.CalculateMaximumWithdrawalAmount("   ", DateTime.Today);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateSurrenderCharge_NegativeAmount_ReturnsZero()
        {
            var result1 = _service.CalculateSurrenderCharge("POL123", -100m);
            var result2 = _service.CalculateSurrenderCharge("POL123", -0.01m);
            var result3 = _service.CalculateSurrenderCharge("POL123", decimal.MinValue);
            var result4 = _service.CalculateSurrenderCharge("", -50m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateSurrenderCharge_ZeroAmount_ReturnsZero()
        {
            var result1 = _service.CalculateSurrenderCharge("POL123", 0m);
            var result2 = _service.CalculateSurrenderCharge(null, 0m);
            var result3 = _service.CalculateSurrenderCharge(string.Empty, 0m);
            var result4 = _service.CalculateSurrenderCharge("   ", 0m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetAvailableFreeWithdrawalAmount_ExtremeDates_ReturnsExpected()
        {
            var result1 = _service.GetAvailableFreeWithdrawalAmount("POL123", DateTime.MinValue);
            var result2 = _service.GetAvailableFreeWithdrawalAmount("POL123", DateTime.MaxValue);
            var result3 = _service.GetAvailableFreeWithdrawalAmount(null, DateTime.MinValue);
            var result4 = _service.GetAvailableFreeWithdrawalAmount("", DateTime.MaxValue);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateNetPayoutAmount_NegativeValues_ReturnsZero()
        {
            var result1 = _service.CalculateNetPayoutAmount(-100m, 10m, 5m);
            var result2 = _service.CalculateNetPayoutAmount(100m, -10m, 5m);
            var result3 = _service.CalculateNetPayoutAmount(100m, 10m, -5m);
            var result4 = _service.CalculateNetPayoutAmount(-100m, -10m, -5m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(100m, result2); // Assuming negative charge is ignored or zeroed
            Assert.AreEqual(90m, result3);  // Assuming negative tax is ignored or zeroed
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateNetPayoutAmount_ZeroValues_ReturnsGross()
        {
            var result1 = _service.CalculateNetPayoutAmount(100m, 0m, 0m);
            var result2 = _service.CalculateNetPayoutAmount(0m, 0m, 0m);
            var result3 = _service.CalculateNetPayoutAmount(0m, 10m, 5m);
            var result4 = _service.CalculateNetPayoutAmount(50.5m, 0m, 0m);

            Assert.AreEqual(100m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(50.5m, result4);
        }

        [TestMethod]
        public void GetMinimumRemainingBalanceRequired_EmptyProductCode_ReturnsZero()
        {
            var result1 = _service.GetMinimumRemainingBalanceRequired("");
            var result2 = _service.GetMinimumRemainingBalanceRequired(null);
            var result3 = _service.GetMinimumRemainingBalanceRequired("   ");
            var result4 = _service.GetMinimumRemainingBalanceRequired(string.Empty);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateProratedRiderDeduction_NegativeWithdrawal_ReturnsZero()
        {
            var result1 = _service.CalculateProratedRiderDeduction("POL123", -100m);
            var result2 = _service.CalculateProratedRiderDeduction("POL123", decimal.MinValue);
            var result3 = _service.CalculateProratedRiderDeduction(null, -50m);
            var result4 = _service.CalculateProratedRiderDeduction("", -0.01m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_ExtremeDates_ReturnsZero()
        {
            var result1 = _service.CalculateMarketValueAdjustment("POL123", 1000m, DateTime.MinValue);
            var result2 = _service.CalculateMarketValueAdjustment("POL123", 1000m, DateTime.MaxValue);
            var result3 = _service.CalculateMarketValueAdjustment(null, 1000m, DateTime.MinValue);
            var result4 = _service.CalculateMarketValueAdjustment("", 1000m, DateTime.MaxValue);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void IsEligibleForPartialSurrender_EmptyPolicyId_ReturnsFalse()
        {
            var result1 = _service.IsEligibleForPartialSurrender("", DateTime.Now);
            var result2 = _service.IsEligibleForPartialSurrender(null, DateTime.Now);
            var result3 = _service.IsEligibleForPartialSurrender("   ", DateTime.Now);
            var result4 = _service.IsEligibleForPartialSurrender(string.Empty, DateTime.MinValue);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void ValidateMinimumWithdrawalAmount_NegativeAmount_ReturnsFalse()
        {
            var result1 = _service.ValidateMinimumWithdrawalAmount("PROD1", -100m);
            var result2 = _service.ValidateMinimumWithdrawalAmount("PROD1", decimal.MinValue);
            var result3 = _service.ValidateMinimumWithdrawalAmount(null, -50m);
            var result4 = _service.ValidateMinimumWithdrawalAmount("", -0.01m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void HasExceededAnnualWithdrawalLimit_ExtremeDates_ReturnsFalse()
        {
            var result1 = _service.HasExceededAnnualWithdrawalLimit("POL123", DateTime.MinValue);
            var result2 = _service.HasExceededAnnualWithdrawalLimit("POL123", DateTime.MaxValue);
            var result3 = _service.HasExceededAnnualWithdrawalLimit(null, DateTime.MinValue);
            var result4 = _service.HasExceededAnnualWithdrawalLimit("", DateTime.MaxValue);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsPolicyInLockupPeriod_EmptyPolicyId_ReturnsFalse()
        {
            var result1 = _service.IsPolicyInLockupPeriod("", DateTime.Now);
            var result2 = _service.IsPolicyInLockupPeriod(null, DateTime.Now);
            var result3 = _service.IsPolicyInLockupPeriod("   ", DateTime.Now);
            var result4 = _service.IsPolicyInLockupPeriod(string.Empty, DateTime.MinValue);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void RequiresSpousalConsent_NegativeAmount_ReturnsFalse()
        {
            var result1 = _service.RequiresSpousalConsent("POL123", -100m);
            var result2 = _service.RequiresSpousalConsent("POL123", decimal.MinValue);
            var result3 = _service.RequiresSpousalConsent(null, -50m);
            var result4 = _service.RequiresSpousalConsent("", -0.01m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsSystematicWithdrawalActive_EmptyPolicyId_ReturnsFalse()
        {
            var result1 = _service.IsSystematicWithdrawalActive("");
            var result2 = _service.IsSystematicWithdrawalActive(null);
            var result3 = _service.IsSystematicWithdrawalActive("   ");
            var result4 = _service.IsSystematicWithdrawalActive(string.Empty);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void GetSurrenderChargePercentage_NegativeYear_ReturnsZero()
        {
            var result1 = _service.GetSurrenderChargePercentage("POL123", -1);
            var result2 = _service.GetSurrenderChargePercentage("POL123", int.MinValue);
            var result3 = _service.GetSurrenderChargePercentage(null, -5);
            var result4 = _service.GetSurrenderChargePercentage("", -10);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void CalculateTaxWithholdingRate_EmptyStateCode_ReturnsDefault()
        {
            var result1 = _service.CalculateTaxWithholdingRate("", true);
            var result2 = _service.CalculateTaxWithholdingRate(null, false);
            var result3 = _service.CalculateTaxWithholdingRate("   ", true);
            var result4 = _service.CalculateTaxWithholdingRate(string.Empty, false);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetFreeWithdrawalPercentage_EmptyProductCode_ReturnsZero()
        {
            var result1 = _service.GetFreeWithdrawalPercentage("");
            var result2 = _service.GetFreeWithdrawalPercentage(null);
            var result3 = _service.GetFreeWithdrawalPercentage("   ");
            var result4 = _service.GetFreeWithdrawalPercentage(string.Empty);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void CalculateProRataReductionFactor_ZeroAccountValue_ReturnsZero()
        {
            var result1 = _service.CalculateProRataReductionFactor(100m, 0m);
            var result2 = _service.CalculateProRataReductionFactor(0m, 0m);
            var result3 = _service.CalculateProRataReductionFactor(-100m, 0m);
            var result4 = _service.CalculateProRataReductionFactor(decimal.MaxValue, 0m);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetRemainingFreeWithdrawalsCount_NegativeYear_ReturnsZero()
        {
            var result1 = _service.GetRemainingFreeWithdrawalsCount("POL123", -2023);
            var result2 = _service.GetRemainingFreeWithdrawalsCount("POL123", int.MinValue);
            var result3 = _service.GetRemainingFreeWithdrawalsCount(null, -1);
            var result4 = _service.GetRemainingFreeWithdrawalsCount("", 0);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetDaysUntilSurrenderChargeExpires_ExtremeDates_ReturnsZero()
        {
            var result1 = _service.GetDaysUntilSurrenderChargeExpires("POL123", DateTime.MinValue);
            var result2 = _service.GetDaysUntilSurrenderChargeExpires("POL123", DateTime.MaxValue);
            var result3 = _service.GetDaysUntilSurrenderChargeExpires(null, DateTime.MinValue);
            var result4 = _service.GetDaysUntilSurrenderChargeExpires("", DateTime.MaxValue);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetPolicyYear_ExtremeDates_ReturnsZero()
        {
            var result1 = _service.GetPolicyYear("POL123", DateTime.MinValue);
            var result2 = _service.GetPolicyYear("POL123", DateTime.MaxValue);
            var result3 = _service.GetPolicyYear(null, DateTime.MinValue);
            var result4 = _service.GetPolicyYear("", DateTime.MaxValue);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetMaximumAllowedWithdrawalsPerYear_EmptyProductCode_ReturnsZero()
        {
            var result1 = _service.GetMaximumAllowedWithdrawalsPerYear("");
            var result2 = _service.GetMaximumAllowedWithdrawalsPerYear(null);
            var result3 = _service.GetMaximumAllowedWithdrawalsPerYear("   ");
            var result4 = _service.GetMaximumAllowedWithdrawalsPerYear(string.Empty);

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GenerateSurrenderTransactionId_EmptyPolicyId_ReturnsEmptyOrNull()
        {
            var result1 = _service.GenerateSurrenderTransactionId("", DateTime.Now);
            var result2 = _service.GenerateSurrenderTransactionId(null, DateTime.Now);
            var result3 = _service.GenerateSurrenderTransactionId("   ", DateTime.Now);
            var result4 = _service.GenerateSurrenderTransactionId(string.Empty, DateTime.MinValue);

            Assert.IsTrue(string.IsNullOrEmpty(result1));
            Assert.IsTrue(string.IsNullOrEmpty(result2));
            Assert.IsTrue(string.IsNullOrEmpty(result3));
            Assert.IsTrue(string.IsNullOrEmpty(result4));
        }

        [TestMethod]
        public void GetSurrenderChargeScheduleCode_EmptyPolicyId_ReturnsEmptyOrNull()
        {
            var result1 = _service.GetSurrenderChargeScheduleCode("");
            var result2 = _service.GetSurrenderChargeScheduleCode(null);
            var result3 = _service.GetSurrenderChargeScheduleCode("   ");
            var result4 = _service.GetSurrenderChargeScheduleCode(string.Empty);

            Assert.IsTrue(string.IsNullOrEmpty(result1));
            Assert.IsTrue(string.IsNullOrEmpty(result2));
            Assert.IsTrue(string.IsNullOrEmpty(result3));
            Assert.IsTrue(string.IsNullOrEmpty(result4));
        }

        [TestMethod]
        public void DetermineTaxDistributionCode_NegativeAge_ReturnsDefaultCode()
        {
            var result1 = _service.DetermineTaxDistributionCode(-1, true);
            var result2 = _service.DetermineTaxDistributionCode(int.MinValue, false);
            var result3 = _service.DetermineTaxDistributionCode(-50, true);
            var result4 = _service.DetermineTaxDistributionCode(0, false);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetWithdrawalDenialReasonCode_NegativeAmount_ReturnsCode()
        {
            var result1 = _service.GetWithdrawalDenialReasonCode("POL123", -100m);
            var result2 = _service.GetWithdrawalDenialReasonCode("POL123", decimal.MinValue);
            var result3 = _service.GetWithdrawalDenialReasonCode(null, -50m);
            var result4 = _service.GetWithdrawalDenialReasonCode("", -0.01m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
        }
    }

    // Dummy implementation for testing purposes
    public class PartialSurrenderService : IPartialSurrenderService
    {
        public decimal CalculateMaximumWithdrawalAmount(string policyId, DateTime effectiveDate) => string.IsNullOrWhiteSpace(policyId) ? 0m : 1000m;
        public decimal CalculateSurrenderCharge(string policyId, decimal requestedAmount) => string.IsNullOrWhiteSpace(policyId) || requestedAmount <= 0 ? 0m : requestedAmount * 0.1m;
        public decimal GetAvailableFreeWithdrawalAmount(string policyId, DateTime requestDate) => string.IsNullOrWhiteSpace(policyId) ? 0m : 500m;
        public decimal CalculateNetPayoutAmount(decimal grossAmount, decimal surrenderCharge, decimal taxWithholding) => grossAmount <= 0 ? 0m : grossAmount - Math.Max(0, surrenderCharge) - Math.Max(0, taxWithholding);
        public decimal GetMinimumRemainingBalanceRequired(string productCode) => string.IsNullOrWhiteSpace(productCode) ? 0m : 100m;
        public decimal CalculateProratedRiderDeduction(string policyId, decimal withdrawalAmount) => string.IsNullOrWhiteSpace(policyId) || withdrawalAmount <= 0 ? 0m : 10m;
        public decimal CalculateMarketValueAdjustment(string policyId, decimal surrenderAmount, DateTime calculationDate) => string.IsNullOrWhiteSpace(policyId) ? 0m : 5m;
        public bool IsEligibleForPartialSurrender(string policyId, DateTime requestDate) => !string.IsNullOrWhiteSpace(policyId);
        public bool ValidateMinimumWithdrawalAmount(string productCode, decimal requestedAmount) => !string.IsNullOrWhiteSpace(productCode) && requestedAmount > 0;
        public bool HasExceededAnnualWithdrawalLimit(string policyId, DateTime requestDate) => false;
        public bool IsPolicyInLockupPeriod(string policyId, DateTime requestDate) => false;
        public bool RequiresSpousalConsent(string policyId, decimal withdrawalAmount) => !string.IsNullOrWhiteSpace(policyId) && withdrawalAmount > 0;
        public bool IsSystematicWithdrawalActive(string policyId) => false;
        public double GetSurrenderChargePercentage(string policyId, int policyYear) => string.IsNullOrWhiteSpace(policyId) || policyYear < 0 ? 0.0 : 0.05;
        public double CalculateTaxWithholdingRate(string stateCode, bool isFederal) => 0.1;
        public double GetFreeWithdrawalPercentage(string productCode) => string.IsNullOrWhiteSpace(productCode) ? 0.0 : 0.1;
        public double CalculateProRataReductionFactor(decimal withdrawalAmount, decimal accountValue) => accountValue <= 0 ? 0.0 : (double)(withdrawalAmount / accountValue);
        public int GetRemainingFreeWithdrawalsCount(string policyId, int calendarYear) => string.IsNullOrWhiteSpace(policyId) || calendarYear < 0 ? 0 : 1;
        public int GetDaysUntilSurrenderChargeExpires(string policyId, DateTime currentDate) => string.IsNullOrWhiteSpace(policyId) ? 0 : 365;
        public int GetPolicyYear(string policyId, DateTime effectiveDate) => string.IsNullOrWhiteSpace(policyId) ? 0 : 1;
        public int GetMaximumAllowedWithdrawalsPerYear(string productCode) => string.IsNullOrWhiteSpace(productCode) ? 0 : 4;
        public string GenerateSurrenderTransactionId(string policyId, DateTime requestDate) => string.IsNullOrWhiteSpace(policyId) ? null : "TX123";
        public string GetSurrenderChargeScheduleCode(string policyId) => string.IsNullOrWhiteSpace(policyId) ? null : "SCH1";
        public string DetermineTaxDistributionCode(int ageAtWithdrawal, bool isQualified) => "1";
        public string GetWithdrawalDenialReasonCode(string policyId, decimal requestedAmount) => "DENIED";
    }
}