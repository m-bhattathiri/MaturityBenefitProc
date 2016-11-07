using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.ReinsuranceAndPoolManagement
{
    /// <summary>Sends automated bordereaux and claim notifications to reinsurance partners.</summary>
    public interface IReinsurerNotificationService
    {
        bool SendBordereauReport(string reinsurerId, DateTime periodStart, DateTime periodEnd);
        
        string GenerateClaimNotificationId(string policyNumber, string reinsurerId);
        
        decimal CalculateReinsurerShare(decimal totalClaimAmount, double retentionRate);
        
        double GetTreatyParticipationPercentage(string treatyId, DateTime effectiveDate);
        
        int CountPendingNotifications(string reinsurerId);
        
        bool NotifyLargeClaim(string claimId, decimal claimAmount, DateTime dateOfLoss);
        
        decimal ComputeCededPremium(decimal grossPremium, double commissionRate);
        
        string GetPrimaryContactEmail(string reinsurerId);
        
        int GetDaysSinceLastBordereau(string reinsurerId);
        
        bool ValidateTreatyLimits(string treatyId, decimal cededAmount);
        
        double CalculateLossRatio(decimal incurredLosses, decimal earnedPremiums);
        
        string SubmitCashCall(string reinsurerId, decimal requestedAmount, DateTime dueDate);
        
        decimal GetTotalRecoverables(string reinsurerId, DateTime asOfDate);
        
        bool AcknowledgeNotificationReceipt(string notificationId, string responseCode);
        
        int GetActiveTreatyCount(string reinsurerId);
        
        string RetrieveReinsurerRating(string reinsurerId);
        
        bool TriggerCatastropheAlert(string eventCode, int estimatedClaimsCount, decimal estimatedGrossLoss);
        
        decimal CalculateReinstatementPremium(decimal lossAmount, double annualRate, int daysRemaining);
    }
}