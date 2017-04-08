using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using License.MetCalWeb.Common;
using System.Threading.Tasks;
using Newtonsoft.Json;
using License.MetCalWeb.Models;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {

        ServiceType webApiType;
        public CartController()
        {
            var serviceType = System.Configuration.ConfigurationManager.AppSettings.Get("ServiceType");
            webApiType = (ServiceType)Enum.Parse(typeof(ServiceType), serviceType);
        }

        public async Task<ActionResult> CartItem()
        {
            List<CartItem> itemList = new List<CartItem>();
            if (LicenseSessionState.Instance.User == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            itemList = await GetCartItems();
            return View(itemList);
        }

        public async Task<ActionResult> RemoveItem(int id)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.DeleteAsync("api/cartitem/Delete" + id);
            //if (response.IsSuccessStatusCode)
            return RedirectToAction("CartItem", "Cart");
        }

        public ActionResult PaymentGateway()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DoPayment()
        {
           await  Purchase();
            return View();
        }

        public async Task Purchase()
        {
           await Common.CentralizedSubscriptionLogic.UpdateUserSubscription();          
        }

        public async Task<ActionResult> OfflinePayment()
        {
            PurchaseOrder poOrder = new PurchaseOrder();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            var response = await client.PostAsync("api/cart/offlinepayment/" + LicenseSessionState.Instance.User.ServerUserId, null);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsonData))
                    poOrder = JsonConvert.DeserializeObject<PurchaseOrder>(jsonData);
            }
            return View(poOrder);

        }

        private async Task<List<CartItem>> GetCartItems()
        {
            List<CartItem> itemList = null;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.GetAsync("api/cart/getItems/" + LicenseSessionState.Instance.User.ServerUserId);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                itemList = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
            }
            return itemList;
        }

    }
}