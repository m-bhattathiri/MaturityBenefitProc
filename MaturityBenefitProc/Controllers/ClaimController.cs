using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaturityBenefitProc.Models;

namespace MaturityBenefitProc.Controllers
{
    public class ClaimController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Maturity Claims";
            ViewBag.StatusFilter = Request.QueryString["status"] ?? "all";
            ViewBag.DateFrom = Request.QueryString["from"];
            ViewBag.DateTo = Request.QueryString["to"];

            ViewBag.PendingCount = 0;
            ViewBag.ApprovedCount = 0;
            ViewBag.RejectedCount = 0;
            ViewBag.DisbursedCount = 0;

            var claims = new List<object>();
            ViewBag.TotalClaims = claims.Count;

            return View(claims);
        }

        [HttpGet]
        public ActionResult Create(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                TempData["ErrorMessage"] = "A valid policy number is required to initiate a maturity claim.";
                return RedirectToAction("Index", "Policy");
            }

            ViewBag.Title = string.Format("Initiate Maturity Claim - {0}", policyNumber);
            ViewBag.PolicyNumber = policyNumber;
            ViewBag.SumAssured = 500000m;
            ViewBag.BonusAmount = 125000m;
            ViewBag.FinalAdditionalBonus = 15000m;
            ViewBag.TotalPayable = 640000m;
            ViewBag.TdsApplicable = 6400m;
            ViewBag.NetPayable = 633600m;

            ViewBag.PaymentModes = new SelectList(new[]
            {
                new { Value = "NEFT", Text = "NEFT - National Electronic Fund Transfer" },
                new { Value = "RTGS", Text = "RTGS - Real Time Gross Settlement" },
                new { Value = "CHEQUE", Text = "Cheque / Demand Draft" },
                new { Value = "IMPS", Text = "IMPS - Immediate Payment Service" }
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(FormCollection claimForm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors and resubmit the claim form.";
                return RedirectToAction("Create", new { policyNumber = claimForm["PolicyNumber"] });
            }

            var policyNumber = claimForm["PolicyNumber"];
            var paymentMode = claimForm["PaymentMode"];
            var bankAccountNumber = claimForm["BankAccountNumber"];
            var ifscCode = claimForm["IfscCode"];

            string claimNumber = string.Format("MC{0}{1:D5}",
                DateTime.UtcNow.ToString("yyyyMM"), new Random().Next(1, 99999));

            TempData["SuccessMessage"] = string.Format(
                "Maturity claim {0} submitted successfully for policy {1}. Expected processing within 5 business days.",
                claimNumber, policyNumber);

            return RedirectToAction("Details", new { claimNumber = claimNumber });
        }

        public ActionResult Details(string claimNumber)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest,
                    "Claim number is required");
            }

            ViewBag.Title = string.Format("Claim Details - {0}", claimNumber);
            ViewBag.ClaimNumber = claimNumber;
            ViewBag.Status = "Pending Verification";
            ViewBag.PolicyNumber = "POL20150001";
            ViewBag.ClaimAmount = 640000m;
            ViewBag.TdsDeducted = 6400m;
            ViewBag.NetPayable = 633600m;
            ViewBag.PaymentMode = "NEFT";
            ViewBag.SubmittedDate = DateTime.UtcNow.AddDays(-2);
            ViewBag.AuditTrail = new List<object>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(string claimNumber)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            TempData["SuccessMessage"] = string.Format(
                "Claim {0} approved. Disbursement will be initiated within 2 business days.", claimNumber);

            return RedirectToAction("Details", new { claimNumber = claimNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(string claimNumber, string reason)
        {
            if (string.IsNullOrWhiteSpace(claimNumber))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                TempData["ErrorMessage"] = "A rejection reason is mandatory for audit compliance.";
                return RedirectToAction("Details", new { claimNumber = claimNumber });
            }

            TempData["WarningMessage"] = string.Format(
                "Claim {0} has been rejected. Reason: {1}", claimNumber, reason);

            return RedirectToAction("Details", new { claimNumber = claimNumber });
        }
    }
}
