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
    [Authorize(Roles = "BackendAdmin")]
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
            GetFeatureList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product productDetails, string[] rboCategory, string[] featuresList)
        {
            if (ModelState.IsValid)
            {
                if (rboCategory.Count() > 0)
                {
                    productDetails.Categories = new List<ProductCategory>();
                    foreach (var categoryIds in rboCategory)
                    {
                        productDetails.Categories.Add(new ProductCategory() { Id = Convert.ToInt32(categoryIds) });
                    }
                }
                productDetails.AssociatedFeatures = new List<Feature>();

                foreach (var featureId in featuresList)
                {
                    productDetails.AssociatedFeatures.Add(new Feature() { Id = Convert.ToInt32(featureId) });
                }
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
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
            GetFeatureList();
            return View(productDetails);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Product pro = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
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
            GetFeatureList();
            // Updating the data 
            if (pro.AssociatedFeatures.Count > 0)
            {
                foreach (var feature in pro.AssociatedFeatures)
                {
                    (ViewBag.Features as List<Feature>).Find(p => p.Id == feature.Id).Selected = true;
                }
            }

            var objList = ViewBag.Categories as List<ProductCategory>;
            if (objList != null)
            {
                foreach (var category in pro.Categories)
                    objList.Find(c => c.Id == category.Id).IsSelected = true;
                ViewBag.categories = objList;
            }
            return View(pro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product productDetails, string[] rboCategory, string[] featuresList)
        {
            if (ModelState.IsValid)
            {
                if (rboCategory.Count() > 0)
                {
                    productDetails.Categories = new List<ProductCategory>();
                    foreach (var categoryId in rboCategory)
                        productDetails.Categories.Add(new ProductCategory() { Id = Convert.ToInt32(categoryId) });
                }

                if (featuresList.Count() > 0)
                {
                    productDetails.AssociatedFeatures = new List<Feature>();
                    foreach (var featureId in featuresList)
                        productDetails.AssociatedFeatures.Add(new Feature() { Id = Convert.ToInt32(featureId) });
                }
                productDetails.ModifiedDate = DateTime.Now;
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
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
            GetFeatureList();
            return View(productDetails);
        }

        public void GetFeatureList()
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/Product/ProductDependency").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                ProductDependency dependencyObj = JsonConvert.DeserializeObject<ProductDependency>(jsonData);
                ViewBag.Categories = dependencyObj.Categories;
                ViewBag.Features = dependencyObj.Features;
            }
            else
            {
                ViewBag.Categories = new List<ProductCategory>();
                ViewBag.Features = new List<Feature>();
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
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