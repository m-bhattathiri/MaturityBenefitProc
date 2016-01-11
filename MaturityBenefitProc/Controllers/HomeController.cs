using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaturityBenefitProc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Maturity Benefit Processing - Dashboard";
            ViewBag.TotalPoliciesMaturing = 1247;
            ViewBag.PendingClaims = 89;
            ViewBag.DisbursedThisMonth = 312;
            ViewBag.TotalDisbursedAmount = 45670000m;
            ViewBag.AvgProcessingDays = 4.2;
            ViewBag.RejectionRate = 3.8;

            var recentActivity = new List<dynamic>();
            ViewBag.RecentActivity = recentActivity;

            ViewBag.MaturityAlertCount = 56;
            ViewBag.OverdueDisbursements = 12;
            ViewBag.TdsFilingPending = 7;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "About MaturityBenefitProc";
            ViewBag.Message = "Life Insurance Maturity Benefit Processing System";
            ViewBag.Version = "1.0.0";
            ViewBag.BuildDate = new DateTime(2017, 3, 15);
            ViewBag.SupportedPolicyTypes = new string[]
            {
                "Endowment Plans",
                "Money Back Plans",
                "Whole Life Plans",
                "Term Plans",
                "Unit Linked Insurance Plans (ULIP)"
            };

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Contact Support";
            ViewBag.Message = "Maturity Benefit Operations Team";
            ViewBag.SupportEmail = "maturity.ops@lifeinsure.co.in";
            ViewBag.SupportPhone = "+91-1800-209-4567";
            ViewBag.OperatingHours = "Monday to Saturday, 9:00 AM - 6:00 PM IST";
            ViewBag.EscalationEmail = "maturity.escalation@lifeinsure.co.in";
            ViewBag.BranchLocatorUrl = "/Branches/Locator";

            ViewBag.RegionalOffices = new Dictionary<string, string>
            {
                { "North Zone", "Delhi Regional Office - Connaught Place" },
                { "South Zone", "Chennai Regional Office - T. Nagar" },
                { "East Zone", "Kolkata Regional Office - Park Street" },
                { "West Zone", "Mumbai Regional Office - Nariman Point" }
            };

            return View();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = new ViewDataDictionary(new HandleErrorInfo(
                    filterContext.Exception,
                    filterContext.RouteData.Values["controller"].ToString(),
                    filterContext.RouteData.Values["action"].ToString()))
            };

            filterContext.ExceptionHandled = true;
        }
    }
}
