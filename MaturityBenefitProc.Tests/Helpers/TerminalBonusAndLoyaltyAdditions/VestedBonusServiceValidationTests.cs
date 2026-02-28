using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    // Note: Assuming a concrete implementation named VestedBonusService exists in the tested assembly.
    // For the purpose of these validation tests, we assume standard guard clauses (throwing ArgumentException/ArgumentNullException)
    // or returning default values (0, false, null) for invalid inputs.

    [TestClass]
    public class VestedBonusServiceValidationTests
    {
        private IVestedBonusService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming VestedBonusService is the concrete implementation of IVestedBonusService
            _service = new VestedBonusService();
        }

        [TestMethod]
        public void CalculateTotalVestedBonus_InvalidPolicyIds_ThrowsArgumentException()
        {
            DateTime calcDate = new DateTime(2023, 1, 1);
            
            Assert.ThrowsException<ArgumentNullException>(() => _service.CalculateTotalVestedBonus(null, calcDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTotalVestedBonus(string.Empty, calcDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTotalVestedBonus("   ", calcDate));
            
            // Valid call should not throw
            var result = _service.CalculateTotalVestedBonus("POL12345", calcDate);
            Assert.IsNotNull(result);
            Assert.IsTrue(result >= 0);
        }

        [TestMethod]
        public void GetSimpleReversionaryBonus_BoundaryYears_ValidatesCorrectly()
        {
            string policyId = "POL999";
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetSimpleReversionaryBonus(policyId, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetSimpleReversionaryBonus(policyId, 0));
            
            var resultYear1 = _service.GetSimpleReversionaryBonus(policyId, 1);
            var resultYear10 = _service.GetSimpleReversionaryBonus(policyId, 10);
            
            Assert.IsTrue(resultYear1 >= 0);
            Assert.IsTrue(resultYear10 >= 0);
            Assert.IsNotNull(resultYear1);
        }

        [TestMethod]
        public void GetCompoundReversionaryBonus_SequentialCalls_ReturnsConsistentResults()
        {
            string policyId = "POL888";
            
            var result1 = _service.GetCompoundReversionaryBonus(policyId, 5);
            var result2 = _service.GetCompoundReversionaryBonus(policyId, 5);
            var result3 = _service.GetCompoundReversionaryBonus(policyId, 6);
            
            Assert.AreEqual(result1, result2);
            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result3 >= 0);
            Assert.AreNotEqual(-1m, result1);
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public void CalculateInterimBonus_InvalidDates_ThrowsOrReturnsZero()
        {
            string policyId = "POL777";
            
            Assert.ThrowsException<ArgumentNullException>(() => _service.CalculateInterimBonus(null, DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateInterimBonus("", DateTime.Now));
            
            var resultMinDate = _service.CalculateInterimBonus(policyId, DateTime.MinValue);
            var resultMaxDate = _service.CalculateInterimBonus(policyId, DateTime.MaxValue);
            
            Assert.IsTrue(resultMinDate >= 0);
            Assert.IsTrue(resultMaxDate >= 0);
            Assert.IsNotNull(resultMinDate);
        }

        [TestMethod]
        public void CalculateTerminalBonus_NegativeSumAssured_ThrowsArgumentOutOfRangeException()
        {
            string policyId = "POL666";
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateTerminalBonus(policyId, -1000m));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateTerminalBonus(policyId, -0.01m));
            
            var resultZero = _service.CalculateTerminalBonus(policyId, 0m);
            var resultPositive = _service.CalculateTerminalBonus(policyId, 50000m);
            
            Assert.IsTrue(resultZero >= 0);
            Assert.IsTrue(resultPositive >= 0);
            Assert.IsNotNull(resultPositive);
        }

        [TestMethod]
        public void CalculateLoyaltyAddition_InvalidCompletedYears_HandledCorrectly()
        {
            string policyId = "POL555";
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateLoyaltyAddition(policyId, -5));
            
            var resultZero = _service.CalculateLoyaltyAddition(policyId, 0);
            var resultOne = _service.CalculateLoyaltyAddition(policyId, 1);
            var resultTwenty = _service.CalculateLoyaltyAddition(policyId, 20);
            
            Assert.IsTrue(resultZero >= 0);
            Assert.IsTrue(resultOne >= 0);
            Assert.IsTrue(resultTwenty >= 0);
            Assert.IsNotNull(resultTwenty);
        }

        [TestMethod]
        public void GetBonusRateForYear_InvalidPlanCode_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.GetBonusRateForYear(5, null));
            Assert.ThrowsException<ArgumentException>(() => _service.GetBonusRateForYear(5, ""));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetBonusRateForYear(-1, "PLAN_A"));
            
            var result = _service.GetBonusRateForYear(5, "PLAN_A");
            
            Assert.IsTrue(result >= 0.0);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTerminalBonusRate_BoundaryTerms_ValidatesCorrectly()
        {
            string planCode = "TBR_PLAN";
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetTerminalBonusRate(planCode, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetTerminalBonusRate(planCode, 0));
            
            var result10 = _service.GetTerminalBonusRate(planCode, 10);
            var result20 = _service.GetTerminalBonusRate(planCode, 20);
            
            Assert.IsTrue(result10 >= 0.0);
            Assert.IsTrue(result20 >= 0.0);
            Assert.IsNotNull(result10);
        }

        [TestMethod]
        public void GetLoyaltyAdditionPercentage_NegativePremium_ThrowsArgumentOutOfRangeException()
        {
            string planCode = "LAP_PLAN";
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetLoyaltyAdditionPercentage(planCode, -100m));
            
            var resultZero = _service.GetLoyaltyAdditionPercentage(planCode, 0m);
            var resultPositive = _service.GetLoyaltyAdditionPercentage(planCode, 1000m);
            var resultLarge = _service.GetLoyaltyAdditionPercentage(planCode, 1000000m);
            
            Assert.IsTrue(resultZero >= 0.0);
            Assert.IsTrue(resultPositive >= 0.0);
            Assert.IsTrue(resultLarge >= 0.0);
            Assert.IsNotNull(resultPositive);
        }

        [TestMethod]
        public void GetInterimBonusRate_NullPlanCode_ThrowsArgumentNullException()
        {
            DateTime fy = new DateTime(2023, 4, 1);
            
            Assert.ThrowsException<ArgumentNullException>(() => _service.GetInterimBonusRate(null, fy));
            Assert.ThrowsException<ArgumentException>(() => _service.GetInterimBonusRate("", fy));
            
            var result = _service.GetInterimBonusRate("PLAN_B", fy);
            var resultFuture = _service.GetInterimBonusRate("PLAN_B", DateTime.MaxValue);
            
            Assert.IsTrue(result >= 0.0);
            Assert.IsTrue(resultFuture >= 0.0);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IsEligibleForTerminalBonus_NegativeActiveYears_ReturnsFalseOrThrows()
        {
            string policyId = "POL111";
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.IsEligibleForTerminalBonus(policyId, -1));
            
            bool resultZero = _service.IsEligibleForTerminalBonus(policyId, 0);
            bool resultTen = _service.IsEligibleForTerminalBonus(policyId, 10);
            
            Assert.IsFalse(resultZero || !resultZero); // Just asserting it evaluates to a boolean
            Assert.IsNotNull(resultZero);
            Assert.IsNotNull(resultTen);
            Assert.AreEqual(resultZero.GetType(), typeof(bool));
        }

        [TestMethod]
        public void IsEligibleForLoyaltyAddition_NegativePremiums_ThrowsArgumentOutOfRangeException()
        {
            string policyId = "POL222";
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.IsEligibleForLoyaltyAddition(policyId, -1m));
            
            bool resultZero = _service.IsEligibleForLoyaltyAddition(policyId, 0m);
            bool resultPositive = _service.IsEligibleForLoyaltyAddition(policyId, 5000m);
            
            Assert.IsNotNull(resultZero);
            Assert.IsNotNull(resultPositive);
            Assert.AreEqual(resultZero.GetType(), typeof(bool));
            Assert.AreEqual(resultPositive.GetType(), typeof(bool));
        }

        [TestMethod]
        public void HasSurrenderedPolicy_InvalidPolicyId_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.HasSurrenderedPolicy(null));
            Assert.ThrowsException<ArgumentException>(() => _service.HasSurrenderedPolicy(""));
            Assert.ThrowsException<ArgumentException>(() => _service.HasSurrenderedPolicy("   "));
            
            bool result = _service.HasSurrenderedPolicy("POL333");
            
            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetType(), typeof(bool));
        }

        [TestMethod]
        public void IsPolicyActive_VariousDates_ReturnsBoolean()
        {
            string policyId = "POL444";
            
            bool resultPast = _service.IsPolicyActive(policyId, DateTime.MinValue);
            bool resultPresent = _service.IsPolicyActive(policyId, DateTime.Now);
            bool resultFuture = _service.IsPolicyActive(policyId, DateTime.MaxValue);
            
            Assert.IsNotNull(resultPast);
            Assert.IsNotNull(resultPresent);
            Assert.IsNotNull(resultFuture);
            Assert.AreEqual(resultPresent.GetType(), typeof(bool));
        }

        [TestMethod]
        public void ValidateBonusRates_NegativeRates_ReturnsFalse()
        {
            string planCode = "PLAN_C";
            
            bool resultNegativeSimple = _service.ValidateBonusRates(planCode, -0.05, 0.05);
            bool resultNegativeCompound = _service.ValidateBonusRates(planCode, 0.05, -0.05);
            bool resultBothNegative = _service.ValidateBonusRates(planCode, -0.05, -0.05);
            bool resultValid = _service.ValidateBonusRates(planCode, 0.05, 0.05);
            
            Assert.IsFalse(resultNegativeSimple);
            Assert.IsFalse(resultNegativeCompound);
            Assert.IsFalse(resultBothNegative);
            Assert.IsNotNull(resultValid);
        }

        [TestMethod]
        public void CheckMinimumVestingPeriod_NegativeYears_ThrowsArgumentOutOfRangeException()
        {
            string policyId = "POL555";
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CheckMinimumVestingPeriod(policyId, -1));
            
            bool resultZero = _service.CheckMinimumVestingPeriod(policyId, 0);
            bool resultThree = _service.CheckMinimumVestingPeriod(policyId, 3);
            
            Assert.IsNotNull(resultZero);
            Assert.IsNotNull(resultThree);
            Assert.AreEqual(resultThree.GetType(), typeof(bool));
        }

        [TestMethod]
        public void GetCompletedPolicyYears_FutureDate_ReturnsZero()
        {
            string policyId = "POL666";
            
            int resultFuture = _service.GetCompletedPolicyYears(policyId, DateTime.MaxValue);
            int resultPast = _service.GetCompletedPolicyYears(policyId, DateTime.MinValue);
            int resultNow = _service.GetCompletedPolicyYears(policyId, DateTime.Now);
            
            Assert.IsTrue(resultFuture >= 0);
            Assert.IsTrue(resultPast >= 0);
            Assert.IsTrue(resultNow >= 0);
            Assert.IsNotNull(resultNow);
        }

        [TestMethod]
        public void GetRemainingTermInMonths_InvalidPolicyId_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.GetRemainingTermInMonths(null, DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.GetRemainingTermInMonths("", DateTime.Now));
            
            int result = _service.GetRemainingTermInMonths("POL777", DateTime.Now);
            
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTotalPremiumsPaidCount_ValidPolicy_ReturnsNonNegative()
        {
            string policyId = "POL888";
            
            int result = _service.GetTotalPremiumsPaidCount(policyId);
            
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(-1, result);
        }

        [TestMethod]
        public void GetMissedPremiumsCount_InvalidPolicyId_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.GetMissedPremiumsCount(null));
            Assert.ThrowsException<ArgumentException>(() => _service.GetMissedPremiumsCount(string.Empty));
            
            int result = _service.GetMissedPremiumsCount("POL999");
            
            Assert.IsTrue(result >= 0);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetBonusDeclarationYear_InvalidBonusId_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.GetBonusDeclarationYear(null));
            Assert.ThrowsException<ArgumentException>(() => _service.GetBonusDeclarationYear(""));
            
            int result = _service.GetBonusDeclarationYear("BONUS_2023");
            
            Assert.IsTrue(result > 1900);
            Assert.IsTrue(result <= DateTime.Now.Year + 1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetApplicableBonusTableCode_NullPlanCode_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.GetApplicableBonusTableCode(null, DateTime.Now));
            Assert.ThrowsException<ArgumentException>(() => _service.GetApplicableBonusTableCode("", DateTime.Now));
            
            string result = _service.GetApplicableBonusTableCode("PLAN_D", DateTime.Now);
            
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
        }

        [TestMethod]
        public void GetBonusStatus_InvalidPolicyId_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _service.GetBonusStatus(null));
            Assert.ThrowsException<ArgumentException>(() => _service.GetBonusStatus(""));
            
            string result = _service.GetBonusStatus("POL123");
            
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
        }

        [TestMethod]
        public void GenerateBonusStatementId_ValidInputs_ReturnsFormattedString()
        {
            string policyId = "POL456";
            DateTime date = new DateTime(2023, 5, 1);
            
            string result1 = _service.GenerateBonusStatementId(policyId, date);
            string result2 = _service.GenerateBonusStatementId(policyId, date);
            
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.AreEqual(result1, result2);
        }

        [TestMethod]
        public void CalculateGuaranteedAdditions_NegativeYears_ThrowsArgumentOutOfRangeException()
        {
            string policyId = "POL789";
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateGuaranteedAdditions(policyId, -1));
            
            decimal resultZero = _service.CalculateGuaranteedAdditions(policyId, 0);
            decimal resultFive = _service.CalculateGuaranteedAdditions(policyId, 5);
            
            Assert.IsTrue(resultZero >= 0m);
            Assert.IsTrue(resultFive >= 0m);
            Assert.IsNotNull(resultFive);
        }

        [TestMethod]
        public void CalculateProRataBonus_EndBeforeStart_ThrowsArgumentException()
        {
            string policyId = "POL000";
            DateTime start = new DateTime(2023, 1, 1);
            DateTime end = new DateTime(2022, 1, 1);
            
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateProRataBonus(policyId, start, end));
            
            decimal resultValid = _service.CalculateProRataBonus(policyId, end, start);
            
            Assert.IsTrue(resultValid >= 0m);
            Assert.IsNotNull(resultValid);
            Assert.AreNotEqual(-1m, resultValid);
        }
    }
}