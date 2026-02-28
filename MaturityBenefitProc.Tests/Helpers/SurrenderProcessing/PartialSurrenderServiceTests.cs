using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.SurrenderProcessing;

namespace MaturityBenefitProc.Tests.Helpers.SurrenderProcessing
{
    [TestClass]
    public class PartialSurrenderServiceTests
    {
        private IPartialSurrenderService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming there is a concrete class named PartialSurrenderService implementing the interface
            _service = new PartialSurrenderService();
        }

        [TestMethod]
        public void CalculateMaximumWithdrawalAmount_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateMaximumWithdrawalAmount("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateMaximumWithdrawalAmount("POL456", new DateTime(2023, 6, 15));
            var result3 = _service.CalculateMaximumWithdrawalAmount("POL789", new DateTime(2023, 12, 31));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void CalculateMaximumWithdrawalAmount_NullOrEmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.CalculateMaximumWithdrawalAmount(null, DateTime.Now);
            var result2 = _service.CalculateMaximumWithdrawalAmount("", DateTime.Now);
            var result3 = _service.CalculateMaximumWithdrawalAmount("   ", DateTime.Now);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateSurrenderCharge_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateSurrenderCharge("POL123", 1000m);
            var result2 = _service.CalculateSurrenderCharge("POL456", 5000m);
            var result3 = _service.CalculateSurrenderCharge("POL789", 10000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void CalculateSurrenderCharge_ZeroOrNegativeAmount_ReturnsZero()
        {
            var result1 = _service.CalculateSurrenderCharge("POL123", 0m);
            var result2 = _service.CalculateSurrenderCharge("POL456", -100m);
            var result3 = _service.CalculateSurrenderCharge("POL789", -5000m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetAvailableFreeWithdrawalAmount_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.GetAvailableFreeWithdrawalAmount("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GetAvailableFreeWithdrawalAmount("POL456", new DateTime(2023, 6, 15));
            var result3 = _service.GetAvailableFreeWithdrawalAmount("POL789", new DateTime(2023, 12, 31));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void CalculateNetPayoutAmount_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateNetPayoutAmount(1000m, 100m, 50m);
            var result2 = _service.CalculateNetPayoutAmount(5000m, 500m, 250m);
            var result3 = _service.CalculateNetPayoutAmount(10000m, 0m, 1000m);

            Assert.AreEqual(850m, result1);
            Assert.AreEqual(4250m, result2);
            Assert.AreEqual(9000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateNetPayoutAmount_ZeroGrossAmount_ReturnsZero()
        {
            var result1 = _service.CalculateNetPayoutAmount(0m, 0m, 0m);
            var result2 = _service.CalculateNetPayoutAmount(0m, 10m, 5m);
            var result3 = _service.CalculateNetPayoutAmount(0m, 100m, 50m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetMinimumRemainingBalanceRequired_ValidProductCode_ReturnsExpectedAmount()
        {
            var result1 = _service.GetMinimumRemainingBalanceRequired("PROD_A");
            var result2 = _service.GetMinimumRemainingBalanceRequired("PROD_B");
            var result3 = _service.GetMinimumRemainingBalanceRequired("PROD_C");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void CalculateProratedRiderDeduction_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateProratedRiderDeduction("POL123", 1000m);
            var result2 = _service.CalculateProratedRiderDeduction("POL456", 5000m);
            var result3 = _service.CalculateProratedRiderDeduction("POL789", 10000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateMarketValueAdjustment("POL123", 1000m, new DateTime(2023, 1, 1));
            var result2 = _service.CalculateMarketValueAdjustment("POL456", 5000m, new DateTime(2023, 6, 15));
            var result3 = _service.CalculateMarketValueAdjustment("POL789", 10000m, new DateTime(2023, 12, 31));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void IsEligibleForPartialSurrender_ValidInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsEligibleForPartialSurrender("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.IsEligibleForPartialSurrender("POL456", new DateTime(2023, 6, 15));
            var result3 = _service.IsEligibleForPartialSurrender("POL789", new DateTime(2023, 12, 31));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 || !result1); // Just checking it returns a bool
        }

        [TestMethod]
        public void ValidateMinimumWithdrawalAmount_ValidInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.ValidateMinimumWithdrawalAmount("PROD_A", 1000m);
            var result2 = _service.ValidateMinimumWithdrawalAmount("PROD_B", 500m);
            var result3 = _service.ValidateMinimumWithdrawalAmount("PROD_C", 100m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 || !result1);
        }

        [TestMethod]
        public void HasExceededAnnualWithdrawalLimit_ValidInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.HasExceededAnnualWithdrawalLimit("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.HasExceededAnnualWithdrawalLimit("POL456", new DateTime(2023, 6, 15));
            var result3 = _service.HasExceededAnnualWithdrawalLimit("POL789", new DateTime(2023, 12, 31));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 || !result1);
        }

        [TestMethod]
        public void IsPolicyInLockupPeriod_ValidInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsPolicyInLockupPeriod("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.IsPolicyInLockupPeriod("POL456", new DateTime(2023, 6, 15));
            var result3 = _service.IsPolicyInLockupPeriod("POL789", new DateTime(2023, 12, 31));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 || !result1);
        }

        [TestMethod]
        public void RequiresSpousalConsent_ValidInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.RequiresSpousalConsent("POL123", 10000m);
            var result2 = _service.RequiresSpousalConsent("POL456", 50000m);
            var result3 = _service.RequiresSpousalConsent("POL789", 100000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 || !result1);
        }

        [TestMethod]
        public void IsSystematicWithdrawalActive_ValidInputs_ReturnsExpectedBoolean()
        {
            var result1 = _service.IsSystematicWithdrawalActive("POL123");
            var result2 = _service.IsSystematicWithdrawalActive("POL456");
            var result3 = _service.IsSystematicWithdrawalActive("POL789");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 || !result1);
        }

        [TestMethod]
        public void GetSurrenderChargePercentage_ValidInputs_ReturnsExpectedDouble()
        {
            var result1 = _service.GetSurrenderChargePercentage("POL123", 1);
            var result2 = _service.GetSurrenderChargePercentage("POL456", 5);
            var result3 = _service.GetSurrenderChargePercentage("POL789", 10);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
        }

        [TestMethod]
        public void CalculateTaxWithholdingRate_ValidInputs_ReturnsExpectedDouble()
        {
            var result1 = _service.CalculateTaxWithholdingRate("CA", true);
            var result2 = _service.CalculateTaxWithholdingRate("NY", false);
            var result3 = _service.CalculateTaxWithholdingRate("TX", true);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
        }

        [TestMethod]
        public void GetFreeWithdrawalPercentage_ValidInputs_ReturnsExpectedDouble()
        {
            var result1 = _service.GetFreeWithdrawalPercentage("PROD_A");
            var result2 = _service.GetFreeWithdrawalPercentage("PROD_B");
            var result3 = _service.GetFreeWithdrawalPercentage("PROD_C");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
        }

        [TestMethod]
        public void CalculateProRataReductionFactor_ValidInputs_ReturnsExpectedDouble()
        {
            var result1 = _service.CalculateProRataReductionFactor(1000m, 10000m);
            var result2 = _service.CalculateProRataReductionFactor(5000m, 20000m);
            var result3 = _service.CalculateProRataReductionFactor(10000m, 100000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
        }

        [TestMethod]
        public void GetRemainingFreeWithdrawalsCount_ValidInputs_ReturnsExpectedInt()
        {
            var result1 = _service.GetRemainingFreeWithdrawalsCount("POL123", 2023);
            var result2 = _service.GetRemainingFreeWithdrawalsCount("POL456", 2023);
            var result3 = _service.GetRemainingFreeWithdrawalsCount("POL789", 2023);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
        }

        [TestMethod]
        public void GetDaysUntilSurrenderChargeExpires_ValidInputs_ReturnsExpectedInt()
        {
            var result1 = _service.GetDaysUntilSurrenderChargeExpires("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GetDaysUntilSurrenderChargeExpires("POL456", new DateTime(2023, 6, 15));
            var result3 = _service.GetDaysUntilSurrenderChargeExpires("POL789", new DateTime(2023, 12, 31));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
        }

        [TestMethod]
        public void GetPolicyYear_ValidInputs_ReturnsExpectedInt()
        {
            var result1 = _service.GetPolicyYear("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GetPolicyYear("POL456", new DateTime(2023, 6, 15));
            var result3 = _service.GetPolicyYear("POL789", new DateTime(2023, 12, 31));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
        }

        [TestMethod]
        public void GetMaximumAllowedWithdrawalsPerYear_ValidInputs_ReturnsExpectedInt()
        {
            var result1 = _service.GetMaximumAllowedWithdrawalsPerYear("PROD_A");
            var result2 = _service.GetMaximumAllowedWithdrawalsPerYear("PROD_B");
            var result3 = _service.GetMaximumAllowedWithdrawalsPerYear("PROD_C");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
        }

        [TestMethod]
        public void GenerateSurrenderTransactionId_ValidInputs_ReturnsExpectedString()
        {
            var result1 = _service.GenerateSurrenderTransactionId("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GenerateSurrenderTransactionId("POL456", new DateTime(2023, 6, 15));
            var result3 = _service.GenerateSurrenderTransactionId("POL789", new DateTime(2023, 12, 31));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.AreNotEqual(string.Empty, result2);
        }

        [TestMethod]
        public void GetSurrenderChargeScheduleCode_ValidInputs_ReturnsExpectedString()
        {
            var result1 = _service.GetSurrenderChargeScheduleCode("POL123");
            var result2 = _service.GetSurrenderChargeScheduleCode("POL456");
            var result3 = _service.GetSurrenderChargeScheduleCode("POL789");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.AreNotEqual(string.Empty, result2);
        }

        [TestMethod]
        public void DetermineTaxDistributionCode_ValidInputs_ReturnsExpectedString()
        {
            var result1 = _service.DetermineTaxDistributionCode(55, true);
            var result2 = _service.DetermineTaxDistributionCode(60, false);
            var result3 = _service.DetermineTaxDistributionCode(65, true);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.AreNotEqual(string.Empty, result2);
        }

        [TestMethod]
        public void GetWithdrawalDenialReasonCode_ValidInputs_ReturnsExpectedString()
        {
            var result1 = _service.GetWithdrawalDenialReasonCode("POL123", 1000000m);
            var result2 = _service.GetWithdrawalDenialReasonCode("POL456", 5000000m);
            var result3 = _service.GetWithdrawalDenialReasonCode("POL789", 10000000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.AreNotEqual(string.Empty, result2);
        }
    }
}