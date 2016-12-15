using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.ChequeDispatch
{
    public class ChequeDispatchResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string ChequeNumber { get; set; }
        public string PayeeName { get; set; }
        public DateTime IssueDate { get; set; }
        public string AwbNumber { get; set; }
        public string CourierPartner { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime DispatchDate { get; set; }

        public ChequeDispatchResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
