using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.NeftDisbursement
{
    public class NeftDisbursementService : INeftDisbursementService
    {
        public NeftDisbursementResult ProcessNeftPayment(string claimNumber, string accountNumber, string ifscCode, decimal amount)
        {
            return new NeftDisbursementResult { Success = false, Message = "Not implemented" };
        }

        public NeftDisbursementResult ValidateNeftPayment(string claimNumber)
        {
            return new NeftDisbursementResult { Success = false, Message = "Not implemented" };
        }

        public bool ValidateIfscCode(string ifscCode)
        {
            return false;
        }

        public bool ValidateBankAccount(string accountNumber, string ifscCode)
        {
            return false;
        }

        public NeftDisbursementResult GetNeftPaymentStatus(string utrNumber)
        {
            return new NeftDisbursementResult { Success = false, Message = "Not implemented" };
        }

        public NeftDisbursementResult RetryNeftPayment(string originalUtr, string reason)
        {
            return new NeftDisbursementResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetNeftTransferLimit()
        {
            return 0m;
        }

        public bool IsWithinNeftWindow(DateTime transactionTime)
        {
            return false;
        }

        public NeftDisbursementResult CancelNeftPayment(string utrNumber, string reason)
        {
            return new NeftDisbursementResult { Success = false, Message = "Not implemented" };
        }

        public string GenerateUtrNumber(string bankCode, DateTime transactionDate)
        {
            return string.Empty;
        }

        public decimal GetNeftCharges(decimal amount)
        {
            return 0m;
        }

        public NeftDisbursementResult GetNeftPaymentDetails(string utrNumber)
        {
            return new NeftDisbursementResult { Success = false, Message = "Not implemented" };
        }

        public List<NeftDisbursementResult> GetNeftPaymentHistory(string claimNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<NeftDisbursementResult>();
        }

        public bool ValidateNeftAmount(decimal amount)
        {
            return false;
        }

        public NeftDisbursementResult SuspendNeftPayment(string utrNumber, string reason)
        {
            return new NeftDisbursementResult { Success = false, Message = "Not implemented" };
        }
    }
}
