using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using License.MetCalWeb.Common;
using Newtonsoft.Json;
using License.Models;
using License.ServiceInvoke;

namespace License.MetCalWeb.Controllers
{
    public class RolesController : BaseController
    {

        private APIInvoke _invoke = null;
        public RolesController()
        {
            _invoke = new APIInvoke();
        }

        // GET: Roles
        public ActionResult Index()
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
            //var response = client.GetAsync("api/Role/all").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsondata = response.Content.ReadAsStringAsync().Result;
            //    roles = JsonConvert.DeserializeObject<List<Role>>(jsondata);
            //}
            //client.Dispose();
            return View(roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(Role role)
        {
            var errorMessage = ""; bool status = false;
            if (ModelState.IsValid)
            {
                role.IsDefault = false;
                WebAPIRequest<Role> request = new WebAPIRequest<Role>()
                {
                    AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                    Functionality = Functionality.Create,
                    InvokeMethod = Method.POST,
                    ServiceModule = Modules.Role,
                    ServiceType = ServiceType.OnPremiseWebApi,
                    ModelObject = role
                };
                var response = _invoke.InvokeService<Role, Role>(request);
                if (response.Status)
                    status = true;
                else
                    errorMessage = response.Error.error + " " + response.Error.Message;
                //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                //var response = client.PostAsJsonAsync("api/role/create", role).Result;
                //if (response.IsSuccessStatusCode)
                //    status = true;
                //else
                //{
                //    status = false;
                //    var jsonData = response.Content.ReadAsStringAsync().Result;
                //    var failure = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                //    errorMessage = failure.Message;
                //}
                //client.Dispose();
            }
            else
            {
                errorMessage = String.Join(Environment.NewLine, ModelState.Values.SelectMany(r => r.Errors).SelectMany(r => r.ErrorMessage));
            }
            return Json(new { success = status, message = errorMessage });
        }

        public ActionResult Update(string id)
        {
            Role role = new Role();
            WebAPIRequest<Role> request = new WebAPIRequest<Role>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.GetById,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Role,
                ServiceType = ServiceType.OnPremiseWebApi,
                Id = id.ToString()
            };
            var response = _invoke.InvokeService<Role, Role>(request);
            if (response.Status)
                role = response.ResponseData;
            else
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.GetAsync("api/role/GetById/" + id).Result;
            //var jsonData = response.Content.ReadAsStringAsync().Result;
            //if (response.IsSuccessStatusCode)
            //    role = JsonConvert.DeserializeObject<Role>(jsonData);
            //else
            //{
            //    var failure = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ModelState.AddModelError("", failure.Message);
            //}
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(string id, Role role)
        {
            var status = false; var errorMessage = "";
            WebAPIRequest<Role> request = new WebAPIRequest<Role>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.Update,
                InvokeMethod = Method.PUT,
                ServiceModule = Modules.Role,
                ServiceType = ServiceType.OnPremiseWebApi,
                Id = id.ToString(),
                ModelObject = role
            };
            var response = _invoke.InvokeService<Role,Role>(request);
            if (response.Status)
                status = true;
            else
                errorMessage = response.Error.error + " " + response.Error.Message;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.PutAsJsonAsync("api/role/update/" + id, role).Result;
            //if (response.IsSuccessStatusCode)
            //    status = true;
            //else
            //{
            //    status = false;
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var failure = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    errorMessage = failure.Message;
            //}
            //client.Dispose();
            return Json(new { success = status, message = errorMessage });
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            var status = false; var errorMessage = "";
            WebAPIRequest<Role> request = new WebAPIRequest<Role>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.Delete,
                InvokeMethod = Method.DELETE,
                ServiceModule = Modules.Role,
                ServiceType = ServiceType.OnPremiseWebApi,
                Id = id.ToString()
            };
            var response = _invoke.InvokeService<Role,Role>(request);
            if (response.Status)
                status = true;
            else
                errorMessage = response.Error.error + " " + response.Error.Message;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.DeleteAsync("api/role/Delete/" + id).Result;
            //if (response.IsSuccessStatusCode)
            //    status = true;
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var failure = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    errorMessage = failure.Message;
            //}
            //client.Dispose();
            return Json(new { success = status, message = errorMessage });
        }
    }
}