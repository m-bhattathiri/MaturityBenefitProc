using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.ChequeDispatch
{
    public interface IChequeDispatchService
    {
        ChequeDispatchResult ProcessChequeDispatch(string claimNumber, string payeeName, decimal amount);

        ChequeDispatchResult ValidateChequeDispatch(string claimNumber);

        string GenerateChequeNumber(string branchCode, int sequenceNumber);

        ChequeDispatchResult TrackCourierStatus(string awbNumber);

        ChequeDispatchResult MarkChequeDelivered(string chequeNumber, DateTime deliveryDate);

        ChequeDispatchResult HandleReturnedCheque(string chequeNumber, string returnReason);

        bool IsChequeExpired(DateTime issueDate, int validityDays);

        ChequeDispatchResult ReissueCheque(string originalChequeNumber, string reason);

        decimal GetChequeDispatchCharges(string dispatchMode);

        ChequeDispatchResult GetChequeDetails(string chequeNumber);

        bool ValidateChequeAmount(decimal amount);

        string GetCourierPartner(string destinationPincode);

        List<ChequeDispatchResult> GetChequeDispatchHistory(string claimNumber, DateTime fromDate, DateTime toDate);

        ChequeDispatchResult CancelCheque(string chequeNumber, string reason);

        decimal GetMaximumChequeAmount();
    }
}
