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
    [Authorize]
    [SessionExpire]
    public class UserController : BaseController
    {
        string ErrorMessage;
        public UserController()
        {

        }

        // GET: User
        public async Task<ActionResult> Index()
        {
            List<User> users = new List<User>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
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


        public ActionResult Profile()
        {
            var user = LicenseSessionState.Instance.User;
            return View(user);
        }

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

        [Authorize]
        public ActionResult ChangePassword()
        {
            var changePwdModel = new Models.ChangePassword();
            return View(changePwdModel);
        }

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

        public async Task<bool> UpdateProfile(User model, ServiceType type, string userId)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(type.ToString());
            switch (type)
            {
                case ServiceType.CentralizeWebApi: client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token); break;
                case ServiceType.OnPremiseWebApi: client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token); break;
            }

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

        public async Task<bool> UpdatePassword(ChangePassword model, ServiceType type, string userId)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(type.ToString());
            switch (type)
            {
                case ServiceType.CentralizeWebApi: client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token); break;
                case ServiceType.OnPremiseWebApi: client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token); break;
            }
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