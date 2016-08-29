using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    /// <summary>Verifies and clears assignments or pledges against the policy before payout.</summary>
    public interface IPolicyPledgeService
    {
        bool HasActivePledge(string policyId);
        
        decimal CalculateTotalPledgedAmount(string policyId, DateTime effectiveDate);
        
        string GetPrimaryAssigneeId(string policyId);
        
        int GetDaysSincePledgeInitiation(string pledgeId);
        
        double GetPledgeInterestRate(string pledgeId);
        
        bool VerifyAssignmentClearance(string policyId, string assigneeId);
        
        decimal GetOutstandingLoanBalance(string policyId);
        
        string InitiatePledgeClearance(string policyId, decimal clearanceAmount, DateTime requestDate);
        
        int CountActiveAssignments(string policyId);
        
        double CalculatePledgeToValueRatio(string policyId, decimal currentCashValue);
        
        bool IsPolicyEligibleForPayout(string policyId);
        
        decimal ComputeRecoveryAmount(string policyId, double penaltyRate);
        
        string GenerateClearanceCertificate(string policyId, string assigneeId);
        
        int GetRemainingLockInPeriodDays(string pledgeId);
        
        double GetAssignmentSharePercentage(string policyId, string assigneeId);
        
        bool ReleasePledge(string pledgeId, string authorizationCode);
        
        decimal CalculateAccruedInterest(string pledgeId, DateTime calculationDate);
        
        string GetPledgeStatusCode(string pledgeId);
    }
}