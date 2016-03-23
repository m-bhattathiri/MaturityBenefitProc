using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaturityBenefitProc.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Administration Dashboard";
            ViewBag.SystemUptime = "99.7%";
            ViewBag.LastBackup = DateTime.UtcNow.AddHours(-6).ToString("dd-MMM-yyyy HH:mm");
            ViewBag.ActiveSessions = 14;
            ViewBag.TotalUsers = 42;
            ViewBag.PendingApprovals = 8;
            ViewBag.UnresolvedAlerts = 3;

            ViewBag.DatabaseSize = "2.4 GB";
            ViewBag.DocumentStorageUsed = "18.7 GB";
            ViewBag.QueueDepth = 23;
            ViewBag.AverageResponseTime = "240ms";

            ViewBag.EnvironmentName = "Production";
            ViewBag.ApplicationVersion = "1.0.0";
            ViewBag.DotNetVersion = "4.6.1";
            ViewBag.ServerName = "APPSVR-MATURITY-01";

            return View();
        }

        public ActionResult Users()
        {
            ViewBag.Title = "User Management";
            ViewBag.TotalUsers = 42;
            ViewBag.ActiveUsers = 38;
            ViewBag.LockedUsers = 2;
            ViewBag.InactiveUsers = 2;

            var users = new List<object>();
            ViewBag.Roles = new List<string>
            {
                "Administrator",
                "Claims Officer",
                "Claims Approver",
                "Disbursement Officer",
                "Audit Viewer",
                "Branch Manager",
                "Regional Head"
            };

            return View(users);
        }

        public ActionResult AuditLog()
        {
            ViewBag.Title = "Audit Trail";
            ViewBag.DateFrom = Request.QueryString["from"] ?? DateTime.UtcNow.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.DateTo = Request.QueryString["to"] ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
            ViewBag.UserFilter = Request.QueryString["user"] ?? "all";
            ViewBag.ActionFilter = Request.QueryString["action"] ?? "all";

            ViewBag.ActionTypes = new List<string>
            {
                "Claim Created",
                "Claim Approved",
                "Claim Rejected",
                "Disbursement Initiated",
                "Disbursement Retried",
                "Policy Updated",
                "User Login",
                "User Logout",
                "Configuration Changed",
                "Report Generated"
            };

            var auditEntries = new List<object>();
            ViewBag.TotalEntries = 0;
            ViewBag.CurrentPage = 1;
            ViewBag.PageSize = 50;

            return View(auditEntries);
        }

        public ActionResult SystemConfig()
        {
            ViewBag.Title = "System Configuration";

            ViewBag.TdsThreshold = 100000m;
            ViewBag.TdsRate = 1.0m;
            ViewBag.TdsRateNoPan = 20.0m;
            ViewBag.MaxClaimProcessingDays = 15;
            ViewBag.AutoEscalationDays = 7;
            ViewBag.NeftCutoffTime = "14:00";
            ViewBag.RtgsCutoffTime = "16:00";
            ViewBag.ChequeValidityDays = 90;
            ViewBag.MaxRetryAttempts = 3;

            ViewBag.SmtpServer = "smtp.lifeinsure.co.in";
            ViewBag.NotificationEnabled = true;
            ViewBag.SmsGatewayEnabled = true;
            ViewBag.BatchProcessingTime = "02:00 AM IST";
            ViewBag.MaintenanceWindow = "Sunday 01:00 - 05:00 AM IST";

            return View();
        }

        public ActionResult ReconciliationDashboard()
        {
            ViewBag.Title = "Reconciliation Dashboard";
            ViewBag.LastReconciliationDate = DateTime.UtcNow.AddDays(-1).ToString("dd-MMM-yyyy");
            ViewBag.ReconciliationStatus = "Completed";

            ViewBag.MatchedTransactions = 0;
            ViewBag.UnmatchedTransactions = 0;
            ViewBag.PendingReconciliation = 0;
            ViewBag.TotalReconciledAmount = 0m;
            ViewBag.DiscrepancyAmount = 0m;

            ViewBag.BankwiseBreakdown = new Dictionary<string, int>
            {
                { "State Bank of India", 0 },
                { "Punjab National Bank", 0 },
                { "HDFC Bank", 0 },
                { "ICICI Bank", 0 },
                { "Bank of Baroda", 0 }
            };

            var reconciliationEntries = new List<object>();

            return View(reconciliationEntries);
        }
    }
}
