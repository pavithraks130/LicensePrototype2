﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using License.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using License.ServiceInvoke;

namespace License.MetCalWeb.Controllers
{
    /// <summary>
    /// Conotroller for performing the functionality related to the Subscription.
    /// </summary>
    [Authorize]
    [SessionExpire]
    public class SubscriptionController : Controller
    {

        private APIInvoke _invoke = null;
        public CentralizedSubscriptionLogic _centralizedSubscriptionLogic = null;
        public SubscriptionController()
        {
            _invoke = new APIInvoke();
            _centralizedSubscriptionLogic = new CentralizedSubscriptionLogic();
        }

        /// <summary>
        ///  Get Aaction for listing all the subscription based on the user Id . Returns the view with the list of subscription display
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            TempData["CartCount"] = "";
            List<Subscription> typeList = new List<Subscription>();
            WebAPIRequest<List<Subscription>> request = new WebAPIRequest<List<Subscription>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = LicenseSessionState.Instance.IsGlobalAdmin ? Functionality.All: Functionality.GetByUser,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Subscription,
                ServiceType = ServiceType.CentralizeWebApi,
                Id = LicenseSessionState.Instance.IsGlobalAdmin ? "" : LicenseSessionState.Instance.User.ServerUserId
            };

            var response = _invoke.InvokeService<List<Subscription>, List<Subscription>>(request);
            if (response.Status)
                typeList = response.ResponseData;
            else
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //HttpResponseMessage response;
            //if (LicenseSessionState.Instance.IsGlobalAdmin)
            //    response = await client.GetAsync("api/subscription/All");
            //else
            //    response = await client.GetAsync("api/subscription/All/" + LicenseSessionState.Instance.User.ServerUserId);
            //if (response.IsSuccessStatusCode)
            //{
            //    var data = response.Content.ReadAsStringAsync().Result;
            //    typeList = JsonConvert.DeserializeObject<List<Subscription>>(data);
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            //}
            //client.Dispose();
            CartItemCount();
            return View(typeList);
        }
        /// <summary>
        /// GET action return view for the Subscription creation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            SubscriptionExtended subType = new SubscriptionExtended();
            WebAPIRequest<List<Product>> request = new WebAPIRequest<List<Product>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.All,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Product,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService<List<Product>, List<Product>>(request);
            if (response.Status)
                subType.Products = response.ResponseData;
            else
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            //List<Product> productList = new List<Product>();
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = client.GetAsync("api/Product/All").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsondata = response.Content.ReadAsStringAsync().Result;
            //    if (!String.IsNullOrEmpty(jsondata))
            //        productList = JsonConvert.DeserializeObject<List<Product>>(jsondata);
            //    subType.Products = productList;
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            //}
            subType.ActivationMonth = 1;
            TempData["ActivationMonth"] = LicenseSessionState.Instance.SubscriptionMonth;
            return View(subType);
        }
        /// <summary>
        /// Get Action returns view for the subscription creation. This view is used by the super admin.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SubscriptionContainer()
        {
            CartItemCount();
            return View();
        }

        /// <summary>
        /// Function to get the Cart Items Count 
        /// </summary>
        private void CartItemCount()
        {
            TempData["CartCount"] = "";
            WebAPIRequest<string> request = new WebAPIRequest<string>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.GetCartItemsCount,
                Id = LicenseSessionState.Instance.User.ServerUserId,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Cart,
                ServiceType = ServiceType.CentralizeWebApi
            };

            var response = _invoke.InvokeService<string, string>(request);
            if (response.Status)
                if (!string.IsNullOrEmpty(response.ResponseData) && Convert.ToInt32(response.ResponseData) > 0)
                    TempData["CartCount"] = "(" + response.ResponseData + ")";
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = client.GetAsync("api/Cart/GetCartItemCount/" + LicenseSessionState.Instance.User.ServerUserId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var data = response.Content.ReadAsStringAsync().Result;
            //    var count = JsonConvert.DeserializeObject<string>(data);
            //    if (!string.IsNullOrEmpty(count))
            //    {

            //        if (Convert.ToInt32(count) > 0)
            //            TempData["CartCount"] = "(" + count + ")";
            //    }
            //}
            //client.Dispose();
        }

        /// <summary>
        /// GET action Return View for listing categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Categories()
        {
            List<SubscriptionCategory> category = null;
            WebAPIRequest<List<SubscriptionCategory>> request = new WebAPIRequest<List<SubscriptionCategory>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.All,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.SubscriptionCategory,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService<List<SubscriptionCategory>, List<SubscriptionCategory>>(request);
            if (response.Status)
                category = response.ResponseData;
            else
                category = new List<SubscriptionCategory>();
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = client.GetAsync("api/SubscriptionCategory/All").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var data = response.Content.ReadAsStringAsync().Result;
            //    category = JsonConvert.DeserializeObject<List<SubscriptionCategory>>(data);
            //}
            //else
            //{
            //    category = new List<SubscriptionCategory>();
            //}
            return View(category);
        }

        /// <summary>
        /// Get Action. List Features based on the category Id  Selection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Features(int id)
        {
            List<Feature> featureList = new List<Feature>();
            WebAPIRequest<List<Feature>> request = new WebAPIRequest<List<Feature>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.GetByCategory,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Feature,
                ServiceType = ServiceType.CentralizeWebApi,
                Id = id.ToString()
            };
            var response = _invoke.InvokeService<List<Feature>, List<Feature>>(request);
            if (response.Status)
                featureList = response.ResponseData;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = client.GetAsync("api/feature/GetByCategory/" + id).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    featureList = JsonConvert.DeserializeObject<List<Feature>>(jsonData);
            //}
            return View(featureList);
        }

        /// <summary>
        /// GET action return the view with CMMS Product listed
        /// </summary>
        /// <returns></returns>
        public ActionResult CMMSProducts()
        {
            List<Product> cmmsProducts = new List<Product>();
            WebAPIRequest<List<Product>> request = new WebAPIRequest<List<Product>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.GetCMMSProducts,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Product,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService< List<Product>, List< Product > >(request);
            if (response.Status)
                cmmsProducts = response.ResponseData;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = client.GetAsync("api/Product/GetCMMSProducts").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    cmmsProducts = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            //}
            return View(cmmsProducts);
        }

        /// <summary>
        ///  Get Action. Returns the view for the feature selection and cmms product selectio.
        ///  This screen is the master view Teamplate for listing Feature and CMMS Product views are the child view 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProductDetails(int id, string categoryName)
        {
            SubscriptionExtended type = new SubscriptionExtended()
            {
                Category = new SubscriptionCategory()
                {
                    Id = id,
                    Name = categoryName
                }
            };
            DataInitialization();
            CartItemCount();
            return View(type);
        }

        /// <summary>
        /// Functionality to initialize the basic data
        /// </summary>
        public void DataInitialization()
        {
            List<SelectListItem> listItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "5 User",
                    Value = "5",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "10 User",
                    Value = "10",

                },
                new SelectListItem
                {
                    Text = "15 User",
                    Value = "15"
                }
            };
            List<SelectListItem> periodList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "12 Months",
                    Value = "1",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "24 Months",
                    Value = "2",

                },
                new SelectListItem
                {
                    Text = "36 Months",
                    Value = "3"
                }
            };
            ViewBag.UserCount = listItems;
            ViewBag.PeriodeList = periodList;
        }

        /// <summary>
        /// POST Action which is used bto create mthe Subscription by making the service call once succedded then redirected to the Index screen
        /// </summary>
        /// <param name="type"></param>
        /// <param name="addToCart"></param>
        /// <param name="selectedFeatures"></param>
        /// <param name="cmmsQty"></param>
        /// <param name="selectedProducts"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(SubscriptionExtended type, string addToCart, string[] selectedFeatures, int[] cmmsQty, params string[] selectedProducts)
        {
            IList<Product> productCollection = new List<Product>();
            // Product creation based on the feature selection
            Product pro = new Product()
            {
                Features = new List<Feature>(),
                Quantity = type.NoOfUsers,
                Categories = new List<SubscriptionCategory>() { type.Category }
            };
            pro.Features = selectedFeatures.ToList().Select(featureId => new Feature() { Id = Convert.ToInt32(featureId) }).ToList();
            productCollection.Add(pro);

            // Creation of the Product for each cmms product selection 
            if (selectedProducts != null)
                for (int i = 0; i < selectedProducts.Length; i++)
                {
                    Product p = new Product()
                    {
                        Id = Convert.ToInt32(selectedProducts[i]),
                        Quantity = Convert.ToInt32(cmmsQty[i])
                    };
                    productCollection.Add(p);
                }

            // Creation of the subscription Object
            SubscriptionExtended subscriptionType = new SubscriptionExtended()
            {
                Name = type.Name,
                Price = type.Price,
                Category = type.Category,
                Products = productCollection.ToList()
            };
            if (LicenseSessionState.Instance.IsSuperAdmin)
                subscriptionType.CreatedBy = LicenseSessionState.Instance.User.ServerUserId;

            subscriptionType.ImagePath = "P_1.PNG";
            int monthType = Convert.ToInt32(type.ActivationMonth);
            switch (monthType)
            {
                case 1: subscriptionType.ActiveDays = 365; break;
                case 2: subscriptionType.ActiveDays = 365 * 2; break;
                case 3: subscriptionType.ActiveDays = 365 * 3; break;
            }

            //// Service call to create the subscription  based on the 

            WebAPIRequest<SubscriptionExtended> request = new WebAPIRequest<SubscriptionExtended>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = bool.Parse(addToCart) ? Functionality.CreateSubscriptionAddToCart : Functionality.Create,
                InvokeMethod = Method.POST,
                ModelObject = subscriptionType,
                ServiceModule = bool.Parse(addToCart) ? Modules.Cart : Modules.Subscription,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService< SubscriptionExtended, SubscriptionExtended>(request);
            if (response.Status)
                return RedirectToAction("Index");
            else
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //if (bool.Parse(addToCart))
            //    response = await client.PostAsJsonAsync("api/cart/CreateSubscriptionAddToCart", subscriptionType);
            //else
            //    response = await client.PostAsJsonAsync("api/subscription/CreateSubscription", subscriptionType);

            //if (response.IsSuccessStatusCode)
            //    return RedirectToAction("Index");
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            //}
            return null;
        }

        /// <summary>
        /// Get Action, returns view with subscription list which expires in a month
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Renew()
        {
            RenewSubscriptionList renewSub = new RenewSubscriptionList();
            renewSub.SubscriptionList = _centralizedSubscriptionLogic.GetExpireSubscription();
            return View(renewSub);
        }

        /// <summary>
        /// POST action , triggers when the form subsmitted for the renewal , data is stored temporarly in temp data and
        /// once payment is done the subscription will be updated  with latest expire details 
        /// </summary>
        /// <param name="renewSub"></param>
        /// <param name="selectedSubscription"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renew(RenewSubscriptionList renewSub, string[] selectedSubscription)
        {
            renewSub.SubscriptionList = new List<UserSubscription>();
            renewSub.RenewalDate = DateTime.Now.Date;
            if (selectedSubscription.Count() > 0)
            {
                foreach (var subId in selectedSubscription)
                {
                    renewSub.SubscriptionList.Add(new UserSubscription() { Id = Convert.ToInt32(subId) });
                }
                TempData["RenewSubscription"] = renewSub;
                return RedirectToAction("PaymentGateway", "Cart", new { total = renewSub.Price });
            }
            return View();
        }
    }
}