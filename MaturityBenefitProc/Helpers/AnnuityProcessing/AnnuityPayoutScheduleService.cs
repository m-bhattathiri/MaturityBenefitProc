using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.AnnuityProcessing
{
    // Fixed implementation — correct business logic
    public class AnnuityPayoutScheduleService : IAnnuityPayoutScheduleService
    {
        private const int DEFAULT_ANNUITY_YEARS = 20;
        private const decimal EARLY_WITHDRAWAL_PENALTY_RATE = 0.10m;
        private const decimal IRS_REPORTING_THRESHOLD = 600.00m;

        public decimal CalculateMonthlyPayout(string policyId, decimal principalAmount, double interestRate)
        {
            if (principalAmount <= 0) return 0m;
            if (interestRate < 0) throw new ArgumentException("Interest rate cannot be negative.");
            
            // Standard amortized monthly payout formula
            double monthlyRate = interestRate / 12.0;
            int totalMonths = DEFAULT_ANNUITY_YEARS * 12;
            
            if (monthlyRate == 0) return principalAmount / totalMonths;
            
            double factor = Math.Pow(1 + monthlyRate, totalMonths);
            decimal payout = principalAmount * (decimal)((monthlyRate * factor) / (factor - 1));
            
            return Math.Round(payout, 2);
        }

        public decimal CalculateQuarterlyPayout(string policyId, decimal principalAmount, double interestRate)
        {
            if (principalAmount <= 0) return 0m;
            
            double quarterlyRate = interestRate / 4.0;
            int totalQuarters = DEFAULT_ANNUITY_YEARS * 4;
            
            if (quarterlyRate == 0) return principalAmount / totalQuarters;
            
            double factor = Math.Pow(1 + quarterlyRate, totalQuarters);
            decimal payout = principalAmount * (decimal)((quarterlyRate * factor) / (factor - 1));
            
            return Math.Round(payout, 2);
        }

        public decimal CalculateAnnualPayout(string policyId, decimal principalAmount, double interestRate)
        {
            if (principalAmount <= 0) return 0m;
            
            if (interestRate == 0) return principalAmount / DEFAULT_ANNUITY_YEARS;
            
            double factor = Math.Pow(1 + interestRate, DEFAULT_ANNUITY_YEARS);
            decimal payout = principalAmount * (decimal)((interestRate * factor) / (factor - 1));
            
            return Math.Round(payout, 2);
        }

        public decimal GetTotalProjectedPayout(string scheduleId)
        {
            if (string.IsNullOrWhiteSpace(scheduleId)) throw new ArgumentNullException(nameof(scheduleId));
            // Mocking a database lookup for the schedule's total projected value
            return 250000.00m; 
        }

        public decimal CalculateTaxWithholding(decimal payoutAmount, double taxRate)
        {
            if (payoutAmount <= 0) return 0m;
            if (taxRate < 0 || taxRate > 1) throw new ArgumentOutOfRangeException(nameof(taxRate), "Tax rate must be between 0 and 1.");
            
            return Math.Round(payoutAmount * (decimal)taxRate, 2);
        }

        public decimal GetRemainingPrincipal(string policyId, DateTime asOfDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            if (asOfDate > DateTime.Now.AddYears(50)) throw new ArgumentException("Date too far in the future.");
            
            // Mocking remaining principal calculation
            return 150000.00m;
        }

        public decimal CalculatePenaltyForEarlyWithdrawal(string policyId, decimal withdrawalAmount)
        {
            if (withdrawalAmount <= 0) return 0m;
            
            // Apply a standard 10% penalty for early withdrawals
            return Math.Round(withdrawalAmount * EARLY_WITHDRAWAL_PENALTY_RATE, 2);
        }

        public double GetCurrentInterestRate(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            // Mocking a fixed interest rate based on policy tier
            return policyId.StartsWith("PREM") ? 0.055 : 0.035;
        }

        public double CalculateCostOfLivingAdjustment(string scheduleId, int year)
        {
            if (year < 2000 || year > 2100) throw new ArgumentOutOfRangeException(nameof(year));
            // Standard COLA adjustment mock (e.g., 2.5%)
            return 0.025;
        }

        public double GetSurvivorBenefitRatio(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            // Standard joint-and-survivor 50% or 100%
            return policyId.EndsWith("-J100") ? 1.0 : 0.5;
        }

        public bool IsEligibleForPayout(string policyId, DateTime requestedDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Basic eligibility rule: requested date must be in the past or today
            return requestedDate.Date <= DateTime.UtcNow.Date;
        }

        public bool ValidateScheduleParameters(string policyId, int payoutFrequency, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            if (amount <= 0) return false;
            // Frequencies: 1 = Annual, 4 = Quarterly, 12 = Monthly
            if (payoutFrequency != 1 && payoutFrequency != 4 && payoutFrequency != 12) return false;
            
            return true;
        }

        public bool HasActiveSchedule(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            // Mocking active schedule check
            return policyId.Length > 5;
        }

        public bool SuspendPayoutSchedule(string scheduleId, string reasonCode)
        {
            if (string.IsNullOrWhiteSpace(scheduleId) || string.IsNullOrWhiteSpace(reasonCode)) return false;
            // Logic to update database status to SUSPENDED
            return true;
        }

        public bool ResumePayoutSchedule(string scheduleId)
        {
            if (string.IsNullOrWhiteSpace(scheduleId)) return false;
            // Logic to update database status to ACTIVE
            return true;
        }

        public bool ApproveScheduleModifications(string scheduleId, string approverId)
        {
            if (string.IsNullOrWhiteSpace(scheduleId) || string.IsNullOrWhiteSpace(approverId)) return false;
            // Validate approver permissions and update schedule
            return approverId.StartsWith("MGR-");
        }

        public int GetRemainingPayoutCount(string scheduleId)
        {
            if (string.IsNullOrWhiteSpace(scheduleId)) throw new ArgumentNullException(nameof(scheduleId));
            // Mocking remaining count
            return 120;
        }

        public int GetDaysUntilNextPayout(string scheduleId, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(scheduleId)) throw new ArgumentNullException(nameof(scheduleId));
            
            // Assume payouts are on the 1st of the next month
            DateTime nextMonth = currentDate.AddMonths(1);
            DateTime nextPayoutDate = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            
            return (nextPayoutDate - currentDate).Days;
        }

        public int CalculateTotalInstallments(DateTime startDate, DateTime endDate, int frequencyCode)
        {
            if (endDate <= startDate) throw new ArgumentException("End date must be after start date.");
            
            int years = endDate.Year - startDate.Year;
            int months = years * 12 + (endDate.Month - startDate.Month);
            
            switch (frequencyCode)
            {
                case 12: return months; // Monthly
                case 4: return months / 3; // Quarterly
                case 1: return years; // Annual
                default: throw new ArgumentException("Invalid frequency code.");
            }
        }

        public int GetGracePeriodDays(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            // Standard grace period is 30 days
            return 30;
        }

        public string GenerateScheduleId(string policyId, DateTime creationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            return $"SCH-{policyId.ToUpper()}-{creationDate:yyyyMMddHHmmss}";
        }

        public string GetPayoutStatusCode(string scheduleId)
        {
            if (string.IsNullOrWhiteSpace(scheduleId)) throw new ArgumentNullException(nameof(scheduleId));
            // Mocking status retrieval
            return "ACTIVE";
        }

        public string DetermineTaxFormType(string policyId, decimal annualPayoutTotal)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            
            if (annualPayoutTotal >= IRS_REPORTING_THRESHOLD)
            {
                return "1099-R";
            }
            return "NONE";
        }

        public string UpdateBeneficiaryDetails(string scheduleId, string beneficiaryId)
        {
            if (string.IsNullOrWhiteSpace(scheduleId) || string.IsNullOrWhiteSpace(beneficiaryId)) 
                throw new ArgumentException("Invalid schedule or beneficiary ID.");
                
            // Logic to update database and return confirmation tracking number
            return $"CONF-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public string GetNextProcessingBatchId(DateTime processingDate)
        {
            return $"BATCH-ANNUITY-{processingDate:yyyyMMdd}";
        }
    }
}