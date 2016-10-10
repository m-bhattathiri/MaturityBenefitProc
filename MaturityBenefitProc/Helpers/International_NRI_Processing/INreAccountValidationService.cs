using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.InternationalAndNriProcessing
{
    /// <summary>Ensures payouts are directed to valid NRE/NRO bank accounts.</summary>
    public interface INreAccountValidationService
    {
        bool ValidateNreAccountFormat(string accountNumber, string ifscCode);

        bool IsAccountRepatriable(string accountId);

        decimal CalculateRepatriationLimit(string customerId, DateTime financialYearStart);

        double GetCurrentExchangeRate(string currencyCode);

        int GetDaysSinceLastKycUpdate(string customerId);

        string GetAuthorizedDealerCode(string ifscCode);

        bool VerifyFemaCompliance(string customerId, decimal payoutAmount);

        decimal ComputeTdsOnNroPayout(decimal payoutAmount, double dtaaRate);

        double GetApplicableDtaaRate(string countryOfResidence);

        int CountActiveNreAccounts(string customerId);

        string GenerateForm15CbReference(string customerId, decimal remittanceAmount);

        bool CheckOciPioStatusValid(string documentId, DateTime expiryDate);

        decimal GetTotalRemittedAmountYtd(string customerId, int financialYear);

        double CalculateNroWithholdingTaxPercentage(bool hasPanCard, string countryCode);

        int GetPendingFemaDeclarationsCount(string customerId);

        string ResolveSwiftCode(string bankName, string branchCode);

        bool ValidateNroToNreTransferEligibility(string sourceAccountId, string targetAccountId, decimal amount);
    }
}