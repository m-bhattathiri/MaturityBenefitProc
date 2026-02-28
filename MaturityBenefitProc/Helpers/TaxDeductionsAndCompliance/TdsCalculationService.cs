using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    // Fixed implementation — correct business logic
    public class TdsCalculationService : ITdsCalculationService
    {
        private const decimal DefaultThreshold194DA = 100000m;
        private const double StandardTdsRate = 0.05; // 5% under 194DA
        private const double HigherTdsRate = 0.20; // 20% if PAN not provided/invalid
        private const double CessRate = 0.04; // 4% Health and Education Cess

        public decimal CalculateBaseTds(string policyId, decimal grossAmount)
        {
            if (grossAmount <= 0) return 0m;
            return grossAmount * (decimal)StandardTdsRate;
        }

        public double GetApplicableTdsRate(string panNumber, DateTime payoutDate)
        {
            if (!IsPanValid(panNumber) || RequiresHigherTdsRate(panNumber))
                return HigherTdsRate;
            return StandardTdsRate;
        }

        public bool IsTdsApplicable(decimal grossAmount, string sectionCode)
        {
            decimal threshold = GetThresholdLimit(sectionCode, DateTime.Now.Year);
            return grossAmount >= threshold;
        }

        public int GetFinancialYear(DateTime transactionDate)
        {
            return transactionDate.Month >= 4 ? transactionDate.Year : transactionDate.Year - 1;
        }

        public string GetTaxCategoryCode(string customerId)
        {
            return string.IsNullOrEmpty(customerId) ? "UNKNOWN" : (IsNriCustomer(customerId) ? "NRI" : "IND");
        }

        public decimal ComputeSurcharge(decimal baseTds, double surchargeRate)
        {
            if (baseTds <= 0 || surchargeRate <= 0) return 0m;
            return baseTds * (decimal)surchargeRate;
        }

        public decimal ComputeEducationCess(decimal taxAmount)
        {
            if (taxAmount <= 0) return 0m;
            return taxAmount * (decimal)CessRate;
        }

        public decimal GetTotalTaxableAmount(string policyId, decimal totalPayout)
        {
            // Assuming 20% of payout is taxable as a placeholder logic
            return totalPayout > 0 ? totalPayout * 0.20m : 0m;
        }

        public decimal CalculateNetPayout(decimal grossAmount, decimal totalTds)
        {
            return Math.Max(0m, grossAmount - totalTds);
        }

        public decimal GetExemptAmount(string policyId, decimal premiumsPaid)
        {
            return premiumsPaid; // Assuming premiums paid are exempt
        }

        public decimal CalculateTdsOnSurrender(string policyId, decimal surrenderValue, int policyDurationMonths)
        {
            if (policyDurationMonths >= 60) return 0m; // Exempt if > 5 years
            return CalculateBaseTds(policyId, surrenderValue);
        }

        public decimal CalculateTdsOnMaturity(string policyId, decimal maturityValue)
        {
            if (maturityValue < DefaultThreshold194DA) return 0m;
            return CalculateBaseTds(policyId, maturityValue);
        }

        public decimal GetThresholdLimit(string sectionCode, int financialYear)
        {
            return sectionCode == "194DA" ? DefaultThreshold194DA : 0m;
        }

        public decimal ComputePenaltyAmount(string panNumber, int daysDelayed)
        {
            if (daysDelayed <= 0) return 0m;
            return daysDelayed * 200m; // 200 per day penalty under 234E
        }

        public double GetEffectiveTaxRate(double baseRate, double surchargeRate, double cessRate)
        {
            double taxWithSurcharge = baseRate * (1 + surchargeRate);
            return taxWithSurcharge * (1 + cessRate);
        }

        public double GetSurchargeRate(decimal totalIncome, string assesseeType)
        {
            if (totalIncome > 50000000m) return 0.37;
            if (totalIncome > 20000000m) return 0.25;
            if (totalIncome > 10000000m) return 0.15;
            if (totalIncome > 5000000m) return 0.10;
            return 0.0;
        }

        public double ComputeCessPercentage(int financialYear)
        {
            return CessRate;
        }

        public double GetMarginalReliefRatio(decimal totalIncome, decimal taxCalculated)
        {
            return 0.0; // Simplified
        }

        public double GetDtaaBenefitRate(string countryCode)
        {
            return countryCode == "US" ? 0.10 : 0.15; // Example rates
        }

        public bool IsPanValid(string panNumber)
        {
            if (string.IsNullOrWhiteSpace(panNumber)) return false;
            return Regex.IsMatch(panNumber, @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$");
        }

        public bool IsNriCustomer(string customerId)
        {
            return customerId?.StartsWith("NRI") ?? false;
        }

        public bool HasForm15GOrH(string customerId, int financialYear)
        {
            return false; // Typically requires DB lookup
        }

        public bool IsAmountAboveThreshold(decimal amount, string sectionCode)
        {
            return amount >= GetThresholdLimit(sectionCode, DateTime.Now.Year);
        }

        public bool IsAadhaarLinked(string panNumber)
        {
            return true; // Assume linked for this implementation
        }

        public bool IsExemptUnderSection10_10D(string policyId, decimal sumAssured, decimal annualPremium)
        {
            if (annualPremium <= 0) return false;
            return (sumAssured / annualPremium) >= 10m; // Premium should not exceed 10% of sum assured
        }

        public bool RequiresHigherTdsRate(string panNumber)
        {
            return !IsPanValid(panNumber) || !IsAadhaarLinked(panNumber);
        }

        public int CalculateDaysDelayed(DateTime expectedDate, DateTime actualDate)
        {
            if (actualDate <= expectedDate) return 0;
            return (actualDate - expectedDate).Days;
        }

        public int GetTotalTransactionsInYear(string panNumber, int financialYear)
        {
            return 1; // Placeholder
        }

        public int GetAgeAtPayout(string customerId, DateTime payoutDate)
        {
            return 35; // Placeholder
        }

        public int GetPolicyDurationInMonths(DateTime issueDate, DateTime surrenderDate)
        {
            if (surrenderDate <= issueDate) return 0;
            return ((surrenderDate.Year - issueDate.Year) * 12) + surrenderDate.Month - issueDate.Month;
        }

        public string GetChallanNumber(string transactionId)
        {
            return $"CHL-{transactionId}-{DateTime.Now.Ticks}";
        }

        public string GenerateTdsCertificateId(string panNumber, int financialYear)
        {
            return $"TDS-{financialYear}-{panNumber}";
        }

        public string GetSectionCode(string payoutType)
        {
            return payoutType?.ToLower() == "maturity" ? "194DA" : "194";
        }

        public string GetAssesseeType(string customerId)
        {
            return "Individual";
        }

        public decimal CalculateTdsForNri(string policyId, decimal grossAmount, string countryCode)
        {
            double dtaaRate = GetDtaaBenefitRate(countryCode);
            return grossAmount * (decimal)dtaaRate;
        }
    }
}