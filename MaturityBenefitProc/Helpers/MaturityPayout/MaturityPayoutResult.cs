using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.MaturityPayout
{
    public class MaturityPayoutResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TdsDeducted { get; set; }
        public decimal NetPayable { get; set; }
        public string PaymentMode { get; set; }
        public string ApprovedBy { get; set; }

        public MaturityPayoutResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
