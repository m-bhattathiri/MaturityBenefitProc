using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.InternationalAndNriProcessing
{
    /// <summary>Applies special DTAA and withholding tax rates for NRI customers.</summary>
    public interface INriTaxationService
    {
        decimal CalculateWithholdingTax(string policyId, decimal maturityAmount, string countryCode);
        
        double GetDtaaRate(string countryCode, DateTime effectiveDate);
        
        bool IsEligibleForDtaaBenefits(string customerId, string countryCode);
        
        string GetTaxResidencyCertificateStatus(string customerId);
        
        int GetDaysPresentInCountry(string customerId, DateTime financialYearStart, DateTime financialYearEnd);
        
        decimal ApplySurchargeAndCess(decimal baseTaxAmount, double surchargeRate, double cessRate);
        
        bool ValidatePanStatus(string panNumber);
        
        string GetFatcaDeclarationId(string customerId);
        
        decimal CalculateNetMaturityAmount(decimal grossAmount, decimal totalTaxDeducted);
        
        double GetEffectiveTdsRate(string customerId, decimal maturityAmount);
        
        int GetRemainingValidityDaysForTrc(string trcDocumentId);
        
        bool CheckForm10FSubmission(string customerId, DateTime financialYear);
        
        decimal ComputeExchangeRateVariance(decimal baseAmount, double exchangeRate);
        
        string ResolveTaxTreatyCode(string countryCode);
        
        bool IsNriStatusConfirmed(string customerId, DateTime evaluationDate);
        
        double GetMaximumMarginalReliefRate(decimal incomeAmount);
        
        decimal CalculateCapitalGainsTax(string policyId, decimal gainAmount, int holdingPeriodDays);
        
        int GetHoldingPeriod(DateTime issueDate, DateTime maturityDate);
        
        string GenerateTdsCertificateNumber(string customerId, string policyId);
        
        bool VerifyOciCardValidity(string ociCardNumber);
        
        decimal GetRepatriableAmount(string policyId, decimal netAmount);
        
        double GetNroAccountTdsRate(string bankCode);
        
        int CountPreviousTaxFilings(string customerId);
        
        string DetermineResidentialStatus(int daysInIndia, int daysInPrecedingYears);
    }
}