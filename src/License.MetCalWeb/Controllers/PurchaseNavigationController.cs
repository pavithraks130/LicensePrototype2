using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.MetCalWeb.Controllers
{
    public class PurchaseNavigationController : Controller
    {
        // GET: PurchaseNavigation
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Shop()
        {
            return View();
        }
        public ActionResult PayQuote()
        {
            return View();
        }
        public ActionResult Renew()
        {
            return View();
        }
        public ActionResult UpGrade()
        {
            return View();

        }
    }
}