using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb.Common;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using License.MetCalWeb.Models;

namespace License.MetCalWeb.Controllers
{
    public class PurchaseOrderController : Controller
    {

        ServiceType webApitype;

        public PurchaseOrderController()
        {
            var serviceType = System.Configuration.ConfigurationManager.AppSettings.Get("ServiceType");
            webApitype = (ServiceType)Enum.Parse(typeof(ServiceType), serviceType);
        }

        // GET: PurchaseOrder
        public async Task<ActionResult> Index()
        {
            List<PurchaseOrder> orderList = new List<PurchaseOrder>();
            HttpClient client = WebApiServiceLogic.CreateClient(webApitype.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var result = await client.GetAsync("api/purchaseorder/All");
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                orderList = JsonConvert.DeserializeObject<List<PurchaseOrder>>(data);
            }
            return View(orderList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string comment, string button, params string[] selectedPurchaseOrder)
        {
            List<PurchaseOrder> orderList = new List<PurchaseOrder>();
            foreach (string str in selectedPurchaseOrder)
            {
                PurchaseOrder po = new PurchaseOrder();
                po.Id = Convert.ToInt32(str);
                orderList.Add(po);
            }

            bool isApproved = false;
            switch (button)
            {
                case "Approve": isApproved = true; break;
                case "Reject": isApproved = false; break;
            }
            foreach (var obj in orderList)
            {
                obj.IsApproved = isApproved;
                obj.Comment = comment;
                obj.ApprovedBy = LicenseSessionState.Instance.User.UserName;
            }
            HttpClient client = WebApiServiceLogic.CreateClient(webApitype.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var result = await client.PutAsJsonAsync("/api/purchaseorder/update", orderList);
            if (result.IsSuccessStatusCode)
                return RedirectToAction("Index", "User");
            else
            {
                ModelState.AddModelError("", result.ReasonPhrase);
            }
            return View();
        }

        public async Task<ActionResult> OrderStatus()
        {
            List<PurchaseOrder> poList = new List<PurchaseOrder>();
            HttpClient client = WebApiServiceLogic.CreateClient(webApitype.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.GetAsync("api/purchaseorder/OrderByUser/" + LicenseSessionState.Instance.User.ServerUserId);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                poList = JsonConvert.DeserializeObject<List<PurchaseOrder>>(jsonData);
            }
            return View(poList);
        }

        public async Task<ActionResult> OrderDetail(int id)
        {
            PurchaseOrder order = new PurchaseOrder();
            HttpClient client = WebApiServiceLogic.CreateClient(webApitype.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.GetAsync("api/purchaseorder/OrderById/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                order = JsonConvert.DeserializeObject<PurchaseOrder>(jsonData);
            }
            return View(order);
        }
    }
}