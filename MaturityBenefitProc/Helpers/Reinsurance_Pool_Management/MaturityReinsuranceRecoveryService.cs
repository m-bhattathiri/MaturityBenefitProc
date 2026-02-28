// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.Reinsurance_Pool_Management
{
    public class MaturityReinsuranceRecoveryService : IMaturityReinsuranceRecoveryService
    {
        private readonly HashSet<string> _sanctionedReinsurers = new HashSet<string> { "REIN_999", "REIN_666" };
        private readonly HashSet<string> _validCurrencies = new HashSet<string> { "USD", "EUR", "GBP", "JPY", "CHF" };

        public decimal CalculateTotalRecoveryAmount(string policyId, decimal totalMaturityBenefit)
        {
            if (string.IsNullOrEmpty(policyId) || totalMaturityBenefit <= 0) return 0m;
            
            double percentage = GetReinsurancePercentage(policyId, DateTime.UtcNow);
            return totalMaturityBenefit * (decimal)percentage;
        }

        public decimal CalculateQuotaShareRecovery(string policyId, decimal maturityAmount, double quotaSharePercentage)
        {
            if (quotaSharePercentage < 0 || quotaSharePercentage > 1)
                throw new ArgumentOutOfRangeException(nameof(quotaSharePercentage), "Must be between 0 and 1.");
            
            return maturityAmount * (decimal)quotaSharePercentage;
        }

        public decimal CalculateSurplusTreatyRecovery(string policyId, decimal maturityAmount, decimal retentionLimit)
        {
            if (maturityAmount <= retentionLimit) return 0m;
            return maturityAmount - retentionLimit;
        }

        public decimal CalculateExcessOfLossRecovery(string policyId, decimal maturityAmount, decimal attachmentPoint)
        {
            if (maturityAmount <= attachmentPoint) return 0m;
            return maturityAmount - attachmentPoint;
        }

        public double GetReinsurancePercentage(string policyId, DateTime maturityDate)
        {
            if (string.IsNullOrEmpty(policyId)) return 0.0;
            // Mock logic based on policy ID length
            return policyId.Length % 2 == 0 ? 0.5 : 0.25;
        }

        public double GetPoolAllocationRatio(string poolId, string reinsurerId)
        {
            if (string.IsNullOrEmpty(poolId) || string.IsNullOrEmpty(reinsurerId)) return 0.0;
            return 0.15; // Mock 15% share
        }

        public bool IsPolicyReinsured(string policyId)
        {
            return !string.IsNullOrEmpty(policyId) && policyId.StartsWith("RE");
        }

        public bool IsReinsurerActive(string reinsurerId, DateTime checkDate)
        {
            if (string.IsNullOrEmpty(reinsurerId)) return false;
            return !CheckSanctionsList(reinsurerId) && checkDate <= DateTime.UtcNow.AddYears(1);
        }

        public bool ValidateTreatyLimits(string treatyId, decimal recoveryAmount)
        {
            decimal limit = GetMaximumRecoveryLimit(treatyId);
            return recoveryAmount <= limit;
        }

        public bool CheckFacultativeEligibility(string policyId, decimal maturityAmount)
        {
            return maturityAmount > 5000000m; // Over 5M requires facultative
        }

        public int GetDaysUntilRecoveryDue(string reinsurerId, DateTime claimDate)
        {
            return Math.Max(0, 30 - (DateTime.UtcNow - claimDate).Days);
        }

        public int GetReinsurerCountForPolicy(string policyId)
        {
            return string.IsNullOrEmpty(policyId) ? 0 : 2;
        }

        public int GetActiveTreatiesCount(string reinsurerId, DateTime asOfDate)
        {
            return string.IsNullOrEmpty(reinsurerId) ? 0 : 3;
        }

        public string GetPrimaryReinsurerId(string policyId)
        {
            if (string.IsNullOrEmpty(policyId)) throw new ArgumentNullException(nameof(policyId));
            return $"REIN_PRI_{policyId.Substring(0, Math.Min(4, policyId.Length))}";
        }

        public string GetTreatyCode(string policyId, DateTime maturityDate)
        {
            return $"TRT-{maturityDate.Year}-{policyId.GetHashCode() % 1000}";
        }

        public string GenerateRecoveryClaimReference(string policyId, string reinsurerId)
        {
            return $"REC-{policyId}-{reinsurerId}-{DateTime.UtcNow:yyyyMMdd}";
        }

        public decimal CalculateProportionalRecovery(string policyId, decimal amount, double proportion)
        {
            if (proportion < 0 || proportion > 1) return 0m;
            return amount * (decimal)proportion;
        }

        public decimal CalculateNonProportionalRecovery(string policyId, decimal amount, decimal deductible)
        {
            return Math.Max(0m, amount - deductible);
        }

        public double GetFacultativeReinsuranceRate(string policyId)
        {
            return 0.05; // 5% rate
        }

        public bool IsPoolArrangementValid(string poolId, DateTime maturityDate)
        {
            return !string.IsNullOrEmpty(poolId) && maturityDate < DateTime.UtcNow.AddYears(5);
        }

        public int GetPoolMemberCount(string poolId)
        {
            return string.IsNullOrEmpty(poolId) ? 0 : 5;
        }

        public string GetPoolAdministratorId(string poolId)
        {
            return $"ADMIN_{poolId}";
        }

        public decimal CalculatePoolMemberShare(string poolId, string memberId, decimal totalRecovery)
        {
            double ratio = GetPoolAllocationRatio(poolId, memberId);
            return totalRecovery * (decimal)ratio;
        }

        public decimal ApplyCurrencyConversion(decimal amount, string fromCurrency, string toCurrency, DateTime conversionDate)
        {
            if (fromCurrency == toCurrency) return amount;
            double rate = GetCurrencyExchangeRate(fromCurrency, toCurrency, conversionDate);
            return amount * (decimal)rate;
        }

        public double GetCurrencyExchangeRate(string fromCurrency, string toCurrency, DateTime date)
        {
            if (!ValidateCurrencyCode(fromCurrency) || !ValidateCurrencyCode(toCurrency))
                throw new ArgumentException("Invalid currency code.");
            
            // Mock exchange rate
            return 1.1;
        }

        public bool ValidateCurrencyCode(string currencyCode)
        {
            return _validCurrencies.Contains(currencyCode?.ToUpper());
        }

        public string GetDefaultCurrency(string reinsurerId)
        {
            return "USD";
        }

        public decimal CalculateLatePaymentInterest(decimal recoveryAmount, double interestRate, int daysLate)
        {
            if (daysLate <= 0 || recoveryAmount <= 0) return 0m;
            return recoveryAmount * (decimal)(interestRate / 365.0) * daysLate;
        }

        public int GetGracePeriodDays(string treatyId)
        {
            return 15;
        }

        public bool IsRecoveryPastDue(string recoveryId, DateTime currentDate)
        {
            // Mock logic assuming recovery was due 30 days ago
            return true; 
        }

        public decimal CalculateNetRecoveryAmount(decimal grossRecovery, decimal brokerageFee, decimal taxes)
        {
            return Math.Max(0m, grossRecovery - brokerageFee - taxes);
        }

        public double GetBrokerageFeePercentage(string treatyId)
        {
            return 0.02; // 2%
        }

        public decimal CalculateReinstatementPremium(string treatyId, decimal recoveryAmount)
        {
            if (!RequiresReinstatement(treatyId, recoveryAmount)) return 0m;
            return recoveryAmount * 0.1m; // 10% premium
        }

        public bool RequiresReinstatement(string treatyId, decimal recoveryAmount)
        {
            return recoveryAmount > 1000000m;
        }

        public string GetReinstatementTerms(string treatyId)
        {
            return "100% premium for 100% reinstatement";
        }

        public decimal GetMaximumRecoveryLimit(string treatyId)
        {
            return 10000000m; // 10M limit
        }

        public bool CheckSanctionsList(string reinsurerId)
        {
            return _sanctionedReinsurers.Contains(reinsurerId);
        }
    }
}