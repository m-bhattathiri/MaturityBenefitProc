using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MaturityBenefitProc.Helpers.TaxCertificate
{
    public class TaxCertificateService : ITaxCertificateService
    {
        private readonly Dictionary<string, TaxCertificateResult> _certificateRecords;
        private readonly Dictionary<string, List<TaxCertificateResult>> _policyHistory;
        private int _certificateCounter;
        private static readonly decimal MaximumTdsAmountLimit = 5000000m;
        private static readonly string TdsSection194DA = "194DA";
        private static readonly string TdsSectionExempt = "Exempt";
        private static readonly string Section10_10DCode = "10(10D)";

        public TaxCertificateService()
        {
            _certificateRecords = new Dictionary<string, TaxCertificateResult>();
            _policyHistory = new Dictionary<string, List<TaxCertificateResult>>();
            _certificateCounter = 1000;
        }

        public TaxCertificateResult GenerateTaxCertificate(string policyNumber, string financialYear)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new TaxCertificateResult
                {
                    Success = false,
                    Message = "Policy number is required for tax certificate generation"
                };
            }

            if (string.IsNullOrWhiteSpace(financialYear))
            {
                return new TaxCertificateResult
                {
                    Success = false,
                    Message = "Financial year is required for tax certificate generation"
                };
            }

            _certificateCounter++;
            var certNumber = string.Format("TAXCERT{0}{1:D6}",
                financialYear.Replace("-", ""),
                _certificateCounter);

            var result = new TaxCertificateResult
            {
                Success = true,
                Message = "Tax certificate generated successfully",
                ReferenceId = policyNumber,
                CertificateNumber = certNumber,
                FinancialYear = financialYear,
                ProcessedDate = DateTime.UtcNow
            };

            _certificateRecords[certNumber] = result;
            AddToHistory(policyNumber, result);

            return result;
        }

        public TaxCertificateResult ValidateTaxCertificate(string certificateNumber)
        {
            if (string.IsNullOrWhiteSpace(certificateNumber))
            {
                return new TaxCertificateResult
                {
                    Success = false,
                    Message = "Certificate number is required for validation"
                };
            }

            if (_certificateRecords.ContainsKey(certificateNumber))
            {
                var existing = _certificateRecords[certificateNumber];
                return new TaxCertificateResult
                {
                    Success = true,
                    Message = "Certificate is valid",
                    CertificateNumber = existing.CertificateNumber,
                    ReferenceId = existing.ReferenceId,
                    FinancialYear = existing.FinancialYear,
                    ProcessedDate = existing.ProcessedDate,
                    TdsSection = existing.TdsSection,
                    TdsRate = existing.TdsRate,
                    TdsAmount = existing.TdsAmount,
                    GrossAmount = existing.GrossAmount,
                    NetAmount = existing.NetAmount
                };
            }

            return new TaxCertificateResult
            {
                Success = false,
                Message = "Certificate not found or invalid"
            };
        }

        public decimal CalculateTdsAmount(decimal disbursementAmount, bool hasPanCard, decimal exemptionLimit)
        {
            if (disbursementAmount <= 0)
            {
                return 0m;
            }

            if (disbursementAmount <= exemptionLimit)
            {
                return 0m;
            }

            decimal taxableAmount = disbursementAmount - exemptionLimit;

            decimal tdsRate;
            if (hasPanCard)
            {
                tdsRate = 0.02m;
            }
            else
            {
                tdsRate = 0.20m;
            }

            decimal tdsAmount = taxableAmount * tdsRate;

            decimal maxTds = GetMaximumTdsAmount();
            if (tdsAmount > maxTds)
            {
                tdsAmount = maxTds;
            }

            return Math.Round(tdsAmount, 2);
        }

        public decimal GetTdsRate(bool hasPanCard, decimal totalPremiumPaid)
        {
            if (totalPremiumPaid <= 0)
            {
                return hasPanCard ? 2m : 20m;
            }

            if (hasPanCard)
            {
                return 2m;
            }

            return 20m;
        }

        public bool IsTdsApplicable(decimal maturityAmount, decimal totalPremiumPaid, int policyTerm)
        {
            if (maturityAmount <= 0 || totalPremiumPaid <= 0)
            {
                return false;
            }

            bool hasGain = maturityAmount > totalPremiumPaid;
            bool isLongTerm = policyTerm >= 5;

            if (isLongTerm && !hasGain)
            {
                return false;
            }

            if (hasGain && !isLongTerm)
            {
                return true;
            }

            if (hasGain && isLongTerm)
            {
                return true;
            }

            return false;
        }

        public TaxCertificateResult GenerateForm10_10D(string policyNumber, string financialYear)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new TaxCertificateResult
                {
                    Success = false,
                    Message = "Policy number is required for Form 10(10D)"
                };
            }

            if (string.IsNullOrWhiteSpace(financialYear))
            {
                return new TaxCertificateResult
                {
                    Success = false,
                    Message = "Financial year is required for Form 10(10D)"
                };
            }

            _certificateCounter++;
            var certNumber = string.Format("F1010D{0}{1:D6}",
                financialYear.Replace("-", ""),
                _certificateCounter);

            var result = new TaxCertificateResult
            {
                Success = true,
                Message = "Form 10(10D) exemption certificate generated",
                ReferenceId = policyNumber,
                CertificateNumber = certNumber,
                FinancialYear = financialYear,
                TdsSection = Section10_10DCode,
                ProcessedDate = DateTime.UtcNow
            };

            result.Metadata["FormType"] = Section10_10DCode;
            result.Metadata["ExemptionType"] = "Full";
            result.Metadata["Section"] = "10(10D) of Income Tax Act";
            result.Metadata["GeneratedDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd");

            _certificateRecords[certNumber] = result;
            AddToHistory(policyNumber, result);

            return result;
        }

        public TaxCertificateResult GenerateForm16A(string policyNumber, string financialYear)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new TaxCertificateResult
                {
                    Success = false,
                    Message = "Policy number is required for Form 16A"
                };
            }

            if (string.IsNullOrWhiteSpace(financialYear))
            {
                return new TaxCertificateResult
                {
                    Success = false,
                    Message = "Financial year is required for Form 16A"
                };
            }

            _certificateCounter++;
            var certNumber = string.Format("F16A{0}{1:D6}",
                financialYear.Replace("-", ""),
                _certificateCounter);

            var result = new TaxCertificateResult
            {
                Success = true,
                Message = "Form 16A TDS certificate generated",
                ReferenceId = policyNumber,
                CertificateNumber = certNumber,
                FinancialYear = financialYear,
                TdsSection = TdsSection194DA,
                ProcessedDate = DateTime.UtcNow
            };

            result.Metadata["FormType"] = "16A";
            result.Metadata["TdsSection"] = TdsSection194DA;
            result.Metadata["CertificateType"] = "TDS";
            result.Metadata["GeneratedDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd");

            _certificateRecords[certNumber] = result;
            AddToHistory(policyNumber, result);

            return result;
        }

        public decimal GetSection10_10DExemption(decimal maturityAmount, decimal totalPremiumPaid, int policyTerm)
        {
            if (maturityAmount <= 0 || totalPremiumPaid <= 0)
            {
                return 0m;
            }

            if (policyTerm < 5)
            {
                return 0m;
            }

            decimal gain = maturityAmount - totalPremiumPaid;
            if (gain <= 0)
            {
                return maturityAmount;
            }

            decimal annualPremiumEstimate = totalPremiumPaid / policyTerm;
            decimal sumAssuredEstimate = maturityAmount;
            decimal premiumLimitRatio = 0.10m;

            if (annualPremiumEstimate <= sumAssuredEstimate * premiumLimitRatio)
            {
                return maturityAmount;
            }

            decimal partialExemption = totalPremiumPaid;
            return Math.Round(partialExemption, 2);
        }

        public decimal GetAnnualPremiumLimit(int commencementYear)
        {
            if (commencementYear <= 0)
            {
                return 0m;
            }

            if (commencementYear <= 2012)
            {
                return 0.20m;
            }

            return 0.10m;
        }

        public bool ValidatePanForTds(string panNumber)
        {
            if (string.IsNullOrWhiteSpace(panNumber))
            {
                return false;
            }

            if (panNumber.Length != 10)
            {
                return false;
            }

            var panPattern = @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$";
            var panRegex = new Regex(panPattern);
            return panRegex.IsMatch(panNumber);
        }

        public decimal CalculateNetTaxableAmount(decimal maturityAmount, decimal totalPremiumPaid, decimal exemption)
        {
            if (maturityAmount <= 0)
            {
                return 0m;
            }

            decimal netTaxable = maturityAmount - totalPremiumPaid - exemption;

            if (netTaxable < 0)
            {
                return 0m;
            }

            return Math.Round(netTaxable, 2);
        }

        public TaxCertificateResult GetTaxCertificateDetails(string certificateNumber)
        {
            if (string.IsNullOrWhiteSpace(certificateNumber))
            {
                return new TaxCertificateResult
                {
                    Success = false,
                    Message = "Certificate number is required"
                };
            }

            if (_certificateRecords.ContainsKey(certificateNumber))
            {
                return _certificateRecords[certificateNumber];
            }

            return new TaxCertificateResult
            {
                Success = false,
                Message = "Tax certificate not found"
            };
        }

        public List<TaxCertificateResult> GetTaxCertificateHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new List<TaxCertificateResult>();
            }

            if (!_policyHistory.ContainsKey(policyNumber))
            {
                return new List<TaxCertificateResult>();
            }

            var filteredHistory = _policyHistory[policyNumber]
                .Where(c => c.ProcessedDate >= fromDate && c.ProcessedDate <= toDate)
                .OrderByDescending(c => c.ProcessedDate)
                .ToList();

            return filteredHistory;
        }

        public decimal GetMaximumTdsAmount()
        {
            return MaximumTdsAmountLimit;
        }

        public string GetTdsSection(decimal maturityAmount, decimal premiumPaid)
        {
            if (maturityAmount <= 0 || premiumPaid <= 0)
            {
                return TdsSectionExempt;
            }

            if (maturityAmount > premiumPaid)
            {
                return TdsSection194DA;
            }

            return TdsSectionExempt;
        }

        private void AddToHistory(string policyNumber, TaxCertificateResult result)
        {
            if (!_policyHistory.ContainsKey(policyNumber))
            {
                _policyHistory[policyNumber] = new List<TaxCertificateResult>();
            }
            _policyHistory[policyNumber].Add(result);
        }
    }
}
