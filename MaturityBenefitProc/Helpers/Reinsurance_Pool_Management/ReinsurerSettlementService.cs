using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.Reinsurance_Pool_Management
{
    // Fixed implementation — correct business logic
    public class ReinsurerSettlementService : IReinsurerSettlementService
    {
        private readonly Dictionary<string, decimal> _treatyCapacities = new Dictionary<string, decimal>();
        private readonly HashSet<string> _approvedBatches = new HashSet<string>();
        private readonly Dictionary<string, string> _discrepancies = new Dictionary<string, string>();
        private readonly Dictionary<string, DateTime> _claimDates = new Dictionary<string, DateTime>();

        public ReinsurerSettlementService()
        {
            // Seed some mock data for demonstration
            _treatyCapacities.Add("TRT-001", 5000000m);
            _treatyCapacities.Add("TRT-002", 15000000m);
            _claimDates.Add("CLM-1001", DateTime.Today.AddDays(-45));
            _claimDates.Add("CLM-1002", DateTime.Today.AddDays(-15));
        }

        public bool ValidateSettlementBatch(string batchId, DateTime receivedDate)
        {
            if (string.IsNullOrWhiteSpace(batchId))
                throw new ArgumentException("Batch ID cannot be null or empty.", nameof(batchId));

            // Batch cannot be received in the future
            if (receivedDate > DateTime.UtcNow)
                return false;

            // Must follow standard batch naming convention
            return batchId.StartsWith("BAT-");
        }

        public decimal CalculateTotalExpectedRecovery(string reinsurerId, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId))
                throw new ArgumentException("Reinsurer ID is required.", nameof(reinsurerId));

            if (startDate > endDate)
                throw new ArgumentException("Start date must be before end date.");

            // Mock calculation based on date span
            int days = (endDate - startDate).Days;
            return days * 1500.50m; // Mock daily expected recovery
        }

        public decimal ProcessIncomingPayment(string paymentId, string reinsurerId, decimal amountReceived)
        {
            if (amountReceived <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.");

            // In a real system, this would update a database ledger
            decimal processingFee = amountReceived > 10000m ? 50m : 15m;
            decimal netAmount = amountReceived - processingFee;

            return netAmount;
        }

        public int GetPendingSettlementCount(string reinsurerId)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId)) return 0;
            
            // Mock pending count
            return Math.Abs(reinsurerId.GetHashCode()) % 15;
        }

        public string GenerateSettlementReceipt(string paymentId, string reinsurerId)
        {
            if (string.IsNullOrWhiteSpace(paymentId) || string.IsNullOrWhiteSpace(reinsurerId))
                throw new ArgumentException("Payment ID and Reinsurer ID are required.");

            return $"RCPT-{reinsurerId}-{paymentId}-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        public double CalculateRecoveryVariancePercentage(decimal expectedAmount, decimal actualAmount)
        {
            if (expectedAmount == 0)
                return actualAmount > 0 ? 100.0 : 0.0;

            decimal variance = actualAmount - expectedAmount;
            return (double)(variance / expectedAmount) * 100.0;
        }

        public bool IsReinsurerEligibleForDiscount(string reinsurerId, DateTime paymentDate)
        {
            // Eligible if payment is made before the 15th of the month
            return paymentDate.Day <= 15;
        }

        public decimal ApplyEarlySettlementDiscount(decimal originalAmount, double discountRate)
        {
            if (originalAmount < 0) throw new ArgumentException("Amount cannot be negative.");
            if (discountRate < 0 || discountRate > 1) throw new ArgumentException("Discount rate must be between 0 and 1.");

            decimal discountAmount = originalAmount * (decimal)discountRate;
            return originalAmount - discountAmount;
        }

        public int CalculateDaysOutstanding(string claimId, DateTime settlementDate)
        {
            if (!_claimDates.TryGetValue(claimId, out DateTime claimDate))
            {
                // Fallback if claim not found
                return 0;
            }

            int days = (settlementDate - claimDate).Days;
            return days > 0 ? days : 0;
        }

        public string MatchPaymentToClaim(string paymentReference, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(paymentReference)) return null;

            // Mock matching logic
            return $"CLM-MATCHED-{paymentReference.Substring(0, Math.Min(4, paymentReference.Length))}";
        }

        public bool VerifyWireTransferDetails(string transactionId, string bankRoutingNumber)
        {
            if (string.IsNullOrWhiteSpace(transactionId) || string.IsNullOrWhiteSpace(bankRoutingNumber))
                return false;

            // Basic routing number validation (US format is 9 digits)
            return bankRoutingNumber.Length == 9 && bankRoutingNumber.All(char.IsDigit);
        }

        public decimal CalculateCurrencyExchangeAdjustment(decimal baseAmount, double exchangeRate)
        {
            if (exchangeRate <= 0) throw new ArgumentException("Exchange rate must be positive.");
            return baseAmount * (decimal)exchangeRate;
        }

        public double GetCurrentExchangeRate(string sourceCurrency, string targetCurrency)
        {
            if (sourceCurrency == targetCurrency) return 1.0;

            // Mock exchange rates
            if (sourceCurrency == "USD" && targetCurrency == "EUR") return 0.92;
            if (sourceCurrency == "EUR" && targetCurrency == "USD") return 1.09;
            if (sourceCurrency == "GBP" && targetCurrency == "USD") return 1.25;

            return 1.0; // Default fallback
        }

        public int GetUnmatchedPaymentCount(string batchId)
        {
            if (string.IsNullOrWhiteSpace(batchId)) return 0;
            return Math.Abs(batchId.GetHashCode()) % 5;
        }

        public string FlagDiscrepancy(string claimId, string reinsurerId, string reasonCode)
        {
            string discrepancyId = $"DISC-{claimId}-{Guid.NewGuid().ToString().Substring(0, 8)}";
            _discrepancies[discrepancyId] = "OPEN";
            return discrepancyId;
        }

        public bool ResolveDiscrepancy(string discrepancyId, string resolutionCode)
        {
            if (_discrepancies.ContainsKey(discrepancyId))
            {
                _discrepancies[discrepancyId] = $"RESOLVED-{resolutionCode}";
                return true;
            }
            return false;
        }

        public decimal CalculateLatePaymentPenalty(decimal principalAmount, int daysLate, double penaltyRate)
        {
            if (daysLate <= 0) return 0m;
            if (principalAmount <= 0) return 0m;

            // Simple interest penalty calculation
            decimal dailyPenaltyRate = (decimal)penaltyRate / 365m;
            return principalAmount * dailyPenaltyRate * daysLate;
        }

        public string AllocateFundsToPool(string poolId, decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Allocation amount must be positive.");
            if (string.IsNullOrWhiteSpace(poolId)) throw new ArgumentException("Pool ID is required.");

            return $"ALLOC-{poolId}-{Guid.NewGuid().ToString().Substring(0, 6)}";
        }

        public bool ValidateTreatyLimits(string reinsurerId, string treatyId, decimal settlementAmount)
        {
            decimal remainingCapacity = GetRemainingTreatyCapacity(treatyId);
            return settlementAmount <= remainingCapacity;
        }

        public decimal GetRemainingTreatyCapacity(string treatyId)
        {
            if (_treatyCapacities.TryGetValue(treatyId, out decimal capacity))
            {
                return capacity;
            }
            return 0m;
        }

        public double CalculateReinsurerShareRatio(string claimId, string reinsurerId)
        {
            // Mock logic: return a fixed percentage based on reinsurer
            if (reinsurerId.StartsWith("RE-A")) return 0.50; // 50%
            if (reinsurerId.StartsWith("RE-B")) return 0.25; // 25%
            return 0.10; // 10% default
        }

        public int GetSettledClaimsCount(string batchId)
        {
            if (string.IsNullOrWhiteSpace(batchId)) return 0;
            return Math.Abs(batchId.GetHashCode()) % 100 + 10;
        }

        public string InitiateRefundProcess(string reinsurerId, decimal overpaymentAmount)
        {
            if (overpaymentAmount <= 0) throw new ArgumentException("Overpayment amount must be greater than zero.");
            return $"REF-{reinsurerId}-{DateTime.UtcNow.Ticks}";
        }

        public bool ApproveSettlementBatch(string batchId, string approverId)
        {
            if (string.IsNullOrWhiteSpace(batchId) || string.IsNullOrWhiteSpace(approverId))
                return false;

            if (_approvedBatches.Contains(batchId))
                return false; // Already approved

            _approvedBatches.Add(batchId);
            return true;
        }

        public decimal CalculateTotalPoolContribution(string poolId, DateTime periodStart, DateTime periodEnd)
        {
            if (periodStart > periodEnd) throw new ArgumentException("Invalid period dates.");
            
            int months = ((periodEnd.Year - periodStart.Year) * 12) + periodEnd.Month - periodStart.Month;
            if (months <= 0) months = 1;

            // Mock calculation
            return months * 25000m;
        }
    }
}