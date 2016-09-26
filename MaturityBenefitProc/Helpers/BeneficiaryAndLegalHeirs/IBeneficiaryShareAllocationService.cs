using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    /// <summary>Calculates split payouts for multiple beneficiaries based on percentage shares.</summary>
    public interface IBeneficiaryShareAllocationService
    {
        decimal CalculateBasePayoutAmount(string policyId, decimal totalBenefit);
        decimal CalculateBeneficiaryShareAmount(decimal totalAmount, double sharePercentage);
        decimal ApplyTaxDeductionToShare(decimal shareAmount, double taxRate);
        decimal CalculateLateInterest(decimal baseAmount, double interestRate, int daysLate);
        decimal GetTotalAllocatedFunds(string policyId);
        decimal CalculateLegalHeirDisputeReserve(decimal totalBenefit, int disputingHeirsCount);
        decimal AdjustPayoutForOutstandingPremiums(decimal payoutAmount, decimal outstandingDebt);

        double GetPrimaryBeneficiarySharePercentage(string beneficiaryId);
        double GetContingentBeneficiarySharePercentage(string beneficiaryId);
        double CalculateRemainingSharePool(string policyId);
        double AdjustShareForMinorBeneficiary(double originalShare, int age);
        double GetApplicableTaxRate(string beneficiaryId, string stateCode);

        bool ValidateTotalSharesEqualOneHundredPercent(string policyId);
        bool IsBeneficiaryEligibleForPayout(string beneficiaryId, DateTime dateOfDeath);
        bool HasLegalHeirDisputes(string policyId);
        bool RequiresGuardianSignoff(string beneficiaryId, DateTime birthDate);
        bool VerifyBankRoutingInformation(string routingNumber);
        bool CheckIfBeneficiaryIsDeceased(string beneficiaryId);

        int GetActiveBeneficiaryCount(string policyId);
        int GetDaysSinceMaturity(DateTime maturityDate);
        int CountEligibleLegalHeirs(string policyId);
        int GetProcessingSlaDays(string policyType);

        string GenerateAllocationTransactionId(string policyId, string beneficiaryId);
        string GetPayoutStatusCode(string beneficiaryId);
        string DetermineTaxFormRequirement(decimal payoutAmount, bool isForeignNational);
        string GetGuardianIdForMinor(string minorBeneficiaryId);
        string ResolveDisputedShareHoldCode(string policyId);
    }
}