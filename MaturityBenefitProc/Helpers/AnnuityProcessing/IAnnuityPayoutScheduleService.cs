using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    /// <summary>Generates and maintains the monthly, quarterly, or annual payout schedules.</summary>
    public interface IAnnuityPayoutScheduleService
    {
        decimal CalculateMonthlyPayout(string policyId, decimal principalAmount, double interestRate);
        
        decimal CalculateQuarterlyPayout(string policyId, decimal principalAmount, double interestRate);
        
        decimal CalculateAnnualPayout(string policyId, decimal principalAmount, double interestRate);
        
        decimal GetTotalProjectedPayout(string scheduleId);
        
        decimal CalculateTaxWithholding(decimal payoutAmount, double taxRate);
        
        decimal GetRemainingPrincipal(string policyId, DateTime asOfDate);
        
        decimal CalculatePenaltyForEarlyWithdrawal(string policyId, decimal withdrawalAmount);

        double GetCurrentInterestRate(string policyId);
        
        double CalculateCostOfLivingAdjustment(string scheduleId, int year);
        
        double GetSurvivorBenefitRatio(string policyId);

        bool IsEligibleForPayout(string policyId, DateTime requestedDate);
        
        bool ValidateScheduleParameters(string policyId, int payoutFrequency, decimal amount);
        
        bool HasActiveSchedule(string policyId);
        
        bool SuspendPayoutSchedule(string scheduleId, string reasonCode);
        
        bool ResumePayoutSchedule(string scheduleId);
        
        bool ApproveScheduleModifications(string scheduleId, string approverId);

        int GetRemainingPayoutCount(string scheduleId);
        
        int GetDaysUntilNextPayout(string scheduleId, DateTime currentDate);
        
        int CalculateTotalInstallments(DateTime startDate, DateTime endDate, int frequencyCode);
        
        int GetGracePeriodDays(string policyId);

        string GenerateScheduleId(string policyId, DateTime creationDate);
        
        string GetPayoutStatusCode(string scheduleId);
        
        string DetermineTaxFormType(string policyId, decimal annualPayoutTotal);
        
        string UpdateBeneficiaryDetails(string scheduleId, string beneficiaryId);
        
        string GetNextProcessingBatchId(DateTime processingDate);
    }
}