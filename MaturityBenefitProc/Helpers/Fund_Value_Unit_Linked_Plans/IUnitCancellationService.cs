using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans
{
    /// <summary>Processes the cancellation of units across multiple funds at maturity.</summary>
    public interface IUnitCancellationService
    {
        decimal CalculateTotalCancellationValue(string policyId, DateTime maturityDate);
        
        bool ValidateFundEligibility(string fundCode, string policyId);
        
        int GetActiveFundCount(string policyId);
        
        double GetFundAllocationRatio(string policyId, string fundCode);
        
        string GetPrimaryFundCode(string policyId);
        
        decimal GetCurrentNav(string fundCode, DateTime valuationDate);
        
        decimal CalculateFundCancellationValue(string policyId, string fundCode, decimal nav);
        
        bool CheckPendingTransactions(string policyId);
        
        int GetDaysSinceLastValuation(string fundCode, DateTime currentDate);
        
        string InitiateUnitCancellation(string policyId, DateTime requestDate);
        
        double CalculateCancellationPenaltyRate(string policyId, int policyTermYears);
        
        decimal ApplyCancellationPenalty(decimal grossValue, double penaltyRate);
        
        bool VerifyUnitBalance(string policyId, string fundCode, decimal expectedUnits);
        
        int RetrieveCancelledUnitCount(string policyId, string fundCode);
        
        string GenerateCancellationReceipt(string policyId, decimal totalValue);
        
        decimal GetTotalUnitsHeld(string policyId, string fundCode);
        
        double GetMarketValueAdjustmentFactor(string fundCode, DateTime adjustmentDate);
        
        decimal ApplyMarketValueAdjustment(decimal baseValue, double mvaFactor);
        
        bool IsFundSuspended(string fundCode, DateTime checkDate);
        
        int GetRemainingLockInPeriodDays(string policyId, DateTime currentDate);
        
        string GetCancellationStatus(string transactionId);
        
        decimal CalculateTerminalBonus(string policyId, decimal totalFundValue);
        
        bool AuthorizeCancellation(string policyId, string authorizedBy);
        
        decimal ComputeNetMaturityValue(string policyId, decimal grossValue, decimal deductions);
    }
}