using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    // Buggy stub — returns incorrect values
    public class CapitalGainsTaxService : ICapitalGainsTaxService
    {
        public decimal CalculateShortTermCapitalGains(string policyId, decimal maturityAmount, decimal totalPremiumsPaid)
        {
            return 0m;
        }

        public decimal CalculateLongTermCapitalGains(string policyId, decimal maturityAmount, decimal totalPremiumsPaid)
        {
            return 0m;
        }

        public bool IsExemptUnderSection10_10D(string policyId, decimal annualPremium, decimal sumAssured)
        {
            return false;
        }

        public double GetApplicableTaxRate(string customerId, string taxResidencyCode, DateTime maturityDate)
        {
            return 0.0;
        }

        public decimal CalculateGrandfatheredValue(string policyId, DateTime grandfatheringDate)
        {
            return 0m;
        }

        public int CalculateHoldingPeriodInDays(string policyId, DateTime issueDate, DateTime maturityDate)
        {
            return 0;
        }

        public bool IsHighPremiumUlip(string policyId, decimal aggregateAnnualPremium)
        {
            return false;
        }

        public decimal CalculateTaxableAmount(string policyId, decimal maturityAmount, decimal exemptAmount)
        {
            return 0m;
        }

        public decimal CalculateSurcharge(decimal computedTaxAmount, decimal totalIncome)
        {
            return 0m;
        }

        public decimal CalculateHealthAndEducationCess(decimal taxAmount, decimal surchargeAmount)
        {
            return 0m;
        }

        public string GetTaxCategoryCode(string policyId, DateTime issueDate)
        {
            return null;
        }

        public decimal GetTotalPremiumsPaid(string policyId, DateTime startDate, DateTime endDate)
        {
            return 0m;
        }

        public bool ValidatePanStatus(string panNumber, string customerId)
        {
            return false;
        }

        public double GetTdsRate(string panNumber, bool isNri)
        {
            return 0.0;
        }

        public decimal CalculateTdsAmount(decimal taxableAmount, double tdsRate)
        {
            return 0m;
        }

        public int GetFinancialYear(DateTime maturityDate)
        {
            return 0;
        }

        public string GenerateTaxComputationCertificateId(string policyId, DateTime computationDate)
        {
            return null;
        }

        public decimal CalculateIndexationBenefit(decimal purchaseCost, int yearOfPurchase, int yearOfSale)
        {
            return 0m;
        }

        public bool RequiresCapitalGainsReporting(string policyId, decimal taxableAmount)
        {
            return false;
        }

        public decimal CalculateNetPayableAfterTaxes(decimal maturityAmount, decimal totalTaxDeducted)
        {
            return 0m;
        }

        public int GetNumberOfFundSwitches(string policyId, DateTime startDate, DateTime endDate)
        {
            return 0;
        }

        public decimal CalculateCapitalLossCarriedForward(string customerId, int financialYear)
        {
            return 0m;
        }

        public bool IsEquityOrientedFund(string fundId, double equityPercentage)
        {
            return false;
        }

        public double GetSttRate(string fundId, DateTime transactionDate)
        {
            return 0.0;
        }

        public decimal CalculateSecuritiesTransactionTax(decimal transactionValue, double sttRate)
        {
            return 0m;
        }

        public string GetTaxResidencyStatus(string customerId, int financialYear)
        {
            return null;
        }
    }
}