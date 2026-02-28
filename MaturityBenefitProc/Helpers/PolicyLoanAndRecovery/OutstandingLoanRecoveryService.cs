using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    // Fixed implementation — correct business logic
    public class OutstandingLoanRecoveryService : IOutstandingLoanRecoveryService
    {
        private const decimal MANUAL_REVIEW_THRESHOLD = 50000m;
        private const int MAX_AUTO_LOAN_COUNT = 3;
        private const double DEFAULT_INTEREST_RATE = 0.05;
        private const double DEFAULT_PENALTY_RATE = 0.02;

        public decimal CalculateTotalOutstandingPrincipal(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            
            // Simulated database lookup for outstanding principal
            decimal basePrincipal = 10000m;
            decimal paymentsMade = 2500m;
            
            return Math.Max(0, basePrincipal - paymentsMade);
        }

        public decimal CalculateAccruedInterest(string loanId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) throw new ArgumentNullException(nameof(loanId));

            // Simulated logic
            decimal principal = 7500m;
            double rate = GetCurrentInterestRate(loanId);
            int days = 45; // Simulated days since last payment
            
            return principal * (decimal)rate * (days / 365m);
        }

        public decimal GetDailyInterestAccrualAmount(string loanId, decimal currentBalance, double interestRate)
        {
            if (currentBalance <= 0) return 0m;
            if (interestRate < 0) throw new ArgumentOutOfRangeException(nameof(interestRate));

            return currentBalance * (decimal)interestRate / 365m;
        }

        public decimal CalculatePenalties(string loanId, int daysInArrears)
        {
            if (daysInArrears <= 0) return 0m;

            // Simulated logic
            decimal outstandingBalance = 7500m;
            double penaltyRate = GetPenaltyRate(loanId, "STANDARD");
            
            return outstandingBalance * (decimal)penaltyRate * (daysInArrears / 365m);
        }

        public decimal GetTotalRecoveryAmount(string policyId, string loanId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(loanId))
                throw new ArgumentNullException("PolicyId and LoanId must be provided.");

            DateTime today = DateTime.UtcNow.Date;
            decimal principal = CalculateTotalOutstandingPrincipal(policyId, today);
            decimal interest = CalculateAccruedInterest(loanId, today);
            int arrears = GetDaysInArrears(loanId, today);
            decimal penalties = CalculatePenalties(loanId, arrears);

            return principal + interest + penalties;
        }

        public decimal CalculateTaxOnRecovery(decimal recoveryAmount, string taxCode)
        {
            if (recoveryAmount <= 0) return 0m;

            decimal taxRate;
            if (taxCode == "TAX_FREE")
            {
                taxRate = 0m;
            }
            else if (taxCode == "STANDARD")
            {
                taxRate = 0.15m;
            }
            else if (taxCode == "HIGH")
            {
                taxRate = 0.25m;
            }
            else
            {
                taxRate = 0.10m;
            }

            return recoveryAmount * taxRate;
        }

        public decimal GetRemainingMaturityProceeds(decimal totalMaturityValue, decimal totalRecoveryAmount)
        {
            if (totalMaturityValue < 0) throw new ArgumentOutOfRangeException(nameof(totalMaturityValue));
            if (totalRecoveryAmount < 0) throw new ArgumentOutOfRangeException(nameof(totalRecoveryAmount));

            return Math.Max(0, totalMaturityValue - totalRecoveryAmount);
        }

        public double GetCurrentInterestRate(string loanId)
        {
            if (string.IsNullOrWhiteSpace(loanId)) throw new ArgumentNullException(nameof(loanId));
            return DEFAULT_INTEREST_RATE;
        }

        public double GetPenaltyRate(string loanId, string policyType)
        {
            if (string.IsNullOrWhiteSpace(loanId)) throw new ArgumentNullException(nameof(loanId));
            
            if (policyType?.ToUpperInvariant() == "PREMIUM")
            {
                return 0.01;
            }
            else if (policyType?.ToUpperInvariant() == "STANDARD")
            {
                return DEFAULT_PENALTY_RATE;
            }
            else if (policyType?.ToUpperInvariant() == "HIGH_RISK")
            {
                return 0.05;
            }
            else
            {
                return DEFAULT_PENALTY_RATE;
            }
        }

        public double CalculateLoanToValueRatio(decimal loanAmount, decimal policyCashValue)
        {
            if (policyCashValue <= 0) return 0.0;
            if (loanAmount < 0) return 0.0;

            return (double)(loanAmount / policyCashValue);
        }

        public double GetHistoricalAverageInterestRate(string loanId, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Start date must be before end date.");
            return DEFAULT_INTEREST_RATE + 0.01; // Simulated historical average
        }

        public bool IsLoanEligibleForRecovery(string loanId, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) return false;
            string status = GetLoanStatusCode(loanId);
            return status == "ACTIVE" || status == "IN_ARREARS";
        }

        public bool HasActiveDefault(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Simulated check
            return policyId.EndsWith("-DEF");
        }

        public bool IsRecoveryAmountWithinLimits(decimal recoveryAmount, decimal maturityProceeds)
        {
            if (maturityProceeds <= 0) return false;
            return recoveryAmount <= maturityProceeds;
        }

        public bool ValidateLoanStatus(string loanId, string expectedStatusCode)
        {
            if (string.IsNullOrWhiteSpace(loanId) || string.IsNullOrWhiteSpace(expectedStatusCode)) return false;
            return GetLoanStatusCode(loanId).Equals(expectedStatusCode, StringComparison.OrdinalIgnoreCase);
        }

        public bool RequiresManualReview(string policyId, decimal totalRecoveryAmount, int activeLoanCount)
        {
            if (HasActiveDefault(policyId)) return true;
            if (totalRecoveryAmount > MANUAL_REVIEW_THRESHOLD) return true;
            if (activeLoanCount > MAX_AUTO_LOAN_COUNT) return true;
            
            return false;
        }

        public int GetDaysInArrears(string loanId, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(loanId)) throw new ArgumentNullException(nameof(loanId));
            // Simulated logic
            DateTime dueDate = currentDate.AddDays(-15);
            return currentDate > dueDate ? (currentDate - dueDate).Days : 0;
        }

        public int GetNumberOfActiveLoans(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            return 1; // Simulated count
        }

        public int GetRemainingTermDays(string loanId, DateTime maturityDate)
        {
            DateTime today = DateTime.UtcNow.Date;
            if (maturityDate <= today) return 0;
            return (maturityDate - today).Days;
        }

        public int GetGracePeriodDays(string policyType)
        {
            if (policyType?.ToUpperInvariant() == "PREMIUM")
            {
                return 60;
            }
            else if (policyType?.ToUpperInvariant() == "STANDARD")
            {
                return 30;
            }
            else
            {
                return 15;
            }
        }

        public string GetPrimaryLoanId(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            return $"LN-{policyId}-01";
        }

        public string GetRecoveryTransactionReference(string policyId, decimal amountRecovered)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            string dateStr = DateTime.UtcNow.ToString("yyyyMMdd");
            return $"REC-{policyId}-{dateStr}-{amountRecovered:0}";
        }

        public string GetLoanStatusCode(string loanId)
        {
            if (string.IsNullOrWhiteSpace(loanId)) throw new ArgumentNullException(nameof(loanId));
            return loanId.Contains("DEF") ? "DEFAULTED" : "ACTIVE";
        }

        public string GenerateRecoveryAuditTrailId(string policyId, string userId, DateTime timestamp)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));
            
            return $"AUD-{policyId}-{userId}-{timestamp.Ticks}";
        }
    }
}