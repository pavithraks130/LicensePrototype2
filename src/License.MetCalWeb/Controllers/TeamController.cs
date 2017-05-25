using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    [SessionExpire]
    public class TeamController : BaseController
    {
        public TeamController()
        {

        }


        public ActionResult Index()
        {
            if (LicenseSessionState.Instance.TeamList == null || LicenseSessionState.Instance.TeamList.Count == 0)
            {
                string userId = string.Empty;
                if (!LicenseSessionState.Instance.IsSuperAdmin)
                    userId = LicenseSessionState.Instance.User.UserId;
                var teamList = OnPremiseSubscriptionLogic.GetTeamList(userId);
                LicenseSessionState.Instance.TeamList = teamList;
            }
            return View(LicenseSessionState.Instance.TeamList);
        }

        public ActionResult CreateTeam()
        {
            return View("Create");
        }

        [HttpPost]
        public ActionResult CreateTeam(Team model)
        {
            if (ModelState.IsValid)
            {
                model.AdminId = LicenseSessionState.Instance.User.UserId;
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                var response = client.PostAsJsonAsync("api/Team/Create", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<Team>(jsonData);
                    LicenseSessionState.Instance.TeamList.Add(data);
                    return RedirectToAction("Index");
                }
                else
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);

                }
            }
            var _message = string.Join(Environment.NewLine, ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
            return Json(new { success = false, message = _message });
        }
        public ActionResult AssignLicense()
        {
            List<Team> teams = new List<Team>();
            if (LicenseSessionState.Instance.TeamList != null && LicenseSessionState.Instance.TeamList.Count > 0)
                teams = LicenseSessionState.Instance.TeamList.ToList();
            else
                teams = new List<Team>();
            return View(teams);
        }

        public ActionResult TeamMapLicense(int teamId)
        {
            TeamMappingDetails teamMappingDetails = new TeamMappingDetails();
            TempData["Teamid"] = teamId;
            teamMappingDetails.SelectedTeamName = LicenseSessionState.Instance.TeamList.ToList().Where(t => t.Id == teamId).FirstOrDefault().Name;
            string adminUserId = string.Empty;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminUserId = LicenseSessionState.Instance.User.UserId;
            else
            {
                adminUserId = LicenseSessionState.Instance.SelectedTeam.AdminId;
            }
            teamMappingDetails.SubscriptionDetailsList = OnPremiseSubscriptionLogic.GetSubscription(adminUserId).ToList();
            return View(teamMappingDetails);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeamMapLicense(TeamMappingDetails teamMappingDetails, params string[] selectedSubscription)
        {
            var responseData = UpdateLicense(teamMappingDetails.ConcurrentUserCount,selectedSubscription);
            if (!String.IsNullOrEmpty(responseData))
            {
                ModelState.AddModelError("", responseData);
                return View("TeamContainer", "TeamManagement");
            }
            return RedirectToAction("TeamContainer", "TeamManagement");
        }

        public string UpdateLicense(int concurrentUserCount, string[] selectedSubscription)
        {

            List<string> teamIdList = new List<string>();
            teamIdList.Add(TempData["Teamid"].ToString());
            List<LicenseData> lstLicData = ExtractLicenseData(selectedSubscription);
            TeamLicenseDataMapping mapping = new TeamLicenseDataMapping() { ConcurrentUserCount= concurrentUserCount, LicenseDataList = lstLicData, TeamList = teamIdList };
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PostAsJsonAsync("api/License/CreateTeamLicence", mapping).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                return response.ReasonPhrase + " - " + obj.Message;
            }
            else
            {
                if (teamIdList.Contains(LicenseSessionState.Instance.SelectedTeam.Id.ToString()))//doubt
                {
                    var subscriptionDetails = OnPremiseSubscriptionLogic.GetUserLicenseForUser();
                    LicenseSessionState.Instance.UserSubscriptionList = subscriptionDetails;
                }
            }
            return String.Empty;
        }

        private static List<LicenseData> ExtractLicenseData(string[] SelectedSubscription)
        {
            List<LicenseData> lstLicData = new List<LicenseData>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];

                LicenseData licData = new LicenseData()
                {
                    UserSubscriptionId = Convert.ToInt32(subscriptionId),
                    ProductId = Convert.ToInt32(prodId)
                };
                lstLicData.Add(licData);
            }

            return lstLicData;
        }

        public IList<SubscriptionDetails> GetLicenseListBySubscription(string userId)
        {
            TempData["UserId"] = userId;
            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            IList<SubscriptionDetails> licenseMapModelList = OnPremiseSubscriptionLogic.GetSubscriptionForLicenseMap(userId, adminUserId);
            return licenseMapModelList;
        }
        public ActionResult EditTeam(int id)
        {
            var team = LicenseSessionState.Instance.TeamList.FirstOrDefault(t => t.Id == id);
            return View("Edit", team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTeam(int id, Team model)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PutAsJsonAsync("api/team/Update/" + id, model).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var teamObj = JsonConvert.DeserializeObject<Team>(data);
                LicenseSessionState.Instance.TeamList.RemoveAll(f => f.Id == id);
                LicenseSessionState.Instance.TeamList.Add(teamObj);
                return RedirectToAction("Index");
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            var _message = string.Join(Environment.NewLine, ModelState.Values
                                      .SelectMany(x => x.Errors)
                                      .Select(x => x.ErrorMessage));
            return Json(new { success = false, message = _message });
        }

        [HttpGet]
        public ActionResult DeleteTeam(int id)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.DeleteAsync("api/team/Delete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                LicenseSessionState.Instance.TeamList.RemoveAll(t => t.Id == id);
                if (LicenseSessionState.Instance.SelectedTeam != null && LicenseSessionState.Instance.SelectedTeam.Id == id)
                    LicenseSessionState.Instance.SelectedTeam = LicenseSessionState.Instance.TeamList.FirstOrDefault(s => s.IsDefaultTeam = true);
                return RedirectToAction("Index");
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View();
        }

    }
}