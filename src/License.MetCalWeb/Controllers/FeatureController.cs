using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using License.MetCalWeb.Common;
using Newtonsoft.Json;
using License.MetCalWeb.Models;

namespace License.MetCalWeb.Controllers
{
    [Authorize(Roles ="BackendAdmin")]
    [SessionExpire]
    public class FeatureController : BaseController
    {
        // GET: Feature
        public ActionResult Index()
        {
            List<Feature> data = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var respons = client.GetAsync("api/Feature/All").Result;
            if (respons.IsSuccessStatusCode)
            {
                var jsonData = respons.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<List<Feature>>(jsonData);
            }
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feature f)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
                var response = client.PostAsJsonAsync("api/Feature/Create", f).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { message = "success", success = true });
                }
                else
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var responseData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    ModelState.AddModelError("", responseData.Message);
                }

            }
            var _message = string.Join(Environment.NewLine, ModelState.Values
                                     .SelectMany(x => x.Errors)
                                     .Select(x => x.ErrorMessage));
            return Json(new { message = _message, success = false });

        }

        public ActionResult Edit(int Id)
        {
            Feature obj = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/feature/GetbyId/" + Id).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                obj = JsonConvert.DeserializeObject<Feature>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", responseData.Message);
            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(int id, Feature f)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
                var response = client.PutAsJsonAsync("api/feature/Update/" + id, f).Result;
                if (response.IsSuccessStatusCode)
                    return Json(new { message = "success", success = true });
                else
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var errorResult = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    ModelState.AddModelError("", errorResult.Message);
                }
            }
            string _message = String.Join(Environment.NewLine, ModelState.Values.SelectMany(s => s.Errors).Select(e => e.ErrorMessage));
            return Json(new { message = _message, success = false });
        }

        public ActionResult Delete(int Id)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.DeleteAsync("api/Feature/Delete/" + Id).Result;
            if (response.IsSuccessStatusCode)
                return Json(new { message = "success", success = true });
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var errorData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                return Json(new { message = errorData.Message, success = false });
            }
        }
    }
}