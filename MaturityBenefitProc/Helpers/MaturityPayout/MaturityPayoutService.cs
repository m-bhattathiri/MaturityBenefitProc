using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.MaturityPayout
{
    public class MaturityPayoutService : IMaturityPayoutService
    {
        private readonly Dictionary<string, MaturityPayoutResult> _payouts = new Dictionary<string, MaturityPayoutResult>();
        private readonly Dictionary<string, List<MaturityPayoutResult>> _history = new Dictionary<string, List<MaturityPayoutResult>>();
        private readonly Dictionary<string, bool> _eligibility = new Dictionary<string, bool>();
        private int _counter = 0;

        public MaturityPayoutResult ProcessMaturityPayout(string policyNumber, decimal totalAmount)
        {
            if (string.IsNullOrEmpty(policyNumber) || totalAmount <= 0)
            {
                return new MaturityPayoutResult
                {
                    Success = false,
                    Message = "Invalid policy number or amount",
                    ReferenceId = string.Empty,
                    Amount = 0m
                };
            }

            _counter++;
            var refId = "MPO-" + _counter.ToString("D6");
            var tds = CalculatePayoutTax(totalAmount, true);
            var netPayable = totalAmount - tds;

            var result = new MaturityPayoutResult
            {
                Success = true,
                Message = "Maturity payout processed successfully for policy " + policyNumber,
                ReferenceId = refId,
                Amount = totalAmount,
                GrossAmount = totalAmount,
                TdsDeducted = tds,
                NetPayable = netPayable,
                PaymentMode = "NEFT",
                ProcessedDate = DateTime.UtcNow
            };

            _payouts[refId] = result;
            _eligibility[policyNumber] = true;

            if (!_history.ContainsKey(policyNumber))
                _history[policyNumber] = new List<MaturityPayoutResult>();
            _history[policyNumber].Add(result);

            return result;
        }

        public MaturityPayoutResult ValidateMaturityPayout(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                return new MaturityPayoutResult
                {
                    Success = false,
                    Message = "Policy number is required for validation"
                };
            }

            return new MaturityPayoutResult
            {
                Success = true,
                Message = "Policy " + policyNumber + " is valid for maturity payout",
                ReferenceId = "VAL-" + policyNumber
            };
        }

        public decimal CalculateMaturityAmount(decimal sumAssured, decimal accruedBonus, decimal terminalBonus, decimal loyaltyAddition)
        {
            if (sumAssured <= 0) return 0m;
            return sumAssured + accruedBonus + terminalBonus + loyaltyAddition;
        }

        public decimal CalculateNetPayableAmount(decimal grossAmount, decimal tdsAmount, decimal otherDeductions)
        {
            if (grossAmount <= 0) return 0m;
            var net = grossAmount - tdsAmount - otherDeductions;
            return net > 0 ? net : 0m;
        }

        public MaturityPayoutResult GetPayoutDetails(string payoutReferenceId)
        {
            if (string.IsNullOrEmpty(payoutReferenceId))
            {
                return new MaturityPayoutResult
                {
                    Success = false,
                    Message = "Payout reference ID is required"
                };
            }

            if (_payouts.ContainsKey(payoutReferenceId))
                return _payouts[payoutReferenceId];

            return new MaturityPayoutResult
            {
                Success = false,
                Message = "Payout not found for reference " + payoutReferenceId
            };
        }

        public decimal GetTotalDeductions(string policyNumber, decimal grossAmount)
        {
            if (string.IsNullOrEmpty(policyNumber) || grossAmount <= 0) return 0m;
            var tds = CalculatePayoutTax(grossAmount, true);
            var serviceFee = grossAmount * 0.001m;
            return Math.Round(tds + serviceFee, 2);
        }

        public bool IsPayoutEligible(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber)) return false;
            if (_eligibility.ContainsKey(policyNumber)) return _eligibility[policyNumber];
            return policyNumber.Length >= 5;
        }

        public MaturityPayoutResult ApproveMaturityPayout(string payoutReferenceId, string approvedBy)
        {
            if (string.IsNullOrEmpty(payoutReferenceId) || string.IsNullOrEmpty(approvedBy))
            {
                return new MaturityPayoutResult
                {
                    Success = false,
                    Message = "Reference ID and approver are required"
                };
            }

            if (_payouts.ContainsKey(payoutReferenceId))
            {
                _payouts[payoutReferenceId].ApprovedBy = approvedBy;
                _payouts[payoutReferenceId].Message = "Payout approved by " + approvedBy;
                return _payouts[payoutReferenceId];
            }

            return new MaturityPayoutResult
            {
                Success = true,
                Message = "Payout " + payoutReferenceId + " approved by " + approvedBy,
                ReferenceId = payoutReferenceId,
                ApprovedBy = approvedBy
            };
        }

        public MaturityPayoutResult RejectMaturityPayout(string payoutReferenceId, string reason)
        {
            if (string.IsNullOrEmpty(payoutReferenceId) || string.IsNullOrEmpty(reason))
            {
                return new MaturityPayoutResult
                {
                    Success = false,
                    Message = "Reference ID and rejection reason are required"
                };
            }

            return new MaturityPayoutResult
            {
                Success = true,
                Message = "Payout " + payoutReferenceId + " rejected: " + reason,
                ReferenceId = payoutReferenceId,
                PaymentMode = "REJECTED"
            };
        }

        public decimal GetMaximumPayoutAmount()
        {
            return 50000000m;
        }

        public decimal GetMinimumPayoutAmount()
        {
            return 1000m;
        }

        public bool ValidatePayoutAmount(decimal amount)
        {
            return amount >= GetMinimumPayoutAmount() && amount <= GetMaximumPayoutAmount();
        }

        public List<MaturityPayoutResult> GetPayoutHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrEmpty(policyNumber))
                return new List<MaturityPayoutResult>();

            if (_history.ContainsKey(policyNumber))
            {
                return _history[policyNumber]
                    .Where(h => h.ProcessedDate >= fromDate && h.ProcessedDate <= toDate)
                    .ToList();
            }

            return new List<MaturityPayoutResult>();
        }

        public decimal CalculatePayoutTax(decimal amount, bool hasPanCard)
        {
            if (amount <= 0) return 0m;
            if (!hasPanCard)
                return Math.Round(amount * 0.20m, 2);
            if (amount <= 100000m)
                return 0m;
            return Math.Round(amount * 0.05m, 2);
        }

        public MaturityPayoutResult SuspendPayout(string payoutReferenceId, string reason)
        {
            if (string.IsNullOrEmpty(payoutReferenceId) || string.IsNullOrEmpty(reason))
            {
                return new MaturityPayoutResult
                {
                    Success = false,
                    Message = "Reference ID and suspension reason are required"
                };
            }

            return new MaturityPayoutResult
            {
                Success = true,
                Message = "Payout " + payoutReferenceId + " suspended: " + reason,
                ReferenceId = payoutReferenceId,
                PaymentMode = "SUSPENDED"
            };
        }
    }
}
