using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.ReinsuranceAndPoolManagement
{
    // Fixed implementation — correct business logic
    public class TreatyCalculationService : ITreatyCalculationService
    {
        private const decimal DefaultQuotaShareRetentionRate = 0.40m; // 40% retention
        private const decimal DefaultQuotaShareCededRate = 0.60m; // 60% ceded
        private const int MaxReinstatements = 3;
        private const decimal MinimumCessionThreshold = 500.00m;
        private const decimal DefaultTreatyCapacity = 50000000.00m;

        public decimal CalculateQuotaShareRetention(string treatyId, decimal maturityAmount)
        {
            if (string.IsNullOrWhiteSpace(treatyId)) throw new ArgumentNullException(nameof(treatyId));
            if (maturityAmount < 0) throw new ArgumentException("Maturity amount cannot be negative.");
            
            // In a real system, the retention rate would be fetched from a database using the treatyId.
            return Math.Round(maturityAmount * DefaultQuotaShareRetentionRate, 2);
        }

        public decimal CalculateQuotaShareCeded(string treatyId, decimal maturityAmount)
        {
            if (string.IsNullOrWhiteSpace(treatyId)) throw new ArgumentNullException(nameof(treatyId));
            if (maturityAmount < 0) throw new ArgumentException("Maturity amount cannot be negative.");

            return Math.Round(maturityAmount * DefaultQuotaShareCededRate, 2);
        }

        public double GetSurplusSharePercentage(string treatyId, decimal sumAssured, decimal retentionLimit)
        {
            if (sumAssured <= 0) return 0.0;
            if (retentionLimit >= sumAssured) return 0.0; // Fully retained

            double surplus = (double)(sumAssured - retentionLimit);
            return surplus / (double)sumAssured;
        }

        public decimal CalculateSurplusCededAmount(string treatyId, decimal maturityAmount, double surplusPercentage)
        {
            if (maturityAmount < 0) return 0m;
            if (surplusPercentage < 0 || surplusPercentage > 1) 
                throw new ArgumentOutOfRangeException(nameof(surplusPercentage), "Percentage must be between 0 and 1.");

            return Math.Round(maturityAmount * (decimal)surplusPercentage, 2);
        }

        public bool IsEligibleForProportionalTreaty(string policyId, string treatyId, DateTime maturityDate)
        {
            if (string.IsNullOrEmpty(policyId) || string.IsNullOrEmpty(treatyId)) return false;
            
            // Example business rule: Treaties established before 2000 are closed to new maturities
            if (treatyId.StartsWith("OLD-") && maturityDate.Year >= 2020)
            {
                return false;
            }
            return true;
        }

        public decimal CalculateExcessOfLossRecovery(string treatyId, decimal totalLossAmount, decimal deductible)
        {
            if (totalLossAmount < 0 || deductible < 0) return 0m;
            
            decimal recovery = totalLossAmount - deductible;
            return recovery > 0 ? recovery : 0m;
        }

        public decimal CalculateStopLossRecovery(string poolId, decimal aggregateLosses, decimal attachmentPoint)
        {
            if (aggregateLosses < 0 || attachmentPoint < 0) return 0m;

            decimal recovery = aggregateLosses - attachmentPoint;
            return recovery > 0 ? recovery : 0m;
        }

        public bool ValidateLayerExhaustion(string layerId, decimal accumulatedLosses)
        {
            // Dummy layer limit lookup
            decimal layerLimit = layerId.Contains("HIGH") ? 10000000m : 2000000m;
            return accumulatedLosses >= layerLimit;
        }

        public int GetRemainingReinstatements(string treatyId, int usedReinstatements)
        {
            if (usedReinstatements < 0) return MaxReinstatements;
            
            int remaining = MaxReinstatements - usedReinstatements;
            return remaining > 0 ? remaining : 0;
        }

        public decimal CalculateReinstatementPremium(string treatyId, decimal recoveredAmount, double proRataRate)
        {
            if (recoveredAmount <= 0) return 0m;
            
            // Base premium rate for reinstatement could be 10%
            decimal basePremiumRate = 0.10m;
            return Math.Round(recoveredAmount * basePremiumRate * (decimal)proRataRate, 2);
        }

        public decimal CalculatePoolCapacity(string poolId, DateTime effectiveDate)
        {
            if (effectiveDate.Year < 2010) return 10000000m;
            return 25000000m; // Increased capacity for newer pools
        }

        public double GetParticipantShareRatio(string poolId, string participantId)
        {
            if (string.IsNullOrEmpty(participantId)) return 0.0;
            
            // Mocking a dictionary lookup for participant shares
            var shares = new Dictionary<string, double>
            {
                { "PART-001", 0.25 },
                { "PART-002", 0.35 },
                { "PART-003", 0.40 }
            };

            return shares.TryGetValue(participantId, out double share) ? share : 0.0;
        }

        public decimal CalculateParticipantLiability(string poolId, string participantId, decimal totalMaturityPayout)
        {
            if (totalMaturityPayout <= 0) return 0m;
            
            double shareRatio = GetParticipantShareRatio(poolId, participantId);
            return Math.Round(totalMaturityPayout * (decimal)shareRatio, 2);
        }

        public string GetLeadReinsurerId(string treatyId)
        {
            if (string.IsNullOrEmpty(treatyId)) return "UNKNOWN";
            
            if (treatyId.StartsWith("EU")) return "RE-MUNICH-01";
            if (treatyId.StartsWith("US")) return "RE-SWISS-02";
            
            return "RE-DEFAULT-00";
        }

        public int GetActivePoolParticipantsCount(string poolId)
        {
            // In a real scenario, query the database for active participants in the pool
            return string.IsNullOrEmpty(poolId) ? 0 : 5;
        }

        public decimal CalculateTerminalBonusCeded(string treatyId, decimal terminalBonus, double cessionRate)
        {
            if (terminalBonus <= 0) return 0m;
            if (cessionRate < 0 || cessionRate > 1) throw new ArgumentOutOfRangeException(nameof(cessionRate));

            return Math.Round(terminalBonus * (decimal)cessionRate, 2);
        }

        public decimal CalculateGuaranteedAdditionRecovery(string treatyId, decimal guaranteedAdditionAmount)
        {
            if (guaranteedAdditionAmount <= 0) return 0m;
            
            // Assume a flat 50% recovery rate for guaranteed additions under standard treaties
            return Math.Round(guaranteedAdditionAmount * 0.50m, 2);
        }

        public bool CheckMaturityDateWithinTreatyPeriod(string treatyId, DateTime maturityDate)
        {
            // Mock treaty period: 2015 to 2025
            DateTime treatyStart = new DateTime(2015, 1, 1);
            DateTime treatyEnd = new DateTime(2025, 12, 31);

            return maturityDate >= treatyStart && maturityDate <= treatyEnd;
        }

        public string ResolveApplicableTreatyCode(string policyId, DateTime issueDate, DateTime maturityDate)
        {
            if (issueDate > maturityDate) throw new ArgumentException("Issue date cannot be after maturity date.");

            int duration = maturityDate.Year - issueDate.Year;
            if (duration > 20) return "TRT-LONG-TERM-01";
            if (duration > 10) return "TRT-MID-TERM-02";
            
            return "TRT-SHORT-TERM-03";
        }

        public int CalculateDaysInForce(DateTime issueDate, DateTime maturityDate)
        {
            if (issueDate > maturityDate) return 0;
            return (maturityDate - issueDate).Days;
        }

        public decimal GetTreatyCapacityLimit(string treatyId)
        {
            if (string.IsNullOrEmpty(treatyId)) return 0m;
            
            return treatyId.Contains("LARGE") ? DefaultTreatyCapacity * 2 : DefaultTreatyCapacity;
        }

        public bool ValidateMinimumCessionAmount(string treatyId, decimal cededAmount)
        {
            return cededAmount >= MinimumCessionThreshold;
        }

        public double CalculateLossRatio(decimal incurredLosses, decimal earnedPremiums)
        {
            if (earnedPremiums <= 0) return 0.0;
            if (incurredLosses < 0) return 0.0;

            return (double)(incurredLosses / earnedPremiums);
        }

        public string GetTreatyCurrencyCode(string treatyId)
        {
            if (string.IsNullOrEmpty(treatyId)) return "USD";
            
            if (treatyId.Contains("-EUR-")) return "EUR";
            if (treatyId.Contains("-GBP-")) return "GBP";
            
            return "USD";
        }

        public decimal ConvertCurrencyForTreaty(string treatyId, decimal amount, double exchangeRate)
        {
            if (amount == 0) return 0m;
            if (exchangeRate <= 0) throw new ArgumentException("Exchange rate must be greater than zero.");

            return Math.Round(amount * (decimal)exchangeRate, 2);
        }
    }
}