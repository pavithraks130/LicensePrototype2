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
            if (LicenseSessionState.Instance.TeamList == null || LicenseSessionState.Instance.TeamList.Count == 0)
            {
                string userId = string.Empty;
                if (!LicenseSessionState.Instance.IsSuperAdmin)
                    userId = LicenseSessionState.Instance.User.UserId;
                var teamList = OnPremiseSubscriptionLogic.GetTeamList(userId);
                LicenseSessionState.Instance.TeamList = teamList;

                if (teamList.Count == 0)
                {
                    ViewData["IsTeamListPopupVisible"] = false;
                    return View();
                }
                else if (teamList.Count == 1)
                {
                    LicenseSessionState.Instance.SelectedTeam = teamList.FirstOrDefault();
                    LoadUserSubscription();
                }
            }
            ViewData["IsTeamListPopupVisible"] = LicenseSessionState.Instance.TeamList.Count > 0 && LicenseSessionState.Instance.SelectedTeam == null;

            if (LicenseSessionState.Instance.IsSuperAdmin)
            {
                var expiredSubscriptipon = CentralizedSubscriptionLogic.GetExpireSubscription();
                ViewData["ExpiredSubCount"] = expiredSubscriptipon == null ? 0 : expiredSubscriptipon.Count;
            }
            else
                ViewData["ExpiredSubCount"] = "";

            if (LicenseSessionState.Instance.SelectedTeam != null)
                return View(LicenseSessionState.Instance.UserSubscriptionList);

           
            return View();

        }


        public ActionResult TeamList()
        {
            return View(LicenseSessionState.Instance.TeamList);
        }

        [HttpPost]
        public ActionResult TeamList(int teamId)
        {
            LicenseSessionState.Instance.SelectedTeam = LicenseSessionState.Instance.TeamList.Where(t => t.Id == teamId).FirstOrDefault();
            var message = LoadUserSubscription();
            return Json(new { success = true, message = message });
        }

        public string LoadUserSubscription()
        {
            LicenseSessionState.Instance.AppTeamContext = new Team()
            {
                Id = LicenseSessionState.Instance.SelectedTeam.Id,
                AdminId = LicenseSessionState.Instance.SelectedTeam.AdminId,
                Name = LicenseSessionState.Instance.SelectedTeam.Name
            };
            if (LicenseSessionState.Instance.UserSubscriptionList == null || LicenseSessionState.Instance.UserSubscriptionList.Count == 0)
            {
                var subscriptionDetails = OnPremiseSubscriptionLogic.GetUserLicenseForUser();
                LicenseSessionState.Instance.UserSubscriptionList = subscriptionDetails;
            }
            return string.Empty;
        }
        // GET: Tab
        public ActionResult About()
        {
            return View();

        }

    }
}