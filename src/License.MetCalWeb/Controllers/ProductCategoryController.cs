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
    [Authorize(Roles = "BackendAdmin")]
    [SessionExpire]
    public class ProductCategoryController : BaseController
    {
        // GET: ProductCategory
        public ActionResult Index()
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/ProductCategory/All").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<ProductCategory>>(jsonData);
            }
            else
            {
                categories = new List<ProductCategory>();
            }
            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
                var response = client.PostAsJsonAsync("api/productcategory/create", category).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { message = "success", success = true });
                }
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

        public ActionResult Edit(int id)
        {
            ProductCategory category = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/productCategory/GetById/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<ProductCategory>(data);
            }
            else
            {
                category = new ProductCategory();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductCategory category)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.PutAsJsonAsync("api/productCategory/update/" + id, category).Result;
            if (!response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var responseFailure = JsonConvert.DeserializeObject<ResponseFailure>(data);
                return Json(new { message = responseFailure.Message, success = false });
            }
            else
                return Json(new { message = "success", success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.DeleteAsync("api/productCategory/Delete/" + id).Result;
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