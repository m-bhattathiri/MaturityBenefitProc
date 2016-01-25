using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaturityBenefitProc.Models;

namespace MaturityBenefitProc.Controllers
{
    public class PolicyController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Maturing Policies";
            ViewBag.TotalCount = 0;
            ViewBag.FilterStatus = Request.QueryString["status"] ?? "all";
            ViewBag.SortBy = Request.QueryString["sort"] ?? "maturityDate";
            ViewBag.SortDirection = Request.QueryString["dir"] ?? "asc";

            var policyList = new List<Policy>();
            ViewBag.CurrentPage = 1;
            ViewBag.TotalPages = 1;
            ViewBag.PageSize = 25;

            return View(policyList);
        }

        public ActionResult Details(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest,
                    "Policy number is required for lookup");
            }

            ViewBag.Title = string.Format("Policy Details - {0}", policyNumber);
            ViewBag.PolicyNumber = policyNumber;

            var policy = new Policy
            {
                PolicyNumber = policyNumber,
                PolicyType = PolicyType.Endowment,
                PolicyStatus = PolicyStatus.Active,
                SumAssured = 500000m,
                PremiumAmount = 12500m,
                PremiumFrequency = PremiumFrequency.Quarterly,
                PolicyTerm = 20,
                PremiumPayingTerm = 15,
                CommencementDate = new DateTime(2000, 4, 1),
                MaturityDate = new DateTime(2020, 4, 1),
                BranchCode = "MUM001"
            };

            ViewBag.BonusAccrued = 125000m;
            ViewBag.FinalAdditionalBonus = 15000m;
            ViewBag.TotalMaturityBenefit = policy.SumAssured + 125000m + 15000m;
            ViewBag.PremiumsPaid = 48;
            ViewBag.PremiumsExpected = 60;

            return View(policy);
        }

        public ActionResult MaturitySchedule()
        {
            ViewBag.Title = "Maturity Schedule";
            ViewBag.CurrentMonth = DateTime.UtcNow.ToString("MMMM yyyy");
            ViewBag.NextThreeMonths = Enumerable.Range(0, 3)
                .Select(i => DateTime.UtcNow.AddMonths(i).ToString("MMMM yyyy"))
                .ToList();

            ViewBag.ScheduleData = new List<object>();
            ViewBag.TotalMaturityValue = 0m;
            ViewBag.PolicyCount = 0;

            return View();
        }

        public ActionResult Search(string query)
        {
            ViewBag.Title = "Policy Search Results";
            ViewBag.SearchQuery = query ?? string.Empty;
            ViewBag.ResultCount = 0;

            if (string.IsNullOrWhiteSpace(query))
            {
                ViewBag.ErrorMessage = "Please enter a policy number, CIF, or name to search.";
                return View(new List<Policy>());
            }

            var trimmedQuery = query.Trim();
            var results = new List<Policy>();

            ViewBag.ResultCount = results.Count;
            ViewBag.SearchType = trimmedQuery.Length == 10 && trimmedQuery.StartsWith("POL")
                ? "PolicyNumber"
                : trimmedQuery.Length <= 15 && trimmedQuery.All(char.IsLetterOrDigit)
                    ? "CIF"
                    : "Name";

            return View(results);
        }
    }
}
