using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    /// <summary>
    /// Calculates short-term and long-term capital gains on ULIP maturities.
    /// </summary>
    public interface ICapitalGainsTaxService
    {
        decimal CalculateShortTermCapitalGains(string policyId, decimal maturityAmount, decimal totalPremiumsPaid);
        decimal CalculateLongTermCapitalGains(string policyId, decimal maturityAmount, decimal totalPremiumsPaid);
        bool IsExemptUnderSection10_10D(string policyId, decimal annualPremium, decimal sumAssured);
        double GetApplicableTaxRate(string customerId, string taxResidencyCode, DateTime maturityDate);
        decimal CalculateGrandfatheredValue(string policyId, DateTime grandfatheringDate);
        int CalculateHoldingPeriodInDays(string policyId, DateTime issueDate, DateTime maturityDate);
        bool IsHighPremiumUlip(string policyId, decimal aggregateAnnualPremium);
        decimal CalculateTaxableAmount(string policyId, decimal maturityAmount, decimal exemptAmount);
        decimal CalculateSurcharge(decimal computedTaxAmount, decimal totalIncome);
        decimal CalculateHealthAndEducationCess(decimal taxAmount, decimal surchargeAmount);
        string GetTaxCategoryCode(string policyId, DateTime issueDate);
        decimal GetTotalPremiumsPaid(string policyId, DateTime startDate, DateTime endDate);
        bool ValidatePanStatus(string panNumber, string customerId);
        double GetTdsRate(string panNumber, bool isNri);
        decimal CalculateTdsAmount(decimal taxableAmount, double tdsRate);
        int GetFinancialYear(DateTime maturityDate);
        string GenerateTaxComputationCertificateId(string policyId, DateTime computationDate);
        decimal CalculateIndexationBenefit(decimal purchaseCost, int yearOfPurchase, int yearOfSale);
        bool RequiresCapitalGainsReporting(string policyId, decimal taxableAmount);
        decimal CalculateNetPayableAfterTaxes(decimal maturityAmount, decimal totalTaxDeducted);
        int GetNumberOfFundSwitches(string policyId, DateTime startDate, DateTime endDate);
        decimal CalculateCapitalLossCarriedForward(string customerId, int financialYear);
        bool IsEquityOrientedFund(string fundId, double equityPercentage);
        double GetSttRate(string fundId, DateTime transactionDate);
        decimal CalculateSecuritiesTransactionTax(decimal transactionValue, double sttRate);
        string GetTaxResidencyStatus(string customerId, int financialYear);
    }
}