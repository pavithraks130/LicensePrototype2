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
    /// <summary>
    /// Controler is used to perform the actions related to the Feature. CRUD operation for the Features
    /// </summary>
    [Authorize(Roles ="BackendAdmin")]
    [SessionExpire]
    public class FeatureController : BaseController
    {
        /// <summary>
        /// Get Action to list all the features
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get Action to display the Create View for the feature
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Post Action to feature for creating the new Record tin db.Service call will be performed to create the feature record in DB
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Action to display the view with the Existing data for the selected Feature.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// POst Action to save the modified data for the selected feature
        /// </summary>
        /// <param name="id"></param>
        /// <param name="f"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete the selected Feature based on the Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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