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
        ServiceType webApiType;
        public ProductController()
        {
            var typeObj = System.Configuration.ConfigurationManager.AppSettings.Get("ServiceType");
            webApiType = (ServiceType)Enum.Parse(typeof(ServiceType), typeObj);
        }

        public async Task<ActionResult> ProductCatalog()
        {
            List<SubscriptionType> typeList = new List<SubscriptionType>();
            HttpClient client = WebApiServiceLogic.CreateClient(webApiType.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.GetAsync("api/subscriptiontype/All");
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                typeList = JsonConvert.DeserializeObject<List<SubscriptionType>>(data);
            }
            return View(typeList);
        }

        public async Task<ActionResult> AddProductToCart(int? Id)
        {
            CartItem item = new CartItem();
            item.SubscriptionTypeId = Convert.ToInt32(Id);
            item.Quantity = 2;
            item.DateCreated = DateTime.Now;
            item.UserId = LicenseSessionState.Instance.User.ServerUserId;
            HttpClient client = WebApiServiceLogic.CreateClient(webApiType.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.PostAsJsonAsync("api/Cart/Create",item);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("ProductCatalog", "Product");
            return null;
        }
    }
}