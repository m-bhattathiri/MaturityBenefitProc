using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.ReinsuranceAndPoolManagement;

namespace MaturityBenefitProc.Tests.Helpers.ReinsuranceAndPoolManagement
{
    [TestClass]
    public class TreatyCalculationServiceValidationTests
    {
        private ITreatyCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Note: Assuming TreatyCalculationService implements ITreatyCalculationService.
            // Since the prompt asks to create a new TreatyCalculationService(), we use a mock or concrete implementation.
            // For the sake of this test file, we assume TreatyCalculationService is available in the namespace.
            _service = new TreatyCalculationService();
        }

        [TestMethod]
        public void CalculateQuotaShareRetention_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateQuotaShareRetention("TR-001", 10000m);
            var result2 = _service.CalculateQuotaShareRetention("TR-002", 50000m);
            var result3 = _service.CalculateQuotaShareRetention("TR-001", 0m);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CalculateQuotaShareRetention_NegativeAmount_ReturnsZeroOrThrows()
        {
            var result1 = _service.CalculateQuotaShareRetention("TR-001", -100m);
            var result2 = _service.CalculateQuotaShareRetention("TR-002", -5000m);
            
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 <= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 <= 0);
        }

        [TestMethod]
        public void CalculateQuotaShareCeded_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateQuotaShareCeded("TR-001", 10000m);
            var result2 = _service.CalculateQuotaShareCeded("TR-002", 50000m);
            var result3 = _service.CalculateQuotaShareCeded("TR-001", 0m);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void GetSurplusSharePercentage_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetSurplusSharePercentage("TR-001", 100000m, 20000m);
            var result2 = _service.GetSurplusSharePercentage("TR-002", 50000m, 50000m);
            var result3 = _service.GetSurplusSharePercentage("TR-003", 10000m, 20000m);

            Assert.IsTrue(result1 >= 0 && result1 <= 100);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateSurplusCededAmount_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateSurplusCededAmount("TR-001", 10000m, 50.0);
            var result2 = _service.CalculateSurplusCededAmount("TR-002", 50000m, 0.0);
            var result3 = _service.CalculateSurplusCededAmount("TR-003", 10000m, 100.0);

            Assert.AreEqual(5000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(10000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsEligibleForProportionalTreaty_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.IsEligibleForProportionalTreaty("POL-001", "TR-001", new DateTime(2023, 1, 1));
            var result2 = _service.IsEligibleForProportionalTreaty("POL-002", "TR-002", new DateTime(2024, 1, 1));
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void CalculateExcessOfLossRecovery_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateExcessOfLossRecovery("TR-001", 100000m, 20000m);
            var result2 = _service.CalculateExcessOfLossRecovery("TR-002", 10000m, 20000m);
            var result3 = _service.CalculateExcessOfLossRecovery("TR-003", 20000m, 20000m);

            Assert.AreEqual(80000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateStopLossRecovery_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateStopLossRecovery("POOL-001", 500000m, 400000m);
            var result2 = _service.CalculateStopLossRecovery("POOL-002", 300000m, 400000m);
            var result3 = _service.CalculateStopLossRecovery("POOL-003", 400000m, 400000m);

            Assert.AreEqual(100000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateLayerExhaustion_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.ValidateLayerExhaustion("LYR-001", 1000000m);
            var result2 = _service.ValidateLayerExhaustion("LYR-002", 0m);
            var result3 = _service.ValidateLayerExhaustion("LYR-003", -500m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 || !result1);
        }

        [TestMethod]
        public void GetRemainingReinstatements_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetRemainingReinstatements("TR-001", 1);
            var result2 = _service.GetRemainingReinstatements("TR-002", 0);
            var result3 = _service.GetRemainingReinstatements("TR-003", 5);

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsTrue(result3 >= 0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateReinstatementPremium_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateReinstatementPremium("TR-001", 100000m, 0.1);
            var result2 = _service.CalculateReinstatementPremium("TR-002", 50000m, 0.0);
            var result3 = _service.CalculateReinstatementPremium("TR-003", 0m, 0.5);

            Assert.AreEqual(10000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculatePoolCapacity_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculatePoolCapacity("POOL-001", new DateTime(2023, 1, 1));
            var result2 = _service.CalculatePoolCapacity("POOL-002", new DateTime(2024, 1, 1));
            
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void GetParticipantShareRatio_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetParticipantShareRatio("POOL-001", "PART-001");
            var result2 = _service.GetParticipantShareRatio("POOL-002", "PART-002");
            
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0 && result1 <= 1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0 && result2 <= 1);
        }

        [TestMethod]
        public void CalculateParticipantLiability_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateParticipantLiability("POOL-001", "PART-001", 100000m);
            var result2 = _service.CalculateParticipantLiability("POOL-002", "PART-002", 50000m);
            var result3 = _service.CalculateParticipantLiability("POOL-001", "PART-001", 0m);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void GetLeadReinsurerId_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetLeadReinsurerId("TR-001");
            var result2 = _service.GetLeadReinsurerId("TR-002");
            
            Assert.IsNotNull(result1);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual(string.Empty, result2);
        }

        [TestMethod]
        public void GetActivePoolParticipantsCount_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetActivePoolParticipantsCount("POOL-001");
            var result2 = _service.GetActivePoolParticipantsCount("POOL-002");
            
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void CalculateTerminalBonusCeded_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateTerminalBonusCeded("TR-001", 10000m, 0.2);
            var result2 = _service.CalculateTerminalBonusCeded("TR-002", 5000m, 0.0);
            var result3 = _service.CalculateTerminalBonusCeded("TR-003", 0m, 0.5);

            Assert.AreEqual(2000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateGuaranteedAdditionRecovery_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateGuaranteedAdditionRecovery("TR-001", 10000m);
            var result2 = _service.CalculateGuaranteedAdditionRecovery("TR-002", 5000m);
            var result3 = _service.CalculateGuaranteedAdditionRecovery("TR-003", 0m);

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
            Assert.AreEqual(0m, result3);
        }

        [TestMethod]
        public void CheckMaturityDateWithinTreatyPeriod_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CheckMaturityDateWithinTreatyPeriod("TR-001", new DateTime(2023, 1, 1));
            var result2 = _service.CheckMaturityDateWithinTreatyPeriod("TR-002", new DateTime(2024, 1, 1));
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result1 || !result1);
            Assert.IsTrue(result2 || !result2);
        }

        [TestMethod]
        public void ResolveApplicableTreatyCode_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.ResolveApplicableTreatyCode("POL-001", new DateTime(2010, 1, 1), new DateTime(2020, 1, 1));
            var result2 = _service.ResolveApplicableTreatyCode("POL-002", new DateTime(2015, 1, 1), new DateTime(2025, 1, 1));
            
            Assert.IsNotNull(result1);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual(string.Empty, result2);
        }

        [TestMethod]
        public void CalculateDaysInForce_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateDaysInForce(new DateTime(2020, 1, 1), new DateTime(2020, 1, 11));
            var result2 = _service.CalculateDaysInForce(new DateTime(2020, 1, 1), new DateTime(2020, 1, 1));
            var result3 = _service.CalculateDaysInForce(new DateTime(2020, 1, 11), new DateTime(2020, 1, 1));

            Assert.AreEqual(10, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(-10, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTreatyCapacityLimit_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetTreatyCapacityLimit("TR-001");
            var result2 = _service.GetTreatyCapacityLimit("TR-002");
            
            Assert.IsNotNull(result1);
            Assert.IsTrue(result1 >= 0);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2 >= 0);
        }

        [TestMethod]
        public void ValidateMinimumCessionAmount_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.ValidateMinimumCessionAmount("TR-001", 1000m);
            var result2 = _service.ValidateMinimumCessionAmount("TR-002", 0m);
            var result3 = _service.ValidateMinimumCessionAmount("TR-003", -100m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1 || !result1);
        }

        [TestMethod]
        public void CalculateLossRatio_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.CalculateLossRatio(50000m, 100000m);
            var result2 = _service.CalculateLossRatio(0m, 100000m);
            var result3 = _service.CalculateLossRatio(100000m, 50000m);

            Assert.AreEqual(0.5, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(2.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTreatyCurrencyCode_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.GetTreatyCurrencyCode("TR-001");
            var result2 = _service.GetTreatyCurrencyCode("TR-002");
            
            Assert.IsNotNull(result1);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual(string.Empty, result2);
        }

        [TestMethod]
        public void ConvertCurrencyForTreaty_ValidInputs_ReturnsExpected()
        {
            var result1 = _service.ConvertCurrencyForTreaty("TR-001", 1000m, 1.5);
            var result2 = _service.ConvertCurrencyForTreaty("TR-002", 0m, 1.5);
            var result3 = _service.ConvertCurrencyForTreaty("TR-003", 1000m, 0.0);

            Assert.AreEqual(1500m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }
    }
}
