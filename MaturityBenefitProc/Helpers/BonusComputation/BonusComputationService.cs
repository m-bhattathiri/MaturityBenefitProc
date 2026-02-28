using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.BonusComputation
{
    public class BonusComputationService : IBonusComputationService
    {
        private readonly Dictionary<string, BonusComputationResult> _records = new Dictionary<string, BonusComputationResult>();
        private readonly Dictionary<string, List<BonusComputationResult>> _history = new Dictionary<string, List<BonusComputationResult>>();
        private readonly Dictionary<string, decimal> _accruedAmounts = new Dictionary<string, decimal>();
        private readonly Dictionary<string, int> _declarationYears = new Dictionary<string, int>();
        private int _counter = 0;

        public BonusComputationResult ComputeSimpleReversionaryBonus(string policyNumber, decimal sumAssured, int policyYears)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || sumAssured <= 0 || policyYears <= 0)
            {
                return new BonusComputationResult
                {
                    Success = false,
                    Message = "Invalid input parameters for simple reversionary bonus computation",
                    ProcessedDate = DateTime.UtcNow,
                    BonusType = "SimpleReversionary",
                    BonusRate = 0m,
                    PolicyYear = policyYears,
                    SumAssured = sumAssured,
                    AccruedAmount = 0m,
                    Metadata = new Dictionary<string, string> { { "Reason", "InvalidInput" } }
                };
            }

            var rate = GetBonusRate("Endowment", policyYears);
            var bonusAmount = sumAssured * rate / 1000m * policyYears;
            var referenceId = string.Format("BNS-SRB-{0:D6}", ++_counter);

            if (!_accruedAmounts.ContainsKey(policyNumber))
                _accruedAmounts[policyNumber] = 0m;
            _accruedAmounts[policyNumber] += bonusAmount;

            _declarationYears[policyNumber] = DateTime.UtcNow.Year;

            var result = new BonusComputationResult
            {
                Success = true,
                Message = "Simple reversionary bonus computed successfully",
                ReferenceId = referenceId,
                Amount = bonusAmount,
                ProcessedDate = DateTime.UtcNow,
                BonusType = "SimpleReversionary",
                BonusRate = rate,
                PolicyYear = policyYears,
                SumAssured = sumAssured,
                AccruedAmount = _accruedAmounts[policyNumber],
                Metadata = new Dictionary<string, string>
                {
                    { "PolicyNumber", policyNumber },
                    { "ComputationMethod", "Simple" },
                    { "RatePerThousand", rate.ToString() },
                    { "TotalRecords", _counter.ToString() }
                }
            };

            _records[referenceId] = result;
            if (!_history.ContainsKey(policyNumber))
                _history[policyNumber] = new List<BonusComputationResult>();
            _history[policyNumber].Add(result);

            return result;
        }

        public BonusComputationResult ComputeCompoundReversionaryBonus(string policyNumber, decimal sumAssured, decimal existingBonus, int policyYears)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || sumAssured <= 0 || policyYears <= 0)
            {
                return new BonusComputationResult
                {
                    Success = false,
                    Message = "Invalid input parameters for compound reversionary bonus",
                    ProcessedDate = DateTime.UtcNow,
                    BonusType = "CompoundReversionary",
                    Metadata = new Dictionary<string, string> { { "Reason", "InvalidInput" } }
                };
            }

            var rate = GetBonusRate("Endowment", policyYears);
            var baseForBonus = sumAssured + existingBonus;
            var bonusAmount = baseForBonus * rate / 1000m * policyYears;
            var referenceId = string.Format("BNS-CRB-{0:D6}", ++_counter);
            var totalAccrued = existingBonus + bonusAmount;

            if (!_accruedAmounts.ContainsKey(policyNumber))
                _accruedAmounts[policyNumber] = 0m;
            _accruedAmounts[policyNumber] += bonusAmount;
            _declarationYears[policyNumber] = DateTime.UtcNow.Year;

            var result = new BonusComputationResult
            {
                Success = true,
                Message = "Compound reversionary bonus computed successfully",
                ReferenceId = referenceId,
                Amount = bonusAmount,
                ProcessedDate = DateTime.UtcNow,
                BonusType = "CompoundReversionary",
                BonusRate = rate,
                PolicyYear = policyYears,
                SumAssured = sumAssured,
                AccruedAmount = totalAccrued,
                Metadata = new Dictionary<string, string>
                {
                    { "PolicyNumber", policyNumber },
                    { "ExistingBonus", existingBonus.ToString() },
                    { "ComputationBase", baseForBonus.ToString() },
                    { "TotalRecords", _counter.ToString() }
                }
            };

            _records[referenceId] = result;
            if (!_history.ContainsKey(policyNumber))
                _history[policyNumber] = new List<BonusComputationResult>();
            _history[policyNumber].Add(result);

            return result;
        }

        public BonusComputationResult ComputeTerminalBonus(string policyNumber, decimal sumAssured, int completedYears, string policyType)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || sumAssured <= 0)
            {
                return new BonusComputationResult
                {
                    Success = false,
                    Message = "Invalid input for terminal bonus computation",
                    ProcessedDate = DateTime.UtcNow,
                    BonusType = "Terminal",
                    Metadata = new Dictionary<string, string> { { "Reason", "InvalidInput" } }
                };
            }

            decimal terminalRate;
            if (completedYears >= 15)
                terminalRate = 0.05m;
            else if (completedYears >= 10)
                terminalRate = 0.03m;
            else if (completedYears >= 5)
                terminalRate = 0.01m;
            else
                terminalRate = 0m;

            var bonusAmount = sumAssured * terminalRate;
            var referenceId = string.Format("BNS-TRM-{0:D6}", ++_counter);
            _declarationYears[policyNumber] = DateTime.UtcNow.Year;

            var result = new BonusComputationResult
            {
                Success = true,
                Message = completedYears >= 5 ? "Terminal bonus computed" : "Terminal bonus not applicable - minimum 5 years required",
                ReferenceId = referenceId,
                Amount = bonusAmount,
                ProcessedDate = DateTime.UtcNow,
                BonusType = "Terminal",
                BonusRate = terminalRate * 100m,
                PolicyYear = completedYears,
                SumAssured = sumAssured,
                AccruedAmount = bonusAmount,
                Metadata = new Dictionary<string, string>
                {
                    { "PolicyNumber", policyNumber },
                    { "PolicyType", policyType ?? "Unknown" },
                    { "CompletedYears", completedYears.ToString() },
                    { "TerminalRate", (terminalRate * 100m).ToString() + "%" }
                }
            };

            _records[referenceId] = result;
            if (!_history.ContainsKey(policyNumber))
                _history[policyNumber] = new List<BonusComputationResult>();
            _history[policyNumber].Add(result);

            return result;
        }

        public decimal GetBonusRate(string policyType, int policyYear)
        {
            if (string.IsNullOrWhiteSpace(policyType))
                return 0m;

            switch (policyType)
            {
                case "Endowment": return 45m;
                case "MoneyBack": return 38m;
                case "WholeLife": return 50m;
                case "TermPlan": return 40m;
                case "ChildPlan": return 42m;
                case "PensionPlan": return 48m;
                default: return 40m;
            }
        }

        public decimal CalculateLoyaltyAddition(decimal sumAssured, int completedYears, string policyType)
        {
            if (sumAssured <= 0 || completedYears < 10)
                return 0m;

            if (completedYears >= 15)
                return sumAssured * 0.05m;
            else
                return sumAssured * 0.03m;
        }

        public decimal GetAccruedBonusAmount(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return 0m;

            return _accruedAmounts.ContainsKey(policyNumber) ? _accruedAmounts[policyNumber] : 0m;
        }

        public bool IsBonusEligible(string policyNumber, int minimumYears)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return false;

            return minimumYears >= 3;
        }

        public decimal CalculateInterimBonus(decimal sumAssured, decimal bonusRate, int daysFromLastValuation)
        {
            if (sumAssured <= 0 || bonusRate <= 0 || daysFromLastValuation <= 0)
                return 0m;

            return Math.Round(sumAssured * bonusRate / 1000m * daysFromLastValuation / 365m, 2);
        }

        public BonusComputationResult GetBonusBreakdown(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new BonusComputationResult
                {
                    Success = false,
                    Message = "Policy number required for bonus breakdown",
                    ProcessedDate = DateTime.UtcNow,
                    Metadata = new Dictionary<string, string>()
                };
            }

            var accrued = _accruedAmounts.ContainsKey(policyNumber) ? _accruedAmounts[policyNumber] : 0m;
            var recordCount = _history.ContainsKey(policyNumber) ? _history[policyNumber].Count : 0;

            return new BonusComputationResult
            {
                Success = true,
                Message = "Bonus breakdown retrieved",
                Amount = accrued,
                ProcessedDate = DateTime.UtcNow,
                AccruedAmount = accrued,
                Metadata = new Dictionary<string, string>
                {
                    { "PolicyNumber", policyNumber },
                    { "TotalRecords", recordCount.ToString() },
                    { "TotalAccrued", accrued.ToString() }
                }
            };
        }

        public decimal GetMaximumBonusRate(string policyType)
        {
            if (string.IsNullOrWhiteSpace(policyType))
                return 0m;

            switch (policyType)
            {
                case "Endowment": return 55m;
                case "MoneyBack": return 48m;
                case "WholeLife": return 60m;
                case "TermPlan": return 50m;
                case "ChildPlan": return 52m;
                case "PensionPlan": return 58m;
                default: return 50m;
            }
        }

        public decimal GetMinimumBonusRate(string policyType)
        {
            if (string.IsNullOrWhiteSpace(policyType))
                return 0m;

            switch (policyType)
            {
                case "Endowment": return 35m;
                case "MoneyBack": return 28m;
                case "WholeLife": return 40m;
                case "TermPlan": return 30m;
                case "ChildPlan": return 32m;
                case "PensionPlan": return 38m;
                default: return 30m;
            }
        }

        public bool ValidateBonusComputation(string policyNumber, decimal computedAmount)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return false;

            return computedAmount > 0 && computedAmount <= 50000000m;
        }

        public List<BonusComputationResult> GetBonusHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new List<BonusComputationResult>();

            if (!_history.ContainsKey(policyNumber))
                return new List<BonusComputationResult>();

            return _history[policyNumber]
                .Where(r => r.ProcessedDate >= fromDate && r.ProcessedDate <= toDate)
                .OrderByDescending(r => r.ProcessedDate)
                .ToList();
        }

        public int GetBonusDeclarationYear(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return 0;

            return _declarationYears.ContainsKey(policyNumber) ? _declarationYears[policyNumber] : 0;
        }

        public decimal CalculateTotalAccruedBonus(decimal sumAssured, decimal bonusRate, int years)
        {
            if (sumAssured <= 0 || bonusRate <= 0 || years <= 0)
                return 0m;

            return sumAssured * bonusRate / 1000m * years;
        }
    }
}
