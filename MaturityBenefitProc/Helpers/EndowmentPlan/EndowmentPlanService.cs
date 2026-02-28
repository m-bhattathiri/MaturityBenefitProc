using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.EndowmentPlan
{
    public class EndowmentPlanService : IEndowmentPlanService
    {
        private readonly Dictionary<string, string> _policies;
        private readonly Dictionary<string, decimal> _sumAssured;
        private readonly Dictionary<string, decimal> _bonusAccrued;
        private readonly Dictionary<string, int> _premiumsPaidCount;
        private readonly Dictionary<string, int> _totalPremiumsDue;
        private readonly Dictionary<string, string> _planCodes;
        private readonly Dictionary<string, int> _policyTerms;
        private readonly Dictionary<string, DateTime> _lastPremiumDates;
        private readonly Dictionary<string, List<EndowmentPlanResult>> _history;

        public EndowmentPlanService()
        {
            _policies = new Dictionary<string, string>
            {
                { "END001", "Active" }, { "END002", "Active" }, { "END003", "Lapsed" },
                { "END004", "Active" }, { "END005", "Active" }, { "END006", "Surrendered" },
                { "END007", "Active" }, { "END008", "Active" }, { "END009", "Active" },
                { "END010", "Active" }
            };

            _sumAssured = new Dictionary<string, decimal>
            {
                { "END001", 500000m }, { "END002", 1000000m }, { "END003", 250000m },
                { "END004", 750000m }, { "END005", 2000000m }, { "END007", 300000m },
                { "END008", 1500000m }, { "END009", 800000m }, { "END010", 600000m }
            };

            _bonusAccrued = new Dictionary<string, decimal>
            {
                { "END001", 75000m }, { "END002", 200000m }, { "END004", 112500m },
                { "END005", 400000m }, { "END007", 45000m }, { "END008", 300000m },
                { "END009", 120000m }, { "END010", 90000m }
            };

            _premiumsPaidCount = new Dictionary<string, int>
            {
                { "END001", 20 }, { "END002", 15 }, { "END003", 5 },
                { "END004", 25 }, { "END005", 10 }, { "END007", 8 },
                { "END008", 30 }, { "END009", 18 }, { "END010", 12 }
            };

            _totalPremiumsDue = new Dictionary<string, int>
            {
                { "END001", 20 }, { "END002", 25 }, { "END003", 20 },
                { "END004", 25 }, { "END005", 30 }, { "END007", 20 },
                { "END008", 30 }, { "END009", 20 }, { "END010", 25 }
            };

            _planCodes = new Dictionary<string, string>
            {
                { "END001", "END20" }, { "END002", "END25" }, { "END003", "END20" },
                { "END004", "END25" }, { "END005", "END30" }, { "END007", "END20" },
                { "END008", "END30" }, { "END009", "END20" }, { "END010", "END25" }
            };

            _policyTerms = new Dictionary<string, int>
            {
                { "END001", 20 }, { "END002", 25 }, { "END003", 20 },
                { "END004", 25 }, { "END005", 30 }, { "END007", 20 },
                { "END008", 30 }, { "END009", 20 }, { "END010", 25 }
            };

            _lastPremiumDates = new Dictionary<string, DateTime>
            {
                { "END001", DateTime.UtcNow.AddDays(-10) }, { "END002", DateTime.UtcNow.AddDays(-5) },
                { "END003", DateTime.UtcNow.AddDays(-90) }, { "END004", DateTime.UtcNow.AddDays(-15) },
                { "END005", DateTime.UtcNow.AddDays(-20) }, { "END007", DateTime.UtcNow.AddDays(-25) },
                { "END008", DateTime.UtcNow.AddDays(-3) }, { "END009", DateTime.UtcNow.AddDays(-35) },
                { "END010", DateTime.UtcNow.AddDays(-40) }
            };

            _history = new Dictionary<string, List<EndowmentPlanResult>>();
        }

        public EndowmentPlanResult ProcessEndowmentMaturity(string policyNumber, decimal sumAssured)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new EndowmentPlanResult { Success = false, Message = "Policy number is required" };
            if (sumAssured <= 0)
                return new EndowmentPlanResult { Success = false, Message = "Sum assured must be positive" };

            if (!_policies.ContainsKey(policyNumber))
                return new EndowmentPlanResult { Success = false, Message = "Policy not found" };

            if (_policies[policyNumber] != "Active")
                return new EndowmentPlanResult { Success = false, Message = $"Policy is {_policies[policyNumber]}" };

            int premiumsPaid = _premiumsPaidCount.ContainsKey(policyNumber) ? _premiumsPaidCount[policyNumber] : 0;
            int totalDue = _totalPremiumsDue.ContainsKey(policyNumber) ? _totalPremiumsDue[policyNumber] : 0;
            decimal bonus = _bonusAccrued.ContainsKey(policyNumber) ? _bonusAccrued[policyNumber] : 0m;
            string planCode = _planCodes.ContainsKey(policyNumber) ? _planCodes[policyNumber] : "END20";
            int term = _policyTerms.ContainsKey(policyNumber) ? _policyTerms[policyNumber] : 20;

            decimal factor = GetEndowmentMaturityFactor(planCode, term);
            decimal maturityBenefit = (sumAssured + bonus) * factor;

            var result = new EndowmentPlanResult
            {
                Success = true,
                Message = "Endowment maturity processed",
                ReferenceId = policyNumber,
                Amount = maturityBenefit,
                PlanCode = planCode,
                PolicyTerm = term,
                PremiumsPaid = premiumsPaid,
                PaidUpValue = CalculatePaidUpValue(sumAssured, premiumsPaid, totalDue),
                MaturityBenefit = maturityBenefit,
                ProcessedDate = DateTime.UtcNow
            };

            StoreHistory(policyNumber, result);
            return result;
        }

        public EndowmentPlanResult ValidateEndowmentMaturity(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new EndowmentPlanResult { Success = false, Message = "Policy number is required" };

            if (!_policies.ContainsKey(policyNumber))
                return new EndowmentPlanResult { Success = false, Message = "Policy not found" };

            int premiumsPaid = _premiumsPaidCount.ContainsKey(policyNumber) ? _premiumsPaidCount[policyNumber] : 0;
            int totalDue = _totalPremiumsDue.ContainsKey(policyNumber) ? _totalPremiumsDue[policyNumber] : 0;

            return new EndowmentPlanResult
            {
                Success = true,
                Message = premiumsPaid >= totalDue ? "Eligible for full maturity" : "Partial maturity applicable",
                ReferenceId = policyNumber,
                PremiumsPaid = premiumsPaid,
                PlanCode = _planCodes.ContainsKey(policyNumber) ? _planCodes[policyNumber] : ""
            };
        }

        public decimal CalculatePaidUpValue(decimal sumAssured, int premiumsPaid, int totalPremiumsDue)
        {
            if (totalPremiumsDue <= 0)
                return 0m;
            if (premiumsPaid <= 0)
                return 0m;
            return sumAssured * premiumsPaid / totalPremiumsDue;
        }

        public decimal CalculateSurrenderValue(decimal paidUpValue, decimal accruedBonus, int completedYears)
        {
            decimal factor;
            if (completedYears >= 7)
                factor = 0.90m;
            else if (completedYears >= 5)
                factor = 0.70m;
            else if (completedYears >= 3)
                factor = 0.50m;
            else
                factor = 0.30m;

            return (paidUpValue + accruedBonus) * factor;
        }

        public decimal GetGuaranteedSurrenderValue(string policyNumber, int completedYears)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return 0m;

            decimal paidUpValue = 0m;
            if (_sumAssured.ContainsKey(policyNumber) && _premiumsPaidCount.ContainsKey(policyNumber) && _totalPremiumsDue.ContainsKey(policyNumber))
            {
                paidUpValue = CalculatePaidUpValue(_sumAssured[policyNumber], _premiumsPaidCount[policyNumber], _totalPremiumsDue[policyNumber]);
            }

            return completedYears >= 3 ? paidUpValue * 0.30m : 0m;
        }

        public decimal GetSpecialSurrenderValue(string policyNumber, int completedYears)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return 0m;

            decimal paidUpValue = 0m;
            if (_sumAssured.ContainsKey(policyNumber) && _premiumsPaidCount.ContainsKey(policyNumber) && _totalPremiumsDue.ContainsKey(policyNumber))
            {
                paidUpValue = CalculatePaidUpValue(_sumAssured[policyNumber], _premiumsPaidCount[policyNumber], _totalPremiumsDue[policyNumber]);
            }

            return completedYears >= 7 ? paidUpValue * 0.90m : 0m;
        }

        public bool IsEligibleForFullMaturity(string policyNumber, int premiumsPaid, int totalDue)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return false;
            return premiumsPaid >= totalDue;
        }

        public EndowmentPlanResult CalculateEndowmentBenefit(string policyNumber, decimal sumAssured, decimal bonus)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new EndowmentPlanResult { Success = false, Message = "Policy number is required" };
            if (sumAssured <= 0)
                return new EndowmentPlanResult { Success = false, Message = "Sum assured must be positive" };

            string planCode = _planCodes.ContainsKey(policyNumber) ? _planCodes[policyNumber] : "END20";
            int term = _policyTerms.ContainsKey(policyNumber) ? _policyTerms[policyNumber] : 20;
            decimal factor = GetEndowmentMaturityFactor(planCode, term);
            decimal benefit = (sumAssured + bonus) * factor;

            return new EndowmentPlanResult
            {
                Success = true,
                Message = "Benefit calculated",
                ReferenceId = policyNumber,
                Amount = benefit,
                MaturityBenefit = benefit,
                PlanCode = planCode,
                PolicyTerm = term
            };
        }

        public decimal GetEndowmentMaturityFactor(string planCode, int policyTerm)
        {
            if (string.IsNullOrWhiteSpace(planCode))
                return 1.0m;

            switch (planCode)
            {
                case "END20":
                    return 1.0m;
                case "END25":
                    return 1.05m;
                case "END30":
                    return 1.10m;
                default:
                    return 1.0m;
            }
        }

        public EndowmentPlanResult GetEndowmentPlanDetails(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new EndowmentPlanResult { Success = false, Message = "Policy number is required" };

            if (!_policies.ContainsKey(policyNumber))
                return new EndowmentPlanResult { Success = false, Message = "Policy not found" };

            return new EndowmentPlanResult
            {
                Success = true,
                Message = "Plan details retrieved",
                ReferenceId = policyNumber,
                PlanCode = _planCodes.ContainsKey(policyNumber) ? _planCodes[policyNumber] : "",
                PolicyTerm = _policyTerms.ContainsKey(policyNumber) ? _policyTerms[policyNumber] : 0,
                PremiumsPaid = _premiumsPaidCount.ContainsKey(policyNumber) ? _premiumsPaidCount[policyNumber] : 0,
                Amount = _sumAssured.ContainsKey(policyNumber) ? _sumAssured[policyNumber] : 0m
            };
        }

        public decimal GetMinimumPremiumsPaidForSurrender(string planCode)
        {
            if (string.IsNullOrWhiteSpace(planCode))
                return 5m;

            switch (planCode)
            {
                case "END20":
                    return 6m;
                case "END25":
                    return 7m;
                default:
                    return 5m;
            }
        }

        public bool IsWithinGracePeriod(string policyNumber, DateTime checkDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return false;
            if (!_lastPremiumDates.ContainsKey(policyNumber))
                return false;

            var lastPremiumDate = _lastPremiumDates[policyNumber];
            return (checkDate - lastPremiumDate).Days <= 30;
        }

        public List<EndowmentPlanResult> GetEndowmentPlanHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new List<EndowmentPlanResult>();

            if (!_history.ContainsKey(policyNumber))
                return new List<EndowmentPlanResult>();

            return _history[policyNumber]
                .Where(h => h.ProcessedDate >= fromDate && h.ProcessedDate <= toDate)
                .ToList();
        }

        public decimal CalculateReducedPaidUpAmount(decimal sumAssured, int premiumsPaid, int totalDue, decimal bonus)
        {
            if (totalDue <= 0 || premiumsPaid <= 0)
                return 0m;

            decimal paidUpValue = sumAssured * premiumsPaid / totalDue;
            return paidUpValue + (bonus * premiumsPaid / totalDue);
        }

        public EndowmentPlanResult ReinstateEndowmentPolicy(string policyNumber, decimal arrearPremium)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new EndowmentPlanResult { Success = false, Message = "Policy number is required" };
            if (arrearPremium <= 0)
                return new EndowmentPlanResult { Success = false, Message = "Arrear premium must be positive" };

            if (!_policies.ContainsKey(policyNumber))
                return new EndowmentPlanResult { Success = false, Message = "Policy not found" };

            return new EndowmentPlanResult
            {
                Success = true,
                Message = "Policy reinstated",
                ReferenceId = policyNumber,
                Amount = arrearPremium
            };
        }

        private void StoreHistory(string policyNumber, EndowmentPlanResult result)
        {
            if (!_history.ContainsKey(policyNumber))
                _history[policyNumber] = new List<EndowmentPlanResult>();
            _history[policyNumber].Add(result);
        }
    }
}
