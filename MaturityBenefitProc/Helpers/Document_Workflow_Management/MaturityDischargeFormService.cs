using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement
{
    // Fixed implementation — correct business logic
    public class MaturityDischargeFormService : IMaturityDischargeFormService
    {
        private const decimal BasePenalInterestRatePerDay = 15.5m;
        private const int MinimumSignaturesRequired = 2;
        private const double DefaultTdsRateWithPan = 5.0;
        private const double DefaultTdsRateWithoutPan = 20.0;

        public string GenerateDischargeVoucher(string policyNumber, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                throw new ArgumentException("Policy number cannot be empty.", nameof(policyNumber));

            string voucherId = $"VCH-{policyNumber}-{maturityDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
            return voucherId;
        }

        public bool ValidateVoucherSignatures(string voucherId, int signatureCount)
        {
            if (string.IsNullOrWhiteSpace(voucherId)) return false;
            return signatureCount >= MinimumSignaturesRequired;
        }

        public string ProcessReturnedVoucher(string voucherId, DateTime receivedDate)
        {
            if (string.IsNullOrWhiteSpace(voucherId))
                throw new ArgumentException("Voucher ID is required.");

            if (receivedDate > DateTime.Now)
                return "REJECTED_FUTURE_DATE";

            return "PROCESSED_SUCCESSFULLY";
        }

        public decimal CalculateGrossMaturityValue(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber)) return 0m;
            
            // Mocking a database retrieval based on policy number length/hash
            decimal sumAssured = 100000m + (Math.Abs(policyNumber.GetHashCode()) % 500000m);
            return sumAssured;
        }

        public decimal CalculateTerminalBonus(string policyNumber, decimal baseAmount)
        {
            if (baseAmount <= 0) return 0m;
            int term = GetPolicyTermInYears(policyNumber);
            double bonusRate = GetBonusRate(policyNumber, term);
            return baseAmount * (decimal)(bonusRate / 100.0);
        }

        public decimal CalculateOutstandingLoanDeduction(string policyNumber, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber)) return 0m;
            
            // Mock logic: policies ending in "L" have a loan
            if (policyNumber.EndsWith("L", StringComparison.OrdinalIgnoreCase))
            {
                return 25000m; // Mock loan amount
            }
            return 0m;
        }

        public decimal CalculateNetPayableAmount(string policyNumber, decimal grossAmount, decimal deductions)
        {
            decimal netAmount = grossAmount - deductions;
            return netAmount < 0 ? 0m : netAmount;
        }

        public decimal GetPenalInterestAmount(string policyNumber, int delayedDays)
        {
            if (delayedDays <= 0) return 0m;
            
            // Penal interest is calculated if the company delays the maturity payout
            return delayedDays * BasePenalInterestRatePerDay;
        }

        public double GetBonusRate(string policyNumber, int policyTerm)
        {
            if (policyTerm >= 20) return 8.5;
            if (policyTerm >= 15) return 6.0;
            if (policyTerm >= 10) return 4.5;
            return 2.0;
        }

        public double CalculateTaxDeductionPercentage(string customerId, bool isPanProvided)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.");

            return isPanProvided ? DefaultTdsRateWithPan : DefaultTdsRateWithoutPan;
        }

        public double GetLoanInterestRate(string policyNumber)
        {
            return 9.5; // Standard mock rate
        }

        public bool IsPolicyEligibleForMaturity(string policyNumber, DateTime currentDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber)) return false;
            
            // Mock logic: assume policy matures if it starts with "MAT"
            return policyNumber.StartsWith("MAT", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyCustomerBankDetails(string customerId, string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber) || accountNumber.Length < 8)
                return false;

            return true;
        }

        public bool CheckNeftMandateStatus(string policyNumber)
        {
            return !string.IsNullOrWhiteSpace(policyNumber);
        }

        public bool IsDischargeFormPrinted(string voucherId)
        {
            return !string.IsNullOrWhiteSpace(voucherId) && voucherId.StartsWith("VCH");
        }

        public bool ValidateWitnessDetails(string voucherId, string witnessId)
        {
            return !string.IsNullOrWhiteSpace(witnessId) && witnessId.Length >= 5;
        }

        public int GetRemainingDaysToMaturity(string policyNumber, DateTime currentDate)
        {
            // Mock maturity date 30 days from now for demonstration
            DateTime mockMaturityDate = currentDate.AddDays(30);
            return (mockMaturityDate - currentDate).Days;
        }

        public int CountPendingRequirements(string voucherId)
        {
            if (string.IsNullOrWhiteSpace(voucherId)) return 3;
            return voucherId.Contains("PENDING") ? 2 : 0;
        }

        public int GetPolicyTermInYears(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber)) return 0;
            return 15; // Mock term
        }

        public int GetDelayedProcessingDays(string voucherId, DateTime maturityDate)
        {
            int days = (DateTime.Now - maturityDate).Days;
            return days > 0 ? days : 0;
        }

        public string GetVoucherStatus(string voucherId)
        {
            if (string.IsNullOrWhiteSpace(voucherId)) return "INVALID";
            return CountPendingRequirements(voucherId) > 0 ? "PENDING_REQUIREMENTS" : "READY_FOR_PAYOUT";
        }

        public string RetrieveDocumentReferenceNumber(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber)) return null;
            return $"DOCREF-{policyNumber}-{DateTime.Now.Year}";
        }

        public string GetTaxExemptionCode(string customerId)
        {
            return "SEC_10_10D";
        }

        public string AssignProcessingUser(string voucherId, int workloadCount)
        {
            if (workloadCount > 50) return "QUEUE_OVERFLOW";
            return workloadCount % 2 == 0 ? "USER_ALPHA" : "USER_BETA";
        }

        public string GenerateNeftReference(string policyNumber, decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.");
            return $"NEFT-{DateTime.Now:yyyyMMdd}-{policyNumber}-{Guid.NewGuid().ToString().Substring(0, 6)}";
        }
    }
}