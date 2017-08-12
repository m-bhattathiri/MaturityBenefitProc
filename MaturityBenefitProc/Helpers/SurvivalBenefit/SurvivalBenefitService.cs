using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.SurvivalBenefit
{
    public class SurvivalBenefitService : ISurvivalBenefitService
    {
        private readonly Dictionary<string, SurvivalBenefitResult> _benefits = new Dictionary<string, SurvivalBenefitResult>();
        private readonly Dictionary<string, List<SurvivalBenefitResult>> _schedule = new Dictionary<string, List<SurvivalBenefitResult>>();
        private readonly Dictionary<string, int> _installmentCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, decimal> _totalPaid = new Dictionary<string, decimal>();
        private int _counter = 0;

        public SurvivalBenefitResult ProcessSurvivalBenefit(string policyNumber, decimal amount)
        {
            if (string.IsNullOrEmpty(policyNumber) || amount <= 0)
            {
                return new SurvivalBenefitResult
                {
                    Success = false,
                    Message = "Invalid policy number or amount for survival benefit",
                    ReferenceId = string.Empty,
                    Amount = 0m
                };
            }

            _counter++;
            var refId = "SB-" + _counter.ToString("D6");
            var installment = _installmentCounts.ContainsKey(policyNumber) ? _installmentCounts[policyNumber] + 1 : 1;
            _installmentCounts[policyNumber] = installment;

            if (!_totalPaid.ContainsKey(policyNumber))
                _totalPaid[policyNumber] = 0m;
            _totalPaid[policyNumber] += amount;

            var result = new SurvivalBenefitResult
            {
                Success = true,
                Message = "Survival benefit processed for policy " + policyNumber + " installment " + installment,
                ReferenceId = refId,
                Amount = amount,
                InstallmentNumber = installment,
                BenefitPercentage = 20m,
                DueDate = DateTime.UtcNow,
                PlanCode = "SB-PLAN-001",
                PolicyYear = installment * 5,
                ProcessedDate = DateTime.UtcNow
            };

            _benefits[refId] = result;

            if (!_schedule.ContainsKey(policyNumber))
                _schedule[policyNumber] = new List<SurvivalBenefitResult>();
            _schedule[policyNumber].Add(result);

            return result;
        }

        public SurvivalBenefitResult ValidateSurvivalBenefit(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                return new SurvivalBenefitResult
                {
                    Success = false,
                    Message = "Policy number is required for survival benefit validation"
                };
            }

            return new SurvivalBenefitResult
            {
                Success = true,
                Message = "Policy " + policyNumber + " validated for survival benefit",
                ReferenceId = "VAL-" + policyNumber,
                PlanCode = "SB-PLAN-001"
            };
        }

        public decimal CalculateSurvivalBenefitAmount(decimal sumAssured, decimal benefitPercentage, int installmentNumber)
        {
            if (sumAssured <= 0 || benefitPercentage <= 0 || installmentNumber <= 0) return 0m;
            return Math.Round(sumAssured * (benefitPercentage / 100m), 2);
        }

        public decimal GetSurvivalBenefitPercentage(string planCode, int installmentNumber)
        {
            if (string.IsNullOrEmpty(planCode) || installmentNumber <= 0) return 0m;
            if (planCode.StartsWith("JEEVAN") || planCode.StartsWith("SB-PLAN"))
            {
                if (installmentNumber <= 2) return 20m;
                if (installmentNumber <= 4) return 25m;
                return 30m;
            }
            return 15m;
        }

        public bool IsSurvivalBenefitDue(string policyNumber, DateTime checkDate)
        {
            if (string.IsNullOrEmpty(policyNumber)) return false;
            return checkDate <= DateTime.UtcNow.AddMonths(6);
        }

        public SurvivalBenefitResult GetNextSurvivalBenefitDue(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                return new SurvivalBenefitResult
                {
                    Success = false,
                    Message = "Policy number is required"
                };
            }

            var nextInstallment = _installmentCounts.ContainsKey(policyNumber) ? _installmentCounts[policyNumber] + 1 : 1;
            return new SurvivalBenefitResult
            {
                Success = true,
                Message = "Next survival benefit due for installment " + nextInstallment,
                InstallmentNumber = nextInstallment,
                DueDate = DateTime.UtcNow.AddYears(5),
                BenefitPercentage = 20m,
                PlanCode = "SB-PLAN-001",
                ReferenceId = "NEXT-" + policyNumber
            };
        }

        public decimal GetTotalSurvivalBenefitsPaid(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber)) return 0m;
            return _totalPaid.ContainsKey(policyNumber) ? _totalPaid[policyNumber] : 0m;
        }

        public int GetRemainingInstallments(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber)) return 0;
            var paid = _installmentCounts.ContainsKey(policyNumber) ? _installmentCounts[policyNumber] : 0;
            var total = 4;
            return Math.Max(0, total - paid);
        }

        public SurvivalBenefitResult ApproveSurvivalBenefit(string referenceId, string approvedBy)
        {
            if (string.IsNullOrEmpty(referenceId) || string.IsNullOrEmpty(approvedBy))
            {
                return new SurvivalBenefitResult
                {
                    Success = false,
                    Message = "Reference ID and approver name are required"
                };
            }

            return new SurvivalBenefitResult
            {
                Success = true,
                Message = "Survival benefit " + referenceId + " approved by " + approvedBy,
                ReferenceId = referenceId
            };
        }

        public SurvivalBenefitResult RejectSurvivalBenefit(string referenceId, string reason)
        {
            if (string.IsNullOrEmpty(referenceId) || string.IsNullOrEmpty(reason))
            {
                return new SurvivalBenefitResult
                {
                    Success = false,
                    Message = "Reference ID and rejection reason are required"
                };
            }

            return new SurvivalBenefitResult
            {
                Success = true,
                Message = "Survival benefit " + referenceId + " rejected: " + reason,
                ReferenceId = referenceId
            };
        }

        public decimal GetMaximumSurvivalBenefitAmount()
        {
            return 25000000m;
        }

        public decimal GetMinimumSurvivalBenefitAmount()
        {
            return 500m;
        }

        public bool ValidateSurvivalBenefitAmount(decimal amount)
        {
            return amount >= GetMinimumSurvivalBenefitAmount() && amount <= GetMaximumSurvivalBenefitAmount();
        }

        public List<SurvivalBenefitResult> GetSurvivalBenefitSchedule(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrEmpty(policyNumber))
                return new List<SurvivalBenefitResult>();

            if (_schedule.ContainsKey(policyNumber))
            {
                return _schedule[policyNumber]
                    .Where(s => s.ProcessedDate >= fromDate && s.ProcessedDate <= toDate)
                    .ToList();
            }

            return new List<SurvivalBenefitResult>();
        }

        public decimal CalculateSurvivalBenefitTax(decimal amount, bool hasPanCard)
        {
            if (amount <= 0) return 0m;
            if (!hasPanCard) return Math.Round(amount * 0.20m, 2);
            if (amount <= 100000m) return 0m;
            return Math.Round(amount * 0.05m, 2);
        }
    }
}
