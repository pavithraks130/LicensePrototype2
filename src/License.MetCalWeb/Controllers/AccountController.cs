using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using License.Logic.Common;
using License.Logic.ServiceLogic;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using License.Model;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Net.Http;

namespace License.MetCalWeb.Controllers
{
    public class AccountController : BaseController
    {
        private UserLogic logic = new UserLogic();
        LicenseServer.Logic.UserLogic userLogic = new LicenseServer.Logic.UserLogic();
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

                LicenseServer.Logic.UserLogic serUserLogic = new LicenseServer.Logic.UserLogic();
                LicenseServer.DataModel.Registration reg = new LicenseServer.DataModel.Registration();
                reg.Email = model.Email;
                reg.FirstName = model.FName;
                reg.LastName = model.LName;
                reg.OrganizationName = model.Organization;
                reg.Password = model.Password;
                reg.PhoneNumber = model.PhoneNumber;

                LicenseServer.Logic.UserTokenLogic tokenLogic = new LicenseServer.Logic.UserTokenLogic();
                var status = tokenLogic.VerifyUserToken(new LicenseServer.DataModel.UserToken() { Email = model.Email, Token = model.Token });
                if (!status)
                {
                    ModelState.AddModelError("", "Invalid Token Specified please verify the token");
                    return View();
                }

                //string servUserId = serUserLogic.CreateUser(reg);
                //model.RegistratoinModel.ServerUserId = servUserId;
                var result = logic.CreateUser(model.RegistratoinModel);
                if (result)
                {
                    User user = logic.GetUserByEmail(model.Email);
                    ViewData["SucessMessageDisplay"] = true;
                    FileStream stream = System.IO.File.Open(Server.MapPath("~/EmailTemplate/WelcometoFlukeCalibration.htm"), FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    string str = reader.ReadToEnd();
                    reader.Close();
                    stream.Close();
                    stream.Dispose();
                    EmailService service = new EmailService();
                    service.SendEmail(model.Email, "Welcome to Fluke", str);
                }
                else
                    ModelState.AddModelError("", logic.ErrorMessage);
            }
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(LoginViewModel model)
        {
            string data = null;
            if (ModelState.IsValid)
            {
                MetCalWeb.Models.UserModel userObj = new Models.UserModel();

                // Authentication is supparated for the On Premises user and Centralized User. Global Admin will  be authenticate with Centralised DB 
                // and on premises user and admin will be authenticated with on premise DB
                string serviceType = System.Configuration.ConfigurationManager.AppSettings.Get("ServiceType");
                HttpClient client = WebApiServiceLogic.CreateClient(serviceType);
                var formContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.Email),
                    new KeyValuePair<string, string>("password", model.Password)
                });
                var response = await client.PostAsync("Authenticate", formContent);
                if (response.IsSuccessStatusCode)
                {
                    data = response.Content.ReadAsStringAsync().Result;
                    var token = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessToken>(data);
                    client.Dispose();
                    client = WebApiServiceLogic.CreateClient(serviceType);
                }




                //// User user = logic.AuthenticateUser(model.Email, model.Password);
                // if (user != null)
                // {
                //     bool status = false;
                //     if (!String.IsNullOrEmpty(user.ServerUserId))
                //         status = userLogic.ValidateUser(model.Email, model.Password);

                //     userObj.Email = user.Email;
                //     userObj.FirstName = user.FirstName;
                //     userObj.LastName = user.LastName;
                //     userObj.Name = user.Name;
                //     userObj.PhoneNumber = user.PhoneNumber;
                //     userObj.Roles = user.Roles;
                //     userObj.ServerUserId = user.ServerUserId;
                //     userObj.UserId = user.UserId;
                //     userObj.UserName = user.UserName;


                // }
                // else
                // {

                //     var status = userLogic.ValidateUser(model.Email, model.Password);
                //     if (status)
                //     {
                //         var obj = userLogic.GetUserByEmail(model.Email);
                //         userObj.Email = obj.Email;
                //         userObj.FirstName = obj.FirstName;
                //         userObj.LastName = obj.LastName;
                //         userObj.ManagerId = String.Empty;
                //         userObj.Name = obj.Name;
                //         userObj.PhoneNumber = obj.PhoneNumber;
                //         userObj.Roles = obj.Roles;
                //         userObj.ServerUserId = String.Empty;
                //         userObj.UserId = obj.UserId;
                //         userObj.UserName = obj.UserName;
                //     }
                //     else
                //     {
                //         ModelState.AddModelError("", "invalid Credentials");
                //         return View();
                //     }
                // }
                // LicenseSessionState.Instance.User = userObj;
                // LicenseSessionState.Instance.IsGlobalAdmin = LicenseSessionState.Instance.User.Roles.Contains("BackendAdmin");
                // LicenseSessionState.Instance.IsSuperAdmin = LicenseSessionState.Instance.User.Roles.Contains("SuperAdmin");
                // if (LicenseSessionState.Instance.IsSuperAdmin)
                //     LicenseSessionState.Instance.IsAdmin = true;
                // else
                //     LicenseSessionState.Instance.IsAdmin = LicenseSessionState.Instance.User.Roles.Contains("Admin");

                // if (!LicenseSessionState.Instance.IsGlobalAdmin && !LicenseSessionState.Instance.IsAdmin)
                //     LicenseSessionState.Instance.IsTeamMember = true;
                // if (!LicenseSessionState.Instance.IsSuperAdmin)
                // {
                //     TeamMemberLogic tmLogic = new TeamMemberLogic();
                //     LicenseSessionState.Instance.AdminId = tmLogic.GetUserAdminDetails(LicenseSessionState.Instance.User.UserId);
                // }
                // SignInAsync(userObj, true);
                // if (LicenseSessionState.Instance.IsSuperAdmin)
                //     SynchPurchaseOrder();
                // LicenseSessionState.Instance.IsAuthenticated = true;
                // if (String.IsNullOrEmpty(userObj.FirstName))
                //     return RedirectToAction("Profile", "User");
                // if (LicenseSessionState.Instance.IsGlobalAdmin)
                //     return RedirectToAction("Index", "User");
                // return RedirectToAction("Home", "Tab");
            }
            return View();
        }

        public async Task SynchPurchaseOrder()
        {
            LicenseServer.Logic.PurchaseOrderLogic reqLogic = new LicenseServer.Logic.PurchaseOrderLogic();
            var poList = reqLogic.GetPOToBeSynchedByUser(LicenseSessionState.Instance.User.ServerUserId);
            foreach (var poItem in poList)
            {
                List<LicenseServer.DataModel.UserSubscription> subsList = new List<LicenseServer.DataModel.UserSubscription>();
                foreach (var item in poItem.OrderItems)
                {

                    LicenseServer.DataModel.UserSubscription usersubs = new LicenseServer.DataModel.UserSubscription();
                    usersubs.UserId = LicenseSessionState.Instance.User.ServerUserId;
                    usersubs.SubscriptionTypeId = item.SubscriptionId;
                    usersubs.SubscriptionDate = DateTime.Now.Date;
                    usersubs.Quantity = item.Quantity;
                    subsList.Add(usersubs);
                }
                CentralizedSubscriptionLogic.UpdateUserSubscription(subsList);
                poItem.IsSynched = true;
                reqLogic.UpdatePurchaseOrder(poItem);
            }

        }

        private void SignInAsync(UserModel user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            System.Security.Claims.ClaimsIdentity identity = null;
            if (LicenseSessionState.Instance.IsGlobalAdmin)
                identity = userLogic.CreateClaimsIdentity(LicenseSessionState.Instance.User.UserId);
            else
            {
                identity = logic.CreateClaimsIdentity(user.UserId);
            }
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
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
                var user = logic.ForgotPassword(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Enter Valid Email ");
                    return View();
                }

                string token = logic.CreateResetPasswordToken(user.UserId);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.UserId, code = token }, protocol: Request.Url.Scheme);
                EmailService service = new EmailService();
                service.SendEmail(model.Email, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Message = "Mail has been sent to the specified email address to reset the password.  !!!!!";
            }

            return View();
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

                var result = logic.ResetPassword(userId, code, model.Password);
                if (result)
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
                    ModelState.AddModelError("", logic.ErrorMessage);
            }
            return View();
        }

        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            if (LicenseSessionState.Instance.IsGlobalAdmin)
                userLogic.UpdateLogInStatus(LicenseSessionState.Instance.User.UserId, false);
            else if (LicenseSessionState.Instance.IsSuperAdmin)
            {
                userLogic.UpdateLogInStatus(LicenseSessionState.Instance.User.ServerUserId, false);
                logic.UpdateLogOutStatus(LicenseSessionState.Instance.User.UserId, false);
            }
            else
                logic.UpdateLogOutStatus(LicenseSessionState.Instance.User.UserId, false);
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
