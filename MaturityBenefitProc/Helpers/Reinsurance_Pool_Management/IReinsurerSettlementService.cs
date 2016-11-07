using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.Reinsurance_Pool_Management
{
    /// <summary>
    /// Reconciles and processes incoming payments from reinsurers for settled claims.
    /// </summary>
    public interface IReinsurerSettlementService
    {
        bool ValidateSettlementBatch(string batchId, DateTime receivedDate);
        decimal CalculateTotalExpectedRecovery(string reinsurerId, DateTime startDate, DateTime endDate);
        decimal ProcessIncomingPayment(string paymentId, string reinsurerId, decimal amountReceived);
        int GetPendingSettlementCount(string reinsurerId);
        string GenerateSettlementReceipt(string paymentId, string reinsurerId);
        double CalculateRecoveryVariancePercentage(decimal expectedAmount, decimal actualAmount);
        bool IsReinsurerEligibleForDiscount(string reinsurerId, DateTime paymentDate);
        decimal ApplyEarlySettlementDiscount(decimal originalAmount, double discountRate);
        int CalculateDaysOutstanding(string claimId, DateTime settlementDate);
        string MatchPaymentToClaim(string paymentReference, decimal amount);
        bool VerifyWireTransferDetails(string transactionId, string bankRoutingNumber);
        decimal CalculateCurrencyExchangeAdjustment(decimal baseAmount, double exchangeRate);
        double GetCurrentExchangeRate(string sourceCurrency, string targetCurrency);
        int GetUnmatchedPaymentCount(string batchId);
        string FlagDiscrepancy(string claimId, string reinsurerId, string reasonCode);
        bool ResolveDiscrepancy(string discrepancyId, string resolutionCode);
        decimal CalculateLatePaymentPenalty(decimal principalAmount, int daysLate, double penaltyRate);
        string AllocateFundsToPool(string poolId, decimal amount);
        bool ValidateTreatyLimits(string reinsurerId, string treatyId, decimal settlementAmount);
        decimal GetRemainingTreatyCapacity(string treatyId);
        double CalculateReinsurerShareRatio(string claimId, string reinsurerId);
        int GetSettledClaimsCount(string batchId);
        string InitiateRefundProcess(string reinsurerId, decimal overpaymentAmount);
        bool ApproveSettlementBatch(string batchId, string approverId);
        decimal CalculateTotalPoolContribution(string poolId, DateTime periodStart, DateTime periodEnd);
    }
}