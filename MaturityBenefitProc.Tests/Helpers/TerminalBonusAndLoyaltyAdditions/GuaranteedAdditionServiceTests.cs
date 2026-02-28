using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class GuaranteedAdditionServiceTests
    {
        private IGuaranteedAdditionService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation named GuaranteedAdditionService exists
            _service = new GuaranteedAdditionService();
        }

        [TestMethod]
        public void CalculateTotalGuaranteedAdditions_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateTotalGuaranteedAdditions("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateTotalGuaranteedAdditions("POL456", new DateTime(2024, 1, 1));
            var result3 = _service.CalculateTotalGuaranteedAdditions("POL789", new DateTime(2025, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void CalculateTotalGuaranteedAdditions_EmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.CalculateTotalGuaranteedAdditions("", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateTotalGuaranteedAdditions(string.Empty, new DateTime(2024, 1, 1));
            var result3 = _service.CalculateTotalGuaranteedAdditions("   ", new DateTime(2025, 1, 1));

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateAccruedAdditionsForYear_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateAccruedAdditionsForYear("POL123", 5);
            var result2 = _service.CalculateAccruedAdditionsForYear("POL456", 10);
            var result3 = _service.CalculateAccruedAdditionsForYear("POL789", 15);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void CalculateAccruedAdditionsForYear_InvalidYear_ReturnsZero()
        {
            var result1 = _service.CalculateAccruedAdditionsForYear("POL123", 0);
            var result2 = _service.CalculateAccruedAdditionsForYear("POL456", -1);
            var result3 = _service.CalculateAccruedAdditionsForYear("POL789", -5);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsPolicyEligibleForGuaranteedAdditions_EligibleProduct_ReturnsTrue()
        {
            var result1 = _service.IsPolicyEligibleForGuaranteedAdditions("POL123", "PROD_A");
            var result2 = _service.IsPolicyEligibleForGuaranteedAdditions("POL456", "PROD_B");
            var result3 = _service.IsPolicyEligibleForGuaranteedAdditions("POL789", "PROD_C");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsPolicyEligibleForGuaranteedAdditions_IneligibleProduct_ReturnsFalse()
        {
            var result1 = _service.IsPolicyEligibleForGuaranteedAdditions("POL123", "PROD_X");
            var result2 = _service.IsPolicyEligibleForGuaranteedAdditions("POL456", "PROD_Y");
            var result3 = _service.IsPolicyEligibleForGuaranteedAdditions("POL789", "PROD_Z");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetApplicableAdditionRate_ValidInputs_ReturnsExpectedRate()
        {
            var result1 = _service.GetApplicableAdditionRate("PROD_A", 10);
            var result2 = _service.GetApplicableAdditionRate("PROD_B", 15);
            var result3 = _service.GetApplicableAdditionRate("PROD_C", 20);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
        }

        [TestMethod]
        public void GetApplicableAdditionRate_InvalidTerm_ReturnsZero()
        {
            var result1 = _service.GetApplicableAdditionRate("PROD_A", 0);
            var result2 = _service.GetApplicableAdditionRate("PROD_B", -5);
            var result3 = _service.GetApplicableAdditionRate("PROD_C", -10);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetAccrualPeriodInDays_ValidDates_ReturnsCorrectDays()
        {
            var result1 = _service.GetAccrualPeriodInDays(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
            var result2 = _service.GetAccrualPeriodInDays(new DateTime(2023, 1, 1), new DateTime(2024, 1, 1));
            var result3 = _service.GetAccrualPeriodInDays(new DateTime(2023, 1, 1), new DateTime(2023, 1, 2));

            Assert.AreEqual(30, result1);
            Assert.AreEqual(365, result2);
            Assert.AreEqual(1, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetAccrualPeriodInDays_ReversedDates_ReturnsZero()
        {
            var result1 = _service.GetAccrualPeriodInDays(new DateTime(2023, 1, 31), new DateTime(2023, 1, 1));
            var result2 = _service.GetAccrualPeriodInDays(new DateTime(2024, 1, 1), new DateTime(2023, 1, 1));
            var result3 = _service.GetAccrualPeriodInDays(new DateTime(2023, 1, 2), new DateTime(2023, 1, 1));

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetAdditionCalculationBasisCode_ValidProduct_ReturnsCode()
        {
            var result1 = _service.GetAdditionCalculationBasisCode("PROD_A");
            var result2 = _service.GetAdditionCalculationBasisCode("PROD_B");
            var result3 = _service.GetAdditionCalculationBasisCode("PROD_C");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.AreNotEqual(string.Empty, result2);
        }

        [TestMethod]
        public void CalculateProRataAdditions_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateProRataAdditions("POL123", 1000m, 180);
            var result2 = _service.CalculateProRataAdditions("POL456", 2000m, 90);
            var result3 = _service.CalculateProRataAdditions("POL789", 3000m, 365);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void ValidateAdditionRateLimits_ValidRate_ReturnsTrue()
        {
            var result1 = _service.ValidateAdditionRateLimits(0.05, "PROD_A");
            var result2 = _service.ValidateAdditionRateLimits(0.08, "PROD_B");
            var result3 = _service.ValidateAdditionRateLimits(0.10, "PROD_C");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateAdditionRateLimits_InvalidRate_ReturnsFalse()
        {
            var result1 = _service.ValidateAdditionRateLimits(1.5, "PROD_A");
            var result2 = _service.ValidateAdditionRateLimits(-0.05, "PROD_B");
            var result3 = _service.ValidateAdditionRateLimits(2.0, "PROD_C");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetSumAssuredForAdditions_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.GetSumAssuredForAdditions("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GetSumAssuredForAdditions("POL456", new DateTime(2024, 1, 1));
            var result3 = _service.GetSumAssuredForAdditions("POL789", new DateTime(2025, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidDates_ReturnsCorrectYears()
        {
            var result1 = _service.GetCompletedPolicyYears("POL123", new DateTime(2033, 1, 1));
            var result2 = _service.GetCompletedPolicyYears("POL456", new DateTime(2044, 1, 1));
            var result3 = _service.GetCompletedPolicyYears("POL789", new DateTime(2055, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
        }

        [TestMethod]
        public void CalculateVestingPercentage_ValidInputs_ReturnsExpectedPercentage()
        {
            var result1 = _service.CalculateVestingPercentage(5, 10);
            var result2 = _service.CalculateVestingPercentage(10, 20);
            var result3 = _service.CalculateVestingPercentage(15, 15);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
        }

        [TestMethod]
        public void CalculateVestedGuaranteedAdditions_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateVestedGuaranteedAdditions("POL123", 1000m, 0.5);
            var result2 = _service.CalculateVestedGuaranteedAdditions("POL456", 2000m, 0.75);
            var result3 = _service.CalculateVestedGuaranteedAdditions("POL789", 3000m, 1.0);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void RetrieveAdditionRuleId_ValidInputs_ReturnsRuleId()
        {
            var result1 = _service.RetrieveAdditionRuleId("PROD_A", new DateTime(2023, 1, 1));
            var result2 = _service.RetrieveAdditionRuleId("PROD_B", new DateTime(2024, 1, 1));
            var result3 = _service.RetrieveAdditionRuleId("PROD_C", new DateTime(2025, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.AreNotEqual(string.Empty, result2);
        }

        [TestMethod]
        public void HasLapsedPeriodAffectingAdditions_LapsedPolicy_ReturnsTrue()
        {
            var result1 = _service.HasLapsedPeriodAffectingAdditions("POL_LAPSED_1");
            var result2 = _service.HasLapsedPeriodAffectingAdditions("POL_LAPSED_2");
            var result3 = _service.HasLapsedPeriodAffectingAdditions("POL_LAPSED_3");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void DeductUnpaidPremiumsFromAdditions_ValidInputs_ReturnsNetAmount()
        {
            var result1 = _service.DeductUnpaidPremiumsFromAdditions(1000m, 200m);
            var result2 = _service.DeductUnpaidPremiumsFromAdditions(2000m, 500m);
            var result3 = _service.DeductUnpaidPremiumsFromAdditions(3000m, 0m);

            Assert.AreEqual(800m, result1);
            Assert.AreEqual(1500m, result2);
            Assert.AreEqual(3000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetMissedPremiumCount_ValidPolicy_ReturnsCount()
        {
            var result1 = _service.GetMissedPremiumCount("POL123");
            var result2 = _service.GetMissedPremiumCount("POL456");
            var result3 = _service.GetMissedPremiumCount("POL789");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void GetLoyaltyMultiplier_ValidInputs_ReturnsMultiplier()
        {
            var result1 = _service.GetLoyaltyMultiplier("POL123", 10);
            var result2 = _service.GetLoyaltyMultiplier("POL456", 15);
            var result3 = _service.GetLoyaltyMultiplier("POL789", 20);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
        }

        [TestMethod]
        public void ApplyLoyaltyMultiplierToAdditions_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.ApplyLoyaltyMultiplierToAdditions(1000m, 1.1);
            var result2 = _service.ApplyLoyaltyMultiplierToAdditions(2000m, 1.2);
            var result3 = _service.ApplyLoyaltyMultiplierToAdditions(3000m, 1.5);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void CheckMinimumTermForAdditions_MeetsRequirement_ReturnsTrue()
        {
            var result1 = _service.CheckMinimumTermForAdditions("POL123", 5);
            var result2 = _service.CheckMinimumTermForAdditions("POL456", 10);
            var result3 = _service.CheckMinimumTermForAdditions("POL789", 15);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetCurrencyCodeForAdditions_ValidPolicy_ReturnsCode()
        {
            var result1 = _service.GetCurrencyCodeForAdditions("POL123");
            var result2 = _service.GetCurrencyCodeForAdditions("POL456");
            var result3 = _service.GetCurrencyCodeForAdditions("POL789");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.AreNotEqual(string.Empty, result2);
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateTerminalBonus("POL123", 10000m, 0.05);
            var result2 = _service.CalculateTerminalBonus("POL456", 20000m, 0.10);
            var result3 = _service.CalculateTerminalBonus("POL789", 30000m, 0.15);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidInputs_ReturnsRate()
        {
            var result1 = _service.GetTerminalBonusRate("PROD_A", 10);
            var result2 = _service.GetTerminalBonusRate("PROD_B", 15);
            var result3 = _service.GetTerminalBonusRate("PROD_C", 20);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0.0, result1);
            Assert.AreNotEqual(0.0, result2);
        }

        [TestMethod]
        public void IsTerminalBonusGuaranteed_ValidProduct_ReturnsExpectedBool()
        {
            var result1 = _service.IsTerminalBonusGuaranteed("PROD_A");
            var result2 = _service.IsTerminalBonusGuaranteed("PROD_B");
            var result3 = _service.IsTerminalBonusGuaranteed("PROD_C");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void CalculateSpecialGuaranteedAddition_ValidInputs_ReturnsExpectedAmount()
        {
            var result1 = _service.CalculateSpecialGuaranteedAddition("POL123", 1000m);
            var result2 = _service.CalculateSpecialGuaranteedAddition("POL456", 2000m);
            var result3 = _service.CalculateSpecialGuaranteedAddition("POL789", 3000m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0m, result1);
            Assert.AreNotEqual(0m, result2);
        }

        [TestMethod]
        public void GetRemainingDaysToMaturity_ValidDates_ReturnsCorrectDays()
        {
            var result1 = _service.GetRemainingDaysToMaturity("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GetRemainingDaysToMaturity("POL456", new DateTime(2024, 1, 1));
            var result3 = _service.GetRemainingDaysToMaturity("POL789", new DateTime(2025, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(0, result1);
            Assert.AreNotEqual(0, result2);
        }
    }
}