using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.International_NRI_Processing
{
    // Fixed implementation — correct business logic
    public class NriRepatriationService : INriRepatriationService
    {
        private const decimal FEMA_USD_MILLION_LIMIT = 1000000m;
        private const decimal CA_CERT_THRESHOLD_INR = 500000m;
        private const decimal DEFAULT_TDS_RATE = 0.312m; // 31.2% default for NRIs
        
        private readonly HashSet<string> _restrictedCountries = new HashSet<string>(StringComparer.OrdinalIgnoreCase) 
        { 
            "KP", "IR", "SY", "CU" 
        };

        private readonly Dictionary<string, double> _dtaaRates = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "US", 0.15 }, { "UK", 0.10 }, { "AE", 0.10 }, { "SG", 0.15 }, { "AU", 0.15 }
        };

        public bool ValidateRepatriationEligibility(string policyId, string customerId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(customerId))
                return false;

            // Business logic: Check FATCA and pending documents
            bool isFatcaCompliant = CheckFatcaComplianceStatus(customerId);
            int pendingDocs = GetPendingDocumentCount(policyId);

            return isFatcaCompliant && pendingDocs == 0;
        }

        public decimal CalculateMaximumRepatriationAmount(string policyId, DateTime evaluationDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Invalid policy ID");

            // Mock logic: Base amount derived from premium paid in foreign currency
            decimal foreignPremium = CalculatePremiumPaidInForeignCurrency(policyId);
            double ratio = GetProportionateRepatriationRatio(policyId);

            return foreignPremium * (decimal)ratio;
        }

        public double GetCurrentFemaRepatriationLimitPercentage(string policyId)
        {
            // Usually 100% for NRE funded, but let's assume a dynamic check
            return VerifyNreAccountFundingSource(policyId, "ACC-MOCK") ? 100.0 : 30.0;
        }

        public int GetDaysSinceLastRepatriation(string customerId)
        {
            // Mocking a database lookup
            DateTime lastRepatriationDate = DateTime.UtcNow.AddDays(-45);
            return (DateTime.UtcNow - lastRepatriationDate).Days;
        }

        public string GetNroAccountStatus(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return "INVALID";
            return accountId.StartsWith("NRO") ? "ACTIVE" : "DORMANT";
        }

        public decimal CalculateTaxDeductionAtSource(decimal repatriationAmount, string taxResidencyCode)
        {
            if (repatriationAmount <= 0) return 0m;

            double dtaaRate = GetDoubleTaxationAvoidanceAgreementRate(taxResidencyCode);
            decimal applicableRate = dtaaRate > 0 ? (decimal)dtaaRate : DEFAULT_TDS_RATE;

            // Add surcharge and cess logic (simplified)
            decimal surcharge = applicableRate > 0.15m ? 0.10m : 0.0m;
            decimal effectiveRate = applicableRate * (1 + surcharge) * 1.04m; // 4% Health & Education Cess

            return Math.Round(repatriationAmount * effectiveRate, 2);
        }

        public bool IsFemaComplianceMet(string customerId, decimal requestedAmount)
        {
            // Under FEMA, NRIs can remit up to USD 1 Million per financial year from NRO account
            decimal ytdAmount = GetTotalRepatriatedAmountYearToDate(customerId, new DateTime(DateTime.UtcNow.Year, 4, 1));
            return (ytdAmount + requestedAmount) <= FEMA_USD_MILLION_LIMIT;
        }

        public string GenerateForm15CbRequirementCode(decimal amount)
        {
            if (amount > CA_CERT_THRESHOLD_INR)
                return "REQ-15CB-MANDATORY";
            
            return "REQ-15CB-EXEMPT";
        }

        public int GetRemainingRepatriationsForFinancialYear(string customerId, DateTime currentFinancialYearStart)
        {
            // Policy allows max 4 repatriations per financial year
            int usedRepatriations = 1; // Mock DB call
            return Math.Max(0, 4 - usedRepatriations);
        }

        public decimal CalculateExchangeRateVariance(string currencyCode, decimal baseAmount)
        {
            // Mock variance calculation
            decimal currentRate = currencyCode == "USD" ? 83.5m : 1.0m;
            decimal bookedRate = currencyCode == "USD" ? 82.0m : 1.0m;
            
            return Math.Round(baseAmount * (currentRate - bookedRate), 2);
        }

        public double GetDoubleTaxationAvoidanceAgreementRate(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return 0.0;
            
            return _dtaaRates.TryGetValue(countryCode, out double rate) ? rate : 0.0;
        }

        public bool VerifyNreAccountFundingSource(string policyId, string accountId)
        {
            // Mock verification logic
            return !string.IsNullOrWhiteSpace(accountId) && accountId.Contains("NRE");
        }

        public string GetReserveBankOfIndiaApprovalCode(string policyId, decimal amount)
        {
            if (amount > FEMA_USD_MILLION_LIMIT)
                return $"RBI-APV-{DateTime.UtcNow:yyyyMMdd}-{policyId.Substring(0, 4)}";
            
            return "RBI-AUTO-CLEARED";
        }

        public decimal ComputeAllowableMaturityProceeds(string policyId, decimal totalMaturityValue)
        {
            double ratio = GetProportionateRepatriationRatio(policyId);
            return Math.Round(totalMaturityValue * (decimal)ratio, 2);
        }

        public int CalculateDaysToMaturity(string policyId, DateTime currentDate)
        {
            DateTime maturityDate = currentDate.AddDays(120); // Mock maturity date
            return (maturityDate - currentDate).Days;
        }

        public bool CheckFatcaComplianceStatus(string customerId)
        {
            // Mock compliance check
            return !string.IsNullOrWhiteSpace(customerId) && customerId.Length > 5;
        }

        public decimal CalculatePremiumPaidInForeignCurrency(string policyId)
        {
            // Mock DB retrieval
            return 50000m; 
        }

        public double GetProportionateRepatriationRatio(string policyId)
        {
            // Ratio of foreign currency premium to total premium
            return 0.85; 
        }

        public string RetrieveAuthorizedDealerBankCode(string customerId)
        {
            return "AD-HDFC-001";
        }

        public bool ValidateCaCertificateRequirement(decimal requestedAmount)
        {
            return requestedAmount > CA_CERT_THRESHOLD_INR;
        }

        public decimal CalculateNetRepatriableAmount(decimal grossAmount, decimal tdsAmount, decimal fees)
        {
            decimal net = grossAmount - tdsAmount - fees;
            return net > 0 ? net : 0m;
        }

        public int GetPendingDocumentCount(string policyId)
        {
            // Mock check for KYC, Form 15CA/CB, etc.
            return policyId.EndsWith("PENDING") ? 2 : 0;
        }

        public string GetRepatriationRejectionReasonCode(string policyId, decimal requestedAmount)
        {
            if (!IsFemaComplianceMet("CUST-123", requestedAmount))
                return "REJ-FEMA-LIMIT-EXCEEDED";
            
            if (GetPendingDocumentCount(policyId) > 0)
                return "REJ-DOCS-PENDING";

            return "NONE";
        }

        public bool IsCountryInRestrictedList(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return true;
            return _restrictedCountries.Contains(countryCode);
        }

        public decimal GetTotalRepatriatedAmountYearToDate(string customerId, DateTime financialYearStart)
        {
            // Mock DB aggregation
            return 250000m; 
        }
    }
}