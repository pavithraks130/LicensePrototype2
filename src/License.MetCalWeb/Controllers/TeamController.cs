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
    public class TeamController : BaseController
    {
        public TeamController()
        {

        }


        public ActionResult TeamList()
        {
            OnPremiseSubscriptionLogic.GetTeamList();
            return View("Teams", LicenseSessionState.Instance.TeamList);
        }

        public ActionResult CreateTeam()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeam(Team model)
        {
            if (ModelState.IsValid)
            {
                model.AdminId = LicenseSessionState.Instance.User.UserId;
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                var response = client.PostAsJsonAsync("api/Team/Create", model).Result;
                if (response.IsSuccessStatusCode)
                {

                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<Team>(jsonData);
                    LicenseSessionState.Instance.TeamList.Add(data);
                    return RedirectToAction("TeamList");
                }
                else
                {
                    var message = response.ReasonPhrase + " - " + response.Content.ReadAsStringAsync().Result;
                    ModelState.AddModelError("", message);
                }
            }
            return View("Create", model);
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
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.PutAsJsonAsync("api/team/Update/" + id, model).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var teamObj = JsonConvert.DeserializeObject<Team>(data);
                LicenseSessionState.Instance.TeamList.RemoveAll(f => f.Id == id);
                LicenseSessionState.Instance.TeamList.Add(teamObj);
                return RedirectToAction("TeamList");
            }
            else
            {
                var message = response.ReasonPhrase + " - " + response.Content.ReadAsStringAsync().Result;
                ModelState.AddModelError("", message);
            }
            model.Id = id;
            return View("Edit", model);
        }

        [HttpGet]
        public ActionResult DeleteTeam(int id)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.DeleteAsync("api/team/Delete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                LicenseSessionState.Instance.TeamList.RemoveAll(t => t.Id == id);
                return RedirectToAction("TeamList");
            }
            else
            {
                var message = response.ReasonPhrase + " - " + response.Content.ReadAsStringAsync().Result;
                ModelState.AddModelError("", message);
            }
            return View();
        }

        public ActionResult Subscriptions()
        {
            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminUserId = LicenseSessionState.Instance.User.UserId;
            var subscriptionList = OnPremiseSubscriptionLogic.GetSubscription(adminUserId).AsEnumerable();
            return View(subscriptionList);
        }
    }
}