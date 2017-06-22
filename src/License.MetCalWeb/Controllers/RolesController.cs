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
        // GET: Roles
        public ActionResult Index()
        {
            List<Role> roles = new List<Role>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/Role/all").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                roles = JsonConvert.DeserializeObject<List<Role>>(jsondata);
            }
            client.Dispose();
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
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                var response = client.PostAsJsonAsync("api/role/create", role).Result;
                if (response.IsSuccessStatusCode)
                    status = true;
                else
                {
                    status = false;
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var failure = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    errorMessage = failure.Message;
                }
                client.Dispose();
            }
            else
            {
                errorMessage = "Issue with Error Name";
            }
            return Json(new { success = status, message = errorMessage });
        }

        public ActionResult Update(string id)
        {
            Role role = new Role();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/role/GetById/" + id).Result;
            var jsonData = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
                role = JsonConvert.DeserializeObject<Role>(jsonData);
            else
            {
                var failure = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", failure.Message);
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(string id, Role role)
        {
            var status = false; var errorMessage = "";
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PutAsJsonAsync("api/role/update/" + id, role).Result;
            if (response.IsSuccessStatusCode)
                status = true;
            else
            {
                status = false;
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var failure = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                errorMessage = failure.Message;
            }
            client.Dispose();
            return Json(new { success = status, message = errorMessage });
        }

        [HttpGet]
        public JsonResult Delete(string id)
        {
            var status = false; var errorMessage = "";
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.DeleteAsync("api/role/Delete/" + id).Result;
            if (response.IsSuccessStatusCode)
                status = true;
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var failure = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                errorMessage = failure.Message;
            }
            client.Dispose();
            return Json(new { success = status, message = errorMessage });
        }
    }
}