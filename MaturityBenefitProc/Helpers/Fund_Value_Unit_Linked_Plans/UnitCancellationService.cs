// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans
{
    public class UnitCancellationService : IUnitCancellationService
    {
        private readonly Dictionary<string, decimal> _mockNavs = new Dictionary<string, decimal>
        {
            { "EQ01", 15.45m },
            { "DB01", 10.20m },
            { "MM01", 12.05m }
        };

        private readonly Dictionary<string, decimal> _mockUnitsHeld = new Dictionary<string, decimal>
        {
            { "POL123_EQ01", 1500.50m },
            { "POL123_DB01", 2000.00m }
        };

        public decimal CalculateTotalCancellationValue(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId))
                throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));

            if (CheckPendingTransactions(policyId))
                throw new InvalidOperationException("Cannot calculate value with pending transactions.");

            decimal totalValue = 0m;
            var activeFunds = new[] { "EQ01", "DB01" }; // Mocking active funds for policy

            foreach (var fundCode in activeFunds)
            {
                decimal nav = GetCurrentNav(fundCode, maturityDate);
                decimal fundValue = CalculateFundCancellationValue(policyId, fundCode, nav);
                totalValue += fundValue;
            }

            decimal terminalBonus = CalculateTerminalBonus(policyId, totalValue);
            return totalValue + terminalBonus;
        }

        public bool ValidateFundEligibility(string fundCode, string policyId)
        {
            if (string.IsNullOrWhiteSpace(fundCode) || string.IsNullOrWhiteSpace(policyId))
                return false;

            // In a real system, this would check the policy's fund allocation table
            return fundCode.Length == 4 && policyId.StartsWith("POL");
        }

        public int GetActiveFundCount(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;
            return 2; // Mocked value
        }

        public double GetFundAllocationRatio(string policyId, string fundCode)
        {
            if (!ValidateFundEligibility(fundCode, policyId)) return 0.0;
            
            return fundCode == "EQ01" ? 0.60 : 0.40;
        }

        public string GetPrimaryFundCode(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return string.Empty;
            return "EQ01"; // Mocked primary fund
        }

        public decimal GetCurrentNav(string fundCode, DateTime valuationDate)
        {
            if (string.IsNullOrWhiteSpace(fundCode))
                throw new ArgumentException("Fund code is required.", nameof(fundCode));

            if (valuationDate > DateTime.UtcNow)
                throw new ArgumentException("Valuation date cannot be in the future.");

            return _mockNavs.TryGetValue(fundCode, out decimal nav) ? nav : 10.00m;
        }

        public decimal CalculateFundCancellationValue(string policyId, string fundCode, decimal nav)
        {
            if (nav < 0) throw new ArgumentException("NAV cannot be negative.", nameof(nav));

            decimal units = GetTotalUnitsHeld(policyId, fundCode);
            return Math.Round(units * nav, 2);
        }

        public bool CheckPendingTransactions(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Simulate that policy POL999 has pending transactions
            return policyId == "POL999";
        }

        public int GetDaysSinceLastValuation(string fundCode, DateTime currentDate)
        {
            // Mocking that the last valuation was 1 day ago
            DateTime lastValuation = currentDate.AddDays(-1);
            return (currentDate - lastValuation).Days;
        }

        public string InitiateUnitCancellation(string policyId, DateTime requestDate)
        {
            if (string.IsNullOrWhiteSpace(policyId))
                throw new ArgumentException("Policy ID required.");

            if (IsFundSuspended(GetPrimaryFundCode(policyId), requestDate))
                throw new InvalidOperationException("Primary fund is suspended. Cannot initiate cancellation.");

            return $"TXN_{policyId}_{requestDate:yyyyMMddHHmmss}";
        }

        public double CalculateCancellationPenaltyRate(string policyId, int policyTermYears)
        {
            if (policyTermYears >= 10) return 0.0; // No penalty for long terms
            if (policyTermYears >= 5) return 0.02; // 2% penalty
            return 0.05; // 5% penalty for early cancellation
        }

        public decimal ApplyCancellationPenalty(decimal grossValue, double penaltyRate)
        {
            if (grossValue < 0) return 0m;
            if (penaltyRate < 0 || penaltyRate > 1) throw new ArgumentOutOfRangeException(nameof(penaltyRate));

            decimal penaltyAmount = grossValue * (decimal)penaltyRate;
            return Math.Round(grossValue - penaltyAmount, 2);
        }

        public bool VerifyUnitBalance(string policyId, string fundCode, decimal expectedUnits)
        {
            decimal actualUnits = GetTotalUnitsHeld(policyId, fundCode);
            // Allowing a small epsilon for floating point/decimal rounding differences
            return Math.Abs(actualUnits - expectedUnits) < 0.01m;
        }

        public int RetrieveCancelledUnitCount(string policyId, string fundCode)
        {
            decimal units = GetTotalUnitsHeld(policyId, fundCode);
            return (int)Math.Floor(units);
        }

        public string GenerateCancellationReceipt(string policyId, decimal totalValue)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return string.Empty;
            return $"RECEIPT | Policy: {policyId} | Value: {totalValue:C} | Date: {DateTime.UtcNow:yyyy-MM-dd}";
        }

        public decimal GetTotalUnitsHeld(string policyId, string fundCode)
        {
            string key = $"{policyId}_{fundCode}";
            return _mockUnitsHeld.TryGetValue(key, out decimal units) ? units : 0m;
        }

        public double GetMarketValueAdjustmentFactor(string fundCode, DateTime adjustmentDate)
        {
            // Mock MVA factor based on fund type
            if (fundCode.StartsWith("EQ")) return 0.98; // 2% reduction
            if (fundCode.StartsWith("DB")) return 1.01; // 1% increase
            return 1.0;
        }

        public decimal ApplyMarketValueAdjustment(decimal baseValue, double mvaFactor)
        {
            if (baseValue < 0) return 0m;
            return Math.Round(baseValue * (decimal)mvaFactor, 2);
        }

        public bool IsFundSuspended(string fundCode, DateTime checkDate)
        {
            if (string.IsNullOrWhiteSpace(fundCode)) return false;
            // Mock suspension logic: Fund SUSP is always suspended
            return fundCode == "SUSP";
        }

        public int GetRemainingLockInPeriodDays(string policyId, DateTime currentDate)
        {
            // Mock lock-in period of 5 years from a fixed start date
            DateTime policyStartDate = new DateTime(2020, 1, 1);
            DateTime lockInEndDate = policyStartDate.AddYears(5);
            
            if (currentDate >= lockInEndDate) return 0;
            return (lockInEndDate - currentDate).Days;
        }

        public string GetCancellationStatus(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return "Unknown";
            return transactionId.Contains("TXN") ? "Completed" : "Pending";
        }

        public decimal CalculateTerminalBonus(string policyId, decimal totalFundValue)
        {
            if (totalFundValue <= 0) return 0m;
            
            // 5% terminal bonus for values over 100,000
            if (totalFundValue > 100000m)
            {
                return Math.Round(totalFundValue * 0.05m, 2);
            }
            return 0m;
        }

        public bool AuthorizeCancellation(string policyId, string authorizedBy)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(authorizedBy))
                return false;

            // Only specific roles/users can authorize
            var authorizedUsers = new[] { "Admin", "Manager", "System" };
            return authorizedUsers.Contains(authorizedBy);
        }

        public decimal ComputeNetMaturityValue(string policyId, decimal grossValue, decimal deductions)
        {
            if (grossValue < 0) throw new ArgumentException("Gross value cannot be negative.");
            if (deductions < 0) throw new ArgumentException("Deductions cannot be negative.");

            decimal netValue = grossValue - deductions;
            return netValue < 0 ? 0m : Math.Round(netValue, 2);
        }
    }
}