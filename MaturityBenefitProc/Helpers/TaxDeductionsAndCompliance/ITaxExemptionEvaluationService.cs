using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    /// <summary>Evaluates policies against section 10(10D) and other tax exemption rules.</summary>
    public interface ITaxExemptionEvaluationService
    {
        bool IsEligibleForSection1010D(string policyId, DateTime issueDate);
        
        decimal CalculateTaxableMaturityAmount(string policyId, decimal totalPremiumsPaid, decimal maturityAmount);
        
        double GetApplicableTdsRate(string panNumber, bool isPanValid);
        
        decimal CalculateTdsAmount(decimal taxableAmount, double tdsRate);
        
        int GetPolicyTermInYears(string policyId);
        
        bool ValidatePremiumToSumAssuredRatio(decimal annualPremium, decimal sumAssured, DateTime issueDate);
        
        string GetExemptionRejectionReasonCode(string policyId);
        
        decimal GetTotalPremiumsPaid(string policyId, DateTime startDate, DateTime endDate);
        
        double CalculatePremiumToSumAssuredPercentage(decimal annualPremium, decimal sumAssured);
        
        int GetDaysUntilTaxFilingDeadline(DateTime currentProcessDate);
        
        bool CheckIfPolicyIsUlip(string policyId);
        
        decimal CalculateUlipExemptionLimit(decimal aggregatePremium, DateTime financialYearStart);
        
        string RetrieveCustomerPanStatus(string customerId);
        
        bool IsDeathBenefitExempt(string policyId, string causeOfDeathCode);
        
        decimal ComputeNetPayableAfterTaxes(decimal grossAmount, decimal tdsAmount, decimal surcharge);
        
        int CountPoliciesExceedingPremiumLimit(string customerId, decimal premiumLimit);
        
        double GetSurchargeRate(decimal totalTaxableIncome);
        
        bool EvaluateNriTaxCompliance(string customerId, string countryCode);
    }
}