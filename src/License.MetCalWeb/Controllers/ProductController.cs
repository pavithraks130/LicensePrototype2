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
    /// <summary>
    /// Controller is used to perform the action relataed to the Product
    /// </summary>
    [Authorize(Roles = "BackendAdmin")]
    [SessionExpire]
    public class ProductController : BaseController
    {

        public ProductController()
        {


        }

        /// <summary>
        /// GET ACtion, returns View with list of products as the Model data to display the product List
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// GET Action , return view for the creation of product
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            GetFeatureList();
            return View();
        }

        /// <summary>
        /// POST Action, triggers when the create form is submitted. The product data with category and Feature will be sent to the 
        /// Server for saving the Data using the Create Service call
        /// </summary>
        /// <param name="productDetails"></param>
        /// <param name="rboCategory"></param>
        /// <param name="featuresList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Product productDetails, string[] rboCategory, string[] featuresList)
        {
            if (ModelState.IsValid)
            {
                productDetails.Categories = rboCategory.ToList().Select(c => new SubscriptionCategory() { Id = Convert.ToInt32(c) }).ToList();
                productDetails.AssociatedFeatures = featuresList.ToList().Select(featureId => new Feature() { Id = Convert.ToInt32(featureId) }).ToList();

                //Service call to save the data in Centralized DB
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

        /// <summary>
        /// GET Action, Returns view with the existing data for the selected product Id for Modifying details if needed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                foreach (var feature in pro.AssociatedFeatures)
                    (ViewBag.Features as List<Feature>).Find(p => p.Id == feature.Id).Selected = true;

            var objList = ViewBag.Categories as List<SubscriptionCategory>;
            if (objList != null)
            {
                foreach (var category in pro.Categories)
                    objList.Find(c => c.Id == category.Id).IsSelected = true;
                ViewBag.categories = objList;
            }
            return View(pro);
        }

        /// <summary>
        /// POST action. action triggered when user submits the Product edit screen.
        /// Modified data is updated for the selected product id. using service call
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDetails"></param>
        /// <param name="rboCategory"></param>
        /// <param name="featuresList"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product productDetails, string[] rboCategory, string[] featuresList)
        {
            if (ModelState.IsValid)
            {
                productDetails.Categories = rboCategory.ToList().Select(categoryId => new SubscriptionCategory() { Id = Convert.ToInt32(categoryId) }).ToList();
                productDetails.AssociatedFeatures = featuresList.ToList().Select(featureId => new Feature() { Id = Convert.ToInt32(featureId) }).ToList();
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

        /// <summary>
        /// Get the list of Features and Categories through service call
        /// </summary>
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
                ViewBag.Categories = new List<SubscriptionCategory>();
                ViewBag.Features = new List<Feature>();
            }
        }

        /// <summary>
        /// POST Action to delete the Product based on the Product Id 
        /// and JSON Response is returned  with status and error  if error exist else empty string will be sent
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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