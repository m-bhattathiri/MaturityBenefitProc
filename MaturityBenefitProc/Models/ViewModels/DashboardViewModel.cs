using System;
using System.Collections.Generic;

namespace MaturityBenefitProc.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalPoliciesMaturing { get; set; }

        public int PendingClaims { get; set; }

        public int DisbursementsToday { get; set; }

        public decimal TotalAmountDisbursed { get; set; }

        public int ClaimsApprovedThisMonth { get; set; }

        public int ClaimsRejectedThisMonth { get; set; }

        public decimal AverageTurnaroundDays { get; set; }

        public List<MaturityClaim> RecentClaims { get; set; }

        public List<DisbursementRecord> OverdueDisbursements { get; set; }

        public List<Policy> UpcomingMaturities { get; set; }

        public DashboardViewModel()
        {
            RecentClaims = new List<MaturityClaim>();
            OverdueDisbursements = new List<DisbursementRecord>();
            UpcomingMaturities = new List<Policy>();
            TotalPoliciesMaturing = 0;
            PendingClaims = 0;
            DisbursementsToday = 0;
            TotalAmountDisbursed = 0m;
            AverageTurnaroundDays = 0m;
        }
    }
}
