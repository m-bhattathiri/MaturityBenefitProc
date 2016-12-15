using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.NeftDisbursement
{
    public interface INeftDisbursementService
    {
        NeftDisbursementResult ProcessNeftPayment(string claimNumber, string accountNumber, string ifscCode, decimal amount);

        NeftDisbursementResult ValidateNeftPayment(string claimNumber);

        bool ValidateIfscCode(string ifscCode);

        bool ValidateBankAccount(string accountNumber, string ifscCode);

        NeftDisbursementResult GetNeftPaymentStatus(string utrNumber);

        NeftDisbursementResult RetryNeftPayment(string originalUtr, string reason);

        decimal GetNeftTransferLimit();

        bool IsWithinNeftWindow(DateTime transactionTime);

        NeftDisbursementResult CancelNeftPayment(string utrNumber, string reason);

        string GenerateUtrNumber(string bankCode, DateTime transactionDate);

        decimal GetNeftCharges(decimal amount);

        NeftDisbursementResult GetNeftPaymentDetails(string utrNumber);

        List<NeftDisbursementResult> GetNeftPaymentHistory(string claimNumber, DateTime fromDate, DateTime toDate);

        bool ValidateNeftAmount(decimal amount);

        NeftDisbursementResult SuspendNeftPayment(string utrNumber, string reason);
    }
}
