using License.MetCalWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.MetCalWeb.Controllers
{
    public class TabController : Controller
    {
        // GET: Tab
        public ActionResult Home()
        {
            OnPremiseSubscriptionLogic.GetUserLicenseForUser();
            return View();

        }

        // GET: Tab
        public ActionResult About()
        {
            return View();

        }

    }
}