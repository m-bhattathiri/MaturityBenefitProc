using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    /// <summary>Manages appointee details and payouts when the nominee is a minor.</summary>
    public interface IMinorNomineeTrusteeService
    {
        bool IsNomineeMinor(string nomineeId, DateTime dateOfBirth, DateTime maturityDate);
        bool ValidateAppointeeEligibility(string appointeeId, string relationshipCode);
        bool HasActiveTrusteeMandate(string policyId, string nomineeId);
        bool IsPayoutAllowedToAppointee(string appointeeId, decimal payoutAmount);
        bool VerifyAppointeeKycStatus(string appointeeId);

        decimal CalculateAppointeePayoutShare(decimal totalMaturityAmount, double sharePercentage);
        decimal GetTotalDisbursedToAppointee(string appointeeId, string policyId);
        decimal CalculateTrusteeMaintenanceAllowance(decimal baseAmount, int durationInMonths);
        decimal ComputeMinorEducationFundAllocation(decimal totalBenefit, double allocationRate);
        decimal GetPendingPayoutAmount(string nomineeId, string policyId);

        double GetAppointeeSharePercentage(string nomineeId, string appointeeId);
        double CalculateTrusteeFeeRate(int managementDurationDays);
        double GetMinorAgeRatioAtMaturity(DateTime dateOfBirth, DateTime maturityDate);
        double GetTaxDeductionRateForAppointee(string appointeeId, string taxCategoryCode);

        int GetDaysUntilNomineeMajority(DateTime dateOfBirth, DateTime currentDate);
        int CountActiveAppointeesForMinor(string nomineeId);
        int GetTrusteeMandateDurationMonths(DateTime mandateStartDate, DateTime mandateEndDate);
        int GetNumberOfInstallmentsProcessed(string appointeeId, string policyId);
        int GetAppointeeAge(DateTime appointeeDob, DateTime currentDate);

        string RegisterNewAppointee(string nomineeId, string firstName, string lastName, DateTime dob);
        string GetPrimaryAppointeeId(string nomineeId);
        string GenerateTrusteeMandateReference(string policyId, string appointeeId);
        string GetAppointeeRelationshipCode(string nomineeId, string appointeeId);
        string RetrieveAppointeeBankAccountId(string appointeeId);
        string ResolvePayoutStatusCategory(string policyId, string appointeeId);
    }
}