using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    // Fixed implementation — correct business logic
    public class SurrenderValueCalculationService : ISurrenderValueCalculationService
    {
        private const int MINIMUM_YEARS_FOR_SURRENDER = 3;
        private const decimal SURRENDER_CHARGE_RATE = 0.05m;
        private const int COOLING_OFF_DAYS = 15;

        // Mock data retrieval methods to simulate database/repository access
        private DateTime GetPolicyIssueDate(string policyId) => new DateTime(2015, 1, 1);
        private string GetPlanCode(string policyId) => "ENDOWMENT_01";
        private decimal GetAnnualPremium(string policyId) => 12000m;
        private decimal GetSumAssured(string policyId) => 250000m;
        private int GetTotalPolicyTerm(string policyId) => 20;

        public decimal CalculateGuaranteedSurrenderValue(string policyId, DateTime surrenderDate)
        {
            if (string.IsNullOrEmpty(policyId)) throw new ArgumentNullException(nameof(policyId));

            int completedYears = GetCompletedPolicyYears(policyId, surrenderDate);
            if (completedYears < MINIMUM_YEARS_FOR_SURRENDER) return 0m;

            string planCode = GetPlanCode(policyId);
            decimal totalPremiums = CalculateTotalPaidPremiums(policyId, surrenderDate);
            double factor = GetSurrenderValueFactor(completedYears, planCode);
            
            decimal vestedBonus = CalculateVestedReversionaryBonus(policyId, surrenderDate);
            double bonusFactor = GetSurrenderValueFactor(completedYears, planCode) * 0.8; // Bonus factor is typically lower

            return (totalPremiums * (decimal)factor) + (vestedBonus * (decimal)bonusFactor);
        }

        public decimal CalculateSpecialSurrenderValue(string policyId, DateTime surrenderDate)
        {
            if (string.IsNullOrEmpty(policyId)) throw new ArgumentNullException(nameof(policyId));

            int completedYears = GetCompletedPolicyYears(policyId, surrenderDate);
            if (completedYears < MINIMUM_YEARS_FOR_SURRENDER) return 0m;

            decimal paidUpValue = CalculatePaidUpValue(policyId, surrenderDate);
            string planCode = GetPlanCode(policyId);
            double ssvFactor = GetSpecialSurrenderValueFactor(completedYears, planCode);

            return paidUpValue * (decimal)ssvFactor;
        }

        public bool IsPolicyEligibleForSurrender(string policyId, DateTime requestDate)
        {
            if (string.IsNullOrEmpty(policyId)) return false;
            
            int completedYears = GetCompletedPolicyYears(policyId, requestDate);
            bool hasActiveAssignments = HasActiveAssignments(policyId);
            
            return completedYears >= MINIMUM_YEARS_FOR_SURRENDER && !hasActiveAssignments;
        }

        public int GetCompletedPolicyYears(string policyId, DateTime surrenderDate)
        {
            DateTime issueDate = GetPolicyIssueDate(policyId);
            if (surrenderDate < issueDate) return 0;

            int years = surrenderDate.Year - issueDate.Year;
            if (surrenderDate.Month < issueDate.Month || (surrenderDate.Month == issueDate.Month && surrenderDate.Day < issueDate.Day))
            {
                years--;
            }
            return Math.Max(0, years);
        }

        public double GetSurrenderValueFactor(int completedYears, string planCode)
        {
            if (completedYears < 3) return 0.0;
            if (completedYears >= 3 && completedYears <= 5) return 0.30;
            if (completedYears > 5 && completedYears <= 10) return 0.50;
            return 0.70; // > 10 years
        }

        public decimal CalculateAccruedBonuses(string policyId, DateTime surrenderDate)
        {
            int completedYears = GetCompletedPolicyYears(policyId, surrenderDate);
            decimal sumAssured = GetSumAssured(policyId);
            // Mocking a 4% annual bonus on sum assured
            return sumAssured * 0.04m * completedYears;
        }

        public decimal CalculateTerminalBonus(string policyId, DateTime surrenderDate)
        {
            int completedYears = GetCompletedPolicyYears(policyId, surrenderDate);
            if (completedYears < 10) return 0m; // Terminal bonus only after 10 years
            
            decimal sumAssured = GetSumAssured(policyId);
            return sumAssured * 0.02m * completedYears;
        }

        public double GetSpecialSurrenderValueFactor(int completedYears, string planCode)
        {
            // Usually depends on remaining term, simplified here
            if (completedYears < 3) return 0.0;
            if (completedYears >= 3 && completedYears <= 7) return 0.45;
            if (completedYears > 7 && completedYears <= 15) return 0.65;
            return 0.85; 
        }

        public decimal CalculateTotalPaidPremiums(string policyId, DateTime surrenderDate)
        {
            int completedYears = GetCompletedPolicyYears(policyId, surrenderDate);
            decimal annualPremium = GetAnnualPremium(policyId);
            return annualPremium * completedYears;
        }

        public bool ValidateSurrenderRequest(string policyId, string customerId, DateTime requestDate)
        {
            if (string.IsNullOrEmpty(policyId) || string.IsNullOrEmpty(customerId)) return false;
            if (requestDate > DateTime.Now) return false;
            
            return IsPolicyEligibleForSurrender(policyId, requestDate);
        }

        public int GetDaysSinceLastPremiumPaid(string policyId, DateTime surrenderDate)
        {
            // Mocking logic: assuming premium is paid on the anniversary date
            DateTime issueDate = GetPolicyIssueDate(policyId);
            DateTime lastAnniversary = new DateTime(surrenderDate.Year, issueDate.Month, issueDate.Day);
            if (lastAnniversary > surrenderDate)
            {
                lastAnniversary = lastAnniversary.AddYears(-1);
            }
            return (surrenderDate - lastAnniversary).Days;
        }

        public decimal CalculateLoanOutstanding(string policyId, DateTime surrenderDate)
        {
            // Mock database lookup
            return 5000m; 
        }

        public decimal CalculateLoanInterestOutstanding(string policyId, DateTime surrenderDate)
        {
            decimal loanAmount = CalculateLoanOutstanding(policyId, surrenderDate);
            // Mocking 9% annual interest for 6 months
            return loanAmount * 0.09m * 0.5m;
        }

        public decimal CalculateNetSurrenderValue(string policyId, DateTime surrenderDate)
        {
            decimal gsv = CalculateGuaranteedSurrenderValue(policyId, surrenderDate);
            decimal ssv = CalculateSpecialSurrenderValue(policyId, surrenderDate);
            
            decimal grossSurrenderValue = Math.Max(gsv, ssv);
            if (grossSurrenderValue == 0m) return 0m;

            decimal loan = CalculateLoanOutstanding(policyId, surrenderDate);
            decimal interest = CalculateLoanInterestOutstanding(policyId, surrenderDate);
            decimal charges = CalculateSurrenderCharges(policyId, surrenderDate);

            decimal netValue = grossSurrenderValue - loan - interest - charges;
            return Math.Max(0m, netValue);
        }

        public string GetSurrenderStatus(string policyId)
        {
            if (string.IsNullOrEmpty(policyId)) return "Invalid";
            return IsPolicyEligibleForSurrender(policyId, DateTime.Now) ? "Eligible" : "Ineligible";
        }

        public double GetPaidUpValueRatio(string policyId, DateTime surrenderDate)
        {
            int completedYears = GetCompletedPolicyYears(policyId, surrenderDate);
            int totalTerm = GetTotalPolicyTerm(policyId);
            
            if (totalTerm == 0) return 0.0;
            return (double)completedYears / totalTerm;
        }

        public decimal CalculatePaidUpValue(string policyId, DateTime surrenderDate)
        {
            decimal sumAssured = GetSumAssured(policyId);
            double ratio = GetPaidUpValueRatio(policyId, surrenderDate);
            decimal vestedBonus = CalculateVestedReversionaryBonus(policyId, surrenderDate);
            
            return (sumAssured * (decimal)ratio) + vestedBonus;
        }

        public bool HasActiveAssignments(string policyId)
        {
            // Mock database check
            return false;
        }

        public int GetRemainingPolicyTerm(string policyId, DateTime surrenderDate)
        {
            int completedYears = GetCompletedPolicyYears(policyId, surrenderDate);
            int totalTerm = GetTotalPolicyTerm(policyId);
            return Math.Max(0, totalTerm - completedYears);
        }

        public decimal CalculateSurrenderCharges(string policyId, DateTime surrenderDate)
        {
            int completedYears = GetCompletedPolicyYears(policyId, surrenderDate);
            if (completedYears >= 10) return 0m; // No charges after 10 years

            decimal totalPremiums = CalculateTotalPaidPremiums(policyId, surrenderDate);
            return totalPremiums * SURRENDER_CHARGE_RATE;
        }

        public string GenerateSurrenderQuoteId(string policyId, DateTime requestDate)
        {
            return $"SQ-{policyId}-{requestDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        }

        public bool IsWithinCoolingOffPeriod(string policyId, DateTime requestDate)
        {
            DateTime issueDate = GetPolicyIssueDate(policyId);
            return (requestDate - issueDate).TotalDays <= COOLING_OFF_DAYS;
        }

        public decimal CalculateVestedReversionaryBonus(string policyId, DateTime surrenderDate)
        {
            return CalculateAccruedBonuses(policyId, surrenderDate);
        }

        public double GetDiscountRate(string planCode, DateTime surrenderDate)
        {
            // Mock discount rate based on current economic factors
            return 0.06; // 6%
        }

        public decimal CalculateDiscountedValue(decimal futureValue, double discountRate, int remainingYears)
        {
            if (remainingYears <= 0) return futureValue;
            if (discountRate < 0) throw new ArgumentException("Discount rate cannot be negative.");

            double discountFactor = Math.Pow(1 + discountRate, remainingYears);
            return futureValue / (decimal)discountFactor;
        }
    }
}