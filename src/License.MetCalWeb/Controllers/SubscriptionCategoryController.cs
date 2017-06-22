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
    /// Controler consist of the functionality realted to Subscription category
    /// </summary>
    [Authorize(Roles = "BackendAdmin")]
    [SessionExpire]
    public class SubscriptionCategoryController : BaseController
    {
        /// <summary>
        /// GET action which fetches list of all the SubscriptionCategory
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<SubscriptionCategory> categories = new List<SubscriptionCategory>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/SubscriptionCategory/All").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<SubscriptionCategory>>(jsonData);
            }
            return View(categories);
        }

        /// <summary>
        /// Get Action, return the Create View
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST Action, action gets triggered when user submit the create form , Invokes the service for subscription category creation 
        /// and saves data to DB
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubscriptionCategory category)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
                var response = client.PostAsJsonAsync("api/SubscriptionCategory/create", category).Result;
                if (response.IsSuccessStatusCode)
                    return Json(new { message = "success", success = true });
                else
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var responseError = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    return Json(new { message = responseError.Message, success = false });
                }
            }
            var _message = String.Join(Environment.NewLine, ModelState.Values.SelectMany(r => r.Errors).SelectMany(r => r.ErrorMessage));
            return Json(new { message = _message, success = false });
        }

        /// <summary>
        /// GET Action , return Edit view with the existing data for selected Category Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            SubscriptionCategory category = new SubscriptionCategory();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/SubscriptionCategory/GetById/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<SubscriptionCategory>(data);
            }
            return View(category);
        }

        /// <summary>
        /// POSt Action Saves the data changes to DB through the service call  for the selected category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SubscriptionCategory category)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.PutAsJsonAsync("api/SubscriptionCategory/update/" + id, category).Result;
            if (!response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var responseFailure = JsonConvert.DeserializeObject<ResponseFailure>(data);
                return Json(new { message = responseFailure.Message, success = false });
            }
            else
                return Json(new { message = "success", success = true });
        }

        /// <summary>
        /// POST ACtion , to delete the selected category using service call once the response Json Response returned based on the response.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.DeleteAsync("api/SubscriptionCategory/Delete/" + id).Result;
            if (!response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var responseFailure = JsonConvert.DeserializeObject<ResponseFailure>(data);
                return Json(new { message = responseFailure.Message, success = false });
            }
            return Json(new { message = "success", success = true });
        }
    }
}