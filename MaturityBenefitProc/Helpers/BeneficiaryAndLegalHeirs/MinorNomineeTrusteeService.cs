// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    public class MinorNomineeTrusteeService : IMinorNomineeTrusteeService
    {
        private const int LegalMajorityAge = 18;
        private const decimal MaxAppointeePayoutLimit = 5000000m; // 5 Million limit without special approval
        private readonly HashSet<string> _validRelationshipCodes = new HashSet<string> { "FATH", "MOTH", "LGUAR", "CGUAR" };

        public bool IsNomineeMinor(string nomineeId, DateTime dateOfBirth, DateTime maturityDate)
        {
            if (string.IsNullOrWhiteSpace(nomineeId)) throw new ArgumentException("Nominee ID required.");
            if (dateOfBirth > maturityDate) return false;

            int age = maturityDate.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > maturityDate.AddYears(-age)) age--;

            return age < LegalMajorityAge;
        }

        public bool ValidateAppointeeEligibility(string appointeeId, string relationshipCode)
        {
            if (string.IsNullOrWhiteSpace(appointeeId) || string.IsNullOrWhiteSpace(relationshipCode))
                return false;

            return _validRelationshipCodes.Contains(relationshipCode.ToUpperInvariant());
        }

        public bool HasActiveTrusteeMandate(string policyId, string nomineeId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(nomineeId))
                return false;
            
            // Simulated DB check
            return policyId.StartsWith("POL") && nomineeId.StartsWith("NOM");
        }

        public bool IsPayoutAllowedToAppointee(string appointeeId, decimal payoutAmount)
        {
            if (string.IsNullOrWhiteSpace(appointeeId) || payoutAmount <= 0) return false;
            return payoutAmount <= MaxAppointeePayoutLimit && VerifyAppointeeKycStatus(appointeeId);
        }

        public bool VerifyAppointeeKycStatus(string appointeeId)
        {
            if (string.IsNullOrWhiteSpace(appointeeId)) return false;
            // Simulated KYC check
            return appointeeId.Length >= 5;
        }

        public decimal CalculateAppointeePayoutShare(decimal totalMaturityAmount, double sharePercentage)
        {
            if (totalMaturityAmount < 0) throw new ArgumentException("Amount cannot be negative.");
            if (sharePercentage < 0 || sharePercentage > 100) throw new ArgumentException("Invalid share percentage.");

            return Math.Round(totalMaturityAmount * (decimal)(sharePercentage / 100.0), 2);
        }

        public decimal GetTotalDisbursedToAppointee(string appointeeId, string policyId)
        {
            // Simulated DB retrieval
            return string.IsNullOrWhiteSpace(appointeeId) ? 0m : 15000.50m;
        }

        public decimal CalculateTrusteeMaintenanceAllowance(decimal baseAmount, int durationInMonths)
        {
            if (baseAmount <= 0 || durationInMonths <= 0) return 0m;
            decimal monthlyAllowanceRate = 0.005m; // 0.5% per month
            return Math.Round(baseAmount * monthlyAllowanceRate * durationInMonths, 2);
        }

        public decimal ComputeMinorEducationFundAllocation(decimal totalBenefit, double allocationRate)
        {
            if (totalBenefit <= 0 || allocationRate <= 0) return 0m;
            double effectiveRate = Math.Min(allocationRate, 1.0); // Cap at 100%
            return Math.Round(totalBenefit * (decimal)effectiveRate, 2);
        }

        public decimal GetPendingPayoutAmount(string nomineeId, string policyId)
        {
            // Simulated calculation
            return 25000.00m;
        }

        public double GetAppointeeSharePercentage(string nomineeId, string appointeeId)
        {
            if (string.IsNullOrWhiteSpace(nomineeId) || string.IsNullOrWhiteSpace(appointeeId)) return 0.0;
            return 100.0; // Default to 100% for primary appointee
        }

        public double CalculateTrusteeFeeRate(int managementDurationDays)
        {
            if (managementDurationDays <= 0) return 0.0;
            if (managementDurationDays < 365) return 0.01; // 1%
            if (managementDurationDays < 1825) return 0.02; // 2%
            return 0.03; // 3%
        }

        public double GetMinorAgeRatioAtMaturity(DateTime dateOfBirth, DateTime maturityDate)
        {
            if (dateOfBirth >= maturityDate) return 0.0;
            double daysLived = (maturityDate - dateOfBirth).TotalDays;
            double daysToMajority = LegalMajorityAge * 365.25;
            return Math.Min(daysLived / daysToMajority, 1.0);
        }

        public double GetTaxDeductionRateForAppointee(string appointeeId, string taxCategoryCode)
        {
            if (string.IsNullOrWhiteSpace(taxCategoryCode)) return 0.10; // Default 10%
            
            switch (taxCategoryCode.ToUpperInvariant())
            {
                case "EXEMPT": return 0.0;
                case "STANDARD": return 0.10;
                case "HIGH": return 0.20;
                default: return 0.15;
            }
        }

        public int GetDaysUntilNomineeMajority(DateTime dateOfBirth, DateTime currentDate)
        {
            DateTime majorityDate = dateOfBirth.AddYears(LegalMajorityAge);
            if (currentDate >= majorityDate) return 0;
            return (int)(majorityDate - currentDate).TotalDays;
        }

        public int CountActiveAppointeesForMinor(string nomineeId)
        {
            if (string.IsNullOrWhiteSpace(nomineeId)) return 0;
            return 1; // Simulated count
        }

        public int GetTrusteeMandateDurationMonths(DateTime mandateStartDate, DateTime mandateEndDate)
        {
            if (mandateEndDate <= mandateStartDate) return 0;
            return ((mandateEndDate.Year - mandateStartDate.Year) * 12) + mandateEndDate.Month - mandateStartDate.Month;
        }

        public int GetNumberOfInstallmentsProcessed(string appointeeId, string policyId)
        {
            return string.IsNullOrWhiteSpace(appointeeId) ? 0 : 3; // Simulated
        }

        public int GetAppointeeAge(DateTime appointeeDob, DateTime currentDate)
        {
            if (appointeeDob > currentDate) return 0;
            int age = currentDate.Year - appointeeDob.Year;
            if (appointeeDob.Date > currentDate.AddYears(-age)) age--;
            return age;
        }

        public string RegisterNewAppointee(string nomineeId, string firstName, string lastName, DateTime dob)
        {
            if (string.IsNullOrWhiteSpace(nomineeId) || string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Invalid appointee details.");
            
            return $"APP-{Guid.NewGuid().ToString().Substring(0, 8).ToUpperInvariant()}";
        }

        public string GetPrimaryAppointeeId(string nomineeId)
        {
            if (string.IsNullOrWhiteSpace(nomineeId)) return null;
            return $"APP-{nomineeId}-01";
        }

        public string GenerateTrusteeMandateReference(string policyId, string appointeeId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(appointeeId)) return null;
            return $"MANDATE-{policyId}-{appointeeId}-{DateTime.UtcNow:yyyyMMdd}";
        }

        public string GetAppointeeRelationshipCode(string nomineeId, string appointeeId)
        {
            return "FATH"; // Simulated
        }

        public string RetrieveAppointeeBankAccountId(string appointeeId)
        {
            if (string.IsNullOrWhiteSpace(appointeeId)) return null;
            return "ACCT-9988776655"; // Simulated DB lookup
        }

        public string ResolvePayoutStatusCategory(string policyId, string appointeeId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(appointeeId))
                return "INVALID";

            return VerifyAppointeeKycStatus(appointeeId) ? "READY_FOR_DISBURSEMENT" : "PENDING_KYC";
        }
    }
}