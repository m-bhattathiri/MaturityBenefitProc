using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.TaxCertificate
{
    public class TaxCertificateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedDate { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public string CertificateNumber { get; set; }
        public string FinancialYear { get; set; }
        public decimal TdsRate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TdsAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string TdsSection { get; set; }

        public TaxCertificateResult()
        {
            Metadata = new Dictionary<string, string>();
            ProcessedDate = DateTime.UtcNow;
        }
    }
}
