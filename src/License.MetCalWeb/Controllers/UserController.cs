using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb.Models;
using System.Threading.Tasks;
using System.Net.Http;
using License.MetCalWeb.Common;
using Newtonsoft.Json;

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
        public UserController()
        {

        }

        /// <summary>
        /// GET Action to list all the User. Returns the View by passing te User List 
        /// </summary>
        /// <returns></returns>
        // GET: User
        public async Task<ActionResult> Index()
        {
            List<User> users = new List<User>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = await client.GetAsync("api/user/All");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<User>>(jsonData);
                var obj = users.FirstOrDefault(u => u.Email == LicenseSessionState.Instance.User.Email);
                users.Remove(obj);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View(users);
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
                if (LicenseSessionState.Instance.IsGlobalAdmin)
                    status = await UpdateProfile(usermodel, ServiceType.CentralizeWebApi, LicenseSessionState.Instance.User.UserId);
                else if (LicenseSessionState.Instance.IsSuperAdmin)
                {
                    status = await UpdateProfile(usermodel, ServiceType.OnPremiseWebApi, LicenseSessionState.Instance.User.UserId);
                    if (status)
                        status = await UpdateProfile(usermodel, ServiceType.CentralizeWebApi, LicenseSessionState.Instance.User.ServerUserId);
                }
                else
                    status = await UpdateProfile(usermodel, ServiceType.OnPremiseWebApi, LicenseSessionState.Instance.User.UserId);

                if (status)
                {
                    LicenseSessionState.Instance.User.FirstName = usermodel.FirstName;
                    LicenseSessionState.Instance.User.LastName = usermodel.LastName;
                    LicenseSessionState.Instance.User.PhoneNumber = usermodel.PhoneNumber;
                    if (LicenseSessionState.Instance.IsGlobalAdmin)
                        return RedirectToAction("Index", "User");
                    else
                        return RedirectToAction("Home", "Dashboard");
                }
                ModelState.AddModelError("", ErrorMessage);
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
            var changePwdModel = new Models.ChangePassword();
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
        public async Task<ActionResult> ChangePassword(ChangePassword model)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                if (LicenseSessionState.Instance.IsGlobalAdmin)
                    status = await UpdatePassword(model, ServiceType.CentralizeWebApi, LicenseSessionState.Instance.User.UserId);
                else if (LicenseSessionState.Instance.IsSuperAdmin)
                {
                    status = await UpdatePassword(model, ServiceType.OnPremiseWebApi, LicenseSessionState.Instance.User.UserId);
                    if (status)
                        status = await UpdatePassword(model, ServiceType.CentralizeWebApi, LicenseSessionState.Instance.User.ServerUserId);
                }
                else
                    status = await UpdatePassword(model, ServiceType.OnPremiseWebApi, LicenseSessionState.Instance.User.UserId);
                if (status)
                {
                    if (LicenseSessionState.Instance.IsGlobalAdmin)
                        return RedirectToAction("Index", "User");
                    else
                        return RedirectToAction("Home", "Dashboard");
                }
                else
                    ModelState.AddModelError("", ErrorMessage);

            }
            return View(model);
        }

        /// <summary>
        /// common function to invoke bothe On Premise and Centralized Service based on the type parameter
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateProfile(User model, ServiceType type, string userId)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(type);
            var response = await client.PutAsJsonAsync("/api/user/update/" + userId, model);
            if (response.IsSuccessStatusCode)
                return true;
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
            }
            return false;
        }

        /// <summary>
        ///  common function to invoke bothe On Premise and Centralized Service based on the type parameter
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePassword(ChangePassword model, ServiceType type, string userId)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(type);
           
            var response = await client.PutAsJsonAsync("api/user/ChangePassword/" + userId, model);
            if (response.IsSuccessStatusCode)
                return true;
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            ErrorMessage = response.ReasonPhrase + " - " + response.Content.ReadAsStringAsync().Result;
            return false;
        }
    }
}