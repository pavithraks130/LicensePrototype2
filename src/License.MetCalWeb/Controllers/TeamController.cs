﻿using System;
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
            if (LicenseSessionState.Instance.IsSuperAdmin)
            {
                var userLicenseRequestList = userLicenseRequestLogic.GetRequestList(LicenseSessionState.Instance.User.UserId);
                ViewBag.LicenseRequestList = userLicenseRequestList;
            }
            return View();
        }

        [HttpPost]
        public ActionResult TeamMembers(UserLicenseRequest userLicense,bool isApproved)
        {
            if (isApproved)
            {
                userLicense.IsApproved = true;
            }
            else
            {
                userLicense.IsRejected = true;
            }
            userLicenseRequestLogic.Update(new List<UserLicenseRequest> {userLicense});
            return View();
        }
        private TeamModel LoadTeamMember()
        {
            License.Model.UserInviteList inviteList = new UserInviteList();
            string adminId = string.Empty;
            TeamModel model = null;
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
                //foreach(var user in model.AcceptedUsers)
                //{
                //    var result = SubscriLogic.GetUserLicenseDetails(user.InviteeUserId,false);
                //}
                model.PendinigUsers = inviteList.PendingInvites;
            }
            if (model == null)
                return null;
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
            if (ModelState.IsValid)
            {
                if (userLogic.GetUserByEmail(model.Email) == null)
                {
                    model.Password = (string)System.Configuration.ConfigurationManager.AppSettings.Get("InvitePassword");
                    model.RegistratoinModel.ManagerId = LicenseSessionState.Instance.User.UserId;
                    status = userLogic.CreateUser(model.RegistratoinModel, "TeamMember");

                }
                else
                    status = true;

                if (status)
                {
                    User user = userLogic.GetUserByEmail(model.Email);
                    TeamMembers invite = new TeamMembers();
                    invite.AdminId = LicenseSessionState.Instance.User.UserId;
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

        public ActionResult MapLicense(string userId, bool bulkLicenseAdd)
        {
            var listdata = GetLicenseListBySubscription(userId, bulkLicenseAdd);
            return View(listdata);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapLicense(params string[] SelectedSubscription)
        {
            UpdateRevokeLicense(SelectedSubscription);
            return RedirectToAction("TeamContainer", "Team");
        }

        /// <summary>
        /// It will store selected subscription list in  temp and disply user list for selection
        /// </summary>
        /// <param name="SelectedSubscription"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectListOfUser(params string[] SelectedSubscription)
        {
            TempData["SelectedSubscription"] = SelectedSubscription;
            var teamMember = LoadTeamMember();
            return View(teamMember);
        }
        [HttpPost]
        public ActionResult SelectedUsers(params string[] SelectedUser)
        {
            string[] temp = TempData["SelectedSubscription"] as string[];
            var teamMember = LoadTeamMember();
            UpdateRevokeLicense(temp, "Add", SelectedUser, true);
            return RedirectToAction("TeamContainer", "Team");
        }
        public List<License.MetCalWeb.Models.LicenseMapModel> GetLicenseListBySubscription(string userId, bool canAddBulkLicense)
        {
            TempData["UserId"] = userId;
            ViewData["TeamMember"] = userId == null ? string.Empty : userLogic.GetUserById(userId).Email;
            TempData["CanAddBulk"] = canAddBulkLicense;


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


        public ActionResult LicenseCart(string userId)
        {
            var listdata = GetLicenseListBySubscription(userId, false);
            return View(listdata);
        }

        public ActionResult RevokeLicense(string userId)
        {
            TempData["UserId"] = userId;
            ViewData["TeamMember"] = userLogic.GetUserById(userId).Email;
            List<Models.LicenseMapModel> licenseMapModelList = SubscriLogic.GetUserLicenseDetails(userId, false);
            return View(licenseMapModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevokeLicense(params string[] SelectedSubscription)
        {
            UpdateRevokeLicense(SelectedSubscription, "Revoke");
            return RedirectToAction("TeamContainer", "Team");
        }

        public void UpdateRevokeLicense(string[] SelectedSubscription, string action = "Add", string[] SelectedUserIdList = null, bool canAddBulkLicense = false)
        {
            List<string> userIdList = new List<string>();
            if (canAddBulkLicense)
            {
                foreach (var userId in SelectedUserIdList)
                {
                    userIdList.Add(userId);
                }
            }
            else
            {
                userIdList.Add(Convert.ToString(TempData["UserId"]));
            }
            UserLicenseLogic logic = new UserLicenseLogic();
            License.Logic.ServiceLogic.LicenseLogic licenseLogic = new LicenseLogic();
            List<License.Model.UserLicense> userLicesList = new List<License.Model.UserLicense>();
            foreach (var data in SelectedSubscription)
            {
                var splitValue = data.Split(new char[] { '-' });
                var prodId = splitValue[0].Split(new char[] { ':' })[1];
                var subscriptionId = splitValue[1].Split(new char[] { ':' })[1];
                License.Model.UserLicense lic = new License.Model.UserLicense();
                lic.UserId = string.Empty;//Multiple users adding user Id is not required here
                License.Model.LicenseData licData = new License.Model.LicenseData();
                licData.UserSubscriptionId = Convert.ToInt32(subscriptionId);
                licData.ProductId = Convert.ToInt32(prodId);
                lic.License = licData;
                userLicesList.Add(lic);
            }
            if (action == "Add")
                logic.CreateUserLicense(userLicesList, userIdList);
            else
                logic.RevokeUserLicense(userLicesList, userIdList.FirstOrDefault());//temporary data changes
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