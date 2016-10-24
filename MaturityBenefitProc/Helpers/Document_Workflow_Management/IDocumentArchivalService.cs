using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.DocumentAndWorkflowManagement
{
    /// <summary>Archives processed maturity claim documents to the enterprise repository.</summary>
    public interface IDocumentArchivalService
    {
        string ArchiveClaimDocument(string policyId, string documentType, DateTime processingDate);
        
        bool ValidateArchivalEligibility(string documentId, int minimumRetentionDays);
        
        int GetArchivedDocumentCount(string policyId, DateTime startDate, DateTime endDate);
        
        decimal CalculateArchivalStorageCost(string documentId, decimal baseStorageRate);
        
        double GetCompressionRatio(string documentId);
        
        bool VerifyDocumentIntegrity(string archiveId, string expectedHashValue);
        
        string RetrieveArchiveLocation(string documentId);
        
        int CalculateRemainingRetentionDays(string archiveId, DateTime currentDate);
        
        decimal EstimateBatchArchivalCost(int documentCount, decimal costPerMegabyte);
        
        double GetArchivalSuccessRate(DateTime processingDate);
        
        bool PurgeExpiredDocument(string archiveId, DateTime expirationDate);
        
        string GenerateArchiveReferenceCode(string policyId, int sequenceNumber);
        
        int GetPendingArchivalQueueSize(string regionCode);
        
        decimal CalculatePenaltyForLateArchival(int daysLate, decimal dailyPenaltyRate);
        
        double GetStorageUtilizationPercentage(string repositoryId);
        
        bool UpdateDocumentMetadata(string archiveId, string newStatus, DateTime updateDate);
        
        string ReindexArchivedDocument(string archiveId, int priorityLevel);
    }
}