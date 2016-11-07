using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.ReinsuranceAndPoolManagement
{
    /// <summary>Allocates maturity liabilities across multiple co-insurers or pools.</summary>
    public interface IPoolAllocationService
    {
        decimal CalculateTotalPoolLiability(string poolId, DateTime maturityDate);
        
        decimal AllocateCedentShare(string policyId, decimal totalMaturityValue);
        
        decimal CalculateReinsurerQuotaShare(string reinsurerId, decimal grossLiability, double quotaSharePercentage);
        
        decimal GetTotalAllocatedAmount(string policyId);
        
        decimal ComputeSurplusTreatyAllocation(string treatyId, decimal maturityAmount, decimal retentionLimit);
        
        decimal CalculatePoolAdministrativeFee(string poolId, decimal totalAllocatedValue);

        double GetReinsurerParticipationRate(string reinsurerId, string poolId);
        
        double CalculateEffectiveTaxRate(string poolId, string jurisdictionCode);
        
        double GetPoolUtilizationRatio(string poolId, DateTime periodStart, DateTime periodEnd);
        
        double CalculateRiskAdjustmentFactor(string policyId, int riskScore);

        bool IsPolicyEligibleForPool(string policyId, string poolId);
        
        bool ValidateAllocationTotals(string policyId, decimal totalMaturityAmount);
        
        bool CheckReinsurerCapacity(string reinsurerId, decimal requestedAllocation);
        
        bool IsTreatyActive(string treatyId, DateTime maturityDate);
        
        bool VerifyPoolSolvency(string poolId);
        
        bool RequiresManualUnderwriterReview(string policyId, decimal allocatedAmount);

        int GetActiveCoInsurersCount(string poolId);
        
        int CalculateDaysToMaturity(string policyId, DateTime currentDate);
        
        int GetAllocationRevisionCount(string policyId);
        
        int GetTreatyDurationInMonths(string treatyId);
        
        int CountEligiblePoliciesInPool(string poolId, DateTime maturityMonth);

        string DeterminePrimaryPool(string policyId, decimal maturityValue);
        
        string GenerateAllocationReferenceNumber(string policyId, string poolId);
        
        string GetReinsurerStatus(string reinsurerId);
        
        string RetrieveTreatyCode(string poolId, DateTime effectiveDate);
        
        string GetAllocationCurrency(string poolId);
    }
}