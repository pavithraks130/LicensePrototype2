using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;
using License.Logic.ServiceLogic;
using License.MetCalWeb;
using License.MetCalWeb.Models;
using License.Model.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebApplication1.Controllers
{
    public class TeamController : Controller
    {
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;


        public TeamController()
        {
            logic = new TeamMemberLogic();
            userLogic = new UserLogic();
        }
        // GET: Team
        public ActionResult TeamContainer()
        {
            return View();
        }

        public ActionResult TeamMembers()
        {
            License.Model.Model.UserInviteList inviteList = new UserInviteList();
            string adminId = string.Empty;
            if (LicenseSessionState.Instance.User.Roles.Contains("Admin"))
                adminId = LicenseSessionState.Instance.User.UserId;
            else
                adminId = logic.GetUserAdminDetails(LicenseSessionState.Instance.User.UserId);
            if (!String.IsNullOrEmpty(adminId))
                inviteList = logic.GetUserInviteDetails(adminId);
            return View(inviteList);
        }

        public ActionResult Subscriptions()
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
            //if (ModelState.IsValid)
            //{
            //    if (userLogic.UserManager == null)
            //        userLogic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
            //    if (userLogic.RoleManager == null)
            //        userLogic.RoleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();
            //    model.RegistratoinModel.OrganizationName = LicenseSessionState.Instance.User.Organization.Name;
            //    model.Password = (string)System.Configuration.ConfigurationManager.AppSettings.Get("InvitePassword");
            //    var result = userLogic.CreateUser(model.RegistratoinModel, "TeamMember");
            //    if (result.Succeeded)
            //    {
            //        AppUser user = userLogic.UserManager.FindByEmail(model.Email);
            //        TeamMembers invite = new TeamMembers();
            //        invite.AdminId = LicenseSessionState.Instance.User.UserId;
            //        invite.InviteeUserId = user.Id;
            //        invite.InvitationDate = DateTime.Now.Date;
            //        invite.InviteeEmail = model.Email;
            //        invite.InviteeStatus = InviteStatus.Pending.ToString();
            //        invite.TeamId = user.OrganizationId;
            //        var data = logic.CreateInvite(invite);
            //        if (data.Id > 0)
            //        {
            //            string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/Invitation.htm"));
            //            body = body.Replace("{{AdminEmail}}", LicenseSessionState.Instance.User.Email);
            //            string encryptString = user.OrganizationId + "," + invite.AdminId + "," + invite.Id;
            //            string passPhrase = System.Configuration.ConfigurationManager.AppSettings.Get("passPhrase");
            //            var dataencrypted = EncryptDecrypt.EncryptString(encryptString, passPhrase);
            //            string token = logic.UserManager.GenerateEmailConfirmationToken(user.Id);

            //            string joinUrl = Url.Action("Confirm", "Account",
            //                new { invite = dataencrypted, status = InviteStatus.Accepted.ToString(), token = token });
            //            string declineUrl = Url.Action("Confirm", "Account",
            //                new { invite = dataencrypted, status = InviteStatus.Declined.ToString(), token = token });

            //            body = body.Replace("{{JoinUrl}}", joinUrl);
            //            body = body.Replace("{{DeclineUrl}}", declineUrl);
            //            body = body.Replace("{{UserName}}", model.Email);
            //            body = body.Replace("{{Password}}", model.Password);

            //            userLogic.UserManager.SendEmail(user.Id, "Invite to fluke Calibration", body);
            //        }
            //    }
            //}
            return View("Dashboard");
        }
    }
}