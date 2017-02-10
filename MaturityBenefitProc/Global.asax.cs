using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MaturityBenefitProc.Data;

namespace MaturityBenefitProc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles();

            Database.SetInitializer(new CreateDatabaseIfNotExists<MaturityBenefitContext>());

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception != null)
            {
                System.Diagnostics.Trace.TraceError(
                    "Unhandled exception in MaturityBenefitProc: {0}", exception.Message);
            }
        }
    }
}
