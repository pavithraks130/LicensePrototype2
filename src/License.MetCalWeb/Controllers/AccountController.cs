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
using Newtonsoft.Json;

namespace License.MetCalWeb.Controllers
{
    public class AccountController : BaseController
    {

        ServiceType webAPiType;
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

        public AccountController()
        {
            string serviceType = System.Configuration.ConfigurationManager.AppSettings.Get("ServiceType");
            webAPiType = (ServiceType)Enum.Parse(typeof(ServiceType), serviceType);
        }

        public ActionResult Register()
        {
            ViewData["SucessMessageDisplay"] = false;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            ViewData["SucessMessageDisplay"] = false;
            if (ModelState.IsValid)
            {

                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
                var response = await client.PostAsJsonAsync("api/user/create", model);
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var datamodel = JsonConvert.DeserializeObject<UserModel>(jsonData);

                    client.Dispose();
                    model.ServerUserId = datamodel.UserId;
                    client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                    response = await client.PostAsJsonAsync("api/user/create", model);
                    if (response.IsSuccessStatusCode)
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
                }
                else
                    ModelState.AddModelError("", response.ReasonPhrase);
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
            UserModel user = null;
            string data = null;
            if (ModelState.IsValid)
            {
                MetCalWeb.Models.UserModel userObj = new Models.UserModel();

                // Authentication is supparated for the On Premises user and Centralized User. Global Admin will  be authenticate with Centralised DB 
                // and on premises user and admin will be authenticated with on premise DB

                HttpClient client = WebApiServiceLogic.CreateClient(webAPiType.ToString());
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
                    switch (webAPiType)
                    {
                        case ServiceType.CentralizeWebApi: LicenseSessionState.Instance.CentralizedToken = token; break;
                        case ServiceType.OnPremiseWebApi: LicenseSessionState.Instance.OnPremiseToken = token; break;
                    }
                    client.Dispose();
                    client = WebApiServiceLogic.CreateClient(webAPiType.ToString());
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);
                    response = await client.GetAsync("api/user/UserById/" + token.Id);
                    if (response.IsSuccessStatusCode)
                    {
                        var userJson = response.Content.ReadAsStringAsync().Result;
                        client.Dispose();
                        user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserModel>(userJson);
                        if (user.Roles.Contains("SuperAdmin"))
                        {
                            client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
                            response = await client.PostAsync("AUthenticate", formContent);
                            if (response.IsSuccessStatusCode)
                            {
                                data = response.Content.ReadAsStringAsync().Result;
                                var token1 = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessToken>(data);
                                LicenseSessionState.Instance.CentralizedToken = token1;
                            }
                        }
                    }
                    LicenseSessionState.Instance.User = user;
                    LicenseSessionState.Instance.IsGlobalAdmin = LicenseSessionState.Instance.User.Roles.Contains("BackendAdmin");
                    LicenseSessionState.Instance.IsSuperAdmin = LicenseSessionState.Instance.User.Roles.Contains("SuperAdmin");
                    if (LicenseSessionState.Instance.IsSuperAdmin)
                        LicenseSessionState.Instance.IsAdmin = true;
                    else
                        LicenseSessionState.Instance.IsAdmin = LicenseSessionState.Instance.User.Roles.Contains("Admin");

                    if (!LicenseSessionState.Instance.IsGlobalAdmin && !LicenseSessionState.Instance.IsAdmin)
                        LicenseSessionState.Instance.IsTeamMember = true;
                    if (!LicenseSessionState.Instance.IsSuperAdmin)
                    {
                        TeamMemberLogic tmLogic = new TeamMemberLogic();
                        LicenseSessionState.Instance.AdminId = tmLogic.GetUserAdminDetails(LicenseSessionState.Instance.User.UserId);
                    }
                    SignInAsync(userObj, true);
                    if (LicenseSessionState.Instance.IsSuperAdmin)
                        SynchPurchaseOrder();
                    LicenseSessionState.Instance.IsAuthenticated = true;
                    if (String.IsNullOrEmpty(userObj.FirstName))
                        return RedirectToAction("Profile", "User");
                    if (LicenseSessionState.Instance.IsGlobalAdmin)
                        return RedirectToAction("Index", "User");
                    return RedirectToAction("Home", "Tab");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                }
            }
            return View();
        }

        public async Task SynchPurchaseOrder()
        {
            HttpClient client = WebApiServiceLogic.CreateClient(webAPiType.ToString());
            client.DefaultRequestHeaders.Add("Authorization", LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.PostAsync("api/purchaseorder/syncpo/" + LicenseSessionState.Instance.User.ServerUserId, null);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<UserSubscriptionList>(jsonData);
                if (obj.SubscriptionList.Count > 0)
                    CentralizedSubscriptionLogic.UpdateSubscriptionOnpremise(obj);
            }

        }

        private void SignInAsync(UserModel user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            System.Security.Claims.ClaimsIdentity identity = new System.Security.Claims.ClaimsIdentity();
            identity.AddClaim(new System.Security.Claims.Claim("username", LicenseSessionState.Instance.User.UserName.ToString()));
            identity.AddClaim(new System.Security.Claims.Claim("userId", LicenseSessionState.Instance.User.UserId.ToString()));
            identity.AddClaim(new System.Security.Claims.Claim("Role", LicenseSessionState.Instance.User.Roles.ToString()));
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = WebApiServiceLogic.CreateClient(webAPiType.ToString());
                var response = client.GetAsync("api/user/GetResetToken" + model.Email).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var user = JsonConvert.DeserializeObject<ForgotPasswordToken>(jsonData);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.UserId, code = user.Token }, protocol: Request.Url.Scheme);
                    EmailService service = new EmailService();
                    service.SendEmail(model.Email, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                    ViewBag.Message = "Mail has been sent to the specified email address to reset the password.  !!!!!";
                }
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
        public ActionResult ResetPassword(ResetPassword model, string userId, string code)
        {
            string email = string.Empty;
            if (ModelState.IsValid)
            {
                model.Token = code;
                model.UserId = userId;
                HttpClient client = WebApiServiceLogic.CreateClient(webAPiType.ToString());
                var response = client.PostAsJsonAsync("api/user/ResetPassword", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsondata = response.Content.ReadAsStringAsync().Result;
                    var user = JsonConvert.DeserializeObject<UserModel>(jsondata);
                    if (user != null && !String.IsNullOrEmpty(user.ServerUserId))
                    {
                        client.Dispose();
                        client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
                        model.UserId = user.ServerUserId;
                        model.Token = string.Empty;
                        response = client.PostAsJsonAsync("api/user/ResetPassword", model).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError("", response.ReasonPhrase);
                            return View();
                        }
                    }
                    ViewBag.Display = "inline";
                    ViewBag.ResetMessage = "Success";
                }
                else
                    ModelState.AddModelError("", response.ReasonPhrase);
            }
            return View();
        }

        public ActionResult LogOut()
        {

            HttpClient client = WebApiServiceLogic.CreateClient(webAPiType.ToString());
            UserModel userModel = new UserModel();
            userModel.UserId = LicenseSessionState.Instance.User.ServerUserId;
            userModel.IsActive = false;
            switch (webAPiType)
            {
                case ServiceType.CentralizeWebApi: client.DefaultRequestHeaders.Add("Authorize", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token); break;
                case ServiceType.OnPremiseWebApi: client.DefaultRequestHeaders.Add("Authorize", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token); break;
            }
            client.PostAsJsonAsync("api/user/UpdateActiveStatus", userModel);

            if (LicenseSessionState.Instance.IsSuperAdmin)
            {
                client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
                userModel.UserId = LicenseSessionState.Instance.User.ServerUserId;
                client.DefaultRequestHeaders.Add("Authorize", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
                client.PostAsJsonAsync("api/user/UpdateActiveStatus", userModel);
            }
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
