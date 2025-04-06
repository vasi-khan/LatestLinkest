using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eMIMPanel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //RedirectPermanent("~/Login.aspx");
            return Redirect("~/Login.aspx");
            //return View();
        }

        public ActionResult About()
        {
            RedirectPermanent("~/Login.aspx");

            return View();
        }

        public ActionResult Contact()
        {
            RedirectPermanent("~/Login.aspx");

            return View();
        }
    }
}