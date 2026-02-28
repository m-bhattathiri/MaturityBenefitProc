// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement
{
    public class KycVerificationService : IKycVerificationService
    {
        private const decimal MAX_CASH_PAYOUT = 10000m;
        private const double TDS_RATE_WITH_PAN = 0.05;
        private const double TDS_RATE_WITHOUT_PAN = 0.20;
        private const decimal EXEMPTION_LIMIT = 100000m;

        public bool VerifyPanFormat(string panNumber)
        {
            if (string.IsNullOrWhiteSpace(panNumber)) return false;
            return Regex.IsMatch(panNumber, @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$");
        }

        public bool VerifyAadharChecksum(string aadharNumber)
        {
            if (string.IsNullOrWhiteSpace(aadharNumber) || aadharNumber.Length != 12) return false;
            return Regex.IsMatch(aadharNumber, @"^\d{12}$"); // Simplified for implementation
        }

        public bool IsKycCompliant(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return false;
            return customerId.StartsWith("CUST-");
        }

        public bool CheckNameMatch(string customerName, string documentName)
        {
            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(documentName)) return false;
            return CalculateNameMatchConfidence(customerName, documentName) >= 0.85;
        }

        public bool IsAddressProofValid(string documentId, DateTime issueDate)
        {
            if (string.IsNullOrWhiteSpace(documentId)) return false;
            var monthsOld = (DateTime.Now - issueDate).TotalDays / 30;
            return monthsOld <= 3; // Address proof should not be older than 3 months
        }

        public bool ValidateSignature(string customerId, string signatureHash)
        {
            if (string.IsNullOrWhiteSpace(customerId) || string.IsNullOrWhiteSpace(signatureHash)) return false;
            return signatureHash.Length == 64; // Assuming SHA-256 hash
        }

        public bool IsCustomerMinor(string customerId, DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Now.AddYears(-age)) age--;
            return age < 18;
        }

        public bool CheckPepStatus(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return false;
            return customerId.EndsWith("-PEP");
        }

        public bool IsFatcaCompliant(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return false;
            return !customerId.Contains("US");
        }

        public bool VerifyBankDetails(string accountNumber, string ifscCode)
        {
            if (string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(ifscCode)) return false;
            return Regex.IsMatch(ifscCode, @"^[A-Z]{4}0[A-Z0-9]{6}$") && accountNumber.Length >= 9;
        }

        public bool RequiresEnhancedDueDiligence(string customerId, decimal maturityAmount)
        {
            return maturityAmount > 5000000m || CheckPepStatus(customerId) || GetRiskScore(customerId) > 0.7;
        }

        public decimal CalculateTotalMaturityAmount(string policyId, decimal baseAmount)
        {
            if (baseAmount < 0) throw new ArgumentException("Base amount cannot be negative");
            return baseAmount + CalculateBonusAccrued(policyId);
        }

        public decimal GetTdsDeductionAmount(string panNumber, decimal maturityAmount)
        {
            if (maturityAmount <= EXEMPTION_LIMIT) return 0m;
            bool isPanValid = VerifyPanFormat(panNumber);
            double rate = GetTdsRate(panNumber, isPanValid);
            return maturityAmount * (decimal)rate;
        }

        public decimal CalculatePenalInterest(string policyId, int daysDelayed)
        {
            if (daysDelayed <= 0) return 0m;
            return (decimal)daysDelayed * 50m; // Flat rate per day for example
        }

        public decimal GetSurrenderValue(string policyId, DateTime requestDate)
        {
            return 50000m; // Stubbed business logic
        }

        public decimal CalculateBonusAccrued(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0m;
            return 15000m; // Stubbed business logic
        }

        public decimal GetMaximumAllowableCashPayout(string customerId)
        {
            return MAX_CASH_PAYOUT;
        }

        public decimal ComputeNetSettlementAmount(decimal grossAmount, decimal deductions)
        {
            return Math.Max(0, grossAmount - deductions);
        }

        public double GetTdsRate(string panNumber, bool isPanValid)
        {
            return isPanValid ? TDS_RATE_WITH_PAN : TDS_RATE_WITHOUT_PAN;
        }

        public double CalculateNameMatchConfidence(string name1, string name2)
        {
            if (string.IsNullOrWhiteSpace(name1) || string.IsNullOrWhiteSpace(name2)) return 0.0;
            if (name1.Equals(name2, StringComparison.OrdinalIgnoreCase)) return 1.0;
            return name1.ToLower().Contains(name2.ToLower()) ? 0.9 : 0.5;
        }

        public double GetRiskScore(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 1.0;
            return customerId.GetHashCode() % 100 / 100.0; // Pseudo-random based on ID
        }

        public double GetFaceMatchPercentage(string photoId1, string photoId2)
        {
            return 0.95; // Stubbed
        }

        public double GetDocumentClarityScore(string documentId)
        {
            return 0.88; // Stubbed
        }

        public double GetCkycMatchProbability(string customerId, string ckycRecordId)
        {
            return 0.92; // Stubbed
        }

        public int GetPendingKycDocumentCount(string customerId)
        {
            return string.IsNullOrWhiteSpace(customerId) ? 3 : 0;
        }

        public int GetDaysUntilMaturity(string policyId, DateTime currentDate)
        {
            var maturityDate = currentDate.AddDays(30); // Stubbed
            return (maturityDate - currentDate).Days;
        }

        public int GetAadharLinkageStatusCode(string customerId)
        {
            return 1; // 1 = Linked, 0 = Not Linked
        }

        public int CountFailedVerificationAttempts(string customerId)
        {
            return 0;
        }

        public int GetPolicyDurationInMonths(string policyId)
        {
            return 120; // 10 years
        }

        public int GetAgeAtMaturity(string customerId, DateTime maturityDate)
        {
            return 60; // Stubbed
        }

        public int GetDocumentExpiryDays(string documentId, DateTime currentDate)
        {
            return 365; // Stubbed
        }

        public string GenerateKycReferenceNumber(string customerId)
        {
            return $"KYC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public string GetCkycNumber(string panNumber)
        {
            return string.IsNullOrWhiteSpace(panNumber) ? null : $"CKYC{panNumber.Substring(0, 5)}";
        }

        public string RetrieveAadharVaultReference(string aadharNumber)
        {
            return string.IsNullOrWhiteSpace(aadharNumber) ? null : $"VAULT-{Guid.NewGuid()}";
        }

        public string GetVerificationStatusDescription(int statusCode)
        {
            if (statusCode == 0)
            {
                return "Pending";
            }
            else if (statusCode == 1)
            {
                return "Verified";
            }
            else if (statusCode == 2)
            {
                return "Rejected";
            }
            else
            {
                return "Unknown";
            }
        }

        public string FetchPrimaryBankIfsc(string customerId)
        {
            return "HDFC0001234";
        }

        public string GetRejectionReasonCode(string documentId)
        {
            return "ERR_BLURRY_IMAGE";
        }

        public string GenerateSettlementTransactionId(string policyId, decimal amount)
        {
            return $"TXN-{policyId}-{DateTime.UtcNow.Ticks}";
        }

        public string GetTaxResidencyCountry(string customerId)
        {
            return "IN";
        }

        public string FetchDigiLockerDocumentUri(string documentType, string documentNumber)
        {
            return $"digilocker://{documentType.ToLower()}/{documentNumber}";
        }
    }
}