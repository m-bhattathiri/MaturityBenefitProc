using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.SurrenderProcessing
{
    // Fixed implementation — correct business logic
    public class PartialSurrenderService : IPartialSurrenderService
    {
        private const decimal MIN_WITHDRAWAL_AMOUNT = 500.00m;
        private const decimal SPOUSAL_CONSENT_THRESHOLD = 10000.00m;
        private const int MAX_SURRENDER_YEARS = 7;

        // Mock method to simulate fetching account value
        private decimal GetMockAccountValue(string policyId) => 100000m;
        
        // Mock method to simulate fetching policy issue date
        private DateTime GetMockIssueDate(string policyId) => new DateTime(2020, 1, 1);

        public decimal CalculateMaximumWithdrawalAmount(string policyId, DateTime effectiveDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));

            decimal accountValue = GetMockAccountValue(policyId);
            decimal minBalance = GetMinimumRemainingBalanceRequired("DEFAULT");
            decimal maxGross = accountValue - minBalance;

            if (maxGross <= 0) return 0m;

            decimal surrenderCharge = CalculateSurrenderCharge(policyId, maxGross);
            return Math.Max(0, maxGross - surrenderCharge);
        }

        public decimal CalculateSurrenderCharge(string policyId, decimal requestedAmount)
        {
            if (requestedAmount <= 0) return 0m;

            decimal freeAmount = GetAvailableFreeWithdrawalAmount(policyId, DateTime.Now);
            decimal chargeableAmount = Math.Max(0, requestedAmount - freeAmount);

            int policyYear = GetPolicyYear(policyId, DateTime.Now);
            double chargePercentage = GetSurrenderChargePercentage(policyId, policyYear);

            return chargeableAmount * (decimal)chargePercentage;
        }

        public decimal GetAvailableFreeWithdrawalAmount(string policyId, DateTime requestDate)
        {
            decimal accountValue = GetMockAccountValue(policyId);
            double freePercentage = GetFreeWithdrawalPercentage("DEFAULT");
            
            // Assuming no prior withdrawals for this mock
            decimal priorWithdrawals = 0m; 
            
            decimal totalFreeAvailable = accountValue * (decimal)freePercentage;
            return Math.Max(0, totalFreeAvailable - priorWithdrawals);
        }

        public decimal CalculateNetPayoutAmount(decimal grossAmount, decimal surrenderCharge, decimal taxWithholding)
        {
            if (grossAmount < 0) throw new ArgumentException("Gross amount cannot be negative.");
            
            decimal net = grossAmount - surrenderCharge - taxWithholding;
            return Math.Max(0, net);
        }

        public decimal GetMinimumRemainingBalanceRequired(string productCode)
        {
            if (productCode?.ToUpper() == "VA_PREMIER")
            {
                return 5000m;
            }
            else if (productCode?.ToUpper() == "FIA_STANDARD")
            {
                return 2500m;
            }
            else
            {
                return 2000m;
            }
        }

        public decimal CalculateProratedRiderDeduction(string policyId, decimal withdrawalAmount)
        {
            decimal accountValue = GetMockAccountValue(policyId);
            if (accountValue <= 0) return 0m;

            decimal annualRiderFee = 150m; // Mock rider fee
            double reductionFactor = CalculateProRataReductionFactor(withdrawalAmount, accountValue);
            
            return annualRiderFee * (decimal)reductionFactor;
        }

        public decimal CalculateMarketValueAdjustment(string policyId, decimal surrenderAmount, DateTime calculationDate)
        {
            if (surrenderAmount <= 0) return 0m;
            
            // Mock MVA calculation based on interest rate shifts
            decimal currentRate = 0.04m;
            decimal guaranteedRate = 0.03m;
            int yearsRemaining = 5;

            decimal mvaFactor = (guaranteedRate - currentRate) * yearsRemaining;
            return surrenderAmount * mvaFactor;
        }

        public bool IsEligibleForPartialSurrender(string policyId, DateTime requestDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return false;
            if (IsPolicyInLockupPeriod(policyId, requestDate)) return false;
            
            decimal accountValue = GetMockAccountValue(policyId);
            return accountValue > GetMinimumRemainingBalanceRequired("DEFAULT");
        }

        public bool ValidateMinimumWithdrawalAmount(string productCode, decimal requestedAmount)
        {
            decimal productMin = productCode == "VA_PREMIER" ? 1000m : MIN_WITHDRAWAL_AMOUNT;
            return requestedAmount >= productMin;
        }

        public bool HasExceededAnnualWithdrawalLimit(string policyId, DateTime requestDate)
        {
            int maxWithdrawals = GetMaximumAllowedWithdrawalsPerYear("DEFAULT");
            int currentWithdrawals = 2; // Mock current withdrawals
            return currentWithdrawals >= maxWithdrawals;
        }

        public bool IsPolicyInLockupPeriod(string policyId, DateTime requestDate)
        {
            DateTime issueDate = GetMockIssueDate(policyId);
            return (requestDate - issueDate).TotalDays < 30; // 30-day free look / lockup
        }

        public bool RequiresSpousalConsent(string policyId, decimal withdrawalAmount)
        {
            // In a real system, we'd check marital status and ERISA rules
            return withdrawalAmount >= SPOUSAL_CONSENT_THRESHOLD;
        }

        public bool IsSystematicWithdrawalActive(string policyId)
        {
            return policyId.EndsWith("-SW"); // Mock logic
        }

        public double GetSurrenderChargePercentage(string policyId, int policyYear)
        {
            if (policyYear < 1) return 0.0;
            if (policyYear > MAX_SURRENDER_YEARS) return 0.0;

            // Standard 7-year declining schedule: 7%, 6%, 5%, 4%, 3%, 2%, 1%, 0%
            return (MAX_SURRENDER_YEARS - policyYear + 1) / 100.0;
        }

        public double CalculateTaxWithholdingRate(string stateCode, bool isFederal)
        {
            if (isFederal) return 0.10; // 10% default federal withholding

            if (stateCode?.ToUpper() == "CA")
            {
                return 0.066;
            }
            else if (stateCode?.ToUpper() == "NY")
            {
                return 0.05;
            }
            else if (stateCode?.ToUpper() == "TX")
            {
                return 0.0;
            }
            else if (stateCode?.ToUpper() == "FL")
            {
                return 0.0;
            }
            else
            {
                return 0.04;
            }
        }

        public double GetFreeWithdrawalPercentage(string productCode)
        {
            return productCode?.ToUpper() == "VA_PREMIER" ? 0.15 : 0.10;
        }

        public double CalculateProRataReductionFactor(decimal withdrawalAmount, decimal accountValue)
        {
            if (accountValue <= 0) return 0.0;
            if (withdrawalAmount >= accountValue) return 1.0;
            
            return (double)(withdrawalAmount / accountValue);
        }

        public int GetRemainingFreeWithdrawalsCount(string policyId, int calendarYear)
        {
            int maxAllowed = GetMaximumAllowedWithdrawalsPerYear("DEFAULT");
            int used = 1; // Mock used count
            return Math.Max(0, maxAllowed - used);
        }

        public int GetDaysUntilSurrenderChargeExpires(string policyId, DateTime currentDate)
        {
            DateTime issueDate = GetMockIssueDate(policyId);
            DateTime expirationDate = issueDate.AddYears(MAX_SURRENDER_YEARS);
            
            if (currentDate >= expirationDate) return 0;
            return (expirationDate - currentDate).Days;
        }

        public int GetPolicyYear(string policyId, DateTime effectiveDate)
        {
            DateTime issueDate = GetMockIssueDate(policyId);
            if (effectiveDate < issueDate) return 0;

            int years = effectiveDate.Year - issueDate.Year;
            if (effectiveDate.Date < issueDate.Date.AddYears(years)) years--;
            
            return years + 1;
        }

        public int GetMaximumAllowedWithdrawalsPerYear(string productCode)
        {
            return productCode?.ToUpper() == "VA_PREMIER" ? 12 : 4;
        }

        public string GenerateSurrenderTransactionId(string policyId, DateTime requestDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            return $"SUR-{policyId}-{requestDate:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4)}";
        }

        public string GetSurrenderChargeScheduleCode(string policyId)
        {
            return "SC-7YR-STD";
        }

        public string DetermineTaxDistributionCode(int ageAtWithdrawal, bool isQualified)
        {
            if (!isQualified) return "7"; // Normal distribution
            return ageAtWithdrawal < 59 ? "1" : "7"; // 1 = Early distribution, no known exception
        }

        public string GetWithdrawalDenialReasonCode(string policyId, decimal requestedAmount)
        {
            if (IsPolicyInLockupPeriod(policyId, DateTime.Now)) return "ERR_LOCKUP_PERIOD";
            if (!ValidateMinimumWithdrawalAmount("DEFAULT", requestedAmount)) return "ERR_BELOW_MIN_AMOUNT";
            if (HasExceededAnnualWithdrawalLimit(policyId, DateTime.Now)) return "ERR_MAX_WITHDRAWALS_EXCEEDED";
            
            decimal maxAvailable = CalculateMaximumWithdrawalAmount(policyId, DateTime.Now);
            if (requestedAmount > maxAvailable) return "ERR_INSUFFICIENT_FUNDS";

            return "APPROVED";
        }
    }
}