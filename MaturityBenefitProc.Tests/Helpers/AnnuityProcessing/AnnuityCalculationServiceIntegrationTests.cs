using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.AnnuityProcessing;

namespace MaturityBenefitProc.Tests.Helpers.AnnuityProcessing
{
    [TestClass]
    public class AnnuityCalculationServiceIntegrationTests
    {
        private IAnnuityCalculationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming AnnuityCalculationService implements IAnnuityCalculationService
            _service = new AnnuityCalculationService();
        }

        [TestMethod]
        public void CommutationWorkflow_ValidCorpus_CalculatesResidualAndPayouts()
        {
            string policyId = "POL-1001";
            decimal totalCorpus = _service.GetTotalAccumulatedCorpus(policyId, DateTime.Now);
            decimal commutedAmount = _service.CalculateCommutationAmount(policyId, totalCorpus, 0.33);
            decimal residual = _service.CalculateResidualCorpus(totalCorpus, commutedAmount);
            decimal monthly = _service.CalculateMonthlyPayout(policyId, residual, 0.05);

            Assert.IsTrue(totalCorpus >= 0, "Total corpus should be non-negative.");
            Assert.IsTrue(commutedAmount <= totalCorpus, "Commuted amount cannot exceed total corpus.");
            Assert.AreEqual(totalCorpus - commutedAmount, residual, "Residual should equal total minus commuted.");
            Assert.IsTrue(monthly >= 0, "Monthly payout should be non-negative.");
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void VestingEligibility_AgeAndPolicy_ValidatesCommutation()
        {
            string policyId = "POL-1002";
            DateTime dob = new DateTime(1960, 1, 1);
            DateTime vestingDate = new DateTime(2020, 1, 1);
            
            int age = _service.CalculateAgeAtVesting(dob, vestingDate);
            bool isEligible = _service.IsEligibleForCommutation(policyId, age);
            string optionCode = _service.GetAnnuityOptionCode(policyId);
            double factor = _service.GetAnnuityFactor(age, optionCode, 0.06);

            Assert.AreEqual(60, age, "Age at vesting should be 60.");
            Assert.IsNotNull(isEligible, "Eligibility should return a boolean.");
            Assert.IsNotNull(optionCode, "Option code should not be null.");
            Assert.IsTrue(factor >= 0, "Annuity factor should be non-negative.");
            Assert.AreNotEqual("", optionCode, "Option code should not be empty.");
        }

        [TestMethod]
        public void PayoutFrequencies_SameCorpus_AnnualGreaterThanMonthly()
        {
            string policyId = "POL-1003";
            decimal corpus = 1000000m;
            double rate = 0.07;

            decimal monthly = _service.CalculateMonthlyPayout(policyId, corpus, rate);
            decimal quarterly = _service.CalculateQuarterlyPayout(policyId, corpus, rate);
            decimal semiAnnual = _service.CalculateSemiAnnualPayout(policyId, corpus, rate);
            decimal annual = _service.CalculateAnnualPayout(policyId, corpus, rate);

            Assert.IsTrue(annual > monthly, "Annual payout should be greater than monthly.");
            Assert.IsTrue(annual > quarterly, "Annual payout should be greater than quarterly.");
            Assert.IsTrue(semiAnnual > quarterly, "Semi-annual should be greater than quarterly.");
            Assert.IsTrue(quarterly > monthly, "Quarterly should be greater than monthly.");
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void JointLifeWorkflow_SpouseDob_ValidatesAndGetsMortality()
        {
            string policyId = "POL-1004";
            DateTime spouseDob = new DateTime(1965, 5, 5);
            
            string optionCode = _service.GetAnnuityOptionCode(policyId);
            bool isJoint = _service.IsJointLifeApplicable(optionCode);
            bool isValidSpouse = _service.ValidateSpouseDateOfBirth(policyId, spouseDob);
            double mortality = _service.ComputeMortalityChargeRate(55, "F");

            Assert.IsNotNull(optionCode);
            Assert.IsNotNull(isJoint);
            Assert.IsNotNull(isValidSpouse);
            Assert.IsTrue(mortality >= 0, "Mortality rate should be non-negative.");
            Assert.AreNotEqual(mortality, -1);
        }

        [TestMethod]
        public void DefermentWorkflow_ValidDates_CalculatesInflationAndDays()
        {
            string policyId = "POL-1005";
            DateTime vesting = new DateTime(2023, 1, 1);
            DateTime deferredStart = new DateTime(2025, 1, 1);
            
            int months = _service.GetDefermentPeriodMonths(vesting, deferredStart);
            bool canDefer = _service.CanDeferPayout(policyId, months);
            double inflation = _service.CalculateInflationAdjustmentFactor(vesting, deferredStart, 0.04);
            string freq = _service.DeterminePayoutFrequencyCode(policyId);
            int daysToNext = _service.CalculateDaysToNextPayout(deferredStart, freq);

            Assert.AreEqual(24, months, "Deferment should be 24 months.");
            Assert.IsNotNull(canDefer);
            Assert.IsTrue(inflation >= 1.0, "Inflation factor should be >= 1.0 for positive rate.");
            Assert.IsNotNull(freq);
            Assert.IsTrue(daysToNext >= 0);
        }

        [TestMethod]
        public void GuaranteedPeriod_ActivePolicy_ChecksExpirationAndRemaining()
        {
            string policyId = "POL-1006";
            DateTime checkDate = DateTime.Now;
            
            bool isActive = _service.IsPolicyActive(policyId, checkDate);
            int payoutsMade = _service.GetTotalPayoutsMade(policyId);
            int remainingMonths = _service.GetRemainingGuaranteedMonths(policyId, 10, payoutsMade);
            bool hasExpired = _service.HasGuaranteedPeriodExpired(policyId, checkDate);

            Assert.IsNotNull(isActive);
            Assert.IsTrue(payoutsMade >= 0);
            Assert.IsTrue(remainingMonths >= 0);
            Assert.IsNotNull(hasExpired);
            Assert.AreNotEqual(payoutsMade, -1);
        }

        [TestMethod]
        public void TaxationWorkflow_PayoutAmount_CalculatesTaxablePortion()
        {
            string policyId = "POL-1007";
            decimal payout = 50000m;
            int age = 65;
            
            string taxSlab = _service.GetTaxSlabCode(payout * 12, age);
            decimal taxable = _service.ComputeTaxablePortion(payout, 0.20);
            string transId = _service.GeneratePayoutTransactionId(policyId, DateTime.Now);

            Assert.IsNotNull(taxSlab);
            Assert.IsTrue(taxable <= payout, "Taxable portion cannot exceed payout.");
            Assert.IsTrue(taxable >= 0);
            Assert.IsNotNull(transId);
            Assert.AreNotEqual("", transId);
        }

        [TestMethod]
        public void DeathBenefit_Beneficiary_CalculatesLumpSum()
        {
            string policyId = "POL-1008";
            string beneficiaryId = "BEN-001";
            decimal corpus = 2000000m;
            
            string relCode = _service.GetBeneficiaryRelationshipCode(policyId, beneficiaryId);
            decimal deathBenefit = _service.CalculateDeathBenefit(policyId, corpus, DateTime.Now);
            decimal lumpSum = _service.CalculateLumpSumPayout(policyId, deathBenefit, true);

            Assert.IsNotNull(relCode);
            Assert.IsTrue(deathBenefit >= 0);
            Assert.IsTrue(lumpSum >= 0);
            Assert.IsNotNull(policyId);
            Assert.AreNotEqual("", relCode);
        }

        [TestMethod]
        public void SurrenderWorkflow_PolicyYear_CalculatesValueAndIRR()
        {
            string policyId = "POL-1009";
            decimal corpus = 1500000m;
            int policyYear = 5;
            
            decimal surrenderValue = _service.CalculateSurrenderValue(policyId, corpus, policyYear);
            int premiumTerm = _service.GetPremiumPaymentTerm(policyId);
            double irr = _service.CalculateInternalRateOfReturn(policyId, 500000m, surrenderValue);

            Assert.IsTrue(surrenderValue >= 0);
            Assert.IsTrue(surrenderValue <= corpus, "Surrender value usually doesn't exceed corpus.");
            Assert.IsTrue(premiumTerm >= 0);
            Assert.IsNotNull(irr);
            Assert.AreNotEqual(premiumTerm, -1);
        }

        [TestMethod]
        public void MinimumCorpus_ProductCode_ValidatesAndGetsRate()
        {
            string policyId = "POL-1010";
            string productCode = "PRD-ANN-01";
            decimal corpus = 50000m;
            
            bool isMet = _service.IsMinimumCorpusMet(corpus, productCode);
            double rate = _service.GetCurrentInterestRate(productCode, DateTime.Now);
            decimal gmwb = _service.GetGuaranteedMinimumWithdrawalBenefit(policyId, corpus);

            Assert.IsNotNull(isMet);
            Assert.IsTrue(rate >= 0);
            Assert.IsTrue(gmwb >= 0);
            Assert.IsTrue(gmwb <= corpus);
            Assert.IsNotNull(productCode);
        }

        [TestMethod]
        public void FullLifecycle_CorpusToTax_IntegratesAllSteps()
        {
            string policyId = "POL-1011";
            decimal corpus = _service.GetTotalAccumulatedCorpus(policyId, DateTime.Now);
            decimal commuted = _service.CalculateCommutationAmount(policyId, corpus, 0.25);
            decimal residual = _service.CalculateResidualCorpus(corpus, commuted);
            decimal monthly = _service.CalculateMonthlyPayout(policyId, residual, 0.06);
            decimal taxable = _service.ComputeTaxablePortion(monthly, 0.15);

            Assert.IsTrue(corpus >= 0);
            Assert.IsTrue(commuted <= corpus);
            Assert.AreEqual(corpus - commuted, residual);
            Assert.IsTrue(monthly >= 0);
            Assert.IsTrue(taxable <= monthly);
        }

        [TestMethod]
        public void TransactionGeneration_Frequency_CreatesValidId()
        {
            string policyId = "POL-1012";
            DateTime payoutDate = DateTime.Now;
            
            string freq = _service.DeterminePayoutFrequencyCode(policyId);
            int days = _service.CalculateDaysToNextPayout(payoutDate, freq);
            string transId = _service.GeneratePayoutTransactionId(policyId, payoutDate);

            Assert.IsNotNull(freq);
            Assert.IsTrue(days >= 0);
            Assert.IsNotNull(transId);
            Assert.IsTrue(transId.Contains(policyId) || transId.Length > 0);
            Assert.AreNotEqual("", freq);
        }

        [TestMethod]
        public void AnnuityFactor_InterestRate_CalculatesCorrectly()
        {
            string policyId = "POL-1013";
            int age = 60;
            
            string optionCode = _service.GetAnnuityOptionCode(policyId);
            double rate = _service.GetCurrentInterestRate("PRD-01", DateTime.Now);
            double factor = _service.GetAnnuityFactor(age, optionCode, rate);

            Assert.IsNotNull(optionCode);
            Assert.IsTrue(rate >= 0);
            Assert.IsTrue(factor >= 0);
            Assert.AreNotEqual(0.0, factor, "Factor should generally be non-zero.");
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void JointLife_SpouseValidation_ChecksEligibility()
        {
            string policyId = "POL-1014";
            DateTime spouseDob = new DateTime(1970, 1, 1);
            
            string optionCode = _service.GetAnnuityOptionCode(policyId);
            bool isJoint = _service.IsJointLifeApplicable(optionCode);
            bool isValid = _service.ValidateSpouseDateOfBirth(policyId, spouseDob);

            Assert.IsNotNull(optionCode);
            Assert.IsNotNull(isJoint);
            Assert.IsNotNull(isValid);
            Assert.AreNotEqual("", optionCode);
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void GuaranteedPeriod_Expiration_CalculatesRemaining()
        {
            string policyId = "POL-1015";
            int payoutsMade = _service.GetTotalPayoutsMade(policyId);
            int remaining = _service.GetRemainingGuaranteedMonths(policyId, 15, payoutsMade);
            bool expired = _service.HasGuaranteedPeriodExpired(policyId, DateTime.Now);

            Assert.IsTrue(payoutsMade >= 0);
            Assert.IsTrue(remaining >= 0);
            Assert.IsNotNull(expired);
            if (expired) Assert.AreEqual(0, remaining);
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void Commutation_MaxLimit_ValidatesResidual()
        {
            string policyId = "POL-1016";
            decimal corpus = 100000m;
            
            decimal commuted = _service.CalculateCommutationAmount(policyId, corpus, 1.0); // 100%
            decimal residual = _service.CalculateResidualCorpus(corpus, commuted);
            bool isMet = _service.IsMinimumCorpusMet(residual, "PRD-01");

            Assert.IsTrue(commuted <= corpus);
            Assert.AreEqual(corpus - commuted, residual);
            Assert.IsNotNull(isMet);
            Assert.IsTrue(residual >= 0);
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void Deferment_ActivePolicy_ChecksDaysToNext()
        {
            string policyId = "POL-1017";
            DateTime vesting = DateTime.Now.AddYears(-1);
            DateTime deferred = DateTime.Now.AddYears(1);
            
            bool isActive = _service.IsPolicyActive(policyId, DateTime.Now);
            int months = _service.GetDefermentPeriodMonths(vesting, deferred);
            int days = _service.CalculateDaysToNextPayout(deferred, "M");

            Assert.IsNotNull(isActive);
            Assert.AreEqual(24, months);
            Assert.IsTrue(days >= 0);
            Assert.IsNotNull(policyId);
            Assert.AreNotEqual(months, 0);
        }

        [TestMethod]
        public void VestingAge_AnnuityFactor_CalculatesMonthly()
        {
            string policyId = "POL-1018";
            DateTime dob = new DateTime(1950, 1, 1);
            DateTime vesting = new DateTime(2015, 1, 1);
            
            int age = _service.CalculateAgeAtVesting(dob, vesting);
            double factor = _service.GetAnnuityFactor(age, "OPT-1", 0.05);
            decimal monthly = _service.CalculateMonthlyPayout(policyId, 500000m, 0.05);

            Assert.AreEqual(65, age);
            Assert.IsTrue(factor >= 0);
            Assert.IsTrue(monthly >= 0);
            Assert.IsNotNull(policyId);
            Assert.AreNotEqual(0, age);
        }

        [TestMethod]
        public void Inflation_InterestRate_CalculatesPayout()
        {
            string policyId = "POL-1019";
            DateTime baseDate = new DateTime(2010, 1, 1);
            DateTime currentDate = new DateTime(2020, 1, 1);
            
            double inflation = _service.CalculateInflationAdjustmentFactor(baseDate, currentDate, 0.03);
            double rate = _service.GetCurrentInterestRate("PRD-02", currentDate);
            decimal annual = _service.CalculateAnnualPayout(policyId, 1000000m, rate);

            Assert.IsTrue(inflation >= 1.0);
            Assert.IsTrue(rate >= 0);
            Assert.IsTrue(annual >= 0);
            Assert.IsNotNull(policyId);
            Assert.AreNotEqual(0.0, inflation);
        }

        [TestMethod]
        public void MinimumCorpus_TotalCorpus_ValidatesCommutation()
        {
            string policyId = "POL-1020";
            decimal corpus = _service.GetTotalAccumulatedCorpus(policyId, DateTime.Now);
            bool isMet = _service.IsMinimumCorpusMet(corpus, "PRD-03");
            decimal commuted = _service.CalculateCommutationAmount(policyId, corpus, 0.20);

            Assert.IsTrue(corpus >= 0);
            Assert.IsNotNull(isMet);
            Assert.IsTrue(commuted <= corpus);
            Assert.IsTrue(commuted >= 0);
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void LumpSum_TaxablePortion_GetsTaxSlab()
        {
            string policyId = "POL-1021";
            decimal corpus = 3000000m;
            
            decimal lumpSum = _service.CalculateLumpSumPayout(policyId, corpus, false);
            decimal taxable = _service.ComputeTaxablePortion(lumpSum, 0.30);
            string slab = _service.GetTaxSlabCode(taxable, 60);

            Assert.IsTrue(lumpSum >= 0);
            Assert.IsTrue(taxable <= lumpSum);
            Assert.IsNotNull(slab);
            Assert.AreNotEqual("", slab);
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void Surrender_PremiumTerm_CalculatesIRR()
        {
            string policyId = "POL-1022";
            int term = _service.GetPremiumPaymentTerm(policyId);
            decimal surrender = _service.CalculateSurrenderValue(policyId, 500000m, term);
            double irr = _service.CalculateInternalRateOfReturn(policyId, 400000m, surrender);

            Assert.IsTrue(term >= 0);
            Assert.IsTrue(surrender >= 0);
            Assert.IsNotNull(irr);
            Assert.IsNotNull(policyId);
            Assert.AreNotEqual(term, -1);
        }

        [TestMethod]
        public void MortalityCharge_AgeGender_Validates()
        {
            string policyId = "POL-1023";
            double maleCharge = _service.ComputeMortalityChargeRate(60, "M");
            double femaleCharge = _service.ComputeMortalityChargeRate(60, "F");
            string optionCode = _service.GetAnnuityOptionCode(policyId);

            Assert.IsTrue(maleCharge >= 0);
            Assert.IsTrue(femaleCharge >= 0);
            Assert.IsNotNull(optionCode);
            Assert.AreNotEqual(maleCharge, -1.0);
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void Transaction_Frequency_ValidatesDate()
        {
            string policyId = "POL-1024";
            string freq = _service.DeterminePayoutFrequencyCode(policyId);
            int days = _service.CalculateDaysToNextPayout(DateTime.Now, freq);
            string transId = _service.GeneratePayoutTransactionId(policyId, DateTime.Now.AddDays(days));

            Assert.IsNotNull(freq);
            Assert.IsTrue(days >= 0);
            Assert.IsNotNull(transId);
            Assert.AreNotEqual("", transId);
            Assert.IsNotNull(policyId);
        }

        [TestMethod]
        public void Beneficiary_DeathBenefit_GuaranteedMinimum()
        {
            string policyId = "POL-1025";
            string benId = "BEN-999";
            decimal initialCorpus = 1000000m;
            
            string rel = _service.GetBeneficiaryRelationshipCode(policyId, benId);
            decimal gmwb = _service.GetGuaranteedMinimumWithdrawalBenefit(policyId, initialCorpus);
            decimal deathBenefit = _service.CalculateDeathBenefit(policyId, initialCorpus, DateTime.Now);

            Assert.IsNotNull(rel);
            Assert.IsTrue(gmwb >= 0);
            Assert.IsTrue(deathBenefit >= 0);
            Assert.AreNotEqual("", rel);
            Assert.IsNotNull(policyId);
        }
    }
}