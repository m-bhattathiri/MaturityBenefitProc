using System.Web.Mvc;
using System.Web.Routing;

namespace MaturityBenefitProc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "PolicyLookup",
                url: "Policy/{policyNumber}",
                defaults: new { controller = "Policy", action = "Details" },
                constraints: new { policyNumber = @"^[A-Z]{2,4}\d{6,10}$" }
            );

            routes.MapRoute(
                name: "ClaimProcessing",
                url: "Claim/{action}/{claimId}",
                defaults: new { controller = "MaturityClaim", action = "Index", claimId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DisbursementStatus",
                url: "Disbursement/{referenceNumber}",
                defaults: new { controller = "Disbursement", action = "Status" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
