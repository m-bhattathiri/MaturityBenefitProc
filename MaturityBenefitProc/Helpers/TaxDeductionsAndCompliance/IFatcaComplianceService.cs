using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    /// <summary>Ensures FATCA/CRS declaration compliance before high-value disbursements.</summary>
    public interface IFatcaComplianceService
    {
        bool ValidateFatcaStatus(string policyNumber, string customerId);

        bool IsCrsDeclarationRequired(string customerId, decimal disbursementAmount);

        string GetTaxResidencyCountryCode(string customerId);

        decimal CalculateWithholdingTax(decimal grossAmount, double withholdingRate);

        double GetApplicableWithholdingRate(string countryCode, bool hasValidW8Ben);

        int GetDaysUntilDeclarationExpiry(string customerId, DateTime currentDate);

        bool VerifyTinFormat(string taxIdentificationNumber, string countryCode);

        string GenerateComplianceReportId(string customerId, DateTime disbursementDate);

        int CountMissingComplianceDocuments(string customerId);

        decimal GetThresholdAmountForReporting(string countryCode);

        bool CheckHighValueDisbursementEligibility(string policyNumber, decimal disbursementAmount);

        string RetrieveFatcaClassificationCode(string customerId);

        double CalculateCrsRiskScore(string customerId, int numberOfForeignAccounts);

        bool HasActiveIndicia(string customerId);

        int GetGracePeriodDays(string customerId, string classificationCode);

        decimal ComputeNetDisbursementAmount(decimal grossAmount, decimal totalWithheld);

        string SubmitCrsReport(string customerId, decimal reportableAmount, DateTime reportingPeriod);
    }
}