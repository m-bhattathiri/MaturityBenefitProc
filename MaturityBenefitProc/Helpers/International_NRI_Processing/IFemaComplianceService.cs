using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.International_NRI_Processing
{
    /// <summary>
    /// Validates payouts against Foreign Exchange Management Act guidelines.
    /// </summary>
    public interface IFemaComplianceService
    {
        bool ValidateRepatriationEligibility(string policyNumber, string nriCustomerId);
        decimal CalculatePermissibleRepatriationAmount(string policyNumber, decimal totalMaturityAmount);
        double GetCurrentFemaWithholdingTaxRate(string countryCode);
        int GetDaysSinceLastRepatriation(string nriCustomerId);
        string GenerateFemaComplianceCertificateId(string transactionReference);
        bool CheckNroToNreTransferValidity(string sourceAccount, string destinationAccount, decimal transferAmount);
        decimal ComputeTdsOnNonRepatriableAmount(decimal nonRepatriableAmount, double currentTaxRate);
        int GetAnnualRepatriationTransactionCount(string nriCustomerId, DateTime currentFinancialYearStart);
        string RetrieveAuthorizedDealerBankCode(string bankName, string branchCode);
        bool IsForm15CbRequired(decimal payoutAmount, string countryCode);
        decimal CalculateExchangeRateVariance(decimal baseAmount, double appliedExchangeRate, double standardExchangeRate);
        double GetApplicableSurchargePercentage(decimal totalPayoutAmount);
        int CalculateRemainingDaysForLrsLimit(string nriCustomerId, DateTime transactionDate);
        string GetFemaViolationCode(string policyNumber, decimal attemptedAmount);
        bool VerifyOciPioStatus(string customerId, string documentReference);
        decimal GetTotalRepatriatedAmountYearToDate(string nriCustomerId, DateTime financialYearStart);
        bool ValidateSourceOfFunds(string policyNumber, string fundSourceCode);
        string RequestReserveBankApproval(string nriCustomerId, decimal requestedAmount, string purposeCode);
    }
}