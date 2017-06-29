using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Models;
using System.Threading.Tasks;
using System.Net.Http;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using Newtonsoft.Json;
using License.ServiceInvoke;

namespace License.MetCalWeb.Controllers
{
    /// <summary>
    /// User Management functionalities are performed in this controller
    /// </summary>
    [Authorize]
    [SessionExpire]
    public class UserController : BaseController
    {
        string ErrorMessage;
        private APIInvoke _invoke = null;
        private Authentication _authentication = null;

        public UserController()
        {
            _invoke = new APIInvoke();
            _authentication = new Authentication();
            _authentication.OnpremiseToken = LicenseSessionState.Instance.OnPremiseToken != null ? LicenseSessionState.Instance.OnPremiseToken.access_token : "";
            _authentication.CentralizedToken = LicenseSessionState.Instance.CentralizedToken != null ? LicenseSessionState.Instance.CentralizedToken.access_token : "";
        }

        /// <summary>
        /// GET Action to list all the User. Returns the View by passing te User List 
        /// </summary>
        /// <returns></returns>
        // GET: User
        public async Task<ActionResult> Index()
        {
            List<User> users = new List<User>();
            WebAPIRequest<List<User>> request = new WebAPIRequest<List<License.Models.User>>()
            {
                AccessToken = LicenseSessionState.Instance.IsGlobalAdmin ? LicenseSessionState.Instance.CentralizedToken.access_token : LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.All,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.User,
                ServiceType = LicenseSessionState.Instance.IsGlobalAdmin ? ServiceType.CentralizeWebApi : ServiceType.OnPremiseWebApi
            };
            var response = _invoke.InvokeService<List<User>, List<User>>(request);
            if (response.Status)
                users = response.ResponseData;
            else
                ModelState.AddModelError("", response.Error + " - " + response.Error.Message);

            //ServiceType typeService = ServiceType.OnPremiseWebApi;
            //if (LicenseSessionState.Instance.IsGlobalAdmin)
            //    typeService = ServiceType.CentralizeWebApi;
            //HttpClient client = WebApiServiceLogic.CreateClient(typeService);
            //var response = await client.GetAsync("api/user/All");
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    users = JsonConvert.DeserializeObject<List<User>>(jsonData);
            //    var obj = users.FirstOrDefault(u => u.Email == LicenseSessionState.Instance.User.Email);
            //    users.Remove(obj);
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            //}
            string viewName = "index";
            if (LicenseSessionState.Instance.IsAdmin || LicenseSessionState.Instance.IsSuperAdmin)
                viewName = "Users";
            return View(viewName, users);
        }

        /// <summary>
        /// Get Action, display the profile screen to update the User Details, returns view with existing user data.
        /// </summary>
        /// <returns></returns>
        public ActionResult Profile()
        {
            var user = LicenseSessionState.Instance.User;
            return View(user);
        }

        /// <summary>
        /// POST Action to update the profile dat to db by making call using serveice  based on the Role the respective Service will be called to update the data.
        /// 1. Global Admin : Centralized Service will be called.
        /// 2. Super Admin : Centralized and On Premisse service both will be called 
        /// 3. Team Member : only On Premise service will be called.
        /// </summary>
        /// <param name="usermodel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(User usermodel, string userId)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                //if (LicenseSessionState.Instance.IsGlobalAdmin)
                //    status = await UpdateProfile(usermodel, ServiceType.CentralizeWebApi, LicenseSessionState.Instance.User.UserId);
                //else if (LicenseSessionState.Instance.IsSuperAdmin)
                //{
                //    status = await UpdateProfile(usermodel, ServiceType.OnPremiseWebApi, LicenseSessionState.Instance.User.UserId);
                //    if (status)
                //        status = await UpdateProfile(usermodel, ServiceType.CentralizeWebApi, LicenseSessionState.Instance.User.ServerUserId);
                //}
                //else
                //    status = await UpdateProfile(usermodel, ServiceType.OnPremiseWebApi, LicenseSessionState.Instance.User.UserId);
                usermodel.Roles = LicenseSessionState.Instance.User.Roles;
                var response = _authentication.UpdateProfile(usermodel);

                if (response.Status)
                {
                    LicenseSessionState.Instance.User.FirstName = usermodel.FirstName;
                    LicenseSessionState.Instance.User.LastName = usermodel.LastName;
                    LicenseSessionState.Instance.User.PhoneNumber = usermodel.PhoneNumber;
                    if (LicenseSessionState.Instance.IsGlobalAdmin)
                        return RedirectToAction("Index", "User");
                    else
                        return RedirectToAction("Home", "Dashboard");
                }
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            }
            return View(usermodel);
        }


        /// <summary>
        /// Get ACtion to display the change Password View
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePassword()
        {
            var changePwdModel = new ChangePasswordExtended();
            return View(changePwdModel);
        }

        /// <summary>
        /// POST Action to update the password to db based on the Role the respective Service will be called to update the data.
        /// 1. Global Admin : Centralized Service will be called.
        /// 2. Super Admin : Centralized and On Premisse service both will be called 
        /// 3. Team Member : only On Premise service will be called. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordExtended model)
        {
            if (ModelState.IsValid)
            {
                var response = _authentication.ChangePassword<ChangePasswordExtended>(model, LicenseSessionState.Instance.User);
                if (response.Status)
                {
                    if (LicenseSessionState.Instance.IsGlobalAdmin)
                        return RedirectToAction("Index", "User");
                    else
                        return RedirectToAction("Home", "Dashboard");
                }
                else
                    ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            }
            return View(model);
        }

        ///// <summary>
        ///// common function to invoke bothe On Premise and Centralized Service based on the type parameter
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="type"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public async Task<bool> UpdateProfile(User model, ServiceType type, string userId)
        //{
        //    HttpClient client = WebApiServiceLogic.CreateClient(type);
        //    var response = await client.PutAsJsonAsync("/api/user/update/" + userId, model);
        //    if (response.IsSuccessStatusCode)
        //        return true;
        //    else
        //    {
        //        var jsonData = response.Content.ReadAsStringAsync().Result;
        //        var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
        //        ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
        //    }
        //    return false;
        //}

        ///// <summary>
        /////  common function to invoke bothe On Premise and Centralized Service based on the type parameter
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="type"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public async Task<bool> UpdatePassword(ChangePassword model, ServiceType type, string userId)
        //{
        //    HttpClient client = WebApiServiceLogic.CreateClient(type);

        //    var response = await client.PutAsJsonAsync("api/user/ChangePassword/" + userId, model);
        //    if (response.IsSuccessStatusCode)
        //        return true;
        //    else
        //    {
        //        var jsonData = response.Content.ReadAsStringAsync().Result;
        //        var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
        //        ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
        //    }
        //    ErrorMessage = response.ReasonPhrase + " - " + response.Content.ReadAsStringAsync().Result;
        //    return false;
        //}


        public ActionResult EditUser(string id)
        {
            UserExtended user = new UserExtended();
            WebAPIRequest<UserExtended> request = new WebAPIRequest<UserExtended>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.GetById,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.User,
                ServiceType = ServiceType.OnPremiseWebApi,
                Id = id
            };
            var response = _invoke.InvokeService<UserExtended, UserExtended>(request);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.GetAsync("api/user/UserById/" + id).Result;
            if (response.Status)
                user = response.ResponseData;
            var roles = GetRoles();
            roles.ForEach(r => { if (user.Roles.Contains(r.Name)) r.IsSelected = true; });
            user.RolesList = roles;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(string id, UserExtended user, string[] selectedRole)
        {

            if (ModelState.IsValid)
            {
                if (selectedRole != null)
                    user.Roles = selectedRole.ToList();
                var response = _authentication.UpdateProfile(user);
                if (response.Status)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
                    var roles = GetRoles();
                    roles.ForEach(r => { if (user.Roles.Contains(r.Name)) r.IsSelected = true; });
                    user.RolesList = roles;
                }
                //client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                //response = client.PutAsJsonAsync("api/user/Update/" + id, user).Result;
                //var jsonData = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //    return RedirectToAction("Index");
                //else
                //{
                //    var failure = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                //    ModelState.AddModelError("", failure.Message);
                //    var roles = GetRoles();
                //    roles.ForEach(r => { if (user.Roles.Contains(r.Name)) r.IsSelected = true; });
                //    user.RolesList = roles;
                //}
            }
            return View(user);
        }

        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();
            WebAPIRequest<List<Role>> request = new WebAPIRequest<List<Role>>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.All,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Role,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            var response = _invoke.InvokeService<List<Role>, List<Role>>(request);
            if (response.Status)
                roles = response.ResponseData;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.GetAsync("api/Role/All").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsondata = response.Content.ReadAsStringAsync().Result;
            //    roles = JsonConvert.DeserializeObject<List<Role>>(jsondata);

            //}
            //client.Dispose();
            return roles;
        }
    }
}