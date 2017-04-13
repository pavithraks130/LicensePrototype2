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
    public class ProductController : BaseController
    {

        public ProductController()
        {


        }

        public async Task<ActionResult> SubscriptionCatalog()
        {
            TempData["CartCount"] = "";
            List<SubscriptionType> typeList = new List<SubscriptionType>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.GetAsync("api/subscription/All");
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                typeList = JsonConvert.DeserializeObject<List<SubscriptionType>>(data);
            }
            client.Dispose();
            client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            response = await client.GetAsync("api/Cart/GetCartItemCount/" + LicenseSessionState.Instance.User.ServerUserId);
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

            return View(typeList);
        }
        [HttpGet]
        public ActionResult CreateSubscription()
        {
            List<Product> productList = new List<Product>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = client.GetAsync("api/Product/All").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsondata))
                    productList = JsonConvert.DeserializeObject<List<Product>>(jsondata);
                TempData["productList"] = productList;
            }
            return View(productList);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSubscription(string subscriptionName, int[] qty, int activeDays, params string[] selectedIndexAndProductIdList)
        {
            IList<Product> productCollection = new List<Product>();
            double totalPrice = 0;

            for (int index = 0; index < selectedIndexAndProductIdList.Length; index++)
            {

                var splitValue = selectedIndexAndProductIdList[index].Split(new char[] { ' ' });
                int indexValue = int.Parse(splitValue[0].Split(new char[] { ':' })[1]);
                int productId = int.Parse(splitValue[1].Split(new char[] { ':' })[1]);
                Product p = new Product();
                if (TempData["productList"] != null)
                {
                    p = (TempData["productList"] as List<Product>).Where(x => x.Id == productId).FirstOrDefault();
                    totalPrice += p.Price * qty[indexValue];
                    p.Quantity = qty[indexValue];
                }
                productCollection.Add(p);
            }
            SubscriptionType subscriptionType = new SubscriptionType();
            subscriptionType.Name = subscriptionName;
            subscriptionType.Price = totalPrice;
            subscriptionType.Products = productCollection.AsEnumerable();
            subscriptionType.ImagePath = "B5.png";
            switch (activeDays)
            {
                case 0: subscriptionType.ActiveDays = 365; break;
                case 1: subscriptionType.ActiveDays = 365 * 2; break;
                case 2: subscriptionType.ActiveDays = 365 * 3; break;
            }

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.PostAsJsonAsync("api/subscription/CreateSubscription", subscriptionType);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("SubscriptionCatalog", "Product");
            return null;
        }

        [HttpGet]
        public ActionResult CreateAndUpdateProduct()
        {
            List<Product> productList = new List<Product>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = client.GetAsync("api/Product/All").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(jsondata))
                    productList = JsonConvert.DeserializeObject<List<Product>>(jsondata);
            }
            return View(productList);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product productDetails)
        {
            return View();
        }

        public async Task<ActionResult> AddProductToCart(int? Id)
        {
            CartItem item = new CartItem();
            item.SubscriptionTypeId = Convert.ToInt32(Id);
            item.Quantity = 2;
            item.DateCreated = DateTime.Now;
            item.UserId = LicenseSessionState.Instance.User.ServerUserId;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.PostAsJsonAsync("api/Cart/Create", item);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("SubscriptionCatalog", "Product");
            return null;
        }
    }
}