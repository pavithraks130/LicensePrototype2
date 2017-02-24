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
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;
        private Product product = null;

        public DashboardController()
        {
            logic = new TeamMemberLogic();
            userLogic = new UserLogic();
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            var model = new List<Product>();
            product = new Product();
            product.CategoryID = 10;
            product.ImagePath = "~/Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product.ProductID = 123;
            product.ProductName = "Calibrator";
            product.UnitPrice = 499;
            model.Add(product);

            Product product1 = new Product();
            product1.CategoryID = 20;
            product1.ImagePath = "~/Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product1.ProductID = 123;
            product1.ProductName = "f-1730-hanger";
            product1.UnitPrice = 600;
            model.Add(product1);

            Product product3 = new Product();
            product3.CategoryID = 30;
            product3.ImagePath = "~/Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product3.ProductID = 123;
            product3.ProductName = "f-3501-fc";
            product3.UnitPrice = 588;
            model.Add(product3);

            return View(model);
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
                model.Password = (string)System.Configuration.ConfigurationManager.AppSettings.Get("InvitePassword");
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