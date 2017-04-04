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
            HttpClient client = WebApiServiceLogic.CreateClient(webApiType.ToString());
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
        public ActionResult DoPayment()
        {
            Purchase();
            return View();
        }

        public void Purchase()
        {
            Common.CentralizedSubscriptionLogic.UpdateUserSubscription();
            //var obj = logic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId);

            //List<UserSubscription> subsList = new List<UserSubscription>();
            //foreach (var item in obj)
            //{

            //    UserSubscription usersubs = new UserSubscription();
            //    usersubs.UserId = LicenseSessionState.Instance.User.ServerUserId;
            //    usersubs.SubscriptionTypeId = item.SubscriptionTypeId;
            //    usersubs.SubscriptionDate = DateTime.Now.Date;
            //    usersubs.Quantity = item.Quantity;
            //    subsList.Add(usersubs);
            //}
            //Common.CentralizedSubscriptionLogic.UpdateUserSubscription(subsList);
            //foreach (var item in obj)
            //{
            //    item.IsPurchased = true;
            //    logic.UpdateCartItem(item);
            //}
        }

        public async Task<ActionResult> OfflinePayment()
        {
            PurchaseOrder poOrder = new PurchaseOrder();
            HttpClient client = WebApiServiceLogic.CreateClient(webApiType.ToString());
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
            HttpClient client = WebApiServiceLogic.CreateClient(webApiType.ToString());
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