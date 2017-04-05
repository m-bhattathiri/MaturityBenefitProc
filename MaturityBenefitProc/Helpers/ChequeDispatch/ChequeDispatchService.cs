using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.ChequeDispatch
{
    public class ChequeDispatchService : IChequeDispatchService
    {
        public ChequeDispatchResult ProcessChequeDispatch(string claimNumber, string payeeName, decimal amount)
        {
            return new ChequeDispatchResult { Success = false, Message = "Not implemented" };
        }

        public ChequeDispatchResult ValidateChequeDispatch(string claimNumber)
        {
            return new ChequeDispatchResult { Success = false, Message = "Not implemented" };
        }

        public string GenerateChequeNumber(string branchCode, int sequenceNumber)
        {
            return string.Empty;
        }

        public ChequeDispatchResult TrackCourierStatus(string awbNumber)
        {
            return new ChequeDispatchResult { Success = false, Message = "Not implemented" };
        }

        public ChequeDispatchResult MarkChequeDelivered(string chequeNumber, DateTime deliveryDate)
        {
            return new ChequeDispatchResult { Success = false, Message = "Not implemented" };
        }

        public ChequeDispatchResult HandleReturnedCheque(string chequeNumber, string returnReason)
        {
            return new ChequeDispatchResult { Success = false, Message = "Not implemented" };
        }

        public bool IsChequeExpired(DateTime issueDate, int validityDays)
        {
            return false;
        }

        public ChequeDispatchResult ReissueCheque(string originalChequeNumber, string reason)
        {
            return new ChequeDispatchResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetChequeDispatchCharges(string dispatchMode)
        {
            return 0m;
        }

        public ChequeDispatchResult GetChequeDetails(string chequeNumber)
        {
            return new ChequeDispatchResult { Success = false, Message = "Not implemented" };
        }

        public bool ValidateChequeAmount(decimal amount)
        {
            return false;
        }

        public string GetCourierPartner(string destinationPincode)
        {
            return string.Empty;
        }

        public List<ChequeDispatchResult> GetChequeDispatchHistory(string claimNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<ChequeDispatchResult>();
        }

        public ChequeDispatchResult CancelCheque(string chequeNumber, string reason)
        {
            return new ChequeDispatchResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetMaximumChequeAmount()
        {
            return 0m;
        }
    }
}
