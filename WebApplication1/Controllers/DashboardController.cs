using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;
using License.Core.Migrations;
using License.Core.Model;
using License.Logic.Common;
using License.Logic.ServiceLogic;
using License.MetCalWeb;
using License.MetCalWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using License.MetCalWeb.Common;
using TeamMembers = License.Model.Model.TeamMembers;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private UserInviteLogic logic = null;
        private UserLogic userLogic = null;

        public DashboardController()
        {
            logic = new UserInviteLogic();
            userLogic = new UserLogic();
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invite(UserInviteModel model)
        {
            if (ModelState.IsValid)
            {
                if (userLogic.UserManager == null)
                    userLogic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                if (userLogic.RoleManager == null)
                    userLogic.RoleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();
                model.RegistratoinModel.OrganizationName = LicenseSessionState.Instance.User.Organization.Name;
                model.Password = (string) System.Configuration.ConfigurationManager.AppSettings.Get("InvitePassword");
                var result = userLogic.CreateUser(model.RegistratoinModel, "TeamMember");
                if (result.Succeeded)
                {
                    AppUser user = userLogic.UserManager.FindByEmail(model.Email);
                    TeamMembers invite = new TeamMembers();
                    invite.AdminId = LicenseSessionState.Instance.User.UserId;
                    invite.InviteeUserId = user.Id;
                    invite.InvitationDate = DateTime.Now.Date;
                    invite.InviteeEmail = model.Email;
                    invite.InviteeStatus = InviteStatus.Pending.ToString();
                    invite.TeamId = user.OrganizationId;
                    var data = logic.CreateInvite(invite);
                    if (data.Id > 0)
                    {
                        string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/Invitation.htm"));
                        body = body.Replace("{{AdminEmail}}", LicenseSessionState.Instance.User.Email);
                        string encryptString = user.OrganizationId + "," + invite.AdminId + "," + invite.Id;
                        string passPhrase = System.Configuration.ConfigurationManager.AppSettings.Get("passPhrase");
                        var dataencrypted = EncryptDecrypt.EncryptString(encryptString, passPhrase);
                        string token = logic.UserManager.GenerateEmailConfirmationToken(user.Id);

                        string joinUrl = Url.Action("Confirm", "Account",
                            new { invite = dataencrypted, status = InviteStatus.Accepted.ToString(), token = token });
                        string declineUrl = Url.Action("Confirm", "Account",
                            new { invite = dataencrypted, status = InviteStatus.Declined.ToString(), token = token });

                        body = body.Replace("{{JoinUrl}}", joinUrl);
                        body = body.Replace("{{DeclineUrl}}", declineUrl);
                        body = body.Replace("{{UserName}}", model.Email);
                        body = body.Replace("{{Password}}", model.Password);

                        userLogic.UserManager.SendEmail(user.Id, "Invite to fluke Calibration", body);
                    }
                }
            }
            return View("Dashboard");
        }
    }
}