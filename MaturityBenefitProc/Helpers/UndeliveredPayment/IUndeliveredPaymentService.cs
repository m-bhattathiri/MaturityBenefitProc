using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.UndeliveredPayment
{
    public interface IUndeliveredPaymentService
    {
        UndeliveredPaymentResult ProcessUndeliveredPayment(string paymentReference, string reason);

        UndeliveredPaymentResult ValidateUndeliveredPayment(string paymentReference);

        UndeliveredPaymentResult UpdateAlternateAddress(string paymentReference, string newAddress, string pincode);

        UndeliveredPaymentResult InitiateRedispatch(string paymentReference, string dispatchMode);

        bool IsEligibleForRedispatch(string paymentReference, int maxAttempts);

        int GetRedispatchAttemptCount(string paymentReference);

        UndeliveredPaymentResult ConvertToNeft(string paymentReference, string accountNumber, string ifscCode);

        UndeliveredPaymentResult GetUndeliveredPaymentDetails(string paymentReference);

        decimal GetRedispatchCharges(string dispatchMode);

        UndeliveredPaymentResult MarkAddressVerified(string paymentReference, string verifiedBy);

        bool HasAlternateAddress(string cifNumber);

        UndeliveredPaymentResult EscalateUndelivered(string paymentReference, string escalationLevel);

        List<UndeliveredPaymentResult> GetUndeliveredPaymentHistory(string cifNumber, DateTime fromDate, DateTime toDate);

        decimal GetMaximumRedispatchAmount();

        UndeliveredPaymentResult CancelRedispatch(string paymentReference, string reason);
    }
}
