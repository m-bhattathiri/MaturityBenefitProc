using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaturityBenefitProc.Controllers
{
    public class DisbursementController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Disbursement Tracking";
            ViewBag.StatusFilter = Request.QueryString["status"] ?? "all";
            ViewBag.PaymentMode = Request.QueryString["mode"] ?? "all";

            ViewBag.PendingCount = 0;
            ViewBag.ProcessedCount = 0;
            ViewBag.FailedCount = 0;
            ViewBag.TotalDisbursedAmount = 0m;

            var disbursements = new List<object>();
            ViewBag.TotalRecords = disbursements.Count;
            ViewBag.CurrentPage = 1;
            ViewBag.PageSize = 20;

            return View(disbursements);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Process(string claimNumber)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest,
                    "Claim number is required to initiate disbursement");
            }

            string utrNumber = string.Format("UTR{0}{1:D8}",
                DateTime.UtcNow.ToString("yyyyMMdd"), new Random().Next(1, 99999999));

            TempData["SuccessMessage"] = string.Format(
                "Disbursement initiated for claim {0}. UTR: {1}. NEFT settlement expected within 24 hours.",
                claimNumber, utrNumber);

            return RedirectToAction("Track", new { utrNumber = utrNumber });
        }

        public ActionResult Track(string utrNumber)
        {
            if (string.IsNullOrWhiteSpace(utrNumber))
            {
                ViewBag.Title = "Track Disbursement";
                ViewBag.ShowSearchForm = true;
                return View();
            }

            ViewBag.Title = string.Format("Disbursement Tracking - {0}", utrNumber);
            ViewBag.UtrNumber = utrNumber;
            ViewBag.ShowSearchForm = false;

            ViewBag.DisbursementStatus = "Processing";
            ViewBag.PaymentMode = "NEFT";
            ViewBag.Amount = 633600m;
            ViewBag.BeneficiaryName = "Rajesh Kumar Sharma";
            ViewBag.BankName = "State Bank of India";
            ViewBag.AccountNumber = "XXXX XXXX 4567";
            ViewBag.IfscCode = "SBIN0001234";
            ViewBag.InitiatedDate = DateTime.UtcNow.AddDays(-1);
            ViewBag.ExpectedSettlement = DateTime.UtcNow.AddDays(1);

            ViewBag.TrackingHistory = new List<object>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Retry(string disbursementId)
        {
            if (string.IsNullOrWhiteSpace(disbursementId))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest,
                    "Disbursement ID is mandatory for retry");
            }

            string newUtrNumber = string.Format("UTR{0}{1:D8}",
                DateTime.UtcNow.ToString("yyyyMMdd"), new Random().Next(1, 99999999));

            TempData["SuccessMessage"] = string.Format(
                "Retry initiated for disbursement {0}. New UTR: {1}. " +
                "If the issue was incorrect bank details, please update before retrying.",
                disbursementId, newUtrNumber);

            return RedirectToAction("Track", new { utrNumber = newUtrNumber });
        }

        public ActionResult ChequeStatus()
        {
            ViewBag.Title = "Cheque / DD Status";
            ViewBag.PendingPrint = 0;
            ViewBag.PrintedNotDispatched = 0;
            ViewBag.Dispatched = 0;
            ViewBag.Encashed = 0;
            ViewBag.Stale = 0;
            ViewBag.Cancelled = 0;

            var cheques = new List<object>();
            ViewBag.TotalCheques = cheques.Count;

            return View(cheques);
        }
    }
}
