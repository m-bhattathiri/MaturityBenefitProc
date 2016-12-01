using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.TaxCertificate
{
    public interface ITaxCertificateService
    {
        TaxCertificateResult GenerateTaxCertificate(string policyNumber, string financialYear);

        TaxCertificateResult ValidateTaxCertificate(string certificateNumber);

        decimal CalculateTdsAmount(decimal disbursementAmount, bool hasPanCard, decimal exemptionLimit);

        decimal GetTdsRate(bool hasPanCard, decimal totalPremiumPaid);

        bool IsTdsApplicable(decimal maturityAmount, decimal totalPremiumPaid, int policyTerm);

        TaxCertificateResult GenerateForm10_10D(string policyNumber, string financialYear);

        TaxCertificateResult GenerateForm16A(string policyNumber, string financialYear);

        decimal GetSection10_10DExemption(decimal maturityAmount, decimal totalPremiumPaid, int policyTerm);

        decimal GetAnnualPremiumLimit(int commencementYear);

        bool ValidatePanForTds(string panNumber);

        decimal CalculateNetTaxableAmount(decimal maturityAmount, decimal totalPremiumPaid, decimal exemption);

        TaxCertificateResult GetTaxCertificateDetails(string certificateNumber);

        List<TaxCertificateResult> GetTaxCertificateHistory(string policyNumber, DateTime fromDate, DateTime toDate);

        decimal GetMaximumTdsAmount();

        string GetTdsSection(decimal maturityAmount, decimal premiumPaid);
    }
}
