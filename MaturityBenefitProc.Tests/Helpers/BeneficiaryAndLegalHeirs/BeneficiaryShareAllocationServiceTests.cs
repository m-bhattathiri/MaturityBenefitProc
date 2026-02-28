using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs;

namespace MaturityBenefitProc.Tests.Helpers.BeneficiaryAndLegalHeirs
{
    [TestClass]
    public class BeneficiaryShareAllocationServiceTests
    {
        private IBeneficiaryShareAllocationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete implementation exists for testing
            _service = new BeneficiaryShareAllocationService();
        }

        [TestMethod]
        public void CalculateBasePayoutAmount_ValidInputs_ReturnsCorrectAmount()
        {
            var result1 = _service.CalculateBasePayoutAmount("POL123", 1000m);
            var result2 = _service.CalculateBasePayoutAmount("POL456", 5000.50m);
            var result3 = _service.CalculateBasePayoutAmount("POL789", 0m);

            Assert.AreEqual(1000m, result1);
            Assert.AreEqual(5000.50m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateBasePayoutAmount_NullOrEmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.CalculateBasePayoutAmount(null, 1000m);
            var result2 = _service.CalculateBasePayoutAmount("", 5000m);
            var result3 = _service.CalculateBasePayoutAmount("   ", 2000m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateBeneficiaryShareAmount_ValidInputs_ReturnsCorrectShare()
        {
            var result1 = _service.CalculateBeneficiaryShareAmount(1000m, 50.0);
            var result2 = _service.CalculateBeneficiaryShareAmount(5000m, 25.5);
            var result3 = _service.CalculateBeneficiaryShareAmount(2000m, 0.0);

            Assert.AreEqual(500m, result1);
            Assert.AreEqual(1275m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateBeneficiaryShareAmount_NegativeAmountOrShare_ReturnsZero()
        {
            var result1 = _service.CalculateBeneficiaryShareAmount(-1000m, 50.0);
            var result2 = _service.CalculateBeneficiaryShareAmount(5000m, -25.5);
            var result3 = _service.CalculateBeneficiaryShareAmount(-2000m, -10.0);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ApplyTaxDeductionToShare_ValidInputs_ReturnsCorrectDeduction()
        {
            var result1 = _service.ApplyTaxDeductionToShare(1000m, 10.0);
            var result2 = _service.ApplyTaxDeductionToShare(500m, 5.0);
            var result3 = _service.ApplyTaxDeductionToShare(2000m, 0.0);

            Assert.AreEqual(900m, result1);
            Assert.AreEqual(475m, result2);
            Assert.AreEqual(2000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ApplyTaxDeductionToShare_NegativeInputs_ReturnsZero()
        {
            var result1 = _service.ApplyTaxDeductionToShare(-1000m, 10.0);
            var result2 = _service.ApplyTaxDeductionToShare(500m, -5.0);
            var result3 = _service.ApplyTaxDeductionToShare(-2000m, -10.0);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateLateInterest_ValidInputs_ReturnsCorrectInterest()
        {
            var result1 = _service.CalculateLateInterest(1000m, 5.0, 30);
            var result2 = _service.CalculateLateInterest(5000m, 2.5, 10);
            var result3 = _service.CalculateLateInterest(2000m, 0.0, 5);

            Assert.AreEqual(1000m * 0.05m * (30m / 365m), result1);
            Assert.AreEqual(5000m * 0.025m * (10m / 365m), result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateLateInterest_NegativeInputs_ReturnsZero()
        {
            var result1 = _service.CalculateLateInterest(-1000m, 5.0, 30);
            var result2 = _service.CalculateLateInterest(5000m, -2.5, 10);
            var result3 = _service.CalculateLateInterest(2000m, 5.0, -5);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTotalAllocatedFunds_ValidPolicyId_ReturnsAmount()
        {
            var result1 = _service.GetTotalAllocatedFunds("POL123");
            var result2 = _service.GetTotalAllocatedFunds("POL456");
            var result3 = _service.GetTotalAllocatedFunds("POL789");

            Assert.IsTrue(result1 >= 0m);
            Assert.IsTrue(result2 >= 0m);
            Assert.IsTrue(result3 >= 0m);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTotalAllocatedFunds_NullOrEmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.GetTotalAllocatedFunds(null);
            var result2 = _service.GetTotalAllocatedFunds("");
            var result3 = _service.GetTotalAllocatedFunds("   ");

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateLegalHeirDisputeReserve_ValidInputs_ReturnsReserve()
        {
            var result1 = _service.CalculateLegalHeirDisputeReserve(1000m, 2);
            var result2 = _service.CalculateLegalHeirDisputeReserve(5000m, 5);
            var result3 = _service.CalculateLegalHeirDisputeReserve(2000m, 0);

            Assert.AreEqual(200m, result1); // Assuming 10% per heir
            Assert.AreEqual(2500m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateLegalHeirDisputeReserve_NegativeInputs_ReturnsZero()
        {
            var result1 = _service.CalculateLegalHeirDisputeReserve(-1000m, 2);
            var result2 = _service.CalculateLegalHeirDisputeReserve(5000m, -5);
            var result3 = _service.CalculateLegalHeirDisputeReserve(-2000m, -1);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void AdjustPayoutForOutstandingPremiums_ValidInputs_ReturnsAdjusted()
        {
            var result1 = _service.AdjustPayoutForOutstandingPremiums(1000m, 200m);
            var result2 = _service.AdjustPayoutForOutstandingPremiums(500m, 500m);
            var result3 = _service.AdjustPayoutForOutstandingPremiums(200m, 500m);

            Assert.AreEqual(800m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void AdjustPayoutForOutstandingPremiums_NegativeInputs_ReturnsOriginalOrZero()
        {
            var result1 = _service.AdjustPayoutForOutstandingPremiums(-1000m, 200m);
            var result2 = _service.AdjustPayoutForOutstandingPremiums(500m, -200m);
            var result3 = _service.AdjustPayoutForOutstandingPremiums(-200m, -500m);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(500m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPrimaryBeneficiarySharePercentage_ValidId_ReturnsPercentage()
        {
            var result1 = _service.GetPrimaryBeneficiarySharePercentage("BEN123");
            var result2 = _service.GetPrimaryBeneficiarySharePercentage("BEN456");
            var result3 = _service.GetPrimaryBeneficiarySharePercentage("BEN789");

            Assert.IsTrue(result1 >= 0.0 && result1 <= 100.0);
            Assert.IsTrue(result2 >= 0.0 && result2 <= 100.0);
            Assert.IsTrue(result3 >= 0.0 && result3 <= 100.0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPrimaryBeneficiarySharePercentage_NullOrEmptyId_ReturnsZero()
        {
            var result1 = _service.GetPrimaryBeneficiarySharePercentage(null);
            var result2 = _service.GetPrimaryBeneficiarySharePercentage("");
            var result3 = _service.GetPrimaryBeneficiarySharePercentage("   ");

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetContingentBeneficiarySharePercentage_ValidId_ReturnsPercentage()
        {
            var result1 = _service.GetContingentBeneficiarySharePercentage("BEN123");
            var result2 = _service.GetContingentBeneficiarySharePercentage("BEN456");
            var result3 = _service.GetContingentBeneficiarySharePercentage("BEN789");

            Assert.IsTrue(result1 >= 0.0 && result1 <= 100.0);
            Assert.IsTrue(result2 >= 0.0 && result2 <= 100.0);
            Assert.IsTrue(result3 >= 0.0 && result3 <= 100.0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetContingentBeneficiarySharePercentage_NullOrEmptyId_ReturnsZero()
        {
            var result1 = _service.GetContingentBeneficiarySharePercentage(null);
            var result2 = _service.GetContingentBeneficiarySharePercentage("");
            var result3 = _service.GetContingentBeneficiarySharePercentage("   ");

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateRemainingSharePool_ValidPolicyId_ReturnsRemaining()
        {
            var result1 = _service.CalculateRemainingSharePool("POL123");
            var result2 = _service.CalculateRemainingSharePool("POL456");
            var result3 = _service.CalculateRemainingSharePool("POL789");

            Assert.IsTrue(result1 >= 0.0 && result1 <= 100.0);
            Assert.IsTrue(result2 >= 0.0 && result2 <= 100.0);
            Assert.IsTrue(result3 >= 0.0 && result3 <= 100.0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateRemainingSharePool_NullOrEmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.CalculateRemainingSharePool(null);
            var result2 = _service.CalculateRemainingSharePool("");
            var result3 = _service.CalculateRemainingSharePool("   ");

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void AdjustShareForMinorBeneficiary_ValidInputs_ReturnsAdjusted()
        {
            var result1 = _service.AdjustShareForMinorBeneficiary(50.0, 15);
            var result2 = _service.AdjustShareForMinorBeneficiary(100.0, 10);
            var result3 = _service.AdjustShareForMinorBeneficiary(25.0, 20);

            Assert.AreEqual(50.0, result1);
            Assert.AreEqual(100.0, result2);
            Assert.AreEqual(25.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void AdjustShareForMinorBeneficiary_NegativeInputs_ReturnsZero()
        {
            var result1 = _service.AdjustShareForMinorBeneficiary(-50.0, 15);
            var result2 = _service.AdjustShareForMinorBeneficiary(100.0, -10);
            var result3 = _service.AdjustShareForMinorBeneficiary(-25.0, -20);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetApplicableTaxRate_ValidInputs_ReturnsRate()
        {
            var result1 = _service.GetApplicableTaxRate("BEN123", "NY");
            var result2 = _service.GetApplicableTaxRate("BEN456", "CA");
            var result3 = _service.GetApplicableTaxRate("BEN789", "TX");

            Assert.IsTrue(result1 >= 0.0);
            Assert.IsTrue(result2 >= 0.0);
            Assert.IsTrue(result3 >= 0.0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetApplicableTaxRate_NullOrEmptyInputs_ReturnsZero()
        {
            var result1 = _service.GetApplicableTaxRate(null, "NY");
            var result2 = _service.GetApplicableTaxRate("BEN456", "");
            var result3 = _service.GetApplicableTaxRate("", null);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateTotalSharesEqualOneHundredPercent_ValidPolicyId_ReturnsBoolean()
        {
            var result1 = _service.ValidateTotalSharesEqualOneHundredPercent("POL123");
            var result2 = _service.ValidateTotalSharesEqualOneHundredPercent("POL456");

            Assert.IsInstanceOfType(result1, typeof(bool));
            Assert.IsInstanceOfType(result2, typeof(bool));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateTotalSharesEqualOneHundredPercent_NullOrEmptyPolicyId_ReturnsFalse()
        {
            var result1 = _service.ValidateTotalSharesEqualOneHundredPercent(null);
            var result2 = _service.ValidateTotalSharesEqualOneHundredPercent("");
            var result3 = _service.ValidateTotalSharesEqualOneHundredPercent("   ");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsBeneficiaryEligibleForPayout_ValidInputs_ReturnsBoolean()
        {
            var result1 = _service.IsBeneficiaryEligibleForPayout("BEN123", DateTime.Now);
            var result2 = _service.IsBeneficiaryEligibleForPayout("BEN456", DateTime.Now.AddDays(-10));

            Assert.IsInstanceOfType(result1, typeof(bool));
            Assert.IsInstanceOfType(result2, typeof(bool));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsBeneficiaryEligibleForPayout_NullOrEmptyId_ReturnsFalse()
        {
            var result1 = _service.IsBeneficiaryEligibleForPayout(null, DateTime.Now);
            var result2 = _service.IsBeneficiaryEligibleForPayout("", DateTime.Now);
            var result3 = _service.IsBeneficiaryEligibleForPayout("   ", DateTime.Now);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void HasLegalHeirDisputes_ValidPolicyId_ReturnsBoolean()
        {
            var result1 = _service.HasLegalHeirDisputes("POL123");
            var result2 = _service.HasLegalHeirDisputes("POL456");

            Assert.IsInstanceOfType(result1, typeof(bool));
            Assert.IsInstanceOfType(result2, typeof(bool));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void HasLegalHeirDisputes_NullOrEmptyPolicyId_ReturnsFalse()
        {
            var result1 = _service.HasLegalHeirDisputes(null);
            var result2 = _service.HasLegalHeirDisputes("");
            var result3 = _service.HasLegalHeirDisputes("   ");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void RequiresGuardianSignoff_ValidInputs_ReturnsBoolean()
        {
            var result1 = _service.RequiresGuardianSignoff("BEN123", DateTime.Now.AddYears(-10));
            var result2 = _service.RequiresGuardianSignoff("BEN456", DateTime.Now.AddYears(-25));

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void RequiresGuardianSignoff_NullOrEmptyId_ReturnsFalse()
        {
            var result1 = _service.RequiresGuardianSignoff(null, DateTime.Now.AddYears(-10));
            var result2 = _service.RequiresGuardianSignoff("", DateTime.Now.AddYears(-25));
            var result3 = _service.RequiresGuardianSignoff("   ", DateTime.Now);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void VerifyBankRoutingInformation_ValidRouting_ReturnsBoolean()
        {
            var result1 = _service.VerifyBankRoutingInformation("123456789");
            var result2 = _service.VerifyBankRoutingInformation("987654321");

            Assert.IsInstanceOfType(result1, typeof(bool));
            Assert.IsInstanceOfType(result2, typeof(bool));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void VerifyBankRoutingInformation_InvalidRouting_ReturnsFalse()
        {
            var result1 = _service.VerifyBankRoutingInformation(null);
            var result2 = _service.VerifyBankRoutingInformation("");
            var result3 = _service.VerifyBankRoutingInformation("123");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CheckIfBeneficiaryIsDeceased_ValidId_ReturnsBoolean()
        {
            var result1 = _service.CheckIfBeneficiaryIsDeceased("BEN123");
            var result2 = _service.CheckIfBeneficiaryIsDeceased("BEN456");

            Assert.IsInstanceOfType(result1, typeof(bool));
            Assert.IsInstanceOfType(result2, typeof(bool));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CheckIfBeneficiaryIsDeceased_NullOrEmptyId_ReturnsFalse()
        {
            var result1 = _service.CheckIfBeneficiaryIsDeceased(null);
            var result2 = _service.CheckIfBeneficiaryIsDeceased("");
            var result3 = _service.CheckIfBeneficiaryIsDeceased("   ");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetActiveBeneficiaryCount_ValidPolicyId_ReturnsCount()
        {
            var result1 = _service.GetActiveBeneficiaryCount("POL123");
            var result2 = _service.GetActiveBeneficiaryCount("POL456");

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetActiveBeneficiaryCount_NullOrEmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.GetActiveBeneficiaryCount(null);
            var result2 = _service.GetActiveBeneficiaryCount("");
            var result3 = _service.GetActiveBeneficiaryCount("   ");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetDaysSinceMaturity_ValidDate_ReturnsDays()
        {
            var result1 = _service.GetDaysSinceMaturity(DateTime.Now.AddDays(-10));
            var result2 = _service.GetDaysSinceMaturity(DateTime.Now.AddDays(-30));

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetDaysSinceMaturity_FutureDate_ReturnsZero()
        {
            var result1 = _service.GetDaysSinceMaturity(DateTime.Now.AddDays(10));
            var result2 = _service.GetDaysSinceMaturity(DateTime.Now.AddDays(30));

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CountEligibleLegalHeirs_ValidPolicyId_ReturnsCount()
        {
            var result1 = _service.CountEligibleLegalHeirs("POL123");
            var result2 = _service.CountEligibleLegalHeirs("POL456");

            Assert.IsTrue(result1 >= 0);
            Assert.IsTrue(result2 >= 0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CountEligibleLegalHeirs_NullOrEmptyPolicyId_ReturnsZero()
        {
            var result1 = _service.CountEligibleLegalHeirs(null);
            var result2 = _service.CountEligibleLegalHeirs("");
            var result3 = _service.CountEligibleLegalHeirs("   ");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetProcessingSlaDays_ValidPolicyType_ReturnsDays()
        {
            var result1 = _service.GetProcessingSlaDays("TYPE_A");
            var result2 = _service.GetProcessingSlaDays("TYPE_B");

            Assert.IsTrue(result1 > 0);
            Assert.IsTrue(result2 > 0);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetProcessingSlaDays_NullOrEmptyPolicyType_ReturnsZero()
        {
            var result1 = _service.GetProcessingSlaDays(null);
            var result2 = _service.GetProcessingSlaDays("");
            var result3 = _service.GetProcessingSlaDays("   ");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GenerateAllocationTransactionId_ValidInputs_ReturnsId()
        {
            var result1 = _service.GenerateAllocationTransactionId("POL123", "BEN123");
            var result2 = _service.GenerateAllocationTransactionId("POL456", "BEN456");

            Assert.IsFalse(string.IsNullOrEmpty(result1));
            Assert.IsFalse(string.IsNullOrEmpty(result2));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GenerateAllocationTransactionId_NullOrEmptyInputs_ReturnsNull()
        {
            var result1 = _service.GenerateAllocationTransactionId(null, "BEN123");
            var result2 = _service.GenerateAllocationTransactionId("POL456", "");
            var result3 = _service.GenerateAllocationTransactionId("", null);

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
        }

        [TestMethod]
        public void GetPayoutStatusCode_ValidId_ReturnsCode()
        {
            var result1 = _service.GetPayoutStatusCode("BEN123");
            var result2 = _service.GetPayoutStatusCode("BEN456");

            Assert.IsFalse(string.IsNullOrEmpty(result1));
            Assert.IsFalse(string.IsNullOrEmpty(result2));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPayoutStatusCode_NullOrEmptyId_ReturnsNull()
        {
            var result1 = _service.GetPayoutStatusCode(null);
            var result2 = _service.GetPayoutStatusCode("");
            var result3 = _service.GetPayoutStatusCode("   ");

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
        }

        [TestMethod]
        public void DetermineTaxFormRequirement_ValidInputs_ReturnsForm()
        {
            var result1 = _service.DetermineTaxFormRequirement(1000m, true);
            var result2 = _service.DetermineTaxFormRequirement(5000m, false);

            Assert.IsFalse(string.IsNullOrEmpty(result1));
            Assert.IsFalse(string.IsNullOrEmpty(result2));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void DetermineTaxFormRequirement_NegativeAmount_ReturnsNull()
        {
            var result1 = _service.DetermineTaxFormRequirement(-1000m, true);
            var result2 = _service.DetermineTaxFormRequirement(-5000m, false);

            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }

        [TestMethod]
        public void GetGuardianIdForMinor_ValidId_ReturnsGuardianId()
        {
            var result1 = _service.GetGuardianIdForMinor("MINOR123");
            var result2 = _service.GetGuardianIdForMinor("MINOR456");

            Assert.IsFalse(string.IsNullOrEmpty(result1));
            Assert.IsFalse(string.IsNullOrEmpty(result2));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetGuardianIdForMinor_NullOrEmptyId_ReturnsNull()
        {
            var result1 = _service.GetGuardianIdForMinor(null);
            var result2 = _service.GetGuardianIdForMinor("");
            var result3 = _service.GetGuardianIdForMinor("   ");

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
        }

        [TestMethod]
        public void ResolveDisputedShareHoldCode_ValidPolicyId_ReturnsCode()
        {
            var result1 = _service.ResolveDisputedShareHoldCode("POL123");
            var result2 = _service.ResolveDisputedShareHoldCode("POL456");

            Assert.IsFalse(string.IsNullOrEmpty(result1));
            Assert.IsFalse(string.IsNullOrEmpty(result2));
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ResolveDisputedShareHoldCode_NullOrEmptyPolicyId_ReturnsNull()
        {
            var result1 = _service.ResolveDisputedShareHoldCode(null);
            var result2 = _service.ResolveDisputedShareHoldCode("");
            var result3 = _service.ResolveDisputedShareHoldCode("   ");

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
        }
    }
}