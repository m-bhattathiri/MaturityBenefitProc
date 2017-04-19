using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.UndeliveredPayment
{
    public class UndeliveredPaymentService : IUndeliveredPaymentService
    {
        public UndeliveredPaymentResult ProcessUndeliveredPayment(string paymentReference, string reason)
        {
            return new UndeliveredPaymentResult { Success = false, Message = "Not implemented" };
        }

        public UndeliveredPaymentResult ValidateUndeliveredPayment(string paymentReference)
        {
            return new UndeliveredPaymentResult { Success = false, Message = "Not implemented" };
        }

        public UndeliveredPaymentResult UpdateAlternateAddress(string paymentReference, string newAddress, string pincode)
        {
            return new UndeliveredPaymentResult { Success = false, Message = "Not implemented" };
        }

        public UndeliveredPaymentResult InitiateRedispatch(string paymentReference, string dispatchMode)
        {
            return new UndeliveredPaymentResult { Success = false, Message = "Not implemented" };
        }

        public bool IsEligibleForRedispatch(string paymentReference, int maxAttempts)
        {
            return false;
        }

        public int GetRedispatchAttemptCount(string paymentReference)
        {
            return 0;
        }

        public UndeliveredPaymentResult ConvertToNeft(string paymentReference, string accountNumber, string ifscCode)
        {
            return new UndeliveredPaymentResult { Success = false, Message = "Not implemented" };
        }

        public UndeliveredPaymentResult GetUndeliveredPaymentDetails(string paymentReference)
        {
            return new UndeliveredPaymentResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetRedispatchCharges(string dispatchMode)
        {
            return 0m;
        }

        public UndeliveredPaymentResult MarkAddressVerified(string paymentReference, string verifiedBy)
        {
            return new UndeliveredPaymentResult { Success = false, Message = "Not implemented" };
        }

        public bool HasAlternateAddress(string cifNumber)
        {
            return false;
        }

        public UndeliveredPaymentResult EscalateUndelivered(string paymentReference, string escalationLevel)
        {
            return new UndeliveredPaymentResult { Success = false, Message = "Not implemented" };
        }

        public List<UndeliveredPaymentResult> GetUndeliveredPaymentHistory(string cifNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<UndeliveredPaymentResult>();
        }

        public decimal GetMaximumRedispatchAmount()
        {
            return 0m;
        }

        public UndeliveredPaymentResult CancelRedispatch(string paymentReference, string reason)
        {
            return new UndeliveredPaymentResult { Success = false, Message = "Not implemented" };
        }
    }
}
