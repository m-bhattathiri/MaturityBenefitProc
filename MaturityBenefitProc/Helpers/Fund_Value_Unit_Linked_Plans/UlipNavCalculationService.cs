using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.Fund_Value_Unit_Linked_Plans
{
    // Fixed implementation — correct business logic
    public class UlipNavCalculationService : IUlipNavCalculationService
    {
        private readonly Dictionary<string, decimal> _mockNavDb = new Dictionary<string, decimal>
        {
            { "EQ01", 150.25m },
            { "DB01", 110.50m },
            { "BL01", 130.75m }
        };

        private readonly Dictionary<string, decimal> _mockUnitsDb = new Dictionary<string, decimal>
        {
            { "POL123_EQ01", 1000.50m },
            { "POL123_DB01", 500.25m }
        };

        public decimal GetNavForDate(string fundCode, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(fundCode)) throw new ArgumentException("Fund code cannot be empty");
            
            // Simulate weekend fallback (use Friday's NAV)
            if (maturityDate.DayOfWeek == DayOfWeek.Saturday) maturityDate = maturityDate.AddDays(-1);
            if (maturityDate.DayOfWeek == DayOfWeek.Sunday) maturityDate = maturityDate.AddDays(-2);

            return _mockNavDb.TryGetValue(fundCode, out var nav) ? nav : 100.00m;
        }

        public decimal CalculateFundValue(string policyId, DateTime calculationDate)
        {
            var funds = new[] { "EQ01", "DB01" };
            decimal totalValue = 0m;
            foreach (var fund in funds)
            {
                var units = GetTotalAllocatedUnits(policyId, fund);
                var nav = GetNavForDate(fund, calculationDate);
                totalValue += CalculateUnitValue(units, nav);
            }
            return totalValue;
        }

        public bool IsNavAvailable(string fundCode, DateTime targetDate)
        {
            return targetDate.Date <= DateTime.Today && targetDate.DayOfWeek != DayOfWeek.Saturday && targetDate.DayOfWeek != DayOfWeek.Sunday;
        }

        public int GetNavDelayDays(string fundCode)
        {
            return IsEquityFund(fundCode) ? 1 : 0;
        }

        public string GetFallbackFundCode(string primaryFundCode)
        {
            return primaryFundCode == "EQ01" ? "BL01" : "DB01";
        }

        public double GetFundAllocationRatio(string policyId, string fundCode)
        {
            if (fundCode == "EQ01") return 0.60;
            if (fundCode == "DB01") return 0.40;
            return 0.0;
        }

        public decimal GetTotalAllocatedUnits(string policyId, string fundCode)
        {
            var key = $"{policyId}_{fundCode}";
            return _mockUnitsDb.TryGetValue(key, out var units) ? units : 0m;
        }

        public decimal CalculateUnitValue(decimal units, decimal nav)
        {
            if (units < 0 || nav < 0) return 0m;
            return Math.Round(units * nav, 2);
        }

        public bool ValidateFundStatus(string fundCode)
        {
            return _mockNavDb.ContainsKey(fundCode);
        }

        public decimal GetHistoricalNav(string fundCode, DateTime historicalDate)
        {
            var baseNav = GetNavForDate(fundCode, historicalDate);
            var daysAgo = (DateTime.Today - historicalDate).TotalDays;
            return Math.Round(baseNav * (decimal)Math.Pow(0.999, daysAgo), 4);
        }

        public double CalculateNavGrowthRate(string fundCode, DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate) return 0.0;
            var startNav = GetHistoricalNav(fundCode, startDate);
            var endNav = GetHistoricalNav(fundCode, endDate);
            if (startNav == 0) return 0.0;
            return (double)((endNav - startNav) / startNav) * 100.0;
        }

        public int GetTotalActiveFunds(string policyId)
        {
            return _mockUnitsDb.Keys.Count(k => k.StartsWith(policyId));
        }

        public string GetDominantFundCode(string policyId)
        {
            var funds = new[] { "EQ01", "DB01" };
            return funds.OrderByDescending(f => GetTotalAllocatedUnits(policyId, f)).FirstOrDefault() ?? "EQ01";
        }

        public decimal CalculateTotalFundValue(string policyId, DateTime maturityDate)
        {
            return CalculateFundValue(policyId, maturityDate);
        }

        public bool IsPolicyEligibleForNavAdjustment(string policyId)
        {
            return policyId.StartsWith("VIP");
        }

        public decimal ApplyNavAdjustment(string policyId, decimal baseNav, double adjustmentRate)
        {
            if (!IsPolicyEligibleForNavAdjustment(policyId)) return baseNav;
            return Math.Round(baseNav * (1m + (decimal)adjustmentRate), 4);
        }

        public int GetUnitsPrecision(string fundCode)
        {
            return IsEquityFund(fundCode) ? 4 : 2;
        }

        public decimal RoundUnits(decimal rawUnits, int precision)
        {
            return Math.Round(rawUnits, precision, MidpointRounding.AwayFromZero);
        }

        public string GetNavCurrency(string fundCode)
        {
            return "INR";
        }

        public double GetExchangeRate(string fromCurrency, string toCurrency, DateTime date)
        {
            if (fromCurrency == toCurrency) return 1.0;
            if (fromCurrency == "USD" && toCurrency == "INR") return 83.50;
            return 1.0;
        }

        public decimal ConvertFundValueCurrency(decimal amount, double exchangeRate)
        {
            return Math.Round(amount * (decimal)exchangeRate, 2);
        }

        public bool HasPendingUnitAllocations(string policyId)
        {
            return GetPendingAllocationCount(policyId) > 0;
        }

        public int GetPendingAllocationCount(string policyId)
        {
            return policyId.EndsWith("PENDING") ? 2 : 0;
        }

        public decimal GetPendingAllocationAmount(string policyId)
        {
            return HasPendingUnitAllocations(policyId) ? 50000m : 0m;
        }

        public string GetLatestNavBatchId(DateTime date)
        {
            return $"BATCH_{date:yyyyMMdd}_001";
        }

        public bool ValidateNavBatch(string batchId)
        {
            return !string.IsNullOrEmpty(batchId) && batchId.StartsWith("BATCH_");
        }

        public decimal GetAverageNav(string fundCode, DateTime startDate, DateTime endDate)
        {
            var startNav = GetHistoricalNav(fundCode, startDate);
            var endNav = GetHistoricalNav(fundCode, endDate);
            return Math.Round((startNav + endNav) / 2m, 4);
        }

        public double GetFundVolatilityIndex(string fundCode)
        {
            return IsEquityFund(fundCode) ? 15.5 : 4.2;
        }

        public decimal CalculateSurrenderValue(string policyId, DateTime surrenderDate)
        {
            var fundValue = CalculateFundValue(policyId, surrenderDate);
            var surrenderCharge = IsLockInPeriodActive(policyId, surrenderDate) ? 0.05m : 0m;
            return Math.Round(fundValue * (1m - surrenderCharge), 2);
        }

        public bool IsLockInPeriodActive(string policyId, DateTime checkDate)
        {
            return GetRemainingLockInDays(policyId, checkDate) > 0;
        }

        public int GetRemainingLockInDays(string policyId, DateTime checkDate)
        {
            var issueDate = checkDate.AddYears(-3); // Mock issue date
            var lockInEndDate = issueDate.AddYears(5);
            var remaining = (lockInEndDate - checkDate).Days;
            return remaining > 0 ? remaining : 0;
        }

        public decimal CalculateMortalityCharge(string policyId, decimal fundValue)
        {
            return Math.Round(fundValue * 0.001m, 2);
        }

        public double GetFundManagementChargeRate(string fundCode)
        {
            return IsEquityFund(fundCode) ? 0.0135 : 0.0080;
        }

        public decimal CalculateFundManagementCharge(decimal fundValue, double fmcRate)
        {
            return Math.Round(fundValue * (decimal)fmcRate, 2);
        }

        public string GetFundCategory(string fundCode)
        {
            if (fundCode.StartsWith("EQ")) return "Equity";
            if (fundCode.StartsWith("DB")) return "Debt";
            return "Balanced";
        }

        public bool IsEquityFund(string fundCode)
        {
            return GetFundCategory(fundCode) == "Equity";
        }

        public decimal CalculateBonusUnits(string policyId, decimal currentUnits)
        {
            var years = GetBonusEligibilityYears(policyId);
            if (years >= 10) return Math.Round(currentUnits * 0.05m, 4);
            if (years >= 5) return Math.Round(currentUnits * 0.02m, 4);
            return 0m;
        }

        public int GetBonusEligibilityYears(string policyId)
        {
            return 6; // Mock value
        }

        public string GetNavSourceSystem(string fundCode)
        {
            return "Bloomberg";
        }

        public decimal CalculateFinalMaturityPayout(string policyId, DateTime maturityDate)
        {
            var fundValue = CalculateTotalFundValue(policyId, maturityDate);
            var pendingAmount = GetPendingAllocationAmount(policyId);
            var mortalityCharge = CalculateMortalityCharge(policyId, fundValue);
            
            var totalUnits = GetTotalAllocatedUnits(policyId, GetDominantFundCode(policyId));
            var bonusUnits = CalculateBonusUnits(policyId, totalUnits);
            var bonusValue = CalculateUnitValue(bonusUnits, GetNavForDate(GetDominantFundCode(policyId), maturityDate));

            return Math.Round(fundValue + pendingAmount + bonusValue - mortalityCharge, 2);
        }
    }
}