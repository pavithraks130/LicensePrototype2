using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace License.MetCalWeb.Controllers
{
    [Authorize(Roles = "SuperAdmin,BackendAdmin")]
    [SessionExpire]
    public class ProductController : BaseController
    {

        public ProductController()
        {


        }

        [HttpGet]
        public ActionResult Index()
        {
            List<Product> productList = new List<Product>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = client.GetAsync("api/Product/All").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsondata))
                    productList = JsonConvert.DeserializeObject<List<Product>>(jsondata);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View(productList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product productDetails, params string[] featureIdList)
        {
            if (ModelState.IsValid)
            {
                productDetails.AssociatedFeatures = new List<Feature>();
                foreach (var featureId in featureIdList)
                {
                    productDetails.AssociatedFeatures.Add(new Feature() { Id = Convert.ToInt32(featureId) });
                }
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
                var response = client.PostAsJsonAsync("api/product/create", productDetails).Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                {
                    var jsondata = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
                    ModelState.AddModelError("", errorResponse.Message);
                }
            }
            return View(productDetails);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            Product pro = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = client.GetAsync("api/product/GetById/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                pro = JsonConvert.DeserializeObject<Product>(jsondata);
            }
            else
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var errorRespoonse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
                ModelState.AddModelError("", errorRespoonse.Message);
            }
            return View(pro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, Product productDetails, params string[] featureIdList)
        {
            if (ModelState.IsValid)
            {
                productDetails.AssociatedFeatures = new List<Feature>();
                foreach (var featureId in featureIdList)
                    productDetails.AssociatedFeatures.Add(new Feature() { Id = Convert.ToInt32(featureId) });

                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
                var response = client.PutAsJsonAsync("api/product/update/" + id, productDetails).Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                {
                    var jsondata = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
                    ModelState.AddModelError("", errorResponse.Message);
                }
            }
            return View(productDetails);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = client.DeleteAsync("api/product/delete/" + id).Result;
            if (response.IsSuccessStatusCode)
                return Json(new { message = "success", status = true });
            else
            {
                var errordata = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ResponseFailure>(errordata);
                return Json(new { message = errorResponse.Message, status = false });
            }
        }

    }
}