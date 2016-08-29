using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    /// <summary>Generates loan closure documentation upon successful recovery from maturity.</summary>
    public interface ILoanDischargeService
    {
        string GenerateDischargeLetter(string policyId, string loanId, DateTime dischargeDate);

        bool ValidateLoanClosureEligibility(string loanId, decimal maturityAmount);

        decimal CalculateFinalSettlementAmount(decimal outstandingPrincipal, decimal accruedInterest, decimal penaltyFees);

        double GetEffectiveRecoveryRate(string policyId, DateTime maturityDate);

        int GetDaysInArrears(string loanId, DateTime calculationDate);

        string IssueClearanceCertificate(string policyId, string customerId);

        bool IsFullRecoveryAchieved(decimal totalRecovered, decimal totalDue);

        decimal ComputeWriteOffAmount(decimal outstandingBalance, double writeOffPercentage);

        int CountActiveLiens(string policyId);

        string RetrieveDischargeTemplateId(string productCode, int regionCode);

        double CalculateInterestRebate(decimal principal, double standardRate, double rebateRate, int daysEarly);

        bool VerifySignatures(string documentId, int requiredSignatures);

        decimal GetTotalRecoveredFromMaturity(string policyId, string loanId);

        string ArchiveDischargeRecord(string policyId, string documentPath, DateTime archiveDate);

        int GetGracePeriodDays(string productCode);

        bool CheckPendingDisputes(string loanId);

        decimal CalculateTaxOnForgivenDebt(decimal forgivenAmount, double taxRate);
    }
}