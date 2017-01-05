using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.UndeliveredPayment
{
    public class UndeliveredPaymentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public DateTime OriginalDispatchDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string ReturnReason { get; set; }
        public int RedispatchCount { get; set; }
        public string AlternateAddress { get; set; }
        public string NewDispatchMode { get; set; }

        public UndeliveredPaymentResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
