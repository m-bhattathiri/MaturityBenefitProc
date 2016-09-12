using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions
{
    /// <summary>Computes loyalty additions for policies completing their full premium term.</summary>
    public interface ILoyaltyAdditionService
    {
        decimal CalculateBaseLoyaltyAddition(string policyId, decimal sumAssured, int premiumTerm);
        
        double GetLoyaltyAdditionRate(string productCode, int completedYears);
        
        bool IsEligibleForLoyaltyAddition(string policyId, DateTime maturityDate);
        
        int GetCompletedPremiumYears(string policyId, DateTime inceptionDate);
        
        string GenerateLoyaltyTransactionId(string policyId, DateTime processingDate);
        
        decimal ComputeFinalLoyaltyAmount(string policyId, decimal baseAmount, double multiplier);
        
        bool HasCompletedPremiumTerm(string policyId, int requiredTerm);
        
        double CalculateLoyaltyMultiplier(int persistencyScore, double baseRate);
        
        decimal GetAccruedLoyaltyValue(string policyId, DateTime calculationDate);
        
        int CalculateDaysSinceLastAddition(string policyId, DateTime currentDate);
        
        string GetLoyaltyFundCode(string productCategory, int issueYear);
        
        bool ValidateLoyaltyAdditionRules(string policyId, decimal calculatedAmount);
        
        decimal CalculateProratedAddition(string policyId, decimal annualAmount, int daysActive);
        
        double GetBonusInterestRate(string fundCode, DateTime effectiveDate);
        
        int GetMissedPremiumCount(string policyId);
        
        string DetermineLoyaltyCategory(int completedYears, decimal sumAssured);
        
        decimal GetSurrenderLoyaltyValue(string policyId, decimal totalPremiumsPaid, double surrenderFactor);
        
        bool IsPolicyInForce(string policyId, DateTime checkDate);
        
        double ComputePersistencyRatio(string policyId, int expectedPremiums, int paidPremiums);
        
        int GetLoyaltyTierLevel(double persistencyRatio);
        
        decimal CalculateSpecialLoyaltyBonus(string policyId, decimal baseLoyalty, bool isHighValuePolicy);
        
        bool CheckSpecialBonusEligibility(string policyId, int tierLevel);
        
        string GetApprovalStatusCode(decimal finalLoyaltyAmount, int tierLevel);
        
        decimal ApplyTaxDeductionsToLoyalty(string policyId, decimal grossLoyaltyAmount, double taxRate);
    }
}