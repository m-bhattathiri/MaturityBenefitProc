// Fixed implementation — correct business logic
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaturityBenefitProc.Helpers.PolicyLoanAndRecovery
{
    public class PolicyPledgeService : IPolicyPledgeService
    {
        // Mock data structures to simulate a database or external service
        private class PledgeRecord
        {
            public string PledgeId { get; set; }
            public string PolicyId { get; set; }
            public decimal PrincipalAmount { get; set; }
            public DateTime InitiationDate { get; set; }
            public double InterestRate { get; set; }
            public string StatusCode { get; set; }
            public int LockInPeriodDays { get; set; }
        }

        private class AssignmentRecord
        {
            public string PolicyId { get; set; }
            public string AssigneeId { get; set; }
            public bool IsPrimary { get; set; }
            public bool IsCleared { get; set; }
            public double SharePercentage { get; set; }
        }

        private readonly List<PledgeRecord> _pledges;
        private readonly List<AssignmentRecord> _assignments;

        public PolicyPledgeService()
        {
            // Initialize with some dummy data for demonstration
            _pledges = new List<PledgeRecord>
            {
                new PledgeRecord { PledgeId = "PLG-1001", PolicyId = "POL-123", PrincipalAmount = 5000m, InitiationDate = DateTime.Now.AddDays(-150), InterestRate = 0.05, StatusCode = "ACTIVE", LockInPeriodDays = 365 },
                new PledgeRecord { PledgeId = "PLG-1002", PolicyId = "POL-456", PrincipalAmount = 12000m, InitiationDate = DateTime.Now.AddDays(-400), InterestRate = 0.045, StatusCode = "CLEARED", LockInPeriodDays = 365 }
            };

            _assignments = new List<AssignmentRecord>
            {
                new AssignmentRecord { PolicyId = "POL-123", AssigneeId = "BANK-001", IsPrimary = true, IsCleared = false, SharePercentage = 100.0 },
                new AssignmentRecord { PolicyId = "POL-789", AssigneeId = "CORP-002", IsPrimary = true, IsCleared = true, SharePercentage = 50.0 }
            };
        }

        public bool HasActivePledge(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            
            return _pledges.Any(p => p.PolicyId == policyId && p.StatusCode == "ACTIVE");
        }

        public decimal CalculateTotalPledgedAmount(string policyId, DateTime effectiveDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));

            var activePledges = _pledges.Where(p => p.PolicyId == policyId && p.StatusCode == "ACTIVE" && p.InitiationDate <= effectiveDate).ToList();
            
            decimal total = 0m;
            foreach (var pledge in activePledges)
            {
                total += pledge.PrincipalAmount + CalculateAccruedInterest(pledge.PledgeId, effectiveDate);
            }
            
            return total;
        }

        public string GetPrimaryAssigneeId(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));

            var primaryAssignment = _assignments.FirstOrDefault(a => a.PolicyId == policyId && a.IsPrimary);
            return primaryAssignment?.AssigneeId ?? "NONE";
        }

        public int GetDaysSincePledgeInitiation(string pledgeId)
        {
            if (string.IsNullOrWhiteSpace(pledgeId)) throw new ArgumentException("Pledge ID cannot be null or empty.", nameof(pledgeId));

            var pledge = _pledges.FirstOrDefault(p => p.PledgeId == pledgeId);
            if (pledge == null) throw new KeyNotFoundException($"Pledge {pledgeId} not found.");

            return (int)(DateTime.Now - pledge.InitiationDate).TotalDays;
        }

        public double GetPledgeInterestRate(string pledgeId)
        {
            if (string.IsNullOrWhiteSpace(pledgeId)) throw new ArgumentException("Pledge ID cannot be null or empty.", nameof(pledgeId));

            var pledge = _pledges.FirstOrDefault(p => p.PledgeId == pledgeId);
            if (pledge == null) throw new KeyNotFoundException($"Pledge {pledgeId} not found.");

            return pledge.InterestRate;
        }

        public bool VerifyAssignmentClearance(string policyId, string assigneeId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (string.IsNullOrWhiteSpace(assigneeId)) throw new ArgumentException("Assignee ID cannot be null or empty.", nameof(assigneeId));

            var assignment = _assignments.FirstOrDefault(a => a.PolicyId == policyId && a.AssigneeId == assigneeId);
            return assignment != null && assignment.IsCleared;
        }

        public decimal GetOutstandingLoanBalance(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));

            return CalculateTotalPledgedAmount(policyId, DateTime.Now);
        }

        public string InitiatePledgeClearance(string policyId, decimal clearanceAmount, DateTime requestDate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (clearanceAmount <= 0) throw new ArgumentException("Clearance amount must be greater than zero.", nameof(clearanceAmount));

            decimal outstanding = GetOutstandingLoanBalance(policyId);
            if (clearanceAmount < outstanding)
            {
                return $"PARTIAL_CLEARANCE_PENDING: Remaining balance {outstanding - clearanceAmount:C}";
            }

            return $"CLEARANCE_INITIATED_REF_{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public int CountActiveAssignments(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));

            return _assignments.Count(a => a.PolicyId == policyId && !a.IsCleared);
        }

        public double CalculatePledgeToValueRatio(string policyId, decimal currentCashValue)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (currentCashValue <= 0) return 0.0;

            decimal totalPledged = GetOutstandingLoanBalance(policyId);
            return (double)(totalPledged / currentCashValue);
        }

        public bool IsPolicyEligibleForPayout(string policyId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));

            bool hasActivePledges = HasActivePledge(policyId);
            bool hasUnclearedAssignments = CountActiveAssignments(policyId) > 0;

            return !hasActivePledges && !hasUnclearedAssignments;
        }

        public decimal ComputeRecoveryAmount(string policyId, double penaltyRate)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (penaltyRate < 0 || penaltyRate > 1) throw new ArgumentException("Penalty rate must be between 0 and 1.", nameof(penaltyRate));

            decimal outstanding = GetOutstandingLoanBalance(policyId);
            decimal penaltyAmount = outstanding * (decimal)penaltyRate;

            return outstanding + penaltyAmount;
        }

        public string GenerateClearanceCertificate(string policyId, string assigneeId)
        {
            if (!VerifyAssignmentClearance(policyId, assigneeId))
            {
                throw new InvalidOperationException("Cannot generate certificate. Assignment is not cleared.");
            }

            return $"CERT-{policyId}-{assigneeId}-{DateTime.Now:yyyyMMdd}";
        }

        public int GetRemainingLockInPeriodDays(string pledgeId)
        {
            if (string.IsNullOrWhiteSpace(pledgeId)) throw new ArgumentException("Pledge ID cannot be null or empty.", nameof(pledgeId));

            var pledge = _pledges.FirstOrDefault(p => p.PledgeId == pledgeId);
            if (pledge == null) throw new KeyNotFoundException($"Pledge {pledgeId} not found.");

            int daysPassed = GetDaysSincePledgeInitiation(pledgeId);
            int remaining = pledge.LockInPeriodDays - daysPassed;

            return remaining > 0 ? remaining : 0;
        }

        public double GetAssignmentSharePercentage(string policyId, string assigneeId)
        {
            if (string.IsNullOrWhiteSpace(policyId)) throw new ArgumentException("Policy ID cannot be null or empty.", nameof(policyId));
            if (string.IsNullOrWhiteSpace(assigneeId)) throw new ArgumentException("Assignee ID cannot be null or empty.", nameof(assigneeId));

            var assignment = _assignments.FirstOrDefault(a => a.PolicyId == policyId && a.AssigneeId == assigneeId);
            return assignment?.SharePercentage ?? 0.0;
        }

        public bool ReleasePledge(string pledgeId, string authorizationCode)
        {
            if (string.IsNullOrWhiteSpace(pledgeId)) throw new ArgumentException("Pledge ID cannot be null or empty.", nameof(pledgeId));
            if (string.IsNullOrWhiteSpace(authorizationCode)) return false;

            var pledge = _pledges.FirstOrDefault(p => p.PledgeId == pledgeId);
            if (pledge == null || pledge.StatusCode != "ACTIVE") return false;

            // Simulate authorization check
            if (authorizationCode.StartsWith("AUTH-"))
            {
                pledge.StatusCode = "CLEARED";
                return true;
            }

            return false;
        }

        public decimal CalculateAccruedInterest(string pledgeId, DateTime calculationDate)
        {
            if (string.IsNullOrWhiteSpace(pledgeId)) throw new ArgumentException("Pledge ID cannot be null or empty.", nameof(pledgeId));

            var pledge = _pledges.FirstOrDefault(p => p.PledgeId == pledgeId);
            if (pledge == null) throw new KeyNotFoundException($"Pledge {pledgeId} not found.");

            if (calculationDate <= pledge.InitiationDate) return 0m;

            double years = (calculationDate - pledge.InitiationDate).TotalDays / 365.25;
            
            // Simple interest calculation for demonstration
            decimal interest = pledge.PrincipalAmount * (decimal)(pledge.InterestRate * years);
            return Math.Round(interest, 2);
        }

        public string GetPledgeStatusCode(string pledgeId)
        {
            if (string.IsNullOrWhiteSpace(pledgeId)) throw new ArgumentException("Pledge ID cannot be null or empty.", nameof(pledgeId));

            var pledge = _pledges.FirstOrDefault(p => p.PledgeId == pledgeId);
            return pledge?.StatusCode ?? "NOT_FOUND";
        }
    }
}