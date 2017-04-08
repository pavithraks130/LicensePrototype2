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
        // GET: Team
        public ActionResult TeamContainer()
        {
            TeamDetails model = LoadTeamMember();
            return View(model);
        }

        public ActionResult TeamMembers()
        {
            return View();
        }



        private TeamDetails LoadTeamMember()
        {

            string adminId = string.Empty;
            TeamDetails model = null;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminId = LicenseSessionState.Instance.User.UserId;
            else
                adminId = LicenseSessionState.Instance.AdminId;
            if (!String.IsNullOrEmpty(adminId))
            {
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                var response = client.GetAsync("api/TeamMember/all/" + adminId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<TeamDetails>(jsonData);
                }
            }
            if (model == null)
            {
                return null;
            }
            return model;
        }

        public ActionResult Subscriptions()
        {
            if (!LicenseSessionState.Instance.IsAdmin)
                return View();

            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminUserId = LicenseSessionState.Instance.User.UserId;
            else
                adminUserId = LicenseSessionState.Instance.AdminId;

            LicenseSessionState.Instance.SubscriptionList = OnPremiseSubscriptionLogic.GetSubscription(adminUserId).AsEnumerable();
            return View(LicenseSessionState.Instance.SubscriptionList);
        }

        public ActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invite(UserInviteModel model)
        {
            bool status = false;
            if (ModelState.IsValid)
            {

                TeamMember invite = new TeamMember();
                invite.AdminId = LicenseSessionState.Instance.IsSuperAdmin ? LicenseSessionState.Instance.User.UserId : LicenseSessionState.Instance.AdminId;
                invite.InvitationDate = DateTime.Now.Date;
                invite.InviteeEmail = model.Email;
                invite.InviteeStatus = InviteStatus.Pending.ToString();

                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                var resposnse = client.PostAsJsonAsync("api/TeamMember/CreateInvite", invite).Result;
                if (resposnse.IsSuccessStatusCode)
                {
                    var jsonData = resposnse.Content.ReadAsStringAsync().Result;
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
                    }
                }
            }
            return RedirectToAction("TeamContainer");
        }

        public ActionResult UserConfiguration(int id, string userId, string actionType)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            HttpResponseMessage response;
            TeamMember mem = new TeamMember();
            mem.Id = id;
            mem.InviteeUserId = userId;
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
    }
}