using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace eMIMPanel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //void Application_Error(object sender, EventArgs e)
        //{
        //    // Code that runs when an unhandled error occurs  
        //    Exception Ex = Server.GetLastError();
        //    //Session["err"] = Ex.Message;
        //    Server.ClearError();
        //    //string s = Convert.ToString(Session["UserType"]);
        //    //if (s == "USER") Server.Transfer("error_u.aspx?error=" + Ex.Message);
        //    //else if (s.Contains("ADMIN")) Server.Transfer("error.aspx?error=" + Ex.Message);
        //    //else
        //    Server.Transfer("errorAll.aspx?error=" + Ex.Message);
        //}
    }
}

