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
using License.Logic.ServiceLogic;
using License.MetCalWeb.Models;
using License.Model.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace License.MetCalWeb.Controllers
{
    public class AccountController : BaseController
    {
        private UserLogic logic = new UserLogic();

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
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
                    logic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                if (logic.RoleManager == null)
                    logic.RoleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();
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
                    logic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                if (logic.RoleManager == null)
                    logic.RoleManager = Request.GetOwinContext().GetUserManager<AppRoleManager>();
                AppUser user = logic.AutheticateUser(model.Email, model.Password);
                if (user != null)
                {
                    SignInAsync(user, model.RememberMe);
                    LicenseSessionState.Instance.User = logic.GetUserDataByAppuser(user);
                    LicenseSessionState.Instance.IsAuthenticated = true;
                    return RedirectToAction("Index", "Home");
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
        public ActionResult ResetPassword(ResetPassword model, string userId, string code)
        {
            string email = string.Empty;
            if (ModelState.IsValid)
            {
                if (logic.UserManager == null)
                    logic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
                IdentityResult result = logic.ResetPassword(userId, code, model.Password);
                if (result.Succeeded)
                {
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
                    logic.UserManager = Request.GetOwinContext().GetUserManager<AppUserManager>();
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
            return View("Login");
        }
    }
}
