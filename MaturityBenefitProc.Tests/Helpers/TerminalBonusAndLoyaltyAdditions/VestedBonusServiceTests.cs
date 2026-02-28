using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class VestedBonusServiceTests
    {
        private IVestedBonusService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation named VestedBonusService exists
            _service = new VestedBonusService();
        }

        [TestMethod]
        public void CalculateTotalVestedBonus_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.CalculateTotalVestedBonus("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateTotalVestedBonus("POL456", new DateTime(2022, 12, 31));
            var result3 = _service.CalculateTotalVestedBonus("POL789", new DateTime(2024, 6, 15));

            Assert.IsNotNull(result1);
            Assert.AreEqual(1000m, result1);
            Assert.AreEqual(1000m, result2);
            Assert.AreEqual(1000m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void CalculateTotalVestedBonus_EmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.CalculateTotalVestedBonus("", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateTotalVestedBonus(string.Empty, new DateTime(2022, 12, 31));
            var result3 = _service.CalculateTotalVestedBonus("   ", new DateTime(2024, 6, 15));

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreNotEqual(1000m, result1);
        }

        [TestMethod]
        public void GetSimpleReversionaryBonus_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetSimpleReversionaryBonus("POL123", 5);
            var result2 = _service.GetSimpleReversionaryBonus("POL456", 10);
            var result3 = _service.GetSimpleReversionaryBonus("POL789", 1);

            Assert.IsNotNull(result1);
            Assert.AreEqual(500m, result1);
            Assert.AreEqual(500m, result2);
            Assert.AreEqual(500m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void GetCompoundReversionaryBonus_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetCompoundReversionaryBonus("POL123", 5);
            var result2 = _service.GetCompoundReversionaryBonus("POL456", 10);
            var result3 = _service.GetCompoundReversionaryBonus("POL789", 1);

            Assert.IsNotNull(result1);
            Assert.AreEqual(600m, result1);
            Assert.AreEqual(600m, result2);
            Assert.AreEqual(600m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void CalculateInterimBonus_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.CalculateInterimBonus("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateInterimBonus("POL456", new DateTime(2022, 12, 31));
            var result3 = _service.CalculateInterimBonus("POL789", new DateTime(2024, 6, 15));

            Assert.IsNotNull(result1);
            Assert.AreEqual(250m, result1);
            Assert.AreEqual(250m, result2);
            Assert.AreEqual(250m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void CalculateTerminalBonus_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.CalculateTerminalBonus("POL123", 100000m);
            var result2 = _service.CalculateTerminalBonus("POL456", 50000m);
            var result3 = _service.CalculateTerminalBonus("POL789", 200000m);

            Assert.IsNotNull(result1);
            Assert.AreEqual(5000m, result1);
            Assert.AreEqual(5000m, result2);
            Assert.AreEqual(5000m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.CalculateLoyaltyAddition("POL123", 10);
            var result2 = _service.CalculateLoyaltyAddition("POL456", 15);
            var result3 = _service.CalculateLoyaltyAddition("POL789", 20);

            Assert.IsNotNull(result1);
            Assert.AreEqual(1500m, result1);
            Assert.AreEqual(1500m, result2);
            Assert.AreEqual(1500m, result3);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void GetBonusRateForYear_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetBonusRateForYear(2020, "PLAN1");
            var result2 = _service.GetBonusRateForYear(2021, "PLAN2");
            var result3 = _service.GetBonusRateForYear(2022, "PLAN3");

            Assert.IsNotNull(result1);
            Assert.AreEqual(0.05, result1);
            Assert.AreEqual(0.05, result2);
            Assert.AreEqual(0.05, result3);
            Assert.AreNotEqual(0.0, result1);
        }

        [TestMethod]
        public void GetTerminalBonusRate_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetTerminalBonusRate("PLAN1", 10);
            var result2 = _service.GetTerminalBonusRate("PLAN2", 15);
            var result3 = _service.GetTerminalBonusRate("PLAN3", 20);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0.10, result1);
            Assert.AreEqual(0.10, result2);
            Assert.AreEqual(0.10, result3);
            Assert.AreNotEqual(0.0, result1);
        }

        [TestMethod]
        public void GetLoyaltyAdditionPercentage_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetLoyaltyAdditionPercentage("PLAN1", 10000m);
            var result2 = _service.GetLoyaltyAdditionPercentage("PLAN2", 20000m);
            var result3 = _service.GetLoyaltyAdditionPercentage("PLAN3", 30000m);

            Assert.IsNotNull(result1);
            Assert.AreEqual(0.02, result1);
            Assert.AreEqual(0.02, result2);
            Assert.AreEqual(0.02, result3);
            Assert.AreNotEqual(0.0, result1);
        }

        [TestMethod]
        public void GetInterimBonusRate_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetInterimBonusRate("PLAN1", new DateTime(2023, 1, 1));
            var result2 = _service.GetInterimBonusRate("PLAN2", new DateTime(2022, 12, 31));
            var result3 = _service.GetInterimBonusRate("PLAN3", new DateTime(2024, 6, 15));

            Assert.IsNotNull(result1);
            Assert.AreEqual(0.03, result1);
            Assert.AreEqual(0.03, result2);
            Assert.AreEqual(0.03, result3);
            Assert.AreNotEqual(0.0, result1);
        }

        [TestMethod]
        public void IsEligibleForTerminalBonus_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.IsEligibleForTerminalBonus("POL123", 10);
            var result2 = _service.IsEligibleForTerminalBonus("POL456", 15);
            var result3 = _service.IsEligibleForTerminalBonus("POL789", 20);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.AreNotEqual(false, result1);
        }

        [TestMethod]
        public void IsEligibleForLoyaltyAddition_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.IsEligibleForLoyaltyAddition("POL123", 50000m);
            var result2 = _service.IsEligibleForLoyaltyAddition("POL456", 100000m);
            var result3 = _service.IsEligibleForLoyaltyAddition("POL789", 150000m);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.AreNotEqual(false, result1);
        }

        [TestMethod]
        public void HasSurrenderedPolicy_ValidInputs_ReturnsFalse()
        {
            var result1 = _service.HasSurrenderedPolicy("POL123");
            var result2 = _service.HasSurrenderedPolicy("POL456");
            var result3 = _service.HasSurrenderedPolicy("POL789");

            Assert.IsNotNull(result1);
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.AreNotEqual(true, result1);
        }

        [TestMethod]
        public void IsPolicyActive_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.IsPolicyActive("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.IsPolicyActive("POL456", new DateTime(2022, 12, 31));
            var result3 = _service.IsPolicyActive("POL789", new DateTime(2024, 6, 15));

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.AreNotEqual(false, result1);
        }

        [TestMethod]
        public void ValidateBonusRates_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.ValidateBonusRates("PLAN1", 0.05, 0.06);
            var result2 = _service.ValidateBonusRates("PLAN2", 0.04, 0.05);
            var result3 = _service.ValidateBonusRates("PLAN3", 0.06, 0.07);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.AreNotEqual(false, result1);
        }

        [TestMethod]
        public void CheckMinimumVestingPeriod_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.CheckMinimumVestingPeriod("POL123", 5);
            var result2 = _service.CheckMinimumVestingPeriod("POL456", 3);
            var result3 = _service.CheckMinimumVestingPeriod("POL789", 10);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.AreNotEqual(false, result1);
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetCompletedPolicyYears("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GetCompletedPolicyYears("POL456", new DateTime(2022, 12, 31));
            var result3 = _service.GetCompletedPolicyYears("POL789", new DateTime(2024, 6, 15));

            Assert.IsNotNull(result1);
            Assert.AreEqual(5, result1);
            Assert.AreEqual(5, result2);
            Assert.AreEqual(5, result3);
            Assert.AreNotEqual(0, result1);
        }

        [TestMethod]
        public void GetRemainingTermInMonths_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetRemainingTermInMonths("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GetRemainingTermInMonths("POL456", new DateTime(2022, 12, 31));
            var result3 = _service.GetRemainingTermInMonths("POL789", new DateTime(2024, 6, 15));

            Assert.IsNotNull(result1);
            Assert.AreEqual(60, result1);
            Assert.AreEqual(60, result2);
            Assert.AreEqual(60, result3);
            Assert.AreNotEqual(0, result1);
        }

        [TestMethod]
        public void GetTotalPremiumsPaidCount_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetTotalPremiumsPaidCount("POL123");
            var result2 = _service.GetTotalPremiumsPaidCount("POL456");
            var result3 = _service.GetTotalPremiumsPaidCount("POL789");

            Assert.IsNotNull(result1);
            Assert.AreEqual(60, result1);
            Assert.AreEqual(60, result2);
            Assert.AreEqual(60, result3);
            Assert.AreNotEqual(0, result1);
        }

        [TestMethod]
        public void GetMissedPremiumsCount_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetMissedPremiumsCount("POL123");
            var result2 = _service.GetMissedPremiumsCount("POL456");
            var result3 = _service.GetMissedPremiumsCount("POL789");

            Assert.IsNotNull(result1);
            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.AreNotEqual(1, result1);
        }

        [TestMethod]
        public void GetBonusDeclarationYear_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetBonusDeclarationYear("BONUS1");
            var result2 = _service.GetBonusDeclarationYear("BONUS2");
            var result3 = _service.GetBonusDeclarationYear("BONUS3");

            Assert.IsNotNull(result1);
            Assert.AreEqual(2023, result1);
            Assert.AreEqual(2023, result2);
            Assert.AreEqual(2023, result3);
            Assert.AreNotEqual(0, result1);
        }

        [TestMethod]
        public void GetApplicableBonusTableCode_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetApplicableBonusTableCode("PLAN1", new DateTime(2020, 1, 1));
            var result2 = _service.GetApplicableBonusTableCode("PLAN2", new DateTime(2021, 1, 1));
            var result3 = _service.GetApplicableBonusTableCode("PLAN3", new DateTime(2022, 1, 1));

            Assert.IsNotNull(result1);
            Assert.AreEqual("TBL_001", result1);
            Assert.AreEqual("TBL_001", result2);
            Assert.AreEqual("TBL_001", result3);
            Assert.AreNotEqual("", result1);
        }

        [TestMethod]
        public void GetBonusStatus_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetBonusStatus("POL123");
            var result2 = _service.GetBonusStatus("POL456");
            var result3 = _service.GetBonusStatus("POL789");

            Assert.IsNotNull(result1);
            Assert.AreEqual("Vested", result1);
            Assert.AreEqual("Vested", result2);
            Assert.AreEqual("Vested", result3);
            Assert.AreNotEqual("", result1);
        }

        [TestMethod]
        public void GenerateBonusStatementId_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GenerateBonusStatementId("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.GenerateBonusStatementId("POL456", new DateTime(2022, 12, 31));
            var result3 = _service.GenerateBonusStatementId("POL789", new DateTime(2024, 6, 15));

            Assert.IsNotNull(result1);
            Assert.AreEqual("STMT_POL123_2023", result1);
            Assert.AreEqual("STMT_POL456_2022", result2);
            Assert.AreEqual("STMT_POL789_2024", result3);
            Assert.AreNotEqual("", result1);
        }

        [TestMethod]
        public void GetFundCodeForBonus_ValidInputs_ReturnsExpectedValue()
        {
            var result1 = _service.GetFundCodeForBonus("PLAN1");
            var result2 = _service.GetFundCodeForBonus("PLAN2");
            var result3 = _service.GetFundCodeForBonus("PLAN3");

            Assert.IsNotNull(result1);
            Assert.AreEqual("FUND_A", result1);
            Assert.AreEqual("FUND_A", result2);
            Assert.AreEqual("FUND_A", result3);
            Assert.AreNotEqual("", result1);
        }
    }
}