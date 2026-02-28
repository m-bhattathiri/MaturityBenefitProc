// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.InternationalAndNriProcessing
{
    public class NriTaxationService : INriTaxationService
    {
        private const double DefaultTdsRate = 0.312; // 30% + 4% cess
        private const double LongTermCapitalGainsRate = 0.10;
        private const double ShortTermCapitalGainsRate = 0.15;
        private const int LongTermHoldingPeriodDays = 1095; // 3 years

        private readonly Dictionary<string, double> _dtaaRates = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "US", 0.15 },
            { "UK", 0.10 },
            { "AE", 0.10 },
            { "SG", 0.15 }
        };

        public decimal CalculateWithholdingTax(string policyId, decimal maturityAmount, string countryCode)
        {
            if (maturityAmount <= 0) return 0m;
            if (string.IsNullOrWhiteSpace(countryCode)) return maturityAmount * (decimal)DefaultTdsRate;

            double rate = _dtaaRates.ContainsKey(countryCode) ? _dtaaRates[countryCode] : DefaultTdsRate;
            return maturityAmount * (decimal)rate;
        }

        public double GetDtaaRate(string countryCode, DateTime effectiveDate)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return DefaultTdsRate;
            if (effectiveDate > DateTime.UtcNow) return DefaultTdsRate; // Future dates fallback

            return _dtaaRates.TryGetValue(countryCode, out double rate) ? rate : DefaultTdsRate;
        }

        public bool IsEligibleForDtaaBenefits(string customerId, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(customerId) || string.IsNullOrWhiteSpace(countryCode)) return false;
            
            // Requires TRC and Form 10F
            string trcStatus = GetTaxResidencyCertificateStatus(customerId);
            bool hasForm10F = CheckForm10FSubmission(customerId, DateTime.UtcNow);

            return trcStatus == "Valid" && hasForm10F && _dtaaRates.ContainsKey(countryCode);
        }

        public string GetTaxResidencyCertificateStatus(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return "Invalid";
            // Mock logic: Even IDs are valid, odd are expired
            return customerId.Length % 2 == 0 ? "Valid" : "Expired";
        }

        public int GetDaysPresentInCountry(string customerId, DateTime financialYearStart, DateTime financialYearEnd)
        {
            if (financialYearStart >= financialYearEnd) return 0;
            // Mock logic
            return Math.Abs(customerId.GetHashCode() % 365);
        }

        public decimal ApplySurchargeAndCess(decimal baseTaxAmount, double surchargeRate, double cessRate)
        {
            if (baseTaxAmount <= 0) return 0m;
            
            decimal surchargeAmount = baseTaxAmount * (decimal)surchargeRate;
            decimal taxWithSurcharge = baseTaxAmount + surchargeAmount;
            decimal cessAmount = taxWithSurcharge * (decimal)cessRate;
            
            return taxWithSurcharge + cessAmount;
        }

        public bool ValidatePanStatus(string panNumber)
        {
            if (string.IsNullOrWhiteSpace(panNumber) || panNumber.Length != 10) return false;
            
            // Basic PAN format validation: 5 letters, 4 digits, 1 letter
            for (int i = 0; i < 5; i++) if (!char.IsLetter(panNumber[i])) return false;
            for (int i = 5; i < 9; i++) if (!char.IsDigit(panNumber[i])) return false;
            if (!char.IsLetter(panNumber[9])) return false;

            return true;
        }

        public string GetFatcaDeclarationId(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) throw new ArgumentException("Customer ID required");
            return $"FATCA-{customerId}-{DateTime.UtcNow.Year}";
        }

        public decimal CalculateNetMaturityAmount(decimal grossAmount, decimal totalTaxDeducted)
        {
            if (grossAmount < 0 || totalTaxDeducted < 0) throw new ArgumentException("Amounts cannot be negative");
            return Math.Max(0, grossAmount - totalTaxDeducted);
        }

        public double GetEffectiveTdsRate(string customerId, decimal maturityAmount)
        {
            if (maturityAmount < 250000) return 0.0; // Exemption limit
            return DefaultTdsRate;
        }

        public int GetRemainingValidityDaysForTrc(string trcDocumentId)
        {
            if (string.IsNullOrWhiteSpace(trcDocumentId)) return 0;
            // Mock logic
            return 180;
        }

        public bool CheckForm10FSubmission(string customerId, DateTime financialYear)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return false;
            return financialYear.Year <= DateTime.UtcNow.Year;
        }

        public decimal ComputeExchangeRateVariance(decimal baseAmount, double exchangeRate)
        {
            if (exchangeRate <= 0) throw new ArgumentException("Exchange rate must be positive");
            decimal converted = baseAmount * (decimal)exchangeRate;
            // Simulate a 1% variance buffer
            return converted * 0.01m;
        }

        public string ResolveTaxTreatyCode(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return "UNKNOWN";
            return $"TREATY-{countryCode.ToUpper()}";
        }

        public bool IsNriStatusConfirmed(string customerId, DateTime evaluationDate)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return false;
            return evaluationDate <= DateTime.UtcNow;
        }

        public double GetMaximumMarginalReliefRate(decimal incomeAmount)
        {
            if (incomeAmount > 50000000m) return 0.37; // 37% surcharge tier
            if (incomeAmount > 20000000m) return 0.25;
            if (incomeAmount > 10000000m) return 0.15;
            if (incomeAmount > 5000000m) return 0.10;
            return 0.0;
        }

        public decimal CalculateCapitalGainsTax(string policyId, decimal gainAmount, int holdingPeriodDays)
        {
            if (gainAmount <= 0) return 0m;
            
            double taxRate = holdingPeriodDays >= LongTermHoldingPeriodDays 
                ? LongTermCapitalGainsRate 
                : ShortTermCapitalGainsRate;
                
            return gainAmount * (decimal)taxRate;
        }

        public int GetHoldingPeriod(DateTime issueDate, DateTime maturityDate)
        {
            if (issueDate >= maturityDate) return 0;
            return (maturityDate - issueDate).Days;
        }

        public string GenerateTdsCertificateNumber(string customerId, string policyId)
        {
            if (string.IsNullOrWhiteSpace(customerId) || string.IsNullOrWhiteSpace(policyId))
                throw new ArgumentException("Invalid inputs for TDS certificate");
                
            return $"TDS-{DateTime.UtcNow:yyyyMM}-{customerId.Substring(0, Math.Min(4, customerId.Length))}-{policyId}";
        }

        public bool VerifyOciCardValidity(string ociCardNumber)
        {
            if (string.IsNullOrWhiteSpace(ociCardNumber)) return false;
            return ociCardNumber.StartsWith("A", StringComparison.OrdinalIgnoreCase) && ociCardNumber.Length == 7;
        }

        public decimal GetRepatriableAmount(string policyId, decimal netAmount)
        {
            if (netAmount <= 0) return 0m;
            // Limit to 1 million USD equivalent per financial year (mocked as 75,000,000 INR)
            const decimal MaxRepatriable = 75000000m;
            return Math.Min(netAmount, MaxRepatriable);
        }

        public double GetNroAccountTdsRate(string bankCode)
        {
            return DefaultTdsRate; // NRO accounts typically attract maximum marginal rate
        }

        public int CountPreviousTaxFilings(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 0;
            return Math.Abs(customerId.GetHashCode() % 10);
        }

        public string DetermineResidentialStatus(int daysInIndia, int daysInPrecedingYears)
        {
            if (daysInIndia >= 182) return "Resident";
            if (daysInIndia >= 60 && daysInPrecedingYears >= 365) return "Resident Not Ordinarily Resident";
            return "Non-Resident Indian";
        }
    }
}