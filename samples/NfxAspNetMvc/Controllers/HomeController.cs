using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace NfxAspNetMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var logger = logger
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("HomeController index action");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}