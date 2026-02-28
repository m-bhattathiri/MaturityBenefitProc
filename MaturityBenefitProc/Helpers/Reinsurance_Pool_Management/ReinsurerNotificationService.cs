// Fixed implementation — correct business logic

using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Helpers.ReinsuranceAndPoolManagement
{
    public class ReinsurerNotificationService : IReinsurerNotificationService
    {
        private const decimal LARGE_CLAIM_THRESHOLD = 500000m;
        private const decimal MAX_TREATY_LIMIT = 10000000m;

        public bool SendBordereauReport(string reinsurerId, DateTime periodStart, DateTime periodEnd)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId))
                throw new ArgumentException("Reinsurer ID cannot be null or empty.", nameof(reinsurerId));

            if (periodStart >= periodEnd)
                return false;

            // Simulate sending bordereau report logic
            Console.WriteLine($"Sending Bordereau for {reinsurerId} from {periodStart.ToShortDateString()} to {periodEnd.ToShortDateString()}");
            return true;
        }

        public string GenerateClaimNotificationId(string policyNumber, string reinsurerId)
        {
            if (string.IsNullOrWhiteSpace(policyNumber) || string.IsNullOrWhiteSpace(reinsurerId))
                return null;

            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            return $"CN-{reinsurerId.ToUpper()}-{policyNumber}-{timestamp}";
        }

        public decimal CalculateReinsurerShare(decimal totalClaimAmount, double retentionRate)
        {
            if (totalClaimAmount < 0)
                throw new ArgumentOutOfRangeException(nameof(totalClaimAmount), "Claim amount cannot be negative.");

            if (retentionRate < 0.0 || retentionRate > 1.0)
                throw new ArgumentOutOfRangeException(nameof(retentionRate), "Retention rate must be between 0.0 and 1.0.");

            decimal reinsurerRate = 1m - (decimal)retentionRate;
            return Math.Round(totalClaimAmount * reinsurerRate, 2);
        }

        public double GetTreatyParticipationPercentage(string treatyId, DateTime effectiveDate)
        {
            if (string.IsNullOrWhiteSpace(treatyId))
                return 0.0;

            // Mock logic: older treaties have higher participation
            if (effectiveDate.Year < 2020)
                return 0.25; // 25%
            
            return 0.15; // 15%
        }

        public int CountPendingNotifications(string reinsurerId)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId))
                return 0;

            // Mock database lookup based on reinsurer ID length
            return reinsurerId.Length * 3;
        }

        public bool NotifyLargeClaim(string claimId, decimal claimAmount, DateTime dateOfLoss)
        {
            if (string.IsNullOrWhiteSpace(claimId) || claimAmount <= 0)
                return false;

            if (claimAmount >= LARGE_CLAIM_THRESHOLD)
            {
                // Simulate sending high-priority notification
                Console.WriteLine($"URGENT: Large claim {claimId} reported for {claimAmount:C} on {dateOfLoss.ToShortDateString()}");
                return true;
            }

            return false;
        }

        public decimal ComputeCededPremium(decimal grossPremium, double commissionRate)
        {
            if (grossPremium < 0)
                return 0m;

            if (commissionRate < 0.0 || commissionRate > 1.0)
                commissionRate = 0.0;

            decimal commissionAmount = grossPremium * (decimal)commissionRate;
            return Math.Round(grossPremium - commissionAmount, 2);
        }

        public string GetPrimaryContactEmail(string reinsurerId)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId))
                return "default-reinsurance@company.com";

            return $"contact@{reinsurerId.ToLower().Replace(" ", "")}.com";
        }

        public int GetDaysSinceLastBordereau(string reinsurerId)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId))
                return -1;

            // Mock logic: simulate last bordereau was sent on the 1st of the current month
            DateTime lastBordereauDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return (DateTime.UtcNow - lastBordereauDate).Days;
        }

        public bool ValidateTreatyLimits(string treatyId, decimal cededAmount)
        {
            if (string.IsNullOrWhiteSpace(treatyId) || cededAmount < 0)
                return false;

            // In a real system, MAX_TREATY_LIMIT would be fetched from a database using treatyId
            return cededAmount <= MAX_TREATY_LIMIT;
        }

        public double CalculateLossRatio(decimal incurredLosses, decimal earnedPremiums)
        {
            if (earnedPremiums == 0m)
                return 0.0;

            double ratio = (double)(incurredLosses / earnedPremiums);
            return Math.Round(ratio, 4);
        }

        public string SubmitCashCall(string reinsurerId, decimal requestedAmount, DateTime dueDate)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId) || requestedAmount <= 0)
                throw new ArgumentException("Invalid cash call parameters.");

            if (dueDate <= DateTime.UtcNow)
                throw new ArgumentException("Due date must be in the future.");

            string reference = $"CC-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
            Console.WriteLine($"Cash call {reference} submitted to {reinsurerId} for {requestedAmount:C}, due {dueDate.ToShortDateString()}");
            
            return reference;
        }

        public decimal GetTotalRecoverables(string reinsurerId, DateTime asOfDate)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId))
                return 0m;

            // Mock calculation
            int seed = reinsurerId.GetHashCode() + asOfDate.DayOfYear;
            Random rand = new Random(seed);
            return Math.Round((decimal)(rand.NextDouble() * 5000000), 2);
        }

        public bool AcknowledgeNotificationReceipt(string notificationId, string responseCode)
        {
            if (string.IsNullOrWhiteSpace(notificationId))
                return false;

            return responseCode == "ACK-200" || responseCode == "SUCCESS";
        }

        public int GetActiveTreatyCount(string reinsurerId)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId))
                return 0;

            return reinsurerId.Length % 5 + 1; // Mock 1 to 5 active treaties
        }

        public string RetrieveReinsurerRating(string reinsurerId)
        {
            if (string.IsNullOrWhiteSpace(reinsurerId))
                return "Unrated";

            // Mock rating logic
            int hash = Math.Abs(reinsurerId.GetHashCode());
            if (hash % 3 == 0) return "A+";
            if (hash % 3 == 1) return "A";
            return "A-";
        }

        public bool TriggerCatastropheAlert(string eventCode, int estimatedClaimsCount, decimal estimatedGrossLoss)
        {
            if (string.IsNullOrWhiteSpace(eventCode))
                return false;

            if (estimatedClaimsCount > 100 || estimatedGrossLoss > 5000000m)
            {
                Console.WriteLine($"CAT ALERT TRIGGERED: {eventCode}. Est Claims: {estimatedClaimsCount}, Est Loss: {estimatedGrossLoss:C}");
                return true;
            }

            return false;
        }

        public decimal CalculateReinstatementPremium(decimal lossAmount, double annualRate, int daysRemaining)
        {
            if (lossAmount <= 0 || annualRate <= 0 || daysRemaining <= 0)
                return 0m;

            if (daysRemaining > 365)
                daysRemaining = 365;

            decimal proRataFactor = daysRemaining / 365m;
            decimal premium = lossAmount * (decimal)annualRate * proRataFactor;
            
            return Math.Round(premium, 2);
        }
    }
}