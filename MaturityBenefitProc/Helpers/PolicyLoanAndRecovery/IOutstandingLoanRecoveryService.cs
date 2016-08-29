using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    /// <summary>
    /// Identifies active policy loans to be recovered from maturity proceeds.
    /// </summary>
    public interface IOutstandingLoanRecoveryService
    {
        decimal CalculateTotalOutstandingPrincipal(string policyId, DateTime maturityDate);
        decimal CalculateAccruedInterest(string loanId, DateTime calculationDate);
        decimal GetDailyInterestAccrualAmount(string loanId, decimal currentBalance, double interestRate);
        decimal CalculatePenalties(string loanId, int daysInArrears);
        decimal GetTotalRecoveryAmount(string policyId, string loanId);
        decimal CalculateTaxOnRecovery(decimal recoveryAmount, string taxCode);
        decimal GetRemainingMaturityProceeds(decimal totalMaturityValue, decimal totalRecoveryAmount);
        
        double GetCurrentInterestRate(string loanId);
        double GetPenaltyRate(string loanId, string policyType);
        double CalculateLoanToValueRatio(decimal loanAmount, decimal policyCashValue);
        double GetHistoricalAverageInterestRate(string loanId, DateTime startDate, DateTime endDate);
        
        bool IsLoanEligibleForRecovery(string loanId, DateTime maturityDate);
        bool HasActiveDefault(string policyId);
        bool IsRecoveryAmountWithinLimits(decimal recoveryAmount, decimal maturityProceeds);
        bool ValidateLoanStatus(string loanId, string expectedStatusCode);
        bool RequiresManualReview(string policyId, decimal totalRecoveryAmount, int activeLoanCount);
        
        int GetDaysInArrears(string loanId, DateTime currentDate);
        int GetNumberOfActiveLoans(string policyId);
        int GetRemainingTermDays(string loanId, DateTime maturityDate);
        int GetGracePeriodDays(string policyType);
        
        string GetPrimaryLoanId(string policyId);
        string GetRecoveryTransactionReference(string policyId, decimal amountRecovered);
        string GetLoanStatusCode(string loanId);
        string GenerateRecoveryAuditTrailId(string policyId, string userId, DateTime timestamp);
    }
}