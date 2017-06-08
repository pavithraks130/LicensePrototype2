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
    /// <summary>
    /// DashBoard controller used to display the data(Product License and Expire Subscription details) to the User once user Log In .
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        /// <summary>
        /// Get Action to display the product License which is mapped already to user based on the Team Context selected.
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
      
            if (LicenseSessionState.Instance.IsSuperAdmin)
            {
                var expiredSubscriptipon = CentralizedSubscriptionLogic.GetExpireSubscription();
                ViewBag.ExpiredSubCount = expiredSubscriptipon == null ? 0 : expiredSubscriptipon.Count;
            }
            else
                ViewBag.ExpiredSubCount = "";

            if (LicenseSessionState.Instance.UserSubscribedProducts != null)
                return View(LicenseSessionState.Instance.UserSubscribedProducts);
            return View();
        }


        /// <summary>
        /// Function to make te Service call to get the data related to the Logged In User.
        /// </summary>
        /// <returns></returns>
     

        // GET: Tab
        public ActionResult About()
        {
            return View();

        }

    }
}