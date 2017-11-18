using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.MoneyBackPlan
{
    public class MoneyBackPlanService : IMoneyBackPlanService
    {
        private readonly Dictionary<string, List<MoneyBackPlanResult>> _payoutHistory;
        private readonly Dictionary<string, MoneyBackPlanResult> _payoutRecords;
        private readonly Dictionary<string, MoneyBackScheduleEntry> _scheduleEntries;
        private readonly Dictionary<string, PolicyMoneyBackInfo> _policyInfoStore;

        public MoneyBackPlanService()
        {
            _payoutHistory = new Dictionary<string, List<MoneyBackPlanResult>>();
            _payoutRecords = new Dictionary<string, MoneyBackPlanResult>();
            _scheduleEntries = new Dictionary<string, MoneyBackScheduleEntry>();
            _policyInfoStore = new Dictionary<string, PolicyMoneyBackInfo>();
        }

        public MoneyBackPlanResult ProcessMoneyBackPayout(string policyNumber, int installmentNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Policy number is required"
                };
            }

            if (installmentNumber <= 0)
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Installment number must be greater than zero"
                };
            }

            if (installmentNumber > 10)
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Installment number exceeds maximum allowed installments"
                };
            }

            var policyInfo = GetPolicyInfo(policyNumber);
            if (policyInfo == null)
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Policy not found in records"
                };
            }

            var payoutPercentage = GetPayoutPercentage(policyInfo.PlanCode, installmentNumber);
            var payoutAmount = CalculateMoneyBackAmount(policyInfo.SumAssured, payoutPercentage);

            if (!ValidateMoneyBackAmount(payoutAmount))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Calculated payout amount is invalid or exceeds maximum limit"
                };
            }

            var referenceId = GenerateReferenceId(policyNumber, installmentNumber);
            var result = new MoneyBackPlanResult
            {
                Success = true,
                Message = "Money-back payout processed successfully",
                ReferenceId = referenceId,
                Amount = payoutAmount,
                InstallmentNumber = installmentNumber,
                PayoutPercentage = payoutPercentage,
                PlanCode = policyInfo.PlanCode,
                ProcessedDate = DateTime.UtcNow
            };

            _payoutRecords[referenceId] = result;

            if (!_payoutHistory.ContainsKey(policyNumber))
            {
                _payoutHistory[policyNumber] = new List<MoneyBackPlanResult>();
            }
            _payoutHistory[policyNumber].Add(result);

            return result;
        }

        public MoneyBackPlanResult ValidateMoneyBackPlan(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Policy number is required for validation"
                };
            }

            var policyInfo = GetPolicyInfo(policyNumber);
            if (policyInfo == null)
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Policy not found for validation"
                };
            }

            if (policyInfo.SumAssured <= 0)
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Sum assured must be greater than zero"
                };
            }

            if (string.IsNullOrWhiteSpace(policyInfo.PlanCode))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Plan code is missing for the policy"
                };
            }

            return new MoneyBackPlanResult
            {
                Success = true,
                Message = "Money-back plan validation successful",
                PlanCode = policyInfo.PlanCode,
                Amount = policyInfo.SumAssured
            };
        }

        public decimal CalculateMoneyBackAmount(decimal sumAssured, decimal payoutPercentage)
        {
            if (sumAssured <= 0)
            {
                return 0m;
            }

            if (payoutPercentage <= 0 || payoutPercentage > 100)
            {
                return 0m;
            }

            return sumAssured * payoutPercentage / 100m;
        }

        public decimal GetPayoutPercentage(string planCode, int installmentNumber)
        {
            if (string.IsNullOrWhiteSpace(planCode) || installmentNumber <= 0)
            {
                return 0m;
            }

            switch (planCode.ToUpperInvariant())
            {
                case "MB20":
                    if (installmentNumber >= 1 && installmentNumber <= 3)
                        return 20m;
                    if (installmentNumber == 4)
                        return 40m;
                    return 20m;

                case "MB25":
                    if (installmentNumber >= 1 && installmentNumber <= 4)
                        return 15m;
                    if (installmentNumber == 5)
                        return 40m;
                    return 15m;

                case "MB15":
                    if (installmentNumber >= 1 && installmentNumber <= 2)
                        return 25m;
                    if (installmentNumber == 3)
                        return 50m;
                    return 25m;

                default:
                    return 20m;
            }
        }

        public MoneyBackPlanResult GetPayoutSchedule(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Policy number is required"
                };
            }

            var policyInfo = GetPolicyInfo(policyNumber);
            if (policyInfo == null)
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Policy not found for schedule generation"
                };
            }

            int totalInstallments = GetTotalInstallmentsForPlan(policyInfo.PlanCode);
            decimal totalScheduledAmount = 0m;
            var scheduleMetadata = new Dictionary<string, string>();

            for (int i = 1; i <= totalInstallments; i++)
            {
                var pct = GetPayoutPercentage(policyInfo.PlanCode, i);
                var amt = CalculateMoneyBackAmount(policyInfo.SumAssured, pct);
                totalScheduledAmount += amt;

                int payoutYear = policyInfo.CommencementYear + (i * GetIntervalForPlan(policyInfo.PlanCode));
                scheduleMetadata[$"Installment_{i}_Year"] = payoutYear.ToString();
                scheduleMetadata[$"Installment_{i}_Percentage"] = pct.ToString("F2");
                scheduleMetadata[$"Installment_{i}_Amount"] = amt.ToString("F2");
            }

            return new MoneyBackPlanResult
            {
                Success = true,
                Message = "Payout schedule generated successfully",
                Amount = totalScheduledAmount,
                PlanCode = policyInfo.PlanCode,
                Metadata = scheduleMetadata,
                InstallmentNumber = totalInstallments
            };
        }

        public bool IsMoneyBackDue(string policyNumber, DateTime checkDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return false;
            }

            var policyInfo = GetPolicyInfo(policyNumber);
            if (policyInfo == null)
            {
                return false;
            }

            int nextPayoutYear = GetNextPayoutYear(policyNumber);
            if (nextPayoutYear <= 0)
            {
                return false;
            }

            return checkDate.Year >= nextPayoutYear;
        }

        public int GetNextPayoutYear(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return 0;
            }

            var policyInfo = GetPolicyInfo(policyNumber);
            if (policyInfo == null)
            {
                return 0;
            }

            int interval = GetIntervalForPlan(policyInfo.PlanCode);
            int paidInstallments = 0;

            if (_payoutHistory.ContainsKey(policyNumber))
            {
                paidInstallments = _payoutHistory[policyNumber].Count(p => p.Success);
            }

            int nextInstallment = paidInstallments + 1;
            int nextYear = policyInfo.CommencementYear + (nextInstallment * interval);

            return nextYear;
        }

        public decimal GetTotalMoneyBackPaid(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return 0m;
            }

            if (!_payoutHistory.ContainsKey(policyNumber))
            {
                return 0m;
            }

            return _payoutHistory[policyNumber]
                .Where(p => p.Success)
                .Sum(p => p.Amount);
        }

        public decimal GetRemainingMoneyBackPayable(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return 0m;
            }

            var scheduleResult = GetPayoutSchedule(policyNumber);
            if (!scheduleResult.Success)
            {
                return 0m;
            }

            var totalScheduled = scheduleResult.Amount;
            var totalPaid = GetTotalMoneyBackPaid(policyNumber);

            var remaining = totalScheduled - totalPaid;
            return remaining > 0 ? remaining : 0m;
        }

        public MoneyBackPlanResult ApproveMoneyBackPayout(string referenceId, string approvedBy)
        {
            if (string.IsNullOrWhiteSpace(referenceId))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Reference ID is required for approval"
                };
            }

            if (string.IsNullOrWhiteSpace(approvedBy))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Approver identity is required"
                };
            }

            if (!_payoutRecords.ContainsKey(referenceId))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Payout record not found for the given reference ID"
                };
            }

            var record = _payoutRecords[referenceId];
            record.Metadata["ApprovedBy"] = approvedBy;
            record.Metadata["ApprovalDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd");
            record.Metadata["Status"] = "Approved";

            return new MoneyBackPlanResult
            {
                Success = true,
                Message = "Money-back payout approved successfully",
                ReferenceId = referenceId,
                Amount = record.Amount,
                Metadata = new Dictionary<string, string>
                {
                    { "ApprovedBy", approvedBy },
                    { "Status", "Approved" }
                }
            };
        }

        public MoneyBackPlanResult RejectMoneyBackPayout(string referenceId, string reason)
        {
            if (string.IsNullOrWhiteSpace(referenceId))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Reference ID is required for rejection"
                };
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Rejection reason is required"
                };
            }

            if (!_payoutRecords.ContainsKey(referenceId))
            {
                return new MoneyBackPlanResult
                {
                    Success = false,
                    Message = "Payout record not found for the given reference ID"
                };
            }

            var record = _payoutRecords[referenceId];
            record.Metadata["RejectedBy"] = "System";
            record.Metadata["RejectionDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd");
            record.Metadata["RejectionReason"] = reason;
            record.Metadata["Status"] = "Rejected";
            record.Success = false;

            return new MoneyBackPlanResult
            {
                Success = true,
                Message = "Money-back payout rejected",
                ReferenceId = referenceId,
                Metadata = new Dictionary<string, string>
                {
                    { "RejectionReason", reason },
                    { "Status", "Rejected" }
                }
            };
        }

        public decimal GetFinalInstallmentAmount(decimal sumAssured, decimal totalPaidOut, decimal accruedBonus)
        {
            if (sumAssured <= 0)
            {
                return 0m;
            }

            if (totalPaidOut < 0)
            {
                totalPaidOut = 0m;
            }

            if (accruedBonus < 0)
            {
                accruedBonus = 0m;
            }

            var finalAmount = sumAssured - totalPaidOut + accruedBonus;
            return finalAmount > 0 ? finalAmount : 0m;
        }

        public bool ValidateMoneyBackAmount(decimal amount)
        {
            return amount > 0 && amount <= 50000000m;
        }

        public List<MoneyBackPlanResult> GetMoneyBackPayoutHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new List<MoneyBackPlanResult>();
            }

            if (fromDate > toDate)
            {
                return new List<MoneyBackPlanResult>();
            }

            if (!_payoutHistory.ContainsKey(policyNumber))
            {
                return new List<MoneyBackPlanResult>();
            }

            return _payoutHistory[policyNumber]
                .Where(p => p.ProcessedDate >= fromDate && p.ProcessedDate <= toDate)
                .OrderBy(p => p.ProcessedDate)
                .ToList();
        }

        public decimal CalculateMoneyBackTax(decimal amount, bool hasPanCard)
        {
            if (amount <= 0)
            {
                return 0m;
            }

            if (hasPanCard)
            {
                return amount * 0.02m;
            }
            else
            {
                return amount * 0.20m;
            }
        }

        #region Private Helper Methods

        private PolicyMoneyBackInfo GetPolicyInfo(string policyNumber)
        {
            if (_policyInfoStore.ContainsKey(policyNumber))
            {
                return _policyInfoStore[policyNumber];
            }

            return null;
        }

        public void RegisterPolicy(string policyNumber, string planCode, decimal sumAssured, int commencementYear)
        {
            _policyInfoStore[policyNumber] = new PolicyMoneyBackInfo
            {
                PolicyNumber = policyNumber,
                PlanCode = planCode,
                SumAssured = sumAssured,
                CommencementYear = commencementYear
            };
        }

        private string GenerateReferenceId(string policyNumber, int installmentNumber)
        {
            return $"MB-{policyNumber}-{installmentNumber}-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        private int GetTotalInstallmentsForPlan(string planCode)
        {
            switch (planCode?.ToUpperInvariant())
            {
                case "MB20": return 4;
                case "MB25": return 5;
                case "MB15": return 3;
                default: return 4;
            }
        }

        private int GetIntervalForPlan(string planCode)
        {
            switch (planCode?.ToUpperInvariant())
            {
                case "MB20": return 5;
                case "MB25": return 5;
                case "MB15": return 5;
                default: return 5;
            }
        }

        #endregion
    }

    internal class PolicyMoneyBackInfo
    {
        public string PolicyNumber { get; set; }
        public string PlanCode { get; set; }
        public decimal SumAssured { get; set; }
        public int CommencementYear { get; set; }
    }

    internal class MoneyBackScheduleEntry
    {
        public int InstallmentNumber { get; set; }
        public int PayoutYear { get; set; }
        public decimal PayoutPercentage { get; set; }
        public decimal PayoutAmount { get; set; }
    }
}
