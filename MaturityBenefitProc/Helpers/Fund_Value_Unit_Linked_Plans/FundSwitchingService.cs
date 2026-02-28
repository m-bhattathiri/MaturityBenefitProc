// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.Fund_Value_Unit_Linked_Plans
{
    public class FundSwitchingService : IFundSwitchingService
    {
        private const decimal MIN_SWITCH_AMOUNT = 1000m;
        private const decimal MAX_SWITCH_AMOUNT = 5000000m;
        private const int MAX_FREE_SWITCHES_PER_YEAR = 12;

        public bool IsPolicyEligibleForAutoSwitch(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Mock logic: Policies starting with "ULIP" are eligible
            return policyId.StartsWith("ULIP", StringComparison.OrdinalIgnoreCase);
        }

        public int GetDaysToMaturity(string policyId, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Invalid policy ID");
            // Mocking a maturity date 3 years from a base date for demonstration
            DateTime mockMaturityDate = new DateTime(currentDate.Year + 3, 12, 31);
            int days = (mockMaturityDate - currentDate).Days;
            return days > 0 ? days : 0;
        }

        public decimal CalculateCurrentEquityValue(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0m;
            // Mock equity value based on policy length
            return 150000.50m;
        }

        public decimal CalculateCurrentDebtValue(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0m;
            // Mock debt value
            return 50000.25m;
        }

        public double GetTargetDebtAllocationPercentage(int daysToMaturity)
        {
            if (daysToMaturity <= 30) return 100.0;
            if (daysToMaturity <= 180) return 80.0;
            if (daysToMaturity <= 365) return 60.0;
            if (daysToMaturity <= 1095) return 40.0;
            return 20.0;
        }

        public double GetCurrentEquityAllocationPercentage(string policyId)
        {
            decimal equity = CalculateCurrentEquityValue(policyId);
            decimal debt = CalculateCurrentDebtValue(policyId);
            decimal total = equity + debt;
            
            if (total == 0) return 0.0;
            return (double)(equity / total) * 100.0;
        }

        public decimal CalculateRequiredSwitchAmount(string policyId, double targetDebtPercentage)
        {
            if (targetDebtPercentage < 0 || targetDebtPercentage > 100)
                throw new ArgumentOutOfRangeException(nameof(targetDebtPercentage));

            decimal equity = CalculateCurrentEquityValue(policyId);
            decimal debt = CalculateCurrentDebtValue(policyId);
            decimal total = equity + debt;

            if (total == 0) return 0m;

            decimal targetDebtValue = total * (decimal)(targetDebtPercentage / 100.0);
            decimal amountToSwitch = targetDebtValue - debt;

            return amountToSwitch > 0 ? amountToSwitch : 0m;
        }

        public bool ValidateSwitchAmountLimits(decimal switchAmount)
        {
            return switchAmount >= MIN_SWITCH_AMOUNT && switchAmount <= MAX_SWITCH_AMOUNT;
        }

        public string InitiateFundSwitch(string policyId, string sourceFundId, string targetFundId, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(sourceFundId) || string.IsNullOrWhiteSpace(targetFundId))
                throw new ArgumentException("Invalid parameters for fund switch.");

            if (!ValidateSwitchAmountLimits(amount))
                throw new InvalidOperationException("Switch amount is out of allowed limits.");

            if (CheckPendingSwitchRequests(policyId))
                throw new InvalidOperationException("Cannot initiate switch. Pending requests exist.");

            return GenerateSwitchTransactionReference(policyId, DateTime.UtcNow);
        }

        public bool CheckPendingSwitchRequests(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Mock logic: return false assuming no pending requests
            return false;
        }

        public int GetCompletedSwitchCount(string policyId, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Start date cannot be after end date.");
            // Mock logic
            return 2;
        }

        public decimal GetApplicableNav(string fundId, DateTime transactionDate)
        {
            if (string.IsNullOrWhiteSpace(fundId)) throw new ArgumentException("Invalid fund ID");
            
            // Mock NAV calculation
            decimal baseNav = 10.0m;
            decimal randomFluctuation = (decimal)(transactionDate.DayOfYear % 5) * 0.1m;
            return baseNav + randomFluctuation;
        }

        public double CalculateSwitchChargePercentage(string policyId)
        {
            int remainingFree = GetRemainingFreeSwitches(policyId, DateTime.UtcNow.Year);
            return remainingFree > 0 ? 0.0 : 0.5; // 0.5% charge if no free switches left
        }

        public decimal CalculateSwitchChargeAmount(decimal switchAmount, double chargePercentage)
        {
            if (switchAmount < 0 || chargePercentage < 0) return 0m;
            return switchAmount * (decimal)(chargePercentage / 100.0);
        }

        public string GetDefaultLiquidFundId(string planCode)
        {
            return string.IsNullOrWhiteSpace(planCode) ? "LIQ-GEN" : $"LIQ-{planCode.ToUpper()}";
        }

        public string GetDefaultDebtFundId(string planCode)
        {
            return string.IsNullOrWhiteSpace(planCode) ? "DBT-GEN" : $"DBT-{planCode.ToUpper()}";
        }

        public bool VerifyFundActiveStatus(string fundId)
        {
            if (string.IsNullOrWhiteSpace(fundId)) return false;
            // Mock logic: Funds ending in "CLS" are closed
            return !fundId.EndsWith("CLS", StringComparison.OrdinalIgnoreCase);
        }

        public decimal GetMinimumSwitchAmount(string planCode)
        {
            return MIN_SWITCH_AMOUNT;
        }

        public decimal GetMaximumSwitchAmount(string planCode)
        {
            return MAX_SWITCH_AMOUNT;
        }

        public int GetRemainingFreeSwitches(string policyId, int currentYear)
        {
            int completed = GetCompletedSwitchCount(policyId, new DateTime(currentYear, 1, 1), new DateTime(currentYear, 12, 31));
            int remaining = MAX_FREE_SWITCHES_PER_YEAR - completed;
            return remaining > 0 ? remaining : 0;
        }

        public bool ProcessSystematicTransferPlan(string policyId, DateTime executionDate)
        {
            if (!IsPolicyEligibleForAutoSwitch(policyId)) return false;
            if (CheckPendingSwitchRequests(policyId)) return false;
            
            // Mock STP processing success
            return true;
        }

        public string GenerateSwitchTransactionReference(string policyId, DateTime executionDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Invalid policy ID");
            string datePart = executionDate.ToString("yyyyMMddHHmmss");
            string shortGuid = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            return $"SW-{policyId}-{datePart}-{shortGuid}";
        }

        public double CalculateMarketVolatilityIndex(DateTime evaluationDate)
        {
            // Mock VIX calculation based on date
            return 15.0 + (evaluationDate.Day % 10);
        }

        public bool ShouldAccelerateSwitch(double volatilityIndex, int daysToMaturity)
        {
            // If market is highly volatile and maturity is within 2 years, accelerate the switch to debt
            return volatilityIndex > 25.0 && daysToMaturity <= 730;
        }

        public decimal CalculateProjectedMaturityValue(string policyId, double assumedGrowthRate)
        {
            decimal equity = CalculateCurrentEquityValue(policyId);
            decimal debt = CalculateCurrentDebtValue(policyId);
            decimal total = equity + debt;

            int daysToMaturity = GetDaysToMaturity(policyId, DateTime.UtcNow);
            double years = daysToMaturity / 365.25;

            // Compound interest formula: A = P(1 + r)^t
            double growthFactor = Math.Pow(1.0 + (assumedGrowthRate / 100.0), years);
            
            return total * (decimal)growthFactor;
        }
    }
}