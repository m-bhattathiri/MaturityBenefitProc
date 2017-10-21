using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.UndeliveredPayment
{
    public class UndeliveredPaymentService : IUndeliveredPaymentService
    {
        private readonly Dictionary<string, UndeliveredPaymentResult> _payments;
        private readonly Dictionary<string, int> _attemptCounts;
        private readonly Dictionary<string, string> _alternateAddresses;
        private readonly Dictionary<string, List<UndeliveredPaymentResult>> _cifHistory;

        public UndeliveredPaymentService()
        {
            _payments = new Dictionary<string, UndeliveredPaymentResult>();
            _attemptCounts = new Dictionary<string, int>
            {
                { "PAY001", 1 }, { "PAY002", 3 }, { "PAY003", 0 },
                { "PAY004", 2 }, { "PAY005", 4 }, { "PAY006", 1 },
                { "PAY007", 0 }, { "PAY008", 2 }, { "PAY009", 3 }
            };
            _alternateAddresses = new Dictionary<string, string>
            {
                { "CIF001", "123 Alt Street, Mumbai 400001" },
                { "CIF003", "456 Backup Lane, Delhi 110001" },
                { "CIF005", "789 Secondary Road, Chennai 600001" },
                { "CIF007", "101 Other Ave, Kolkata 700001" }
            };
            _cifHistory = new Dictionary<string, List<UndeliveredPaymentResult>>();
        }

        public UndeliveredPaymentResult ProcessUndeliveredPayment(string paymentReference, string reason)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return new UndeliveredPaymentResult { Success = false, Message = "Payment reference is required" };
            if (string.IsNullOrWhiteSpace(reason))
                return new UndeliveredPaymentResult { Success = false, Message = "Reason is required" };

            var result = new UndeliveredPaymentResult
            {
                Success = true,
                Message = "Undelivered payment processed",
                ReferenceId = paymentReference,
                ReturnReason = reason,
                ReturnDate = DateTime.UtcNow,
                RedispatchCount = _attemptCounts.ContainsKey(paymentReference) ? _attemptCounts[paymentReference] : 0,
                ProcessedDate = DateTime.UtcNow
            };
            result.Metadata["Status"] = "Undelivered";
            result.Metadata["Reason"] = reason;

            _payments[paymentReference] = result;
            return result;
        }

        public UndeliveredPaymentResult ValidateUndeliveredPayment(string paymentReference)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return new UndeliveredPaymentResult { Success = false, Message = "Payment reference is required" };

            if (_payments.ContainsKey(paymentReference))
            {
                var payment = _payments[paymentReference];
                return new UndeliveredPaymentResult
                {
                    Success = true,
                    Message = "Payment validated",
                    ReferenceId = paymentReference,
                    ReturnReason = payment.ReturnReason,
                    RedispatchCount = payment.RedispatchCount
                };
            }

            return new UndeliveredPaymentResult { Success = true, Message = "No undelivered record found", ReferenceId = paymentReference };
        }

        public UndeliveredPaymentResult UpdateAlternateAddress(string paymentReference, string newAddress, string pincode)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return new UndeliveredPaymentResult { Success = false, Message = "Payment reference is required" };
            if (string.IsNullOrWhiteSpace(newAddress))
                return new UndeliveredPaymentResult { Success = false, Message = "New address is required" };
            if (string.IsNullOrWhiteSpace(pincode))
                return new UndeliveredPaymentResult { Success = false, Message = "Pincode is required" };

            var fullAddress = $"{newAddress}, {pincode}";
            return new UndeliveredPaymentResult
            {
                Success = true,
                Message = "Alternate address updated",
                ReferenceId = paymentReference,
                AlternateAddress = fullAddress
            };
        }

        public UndeliveredPaymentResult InitiateRedispatch(string paymentReference, string dispatchMode)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return new UndeliveredPaymentResult { Success = false, Message = "Payment reference is required" };
            if (string.IsNullOrWhiteSpace(dispatchMode))
                return new UndeliveredPaymentResult { Success = false, Message = "Dispatch mode is required" };

            var charges = GetRedispatchCharges(dispatchMode);
            var attemptCount = GetRedispatchAttemptCount(paymentReference);

            if (!_attemptCounts.ContainsKey(paymentReference))
                _attemptCounts[paymentReference] = 0;
            _attemptCounts[paymentReference]++;

            return new UndeliveredPaymentResult
            {
                Success = true,
                Message = "Redispatch initiated",
                ReferenceId = paymentReference,
                NewDispatchMode = dispatchMode,
                RedispatchCount = _attemptCounts[paymentReference],
                Amount = charges
            };
        }

        public bool IsEligibleForRedispatch(string paymentReference, int maxAttempts)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return false;
            var attemptCount = GetRedispatchAttemptCount(paymentReference);
            return attemptCount < maxAttempts;
        }

        public int GetRedispatchAttemptCount(string paymentReference)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return 0;
            if (!_attemptCounts.ContainsKey(paymentReference))
                return 0;
            return _attemptCounts[paymentReference];
        }

        public UndeliveredPaymentResult ConvertToNeft(string paymentReference, string accountNumber, string ifscCode)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return new UndeliveredPaymentResult { Success = false, Message = "Payment reference is required" };
            if (string.IsNullOrWhiteSpace(accountNumber))
                return new UndeliveredPaymentResult { Success = false, Message = "Account number is required" };
            if (string.IsNullOrWhiteSpace(ifscCode))
                return new UndeliveredPaymentResult { Success = false, Message = "IFSC code is required" };

            return new UndeliveredPaymentResult
            {
                Success = true,
                Message = "Payment converted to NEFT",
                ReferenceId = paymentReference,
                NewDispatchMode = "NEFT"
            };
        }

        public UndeliveredPaymentResult GetUndeliveredPaymentDetails(string paymentReference)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return new UndeliveredPaymentResult { Success = false, Message = "Payment reference is required" };

            if (_payments.ContainsKey(paymentReference))
            {
                var payment = _payments[paymentReference];
                return new UndeliveredPaymentResult
                {
                    Success = true,
                    Message = "Payment details retrieved",
                    ReferenceId = paymentReference,
                    ReturnReason = payment.ReturnReason,
                    RedispatchCount = payment.RedispatchCount,
                    ReturnDate = payment.ReturnDate,
                    AlternateAddress = payment.AlternateAddress
                };
            }

            return new UndeliveredPaymentResult { Success = false, Message = "Payment details not found" };
        }

        public decimal GetRedispatchCharges(string dispatchMode)
        {
            if (string.IsNullOrWhiteSpace(dispatchMode))
                return 100m;

            switch (dispatchMode)
            {
                case "Speed":
                    return 150m;
                case "Registered":
                    return 75m;
                case "Courier":
                    return 200m;
                default:
                    return 100m;
            }
        }

        public UndeliveredPaymentResult MarkAddressVerified(string paymentReference, string verifiedBy)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return new UndeliveredPaymentResult { Success = false, Message = "Payment reference is required" };
            if (string.IsNullOrWhiteSpace(verifiedBy))
                return new UndeliveredPaymentResult { Success = false, Message = "Verified by is required" };

            return new UndeliveredPaymentResult
            {
                Success = true,
                Message = "Address verified",
                ReferenceId = paymentReference
            };
        }

        public bool HasAlternateAddress(string cifNumber)
        {
            if (string.IsNullOrWhiteSpace(cifNumber))
                return false;
            return _alternateAddresses.ContainsKey(cifNumber);
        }

        public UndeliveredPaymentResult EscalateUndelivered(string paymentReference, string escalationLevel)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return new UndeliveredPaymentResult { Success = false, Message = "Payment reference is required" };
            if (string.IsNullOrWhiteSpace(escalationLevel))
                return new UndeliveredPaymentResult { Success = false, Message = "Escalation level is required" };

            return new UndeliveredPaymentResult
            {
                Success = true,
                Message = $"Escalated to {escalationLevel}",
                ReferenceId = paymentReference
            };
        }

        public List<UndeliveredPaymentResult> GetUndeliveredPaymentHistory(string cifNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(cifNumber))
                return new List<UndeliveredPaymentResult>();

            if (!_cifHistory.ContainsKey(cifNumber))
                return new List<UndeliveredPaymentResult>();

            return _cifHistory[cifNumber]
                .Where(p => p.ProcessedDate >= fromDate && p.ProcessedDate <= toDate)
                .ToList();
        }

        public decimal GetMaximumRedispatchAmount()
        {
            return 10000000m;
        }

        public UndeliveredPaymentResult CancelRedispatch(string paymentReference, string reason)
        {
            if (string.IsNullOrWhiteSpace(paymentReference))
                return new UndeliveredPaymentResult { Success = false, Message = "Payment reference is required" };
            if (string.IsNullOrWhiteSpace(reason))
                return new UndeliveredPaymentResult { Success = false, Message = "Reason is required" };

            return new UndeliveredPaymentResult
            {
                Success = true,
                Message = "Redispatch cancelled",
                ReferenceId = paymentReference
            };
        }
    }
}
