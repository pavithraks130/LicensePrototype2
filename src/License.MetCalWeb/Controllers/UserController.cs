using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb.Models;
using System.Threading.Tasks;
using System.Net.Http;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using Newtonsoft.Json;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        ServiceType webAPiType;
        public UserController()
        {
            var serviceType = System.Configuration.ConfigurationManager.AppSettings.Get("ServiceType");
            webAPiType = (ServiceType)Enum.Parse(typeof(ServiceType), serviceType);
        }

        // GET: User
        public async Task<ActionResult> Index()
        {
            List<User> users = new List<User>();
            HttpClient client = WebApiServiceLogic.CreateClient(webAPiType.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<User>>(jsonData);
                var obj = users.FirstOrDefault(u => u.Email == LicenseSessionState.Instance.User.Email);
                users.Remove(obj);
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
            if (ModelState.IsValid)
            {

                HttpClient client = WebApiServiceLogic.CreateClient(webAPiType.ToString());
                switch (webAPiType)
                {
                    case ServiceType.CentralizeWebApi: client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token); break;
                    case ServiceType.OnPremiseWebApi: client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token); break;
                }

                var response = await client.PutAsJsonAsync("/api/user/update/" + LicenseSessionState.Instance.User.UserId, usermodel);

                if (response.IsSuccessStatusCode)
                {
                    client.Dispose();
                    if (LicenseSessionState.Instance.IsSuperAdmin)
                    {
                        client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
                        response = await client.PutAsJsonAsync("/api/user/update/" + LicenseSessionState.Instance.User.ServerUserId, usermodel);
                    }
                    LicenseSessionState.Instance.User.FirstName = usermodel.FirstName;
                    LicenseSessionState.Instance.User.LastName = usermodel.LastName;
                    LicenseSessionState.Instance.User.PhoneNumber = usermodel.PhoneNumber;
                    if (LicenseSessionState.Instance.IsGlobalAdmin)
                        return RedirectToAction("Index", "User");
                    else
                        return RedirectToAction("Home", "Tab");
                }
                ModelState.AddModelError("", response.ReasonPhrase);
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
            if (ModelState.IsValid)
            {

                HttpClient client = WebApiServiceLogic.CreateClient(webAPiType.ToString());
                switch (webAPiType)
                {
                    case ServiceType.CentralizeWebApi: client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token); break;
                    case ServiceType.OnPremiseWebApi: client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token); break;
                }

                var response = await client.PutAsJsonAsync("api/user/UpdatePassword/" + LicenseSessionState.Instance.User.UserId, model);

                if (response.IsSuccessStatusCode)
                {
                    client.Dispose();
                    if (LicenseSessionState.Instance.IsSuperAdmin)
                    {
                        client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
                        response = await client.PutAsJsonAsync("api/user/UpdatePassword/" + LicenseSessionState.Instance.User.ServerUserId, model);
                    }
                    if (LicenseSessionState.Instance.IsGlobalAdmin)
                        return RedirectToAction("Index", "User");
                    else
                        return RedirectToAction("Home", "Tab");
                }
                ModelState.AddModelError("", response.ReasonPhrase);
            }
            return View(model);
        }
    }
}