using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class TabController : Controller
    {
        // GET: Tab
        public ActionResult Home()
        {
            return View();
        }

        // GET: Tab
        public ActionResult About()
        {
            return View();

        }

        //GET: Tab
        public ActionResult Products()
        {
            return View();
        }
    }
}