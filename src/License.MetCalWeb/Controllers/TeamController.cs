﻿using System;
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
        // [ValidateAntiForgeryToken]
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