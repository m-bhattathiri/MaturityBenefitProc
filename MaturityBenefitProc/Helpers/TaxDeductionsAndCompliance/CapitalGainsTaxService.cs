using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    // Fixed implementation — correct business logic
    public class CapitalGainsTaxService : ICapitalGainsTaxService
    {
        private const decimal ULIP_HIGH_PREMIUM_THRESHOLD = 250000m;
        private const decimal HEALTH_AND_EDUCATION_CESS_RATE = 0.04m;
        private const double EQUITY_FUND_THRESHOLD = 65.0;

        public decimal CalculateShortTermCapitalGains(string policyId, decimal maturityAmount, decimal totalPremiumsPaid)
        {
            if (maturityAmount < 0 || totalPremiumsPaid < 0) throw new ArgumentException("Amounts cannot be negative.");
            decimal gains = maturityAmount - totalPremiumsPaid;
            return gains > 0 ? gains : 0m;
        }

        public decimal CalculateLongTermCapitalGains(string policyId, decimal maturityAmount, decimal totalPremiumsPaid)
        {
            if (maturityAmount < 0 || totalPremiumsPaid < 0) throw new ArgumentException("Amounts cannot be negative.");
            // Assuming indexation is applied elsewhere or not applicable for equity-oriented ULIPs
            decimal gains = maturityAmount - totalPremiumsPaid;
            return gains > 0 ? gains : 0m;
        }

        public bool IsExemptUnderSection10_10D(string policyId, decimal annualPremium, decimal sumAssured)
        {
            if (sumAssured <= 0) return false;
            // Exemption typically applies if premium is <= 10% of sum assured
            return annualPremium <= (sumAssured * 0.10m);
        }

        public double GetApplicableTaxRate(string customerId, string taxResidencyCode, DateTime maturityDate)
        {
            // Simplified tax rate logic
            if (taxResidencyCode == "NRI") return 0.15; // 15% for NRI
            return 0.10; // 10% standard LTCG for equity-oriented funds over 1 Lakh
        }

        public decimal CalculateGrandfatheredValue(string policyId, DateTime grandfatheringDate)
        {
            // Mocking a grandfathered value retrieval
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            return 100000m; // Placeholder for actual DB lookup
        }

        public int CalculateHoldingPeriodInDays(string policyId, DateTime issueDate, DateTime maturityDate)
        {
            if (maturityDate < issueDate) throw new ArgumentException("Maturity date cannot be before issue date.");
            return (maturityDate - issueDate).Days;
        }

        public bool IsHighPremiumUlip(string policyId, decimal aggregateAnnualPremium)
        {
            // For ULIPs issued on or after Feb 1, 2021
            return aggregateAnnualPremium > ULIP_HIGH_PREMIUM_THRESHOLD;
        }

        public decimal CalculateTaxableAmount(string policyId, decimal maturityAmount, decimal exemptAmount)
        {
            decimal taxable = maturityAmount - exemptAmount;
            return taxable > 0 ? taxable : 0m;
        }

        public decimal CalculateSurcharge(decimal computedTaxAmount, decimal totalIncome)
        {
            if (totalIncome > 50000000m) return computedTaxAmount * 0.37m; // 37% for > 5Cr
            if (totalIncome > 20000000m) return computedTaxAmount * 0.25m; // 25% for > 2Cr
            if (totalIncome > 10000000m) return computedTaxAmount * 0.15m; // 15% for > 1Cr
            if (totalIncome > 5000000m) return computedTaxAmount * 0.10m;  // 10% for > 50L
            return 0m;
        }

        public decimal CalculateHealthAndEducationCess(decimal taxAmount, decimal surchargeAmount)
        {
            return (taxAmount + surchargeAmount) * HEALTH_AND_EDUCATION_CESS_RATE;
        }

        public string GetTaxCategoryCode(string policyId, DateTime issueDate)
        {
            if (issueDate >= new DateTime(2021, 2, 1)) return "ULIP_POST_2021";
            return "ULIP_PRE_2021";
        }

        public decimal GetTotalPremiumsPaid(string policyId, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Start date must be before end date.");
            return 500000m; // Placeholder for DB aggregation
        }

        public bool ValidatePanStatus(string panNumber, string customerId)
        {
            if (string.IsNullOrWhiteSpace(panNumber) || panNumber.Length != 10) return false;
            // Basic PAN format validation
            foreach (char c in panNumber.Substring(0, 5)) if (!char.IsLetter(c)) return false;
            foreach (char c in panNumber.Substring(5, 4)) if (!char.IsDigit(c)) return false;
            if (!char.IsLetter(panNumber[9])) return false;
            return true;
        }

        public double GetTdsRate(string panNumber, bool isNri)
        {
            if (isNri) return 0.312; // 30% + cess for NRI
            bool hasValidPan = ValidatePanStatus(panNumber, "SYSTEM");
            return hasValidPan ? 0.05 : 0.20; // 5% with PAN, 20% without PAN
        }

        public decimal CalculateTdsAmount(decimal taxableAmount, double tdsRate)
        {
            if (taxableAmount <= 0) return 0m;
            return Math.Round(taxableAmount * (decimal)tdsRate, 2);
        }

        public int GetFinancialYear(DateTime maturityDate)
        {
            return maturityDate.Month >= 4 ? maturityDate.Year : maturityDate.Year - 1;
        }

        public string GenerateTaxComputationCertificateId(string policyId, DateTime computationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            return $"TCC-{policyId}-{computationDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        }

        public decimal CalculateIndexationBenefit(decimal purchaseCost, int yearOfPurchase, int yearOfSale)
        {
            if (yearOfSale < yearOfPurchase) throw new ArgumentException("Sale year cannot be before purchase year.");
            // Mock Cost Inflation Index (CII) values
            decimal ciiPurchase = 100m; // Mock value
            decimal ciiSale = 150m;     // Mock value
            return purchaseCost * (ciiSale / ciiPurchase);
        }

        public bool RequiresCapitalGainsReporting(string policyId, decimal taxableAmount)
        {
            return taxableAmount > 250000m; // Basic exemption limit
        }

        public decimal CalculateNetPayableAfterTaxes(decimal maturityAmount, decimal totalTaxDeducted)
        {
            return Math.Max(0m, maturityAmount - totalTaxDeducted);
        }

        public int GetNumberOfFundSwitches(string policyId, DateTime startDate, DateTime endDate)
        {
            return 2; // Placeholder for actual transaction count
        }

        public decimal CalculateCapitalLossCarriedForward(string customerId, int financialYear)
        {
            return 0m; // Placeholder for DB lookup
        }

        public bool IsEquityOrientedFund(string fundId, double equityPercentage)
        {
            return equityPercentage >= EQUITY_FUND_THRESHOLD;
        }

        public double GetSttRate(string fundId, DateTime transactionDate)
        {
            return 0.001; // 0.1% STT
        }

        public decimal CalculateSecuritiesTransactionTax(decimal transactionValue, double sttRate)
        {
            if (transactionValue <= 0) return 0m;
            return Math.Round(transactionValue * (decimal)sttRate, 2);
        }

        public string GetTaxResidencyStatus(string customerId, int financialYear)
        {
            if (string.IsNullOrWhiteSpace(customerId)) throw new ArgumentNullException(nameof(customerId));
            return "RESIDENT"; // Defaulting to resident for placeholder
        }
    }
}