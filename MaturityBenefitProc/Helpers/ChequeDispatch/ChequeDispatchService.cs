using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.ChequeDispatch
{
    public class ChequeDispatchService : IChequeDispatchService
    {
        private readonly Dictionary<string, ChequeDispatchResult> _cheques = new Dictionary<string, ChequeDispatchResult>();
        private readonly Dictionary<string, List<ChequeDispatchResult>> _history = new Dictionary<string, List<ChequeDispatchResult>>();
        private int _counter = 0;

        public ChequeDispatchResult ProcessChequeDispatch(string claimNumber, string payeeName, decimal amount)
        {
            if (string.IsNullOrEmpty(claimNumber) || string.IsNullOrEmpty(payeeName) || amount <= 0)
            {
                return new ChequeDispatchResult
                {
                    Success = false,
                    Message = "Claim number, payee name and positive amount are required",
                    ReferenceId = string.Empty,
                    Amount = 0m
                };
            }

            _counter++;
            var chequeNum = GenerateChequeNumber("MBP", _counter);
            var result = new ChequeDispatchResult
            {
                Success = true,
                Message = "Cheque dispatched for claim " + claimNumber + " to " + payeeName,
                ReferenceId = "CHQ-" + _counter.ToString("D6"),
                Amount = amount,
                ChequeNumber = chequeNum,
                PayeeName = payeeName,
                IssueDate = DateTime.UtcNow,
                AwbNumber = "AWB" + _counter.ToString("D8"),
                CourierPartner = "BlueDart",
                DeliveryStatus = "Dispatched",
                DispatchDate = DateTime.UtcNow,
                ProcessedDate = DateTime.UtcNow
            };

            _cheques[chequeNum] = result;

            if (!_history.ContainsKey(claimNumber))
                _history[claimNumber] = new List<ChequeDispatchResult>();
            _history[claimNumber].Add(result);

            return result;
        }

        public ChequeDispatchResult ValidateChequeDispatch(string claimNumber)
        {
            if (string.IsNullOrEmpty(claimNumber))
            {
                return new ChequeDispatchResult
                {
                    Success = false,
                    Message = "Claim number is required for cheque dispatch validation"
                };
            }

            return new ChequeDispatchResult
            {
                Success = true,
                Message = "Claim " + claimNumber + " validated for cheque dispatch",
                ReferenceId = "VAL-" + claimNumber
            };
        }

        public string GenerateChequeNumber(string branchCode, int sequenceNumber)
        {
            if (string.IsNullOrEmpty(branchCode) || sequenceNumber <= 0) return string.Empty;
            return branchCode + "-" + sequenceNumber.ToString("D8");
        }

        public ChequeDispatchResult TrackCourierStatus(string awbNumber)
        {
            if (string.IsNullOrEmpty(awbNumber))
            {
                return new ChequeDispatchResult
                {
                    Success = false,
                    Message = "AWB number is required for courier tracking"
                };
            }

            return new ChequeDispatchResult
            {
                Success = true,
                Message = "Courier status for AWB " + awbNumber + ": In Transit",
                AwbNumber = awbNumber,
                DeliveryStatus = "InTransit",
                ReferenceId = "TRK-" + awbNumber
            };
        }

        public ChequeDispatchResult MarkChequeDelivered(string chequeNumber, DateTime deliveryDate)
        {
            if (string.IsNullOrEmpty(chequeNumber))
            {
                return new ChequeDispatchResult
                {
                    Success = false,
                    Message = "Cheque number is required to mark delivery"
                };
            }

            if (_cheques.ContainsKey(chequeNumber))
            {
                _cheques[chequeNumber].DeliveryStatus = "Delivered";
                _cheques[chequeNumber].Message = "Cheque delivered on " + deliveryDate.ToString("yyyy-MM-dd");
                return _cheques[chequeNumber];
            }

            return new ChequeDispatchResult
            {
                Success = true,
                Message = "Cheque " + chequeNumber + " marked as delivered on " + deliveryDate.ToString("yyyy-MM-dd"),
                ChequeNumber = chequeNumber,
                DeliveryStatus = "Delivered",
                ReferenceId = chequeNumber
            };
        }

        public ChequeDispatchResult HandleReturnedCheque(string chequeNumber, string returnReason)
        {
            if (string.IsNullOrEmpty(chequeNumber) || string.IsNullOrEmpty(returnReason))
            {
                return new ChequeDispatchResult
                {
                    Success = false,
                    Message = "Cheque number and return reason are required"
                };
            }

            return new ChequeDispatchResult
            {
                Success = true,
                Message = "Cheque " + chequeNumber + " returned: " + returnReason,
                ChequeNumber = chequeNumber,
                DeliveryStatus = "Returned",
                ReferenceId = chequeNumber
            };
        }

        public bool IsChequeExpired(DateTime issueDate, int validityDays)
        {
            if (validityDays <= 0) return true;
            return DateTime.UtcNow > issueDate.AddDays(validityDays);
        }

        public ChequeDispatchResult ReissueCheque(string originalChequeNumber, string reason)
        {
            if (string.IsNullOrEmpty(originalChequeNumber) || string.IsNullOrEmpty(reason))
            {
                return new ChequeDispatchResult
                {
                    Success = false,
                    Message = "Original cheque number and reissue reason are required"
                };
            }

            _counter++;
            var newChequeNum = GenerateChequeNumber("REISSUE", _counter);
            return new ChequeDispatchResult
            {
                Success = true,
                Message = "Cheque reissued from " + originalChequeNumber + ": " + reason,
                ChequeNumber = newChequeNum,
                ReferenceId = "REISSUE-" + _counter.ToString("D6"),
                IssueDate = DateTime.UtcNow
            };
        }

        public decimal GetChequeDispatchCharges(string dispatchMode)
        {
            if (string.IsNullOrEmpty(dispatchMode)) return 0m;
            switch (dispatchMode.ToUpper())
            {
                case "SPEED_POST": return 50m;
                case "COURIER": return 100m;
                case "REGISTERED_POST": return 30m;
                default: return 25m;
            }
        }

        public ChequeDispatchResult GetChequeDetails(string chequeNumber)
        {
            if (string.IsNullOrEmpty(chequeNumber))
            {
                return new ChequeDispatchResult
                {
                    Success = false,
                    Message = "Cheque number is required"
                };
            }

            if (_cheques.ContainsKey(chequeNumber))
                return _cheques[chequeNumber];

            return new ChequeDispatchResult
            {
                Success = false,
                Message = "Cheque not found: " + chequeNumber
            };
        }

        public bool ValidateChequeAmount(decimal amount)
        {
            return amount > 0 && amount <= GetMaximumChequeAmount();
        }

        public string GetCourierPartner(string destinationPincode)
        {
            if (string.IsNullOrEmpty(destinationPincode)) return string.Empty;
            if (destinationPincode.StartsWith("1") || destinationPincode.StartsWith("2"))
                return "BlueDart";
            if (destinationPincode.StartsWith("3") || destinationPincode.StartsWith("4"))
                return "DTDC";
            if (destinationPincode.StartsWith("5") || destinationPincode.StartsWith("6"))
                return "FedEx";
            return "IndiaPost";
        }

        public List<ChequeDispatchResult> GetChequeDispatchHistory(string claimNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrEmpty(claimNumber))
                return new List<ChequeDispatchResult>();

            if (_history.ContainsKey(claimNumber))
            {
                return _history[claimNumber]
                    .Where(h => h.ProcessedDate >= fromDate && h.ProcessedDate <= toDate)
                    .ToList();
            }

            return new List<ChequeDispatchResult>();
        }

        public ChequeDispatchResult CancelCheque(string chequeNumber, string reason)
        {
            if (string.IsNullOrEmpty(chequeNumber) || string.IsNullOrEmpty(reason))
            {
                return new ChequeDispatchResult
                {
                    Success = false,
                    Message = "Cheque number and cancellation reason are required"
                };
            }

            return new ChequeDispatchResult
            {
                Success = true,
                Message = "Cheque " + chequeNumber + " cancelled: " + reason,
                ChequeNumber = chequeNumber,
                DeliveryStatus = "Cancelled",
                ReferenceId = chequeNumber
            };
        }

        public decimal GetMaximumChequeAmount()
        {
            return 10000000m;
        }
    }
}
