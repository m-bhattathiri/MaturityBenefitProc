using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.NomineeVerification
{
    public interface INomineeVerificationService
    {
        NomineeVerificationResult VerifyNominee(string policyNumber, string nomineeId);

        NomineeVerificationResult ValidateNomineeDocuments(string nomineeId, string documentType, string documentNumber);

        bool IsNomineeMinor(string nomineeId, DateTime checkDate);

        NomineeVerificationResult AssignGuardian(string nomineeId, string guardianName, string guardianRelation);

        NomineeVerificationResult UpdateNomineeShare(string policyNumber, string nomineeId, decimal sharePercentage);

        bool ValidateShareAllocation(string policyNumber);

        decimal GetTotalShareAllocated(string policyNumber);

        NomineeVerificationResult GetNomineeDetails(string nomineeId);

        NomineeVerificationResult VerifyNomineeKyc(string nomineeId, string panNumber, string aadhaarNumber);

        bool HasValidGuardian(string nomineeId);

        int GetNomineeCount(string policyNumber);

        NomineeVerificationResult RemoveNominee(string policyNumber, string nomineeId, string reason);

        List<NomineeVerificationResult> GetNomineeVerificationHistory(string policyNumber, DateTime fromDate, DateTime toDate);

        bool IsNomineeShareValid(decimal sharePercentage);

        NomineeVerificationResult AddNominee(string policyNumber, string nomineeName, string relation, decimal sharePercentage);
    }
}
