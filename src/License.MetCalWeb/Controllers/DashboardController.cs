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
                if (LicenseSessionState.Instance.IsTeamMember)
                    userId = LicenseSessionState.Instance.User.UserId;
                var teamList = OnPremiseSubscriptionLogic.GetTeamList(userId);
                LicenseSessionState.Instance.TeamList = teamList;
            }
            ViewData["IsTeamListPopupVisible"] = LicenseSessionState.Instance.TeamList.Count > 0 && LicenseSessionState.Instance.SelectedTeam == null;
            if (LicenseSessionState.Instance.SelectedTeam != null)
            {
                return View(LicenseSessionState.Instance.UserSubscriptionList);
            }
            return View();
        }

        public ActionResult GetTeamList()
        {
            return View(LicenseSessionState.Instance.TeamList);
        }

        [HttpPost]
        public ActionResult GetTeamList(int teamId)
        {
            LicenseSessionState.Instance.SelectedTeam = LicenseSessionState.Instance.TeamList.Where(t => t.Id == teamId).FirstOrDefault();
            LicenseSessionState.Instance.AppTeamContext = new Team() {
                Id = LicenseSessionState.Instance.SelectedTeam.Id,
                AdminId = LicenseSessionState.Instance.SelectedTeam.AdminId,
                Name = LicenseSessionState.Instance.SelectedTeam.Name
            };
            if (LicenseSessionState.Instance.UserSubscriptionList == null)
            {
                var subscriptionDetails = OnPremiseSubscriptionLogic.GetUserLicenseForUser();
                LicenseSessionState.Instance.UserSubscriptionList = subscriptionDetails;
            }
            return Json(new { success = true, message = "" });
        }

        // GET: Tab
        public ActionResult About()
        {
            return View();

        }

    }
}