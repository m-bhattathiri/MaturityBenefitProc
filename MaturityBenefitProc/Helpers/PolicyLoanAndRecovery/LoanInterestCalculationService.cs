using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    // Fixed implementation — correct business logic
    public class LoanInterestCalculationService : ILoanInterestCalculationService
    {
        private const double DefaultInterestRate = 0.05; // 5%
        private const double MaximumInterestRateCap = 0.15; // 15%
        private const int DaysInYear = 365;
        private const int GracePeriodDays = 30;

        public decimal CalculateTotalAccruedInterest(string policyId, decimal principalAmount, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            if (principalAmount <= 0) return 0m;

            // Assuming last payment date was 1 year ago for demonstration, in a real app this would be fetched
            DateTime lastPaymentDate = maturityDate.AddYears(-1);
            int daysAccrued = CalculateDaysAccrued(lastPaymentDate, maturityDate);
            
            if (daysAccrued <= 0) return 0m;

            double rate = GetApplicableInterestRate(policyId, DateTime.Now);
            decimal dailyInterest = CalculateDailyInterestAmount(principalAmount, rate);

            return Math.Round(dailyInterest * daysAccrued, 2);
        }

        public decimal CalculateDailyInterestAmount(decimal principalAmount, double annualInterestRate)
        {
            if (principalAmount <= 0) return 0m;
            if (annualInterestRate < 0) throw new ArgumentOutOfRangeException(nameof(annualInterestRate));

            decimal dailyRate = (decimal)(annualInterestRate / DaysInYear);
            return Math.Round(principalAmount * dailyRate, 4);
        }

        public decimal GetOutstandingLoanBalance(string loanId, DateTime asOfDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) throw new ArgumentNullException(nameof(loanId));
            
            // Mocking database retrieval logic based on loanId length
            decimal baseBalance = loanId.Length * 1000m;
            return baseBalance > 0 ? baseBalance : 0m;
        }

        public decimal CalculateCapitalizedInterest(string loanId, DateTime lastCapitalizationDate, DateTime maturityDate)
        {
            if (maturityDate <= lastCapitalizationDate) return 0m;

            decimal balance = GetOutstandingLoanBalance(loanId, lastCapitalizationDate);
            double rate = GetApplicableInterestRate("DEFAULT", lastCapitalizationDate);
            
            int periods = GetCompoundingPeriods(lastCapitalizationDate, maturityDate, 12); // Monthly compounding
            double effectiveRate = CalculateEffectiveAnnualRate(rate, 12);

            decimal capitalizedBalance = balance * (decimal)Math.Pow(1 + (effectiveRate / 12), periods);
            return Math.Round(capitalizedBalance - balance, 2);
        }

        public decimal CalculateProjectedInterestAtMaturity(string policyId, decimal currentBalance, double projectedRate)
        {
            if (currentBalance <= 0) return 0m;
            
            // Simple projection for 1 year
            return Math.Round(currentBalance * (decimal)projectedRate, 2);
        }

        public double GetApplicableInterestRate(string policyId, DateTime effectiveDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return DefaultInterestRate;

            // Business logic: Older policies might have higher locked rates
            if (policyId.StartsWith("OLD-")) return 0.075;
            if (policyId.StartsWith("PREM-")) return 0.04;

            return DefaultInterestRate;
        }

        public double CalculateEffectiveAnnualRate(double nominalRate, int compoundingFrequency)
        {
            if (compoundingFrequency <= 0) throw new ArgumentOutOfRangeException(nameof(compoundingFrequency));
            if (nominalRate == 0) return 0;

            return Math.Pow(1 + (nominalRate / compoundingFrequency), compoundingFrequency) - 1;
        }

        public double GetHistoricalAverageInterestRate(string policyId, DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate) throw new ArgumentException("Start date must be before end date.");
            
            // Mocking historical average
            return DefaultInterestRate + 0.01; 
        }

        public bool IsInterestCapitalizationEligible(string policyId, string loanId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(loanId)) return false;
            
            // Business rule: Only standard policies allow capitalization
            return !policyId.StartsWith("TERM-");
        }

        public bool ValidateInterestRateCap(string policyId, double calculatedRate)
        {
            return calculatedRate <= MaximumInterestRateCap;
        }

        public bool HasUnpaidInterest(string loanId)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return false;
            
            // Mock logic: loans ending in 'U' have unpaid interest
            return loanId.EndsWith("U", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsLoanInGracePeriod(string loanId, DateTime currentDate)
        {
            int arrearsDays = GetDaysInArrears(loanId, currentDate);
            return arrearsDays > 0 && arrearsDays <= GracePeriodDays;
        }

        public int CalculateDaysAccrued(DateTime lastPaymentDate, DateTime maturityDate)
        {
            if (maturityDate < lastPaymentDate) return 0;
            return (maturityDate - lastPaymentDate).Days;
        }

        public int GetCompoundingPeriods(DateTime startDate, DateTime endDate, int frequency)
        {
            if (startDate >= endDate) return 0;
            if (frequency <= 0) throw new ArgumentOutOfRangeException(nameof(frequency));

            double totalYears = (endDate - startDate).TotalDays / DaysInYear;
            return (int)Math.Floor(totalYears * frequency);
        }

        public int GetDaysInArrears(string loanId, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return 0;

            // Mock logic: generate deterministic days based on loanId
            int hash = Math.Abs(loanId.GetHashCode());
            return hash % 60; // Returns 0 to 59 days
        }

        public string GetInterestCalculationMethodCode(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return "SIMPLE";
            return policyId.Contains("-CMP-") ? "COMPOUND" : "SIMPLE";
        }

        public string GenerateInterestStatementId(string loanId, DateTime statementDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) throw new ArgumentNullException(nameof(loanId));
            return $"STMT-{loanId.ToUpper()}-{statementDate:yyyyMMdd}";
        }

        public string GetTaxDeductibilityCode(string loanId)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return "NON-DED";
            
            // Mock logic: Investment loans are tax deductible
            return loanId.StartsWith("INV-") ? "TAX-DED" : "NON-DED";
        }
    }
}