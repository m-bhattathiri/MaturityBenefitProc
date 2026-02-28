using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans
{
    // Fixed implementation — correct business logic
    public class UlipMaturityService : IUlipMaturityService
    {
        private const decimal BASE_NAV = 10.0m;
        private const decimal DISCONTINUANCE_RATE = 0.05m;
        private const decimal MAX_DISCONTINUANCE_CHARGE = 6000m;

        public decimal CalculateTotalFundValue(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrEmpty(policyId)) throw new ArgumentNullException(nameof(policyId));
            return CalculateBaseFundValue(policyId, maturityDate) + CalculateTopUpFundValue(policyId, maturityDate);
        }

        public decimal GetNavOnDate(string fundId, DateTime date)
        {
            if (string.IsNullOrEmpty(fundId)) return BASE_NAV;
            // Simulated NAV logic based on date
            int daysSinceInception = (date - new DateTime(2010, 1, 1)).Days;
            return BASE_NAV + (daysSinceInception * 0.001m);
        }

        public decimal CalculateMortalityChargeRefund(string policyId)
        {
            if (!IsEligibleForMortalityRefund(policyId)) return 0m;
            return GetTotalPremiumPaid(policyId) * 0.02m; // 2% of total premium as refund
        }

        public decimal CalculateLoyaltyAdditions(string policyId, int policyTerm)
        {
            if (!IsEligibleForLoyaltyAdditions(policyId)) return 0m;
            return policyTerm >= 10 ? 5000m : 2000m;
        }

        public decimal CalculateWealthBoosters(string policyId, decimal averageFundValue)
        {
            if (!IsEligibleForWealthBoosters(policyId)) return 0m;
            return averageFundValue * 0.015m;
        }

        public decimal GetTotalAllocatedUnits(string policyId, string fundId)
        {
            if (string.IsNullOrEmpty(policyId) || string.IsNullOrEmpty(fundId)) return 0m;
            return 15000.50m; // Simulated allocated units
        }

        public decimal CalculateSurrenderValue(string policyId, DateTime surrenderDate)
        {
            decimal fundValue = CalculateTotalFundValue(policyId, surrenderDate);
            return ApplyDiscontinuanceCharge(policyId, fundValue);
        }

        public decimal GetFundManagementCharge(string fundId, double fmcRate)
        {
            return 100000m * (decimal)fmcRate; // Simulated FMC on 1L base
        }

        public decimal CalculateGuaranteeAddition(string policyId, decimal basePremium)
        {
            return basePremium * 0.05m;
        }

        public decimal GetTotalPremiumPaid(string policyId)
        {
            return 500000m; // Simulated total premium
        }

        public double GetFundAllocationRatio(string policyId, string fundId)
        {
            return 0.5; // 50% allocation
        }

        public double GetMortalityRate(int ageAtEntry, int policyYear)
        {
            return 0.001 + (ageAtEntry * 0.0001) + (policyYear * 0.00005);
        }

        public double GetNavGrowthRate(string fundId, DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate) return 0.0;
            return 0.08; // 8% annualized growth
        }

        public double GetBonusRate(string policyId, int policyYear)
        {
            return policyYear > 5 ? 0.04 : 0.02;
        }

        public double GetTaxRate(string stateCode, string taxCategory)
        {
            return 0.18; // 18% GST
        }

        public bool IsEligibleForMortalityRefund(string policyId)
        {
            return GetPolicyStatus(policyId) == "InForce";
        }

        public bool IsEligibleForLoyaltyAdditions(string policyId)
        {
            return GetCompletedPolicyYears(policyId, DateTime.Now) >= 5;
        }

        public bool IsEligibleForWealthBoosters(string policyId)
        {
            return GetCompletedPolicyYears(policyId, DateTime.Now) >= 10;
        }

        public bool ValidateFundSwitch(string policyId, string fromFundId, string toFundId, decimal amount)
        {
            return amount > 0 && fromFundId != toFundId;
        }

        public bool HasActivePremiumHoliday(string policyId, DateTime checkDate)
        {
            return false;
        }

        public bool IsPolicyMatured(string policyId, DateTime currentDate)
        {
            return GetDaysToMaturity(policyId, currentDate) <= 0;
        }

        public bool ValidateNavDate(string fundId, DateTime navDate)
        {
            return navDate.DayOfWeek != DayOfWeek.Saturday && navDate.DayOfWeek != DayOfWeek.Sunday;
        }

        public bool IsTopUpAllowed(string policyId, decimal topUpAmount)
        {
            return topUpAmount >= 1000m && GetPolicyStatus(policyId) == "InForce";
        }

        public int GetCompletedPolicyYears(string policyId, DateTime currentDate)
        {
            return 10; // Simulated
        }

        public int GetRemainingPremiumTerms(string policyId)
        {
            return 0; // Simulated
        }

        public int GetTotalFundSwitchesUsed(string policyId, int year)
        {
            return 2; // Simulated
        }

        public int GetDaysToMaturity(string policyId, DateTime currentDate)
        {
            DateTime maturityDate = new DateTime(2025, 1, 1);
            return (maturityDate - currentDate).Days;
        }

        public int GetGracePeriodDays(string policyId)
        {
            return 30;
        }

        public int GetFreeLookPeriodDays(string policyId)
        {
            return 15;
        }

        public string GetPrimaryFundId(string policyId)
        {
            return "FUND_EQ_01";
        }

        public string GetPolicyStatus(string policyId)
        {
            return "InForce";
        }

        public string GenerateMaturityStatementId(string policyId, DateTime maturityDate)
        {
            return $"MAT-{policyId}-{maturityDate:yyyyMMdd}";
        }

        public string GetTaxCategoryCode(string policyId)
        {
            return "TAX_ULIP_01";
        }

        public string GetRiderCode(string policyId, string riderType)
        {
            return $"{riderType}_RIDER";
        }

        public string GetFundName(string fundId)
        {
            return "Bluechip Equity Fund";
        }

        public decimal CalculateFinalMaturityPayout(string policyId, DateTime maturityDate, out string payoutStatus)
        {
            if (string.IsNullOrEmpty(policyId))
            {
                payoutStatus = "Error";
                return 0m;
            }

            decimal fundValue = CalculateTotalFundValue(policyId, maturityDate);
            decimal mortalityRefund = CalculateMortalityChargeRefund(policyId);
            decimal loyaltyAdditions = CalculateLoyaltyAdditions(policyId, GetCompletedPolicyYears(policyId, maturityDate));
            decimal wealthBoosters = CalculateWealthBoosters(policyId, fundValue);

            decimal totalPayout = fundValue + mortalityRefund + loyaltyAdditions + wealthBoosters;
            
            payoutStatus = "Success";
            return totalPayout;
        }

        public decimal CalculateTopUpFundValue(string policyId, DateTime maturityDate)
        {
            return 50000m; // Simulated top-up fund value
        }

        public decimal CalculateBaseFundValue(string policyId, DateTime maturityDate)
        {
            string primaryFund = GetPrimaryFundId(policyId);
            decimal units = GetTotalAllocatedUnits(policyId, primaryFund);
            decimal nav = GetNavOnDate(primaryFund, maturityDate);
            return units * nav;
        }

        public decimal ApplyDiscontinuanceCharge(string policyId, decimal fundValue)
        {
            int completedYears = GetCompletedPolicyYears(policyId, DateTime.Now);
            if (completedYears >= 5) return fundValue; // No charge after 5 years

            decimal charge = Math.Min(fundValue * DISCONTINUANCE_RATE, MAX_DISCONTINUANCE_CHARGE);
            return fundValue - charge;
        }

        public decimal CalculatePartialWithdrawalImpact(string policyId, decimal withdrawalAmount)
        {
            return withdrawalAmount * 1.05m; // Withdrawal amount + 5% penalty/impact
        }
    }
}