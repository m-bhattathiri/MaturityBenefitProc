using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    /// <summary>Calculates accrued interest on policy loans up to the maturity date.</summary>
    public interface ILoanInterestCalculationService
    {
        decimal CalculateTotalAccruedInterest(string policyId, decimal principalAmount, DateTime maturityDate);
        
        decimal CalculateDailyInterestAmount(decimal principalAmount, double annualInterestRate);
        
        decimal GetOutstandingLoanBalance(string loanId, DateTime asOfDate);
        
        decimal CalculateCapitalizedInterest(string loanId, DateTime lastCapitalizationDate, DateTime maturityDate);
        
        decimal CalculateProjectedInterestAtMaturity(string policyId, decimal currentBalance, double projectedRate);
        
        double GetApplicableInterestRate(string policyId, DateTime effectiveDate);
        
        double CalculateEffectiveAnnualRate(double nominalRate, int compoundingFrequency);
        
        double GetHistoricalAverageInterestRate(string policyId, DateTime startDate, DateTime endDate);
        
        bool IsInterestCapitalizationEligible(string policyId, string loanId);
        
        bool ValidateInterestRateCap(string policyId, double calculatedRate);
        
        bool HasUnpaidInterest(string loanId);
        
        bool IsLoanInGracePeriod(string loanId, DateTime currentDate);
        
        int CalculateDaysAccrued(DateTime lastPaymentDate, DateTime maturityDate);
        
        int GetCompoundingPeriods(DateTime startDate, DateTime endDate, int frequency);
        
        int GetDaysInArrears(string loanId, DateTime currentDate);
        
        string GetInterestCalculationMethodCode(string policyId);
        
        string GenerateInterestStatementId(string loanId, DateTime statementDate);
        
        string GetTaxDeductibilityCode(string loanId);
    }
}