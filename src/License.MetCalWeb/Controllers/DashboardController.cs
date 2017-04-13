using License.MetCalWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using License.MetCalWeb.Models;
using Newtonsoft.Json;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Tab
        public ActionResult Home()
        {
            var subscriptionDetails = OnPremiseSubscriptionLogic.GetUserLicenseForUser();
            return View(subscriptionDetails);
        }

        // GET: Tab
        public ActionResult About()
        {
            return View();

        }

    }
}