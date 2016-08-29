using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    /// <summary>Performs the net calculation of maturity value minus loan and interest.</summary>
    public interface IMaturityLoanAdjustmentService
    {
        decimal CalculateNetMaturityValue(string policyId, DateTime maturityDate);
        decimal CalculateTotalOutstandingLoan(string policyId, DateTime calculationDate);
        decimal CalculateAccruedInterest(string loanId, double interestRate, DateTime toDate);
        bool IsPolicyEligibleForMaturity(string policyId);
        bool HasOutstandingLoans(string policyId);
        double GetApplicableInterestRate(string policyId, string loanType);
        int GetDaysInArrears(string loanId, DateTime currentDate);
        string GetLoanStatusCode(string loanId);
        decimal CalculatePenalInterest(string loanId, int daysOverdue, double penaltyRate);
        decimal GetGrossMaturityBenefit(string policyId);
        decimal ApplyLoanRecoveryDeduction(string policyId, decimal grossMaturityAmount, decimal recoveryAmount);
        bool ValidateRecoveryAmount(string policyId, decimal recoveryAmount);
        int GetActiveLoanCount(string policyId);
        string GenerateRecoveryTransactionId(string policyId, DateTime transactionDate);
        double CalculateLoanToValueRatio(string policyId, decimal loanAmount, decimal policyValue);
        decimal CalculateCapitalizedInterest(string loanId, DateTime lastCapitalizationDate);
        bool IsInterestCapitalizationAllowed(string policyId);
        decimal GetUnpaidPremiumDeduction(string policyId, DateTime maturityDate);
        int GetMonthsToMaturity(string policyId, DateTime currentDate);
        string RetrievePolicyCurrencyCode(string policyId);
        decimal CalculateTaxOnRecovery(decimal recoveryAmount, double taxRate);
        bool CheckLoanDefaultStatus(string loanId);
        decimal GetTotalRecoverableAmount(string policyId);
    }
}