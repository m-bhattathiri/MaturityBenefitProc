using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.TaxCertificate
{
    public class TaxCertificateService : ITaxCertificateService
    {
        public TaxCertificateResult GenerateTaxCertificate(string policyNumber, string financialYear)
        {
            return new TaxCertificateResult { Success = false, Message = "Not implemented" };
        }

        public TaxCertificateResult ValidateTaxCertificate(string certificateNumber)
        {
            return new TaxCertificateResult { Success = false, Message = "Not implemented" };
        }

        public decimal CalculateTdsAmount(decimal disbursementAmount, bool hasPanCard, decimal exemptionLimit)
        {
            return 0m;
        }

        public decimal GetTdsRate(bool hasPanCard, decimal totalPremiumPaid)
        {
            return 0m;
        }

        public bool IsTdsApplicable(decimal maturityAmount, decimal totalPremiumPaid, int policyTerm)
        {
            return false;
        }

        public TaxCertificateResult GenerateForm10_10D(string policyNumber, string financialYear)
        {
            return new TaxCertificateResult { Success = false, Message = "Not implemented" };
        }

        public TaxCertificateResult GenerateForm16A(string policyNumber, string financialYear)
        {
            return new TaxCertificateResult { Success = false, Message = "Not implemented" };
        }

        public decimal GetSection10_10DExemption(decimal maturityAmount, decimal totalPremiumPaid, int policyTerm)
        {
            return 0m;
        }

        public decimal GetAnnualPremiumLimit(int commencementYear)
        {
            return 0m;
        }

        public bool ValidatePanForTds(string panNumber)
        {
            return false;
        }

        public decimal CalculateNetTaxableAmount(decimal maturityAmount, decimal totalPremiumPaid, decimal exemption)
        {
            return 0m;
        }

        public TaxCertificateResult GetTaxCertificateDetails(string certificateNumber)
        {
            return new TaxCertificateResult { Success = false, Message = "Not implemented" };
        }

        public List<TaxCertificateResult> GetTaxCertificateHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            return new List<TaxCertificateResult>();
        }

        public decimal GetMaximumTdsAmount()
        {
            return 0m;
        }

        public string GetTdsSection(decimal maturityAmount, decimal premiumPaid)
        {
            return string.Empty;
        }
    }
}
