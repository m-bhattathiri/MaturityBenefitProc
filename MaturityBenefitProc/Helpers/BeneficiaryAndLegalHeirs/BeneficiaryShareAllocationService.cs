using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    // Fixed implementation — correct business logic
    public class BeneficiaryShareAllocationService : IBeneficiaryShareAllocationService
    {
        private const int LegalAgeOfMajority = 18;
        private const decimal DisputeReservePercentagePerHeir = 0.05m;
        private const decimal MaxDisputeReservePercentage = 0.50m;
        
        public decimal CalculateBasePayoutAmount(string policyId, decimal totalBenefit)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentNullException(nameof(policyId));
            if (totalBenefit < 0) throw new ArgumentOutOfRangeException(nameof(totalBenefit), "Benefit cannot be negative.");
            
            // In a real system, this might deduct standard administrative fees first
            decimal adminFee = 50.00m;
            return Math.Max(0, totalBenefit - adminFee);
        }

        public decimal CalculateBeneficiaryShareAmount(decimal totalAmount, double sharePercentage)
        {
            if (sharePercentage < 0 || sharePercentage > 100) 
                throw new ArgumentOutOfRangeException(nameof(sharePercentage), "Share percentage must be between 0 and 100.");
                
            return Math.Round(totalAmount * (decimal)(sharePercentage / 100.0), 2);
        }

        public decimal ApplyTaxDeductionToShare(decimal shareAmount, double taxRate)
        {
            if (taxRate < 0 || taxRate > 1) 
                throw new ArgumentOutOfRangeException(nameof(taxRate), "Tax rate must be expressed as a decimal between 0 and 1.");
                
            decimal taxAmount = Math.Round(shareAmount * (decimal)taxRate, 2);
            return Math.Max(0, shareAmount - taxAmount);
        }

        public decimal CalculateLateInterest(decimal baseAmount, double interestRate, int daysLate)
        {
            if (daysLate <= 0) return 0m;
            
            // Simple interest calculation: Principal * Rate * (Days / 365)
            decimal interest = baseAmount * (decimal)interestRate * (daysLate / 365.0m);
            return Math.Round(interest, 2);
        }

        public decimal GetTotalAllocatedFunds(string policyId)
        {
            // Mock database retrieval
            return 150000.00m; 
        }

        public decimal CalculateLegalHeirDisputeReserve(decimal totalBenefit, int disputingHeirsCount)
        {
            if (disputingHeirsCount <= 0) return 0m;
            
            decimal reservePercentage = Math.Min(MaxDisputeReservePercentage, disputingHeirsCount * DisputeReservePercentagePerHeir);
            return Math.Round(totalBenefit * reservePercentage, 2);
        }

        public decimal AdjustPayoutForOutstandingPremiums(decimal payoutAmount, decimal outstandingDebt)
        {
            if (outstandingDebt < 0) return payoutAmount;
            return Math.Max(0, payoutAmount - outstandingDebt);
        }

        public double GetPrimaryBeneficiarySharePercentage(string beneficiaryId)
        {
            // Mock lookup
            return string.IsNullOrEmpty(beneficiaryId) ? 0.0 : 50.0;
        }

        public double GetContingentBeneficiarySharePercentage(string beneficiaryId)
        {
            // Mock lookup
            return string.IsNullOrEmpty(beneficiaryId) ? 0.0 : 25.0;
        }

        public double CalculateRemainingSharePool(string policyId)
        {
            // Mock logic: assuming 75% is already allocated
            double allocatedShares = 75.0; 
            return Math.Max(0.0, 100.0 - allocatedShares);
        }

        public double AdjustShareForMinorBeneficiary(double originalShare, int age)
        {
            // If minor, their direct share might be restricted or placed in trust, but percentage remains the same
            // In some business rules, it might be capped or deferred. Returning original for now.
            if (age < 0) throw new ArgumentException("Age cannot be negative.");
            return originalShare;
        }

        public double GetApplicableTaxRate(string beneficiaryId, string stateCode)
        {
            if (string.IsNullOrWhiteSpace(stateCode)) return 0.0;
            
            // Mock state tax rates
            if (stateCode.ToUpper() == "CA")
            {
                return 0.08;
            }
            else if (stateCode.ToUpper() == "NY")
            {
                return 0.06;
            }
            else if (stateCode.ToUpper() == "TX")
            {
                return 0.00;
            }
            else if (stateCode.ToUpper() == "FL")
            {
                return 0.00;
            }
            else
            {
                return 0.05;
            }
        }

        public bool ValidateTotalSharesEqualOneHundredPercent(string policyId)
        {
            // Mock validation
            double totalShares = 100.0; // Would sum from DB
            return Math.Abs(totalShares - 100.0) < 0.001;
        }

        public bool IsBeneficiaryEligibleForPayout(string beneficiaryId, DateTime dateOfDeath)
        {
            if (string.IsNullOrWhiteSpace(beneficiaryId)) return false;
            return dateOfDeath <= DateTime.UtcNow;
        }

        public bool HasLegalHeirDisputes(string policyId)
        {
            // Mock check
            return !string.IsNullOrWhiteSpace(policyId) && policyId.StartsWith("DISP");
        }

        public bool RequiresGuardianSignoff(string beneficiaryId, DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            
            return age < LegalAgeOfMajority;
        }

        public bool VerifyBankRoutingInformation(string routingNumber)
        {
            if (string.IsNullOrWhiteSpace(routingNumber) || routingNumber.Length != 9) return false;
            return routingNumber.All(char.IsDigit);
        }

        public bool CheckIfBeneficiaryIsDeceased(string beneficiaryId)
        {
            // Mock check
            return beneficiaryId.EndsWith("-DEC");
        }

        public int GetActiveBeneficiaryCount(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) return 0;
            return 3; // Mock count
        }

        public int GetDaysSinceMaturity(DateTime maturityDate)
        {
            if (maturityDate > DateTime.UtcNow) return 0;
            return (DateTime.UtcNow - maturityDate).Days;
        }

        public int CountEligibleLegalHeirs(string policyId)
        {
            return string.IsNullOrWhiteSpace(policyId) ? 0 : 2; // Mock count
        }

        public int GetProcessingSlaDays(string policyType)
        {
            if (policyType?.ToUpper() == "LIFE")
            {
                return 15;
            }
            else if (policyType?.ToUpper() == "ANNUITY")
            {
                return 10;
            }
            else if (policyType?.ToUpper() == "ENDOWMENT")
            {
                return 20;
            }
            else
            {
                return 30;
            }
        }

        public string GenerateAllocationTransactionId(string policyId, string beneficiaryId)
        {
            if (string.IsNullOrWhiteSpace(policyId) || string.IsNullOrWhiteSpace(beneficiaryId))
                throw new ArgumentException("Policy ID and Beneficiary ID are required.");
                
            return $"TXN-{policyId}-{beneficiaryId}-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        public string GetPayoutStatusCode(string beneficiaryId)
        {
            return "PENDING_DOCS"; // Mock status
        }

        public string DetermineTaxFormRequirement(decimal payoutAmount, bool isForeignNational)
        {
            if (isForeignNational) return "W-8BEN";
            if (payoutAmount >= 600m) return "1099-MISC";
            if (payoutAmount >= 10m) return "1099-INT";
            return "NONE";
        }

        public string GetGuardianIdForMinor(string minorBeneficiaryId)
        {
            if (string.IsNullOrWhiteSpace(minorBeneficiaryId)) return null;
            return $"GDN-{minorBeneficiaryId}"; // Mock ID
        }

        public string ResolveDisputedShareHoldCode(string policyId)
        {
            return HasLegalHeirDisputes(policyId) ? "HOLD-LEGAL-DISPUTE" : "CLEARED";
        }
    }
}