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
    /// <summary>
    /// Controller is responsible for performing the team member functionalities
    /// </summary>
    [Authorize]
    [SessionExpire]
    public class TeamManagementController : Controller
    {
      /// <summary>
      /// Get action return a main view Container which   display the team Selectcion option based on which the Team membere will de displayed. If initialy the team List is empty the the Team List will
      /// be fetced from DB through service call
      /// </summary>
      /// <returns></returns>
        public ActionResult TeamContainer()
        {
            string userId = string.Empty;
           
            if (!LicenseSessionState.Instance.IsSuperAdmin)
                userId = LicenseSessionState.Instance.User.UserId;
            if (LicenseSessionState.Instance.TeamList == null || LicenseSessionState.Instance.TeamList.Count == 0)
            {
                var teamList = OnPremiseSubscriptionLogic.GetTeamList(userId);
                if (teamList == null || teamList.Count == 0)
                {
                    ViewBag.SelectedTeamId = "";
                    return View();
                }
                else
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
        
        /// <summary>
        /// Get ACtion returns view with list of team members based on the team ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TeamMembers(int id)
        {
            TeamDetails model = LoadTeamMember(Convert.ToInt32(id));
            return View(model);
        }
        /// <summary>
        /// returns the team details with list of team members whoc has accepted te request.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private TeamDetails LoadTeamMember(int teamId)
        {
            TeamDetails model = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
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

        /// <summary>
        /// GET Action returns view to invite user
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public ActionResult Invite(int teamId)
        {
            ViewBag.TeamId = teamId;
            return View();
        }

        /// <summary>
        /// Post Action triggers when user submit the pop up window form to create the Invite record and user Record in DB through Service call . Once service response is success then 
        /// the invitation mail sent to the user
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invite(int teamId, UserInviteModel model)
        {
            bool status = false;
            if (ModelState.IsValid)
            {

                TeamMember invite = new TeamMember();
                invite.InvitationDate = DateTime.Now.Date;
                invite.InviteeEmail = model.Email;
                invite.TeamId = LicenseSessionState.Instance.SelectedTeam.Id;
                invite.InviteeStatus = InviteStatus.Pending.ToString();

                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                var response = client.PostAsJsonAsync("api/TeamMember/CreateInvite", invite).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    if (!String.IsNullOrEmpty(jsonData))
                    {
                        var teamMemResObj = JsonConvert.DeserializeObject<TeamMemberResponse>(jsonData);
                        string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/Invitation.htm"));
                        body = body.Replace("{{AdminEmail}}", LicenseSessionState.Instance.User.Email);
                        string encryptString = invite.TeamId + "," + teamMemResObj.TeamMemberId;
                        string passPhrase = System.Configuration.ConfigurationManager.AppSettings.Get("passPhrase");
                        var dataencrypted = EncryptDecrypt.EncryptString(encryptString, passPhrase);

                        string joinUrl = Url.Action("Confirm", "Account",
                            new { invite = dataencrypted, status = InviteStatus.Accepted.ToString() }, protocol: Request.Url.Scheme);
                        string declineUrl = Url.Action("Confirm", "Account",
                            new { invite = dataencrypted, status = InviteStatus.Declined.ToString() }, protocol: Request.Url.Scheme);

                        body = body.Replace("{{JoinUrl}}", joinUrl);
                        body = body.Replace("{{DeclineUrl}}", declineUrl);
                        body = body.Replace("{{UserName}}", teamMemResObj.UserName);
                        body = body.Replace("{{Password}}", teamMemResObj.Password);
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

        /// <summary>
        /// Get Action which are called using ajax calls. To perform respective action based on the action Type and ,Based on the Team Member id and User Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public ActionResult UserConfiguration(int id, string userId, string actionType)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
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
                    response = client.DeleteAsync("api/TeamMember/DeleteTeamMember/" + id).Result;
                    break;

            }
            return RedirectToAction("TeamContainer");
        }

        /// <summary>
        /// Return view with the list of Team for assiging user to multiple team or list of team for which he has assigned to  revoke from multiple team.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public ActionResult AssignRevokeTeam(string userId, string actionType)
        {
            var teamModel = LoadTeamsByUserId(userId, actionType);
            ViewBag.UserId = userId;
            ViewBag.actionType = actionType;
            ViewBag.UserEmail = LicenseSessionState.Instance.SelectedTeam.TeamMembers.FirstOrDefault(t => t.InviteeUserId == userId).InviteeEmail;
            return View("AssignRevokeTeam", teamModel);
        }

        /// <summary>
        /// POST action to perform respecctie assign or revoke to team based on the action Type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="actionType"></param>
        /// <param name="selectedTeams"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRevokeTeam(string userId, string actionType, params string[] selectedTeams)
        {
            var status = UpdateOrRevokeTeam(selectedTeams, userId, actionType);
            if (status)
                return RedirectToAction("TeamContainer");
            return View();
        }

        /// <summary>
        /// Return View with the Subscriptiion  with product details  and available license count
        /// </summary>
        /// <returns></returns>
        public ActionResult Subscriptions()
        {
            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminUserId = LicenseSessionState.Instance.User.UserId;
            else
                adminUserId = LicenseSessionState.Instance.SelectedTeam.AdminId;
            var subscriptionList = OnPremiseSubscriptionLogic.GetSubscription(adminUserId).AsEnumerable();
            return View(subscriptionList);
        }
        /// <summary>
        /// Function to list the Team based on the user Id and ACtion Type by making service Call
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="actiontype"></param>
        /// <returns></returns>
        public List<Team> LoadTeamsByUserId(string userId, string actiontype)
        {
            List<Team> teamList = null;
            var mappedTeams = OnPremiseSubscriptionLogic.GetTeamList(userId);
            var existingTeamIdList = mappedTeams.Select(t => t.Id).ToList();
            if (actiontype == "AssignTeam")
                teamList = LicenseSessionState.Instance.TeamList.Where(t => !existingTeamIdList.Contains(t.Id) && t.AdminId == LicenseSessionState.Instance.SelectedTeam.AdminId).ToList();
            else 
                teamList = mappedTeams.Where(t=>t.IsDefaultTeam == false).ToList();
            return teamList;
        }

        /// <summary>
        /// Updating the Assign or Revoke of team based on the userId and TeamId
        /// </summary>
        /// <param name="teamIds"></param>
        /// <param name="userId"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public bool UpdateOrRevokeTeam(String[] teamIds, string userId, string actionType)
        {
            List<TeamMember> teamMembers = new List<TeamMember>();
            foreach (string teamId in teamIds)
            {
                TeamMember mem = new TeamMember()
                {
                    InviteeStatus = Common.InviteStatus.Accepted.ToString(),
                    TeamId = Convert.ToInt32(teamId),
                    InviteeUserId = userId,
                    InviteeEmail = LicenseSessionState.Instance.SelectedTeam.TeamMembers.FirstOrDefault(t => t.InviteeUserId == userId).InviteeEmail                    

                };
                teamMembers.Add(mem);
            }

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            string url;
            if (actionType == "AssignTeam")
                url = "api/TeamMember/CreateTeamMember";
            else
                url = "api/TeamMember/RemoveTeamMember";
            var response = client.PostAsJsonAsync(url, teamMembers).Result;
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