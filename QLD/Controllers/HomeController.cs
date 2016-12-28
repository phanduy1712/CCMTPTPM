using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLD.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact paage.";

            return View();
        }
        public ActionResult test()
        {
            ViewBag.Message = "Your applicatioan description page.";

            return View();
        }

        public ActionResult demo()
        {
            ViewBag.Message = "Your contact apage.";

            return View();
        }
        public ActionResult demo1()
        {
            ViewBag.Message = "Your applicatioan description page.";

            return View();
        }

        public ActionResult demo2()
        {
            ViewBag.Message = "Your contacta page.";

            return View();
        }
        public ActionResult demo3()
        {
            ViewBag.Message = "Your application description apage.";

            return View();
        }

        public ActionResult demo4()
        {
            ViewBag.Message = "Your contact pagea.";

            return View();
        }
    }
}