using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    // Buggy stub — returns incorrect values
    public class AnnuityCalculationService : IAnnuityCalculationService
    {
        public decimal CalculateMonthlyPayout(string policyId, decimal accumulatedCorpus, double interestRate) => 0m;
        public decimal CalculateAnnualPayout(string policyId, decimal accumulatedCorpus, double interestRate) => 0m;
        public decimal CalculateQuarterlyPayout(string policyId, decimal accumulatedCorpus, double interestRate) => 0m;
        public decimal CalculateSemiAnnualPayout(string policyId, decimal accumulatedCorpus, double interestRate) => 0m;
        
        public decimal GetTotalAccumulatedCorpus(string policyId, DateTime maturityDate) => 0m;
        public decimal CalculateCommutationAmount(string policyId, decimal totalCorpus, double commutationPercentage) => 0m;
        public decimal CalculateResidualCorpus(decimal totalCorpus, decimal commutedAmount) => 0m;
        
        public double GetAnnuityFactor(int ageAtVesting, string annuityOptionCode, double prevailingRate) => 0.0;
        public double GetCurrentInterestRate(string productCode, DateTime effectiveDate) => 0.0;
        public double CalculateInternalRateOfReturn(string policyId, decimal totalPremiumsPaid, decimal expectedPayout) => 0.0;
        public double ComputeMortalityChargeRate(int currentAge, string genderCode) => 0.0;
        public double CalculateInflationAdjustmentFactor(DateTime baseDate, DateTime currentDate, double inflationRate) => 0.0;
        
        public bool IsEligibleForCommutation(string policyId, int ageAtVesting) => false;
        public bool IsPolicyActive(string policyId, DateTime checkDate) => false;
        public bool ValidateSpouseDateOfBirth(string policyId, DateTime spouseDob) => false;
        public bool IsJointLifeApplicable(string annuityOptionCode) => false;
        public bool HasGuaranteedPeriodExpired(string policyId, DateTime currentDate) => false;
        public bool CanDeferPayout(string policyId, int requestedDefermentMonths) => false;
        public bool IsMinimumCorpusMet(decimal accumulatedCorpus, string productCode) => false;
        
        public int CalculateAgeAtVesting(DateTime dateOfBirth, DateTime vestingDate) => 0;
        public int GetRemainingGuaranteedMonths(string policyId, int guaranteedPeriodYears, int payoutsMade) => 0;
        public int GetDefermentPeriodMonths(DateTime vestingDate, DateTime deferredStartDate) => 0;
        public int GetTotalPayoutsMade(string policyId) => 0;
        public int CalculateDaysToNextPayout(DateTime lastPayoutDate, string frequencyCode) => 0;
        public int GetPremiumPaymentTerm(string policyId) => 0;
        
        public string GetAnnuityOptionCode(string policyId) => null;
        public string GeneratePayoutTransactionId(string policyId, DateTime payoutDate) => null;
        public string GetTaxSlabCode(decimal annualIncome, int age) => null;
        public string DeterminePayoutFrequencyCode(string policyId) => null;
        public string GetBeneficiaryRelationshipCode(string policyId, string beneficiaryId) => null;
        
        public decimal CalculateDeathBenefit(string policyId, decimal accumulatedCorpus, DateTime dateOfDeath) => 0m;
        public decimal CalculateSurrenderValue(string policyId, decimal accumulatedCorpus, int policyYear) => 0m;
        public decimal ComputeTaxablePortion(decimal payoutAmount, double taxRate) => 0m;
        public decimal GetGuaranteedMinimumWithdrawalBenefit(string policyId, decimal initialCorpus) => 0m;
        public decimal CalculateLumpSumPayout(string policyId, decimal accumulatedCorpus, bool includesTerminalBonus) => 0m;
    }
}