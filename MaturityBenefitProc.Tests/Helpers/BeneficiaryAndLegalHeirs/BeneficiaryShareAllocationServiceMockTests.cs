using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs;

namespace MaturityBenefitProc.Tests.Helpers.BeneficiaryAndLegalHeirs
{
    [TestClass]
    public class BeneficiaryShareAllocationServiceMockTests
    {
        private Mock<IBeneficiaryShareAllocationService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IBeneficiaryShareAllocationService>();
        }

        [TestMethod]
        public void CalculateBasePayoutAmount_ValidInputs_ReturnsExpectedAmount()
        {
            string policyId = "POL123";
            decimal totalBenefit = 10000m;
            decimal expected = 9500m;

            _mockService.Setup(s => s.CalculateBasePayoutAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateBasePayoutAmount(policyId, totalBenefit);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(totalBenefit, result);

            _mockService.Verify(s => s.CalculateBasePayoutAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBasePayoutAmount_ZeroBenefit_ReturnsZero()
        {
            string policyId = "POL124";
            decimal totalBenefit = 0m;
            decimal expected = 0m;

            _mockService.Setup(s => s.CalculateBasePayoutAmount(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.CalculateBasePayoutAmount(policyId, totalBenefit);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);

            _mockService.Verify(s => s.CalculateBasePayoutAmount(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBeneficiaryShareAmount_ValidShare_ReturnsExpectedAmount()
        {
            decimal totalAmount = 10000m;
            double sharePercentage = 50.0;
            decimal expected = 5000m;

            _mockService.Setup(s => s.CalculateBeneficiaryShareAmount(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateBeneficiaryShareAmount(totalAmount, sharePercentage);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateBeneficiaryShareAmount(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateBeneficiaryShareAmount_ZeroShare_ReturnsZero()
        {
            decimal totalAmount = 10000m;
            double sharePercentage = 0.0;
            decimal expected = 0m;

            _mockService.Setup(s => s.CalculateBeneficiaryShareAmount(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.CalculateBeneficiaryShareAmount(totalAmount, sharePercentage);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result > 0);

            _mockService.Verify(s => s.CalculateBeneficiaryShareAmount(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void ApplyTaxDeductionToShare_ValidTaxRate_ReturnsDeductedAmount()
        {
            decimal shareAmount = 5000m;
            double taxRate = 10.0;
            decimal expected = 4500m;

            _mockService.Setup(s => s.ApplyTaxDeductionToShare(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expected);

            var result = _mockService.Object.ApplyTaxDeductionToShare(shareAmount, taxRate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < shareAmount);
            Assert.AreNotEqual(shareAmount, result);

            _mockService.Verify(s => s.ApplyTaxDeductionToShare(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLateInterest_ValidDays_ReturnsExpectedInterest()
        {
            decimal baseAmount = 10000m;
            double interestRate = 5.0;
            int daysLate = 30;
            decimal expected = 41.10m;

            _mockService.Setup(s => s.CalculateLateInterest(It.IsAny<decimal>(), It.IsAny<double>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.CalculateLateInterest(baseAmount, interestRate, daysLate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateLateInterest(It.IsAny<decimal>(), It.IsAny<double>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalAllocatedFunds_ValidPolicy_ReturnsTotal()
        {
            string policyId = "POL123";
            decimal expected = 10000m;

            _mockService.Setup(s => s.GetTotalAllocatedFunds(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetTotalAllocatedFunds(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.GetTotalAllocatedFunds(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateLegalHeirDisputeReserve_ValidDisputes_ReturnsReserveAmount()
        {
            decimal totalBenefit = 10000m;
            int disputingHeirsCount = 2;
            decimal expected = 2000m;

            _mockService.Setup(s => s.CalculateLegalHeirDisputeReserve(It.IsAny<decimal>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.CalculateLegalHeirDisputeReserve(totalBenefit, disputingHeirsCount);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0m, result);

            _mockService.Verify(s => s.CalculateLegalHeirDisputeReserve(It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void AdjustPayoutForOutstandingPremiums_ValidDebt_ReturnsAdjustedAmount()
        {
            decimal payoutAmount = 10000m;
            decimal outstandingDebt = 500m;
            decimal expected = 9500m;

            _mockService.Setup(s => s.AdjustPayoutForOutstandingPremiums(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(expected);

            var result = _mockService.Object.AdjustPayoutForOutstandingPremiums(payoutAmount, outstandingDebt);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result < payoutAmount);
            Assert.AreNotEqual(payoutAmount, result);

            _mockService.Verify(s => s.AdjustPayoutForOutstandingPremiums(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void GetPrimaryBeneficiarySharePercentage_ValidBeneficiary_ReturnsPercentage()
        {
            string beneficiaryId = "BEN123";
            double expected = 50.0;

            _mockService.Setup(s => s.GetPrimaryBeneficiarySharePercentage(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetPrimaryBeneficiarySharePercentage(beneficiaryId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetPrimaryBeneficiarySharePercentage(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetContingentBeneficiarySharePercentage_ValidBeneficiary_ReturnsPercentage()
        {
            string beneficiaryId = "BEN124";
            double expected = 25.0;

            _mockService.Setup(s => s.GetContingentBeneficiarySharePercentage(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetContingentBeneficiarySharePercentage(beneficiaryId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetContingentBeneficiarySharePercentage(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateRemainingSharePool_ValidPolicy_ReturnsRemainingPercentage()
        {
            string policyId = "POL123";
            double expected = 25.0;

            _mockService.Setup(s => s.CalculateRemainingSharePool(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CalculateRemainingSharePool(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.CalculateRemainingSharePool(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void AdjustShareForMinorBeneficiary_MinorAge_ReturnsAdjustedShare()
        {
            double originalShare = 50.0;
            int age = 15;
            double expected = 50.0;

            _mockService.Setup(s => s.AdjustShareForMinorBeneficiary(It.IsAny<double>(), It.IsAny<int>())).Returns(expected);

            var result = _mockService.Object.AdjustShareForMinorBeneficiary(originalShare, age);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.AdjustShareForMinorBeneficiary(It.IsAny<double>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetApplicableTaxRate_ValidState_ReturnsTaxRate()
        {
            string beneficiaryId = "BEN123";
            string stateCode = "NY";
            double expected = 8.5;

            _mockService.Setup(s => s.GetApplicableTaxRate(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetApplicableTaxRate(beneficiaryId, stateCode);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0.0, result);

            _mockService.Verify(s => s.GetApplicableTaxRate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ValidateTotalSharesEqualOneHundredPercent_ValidShares_ReturnsTrue()
        {
            string policyId = "POL123";
            bool expected = true;

            _mockService.Setup(s => s.ValidateTotalSharesEqualOneHundredPercent(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.ValidateTotalSharesEqualOneHundredPercent(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.ValidateTotalSharesEqualOneHundredPercent(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsBeneficiaryEligibleForPayout_Eligible_ReturnsTrue()
        {
            string beneficiaryId = "BEN123";
            DateTime dateOfDeath = new DateTime(2023, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.IsBeneficiaryEligibleForPayout(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.IsBeneficiaryEligibleForPayout(beneficiaryId, dateOfDeath);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.IsBeneficiaryEligibleForPayout(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void HasLegalHeirDisputes_HasDisputes_ReturnsTrue()
        {
            string policyId = "POL123";
            bool expected = true;

            _mockService.Setup(s => s.HasLegalHeirDisputes(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.HasLegalHeirDisputes(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.HasLegalHeirDisputes(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RequiresGuardianSignoff_Minor_ReturnsTrue()
        {
            string beneficiaryId = "BEN123";
            DateTime birthDate = new DateTime(2010, 1, 1);
            bool expected = true;

            _mockService.Setup(s => s.RequiresGuardianSignoff(It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.RequiresGuardianSignoff(beneficiaryId, birthDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.RequiresGuardianSignoff(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void VerifyBankRoutingInformation_ValidRouting_ReturnsTrue()
        {
            string routingNumber = "123456789";
            bool expected = true;

            _mockService.Setup(s => s.VerifyBankRoutingInformation(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.VerifyBankRoutingInformation(routingNumber);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.VerifyBankRoutingInformation(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CheckIfBeneficiaryIsDeceased_Deceased_ReturnsTrue()
        {
            string beneficiaryId = "BEN123";
            bool expected = true;

            _mockService.Setup(s => s.CheckIfBeneficiaryIsDeceased(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CheckIfBeneficiaryIsDeceased(beneficiaryId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreNotEqual(false, result);

            _mockService.Verify(s => s.CheckIfBeneficiaryIsDeceased(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetActiveBeneficiaryCount_ValidPolicy_ReturnsCount()
        {
            string policyId = "POL123";
            int expected = 3;

            _mockService.Setup(s => s.GetActiveBeneficiaryCount(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetActiveBeneficiaryCount(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetActiveBeneficiaryCount(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysSinceMaturity_ValidDate_ReturnsDays()
        {
            DateTime maturityDate = new DateTime(2023, 1, 1);
            int expected = 30;

            _mockService.Setup(s => s.GetDaysSinceMaturity(It.IsAny<DateTime>())).Returns(expected);

            var result = _mockService.Object.GetDaysSinceMaturity(maturityDate);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetDaysSinceMaturity(It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CountEligibleLegalHeirs_ValidPolicy_ReturnsCount()
        {
            string policyId = "POL123";
            int expected = 2;

            _mockService.Setup(s => s.CountEligibleLegalHeirs(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.CountEligibleLegalHeirs(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.CountEligibleLegalHeirs(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetProcessingSlaDays_ValidType_ReturnsDays()
        {
            string policyType = "LIFE";
            int expected = 15;

            _mockService.Setup(s => s.GetProcessingSlaDays(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetProcessingSlaDays(policyType);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
            Assert.AreNotEqual(0, result);

            _mockService.Verify(s => s.GetProcessingSlaDays(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateAllocationTransactionId_ValidInputs_ReturnsTransactionId()
        {
            string policyId = "POL123";
            string beneficiaryId = "BEN123";
            string expected = "TXN-POL123-BEN123";

            _mockService.Setup(s => s.GenerateAllocationTransactionId(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GenerateAllocationTransactionId(policyId, beneficiaryId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Contains("TXN"));

            _mockService.Verify(s => s.GenerateAllocationTransactionId(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetPayoutStatusCode_ValidBeneficiary_ReturnsStatusCode()
        {
            string beneficiaryId = "BEN123";
            string expected = "PENDING";

            _mockService.Setup(s => s.GetPayoutStatusCode(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetPayoutStatusCode(beneficiaryId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.GetPayoutStatusCode(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void DetermineTaxFormRequirement_ForeignNational_ReturnsFormW8()
        {
            decimal payoutAmount = 10000m;
            bool isForeignNational = true;
            string expected = "W-8BEN";

            _mockService.Setup(s => s.DetermineTaxFormRequirement(It.IsAny<decimal>(), It.IsAny<bool>())).Returns(expected);

            var result = _mockService.Object.DetermineTaxFormRequirement(payoutAmount, isForeignNational);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("W-9", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.DetermineTaxFormRequirement(It.IsAny<decimal>(), It.IsAny<bool>()), Times.Once());
        }

        [TestMethod]
        public void GetGuardianIdForMinor_ValidMinor_ReturnsGuardianId()
        {
            string minorBeneficiaryId = "MIN123";
            string expected = "GRD123";

            _mockService.Setup(s => s.GetGuardianIdForMinor(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.GetGuardianIdForMinor(minorBeneficiaryId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.GetGuardianIdForMinor(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ResolveDisputedShareHoldCode_ValidPolicy_ReturnsHoldCode()
        {
            string policyId = "POL123";
            string expected = "HOLD-DISPUTE";

            _mockService.Setup(s => s.ResolveDisputedShareHoldCode(It.IsAny<string>())).Returns(expected);

            var result = _mockService.Object.ResolveDisputedShareHoldCode(policyId);

            Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
            Assert.IsTrue(result.Length > 0);

            _mockService.Verify(s => s.ResolveDisputedShareHoldCode(It.IsAny<string>()), Times.Once());
        }
    }
}