using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb.Common;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Net.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using License.Models;
using License.ServiceInvoke;

namespace License.MetCalWeb.Controllers
{
    /// <summary>
    /// User Autentication , Reset Password, Forgot password, Register actions are performed here.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        string ErrorMessage;
        private IAuthenticationManager _authManager = null;
        private Authentication _authentication = null;
        private APIInvoke _invoke = null;
        private OnPremiseSubscriptionLogic _onPremiseSubscriptionLogic = null;

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
            //_accountLogic = new AccountLogic();
            _authentication = new Authentication();
            _invoke = new APIInvoke();
            _onPremiseSubscriptionLogic = new OnPremiseSubscriptionLogic();
            _authentication.ApplicationSecretKey = System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationSecretKey");// "kXtpZlZCESG7F8jo+uVuaA==";
            _authentication.ApplicationCode = System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationCode");//"MetCal";
        }

        public ActionResult Register()
        {
            ViewBag.SucessMessageDisplay = false;
            return View();

        }

        /// <summary>
        /// POST Action : Action is called when user submits the Registration form. Restered User record will be created  in both Centralized and On Premise DB.
        /// 1. The User record will be created in Centralized and send the Centralized server User Id.
        /// 2. User Record will be created in the ON Premise Db with server User Id Updated to Server user Id Property
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Registration model)
        {
            ViewBag.SucessMessageDisplay = false;
            if (ModelState.IsValid)
            {
                var response = _authentication.Register<Registration>(model);
                if (response.Status)
                {
                    ViewBag.SucessMessageDisplay = true;
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
                    ModelState.AddModelError("", response.Error.Message);
                ////Invoking the Create User Service ofo the centralized Server
                //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
                //var response = await client.PostAsJsonAsync("api/user/create", model);
                //if (response.IsSuccessStatusCode)
                //{
                //    var jsonData = response.Content.ReadAsStringAsync().Result;
                //    var datamodel = JsonConvert.DeserializeObject<User>(jsonData);

                //    client.Dispose();
                //    model.ServerUserId = datamodel.UserId;
                //    //Invoking the Create User Service ofo the OnPremise Server
                //    client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                //    response = await client.PostAsJsonAsync("api/user/create", model);
                //    if (response.IsSuccessStatusCode)
                //    {


                //    }
                //}
                //else
                //{

                //}
            }
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        /// <summary>
        /// Post Actioon called on the Login Form Submission for suthetication
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(Login model)
        {
            User user = null;
            if (ModelState.IsValid)
            {
                var resposne = _authentication.LoginAuthentication<User>(model);
                if (!String.IsNullOrEmpty(resposne.ErrorMessage))
                {
                    ModelState.AddModelError("", "Error Code:"+resposne.ErrorMessage);
                    return View();
                }
                user = resposne.User;
                LicenseSessionState.Instance.OnPremiseToken = resposne.OnPremiseToken;
                LicenseSessionState.Instance.CentralizedToken = resposne.CentralizedToken;
                // Once the Authentication is performed and user  object returned is not equal to null , then the global session object are set  as shown below.
                //if (user == null)
                //    //ModelState.AddModelError("", _accountLogic.ErrorMessage);
                //    return View();
                LicenseSessionState.Instance.User = user;
                LicenseSessionState.Instance.IsGlobalAdmin = LicenseSessionState.Instance.User.Roles.Contains("Fluke Admin");
                LicenseSessionState.Instance.IsSuperAdmin = LicenseSessionState.Instance.User.Roles.Contains("Super Admin");

                if (LicenseSessionState.Instance.IsSuperAdmin)
                    LicenseSessionState.Instance.IsAdmin = true;
                else
                    LicenseSessionState.Instance.IsAdmin = LicenseSessionState.Instance.User.Roles.Contains("Tenant Admin");

                if (!LicenseSessionState.Instance.IsGlobalAdmin && !LicenseSessionState.Instance.IsAdmin)
                    LicenseSessionState.Instance.IsTeamMember = true;
                LicenseSessionState.UserId = user.UserId;
                LicenseSessionState.ServerUserId = user.ServerUserId;
                SignInAsync(user, true);

                // If the Logged user is  Super Admin then Async call is made to sync the Offline purchased Subscription if exist and approved by the global admin
                if (LicenseSessionState.Instance.IsSuperAdmin)
                {
                    AuthorizeBackendService service = new AuthorizeBackendService();
                    service.SyncDataToOnpremise();
                }
                LicenseSessionState.Instance.IsAuthenticated = true;

                if (LicenseSessionState.Instance.IsGlobalAdmin)
                {
                    if (String.IsNullOrEmpty(user.FirstName))
                        return RedirectToAction("Profile", "User");
                    if (LicenseSessionState.Instance.IsGlobalAdmin)
                        return RedirectToAction("Index", "User");
                    return RedirectToAction("Home", "Dashboard");
                }

                // Here the concurrent user login and Team Selection functionalities are executed.
                if (LicenseSessionState.Instance.TeamList == null || LicenseSessionState.Instance.TeamList.Count == 0)
                {
                    string userId = string.Empty;
                    if (!LicenseSessionState.Instance.IsSuperAdmin)
                        userId = LicenseSessionState.Instance.User.UserId;

                    // Get the Team list for which the logged in User belongs to  if the us3er belongs to more than one team then the pop up window is displayed for the 
                    // team selection
                    var teamList = _onPremiseSubscriptionLogic.GetTeamList(userId);
                    LicenseSessionState.Instance.TeamList = teamList;

                    if (teamList.Count == 0)
                    {
                        ViewBag.IsTeamListPopupVisible = false;
                        return RedirectToAction("Home", "Dashboard");
                    }

                    // if the user belongs to the single team then the user logged in using the  team context to which he belongs.
                    // if the logged in user cout reaches the maximum limit then alert message will be displayed and user will be logged Out
                    else if (teamList.Count == 1)
                    {
                        LicenseSessionState.Instance.SelectedTeam = teamList.FirstOrDefault();
                        var userLoginObj = IsConcurrentUserLoggedIn();
                        if (userLoginObj.IsUserLoggedIn)
                        {
                            if (String.IsNullOrEmpty(user.FirstName))
                                return RedirectToAction("Profile", "User");
                            if (LicenseSessionState.Instance.IsGlobalAdmin)
                                return RedirectToAction("Index", "User");
                            return RedirectToAction("Home", "Dashboard");
                        }
                        else
                        {
                            ClearSession();
                            ModelState.AddModelError("", "Maximum user has logged in Please try after some time");
                            return View();
                        }
                    }
                }

                // ViewData["IsTeamListPopupVisible"] is used to display the popup window for TeamList Selection 
                ViewBag.IsTeamListPopupVisible = LicenseSessionState.Instance.TeamList.Count > 0 && LicenseSessionState.Instance.SelectedTeam == null;
                return View();
            }
            //else
            //    ModelState.AddModelError("", "Invalid Credentials");
            return View();
        }

        private void SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            List<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();

            claims.Add(new System.Security.Claims.Claim(ClaimTypes.Name, String.IsNullOrEmpty(user.Name) ? user.UserName : user.Name)); //user.Name from my database
            claims.Add(new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.UserId)); //user.Id from my database
            claims.Add(new System.Security.Claims.Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "MyApplication"));
            if (!String.IsNullOrEmpty(user.FirstName))
                claims.Add(new System.Security.Claims.Claim("FirstName", user.FirstName)); //user.FirstName from my database


            foreach (var role in user.Roles)
                claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, role));

            System.Security.Claims.ClaimsIdentity identity = new System.Security.Claims.ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "";
            return View();
        }

        /// <summary>
        /// Action for the Forgot Password submission.  Intialy the ONPremise service will called to generate the Token for rthe forgot password  if the value is returned null then the 
        /// Centralized ser vice will be called for the token generation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var response = _authentication.ForgotPassword<ForgotPassword>(model);
                var tokenModel = response.ResponseData;
                //// Invoking the On Premise Service for token generation
                //var tokenModel = _accountLogic.GetForgotPasswordToken(model, ServiceType.OnPremiseWebApi);
                //// If the return value is Null then centralized service called
                //if (tokenModel == null)
                //    tokenModel = _accountLogic.GetForgotPasswordToken(model, ServiceType.CentralizeWebApi);
                if (tokenModel != null)
                {
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = tokenModel.UserId, code = tokenModel.Token }, protocol: Request.Url.Scheme);
                    EmailService service = new EmailService();
                    service.SendEmail(model.Email, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                    ViewBag.Message = "Mail has been sent to the specified email address to reset the password.  !!!!!";
                }
                else
                    ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            }
            return View();
        }

        public ActionResult ResetPassword(string userId, string code)
        {
            License.Models.ResetPassword model = new ResetPassword()
            {
                Token = code,
                UserId = userId
            };
            return View(model);
        }

        /// <summary>
        /// Action for Reset password form submission. Here too the first call will be made
        /// to the On Premise  if it fails or if the ServeruserId is not empty then the Centralized service call will be made
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model, string userId, string code)
        {
            string email = string.Empty;
            if (ModelState.IsValid)
            {
                model.Token = code;
                model.UserId = userId;
                var response = _authentication.ResetPasswordAction(model);
                //var user = _accountLogic.ResetUserPassword(model, ServiceType.OnPremiseWebApi);
                //if (user == null)
                //    user = _accountLogic.ResetUserPassword(model, ServiceType.CentralizeWebApi);
                //else if (!String.IsNullOrEmpty(user.ServerUserId))
                //{
                //    model.UserId = user.ServerUserId;
                //    model.Token = String.Empty;
                //    user = _accountLogic.ResetUserPassword(model, ServiceType.CentralizeWebApi);
                //    ErrorMessage = "Centralized Server : " + _accountLogic.ErrorMessage;
                //}

                if (!response.Status)
                {
                    ModelState.AddModelError("", response.Error.Message);
                    return View(model);
                }
                ViewBag.Display = "inline";
                ViewBag.ResetMessage = "Success";
            }
            return View(model);
        }

        /// <summary>
        /// Get Action to clear the session  on Log out
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            LicenseSessionState.Instance.User.IsActive = false;
            WebAPIRequest<User> request = new WebAPIRequest<User>()
            {
                ModelObject = LicenseSessionState.Instance.User,
                InvokeMethod = Method.PUT,
                ServiceModule = Modules.User,
                ServiceType = ServiceType.All,
                Functionality = Functionality.UpdateLogoutStatus
            };
            _authentication.LogoutUser(LicenseSessionState.Instance.User);
            ClearSession();
            return RedirectToAction("LogIn");
        }

        // Used to sign out from AUthentication manager
        public void ClearSession()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            System.Web.HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// Get Action for updating the User Invitation
        /// </summary>
        /// <param name="invite"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult Confirm(string invite, string status)
        {
            string passPhrase = System.Configuration.ConfigurationManager.AppSettings.Get("passPhrase");
            string data = EncryptDecrypt.DecryptString(invite, passPhrase);
            var details = data.Split(new char[] { ',' });
            ViewBag.ErrorMessage = string.Empty;
            ViewBag.Message = string.Empty;

            string teamId = details[0];
            string inviteId = details[1];

            WebAPIRequest<TeamMember> _apiRequest = new WebAPIRequest<TeamMember>();
            TeamMember mem = new TeamMember();
            mem.Id = Convert.ToInt32(inviteId);
            mem.TeamId = Convert.ToInt32(teamId);
            mem.InviteeStatus = status;
            _apiRequest.ModelObject = mem;
            _apiRequest.InvokeMethod = Method.PUT;
            _apiRequest.ServiceModule = Modules.TeamMember;
            _apiRequest.ServiceType = ServiceType.OnPremiseWebApi;
            _apiRequest.Functionality = Functionality.UpdateInvite;
            var response = _invoke.InvokeService<TeamMember, TeamMember>(_apiRequest);
            if (response.Status)
            {
                var stat = (InviteStatus)Enum.Parse(typeof(InviteStatus), status);
                if (stat == InviteStatus.Accepted)
                    ViewBag.Message = "You have accepted the invitation. Click below to Login with credentials which was shared through Mail";
            }
            else
                ViewBag.ErrorMessage = response.Error.error + " " + response.Error.Message;

            return View();
        }

        /// <summary>
        /// Get ACtioin passes the List of team to the View.
        /// </summary>
        /// <returns></returns>
        public ActionResult TeamList()
        {
            return View(LicenseSessionState.Instance.TeamList);
        }

        /// <summary>
        /// POST action to validate the user for Login based on the
        /// concurrent user Count , If its valid then the team License will be assigned based on the team Context selected
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TeamList(int teamId)
        {
            TempData["ConcurrentUserLoggedMessage"] = "";
            LicenseSessionState.Instance.SelectedTeam = LicenseSessionState.Instance.TeamList.Where(t => t.Id == teamId).FirstOrDefault();
            var userLoginObj = IsConcurrentUserLoggedIn();
            if (userLoginObj.IsUserLoggedIn)
            {
                var url = string.Empty;
                if (String.IsNullOrEmpty(LicenseSessionState.Instance.User.FirstName))
                    url = Url.Action("Profile", "User");
                if (LicenseSessionState.Instance.IsGlobalAdmin)
                    url = Url.Action("Index", "User");
                url = Url.Action("Home", "Dashboard");
                return Json(new { success = true, message = "", url = url });
            }
            else
            {
                ClearSession();
                return Json(new { success = false, message = "Maximum users as already logged in" });
            }
        }

        //  Service call to Verify is Authenticated User can loggin in the Selected Team Context for the concurrent user Aauthentcation
        public ConcurrentUserLogin IsConcurrentUserLoggedIn()
        {
            LicenseSessionState.Instance.AppTeamContext = new Team()
            {
                Id = LicenseSessionState.Instance.SelectedTeam.Id,
                AdminId = LicenseSessionState.Instance.SelectedTeam.AdminId,
                Name = LicenseSessionState.Instance.SelectedTeam.Name
            };
            ConcurrentUserLogin userLogin = new ConcurrentUserLogin()
            {
                TeamId = LicenseSessionState.Instance.SelectedTeam.Id,
                UserId = LicenseSessionState.Instance.User.UserId
            };
            WebAPIRequest<ConcurrentUserLogin> _request = new WebAPIRequest<ConcurrentUserLogin>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.UpdateConcurentUser,
                InvokeMethod = Method.POST,
                ModelObject = userLogin,
                ServiceModule = Modules.User,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            var response = _invoke.InvokeService<ConcurrentUserLogin, ConcurrentUserLogin>(_request);
            if (response.Error != null)
                return null;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.PostAsJsonAsync("api/User/IsConcurrentUserLoggedIn", userLogin).Result;
            //var jsonData = response.Content.ReadAsStringAsync().Result;
            //var userLoginObj = JsonConvert.DeserializeObject<ConcurrentUserLogin>(jsonData);
            LicenseSessionState.Instance.UserSubscribedProducts = response.ResponseData.Products;
            return response.ResponseData;

        }
    }
}
