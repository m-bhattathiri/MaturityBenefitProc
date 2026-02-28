using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    // Fixed implementation — correct business logic
    public class MaturityLoanAdjustmentService : IMaturityLoanAdjustmentService
    {
        private const decimal MAX_LTV_RATIO = 0.90m;
        private const string DEFAULT_CURRENCY = "USD";
        private const double BASE_INTEREST_RATE = 0.05;

        public decimal CalculateNetMaturityValue(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.");

            decimal grossBenefit = GetGrossMaturityBenefit(policyId);
            decimal totalOutstandingLoan = CalculateTotalOutstandingLoan(policyId, maturityDate);
            decimal unpaidPremiums = GetUnpaidPremiumDeduction(policyId, maturityDate);

            decimal netValue = grossBenefit - totalOutstandingLoan - unpaidPremiums;
            return netValue > 0 ? netValue : 0m;
        }

        public decimal CalculateTotalOutstandingLoan(string policyId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0m;

            // Simulated DB fetch for active loans
            decimal principal = 10000m; 
            decimal accruedInterest = CalculateAccruedInterest("L-" + policyId, BASE_INTEREST_RATE, calculationDate);
            
            return principal + accruedInterest;
        }

        public decimal CalculateAccruedInterest(string loanId, double interestRate, DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return 0m;
            if (interestRate < 0) throw new ArgumentException("Interest rate cannot be negative.");

            // Simulated loan start date
            DateTime loanStartDate = toDate.AddYears(-2);
            if (toDate <= loanStartDate) return 0m;

            int daysElapsed = (toDate - loanStartDate).Days;
            decimal principal = 10000m; // Mock principal
            
            // Simple interest calculation: P * R * T
            decimal interest = principal * (decimal)interestRate * (daysElapsed / 365m);
            return Math.Round(interest, 2);
        }

        public bool IsPolicyEligibleForMaturity(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            
            // Mock logic: Policy is eligible if it has less than 3 months to maturity
            int monthsToMaturity = GetMonthsToMaturity(policyId, DateTime.UtcNow);
            return monthsToMaturity <= 3 && monthsToMaturity >= 0;
        }

        public bool HasOutstandingLoans(string policyId)
        {
            return GetActiveLoanCount(policyId) > 0;
        }

        public double GetApplicableInterestRate(string policyId, string loanType)
        {
            if (string.IsNullOrWhiteSpace(loanType)) return BASE_INTEREST_RATE;

            if (loanType.ToUpper() == "SECURED")
            {
                return 0.04;
            }
            else if (loanType.ToUpper() == "UNSECURED")
            {
                return 0.08;
            }
            else if (loanType.ToUpper() == "PREMIUM_LOAN")
            {
                return 0.06;
            }
            else
            {
                return BASE_INTEREST_RATE;
            }
        }

        public int GetDaysInArrears(string loanId, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return 0;

            // Mock due date
            DateTime dueDate = currentDate.AddDays(-45); 
            if (currentDate <= dueDate) return 0;

            return (currentDate - dueDate).Days;
        }

        public string GetLoanStatusCode(string loanId)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return "UNKNOWN";

            int arrears = GetDaysInArrears(loanId, DateTime.UtcNow);
            if (arrears == 0) return "ACTIVE_GOOD_STANDING";
            if (arrears <= 30) return "GRACE_PERIOD";
            if (arrears <= 90) return "DELINQUENT";
            return "DEFAULTED";
        }

        public decimal CalculatePenalInterest(string loanId, int daysOverdue, double penaltyRate)
        {
            if (daysOverdue <= 0 || penaltyRate <= 0) return 0m;

            decimal outstandingBalance = 10000m; // Mock balance
            decimal penalInterest = outstandingBalance * (decimal)penaltyRate * (daysOverdue / 365m);
            
            return Math.Round(penalInterest, 2);
        }

        public decimal GetGrossMaturityBenefit(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Invalid policy ID");
            
            // Mock DB retrieval
            return 50000m; 
        }

        public decimal ApplyLoanRecoveryDeduction(string policyId, decimal grossMaturityAmount, decimal recoveryAmount)
        {
            if (grossMaturityAmount < 0 || recoveryAmount < 0) 
                throw new ArgumentException("Amounts cannot be negative.");

            if (!ValidateRecoveryAmount(policyId, recoveryAmount))
                throw new InvalidOperationException("Recovery amount exceeds allowable limits.");

            decimal netAmount = grossMaturityAmount - recoveryAmount;
            return netAmount < 0 ? 0m : netAmount;
        }

        public bool ValidateRecoveryAmount(string policyId, decimal recoveryAmount)
        {
            decimal totalRecoverable = GetTotalRecoverableAmount(policyId);
            return recoveryAmount <= totalRecoverable;
        }

        public int GetActiveLoanCount(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;
            // Mock DB call
            return policyId.StartsWith("POL") ? 1 : 0;
        }

        public string GenerateRecoveryTransactionId(string policyId, DateTime transactionDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID required.");
            
            string datePart = transactionDate.ToString("yyyyMMddHHmmss");
            string shortPolicy = policyId.Length > 4 ? policyId.Substring(policyId.Length - 4) : policyId;
            
            return $"REC-{datePart}-{shortPolicy}";
        }

        public double CalculateLoanToValueRatio(string policyId, decimal loanAmount, decimal policyValue)
        {
            if (policyValue <= 0) return 0.0;
            return (double)(loanAmount / policyValue);
        }

        public decimal CalculateCapitalizedInterest(string loanId, DateTime lastCapitalizationDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return 0m;
            
            DateTime now = DateTime.UtcNow;
            if (now <= lastCapitalizationDate) return 0m;

            // Mock logic
            decimal uncapitalizedInterest = 500m; 
            return uncapitalizedInterest;
        }

        public bool IsInterestCapitalizationAllowed(string policyId)
        {
            // Mock business rule: Allowed for specific policy prefixes
            return !string.IsNullOrWhiteSpace(policyId) && policyId.StartsWith("ULIP");
        }

        public decimal GetUnpaidPremiumDeduction(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0m;
            
            // Mock logic
            return 1200m; 
        }

        public int GetMonthsToMaturity(string policyId, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;

            // Mock maturity date
            DateTime maturityDate = currentDate.AddMonths(2); 
            
            int months = ((maturityDate.Year - currentDate.Year) * 12) + maturityDate.Month - currentDate.Month;
            return months;
        }

        public string RetrievePolicyCurrencyCode(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return DEFAULT_CURRENCY;
            
            // Mock logic
            return policyId.EndsWith("-UK") ? "GBP" : "USD";
        }

        public decimal CalculateTaxOnRecovery(decimal recoveryAmount, double taxRate)
        {
            if (recoveryAmount <= 0 || taxRate <= 0) return 0m;
            return Math.Round(recoveryAmount * (decimal)taxRate, 2);
        }

        public bool CheckLoanDefaultStatus(string loanId)
        {
            string status = GetLoanStatusCode(loanId);
            return status == "DEFAULTED";
        }

        public decimal GetTotalRecoverableAmount(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0m;
            
            decimal outstandingLoan = CalculateTotalOutstandingLoan(policyId, DateTime.UtcNow);
            decimal unpaidPremiums = GetUnpaidPremiumDeduction(policyId, DateTime.UtcNow);
            
            return outstandingLoan + unpaidPremiums;
        }
    }
}