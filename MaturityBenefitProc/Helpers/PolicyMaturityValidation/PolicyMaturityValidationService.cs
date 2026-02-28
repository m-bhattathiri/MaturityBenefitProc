using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.PolicyMaturityValidation
{
    public class PolicyMaturityValidationService : IPolicyMaturityValidationService
    {
        private readonly Dictionary<string, PolicyMaturityValidationResult> _validationRecords = new Dictionary<string, PolicyMaturityValidationResult>();
        private readonly Dictionary<string, List<PolicyMaturityValidationResult>> _validationHistory = new Dictionary<string, List<PolicyMaturityValidationResult>>();
        private readonly Dictionary<string, DateTime> _maturityDates = new Dictionary<string, DateTime>();
        private readonly Dictionary<string, string> _kycStatus = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> _premiumStatus = new Dictionary<string, bool>();
        private readonly Dictionary<string, bool> _loanStatus = new Dictionary<string, bool>();
        private readonly Dictionary<string, decimal> _loanAmounts = new Dictionary<string, decimal>();
        private readonly Dictionary<string, bool> _nomineeStatus = new Dictionary<string, bool>();
        private readonly Dictionary<string, string> _panRecords = new Dictionary<string, string>();
        private readonly Dictionary<string, DateTime> _dobRecords = new Dictionary<string, DateTime>();
        private int _counter = 0;

        public PolicyMaturityValidationResult ValidatePolicyForMaturity(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new PolicyMaturityValidationResult
                {
                    Success = false,
                    Message = "Policy number is required for maturity validation",
                    ProcessedDate = DateTime.UtcNow,
                    ValidationStatus = "Failed",
                    Metadata = new Dictionary<string, string> { { "Reason", "MissingPolicyNumber" } }
                };
            }

            var referenceId = string.Format("PMV-{0:D6}", ++_counter);
            var result = new PolicyMaturityValidationResult
            {
                Success = true,
                Message = "Policy validated for maturity processing",
                ReferenceId = referenceId,
                ProcessedDate = DateTime.UtcNow,
                ValidationStatus = "Validated",
                DocumentsVerified = true,
                PremiumStatus = _premiumStatus.ContainsKey(policyNumber) && _premiumStatus[policyNumber] ? "AllPaid" : "Pending",
                KycStatus = _kycStatus.ContainsKey(policyNumber) ? _kycStatus[policyNumber] : "NotVerified",
                OutstandingLoan = _loanAmounts.ContainsKey(policyNumber) ? _loanAmounts[policyNumber] : 0m,
                DaysToMaturity = _maturityDates.ContainsKey(policyNumber)
                    ? Math.Max(0, (int)(_maturityDates[policyNumber] - DateTime.UtcNow).TotalDays)
                    : 0,
                Metadata = new Dictionary<string, string>
                {
                    { "PolicyNumber", policyNumber },
                    { "ValidatedAt", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "ReferenceId", referenceId }
                }
            };

            _validationRecords[referenceId] = result;
            if (!_validationHistory.ContainsKey(policyNumber))
                _validationHistory[policyNumber] = new List<PolicyMaturityValidationResult>();
            _validationHistory[policyNumber].Add(result);

            return result;
        }

        public PolicyMaturityValidationResult VerifyDocuments(string policyNumber, string documentType, string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || string.IsNullOrWhiteSpace(documentType) || string.IsNullOrWhiteSpace(documentNumber))
            {
                return new PolicyMaturityValidationResult
                {
                    Success = false,
                    Message = "Policy number, document type, and document number are required",
                    ProcessedDate = DateTime.UtcNow,
                    ValidationStatus = "DocumentVerificationFailed",
                    DocumentsVerified = false,
                    Metadata = new Dictionary<string, string> { { "Reason", "MissingInput" } }
                };
            }

            var isValidDocument = false;
            switch (documentType.ToUpperInvariant())
            {
                case "PAN":
                    isValidDocument = documentNumber.Length == 10 && documentNumber.All(c => char.IsLetterOrDigit(c));
                    break;
                case "AADHAAR":
                    isValidDocument = documentNumber.Length == 12 && documentNumber.All(char.IsDigit);
                    break;
                case "PASSPORT":
                    isValidDocument = documentNumber.Length >= 8 && documentNumber.Length <= 12;
                    break;
                case "VOTERID":
                    isValidDocument = documentNumber.Length >= 10 && documentNumber.Length <= 15;
                    break;
                case "DRIVING_LICENSE":
                    isValidDocument = documentNumber.Length >= 10 && documentNumber.Length <= 20;
                    break;
                default:
                    isValidDocument = !string.IsNullOrWhiteSpace(documentNumber) && documentNumber.Length >= 5;
                    break;
            }

            var referenceId = string.Format("PMV-DOC-{0:D6}", ++_counter);
            var result = new PolicyMaturityValidationResult
            {
                Success = isValidDocument,
                Message = isValidDocument ? "Document verified successfully" : "Document verification failed - invalid format",
                ReferenceId = referenceId,
                ProcessedDate = DateTime.UtcNow,
                ValidationStatus = isValidDocument ? "DocumentVerified" : "DocumentRejected",
                DocumentsVerified = isValidDocument,
                Metadata = new Dictionary<string, string>
                {
                    { "PolicyNumber", policyNumber },
                    { "DocumentType", documentType },
                    { "DocumentNumber", documentNumber.Length > 4 ? documentNumber.Substring(0, 4) + "****" : "****" },
                    { "VerificationResult", isValidDocument ? "Pass" : "Fail" }
                }
            };

            if (!_validationHistory.ContainsKey(policyNumber))
                _validationHistory[policyNumber] = new List<PolicyMaturityValidationResult>();
            _validationHistory[policyNumber].Add(result);

            return result;
        }

        public bool IsPolicyMatured(string policyNumber, DateTime checkDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return false;

            if (!_maturityDates.ContainsKey(policyNumber))
            {
                _maturityDates[policyNumber] = DateTime.UtcNow.AddDays(-30);
            }

            return checkDate >= _maturityDates[policyNumber];
        }

        public bool IsKycComplete(string cifNumber)
        {
            if (string.IsNullOrWhiteSpace(cifNumber))
                return false;

            if (!_kycStatus.ContainsKey(cifNumber))
                return false;

            return _kycStatus[cifNumber] == "Verified";
        }

        public PolicyMaturityValidationResult CheckPremiumStatus(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new PolicyMaturityValidationResult
                {
                    Success = false,
                    Message = "Policy number required for premium status check",
                    ProcessedDate = DateTime.UtcNow,
                    ValidationStatus = "Failed",
                    PremiumStatus = "Unknown",
                    Metadata = new Dictionary<string, string>()
                };
            }

            var allPaid = _premiumStatus.ContainsKey(policyNumber) && _premiumStatus[policyNumber];
            var referenceId = string.Format("PMV-PRM-{0:D6}", ++_counter);

            return new PolicyMaturityValidationResult
            {
                Success = true,
                Message = allPaid ? "All premiums paid in full" : "Premium payments are pending",
                ReferenceId = referenceId,
                ProcessedDate = DateTime.UtcNow,
                ValidationStatus = allPaid ? "PremiumsClear" : "PremiumsPending",
                PremiumStatus = allPaid ? "AllPaid" : "Pending",
                Metadata = new Dictionary<string, string>
                {
                    { "PolicyNumber", policyNumber },
                    { "PremiumStatus", allPaid ? "Clear" : "Outstanding" }
                }
            };
        }

        public bool HasAllPremiumsPaid(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return false;

            return _premiumStatus.ContainsKey(policyNumber) && _premiumStatus[policyNumber];
        }

        public bool HasOutstandingLoan(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return false;

            return _loanStatus.ContainsKey(policyNumber) && _loanStatus[policyNumber];
        }

        public decimal GetOutstandingLoanAmount(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return 0m;

            return _loanAmounts.ContainsKey(policyNumber) ? _loanAmounts[policyNumber] : 0m;
        }

        public PolicyMaturityValidationResult ValidateNomineeDetails(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new PolicyMaturityValidationResult
                {
                    Success = false,
                    Message = "Policy number required for nominee validation",
                    ProcessedDate = DateTime.UtcNow,
                    ValidationStatus = "Failed",
                    Metadata = new Dictionary<string, string>()
                };
            }

            var hasNominee = _nomineeStatus.ContainsKey(policyNumber) && _nomineeStatus[policyNumber];
            var referenceId = string.Format("PMV-NOM-{0:D6}", ++_counter);

            return new PolicyMaturityValidationResult
            {
                Success = hasNominee,
                Message = hasNominee ? "Nominee details verified" : "Nominee details incomplete or missing",
                ReferenceId = referenceId,
                ProcessedDate = DateTime.UtcNow,
                ValidationStatus = hasNominee ? "NomineeVerified" : "NomineePending",
                Metadata = new Dictionary<string, string>
                {
                    { "PolicyNumber", policyNumber },
                    { "NomineeStatus", hasNominee ? "Complete" : "Incomplete" }
                }
            };
        }

        public PolicyMaturityValidationResult ValidateBankDetails(string cifNumber, string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(cifNumber) || string.IsNullOrWhiteSpace(accountNumber))
            {
                return new PolicyMaturityValidationResult
                {
                    Success = false,
                    Message = "CIF number and account number are required",
                    ProcessedDate = DateTime.UtcNow,
                    ValidationStatus = "BankValidationFailed",
                    Metadata = new Dictionary<string, string>()
                };
            }

            var isValidAccount = accountNumber.Length >= 9 && accountNumber.Length <= 18 && accountNumber.All(char.IsDigit);
            var referenceId = string.Format("PMV-BNK-{0:D6}", ++_counter);

            return new PolicyMaturityValidationResult
            {
                Success = isValidAccount,
                Message = isValidAccount ? "Bank account validated successfully" : "Invalid bank account number format",
                ReferenceId = referenceId,
                ProcessedDate = DateTime.UtcNow,
                ValidationStatus = isValidAccount ? "BankVerified" : "BankRejected",
                Metadata = new Dictionary<string, string>
                {
                    { "CifNumber", cifNumber },
                    { "AccountMasked", accountNumber.Length > 4 ? new string('*', accountNumber.Length - 4) + accountNumber.Substring(accountNumber.Length - 4) : "****" }
                }
            };
        }

        public bool IsWithinClaimWindow(string policyNumber, DateTime claimDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return false;

            if (!_maturityDates.ContainsKey(policyNumber))
                return false;

            var maturityDate = _maturityDates[policyNumber];
            var daysSinceMaturity = (claimDate - maturityDate).TotalDays;

            return daysSinceMaturity >= 0 && daysSinceMaturity <= 1095;
        }

        public int GetDaysToMaturity(string policyNumber, DateTime fromDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return 0;

            if (!_maturityDates.ContainsKey(policyNumber))
                return 0;

            var days = (int)(_maturityDates[policyNumber] - fromDate).TotalDays;
            return Math.Max(0, days);
        }

        public PolicyMaturityValidationResult GetValidationSummary(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new PolicyMaturityValidationResult
                {
                    Success = false,
                    Message = "Policy number required for validation summary",
                    ProcessedDate = DateTime.UtcNow,
                    ValidationStatus = "Failed",
                    Metadata = new Dictionary<string, string>()
                };
            }

            var hasKyc = _kycStatus.ContainsKey(policyNumber) && _kycStatus[policyNumber] == "Verified";
            var allPremiums = _premiumStatus.ContainsKey(policyNumber) && _premiumStatus[policyNumber];
            var hasLoan = _loanStatus.ContainsKey(policyNumber) && _loanStatus[policyNumber];
            var loanAmt = _loanAmounts.ContainsKey(policyNumber) ? _loanAmounts[policyNumber] : 0m;
            var hasNominee = _nomineeStatus.ContainsKey(policyNumber) && _nomineeStatus[policyNumber];
            var daysToMaturity = _maturityDates.ContainsKey(policyNumber)
                ? Math.Max(0, (int)(_maturityDates[policyNumber] - DateTime.UtcNow).TotalDays)
                : 0;

            var allClear = hasKyc && allPremiums && !hasLoan && hasNominee;
            var referenceId = string.Format("PMV-SUM-{0:D6}", ++_counter);

            return new PolicyMaturityValidationResult
            {
                Success = allClear,
                Message = allClear ? "All validations passed - ready for maturity processing" : "Some validations pending - review required",
                ReferenceId = referenceId,
                ProcessedDate = DateTime.UtcNow,
                ValidationStatus = allClear ? "AllClear" : "PendingReview",
                DocumentsVerified = hasKyc,
                PremiumStatus = allPremiums ? "AllPaid" : "Pending",
                KycStatus = hasKyc ? "Verified" : "NotVerified",
                OutstandingLoan = loanAmt,
                DaysToMaturity = daysToMaturity,
                Metadata = new Dictionary<string, string>
                {
                    { "PolicyNumber", policyNumber },
                    { "KycStatus", hasKyc ? "Complete" : "Pending" },
                    { "PremiumStatus", allPremiums ? "Clear" : "Outstanding" },
                    { "LoanStatus", hasLoan ? "Outstanding" : "Clear" },
                    { "NomineeStatus", hasNominee ? "Verified" : "Pending" },
                    { "OverallStatus", allClear ? "ReadyForProcessing" : "ReviewRequired" }
                }
            };
        }

        public List<PolicyMaturityValidationResult> GetValidationHistory(string policyNumber, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                return new List<PolicyMaturityValidationResult>();

            if (!_validationHistory.ContainsKey(policyNumber))
                return new List<PolicyMaturityValidationResult>();

            return _validationHistory[policyNumber]
                .Where(r => r.ProcessedDate >= fromDate && r.ProcessedDate <= toDate)
                .OrderByDescending(r => r.ProcessedDate)
                .ToList();
        }

        public bool ValidateClaimantIdentity(string cifNumber, string panNumber, DateTime dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(cifNumber) || string.IsNullOrWhiteSpace(panNumber))
                return false;

            if (panNumber.Length != 10)
                return false;

            for (int i = 0; i < 5; i++)
            {
                if (!char.IsLetter(panNumber[i]))
                    return false;
            }
            for (int i = 5; i < 9; i++)
            {
                if (!char.IsDigit(panNumber[i]))
                    return false;
            }
            if (!char.IsLetter(panNumber[9]))
                return false;

            if (dateOfBirth > DateTime.UtcNow.AddYears(-18))
                return false;

            if (_panRecords.ContainsKey(cifNumber) && _panRecords[cifNumber] != panNumber)
                return false;

            if (_dobRecords.ContainsKey(cifNumber) && _dobRecords[cifNumber].Date != dateOfBirth.Date)
                return false;

            _panRecords[cifNumber] = panNumber;
            _dobRecords[cifNumber] = dateOfBirth;

            return true;
        }
    }
}
