using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs
{
    /// <summary>Validates legal heir documentation when no nominee is present.</summary>
    public interface ILegalHeirValidationService
    {
        bool ValidateSuccessionCertificate(string certificateId, DateTime issueDate);
        
        bool VerifyIndemnityBond(string bondId, decimal bondAmount);
        
        int CalculateDaysSinceDeath(DateTime dateOfDeath, DateTime claimDate);
        
        decimal CalculateHeirShareAmount(decimal totalBenefitAmount, double heirSharePercentage);
        
        double GetStatutorySharePercentage(string relationshipCode, int totalHeirs);
        
        string GenerateHeirReferenceId(string policyNumber, string nationalId);
        
        bool CheckMinorHeirStatus(DateTime dateOfBirth, DateTime claimDate);
        
        decimal CalculateGuardianshipBondAmount(decimal minorShareAmount, double riskFactor);
        
        int GetRequiredAffidavitCount(string claimType, double totalBenefitValue);
        
        string RetrieveCourtOrderCode(string courtName, DateTime orderDate);
        
        bool ValidateNotarySignature(string notaryId, DateTime notarizationDate);
        
        double CalculateDisputedShareRatio(int disputingHeirs, double totalDisputedPercentage);
        
        decimal ComputeTaxWithholdingForHeir(decimal shareAmount, string taxCategoryCode);
        
        bool IsRelinquishmentDeedValid(string deedId, string relinquishingHeirId, string benefitingHeirId);
        
        int GetPendingDocumentCount(string claimId, string heirId);
        
        string DetermineNextActionCode(bool isDisputed, decimal totalClaimAmount);
        
        bool VerifyFamilyTreeDocument(string documentId, int declaredHeirCount);
        
        decimal GetMaximumAllowedWithoutProbate(string stateCode, DateTime dateOfDeath);
    }
}