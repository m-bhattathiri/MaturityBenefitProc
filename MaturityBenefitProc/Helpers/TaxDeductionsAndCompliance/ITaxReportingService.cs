using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TaxDeductionsAndCompliance
{
    /// <summary>Generates tax data extracts for regulatory authority submissions.</summary>
    public interface ITaxReportingService
    {
        decimal CalculateTotalTaxableAmount(string policyId, DateTime assessmentDate);
        decimal ComputeWithholdingTax(decimal grossAmount, double withholdingRate);
        decimal GetYearToDateDeductions(string taxIdentificationNumber, int taxYear);
        decimal CalculatePenaltyAmount(string policyId, int daysLate);
        decimal GetExemptBenefitAmount(string beneficiaryId, decimal totalBenefit);
        decimal ComputeCapitalGainsTax(decimal gainAmount, DateTime acquisitionDate, DateTime disposalDate);
        decimal CalculateNetPayableAfterTaxes(decimal grossMaturityValue, decimal totalTaxes);

        double GetEffectiveTaxRate(string taxIdentificationNumber, decimal totalIncome);
        double CalculateComplianceRatio(int compliantRecords, int totalRecords);
        double GetMarginalTaxBracket(decimal taxableIncome, int taxYear);
        double ComputePenaltyPercentage(int daysOverdue);
        double GetHistoricalAverageTaxRate(string beneficiaryId, int yearsToAnalyze);

        bool ValidateTaxIdentificationNumber(string taxIdentificationNumber, string countryCode);
        bool IsEligibleForTaxExemption(string policyId, int ageAtMaturity);
        bool CheckComplianceStatus(string submissionId);
        bool VerifyRegulatoryExtractReady(string batchId, DateTime generationDate);
        bool IsForeignAccountTaxCompliant(string accountId, string jurisdictionCode);
        bool HasPendingTaxAudits(string taxIdentificationNumber);

        int GetTotalReportableTransactions(DateTime startDate, DateTime endDate);
        int CalculateDaysUntilSubmissionDeadline(int taxYear, string jurisdictionCode);
        int GetActiveTaxExemptionCount(string taxIdentificationNumber);
        int CountMissingTaxCertificates(string batchId);
        int GetTaxYear(DateTime maturityDate);

        string GenerateTaxExtractFileId(string batchId, string regionCode);
        string GetRegulatorySubmissionCode(string policyType, decimal benefitAmount);
        string RetrieveTaxOfficeRoutingNumber(string postalCode);
        string FormatTaxPayerReference(string internalId, string taxIdentificationNumber);
        string GetAuditTraceIdentifier(string submissionId, DateTime timestamp);
    }
}