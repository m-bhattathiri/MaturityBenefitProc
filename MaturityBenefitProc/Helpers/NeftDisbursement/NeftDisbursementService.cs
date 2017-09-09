using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MaturityBenefitProc.Helpers.NeftDisbursement
{
    public class NeftDisbursementService : INeftDisbursementService
    {
        private int _counter;
        private readonly Dictionary<string, NeftDisbursementResult> _payments;
        private readonly Dictionary<string, List<NeftDisbursementResult>> _claimPayments;
        private static readonly Regex IfscPattern = new Regex(@"^[A-Z]{4}0[A-Z0-9]{6}$", RegexOptions.Compiled);

        public NeftDisbursementService()
        {
            _counter = 0;
            _payments = new Dictionary<string, NeftDisbursementResult>();
            _claimPayments = new Dictionary<string, List<NeftDisbursementResult>>();
        }

        public NeftDisbursementResult ProcessNeftPayment(string claimNumber, string accountNumber, string ifscCode, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new NeftDisbursementResult { Success = false, Message = "Claim number is required" };

            if (!ValidateBankAccount(accountNumber, ifscCode))
                return new NeftDisbursementResult { Success = false, Message = "Invalid bank account details" };

            if (!ValidateIfscCode(ifscCode))
                return new NeftDisbursementResult { Success = false, Message = "Invalid IFSC code" };

            if (!ValidateNeftAmount(amount))
                return new NeftDisbursementResult { Success = false, Message = "Invalid NEFT amount" };

            if (!IsWithinNeftWindow(DateTime.UtcNow))
                return new NeftDisbursementResult { Success = false, Message = "Outside NEFT processing window" };

            var utr = GenerateUtrNumber("SBIN", DateTime.UtcNow);
            var charges = GetNeftCharges(amount);

            var result = new NeftDisbursementResult
            {
                Success = true,
                Message = "NEFT payment processed successfully",
                ReferenceId = claimNumber,
                Amount = amount,
                UtrNumber = utr,
                IfscCode = ifscCode,
                AccountNumber = accountNumber,
                TransactionDate = DateTime.UtcNow,
                SettlementDate = DateTime.UtcNow.AddDays(1),
                ProcessedDate = DateTime.UtcNow
            };
            result.Metadata["Charges"] = charges.ToString("F2");
            result.Metadata["Status"] = "Processed";

            _payments[utr] = result;
            if (!_claimPayments.ContainsKey(claimNumber))
                _claimPayments[claimNumber] = new List<NeftDisbursementResult>();
            _claimPayments[claimNumber].Add(result);

            return result;
        }

        public NeftDisbursementResult ValidateNeftPayment(string claimNumber)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new NeftDisbursementResult { Success = false, Message = "Claim number is required" };

            if (_claimPayments.ContainsKey(claimNumber) && _claimPayments[claimNumber].Any())
            {
                var latest = _claimPayments[claimNumber].Last();
                return new NeftDisbursementResult
                {
                    Success = true,
                    Message = "Payment validated",
                    ReferenceId = claimNumber,
                    UtrNumber = latest.UtrNumber,
                    Amount = latest.Amount
                };
            }

            return new NeftDisbursementResult { Success = true, Message = "No existing payments found for claim", ReferenceId = claimNumber };
        }

        public bool ValidateIfscCode(string ifscCode)
        {
            if (string.IsNullOrWhiteSpace(ifscCode))
                return false;
            return IfscPattern.IsMatch(ifscCode);
        }

        public bool ValidateBankAccount(string accountNumber, string ifscCode)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return false;
            if (accountNumber.Length < 9 || accountNumber.Length > 18)
                return false;
            if (!accountNumber.All(char.IsDigit))
                return false;
            return true;
        }

        public NeftDisbursementResult GetNeftPaymentStatus(string utrNumber)
        {
            if (string.IsNullOrWhiteSpace(utrNumber))
                return new NeftDisbursementResult { Success = false, Message = "UTR number is required" };

            if (_payments.ContainsKey(utrNumber))
            {
                var payment = _payments[utrNumber];
                return new NeftDisbursementResult
                {
                    Success = true,
                    Message = "Payment found",
                    UtrNumber = utrNumber,
                    Amount = payment.Amount,
                    AccountNumber = payment.AccountNumber,
                    IfscCode = payment.IfscCode
                };
            }

            return new NeftDisbursementResult { Success = false, Message = "Payment not found" };
        }

        public NeftDisbursementResult RetryNeftPayment(string originalUtr, string reason)
        {
            if (string.IsNullOrWhiteSpace(originalUtr))
                return new NeftDisbursementResult { Success = false, Message = "Original UTR is required" };
            if (string.IsNullOrWhiteSpace(reason))
                return new NeftDisbursementResult { Success = false, Message = "Reason is required" };

            if (_payments.ContainsKey(originalUtr))
            {
                var original = _payments[originalUtr];
                var newUtr = GenerateUtrNumber("SBIN", DateTime.UtcNow);
                var retryResult = new NeftDisbursementResult
                {
                    Success = true,
                    Message = "NEFT payment retry initiated",
                    UtrNumber = newUtr,
                    ReferenceId = original.ReferenceId,
                    Amount = original.Amount,
                    AccountNumber = original.AccountNumber,
                    IfscCode = original.IfscCode,
                    TransactionDate = DateTime.UtcNow
                };
                retryResult.Metadata["OriginalUtr"] = originalUtr;
                retryResult.Metadata["RetryReason"] = reason;
                _payments[newUtr] = retryResult;
                return retryResult;
            }

            return new NeftDisbursementResult { Success = false, Message = "Original payment not found" };
        }

        public decimal GetNeftTransferLimit()
        {
            return 1000000000m;
        }

        public bool IsWithinNeftWindow(DateTime transactionTime)
        {
            int hour = transactionTime.Hour;
            return hour >= 8 && hour < 19;
        }

        public NeftDisbursementResult CancelNeftPayment(string utrNumber, string reason)
        {
            if (string.IsNullOrWhiteSpace(utrNumber))
                return new NeftDisbursementResult { Success = false, Message = "UTR number is required" };
            if (string.IsNullOrWhiteSpace(reason))
                return new NeftDisbursementResult { Success = false, Message = "Reason is required" };

            if (_payments.ContainsKey(utrNumber))
            {
                var payment = _payments[utrNumber];
                payment.Metadata["Status"] = "Cancelled";
                payment.Metadata["CancelReason"] = reason;
                return new NeftDisbursementResult
                {
                    Success = true,
                    Message = "NEFT payment cancelled",
                    UtrNumber = utrNumber,
                    Amount = payment.Amount
                };
            }

            return new NeftDisbursementResult { Success = false, Message = "Payment not found for cancellation" };
        }

        public string GenerateUtrNumber(string bankCode, DateTime transactionDate)
        {
            if (string.IsNullOrWhiteSpace(bankCode))
                return string.Empty;
            _counter++;
            return $"UTR{bankCode}{transactionDate:yyyyMMdd}{_counter:D6}";
        }

        public decimal GetNeftCharges(decimal amount)
        {
            if (amount <= 10000m)
                return 2.50m;
            if (amount <= 100000m)
                return 5m;
            if (amount <= 200000m)
                return 15m;
            return 25m;
        }

        public NeftDisbursementResult GetNeftPaymentDetails(string utrNumber)
        {
            if (string.IsNullOrWhiteSpace(utrNumber))
                return new NeftDisbursementResult { Success = false, Message = "UTR number is required" };

            if (_payments.ContainsKey(utrNumber))
            {
                var payment = _payments[utrNumber];
                return new NeftDisbursementResult
                {
                    Success = true,
                    Message = "Payment details retrieved",
                    UtrNumber = utrNumber,
                    Amount = payment.Amount,
                    AccountNumber = payment.AccountNumber,
                    IfscCode = payment.IfscCode,
                    TransactionDate = payment.TransactionDate,
                    SettlementDate = payment.SettlementDate,
                    ReferenceId = payment.ReferenceId
                };
            }

            return new NeftDisbursementResult { Success = false, Message = "Payment details not found" };
        }

        public List<NeftDisbursementResult> GetNeftPaymentHistory(string claimNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
                return new List<NeftDisbursementResult>();

            if (!_claimPayments.ContainsKey(claimNumber))
                return new List<NeftDisbursementResult>();

            return _claimPayments[claimNumber]
                .Where(p => p.TransactionDate >= fromDate && p.TransactionDate <= toDate)
                .ToList();
        }

        public bool ValidateNeftAmount(decimal amount)
        {
            return amount > 0 && amount <= 1000000000m;
        }

        public NeftDisbursementResult SuspendNeftPayment(string utrNumber, string reason)
        {
            if (string.IsNullOrWhiteSpace(utrNumber))
                return new NeftDisbursementResult { Success = false, Message = "UTR number is required" };
            if (string.IsNullOrWhiteSpace(reason))
                return new NeftDisbursementResult { Success = false, Message = "Reason is required" };

            if (_payments.ContainsKey(utrNumber))
            {
                var payment = _payments[utrNumber];
                payment.Metadata["Status"] = "Suspended";
                payment.Metadata["SuspendReason"] = reason;
                return new NeftDisbursementResult
                {
                    Success = true,
                    Message = "NEFT payment suspended",
                    UtrNumber = utrNumber,
                    Amount = payment.Amount
                };
            }

            return new NeftDisbursementResult { Success = false, Message = "Payment not found for suspension" };
        }
    }
}
