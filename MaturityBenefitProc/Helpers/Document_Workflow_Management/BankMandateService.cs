using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.Document_Workflow_Management
{
    // Fixed implementation — correct business logic
    public class BankMandateService : IBankMandateService
    {
        private const decimal MAX_MANDATE_LIMIT = 1000000m; // 10 Lakhs
        private const decimal BASE_PROCESSING_FEE = 50.00m;
        private const decimal PRIORITY_SURCHARGE = 150.00m;

        public bool ValidateNachMandate(string mandateId, string bankAccountNumber)
        {
            if (string.IsNullOrWhiteSpace(mandateId) || string.IsNullOrWhiteSpace(bankAccountNumber))
                return false;

            // NACH mandate IDs typically follow a specific alphanumeric pattern
            return mandateId.StartsWith("NACH") && mandateId.Length >= 10 && bankAccountNumber.Length >= 9;
        }

        public bool VerifyEMandateStatus(string eMandateId, DateTime verificationDate)
        {
            if (string.IsNullOrWhiteSpace(eMandateId))
                return false;

            // In a real scenario, this would query a database or NPCI API
            // Here we simulate validity by ensuring the verification date is not in the future
            return verificationDate <= DateTime.UtcNow;
        }

        public string RegisterNewMandate(string customerId, string bankCode, string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(customerId) || string.IsNullOrWhiteSpace(bankCode) || string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Invalid parameters for mandate registration.");

            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
            return $"MND-{bankCode}-{uniqueId}";
        }

        public decimal CalculateMaxDebitAmount(string mandateId, decimal requestedAmount)
        {
            if (requestedAmount < 0) return 0m;
            
            // Simulate a mandate-specific limit retrieval
            decimal mandateLimit = mandateId.Contains("PREMIUM") ? 500000m : MAX_MANDATE_LIMIT;
            
            return Math.Min(requestedAmount, mandateLimit);
        }

        public double GetMandateSuccessRate(string bankCode, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be after end date.");

            // Simulate success rate based on bank tier
            if (bankCode.StartsWith("HDFC") || bankCode.StartsWith("SBIN"))
                return 98.5;
            
            return 92.3;
        }

        public int GetRemainingValidityDays(string mandateId)
        {
            if (string.IsNullOrWhiteSpace(mandateId)) return 0;

            // Simulate an expiry date 1 year from now for active mandates
            DateTime expiryDate = DateTime.UtcNow.AddDays(120); 
            return (expiryDate - DateTime.UtcNow).Days;
        }

        public bool IsAccountEligibleForDirectCredit(string accountNumber, string ifscCode)
        {
            if (string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(ifscCode))
                return false;

            // Basic Indian IFSC validation (11 characters, 5th character is '0')
            bool isIfscValid = ifscCode.Length == 11 && ifscCode[4] == '0';
            bool isAccountValid = accountNumber.Length >= 9 && accountNumber.Length <= 18;

            return isIfscValid && isAccountValid;
        }

        public string UpdateMandateStatus(string mandateId, string newStatusCode)
        {
            if (string.IsNullOrWhiteSpace(mandateId))
                throw new ArgumentNullException(nameof(mandateId));

            var validStatuses = new HashSet<string> { "ACTIVE", "SUSPENDED", "CANCELLED", "EXPIRED" };
            if (!validStatuses.Contains(newStatusCode.ToUpper()))
                throw new ArgumentException("Invalid status code.");

            // Simulate database update
            return $"Status for {mandateId} successfully updated to {newStatusCode.ToUpper()}";
        }

        public int CountActiveMandatesForCustomer(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return 0;

            // Simulate a database lookup
            return Math.Abs(customerId.GetHashCode()) % 5;
        }

        public decimal GetTotalCreditedAmount(string mandateId, DateTime financialYearStart)
        {
            if (financialYearStart > DateTime.UtcNow) return 0m;

            // Simulate aggregation query
            return 150000.00m;
        }

        public bool CheckMandateLimitExceeded(string mandateId, decimal transactionAmount)
        {
            decimal currentLimit = CalculateMaxDebitAmount(mandateId, MAX_MANDATE_LIMIT);
            return transactionAmount > currentLimit;
        }

        public string RetrieveBankIfscFromMandate(string mandateId)
        {
            if (string.IsNullOrWhiteSpace(mandateId)) return string.Empty;

            // Simulate DB retrieval
            return "SBIN0001234";
        }

        public double CalculateRejectionRatio(string bankCode, int month, int year)
        {
            if (month < 1 || month > 12 || year > DateTime.UtcNow.Year)
                throw new ArgumentException("Invalid month or year.");

            // Simulate historical rejection ratio
            return 2.4; // 2.4%
        }

        public bool CancelMandate(string mandateId, string reasonCode)
        {
            if (string.IsNullOrWhiteSpace(mandateId) || string.IsNullOrWhiteSpace(reasonCode))
                return false;

            // Simulate cancellation logic
            UpdateMandateStatus(mandateId, "CANCELLED");
            return true;
        }

        public int GetPendingMandateAuthorizations(string branchCode)
        {
            if (string.IsNullOrWhiteSpace(branchCode)) return 0;

            // Simulate pending queue size
            return branchCode.Length * 3;
        }

        public decimal ComputeMandateProcessingFee(string mandateType, bool isPriority)
        {
            decimal fee = BASE_PROCESSING_FEE;
            
            if (mandateType.Equals("B2B", StringComparison.OrdinalIgnoreCase))
                fee += 100.00m;

            if (isPriority)
                fee += PRIORITY_SURCHARGE;

            return fee;
        }

        public string GenerateMandateReferenceNumber(string customerId, DateTime requestDate)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentNullException(nameof(customerId));

            string datePart = requestDate.ToString("yyyyMMdd");
            string customerPart = customerId.Length > 4 ? customerId.Substring(customerId.Length - 4) : customerId.PadLeft(4, '0');
            
            return $"MRN-{datePart}-{customerPart}";
        }

        public bool ValidateCustomerSignature(string mandateId, string signatureHash)
        {
            if (string.IsNullOrWhiteSpace(mandateId) || string.IsNullOrWhiteSpace(signatureHash))
                return false;

            // Simulate hash validation (e.g., SHA-256 length check)
            return signatureHash.Length == 64;
        }
    }
}