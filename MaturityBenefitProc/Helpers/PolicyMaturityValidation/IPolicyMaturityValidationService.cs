using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.PolicyMaturityValidation
{
    public interface IPolicyMaturityValidationService
    {
        PolicyMaturityValidationResult ValidatePolicyForMaturity(string policyNumber);

        PolicyMaturityValidationResult VerifyDocuments(string policyNumber, string documentType, string documentNumber);

        bool IsPolicyMatured(string policyNumber, DateTime checkDate);

        bool IsKycComplete(string cifNumber);

        PolicyMaturityValidationResult CheckPremiumStatus(string policyNumber);

        bool HasAllPremiumsPaid(string policyNumber);

        bool HasOutstandingLoan(string policyNumber);

        decimal GetOutstandingLoanAmount(string policyNumber);

        PolicyMaturityValidationResult ValidateNomineeDetails(string policyNumber);

        PolicyMaturityValidationResult ValidateBankDetails(string cifNumber, string accountNumber);

        bool IsWithinClaimWindow(string policyNumber, DateTime claimDate);

        int GetDaysToMaturity(string policyNumber, DateTime fromDate);

        PolicyMaturityValidationResult GetValidationSummary(string policyNumber);

        List<PolicyMaturityValidationResult> GetValidationHistory(string policyNumber, DateTime fromDate, DateTime toDate);

        bool ValidateClaimantIdentity(string cifNumber, string panNumber, DateTime dateOfBirth);
    }
}
