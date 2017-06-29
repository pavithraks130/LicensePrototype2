using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using License.MetCalWeb.Common;
using System.Threading.Tasks;
using Newtonsoft.Json;
using License.Models;
using License.ServiceInvoke;

namespace License.MetCalWeb.Controllers
{
    /// <summary>
    /// Controller to provide Funuctionality related to the Cart
    /// </summary>
    [Authorize]
    [SessionExpire]
    public class CartController : BaseController
    {
        private APIInvoke _invoke = null;
        private CentralizedSubscriptionLogic _centralizedSubscriptionLogic = null;
        public CartController()
        {
            _invoke = new APIInvoke();
            _centralizedSubscriptionLogic = new CentralizedSubscriptionLogic();
        }

        /// <summary>
        /// Get Action to list the Cart Items based on the user Id who as logged in and display the view by passing the item list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CartItem()
        {
            List<CartItem> itemList = new List<CartItem>();
            if (LicenseSessionState.Instance.User == null)
                return RedirectToAction("LogIn", "Account");
            itemList = await GetCartItems();
            return View(itemList);
        }

        /// <summary>
        /// Action to remove the cart item based on the Cart Item Id  and return to the Cart index
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> RemoveItem(int id)
        {
            WebAPIRequest<CartItem> request = new WebAPIRequest<License.Models.CartItem>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.Delete,
                Id = id.ToString(),
                InvokeMethod = Method.DELETE,
                ServiceModule = Modules.Cart,
                ServiceType = ServiceType.CentralizeWebApi

            };
            var response = _invoke.InvokeService<CartItem, CartItem>(request);
            if (response.Status)
                return RedirectToAction("CartItem", "Cart");
            else
                return Json(new { success = false, message = response.Error.error + " " + response.Error.Message });
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = await client.DeleteAsync("api/cart/Delete/" + id);

        }

        /// <summary>
        /// To Redirect to the Payment process once user checkout 
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        public ActionResult PaymentGateway(string total)
        {
            ViewBag.Total = total;
            return View();
        }

        /// <summary>
        /// Post action to update the payment details in the Centralized Db and sync the Subscriptions purchased from
        /// centralized to On Premise
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DoPayment(string action)
        {
            if (action == string.Empty)
            {
                await Purchase();
                return View();
            }
            return RedirectToAction("CartItem", "Cart");
        }

        // Async call to sync the data  by making a service call
        public async Task Purchase()
        {
            if (TempData["RenewSubscription"] != null)
                await _centralizedSubscriptionLogic.RenewSubscription((RenewSubscriptionList)TempData["RenewSubscription"]);
            else
                await _centralizedSubscriptionLogic.UpdateUserSubscription();
        }

        ///// <summary>
        ///// Async function  call to synch the Renewed subscription
        ///// </summary>
        //public void SyncRenewData()
        //{
        //    HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
        //    var response = client.PostAsJsonAsync("api/UserSubscription/RenewSubscription/" + LicenseSessionState.Instance.User.ServerUserId, TempData["RenewSubscription"]).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var jsonData = response.Content.ReadAsStringAsync().Result;
        //    }
        //}

        /// <summary>
        /// Action to perform the Offline Paymentonce the data submitted to the offline payment the Purchase Order Id is 
        /// returned to track the status
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OfflinePayment()
        {
            PurchaseOrder poOrder = new PurchaseOrder();
            WebAPIRequest<PurchaseOrder> request = new WebAPIRequest<PurchaseOrder>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.OfflinePayment,
                InvokeMethod = Method.POST,
                Id = LicenseSessionState.Instance.User.ServerUserId,
                ServiceModule = Modules.Cart,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService<PurchaseOrder,PurchaseOrder>(request);
            if (response.Status)
                poOrder = response.ResponseData;
            else
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            //
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = await client.PostAsync("api/cart/offlinepayment/" + LicenseSessionState.Instance.User.ServerUserId, null);
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    if (!string.IsNullOrEmpty(jsonData))
            //        poOrder = JsonConvert.DeserializeObject<PurchaseOrder>(jsonData);
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            //}
            return View(poOrder);

        }

        /// <summary>
        /// Async call to get the list of cart Items based on the centralized Server User Id  which is updated in ServerUserId 
        /// in the User object
        /// </summary>
        /// <returns></returns>
        private async Task<List<CartItem>> GetCartItems()
        {
            List<CartItem> itemList = null;
            WebAPIRequest<List<CartItem>> request = new WebAPIRequest<List<CartItem>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.GetByUser,
                InvokeMethod = Method.GET,
                Id = LicenseSessionState.Instance.User.ServerUserId,
                ServiceModule = Modules.Cart,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService<List<CartItem>,List<CartItem>>(request);
            if (response.Status)
                itemList = response.ResponseData;
            else
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = await client.GetAsync("api/cart/getItems/" + LicenseSessionState.Instance.User.ServerUserId);
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    itemList = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            //}
            return itemList;
        }

        /// <summary>
        /// Async Action to add Subscription to the cart
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ActionResult> AddProductToCart(int? Id)
        {
            CartItem item = new CartItem()
            {
                SubscriptionId = Convert.ToInt32(Id),
                Quantity = 1,
                DateCreated = DateTime.Now,
                UserId = LicenseSessionState.Instance.User.ServerUserId
            };
            WebAPIRequest<CartItem> request = new WebAPIRequest<CartItem>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.Create,
                InvokeMethod = Method.POST,
                Id = LicenseSessionState.Instance.User.ServerUserId,
                ServiceModule = Modules.Cart,
                ServiceType = ServiceType.CentralizeWebApi,
                 ModelObject = item
            };
            var response = _invoke.InvokeService<CartItem,CartItem>(request);
            if (response.Status)
                return RedirectToAction("Index", "Subscription");
            else
                ModelState.AddModelError("", response.Error.error + " " + response.Error.Message);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = await client.PostAsJsonAsync("api/Cart/Create", item);
            //if (response.IsSuccessStatusCode)
            //    return RedirectToAction("Index", "Subscription");
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            //}
            return null;
        }

    }
}