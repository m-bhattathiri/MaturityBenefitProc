using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaturityBenefitProc.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Reports & Analytics";
            ViewBag.AvailableFinancialYears = new List<string>
            {
                "2017-18", "2016-17", "2015-16", "2014-15"
            };

            ViewBag.LastGeneratedMaturity = DateTime.UtcNow.AddDays(-1).ToString("dd-MMM-yyyy HH:mm");
            ViewBag.LastGeneratedDisbursement = DateTime.UtcNow.AddDays(-1).ToString("dd-MMM-yyyy HH:mm");
            ViewBag.LastGeneratedTds = DateTime.UtcNow.AddDays(-7).ToString("dd-MMM-yyyy HH:mm");

            return View();
        }

        public ActionResult MaturityReport(DateTime? from, DateTime? to)
        {
            ViewBag.Title = "Maturity Benefit Report";

            var fromDate = from ?? DateTime.UtcNow.AddMonths(-1);
            var toDate = to ?? DateTime.UtcNow;

            ViewBag.FromDate = fromDate.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.ToString("yyyy-MM-dd");
            ViewBag.ReportPeriod = string.Format("{0} to {1}",
                fromDate.ToString("dd-MMM-yyyy"), toDate.ToString("dd-MMM-yyyy"));

            ViewBag.TotalPoliciesMatured = 0;
            ViewBag.TotalMaturityAmount = 0m;
            ViewBag.AverageBenefitAmount = 0m;
            ViewBag.EndowmentCount = 0;
            ViewBag.MoneyBackCount = 0;
            ViewBag.UlipCount = 0;

            var reportData = new List<object>();

            return View(reportData);
        }

        public ActionResult DisbursementReport(DateTime? from, DateTime? to)
        {
            ViewBag.Title = "Disbursement Report";

            var fromDate = from ?? DateTime.UtcNow.AddMonths(-1);
            var toDate = to ?? DateTime.UtcNow;

            ViewBag.FromDate = fromDate.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.ToString("yyyy-MM-dd");

            ViewBag.TotalDisbursed = 0;
            ViewBag.TotalAmount = 0m;
            ViewBag.NeftCount = 0;
            ViewBag.RtgsCount = 0;
            ViewBag.ChequeCount = 0;
            ViewBag.FailedCount = 0;
            ViewBag.RetryCount = 0;
            ViewBag.AverageProcessingDays = 0.0;

            var reportData = new List<object>();

            return View(reportData);
        }

        public ActionResult TdsReport(string financialYear)
        {
            ViewBag.Title = "TDS Deduction Report";

            if (string.IsNullOrWhiteSpace(financialYear))
            {
                financialYear = DateTime.UtcNow.Month >= 4
                    ? string.Format("{0}-{1}", DateTime.UtcNow.Year, (DateTime.UtcNow.Year + 1) % 100)
                    : string.Format("{0}-{1}", DateTime.UtcNow.Year - 1, DateTime.UtcNow.Year % 100);
            }

            ViewBag.FinancialYear = financialYear;
            ViewBag.TotalTdsDeducted = 0m;
            ViewBag.TotalPoliciesWithTds = 0;
            ViewBag.TdsThresholdAmount = 100000m;
            ViewBag.TdsRate = 1.0m;
            ViewBag.TdsRateNoPan = 20.0m;
            ViewBag.Section = "Section 194DA - Insurance Maturity Proceeds";

            ViewBag.QuarterlyBreakdown = new Dictionary<string, decimal>
            {
                { "Q1 (Apr-Jun)", 0m },
                { "Q2 (Jul-Sep)", 0m },
                { "Q3 (Oct-Dec)", 0m },
                { "Q4 (Jan-Mar)", 0m }
            };

            var reportData = new List<object>();

            return View(reportData);
        }

        public ActionResult PendingClaimsReport()
        {
            ViewBag.Title = "Pending Claims Report";
            ViewBag.GeneratedOn = DateTime.UtcNow.ToString("dd-MMM-yyyy HH:mm:ss");

            ViewBag.TotalPending = 0;
            ViewBag.PendingVerification = 0;
            ViewBag.PendingApproval = 0;
            ViewBag.PendingDisbursement = 0;
            ViewBag.OlderThan7Days = 0;
            ViewBag.OlderThan15Days = 0;
            ViewBag.OlderThan30Days = 0;
            ViewBag.EscalationRequired = 0;

            var reportData = new List<object>();

            return View(reportData);
        }
    }
}
