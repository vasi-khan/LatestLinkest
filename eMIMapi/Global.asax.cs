using eMIMapi.Helper;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace eMIMapi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initialize RabbitMQ connection when the application starts
            //RabbitMQSingleton.Instance.InitializeRabbitMQConnection();
        }

        protected void Application_End()
        {
            // Close RabbitMQ connection when the application ends
            //RabbitMQSingleton.Instance.CloseRabbitMQConnection();
        }
    }
}