using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    /// <summary>Calculates periodic annuity payout amounts based on accumulated corpus.</summary>
    public interface IAnnuityCalculationService
    {
        decimal CalculateMonthlyPayout(string policyId, decimal accumulatedCorpus, double interestRate);
        decimal CalculateAnnualPayout(string policyId, decimal accumulatedCorpus, double interestRate);
        decimal CalculateQuarterlyPayout(string policyId, decimal accumulatedCorpus, double interestRate);
        decimal CalculateSemiAnnualPayout(string policyId, decimal accumulatedCorpus, double interestRate);
        
        decimal GetTotalAccumulatedCorpus(string policyId, DateTime maturityDate);
        decimal CalculateCommutationAmount(string policyId, decimal totalCorpus, double commutationPercentage);
        decimal CalculateResidualCorpus(decimal totalCorpus, decimal commutedAmount);
        
        double GetAnnuityFactor(int ageAtVesting, string annuityOptionCode, double prevailingRate);
        double GetCurrentInterestRate(string productCode, DateTime effectiveDate);
        double CalculateInternalRateOfReturn(string policyId, decimal totalPremiumsPaid, decimal expectedPayout);
        double ComputeMortalityChargeRate(int currentAge, string genderCode);
        double CalculateInflationAdjustmentFactor(DateTime baseDate, DateTime currentDate, double inflationRate);
        
        bool IsEligibleForCommutation(string policyId, int ageAtVesting);
        bool IsPolicyActive(string policyId, DateTime checkDate);
        bool ValidateSpouseDateOfBirth(string policyId, DateTime spouseDob);
        bool IsJointLifeApplicable(string annuityOptionCode);
        bool HasGuaranteedPeriodExpired(string policyId, DateTime currentDate);
        bool CanDeferPayout(string policyId, int requestedDefermentMonths);
        bool IsMinimumCorpusMet(decimal accumulatedCorpus, string productCode);
        
        int CalculateAgeAtVesting(DateTime dateOfBirth, DateTime vestingDate);
        int GetRemainingGuaranteedMonths(string policyId, int guaranteedPeriodYears, int payoutsMade);
        int GetDefermentPeriodMonths(DateTime vestingDate, DateTime deferredStartDate);
        int GetTotalPayoutsMade(string policyId);
        int CalculateDaysToNextPayout(DateTime lastPayoutDate, string frequencyCode);
        int GetPremiumPaymentTerm(string policyId);
        
        string GetAnnuityOptionCode(string policyId);
        string GeneratePayoutTransactionId(string policyId, DateTime payoutDate);
        string GetTaxSlabCode(decimal annualIncome, int age);
        string DeterminePayoutFrequencyCode(string policyId);
        string GetBeneficiaryRelationshipCode(string policyId, string beneficiaryId);
        
        decimal CalculateDeathBenefit(string policyId, decimal accumulatedCorpus, DateTime dateOfDeath);
        decimal CalculateSurrenderValue(string policyId, decimal accumulatedCorpus, int policyYear);
        decimal ComputeTaxablePortion(decimal payoutAmount, double taxRate);
        decimal GetGuaranteedMinimumWithdrawalBenefit(string policyId, decimal initialCorpus);
        decimal CalculateLumpSumPayout(string policyId, decimal accumulatedCorpus, bool includesTerminalBonus);
    }
}