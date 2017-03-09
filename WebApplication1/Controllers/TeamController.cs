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

namespace WebApplication1.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;
        private UserSubscriptionLogic subscriptionLogic = null;
        List<LicenseMapModel> licenseMapModelList;
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
                logic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();

            if (logic.RoleManager == null)
                logic.RoleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();

            if (LicenseSessionState.Instance.User.Roles.Contains("Admin"))
            {
                adminId = LicenseSessionState.Instance.User.UserId;
                LicenseSessionState.Instance.IsTeamAdmin = true;
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
            if (model.AcceptedUsers.Count <= 0 || LicenseSessionState.Instance.IsTeamAdmin)
                return model;
            var obj =
                model.AcceptedUsers
                    .FirstOrDefault(t => t.InviteeUserId == LicenseSessionState.Instance.User.UserId);
            LicenseSessionState.Instance.IsTeamAdmin = obj?.IsAdmin ?? false;
            return model;
        }

        public ActionResult Subscriptions()
        {
            IList<License.MetCalWeb.Models.SubscriptionProductModel> subscriptionProList = new List<License.MetCalWeb.Models.SubscriptionProductModel>();
            ProductSubscriptionLogic proSubLogic = new ProductSubscriptionLogic();
            var dataList = proSubLogic.GetSubscriptionFromFile();
            var subscriptionList = subscriptionLogic.GetSubscription(LicenseSessionState.Instance.User.UserId);
            foreach (var userSub in subscriptionList)
            {
                var subType = dataList.FirstOrDefault(s => s.Id == userSub.SubscriptionId);
                License.MetCalWeb.Models.SubscriptionProductModel model = new SubscriptionProductModel();
                model.SubscriptionId = subType.Id;
                model.SubscriptionName = subType.SubscriptionName;
                foreach (var pro in subType.Product)
                {
                    UserLicenseLogic userLicLogic = new UserLicenseLogic();
                    int usedLicCount = userLicLogic.GetUserLicenseCount(userSub.Id, pro.Id);
                    model.ProductDtls.Add(new ProductDetails() { ProductId = pro.Id, ProductName = pro.Name, ProductCode = pro.ProductCode, TotalCount = (pro.QtyPerSubscription * userSub.Quantity), UsedLicenseCount = usedLicCount });
                }
                subscriptionProList.Add(model);
            }
            LicenseSessionState.Instance.SubscriptionList = subscriptionProList;
            return View(subscriptionProList);
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
            IdentityResult result;
            if (ModelState.IsValid)
            {
                //if(logic.VerifyUserInvited(model.Email, LicenseSessionState.Instance.User.UserId)!= null)
                //    return Json(new { success = false, Message = "User has already been invited" });

                if (userLogic.UserManager == null)
                    userLogic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                if (userLogic.RoleManager == null)
                    userLogic.RoleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();
                if (!userLogic.GetUserByEmail(model.Email))
                {
                    model.Password = (string)System.Configuration.ConfigurationManager.AppSettings.Get("InvitePassword");
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
            GetLicenseListBySubscription(userId);
            return View(LicenseSessionState.Instance.LicenseMapModelList);
        }

        public void GetLicenseListBySubscription(string userId)
        {
            TempData["UserId"] = userId;
            userLogic.UserManager = Request.GetOwinContext().Get<AppUserManager>();
            userLogic.RoleManager = Request.GetOwinContext().Get<AppRoleManager>();
            ViewData["TeamMember"] = userLogic.GetUserById(userId).Email;
            licenseMapModelList = new List<LicenseMapModel>();
            UserLicenseLogic logic = new UserLicenseLogic();
            var data = logic.GetUserLicense(userId);
            UserSubscriptionLogic subscriptionLogic = new UserSubscriptionLogic();
            var userSubscriptionList = subscriptionLogic.GetSubscription(LicenseSessionState.Instance.User.UserId);
            var subscriptionIdList = userSubscriptionList.Select(s => s.SubscriptionId);
            var subscriptionList = LicenseSessionState.Instance.SubscriptionList.Where(s => subscriptionIdList.Contains(s.SubscriptionId)).ToList();
            foreach (var subs in subscriptionList)
            {
                LicenseMapModel mapModel = new LicenseMapModel();
                mapModel.SubscriptionName = subs.SubscriptionName;
                mapModel.UserSubscriptionId = userSubscriptionList.FirstOrDefault(us => us.SubscriptionId == subs.SubscriptionId).Id;

                foreach (var pro in subs.ProductDtls)
                {
                    SubscriptionProduct prod = new SubscriptionProduct();
                    prod.ProductId = pro.ProductId;
                    prod.ProductName = pro.ProductName;
                    prod.IsDisabled = pro.AvailableCount == 0;
                    mapModel.ProductList.Add(prod);
                }
                licenseMapModelList.Add(mapModel);
            }
            LicenseSessionState.Instance.LicenseMapModelList = licenseMapModelList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapLicense(params string[] SelectedSubscription)
        {
            UpdateLicense(SelectedSubscription);
            return RedirectToAction("TeamContainer", "Team");
        }
        public ActionResult LicenseCart(string userId)
        {
            GetLicenseListBySubscription(userId);
            return View(LicenseSessionState.Instance.LicenseMapModelList);
        }

        public ActionResult RevokeLicense(string userId)
        {
            TempData["UserId"] = userId;
            userLogic.UserManager = Request.GetOwinContext().Get<AppUserManager>();
            userLogic.RoleManager = Request.GetOwinContext().Get<AppRoleManager>();
            ViewData["TeamMember"] = userLogic.GetUserById(userId).Email;
            licenseMapModelList = new List<LicenseMapModel>();
            UserLicenseLogic logic = new UserLicenseLogic();
            var data = logic.GetUserLicense(userId);
            if (data.Count > 0)
            {
                var subscriptionList = subscriptionLogic.GetSubscription(LicenseSessionState.Instance.User.UserId);
                foreach (var obj in subscriptionList)
                {
                    //var obj1 = data.FirstOrDefault(f => f.LicenseId == obj.LicenseDetails.LicenseId);
                    //if (obj1 != null)
                    //{
                    //    LicenseMapModel model = new LicenseMapModel();
                    //    model.IsDisabled = (obj.LicenseDetails.AvailableLicenseCount == 0);
                    //    model.UserSubscriptionId = obj.Id;
                    //    model.IsSelected = false;
                    //    var ul = data.FirstOrDefault(u => u.License.UserSubscriptionId == obj.Id);
                    //    if (ul != null)
                    //        model.ExistingUserLicenseId = ul.Id;
                    //    licenseMapModelList.Add(model);
                    //}
                }
                LicenseSessionState.Instance.LicenseMapModelList = licenseMapModelList;
            }
            return View(licenseMapModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevokeLicense(params string[] SelectedSubscription)
        {
            RevokeLicenseData(SelectedSubscription);
            return RedirectToAction("TeamContainer", "Team");
        }

        public void UpdateLicense(string[] SelectedSubscription)
        {
            string userId = Convert.ToString(TempData["UserId"]);
            UserLicenseLogic logic = new UserLicenseLogic();
            var models = LicenseSessionState.Instance.LicenseMapModelList;
            foreach (var data in models)
            {
                data.IsSelected = SelectedSubscription.Contains(data.UserSubscriptionId.ToString());
                if (data.IsSelected != data.InitialSelected)
                {
                    if (data.IsSelected)
                    {
                        License.Model.UserLicense lic = new License.Model.UserLicense();
                        lic.UserId = userId;
                        logic.CreateUserLicense(lic);
                    }
                }
            }
            if (SelectedSubscription.Count() > 0)
                logic.save();
        }

        public void RevokeLicenseData(string[] SelectedSubscription)
        {
            string userId = Convert.ToString(TempData["UserId"]);
            UserLicenseLogic logic = new UserLicenseLogic();
            var models = LicenseSessionState.Instance.LicenseMapModelList;
            foreach (var data in models)
            {
                data.IsSelected = SelectedSubscription.Contains(data.UserSubscriptionId.ToString());
                if (data.IsSelected)
                {
                    //LicenseSessionState.Instance.SubscriptionList.FirstOrDefault(f => f.Id == data.UserSubscriptionId).LicenseDetails.UsedLicenseCount -= 1;
                    //logic.RemoveById(data.ExistingUserLicenseId);
                }
            }
            if (SelectedSubscription.Count() > 0)
                logic.save();
        }
    }
}