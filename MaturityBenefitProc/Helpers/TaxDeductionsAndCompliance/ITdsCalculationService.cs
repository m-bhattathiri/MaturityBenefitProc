using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    /// <summary>Computes Tax Deducted at Source (TDS) for maturity and surrender payouts.</summary>
    public interface ITdsCalculationService
    {
        decimal CalculateBaseTds(string policyId, decimal grossAmount);
        double GetApplicableTdsRate(string panNumber, DateTime payoutDate);
        bool IsTdsApplicable(decimal grossAmount, string sectionCode);
        int GetFinancialYear(DateTime transactionDate);
        string GetTaxCategoryCode(string customerId);
        
        decimal ComputeSurcharge(decimal baseTds, double surchargeRate);
        decimal ComputeEducationCess(decimal taxAmount);
        decimal GetTotalTaxableAmount(string policyId, decimal totalPayout);
        decimal CalculateNetPayout(decimal grossAmount, decimal totalTds);
        decimal GetExemptAmount(string policyId, decimal premiumsPaid);
        
        decimal CalculateTdsOnSurrender(string policyId, decimal surrenderValue, int policyDurationMonths);
        decimal CalculateTdsOnMaturity(string policyId, decimal maturityValue);
        decimal GetThresholdLimit(string sectionCode, int financialYear);
        decimal ComputePenaltyAmount(string panNumber, int daysDelayed);
        
        double GetEffectiveTaxRate(double baseRate, double surchargeRate, double cessRate);
        double GetSurchargeRate(decimal totalIncome, string assesseeType);
        double ComputeCessPercentage(int financialYear);
        double GetMarginalReliefRatio(decimal totalIncome, decimal taxCalculated);
        double GetDtaaBenefitRate(string countryCode);
        
        bool IsPanValid(string panNumber);
        bool IsNriCustomer(string customerId);
        bool HasForm15GOrH(string customerId, int financialYear);
        bool IsAmountAboveThreshold(decimal amount, string sectionCode);
        bool IsAadhaarLinked(string panNumber);
        bool IsExemptUnderSection10_10D(string policyId, decimal sumAssured, decimal annualPremium);
        bool RequiresHigherTdsRate(string panNumber);
        
        int CalculateDaysDelayed(DateTime expectedDate, DateTime actualDate);
        int GetTotalTransactionsInYear(string panNumber, int financialYear);
        int GetAgeAtPayout(string customerId, DateTime payoutDate);
        int GetPolicyDurationInMonths(DateTime issueDate, DateTime surrenderDate);
        
        string GetChallanNumber(string transactionId);
        string GenerateTdsCertificateId(string panNumber, int financialYear);
        string GetSectionCode(string payoutType);
        string GetAssesseeType(string customerId);
        
        decimal CalculateTdsForNri(string policyId, decimal grossAmount, string countryCode);
    }
}