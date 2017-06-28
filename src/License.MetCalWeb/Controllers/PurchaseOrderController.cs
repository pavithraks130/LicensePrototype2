using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb.Common;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using License.Models;
using License.ServiceInvoke;

namespace License.MetCalWeb.Controllers
{
    /// <summary>
    /// Controller to perform purchase order related functionality
    /// </summary>
    [Authorize]
    [SessionExpire]
    public class PurchaseOrderController : Controller
    {
        public PurchaseOrderController()
        {

        }

        /// <summary>
        /// Get Action to list all the purchase Order  for approve reject the Purchase order. Purchase order list is sent view
        /// for listing the order
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            List<PurchaseOrder> orderList = new List<PurchaseOrder>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = await client.GetAsync("api/purchaseorder/All");
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                orderList = JsonConvert.DeserializeObject<List<PurchaseOrder>>(data);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View(orderList);
        }

        /// <summary>
        /// POSt Action is triggered when the  form submitted for the approval or rejected, service call made to update the data
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="button"></param>
        /// <param name="selectedPurchaseOrder"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string comment, string button, params string[] selectedPurchaseOrder)
        {
            bool isApproved = false;
            switch (button)
            {
                case "Approve": isApproved = true; break;
                case "Reject": isApproved = false; break;
            }
            var orderList = selectedPurchaseOrder.ToList().Select(poId => new PurchaseOrder()
            {
                Id = Convert.ToInt32(poId),
                IsApproved = isApproved,
                Comment = comment,
                ApprovedBy = LicenseSessionState.Instance.User.UserName
            }).ToList();

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = await client.PutAsJsonAsync("/api/purchaseorder/UpdataMuliplePO", orderList);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "User");
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View();
        }

        /// <summary>
        /// Get Action to list all the Ourchase order with the status listing.
        /// Returns view with list of Purchase order which will listed in the view with the sstataus
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OrderStatus()
        {
            List<PurchaseOrder> poList = new List<PurchaseOrder>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = await client.GetAsync("api/purchaseorder/OrderByUser/" + LicenseSessionState.Instance.User.ServerUserId);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                poList = JsonConvert.DeserializeObject<List<PurchaseOrder>>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View(poList);
        }

        /// <summary>
        ///  Get Aaction. On click on the Purchaase order will be redirected to the detail view to view the order history.
        ///  View will be returned with the PO Order Detail based on PO Order id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> OrderDetail(int id)
        {
            PurchaseOrder order = new PurchaseOrder();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = await client.GetAsync("api/purchaseorder/OrderById/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                order = JsonConvert.DeserializeObject<PurchaseOrder>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View(order);
        }
    }
}