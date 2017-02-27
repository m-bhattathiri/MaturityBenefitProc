using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.International_NRI_Processing
{
    // Buggy stub — returns incorrect values
    public class FemaComplianceService : IFemaComplianceService
    {
        public bool ValidateRepatriationEligibility(string policyNumber, string nriCustomerId)
        {
            return false;
        }

        public decimal CalculatePermissibleRepatriationAmount(string policyNumber, decimal totalMaturityAmount)
        {
            return 0m;
        }

        public double GetCurrentFemaWithholdingTaxRate(string countryCode)
        {
            return 0.0;
        }

        public int GetDaysSinceLastRepatriation(string nriCustomerId)
        {
            return 0;
        }

        public string GenerateFemaComplianceCertificateId(string transactionReference)
        {
            return null;
        }

        public bool CheckNroToNreTransferValidity(string sourceAccount, string destinationAccount, decimal transferAmount)
        {
            return false;
        }

        public decimal ComputeTdsOnNonRepatriableAmount(decimal nonRepatriableAmount, double currentTaxRate)
        {
            return 0m;
        }

        public int GetAnnualRepatriationTransactionCount(string nriCustomerId, DateTime currentFinancialYearStart)
        {
            return 0;
        }

        public string RetrieveAuthorizedDealerBankCode(string bankName, string branchCode)
        {
            return null;
        }

        public bool IsForm15CbRequired(decimal payoutAmount, string countryCode)
        {
            return false;
        }

        public decimal CalculateExchangeRateVariance(decimal baseAmount, double appliedExchangeRate, double standardExchangeRate)
        {
            return 0m;
        }

        public double GetApplicableSurchargePercentage(decimal totalPayoutAmount)
        {
            return 0.0;
        }

        public int CalculateRemainingDaysForLrsLimit(string nriCustomerId, DateTime transactionDate)
        {
            return 0;
        }

        public string GetFemaViolationCode(string policyNumber, decimal attemptedAmount)
        {
            return null;
        }

        public bool VerifyOciPioStatus(string customerId, string documentReference)
        {
            return false;
        }

        public decimal GetTotalRepatriatedAmountYearToDate(string nriCustomerId, DateTime financialYearStart)
        {
            return 0m;
        }

        public bool ValidateSourceOfFunds(string policyNumber, string fundSourceCode)
        {
            return false;
        }

        public string RequestReserveBankApproval(string nriCustomerId, decimal requestedAmount, string purposeCode)
        {
            return null;
        }
    }
}