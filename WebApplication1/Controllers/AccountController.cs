using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Owin.Security;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;
using License.Core.Model;
using License.Logic.Common;
using License.Logic.ServiceLogic;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using License.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace License.MetCalWeb.Controllers
{
    public class AccountController : BaseController
    {
        private UserLogic logic = new UserLogic();

        private IAuthenticationManager _authManager = null;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (_authManager == null)
                    _authManager = HttpContext.GetOwinContext().Authentication;
                return _authManager;
            }
        }

        public ActionResult Register()
        {
            ViewData["SucessMessageDisplay"] = false;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ViewData["SucessMessageDisplay"] = false;
            if (ModelState.IsValid)
            {
                if (logic.UserManager == null)
                    logic.UserManager = UserManager;
                if (logic.RoleManager == null)
                    logic.RoleManager = RoleManager;

                LicenseServer.Logic.UserLogic serUserLogic = new LicenseServer.Logic.UserLogic();
                LicenseServer.DataModel.Registration reg = new LicenseServer.DataModel.Registration();
                reg.Email = model.Email;
                reg.FirstName = model.FName;
                reg.LastName = model.LName;
                reg.OrganizationName = model.Organization;
                reg.Password = model.Password;
                reg.PhoneNumber = model.PhoneNumber;

                //LicenseServer.Logic.UserTokenLogic tokenLogic = new LicenseServer.Logic.UserTokenLogic();
                //var status = tokenLogic.VerifyUserToken(new LicenseServer.DataModel.UserToken() { Email = model.Email, Token = model.Token });
                //if (!status)
                //{
                //    ModelState.AddModelError("", "Invalid Token Specified please verify the token");
                //    return View();
                //}

                string servUserId = serUserLogic.CreateUser(reg);
                model.RegistratoinModel.ServerUserId = servUserId;
                IdentityResult result = logic.CreateUser(model.RegistratoinModel);
                if (result.Succeeded)
                {
                    AppUser user = logic.UserManager.FindByEmail(model.Email);
                    ViewData["SucessMessageDisplay"] = true;
                    FileStream stream = System.IO.File.Open(Server.MapPath("~/EmailTemplate/WelcometoFlukeCalibration.htm"), FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    string str = reader.ReadToEnd();
                    reader.Close();
                    stream.Close();
                    stream.Dispose();
                    await logic.UserManager.SendEmailAsync(user.Id, "Welcome to Fluke", str);
                }
                else
                    GetErrorResult(result);
            }
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (logic.UserManager == null)
                    logic.UserManager = UserManager;
                if (logic.RoleManager == null)
                    logic.RoleManager = RoleManager;
                AppUser user = logic.AutheticateUser(model.Email, model.Password);
                if (user != null)
                {
                    //Code need to be removed added only for verfication

                    LicenseServer.Logic.UserLogic userLogic = new LicenseServer.Logic.UserLogic();
                    var status = userLogic.ValidateUser(model.Email, model.Password);
                    SignInAsync(user, model.RememberMe);
                    LicenseSessionState.Instance.User = logic.GetUserDataByAppuser(user);
                    LicenseSessionState.Instance.IsSuperAdmin = LicenseSessionState.Instance.User.Roles.Contains("SuperAdmin");
                    if (LicenseSessionState.Instance.IsSuperAdmin)
                        LicenseSessionState.Instance.IsAdmin = true;
                    else
                        LicenseSessionState.Instance.IsAdmin = LicenseSessionState.Instance.User.Roles.Contains("Admin");
                    LicenseSessionState.Instance.IsAuthenticated = true;
                    SubscriLogic.GetUserLicenseForUser();
                    return RedirectToAction("Home", "Tab");
                }
                else
                {
                    ModelState.AddModelError("", "invalid Credentials");
                }
            }
            return View();
        }

        private void SignInAsync(AppUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = logic.UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        public ActionResult ResetPassword(string userId, string code)
        {
            License.MetCalWeb.Models.ResetPassword model = new ResetPassword();
            model.Token = code;
            model.UserId = userId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPassword model, string userId, string code)
        {
            string email = string.Empty;
            if (ModelState.IsValid)
            {
                if (logic.UserManager == null)
                    logic.UserManager = UserManager;
                IdentityResult result = logic.ResetPassword(userId, code, model.Password);
                if (result.Succeeded)
                {
                    var user = logic.GetUserById(userId);
                    if (!String.IsNullOrEmpty(user.ServerUserId))
                    {
                        LicenseServer.Logic.UserLogic userLogic = new LicenseServer.Logic.UserLogic();
                        var status = await userLogic.ResetPassword(user.ServerUserId, model.Password);
                    }
                    ViewBag.Display = "inline";
                    ViewBag.ResetMessage = "Success";
                }
                else
                    GetErrorResult(result);
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                if (logic.UserManager == null)
                    logic.UserManager = UserManager;
                var user = logic.ForgotPassword(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Enter Valid Email ");
                    return View();
                }
                string token = logic.UserManager.GeneratePasswordResetToken(user.UserId);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.UserId, code = token }, protocol: Request.Url.Scheme);
                await logic.UserManager.SendEmailAsync(user.UserId, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Message = "Mail has been sent to the specified email address to reset the password.  !!!!!";
            }

            return View();
        }

        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            System.Web.HttpContext.Current.Session.Clear();
            return RedirectToAction("LogIn");
        }

        public ActionResult Confirm(string invite, string status)
        {
            string passPhrase = System.Configuration.ConfigurationManager.AppSettings.Get("passPhrase");
            string data = EncryptDecrypt.DecryptString(invite, passPhrase);
            var details = data.Split(new char[] { ',' });

            string adminId = details[0];
            string inviteId = details[1];

            TeamMemberLogic logic = new TeamMemberLogic();
            logic.UpdateInviteStatus(Convert.ToInt32(inviteId), status);
            ViewBag.Message = String.Empty;
            License.Logic.Common.InviteStatus stat = (License.Logic.Common.InviteStatus)Enum.Parse(typeof(License.Logic.Common.InviteStatus), status);
            if (stat == InviteStatus.Accepted)
                ViewBag.Message = "You have accepted the invitation. Click below to Login with credentials which was shared through Mail";
            return View();
        }
    }
}
