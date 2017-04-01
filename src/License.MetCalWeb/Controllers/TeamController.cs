using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Logic.Common;
using License.Logic.ServiceLogic;
using License.MetCalWeb;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using License.Model;

using TeamMembers = License.Model.TeamMembers;
using System.Collections;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class TeamController : BaseController
    {
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;
        private UserSubscriptionLogic subscriptionLogic = null;
        private UserLicenseRequestLogic userLicenseRequestLogic = null;

        public TeamController()
        {
            logic = new TeamMemberLogic();
            userLogic = new UserLogic();
            subscriptionLogic = new UserSubscriptionLogic();
            userLicenseRequestLogic = new UserLicenseRequestLogic();
        }
        // GET: Team
        public ActionResult TeamContainer()
        {
            TeamModel model = LoadTeamMember();
            return View(model);
        }

        public ActionResult TeamMembers()
        {
            return View();
        }



        private TeamModel LoadTeamMember()
        {
            License.Model.UserInviteList inviteList = new UserInviteList();
            string adminId = string.Empty;
            TeamModel model = null;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminId = LicenseSessionState.Instance.User.UserId;
            else
                adminId = LicenseSessionState.Instance.AdminId;
            if (!String.IsNullOrEmpty(adminId))
            {
                inviteList = logic.GetUserInviteDetails(adminId);
                model = new TeamModel();
                model.AdminUser = inviteList.AdminUser;
                model.AcceptedUsers = inviteList.AcceptedInvites;
                model.PendinigUsers = inviteList.PendingInvites;
            }
            if (model == null)
            {
                return null;
            }
            if (model.AcceptedUsers.Count <= 0 || LicenseSessionState.Instance.IsTeamMember)
                return model;
            return model;
        }

        public ActionResult Subscriptions()
        {
            if (!LicenseSessionState.Instance.IsAdmin)
                return View();

            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            if (LicenseSessionState.Instance.IsAdmin)
                adminUserId = LicenseSessionState.Instance.User.UserId;
            else
            {
                License.Logic.ServiceLogic.TeamMemberLogic teamMemlogic = new TeamMemberLogic();
                adminUserId = teamMemlogic.GetUserAdminDetails(LicenseSessionState.Instance.User.UserId);
            }

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
                if (userLogic.GetUserByEmail(model.Email) == null)
                {
                    model.Password = (string)System.Configuration.ConfigurationManager.AppSettings.Get("InvitePassword");
                    status = userLogic.CreateUser(model.RegistratoinModel, "TeamMember");

                }
                else
                    status = true;

                if (status)
                {
                    User user = userLogic.GetUserByEmail(model.Email);
                    TeamMembers invite = new TeamMembers();
                    if (LicenseSessionState.Instance.IsSuperAdmin)
                        invite.AdminId = LicenseSessionState.Instance.User.UserId;
                    else
                        invite.AdminId = LicenseSessionState.Instance.AdminId;
                    invite.InviteeUserId = user.UserId;
                    invite.InvitationDate = DateTime.Now.Date;
                    invite.InviteeEmail = model.Email;
                    invite.InviteeStatus = InviteStatus.Pending.ToString();
                    var data = logic.CreateInvite(invite);
                    if (data.Id > 0)
                    {
                        string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/Invitation.htm"));
                        body = body.Replace("{{AdminEmail}}", LicenseSessionState.Instance.User.Email);
                        string encryptString = invite.AdminId + "," + data.Id;
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
                else
                {
                    ModelState.AddModelError("", logic.ErrorMessage);
                    return View();
                }
            }
            return RedirectToAction("TeamContainer");
        }

        public ActionResult UserConfiguration(int id, string userId, string actionType)
        {
            TeamMemberLogic logic = new TeamMemberLogic();
            switch (actionType)
            {
                case "Admin":
                    logic.SetAsAdmin(id, userId, true);
                    break;
                case "RemoveAdmin":
                    logic.SetAsAdmin(id, userId, false);
                    break;
                case "Remove":
                    logic.DeleteTeamMember(id);
                    break;
            }
            return RedirectToAction("TeamContainer");
        }
    }
}