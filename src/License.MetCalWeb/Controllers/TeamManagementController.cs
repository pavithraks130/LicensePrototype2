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
    public class TeamManagementController : Controller
    {
        // GET: Team
        public ActionResult TeamContainer()
        {
            string userId = string.Empty;
            List<Team> teamList = null;
            if (!LicenseSessionState.Instance.IsSuperAdmin)
                userId = LicenseSessionState.Instance.User.UserId;
            if (LicenseSessionState.Instance.TeamList == null || LicenseSessionState.Instance.TeamList.Count == 0)
            {
                teamList = OnPremiseSubscriptionLogic.GetTeamList(userId);
                LicenseSessionState.Instance.TeamList = teamList;
            }
            if (LicenseSessionState.Instance.SelectedTeam == null)
            {
                var teamObj = LicenseSessionState.Instance.TeamList.FirstOrDefault(t => t.IsDefaultTeam == true);
                if (teamObj == null)
                    teamObj = LicenseSessionState.Instance.TeamList.First();
                ViewBag.SelectedTeamId = teamObj.Id;
            }
            else
                ViewBag.SelectedTeamId = LicenseSessionState.Instance.SelectedTeam.Id;
            return View();
        }

        public ActionResult TeamMembers(int id)
        {
            TeamDetails model = LoadTeamMember(Convert.ToInt32(id));
            TempData["IsAdmin"] = model.AcceptedUsers.FirstOrDefault(f => f.InviteeUserId == LicenseSessionState.Instance.User.UserId).IsAdmin;
            return View(model);
        }

        private TeamDetails LoadTeamMember(int teamId)
        {
            TeamDetails model = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/Team/GetById/" + teamId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<TeamDetails>(jsonData);
                var SelectedTeam = new Team();
                SelectedTeam.AdminId = model.Team.AdminId;
                SelectedTeam.Id = model.Team.Id;
                SelectedTeam.Name = model.Team.Name;
                SelectedTeam.TeamMembers = model.AcceptedUsers;
                LicenseSessionState.Instance.SelectedTeam = SelectedTeam;
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return model;
        }


        public ActionResult Invite(int teamId)
        {
            ViewBag.TeamId = teamId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invite(int teamId, UserInviteModel model)
        {
            bool status = false;
            if (ModelState.IsValid)
            {

                TeamMember invite = new TeamMember();
                invite.AdminId = LicenseSessionState.Instance.SelectedTeam.AdminId;
                invite.InvitationDate = DateTime.Now.Date;
                invite.InviteeEmail = model.Email;
                invite.TeamId = LicenseSessionState.Instance.SelectedTeam.Id;
                invite.InviteeStatus = InviteStatus.Pending.ToString();

                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                var response = client.PostAsJsonAsync("api/TeamMember/CreateInvite", invite).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    if (!String.IsNullOrEmpty(jsonData))
                    {
                        var teamMemResObj = JsonConvert.DeserializeObject<TeamMemberResponse>(jsonData);
                        string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/Invitation.htm"));
                        body = body.Replace("{{AdminEmail}}", LicenseSessionState.Instance.User.Email);
                        string encryptString = invite.AdminId + "," + teamMemResObj.TeamMemberId;
                        string passPhrase = System.Configuration.ConfigurationManager.AppSettings.Get("passPhrase");
                        var dataencrypted = EncryptDecrypt.EncryptString(encryptString, passPhrase);

                        string joinUrl = Url.Action("Confirm", "Account",
                            new { invite = dataencrypted, status = InviteStatus.Accepted.ToString() }, protocol: Request.Url.Scheme);
                        string declineUrl = Url.Action("Confirm", "Account",
                            new { invite = dataencrypted, status = InviteStatus.Declined.ToString() }, protocol: Request.Url.Scheme);

                        body = body.Replace("{{JoinUrl}}", joinUrl);
                        body = body.Replace("{{DeclineUrl}}", declineUrl);
                        body = body.Replace("{{UserName}}", model.Email);
                        body = body.Replace("{{Password}}", model.Password);
                        EmailService service = new EmailService();
                        service.SendEmail(model.Email, "Invite to fluke Calibration", body);

                        return RedirectToAction("TeamContainer");
                    }
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

        public ActionResult UserConfiguration(int id, string userId, string actionType)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            HttpResponseMessage response;
            TeamMember mem = new TeamMember()
            {
                Id = id,
                InviteeUserId = userId
            };
            switch (actionType)
            {
                case "Admin":
                    mem.IsAdmin = true;
                    response = client.PutAsJsonAsync("api/TeamMember/UpdateAdminAccess", mem).Result;
                    break;
                case "RemoveAdmin":
                    mem.IsAdmin = false;
                    response = client.PutAsJsonAsync("api/TeamMember/UpdateAdminAccess", mem).Result;
                    break;
                case "Remove":
                    response = client.DeleteAsync("api/TeamMember/DeleteInvite/" + id).Result;
                    break;
            }
            return RedirectToAction("TeamContainer");
        }

        public ActionResult Subscriptions()
        {
            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminUserId = LicenseSessionState.Instance.User.UserId;
            else
            {
                adminUserId = LicenseSessionState.Instance.SelectedTeam.AdminId;
            }
            var subscriptionList = OnPremiseSubscriptionLogic.GetSubscription(adminUserId).AsEnumerable();
            return View(subscriptionList);
        }

        public List<Team> LoadTeamsByUserId(string userId, string actiontype)
        {
            List<Team> teamList = null;
            var mappedTeams = OnPremiseSubscriptionLogic.GetTeamList(userId);
            if (actiontype == "Add")
                teamList = LicenseSessionState.Instance.TeamList.Where(t => teamList.Contains(t) == false).ToList();
            else
                teamList = mappedTeams;
            return teamList;
        }

        public bool UpdateOrRevokeTeam(String[] teamIds, string userId)
        {
            List<TeamMember> teamMembers = new List<TeamMember>();
            foreach (string teamId in teamIds)
            {
                TeamMember mem = new TeamMember()
                {
                    AdminId = LicenseSessionState.Instance.User.UserId,
                    InviteeStatus = Common.InviteStatus.Accepted.ToString(),
                    TeamId = Convert.ToInt32(teamId),
                    InviteeUserId = userId
                };
                teamMembers.Add(mem);
            }

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("", teamMembers).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + data.Message);
                return false;
            }
            return true;
        }

    }
}