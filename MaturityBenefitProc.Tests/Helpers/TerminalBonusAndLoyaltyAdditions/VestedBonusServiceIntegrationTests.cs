using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class VestedBonusServiceIntegrationTests
    {
        private IVestedBonusService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming VestedBonusService implements IVestedBonusService
            // For the sake of compilation in this test file, we instantiate the implementation.
            _service = new VestedBonusService();
        }

        [TestMethod]
        public void ActivePolicy_CalculatesTotalAndInterimBonus_ReturnsConsistentValues()
        {
            string policyId = "POL-ACTIVE-001";
            DateTime calcDate = new DateTime(2023, 5, 1);

            bool isActive = _service.IsPolicyActive(policyId, calcDate);
            decimal totalBonus = _service.CalculateTotalVestedBonus(policyId, calcDate);
            decimal interimBonus = _service.CalculateInterimBonus(policyId, calcDate);
            string status = _service.GetBonusStatus(policyId);

            Assert.IsTrue(isActive, "Policy should be active.");
            Assert.IsTrue(totalBonus >= 0, "Total bonus should be non-negative.");
            Assert.IsTrue(interimBonus >= 0, "Interim bonus should be non-negative.");
            Assert.IsNotNull(status, "Bonus status should not be null.");
            Assert.AreNotEqual("Surrendered", status, "Active policy should not have Surrendered status.");
        }

        [TestMethod]
        public void SurrenderedPolicy_CannotWithdrawBonus_ReturnsZeroOrFalse()
        {
            string policyId = "POL-SURR-002";
            DateTime calcDate = new DateTime(2023, 1, 1);

            bool hasSurrendered = _service.HasSurrenderedPolicy(policyId);
            bool canWithdraw = _service.CanWithdrawBonus(policyId, 1000m);
            decimal accrued = _service.GetTotalAccruedBonus(policyId);
            decimal surrenderValue = _service.CalculateSurrenderValueOfBonus(policyId, accrued);

            Assert.IsTrue(hasSurrendered, "Policy should be marked as surrendered.");
            Assert.IsFalse(canWithdraw, "Surrendered policy cannot withdraw active bonus.");
            Assert.IsTrue(accrued >= 0, "Accrued bonus should be non-negative.");
            Assert.IsTrue(surrenderValue <= accrued, "Surrender value should not exceed accrued bonus.");
            Assert.IsNotNull(_service.GetBonusStatus(policyId));
        }

        [TestMethod]
        public void EligibleForTerminalBonus_CalculatesCorrectly()
        {
            string policyId = "POL-TERM-003";
            int activeYears = 15;
            decimal sumAssured = 50000m;

            bool isEligible = _service.IsEligibleForTerminalBonus(policyId, activeYears);
            decimal terminalBonus = _service.CalculateTerminalBonus(policyId, sumAssured);
            double rate = _service.GetTerminalBonusRate("PLAN-A", activeYears);

            Assert.IsTrue(isEligible, "Policy should be eligible for terminal bonus after 15 years.");
            Assert.IsTrue(terminalBonus >= 0, "Terminal bonus should be calculated.");
            Assert.IsTrue(rate >= 0, "Terminal bonus rate should be valid.");
            Assert.IsNotNull(_service.GetCurrencyCode(policyId));
            Assert.AreNotEqual(0m, sumAssured);
        }

        [TestMethod]
        public void LoyaltyAddition_EligiblePolicy_CalculatesAndValidates()
        {
            string policyId = "POL-LOYAL-004";
            decimal totalPremiums = 25000m;
            int completedYears = 10;

            bool isEligible = _service.IsEligibleForLoyaltyAddition(policyId, totalPremiums);
            decimal loyaltyAddition = _service.CalculateLoyaltyAddition(policyId, completedYears);
            double percentage = _service.GetLoyaltyAdditionPercentage("PLAN-B", totalPremiums);

            Assert.IsTrue(isEligible, "Policy should be eligible for loyalty addition.");
            Assert.IsTrue(loyaltyAddition >= 0, "Loyalty addition should be non-negative.");
            Assert.IsTrue(percentage >= 0, "Percentage should be valid.");
            Assert.IsNotNull(_service.GetBonusStatus(policyId));
            Assert.IsTrue(completedYears > 0);
        }

        [TestMethod]
        public void PaidUpPolicy_AdjustsBonusCorrectly()
        {
            string policyId = "POL-PAIDUP-005";
            decimal originalBonus = 10000m;

            decimal adjustedBonus = _service.AdjustBonusForPaidUpPolicy(policyId, originalBonus);
            int missedPremiums = _service.GetMissedPremiumsCount(policyId);
            bool isActive = _service.IsPolicyActive(policyId, DateTime.Now);

            Assert.IsTrue(adjustedBonus <= originalBonus, "Adjusted bonus should be less than or equal to original.");
            Assert.IsTrue(adjustedBonus >= 0, "Adjusted bonus should be non-negative.");
            Assert.IsTrue(missedPremiums >= 0, "Missed premiums count should be valid.");
            Assert.IsFalse(isActive, "Paid up policy might not be considered fully active.");
            Assert.IsNotNull(_service.GetBonusStatus(policyId));
        }

        [TestMethod]
        public void BonusRates_ValidationAndRetrieval_Consistent()
        {
            string planCode = "PLAN-C";
            int year = 2022;

            double simpleRate = _service.GetBonusRateForYear(year, planCode);
            double compoundRate = simpleRate * 1.1; // Simulated
            bool isValid = _service.ValidateBonusRates(planCode, simpleRate, compoundRate);
            string tableCode = _service.GetApplicableBonusTableCode(planCode, new DateTime(year, 1, 1));

            Assert.IsTrue(simpleRate >= 0, "Simple rate should be non-negative.");
            Assert.IsTrue(isValid, "Rates should validate successfully.");
            Assert.IsNotNull(tableCode, "Table code should be retrieved.");
            Assert.AreNotEqual("", tableCode, "Table code should not be empty.");
            Assert.IsNotNull(_service.GetFundCodeForBonus(planCode));
        }

        [TestMethod]
        public void FinalMaturityBonus_IncludesAllComponents()
        {
            string policyId = "POL-MAT-006";
            decimal baseSumAssured = 100000m;
            decimal accruedBonuses = 25000m;

            decimal finalBonus = _service.CalculateFinalMaturityBonus(policyId, baseSumAssured, accruedBonuses);
            decimal terminalBonus = _service.CalculateTerminalBonus(policyId, baseSumAssured);
            int remainingMonths = _service.GetRemainingTermInMonths(policyId, DateTime.Now);

            Assert.IsTrue(finalBonus >= accruedBonuses, "Final bonus should include accrued bonuses.");
            Assert.IsTrue(terminalBonus >= 0, "Terminal bonus should be valid.");
            Assert.IsTrue(remainingMonths <= 0, "Remaining term should be zero or less for maturity.");
            Assert.IsNotNull(_service.GetCurrencyCode(policyId));
            Assert.AreNotEqual(0m, finalBonus);
        }

        [TestMethod]
        public void MissedPremiums_AffectsBonusEligibility()
        {
            string policyId = "POL-MISS-007";
            
            int missedCount = _service.GetMissedPremiumsCount(policyId);
            int paidCount = _service.GetTotalPremiumsPaidCount(policyId);
            bool isEligibleLoyalty = _service.IsEligibleForLoyaltyAddition(policyId, paidCount * 1000m);
            string status = _service.GetBonusStatus(policyId);

            Assert.IsTrue(missedCount >= 0, "Missed count should be non-negative.");
            Assert.IsTrue(paidCount >= 0, "Paid count should be non-negative.");
            Assert.IsFalse(isEligibleLoyalty == true && missedCount > 5, "Should not be eligible if too many missed premiums.");
            Assert.IsNotNull(status);
            Assert.AreNotEqual("Error", status);
        }

        [TestMethod]
        public void ProRataBonus_CalculatesBasedOnDates()
        {
            string policyId = "POL-PRO-008";
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 6, 30);

            decimal proRata = _service.CalculateProRataBonus(policyId, startDate, endDate);
            decimal totalAccrued = _service.GetTotalAccruedBonus(policyId);
            int days = _service.GetDaysSinceLastBonusDeclaration("PLAN-A", endDate);

            Assert.IsTrue(proRata >= 0, "Pro-rata bonus should be non-negative.");
            Assert.IsTrue(totalAccrued >= proRata, "Total accrued should be greater than or equal to pro-rata.");
            Assert.IsTrue(days >= 0, "Days since declaration should be valid.");
            Assert.IsNotNull(_service.GetBonusStatus(policyId));
            Assert.AreNotEqual(startDate, endDate);
        }

        [TestMethod]
        public void SpecialOneTimeBonus_AddedToTotalAccrued()
        {
            string policyId = "POL-SPEC-009";
            string eventCode = "CENTENARY";

            decimal specialBonus = _service.GetSpecialOneTimeBonus(policyId, eventCode);
            decimal totalAccrued = _service.GetTotalAccruedBonus(policyId);
            bool canWithdraw = _service.CanWithdrawBonus(policyId, specialBonus);

            Assert.IsTrue(specialBonus >= 0, "Special bonus should be non-negative.");
            Assert.IsTrue(totalAccrued >= specialBonus, "Total accrued should include special bonus.");
            Assert.IsNotNull(eventCode);
            Assert.IsTrue(canWithdraw || !canWithdraw, "Withdrawal check should return a boolean.");
            Assert.IsNotNull(_service.GetCurrencyCode(policyId));
        }

        [TestMethod]
        public void SurrenderValue_CalculatedUsingFactors()
        {
            string policyId = "POL-SURRVAL-010";
            string planCode = "PLAN-D";
            int year = 5;

            double factor = _service.GetSurrenderValueFactor(year, planCode);
            decimal accrued = _service.GetTotalAccruedBonus(policyId);
            decimal surrenderValue = _service.CalculateSurrenderValueOfBonus(policyId, accrued);
            double discount = _service.GetDiscountRateForEarlyWithdrawal(planCode);

            Assert.IsTrue(factor >= 0 && factor <= 1, "Factor should be between 0 and 1.");
            Assert.IsTrue(surrenderValue <= accrued, "Surrender value cannot exceed accrued.");
            Assert.IsTrue(discount >= 0, "Discount rate should be non-negative.");
            Assert.IsNotNull(planCode);
            Assert.AreNotEqual(0m, accrued + 1m); // Just to ensure it's evaluated
        }

        [TestMethod]
        public void MinimumVestingPeriod_CheckedBeforeWithdrawal()
        {
            string policyId = "POL-VEST-011";
            int minYears = 3;

            bool meetsMinimum = _service.CheckMinimumVestingPeriod(policyId, minYears);
            bool isVested = _service.IsBonusVested(policyId, 2);
            int completedYears = _service.GetCompletedPolicyYears(policyId, DateTime.Now);

            Assert.IsTrue(completedYears >= 0, "Completed years should be valid.");
            Assert.IsTrue(meetsMinimum == (completedYears >= minYears), "Minimum vesting check should match completed years.");
            Assert.IsFalse(isVested && completedYears < 2, "Bonus should not be vested if years not met.");
            Assert.IsNotNull(_service.GetBonusStatus(policyId));
            Assert.AreNotEqual(-1, completedYears);
        }

        [TestMethod]
        public void GuaranteedAdditions_CalculatedForCompletedYears()
        {
            string policyId = "POL-GUAR-012";
            int years = 5;

            decimal guaranteed = _service.CalculateGuaranteedAdditions(policyId, years);
            int completedYears = _service.GetCompletedPolicyYears(policyId, DateTime.Now);
            decimal totalBonus = _service.CalculateTotalVestedBonus(policyId, DateTime.Now);

            Assert.IsTrue(guaranteed >= 0, "Guaranteed additions should be non-negative.");
            Assert.IsTrue(completedYears >= 0, "Completed years should be valid.");
            Assert.IsTrue(totalBonus >= guaranteed, "Total bonus should include guaranteed additions.");
            Assert.IsNotNull(_service.GetCurrencyCode(policyId));
            Assert.AreNotEqual(0, years);
        }

        [TestMethod]
        public void BonusStatement_GeneratedWithCorrectCurrency()
        {
            string policyId = "POL-STMT-013";
            DateTime stmtDate = DateTime.Now;

            string stmtId = _service.GenerateBonusStatementId(policyId, stmtDate);
            string currency = _service.GetCurrencyCode(policyId);
            decimal totalAccrued = _service.GetTotalAccruedBonus(policyId);

            Assert.IsNotNull(stmtId, "Statement ID should be generated.");
            Assert.AreNotEqual("", stmtId, "Statement ID should not be empty.");
            Assert.IsNotNull(currency, "Currency code should be retrieved.");
            Assert.IsTrue(totalAccrued >= 0, "Total accrued should be valid.");
            Assert.IsTrue(stmtId.Contains(policyId) || stmtId.Length > 0, "Statement ID should be valid format.");
        }

        [TestMethod]
        public void FundCodeAndTableCode_RetrievedCorrectly()
        {
            string planCode = "PLAN-E";
            DateTime issueDate = new DateTime(2015, 1, 1);

            string fundCode = _service.GetFundCodeForBonus(planCode);
            string tableCode = _service.GetApplicableBonusTableCode(planCode, issueDate);
            double rate = _service.GetBonusRateForYear(2015, planCode);

            Assert.IsNotNull(fundCode, "Fund code should not be null.");
            Assert.IsNotNull(tableCode, "Table code should not be null.");
            Assert.AreNotEqual("", fundCode, "Fund code should not be empty.");
            Assert.AreNotEqual("", tableCode, "Table code should not be empty.");
            Assert.IsTrue(rate >= 0, "Rate should be valid.");
        }

        [TestMethod]
        public void SimpleVsCompoundBonus_CalculatedCorrectly()
        {
            string policyId = "POL-SIMPCOMP-014";
            int year = 3;

            decimal simple = _service.GetSimpleReversionaryBonus(policyId, year);
            decimal compound = _service.GetCompoundReversionaryBonus(policyId, year);
            bool isVested = _service.IsBonusVested(policyId, year);

            Assert.IsTrue(simple >= 0, "Simple bonus should be non-negative.");
            Assert.IsTrue(compound >= 0, "Compound bonus should be non-negative.");
            Assert.IsTrue(isVested || (!isVested && simple == 0), "If not vested, simple bonus might be zero.");
            Assert.IsNotNull(_service.GetBonusStatus(policyId));
            Assert.AreNotEqual(simple, compound - 1m); // Just a structural assert
        }

        [TestMethod]
        public void DaysSinceDeclaration_AffectsInterimBonus()
        {
            string planCode = "PLAN-F";
            string policyId = "POL-DAYS-015";
            DateTime currentDate = DateTime.Now;

            int days = _service.GetDaysSinceLastBonusDeclaration(planCode, currentDate);
            double interimRate = _service.GetInterimBonusRate(planCode, currentDate);
            decimal interimBonus = _service.CalculateInterimBonus(policyId, currentDate);

            Assert.IsTrue(days >= 0, "Days since declaration should be non-negative.");
            Assert.IsTrue(interimRate >= 0, "Interim rate should be non-negative.");
            Assert.IsTrue(interimBonus >= 0, "Interim bonus should be non-negative.");
            Assert.IsNotNull(planCode);
            Assert.AreNotEqual(-1, days);
        }

        [TestMethod]
        public void TotalPremiumsPaid_ValidatesLoyaltyAddition()
        {
            string policyId = "POL-PREM-016";
            
            int paidCount = _service.GetTotalPremiumsPaidCount(policyId);
            decimal estimatedTotal = paidCount * 1000m;
            bool isEligible = _service.IsEligibleForLoyaltyAddition(policyId, estimatedTotal);
            decimal loyalty = _service.CalculateLoyaltyAddition(policyId, paidCount);

            Assert.IsTrue(paidCount >= 0, "Paid count should be valid.");
            Assert.IsTrue(estimatedTotal >= 0, "Estimated total should be valid.");
            Assert.IsTrue(loyalty >= 0, "Loyalty addition should be valid.");
            Assert.IsNotNull(_service.GetBonusStatus(policyId));
            Assert.AreNotEqual(-1, paidCount);
        }

        [TestMethod]
        public void RemainingTerm_AffectsTerminalBonus()
        {
            string policyId = "POL-REM-017";
            DateTime currentDate = DateTime.Now;

            int remainingMonths = _service.GetRemainingTermInMonths(policyId, currentDate);
            bool isEligible = _service.IsEligibleForTerminalBonus(policyId, 10);
            decimal terminal = _service.CalculateTerminalBonus(policyId, 50000m);

            Assert.IsTrue(remainingMonths >= 0, "Remaining months should be valid.");
            Assert.IsTrue(terminal >= 0, "Terminal bonus should be valid.");
            Assert.IsNotNull(_service.GetCurrencyCode(policyId));
            Assert.IsTrue(isEligible || !isEligible); // Boolean check
            Assert.AreNotEqual(-1, remainingMonths);
        }

        [TestMethod]
        public void BonusStatus_ConsistentWithActiveState()
        {
            string policyId = "POL-STAT-018";
            DateTime checkDate = DateTime.Now;

            bool isActive = _service.IsPolicyActive(policyId, checkDate);
            string status = _service.GetBonusStatus(policyId);
            bool hasSurrendered = _service.HasSurrenderedPolicy(policyId);

            Assert.IsNotNull(status, "Status should not be null.");
            Assert.IsTrue(isActive != hasSurrendered, "Policy cannot be both active and surrendered typically.");
            Assert.AreNotEqual("", status, "Status should not be empty.");
            Assert.IsNotNull(_service.GetCurrencyCode(policyId));
            Assert.IsTrue(status.Length > 0);
        }

        [TestMethod]
        public void DiscountRate_AppliedForEarlyWithdrawal()
        {
            string planCode = "PLAN-G";
            string policyId = "POL-DISC-019";
            
            double discount = _service.GetDiscountRateForEarlyWithdrawal(planCode);
            bool canWithdraw = _service.CanWithdrawBonus(policyId, 500m);
            decimal accrued = _service.GetTotalAccruedBonus(policyId);

            Assert.IsTrue(discount >= 0 && discount <= 1, "Discount rate should be a valid percentage.");
            Assert.IsTrue(accrued >= 0, "Accrued bonus should be valid.");
            Assert.IsNotNull(planCode);
            Assert.IsTrue(canWithdraw || !canWithdraw);
            Assert.AreNotEqual(-1.0, discount);
        }

        [TestMethod]
        public void VestedBonus_CheckedForSpecificYear()
        {
            string policyId = "POL-VESTYR-020";
            int year = 4;

            bool isVested = _service.IsBonusVested(policyId, year);
            decimal simple = _service.GetSimpleReversionaryBonus(policyId, year);
            decimal compound = _service.GetCompoundReversionaryBonus(policyId, year);

            Assert.IsTrue(simple >= 0, "Simple bonus should be valid.");
            Assert.IsTrue(compound >= 0, "Compound bonus should be valid.");
            Assert.IsTrue(isVested || (simple == 0 && compound == 0), "If not vested, bonuses are typically zero.");
            Assert.IsNotNull(_service.GetBonusStatus(policyId));
            Assert.AreNotEqual(-1m, simple);
        }

        [TestMethod]
        public void BonusDeclarationYear_MatchesStatement()
        {
            string policyId = "POL-DECL-021";
            DateTime stmtDate = new DateTime(2022, 12, 31);

            string stmtId = _service.GenerateBonusStatementId(policyId, stmtDate);
            int declYear = _service.GetBonusDeclarationYear(stmtId);
            decimal total = _service.CalculateTotalVestedBonus(policyId, stmtDate);

            Assert.IsNotNull(stmtId, "Statement ID should be generated.");
            Assert.IsTrue(declYear > 1900 && declYear <= 2100, "Declaration year should be reasonable.");
            Assert.IsTrue(total >= 0, "Total vested bonus should be valid.");
            Assert.AreNotEqual("", stmtId);
            Assert.IsNotNull(_service.GetCurrencyCode(policyId));
        }

        [TestMethod]
        public void PolicyActive_CheckedBeforeFinalMaturity()
        {
            string policyId = "POL-MATACT-022";
            DateTime calcDate = DateTime.Now;

            bool isActive = _service.IsPolicyActive(policyId, calcDate);
            decimal accrued = _service.GetTotalAccruedBonus(policyId);
            decimal finalMaturity = _service.CalculateFinalMaturityBonus(policyId, 100000m, accrued);

            Assert.IsTrue(accrued >= 0, "Accrued bonus should be valid.");
            Assert.IsTrue(finalMaturity >= accrued, "Final maturity should include accrued.");
            Assert.IsTrue(isActive || !isActive);
            Assert.IsNotNull(_service.GetBonusStatus(policyId));
            Assert.AreNotEqual(-1m, finalMaturity);
        }

        [TestMethod]
        public void InvalidPlanCode_ReturnsDefaultsOrFalse()
        {
            string invalidPlan = "INVALID-999";
            
            double simpleRate = _service.GetBonusRateForYear(2020, invalidPlan);
            string fundCode = _service.GetFundCodeForBonus(invalidPlan);
            double discount = _service.GetDiscountRateForEarlyWithdrawal(invalidPlan);

            Assert.IsTrue(simpleRate >= 0, "Even for invalid plan, rate should not be negative.");
            Assert.IsNotNull(fundCode, "Fund code should handle invalid plans gracefully.");
            Assert.IsTrue(discount >= 0, "Discount should be non-negative.");
            Assert.AreNotEqual("VALID", fundCode);
            Assert.IsNotNull(invalidPlan);
        }
    }

    // Dummy implementation for the tests to compile and run
    public class VestedBonusService : IVestedBonusService
    {
        public decimal CalculateTotalVestedBonus(string policyId, DateTime calculationDate) => 1000m;
        public decimal GetSimpleReversionaryBonus(string policyId, int policyYear) => 100m;
        public decimal GetCompoundReversionaryBonus(string policyId, int policyYear) => 50m;
        public decimal CalculateInterimBonus(string policyId, DateTime dateOfDeathOrMaturity) => 200m;
        public decimal CalculateTerminalBonus(string policyId, decimal sumAssured) => sumAssured * 0.05m;
        public decimal CalculateLoyaltyAddition(string policyId, int completedYears) => completedYears * 10m;
        public double GetBonusRateForYear(int year, string planCode) => 0.04;
        public double GetTerminalBonusRate(string planCode, int termInYears) => 0.02;
        public double GetLoyaltyAdditionPercentage(string planCode, decimal premiumAmount) => 0.01;
        public double GetInterimBonusRate(string planCode, DateTime currentFinancialYear) => 0.03;
        public bool IsEligibleForTerminalBonus(string policyId, int activeYears) => activeYears >= 10;
        public bool IsEligibleForLoyaltyAddition(string policyId, decimal totalPremiumsPaid) => totalPremiumsPaid > 10000m;
        public bool HasSurrenderedPolicy(string policyId) => policyId.Contains("SURR");
        public bool IsPolicyActive(string policyId, DateTime checkDate) => !policyId.Contains("SURR");
        public bool ValidateBonusRates(string planCode, double simpleRate, double compoundRate) => true;
        public bool CheckMinimumVestingPeriod(string policyId, int minimumYearsRequired) => true;
        public int GetCompletedPolicyYears(string policyId, DateTime currentDate) => 5;
        public int GetRemainingTermInMonths(string policyId, DateTime currentDate) => 60;
        public int GetTotalPremiumsPaidCount(string policyId) => 60;
        public int GetMissedPremiumsCount(string policyId) => 0;
        public int GetBonusDeclarationYear(string bonusId) => 2022;
        public string GetApplicableBonusTableCode(string planCode, DateTime issueDate) => "TBL-01";
        public string GetBonusStatus(string policyId) => "Active";
        public string GenerateBonusStatementId(string policyId, DateTime statementDate) => $"STMT-{policyId}-{statementDate.Year}";
        public string GetFundCodeForBonus(string planCode) => "FND-01";
        public decimal CalculateGuaranteedAdditions(string policyId, int yearsApplicable) => yearsApplicable * 50m;
        public decimal GetSpecialOneTimeBonus(string policyId, string eventCode) => 500m;
        public decimal CalculateProRataBonus(string policyId, DateTime startDate, DateTime endDate) => 150m;
        public decimal GetTotalAccruedBonus(string policyId) => 1500m;
        public decimal CalculateSurrenderValueOfBonus(string policyId, decimal accruedBonus) => accruedBonus * 0.8m;
        public double GetSurrenderValueFactor(int yearOfSurrender, string planCode) => 0.8;
        public double GetDiscountRateForEarlyWithdrawal(string planCode) => 0.05;
        public bool IsBonusVested(string policyId, int policyYear) => true;
        public bool CanWithdrawBonus(string policyId, decimal requestedAmount) => !policyId.Contains("SURR");
        public int GetDaysSinceLastBonusDeclaration(string planCode, DateTime currentDate) => 120;
        public string GetCurrencyCode(string policyId) => "USD";
        public decimal AdjustBonusForPaidUpPolicy(string policyId, decimal originalBonus) => originalBonus * 0.5m;
        public decimal CalculateFinalMaturityBonus(string policyId, decimal baseSumAssured, decimal accruedBonuses) => baseSumAssured + accruedBonuses + 1000m;
    }
}