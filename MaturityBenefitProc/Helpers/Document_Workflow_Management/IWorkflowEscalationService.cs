using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement
{
    /// <summary>Escalates pending maturity cases approaching regulatory turnaround times.</summary>
    public interface IWorkflowEscalationService
    {
        bool EvaluateEscalationEligibility(string caseId, DateTime submissionDate);
        int CalculateRemainingSlaDays(string caseId, DateTime regulatoryDeadline);
        string TriggerManagerEscalation(string caseId, int escalationLevel);
        decimal CalculatePotentialRegulatoryPenalty(string caseId, int daysOverdue);
        double GetEscalationRiskScore(string caseId, double currentProcessingRate);
        bool ReassignToPriorityQueue(string caseId, string targetQueueId);
        string GetCurrentWorkflowState(string caseId);
        int GetEscalationCount(string policyNumber);
        decimal ComputeInterestOnDelayedMaturity(decimal baseAmount, double penaltyRate, int delayedDays);
        bool NotifyComplianceOfficer(string caseId, string officerId, DateTime notificationDate);
        double CalculateDepartmentSlaComplianceRate(string departmentId, DateTime startDate, DateTime endDate);
        string GenerateEscalationTicket(string caseId, string reasonCode);
        int GetPendingMaturityCasesCount(string queueId);
        decimal GetTotalAtRiskMaturityValue(string departmentId);
        bool FlagForExpeditedProcessing(string caseId, bool requiresDirectorApproval);
        string ResolveEscalation(string ticketId, string resolutionCode);
        double GetAverageResolutionTimeHours(string queueId);
        int DetermineEscalationLevel(int daysPending, double riskFactor);
        decimal AdjustMaturityPayoutForDelay(string caseId, decimal originalPayout);
        bool ValidateRegulatoryTurnaroundTime(string caseId, DateTime processingDate);
        string AssignDedicatedHandler(string caseId, string handlerId);
        int GetDaysUntilRegulatoryBreach(string caseId);
        double CalculateEscalationProbability(string caseId, int documentDeficiencyCount);
        bool SuspendAutoEscalation(string caseId, string overrideReason);
    }
}