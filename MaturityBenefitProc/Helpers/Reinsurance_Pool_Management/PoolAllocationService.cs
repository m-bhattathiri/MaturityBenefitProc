using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.ReinsuranceAndPoolManagement
{
    // Fixed implementation — correct business logic
    public class PoolAllocationService : IPoolAllocationService
    {
        private const decimal DefaultCedentRetentionRate = 0.20m;
        private const decimal AdminFeeRate = 0.015m;
        private const decimal MaxAutoApprovalLimit = 5000000m;
        
        private readonly Dictionary<string, decimal> _mockAllocations = new Dictionary<string, decimal>();
        private readonly Dictionary<string, decimal> _reinsurerCapacities = new Dictionary<string, decimal>();

        public PoolAllocationService()
        {
            // Seed some mock capacities for demonstration
            _reinsurerCapacities["RE-001"] = 15000000m;
            _reinsurerCapacities["RE-002"] = 50000000m;
        }

        public decimal CalculateTotalPoolLiability(string poolId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(poolId)) throw new ArgumentNullException(nameof(poolId));
            
            // In a real scenario, this would query a database for all policies in the pool maturing on the date
            int mockPolicyCount = Math.Abs(poolId.GetHashCode()) % 100 + 10;
            decimal averageLiability = 250000m;
            
            return mockPolicyCount * averageLiability;
        }
        
        public decimal AllocateCedentShare(string policyId, decimal totalMaturityValue)
        {
            if (totalMaturityValue < 0) throw new ArgumentOutOfRangeException(nameof(totalMaturityValue));
            
            // Cedent retains a fixed percentage of the liability
            decimal cedentShare = totalMaturityValue * DefaultCedentRetentionRate;
            _mockAllocations[policyId] = cedentShare;
            
            return cedentShare;
        }
        
        public decimal CalculateReinsurerQuotaShare(string reinsurerId, decimal grossLiability, double quotaSharePercentage)
        {
            if (grossLiability < 0) throw new ArgumentOutOfRangeException(nameof(grossLiability));
            if (quotaSharePercentage < 0 || quotaSharePercentage > 1) throw new ArgumentOutOfRangeException(nameof(quotaSharePercentage));
            
            return grossLiability * (decimal)quotaSharePercentage;
        }
        
        public decimal GetTotalAllocatedAmount(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            
            return _mockAllocations.TryGetValue(policyId, out var amount) ? amount : 0m;
        }
        
        public decimal ComputeSurplusTreatyAllocation(string treatyId, decimal maturityAmount, decimal retentionLimit)
        {
            if (retentionLimit < 0) throw new ArgumentOutOfRangeException(nameof(retentionLimit));
            
            // Surplus treaty only covers amounts exceeding the retention limit
            return Math.Max(0m, maturityAmount - retentionLimit);
        }
        
        public decimal CalculatePoolAdministrativeFee(string poolId, decimal totalAllocatedValue)
        {
            if (totalAllocatedValue < 0) return 0m;
            
            // Base fee plus a volume discount for large allocations
            decimal feeRate = totalAllocatedValue > 10000000m ? AdminFeeRate * 0.8m : AdminFeeRate;
            return totalAllocatedValue * feeRate;
        }

        public double GetReinsurerParticipationRate(string reinsurerId, string poolId)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId) || string.IsNullOrWhiteSpace(poolId)) return 0.0;
            
            // Mock participation rate based on reinsurer ID
            return reinsurerId.EndsWith("1") ? 0.25 : 0.10;
        }
        
        public double CalculateEffectiveTaxRate(string poolId, string jurisdictionCode)
        {
            if (jurisdictionCode?.ToUpper() == "US")
            {
                return 0.21;
            }
            else if (jurisdictionCode?.ToUpper() == "UK")
            {
                return 0.19;
            }
            else if (jurisdictionCode?.ToUpper() == "BM")
            {
                return 0.00;
            }
            else if (jurisdictionCode?.ToUpper() == "CH")
            {
                return 0.15;
            }
            else
            {
                return 0.20;
            }
        }
        
        public double GetPoolUtilizationRatio(string poolId, DateTime periodStart, DateTime periodEnd)
        {
            if (periodEnd <= periodStart) throw new ArgumentException("End date must be after start date.");
            
            // Mock utilization ratio between 40% and 95%
            return 0.40 + ((Math.Abs(poolId.GetHashCode()) % 55) / 100.0);
        }
        
        public double CalculateRiskAdjustmentFactor(string policyId, int riskScore)
        {
            // Base factor is 1.0. Risk score (0-100) increases the factor up to 1.5
            int normalizedScore = Math.Min(Math.Max(riskScore, 0), 100);
            return 1.0 + (normalizedScore * 0.005);
        }

        public bool IsPolicyEligibleForPool(string policyId, string poolId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(poolId)) return false;
            
            // Basic validation rule: policy ID must match pool's expected prefix or criteria
            return policyId.Length >= 5 && poolId.Length >= 3;
        }
        
        public bool ValidateAllocationTotals(string policyId, decimal totalMaturityAmount)
        {
            decimal allocated = GetTotalAllocatedAmount(policyId);
            // Allow a small rounding tolerance
            return Math.Abs(allocated - totalMaturityAmount) < 0.01m;
        }
        
        public bool CheckReinsurerCapacity(string reinsurerId, decimal requestedAllocation)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId)) return false;
            
            if (_reinsurerCapacities.TryGetValue(reinsurerId, out var capacity))
            {
                return capacity >= requestedAllocation;
            }
            
            // Default conservative capacity if unknown
            return requestedAllocation <= 1000000m;
        }
        
        public bool IsTreatyActive(string treatyId, DateTime maturityDate)
        {
            // Mock logic: Treaties starting with "EXP" are expired
            if (treatyId?.StartsWith("EXP") == true) return false;
            
            return maturityDate > new DateTime(2020, 1, 1);
        }
        
        public bool VerifyPoolSolvency(string poolId)
        {
            // In reality, this would check assets vs liabilities
            return !string.IsNullOrWhiteSpace(poolId) && !poolId.Contains("INSOLVENT");
        }
        
        public bool RequiresManualUnderwriterReview(string policyId, decimal allocatedAmount)
        {
            return allocatedAmount > MaxAutoApprovalLimit || policyId.StartsWith("HR-"); // HR = High Risk
        }

        public int GetActiveCoInsurersCount(string poolId)
        {
            if (string.IsNullOrWhiteSpace(poolId)) return 0;
            // Mock count
            return Math.Abs(poolId.GetHashCode()) % 5 + 2; // Returns 2 to 6
        }
        
        public int CalculateDaysToMaturity(string policyId, DateTime currentDate)
        {
            // Mocking a future maturity date based on policy ID
            DateTime mockMaturity = currentDate.AddDays(Math.Abs(policyId.GetHashCode()) % 1000);
            return (mockMaturity - currentDate).Days;
        }
        
        public int GetAllocationRevisionCount(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;
            return policyId.Contains("-REV") ? 2 : 0;
        }
        
        public int GetTreatyDurationInMonths(string treatyId)
        {
            // Standard treaties are usually 12, 36, or 60 months
            if (string.IsNullOrWhiteSpace(treatyId)) return 12;
            return treatyId.Contains("LONG") ? 60 : 12;
        }
        
        public int CountEligiblePoliciesInPool(string poolId, DateTime maturityMonth)
        {
            if (string.IsNullOrWhiteSpace(poolId)) return 0;
            return Math.Abs(poolId.GetHashCode() ^ maturityMonth.Month) % 500;
        }

        public string DeterminePrimaryPool(string policyId, decimal maturityValue)
        {
            if (maturityValue > 10000000m) return "POOL-JUMBO";
            if (policyId.StartsWith("LIFE")) return "POOL-LIFE-STD";
            return "POOL-GENERAL";
        }
        
        public string GenerateAllocationReferenceNumber(string policyId, string poolId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(poolId)) 
                throw new ArgumentException("Policy ID and Pool ID are required.");
                
            string dateStr = DateTime.UtcNow.ToString("yyyyMMdd");
            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            
            return $"ALLOC-{poolId}-{dateStr}-{uniqueId}";
        }
        
        public string GetReinsurerStatus(string reinsurerId)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId)) return "Unknown";
            
            return reinsurerId.StartsWith("SUSP") ? "Suspended" : "Active";
        }
        
        public string RetrieveTreatyCode(string poolId, DateTime effectiveDate)
        {
            if (string.IsNullOrWhiteSpace(poolId)) return "DEFAULT-TRT";
            return $"TRT-{poolId.ToUpper()}-{effectiveDate.Year}";
        }
        
        public string GetAllocationCurrency(string poolId)
        {
            if (string.IsNullOrWhiteSpace(poolId)) return "USD";
            
            if (poolId.Contains("EUR")) return "EUR";
            if (poolId.Contains("GBP")) return "GBP";
            if (poolId.Contains("JPY")) return "JPY";
            
            return "USD";
        }
    }
}