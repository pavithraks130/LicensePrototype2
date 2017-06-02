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
    [Authorize]
    [SessionExpire]
    public class SubscriptionController : Controller
    {

        public async Task<ActionResult> Index()
        {
            TempData["CartCount"] = "";
            List<SubscriptionType> typeList = new List<SubscriptionType>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            HttpResponseMessage response;
            if (LicenseSessionState.Instance.IsGlobalAdmin)
                response = await client.GetAsync("api/subscription/All");
            else
                response = await client.GetAsync("api/subscription/All/" + LicenseSessionState.Instance.User.ServerUserId);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                typeList = JsonConvert.DeserializeObject<List<SubscriptionType>>(data);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            client.Dispose();
            CartItemCount();
            client.Dispose();
            return View(typeList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SubscriptionType subType = new SubscriptionType();
            List<Product> productList = new List<Product>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/Product/All").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsondata))
                    productList = JsonConvert.DeserializeObject<List<Product>>(jsondata);
                subType.Products = productList;
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            subType.ActivationMonth = 1;
            TempData["ActivationMonth"] = LicenseSessionState.Instance.SubscriptionMonth;
            return View(subType);
        }

        [HttpGet]
        public ActionResult SubscriptionContainer()
        {
            CartItemCount();
            return View();
        }

        private void CartItemCount()
        {
            TempData["CartCount"] = "";
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/Cart/GetCartItemCount/" + LicenseSessionState.Instance.User.ServerUserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var count = JsonConvert.DeserializeObject<string>(data);
                if (!string.IsNullOrEmpty(count))
                {

                    if (Convert.ToInt32(count) > 0)
                        TempData["CartCount"] = "(" + count + ")";
                }
            }
            client.Dispose();
        }

        [HttpGet]
        public ActionResult Categories()
        {
            List<ProductCategory> category = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/productCategory/All").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<List<ProductCategory>>(data);
            }
            else
            {
                category = new List<ProductCategory>();
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Features(int id)
        {
            List<Feature> featureList = new List<Feature>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/feature/GetByCategory/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                featureList = JsonConvert.DeserializeObject<List<Feature>>(jsonData);
            }
            return View(featureList);
        }

        public ActionResult CMMSProducts()
        {
            List<Product> cmmsProducts = new List<Product>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/Product/GetCMMSProducts").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                cmmsProducts = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            }
            return View(cmmsProducts);
        }

        [HttpGet]
        public ActionResult ProductDetails(int id, string categoryName)
        {
            SubscriptionType type = new SubscriptionType();
            type.Category = new ProductCategory();
            type.Category.Id = id;
            type.Category.Name = categoryName;
            DataInitialization();
            CartItemCount();
            return View(type);
        }

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
            ViewData["UserCount"] = listItems;
            ViewData["PeriodeList"] = periodList;
        }

        [HttpPost]
        public async Task<ActionResult> Create(SubscriptionType type, string addToCart, string[] selectedFeatures, int[] cmmsQty, params string[] selectedProducts)
        {
            IList<Product> productCollection = new List<Product>();

            Product pro = new Product()
            {
                AssociatedFeatures = new List<Feature>(),
                Quantity = type.NoOfUsers,
                Categories = new List<ProductCategory>() { type.Category }
            };
            for (int i = 0; i < selectedFeatures.Length; i++)
            {
                Feature f = new Feature()
                {
                    Id = Convert.ToInt32(selectedFeatures[i])
                };
                pro.AssociatedFeatures.Add(f);
            }
            productCollection.Add(pro);

            HttpResponseMessage response = null;
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

            SubscriptionType subscriptionType = new SubscriptionType()
            {
                Name = type.Name,
                Price = type.Price,
                Category = type.Category,
                Products = productCollection.AsEnumerable()
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

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            if (bool.Parse(addToCart))
                response = await client.PostAsJsonAsync("api/cart/CreateSubscriptionAddToCart", subscriptionType);
            else
                response = await client.PostAsJsonAsync("api/subscription/CreateSubscription", subscriptionType);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return null;
        }

        [HttpGet]
        public ActionResult Renew()
        {
            RenewSubscriptionList renewSub = new RenewSubscriptionList();
            renewSub.SubscriptionList = CentralizedSubscriptionLogic.GetExpireSubscription();
            return View(renewSub);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renew(RenewSubscriptionList renewSub, string[] selectedSubscription)
        {
            renewSub.SubscriptionList = new List<SubscriptionType>();
            renewSub.RenewalDate = DateTime.Now.Date;
            if (selectedSubscription.Count() > 0)
            {
                foreach (var subId in selectedSubscription)
                {
                    renewSub.SubscriptionList.Add(new SubscriptionType() { Id = Convert.ToInt32(subId) });
                }
                TempData["RenewSubscription"] = renewSub;
                return RedirectToAction("PaymentGateway", "Cart", new { total = renewSub.Price });
            }
            return View();
        }
    }
}