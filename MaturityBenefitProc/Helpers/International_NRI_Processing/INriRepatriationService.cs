using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.International_NRI_Processing
{
    /// <summary>
    /// Evaluates repatriation limits and rules for Non-Resident Indian policies.
    /// </summary>
    public interface INriRepatriationService
    {
        bool ValidateRepatriationEligibility(string policyId, string customerId);
        decimal CalculateMaximumRepatriationAmount(string policyId, DateTime evaluationDate);
        double GetCurrentFemaRepatriationLimitPercentage(string policyId);
        int GetDaysSinceLastRepatriation(string customerId);
        string GetNroAccountStatus(string accountId);
        decimal CalculateTaxDeductionAtSource(decimal repatriationAmount, string taxResidencyCode);
        bool IsFemaComplianceMet(string customerId, decimal requestedAmount);
        string GenerateForm15CbRequirementCode(decimal amount);
        int GetRemainingRepatriationsForFinancialYear(string customerId, DateTime currentFinancialYearStart);
        decimal CalculateExchangeRateVariance(string currencyCode, decimal baseAmount);
        double GetDoubleTaxationAvoidanceAgreementRate(string countryCode);
        bool VerifyNreAccountFundingSource(string policyId, string accountId);
        string GetReserveBankOfIndiaApprovalCode(string policyId, decimal amount);
        decimal ComputeAllowableMaturityProceeds(string policyId, decimal totalMaturityValue);
        int CalculateDaysToMaturity(string policyId, DateTime currentDate);
        bool CheckFatcaComplianceStatus(string customerId);
        decimal CalculatePremiumPaidInForeignCurrency(string policyId);
        double GetProportionateRepatriationRatio(string policyId);
        string RetrieveAuthorizedDealerBankCode(string customerId);
        bool ValidateCaCertificateRequirement(decimal requestedAmount);
        decimal CalculateNetRepatriableAmount(decimal grossAmount, decimal tdsAmount, decimal fees);
        int GetPendingDocumentCount(string policyId);
        string GetRepatriationRejectionReasonCode(string policyId, decimal requestedAmount);
        bool IsCountryInRestrictedList(string countryCode);
        decimal GetTotalRepatriatedAmountYearToDate(string customerId, DateTime financialYearStart);
    }
}