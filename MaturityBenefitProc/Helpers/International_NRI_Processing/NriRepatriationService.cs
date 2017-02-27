using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.International_NRI_Processing
{
    // Buggy stub — returns incorrect values
    public class NriRepatriationService : INriRepatriationService
    {
        public bool ValidateRepatriationEligibility(string policyId, string customerId)
        {
            return false;
        }

        public decimal CalculateMaximumRepatriationAmount(string policyId, DateTime evaluationDate)
        {
            return 0m;
        }

        public double GetCurrentFemaRepatriationLimitPercentage(string policyId)
        {
            return 0.0;
        }

        public int GetDaysSinceLastRepatriation(string customerId)
        {
            return 0;
        }

        public string GetNroAccountStatus(string accountId)
        {
            return null;
        }

        public decimal CalculateTaxDeductionAtSource(decimal repatriationAmount, string taxResidencyCode)
        {
            return 0m;
        }

        public bool IsFemaComplianceMet(string customerId, decimal requestedAmount)
        {
            return false;
        }

        public string GenerateForm15CbRequirementCode(decimal amount)
        {
            return null;
        }

        public int GetRemainingRepatriationsForFinancialYear(string customerId, DateTime currentFinancialYearStart)
        {
            return 0;
        }

        public decimal CalculateExchangeRateVariance(string currencyCode, decimal baseAmount)
        {
            return 0m;
        }

        public double GetDoubleTaxationAvoidanceAgreementRate(string countryCode)
        {
            return 0.0;
        }

        public bool VerifyNreAccountFundingSource(string policyId, string accountId)
        {
            return false;
        }

        public string GetReserveBankOfIndiaApprovalCode(string policyId, decimal amount)
        {
            return null;
        }

        public decimal ComputeAllowableMaturityProceeds(string policyId, decimal totalMaturityValue)
        {
            return 0m;
        }

        public int CalculateDaysToMaturity(string policyId, DateTime currentDate)
        {
            return 0;
        }

        public bool CheckFatcaComplianceStatus(string customerId)
        {
            return false;
        }

        public decimal CalculatePremiumPaidInForeignCurrency(string policyId)
        {
            return 0m;
        }

        public double GetProportionateRepatriationRatio(string policyId)
        {
            return 0.0;
        }

        public string RetrieveAuthorizedDealerBankCode(string customerId)
        {
            return null;
        }

        public bool ValidateCaCertificateRequirement(decimal requestedAmount)
        {
            return false;
        }

        public decimal CalculateNetRepatriableAmount(decimal grossAmount, decimal tdsAmount, decimal fees)
        {
            return 0m;
        }

        public int GetPendingDocumentCount(string policyId)
        {
            return 0;
        }

        public string GetRepatriationRejectionReasonCode(string policyId, decimal requestedAmount)
        {
            return null;
        }

        public bool IsCountryInRestrictedList(string countryCode)
        {
            return false;
        }

        public decimal GetTotalRepatriatedAmountYearToDate(string customerId, DateTime financialYearStart)
        {
            return 0m;
        }
    }
}