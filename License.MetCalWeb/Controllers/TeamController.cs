using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;
using License.Core.Model;
using License.Logic.Common;
using License.Logic.ServiceLogic;
using License.MetCalWeb;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using License.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TeamMembers = License.Model.TeamMembers;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class TeamController : BaseController
    {
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;
        private UserSubscriptionLogic subscriptionLogic = null;

        public TeamController()
        {
            logic = new TeamMemberLogic();
            userLogic = new UserLogic();
            subscriptionLogic = new UserSubscriptionLogic();


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
            if (logic.UserManager == null)
                logic.UserManager = UserManager;

            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;

            TempData["IsTeamAdmin"] = false;
            if (LicenseSessionState.Instance.IsAdmin)
            {
                adminId = LicenseSessionState.Instance.User.UserId;
                TempData["IsTeamAdmin"] = true;
            }
            else
                adminId = logic.GetUserAdminDetails(LicenseSessionState.Instance.User.UserId);
            if (!String.IsNullOrEmpty(adminId))
            {
                inviteList = logic.GetUserInviteDetails(adminId);
                model = new TeamModel();
                model.AdminUser = inviteList.AdminUser;
                model.AcceptedUsers = inviteList.AcceptedInvites;
                model.PendinigUsers = inviteList.PendingInvites;
            }
            if (model.AcceptedUsers.Count <= 0 || Convert.ToBoolean(TempData["IsTeamAdmin"]))
                return model;
            var obj =
                model.AcceptedUsers
                    .FirstOrDefault(t => t.InviteeUserId == LicenseSessionState.Instance.User.UserId);
            TempData["IsTeamAdmin"] = obj?.IsAdmin ?? false;
            return model;
        }

        public ActionResult Subscriptions()
        {
            if (!Convert.ToBoolean(TempData["IsTeamAdmin"]))
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

            LicenseSessionState.Instance.SubscriptionList = SubscriLogic.GetSubscription(adminUserId).AsEnumerable();
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
            if (userLogic.UserManager == null)
                userLogic.UserManager = UserManager;
            if (userLogic.RoleManager == null)
                userLogic.RoleManager = RoleManager;
            IdentityResult result;
            if (ModelState.IsValid)
            {
                if (userLogic.GetUserByEmail(model.Email) == null)
                {
                    model.Password = (string)System.Configuration.ConfigurationManager.AppSettings.Get("InvitePassword");
                    model.RegistratoinModel.ManagerId = LicenseSessionState.Instance.User.UserId;
                    result = userLogic.CreateUser(model.RegistratoinModel, "TeamMember");
                    status = result.Succeeded;
                }
                else
                    status = true;

                if (status)
                {
                    AppUser user = userLogic.UserManager.FindByEmail(model.Email);
                    TeamMembers invite = new TeamMembers();
                    invite.AdminId = LicenseSessionState.Instance.User.UserId;
                    invite.InviteeUserId = user.Id;
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
                        string token = userLogic.UserManager.GenerateEmailConfirmationToken(user.Id);

                        string joinUrl = Url.Action("Confirm", "Account",
                            new { invite = dataencrypted, status = InviteStatus.Accepted.ToString() }, protocol: Request.Url.Scheme);
                        string declineUrl = Url.Action("Confirm", "Account",
                            new { invite = dataencrypted, status = InviteStatus.Declined.ToString() }, protocol: Request.Url.Scheme);

                        body = body.Replace("{{JoinUrl}}", joinUrl);
                        body = body.Replace("{{DeclineUrl}}", declineUrl);
                        body = body.Replace("{{UserName}}", model.Email);
                        body = body.Replace("{{Password}}", model.Password);

                        userLogic.UserManager.SendEmail(user.Id, "Invite to fluke Calibration", body);
                    }
                }
            }
            return RedirectToAction("TeamContainer");
        }

        public ActionResult MapLicense(string userId)
        {
            var listdata = GetLicenseListBySubscription(userId);
            return View(listdata);
        }

        public List<LicenseMapModel> GetLicenseListBySubscription(string userId)
        {
            TempData["UserId"] = userId;
            if (userLogic.UserManager == null)
                userLogic.UserManager = UserManager;
            if (userLogic.RoleManager == null)
                userLogic.RoleManager = RoleManager;
            ViewData["TeamMember"] = userLogic.GetUserById(userId).Email;

            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            if (LicenseSessionState.Instance.IsAdmin)
                adminUserId = LicenseSessionState.Instance.User.UserId;
            else
            {
                License.Logic.ServiceLogic.TeamMemberLogic teamMemlogic = new TeamMemberLogic();
                adminUserId = teamMemlogic.GetUserAdminDetails(LicenseSessionState.Instance.User.UserId);
            }

            var licenseMapModelList = SubscriLogic.GetSubForLicenseMap(userId, adminUserId);
            return licenseMapModelList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapLicense(params string[] SelectedSubscription)
        {
            UpdateRevokeLicense(SelectedSubscription);
            return RedirectToAction("TeamContainer", "Team");
        }
        public ActionResult LicenseCart(string userId)
        {
            var listdata = GetLicenseListBySubscription(userId);
            return View(listdata);
        }

        public ActionResult RevokeLicense(string userId)
        {
            TempData["UserId"] = userId;
            userLogic.UserManager = UserManager;
            userLogic.RoleManager = RoleManager;
            ViewData["TeamMember"] = userLogic.GetUserById(userId).Email;
            List<LicenseMapModel> licenseMapModelList = SubscriLogic.GetUserLicenseDetails(userId, false);
            return View(licenseMapModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevokeLicense(params string[] SelectedSubscription)
        {
            UpdateRevokeLicense(SelectedSubscription, "Revoke");
            return RedirectToAction("TeamContainer", "Team");
        }

        public void UpdateRevokeLicense(string[] SelectedSubscription, string action = "Add")
        {
            string userId = Convert.ToString(TempData["UserId"]);
            UserLicenseLogic logic = new UserLicenseLogic();
            License.Logic.ServiceLogic.LicenseLogic licenseLogic = new LicenseLogic();
            List<License.Model.UserLicense> userLicesList = new List<License.Model.UserLicense>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];
                License.Model.UserLicense lic = new License.Model.UserLicense();
                lic.UserId = userId;
                License.Model.LicenseData licData = new License.Model.LicenseData();
                licData.UserSubscriptionId = Convert.ToInt32(subscriptionId);
                licData.ProductId = Convert.ToInt32(prodId);
                lic.License = licData;
                userLicesList.Add(lic);
            }
            if (action == "Add")
                logic.CreateUserLicense(userLicesList, userId);
            else
                logic.RevokeUserLicense(userLicesList, userId);
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