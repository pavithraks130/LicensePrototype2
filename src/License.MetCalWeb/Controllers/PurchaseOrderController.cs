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
        public ActionResult Index(string comment, string button, params string[] selectedPurchaseOrder)
        {
            List<int> poIdList = new List<int>();
            foreach (string str in selectedPurchaseOrder)
            {
                poIdList.Add(Convert.ToInt32(str));
            }
            List<PurchaseOrder> orderList = new List<PurchaseOrder>();
            HttpClient client = WebApiServiceLogic.CreateClient(webApitype.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var result = await client.GetAsync("api/purchaseorder/All");
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                orderList = JsonConvert.DeserializeObject<List<PurchaseOrder>>(data);
            }
            var orderList = orderLogic.GetPurchaseOrderByIds(poIdList);
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
            orderLogic.UpdatePurchaseOrder(orderList);
            return RedirectToAction("Index", "User");
        }

        public ActionResult OrderStatus()
        {
            var poList = orderLogic.GetPurchaseOrderByUser(LicenseSessionState.Instance.User.ServerUserId);
            return View(poList);
        }

        public ActionResult OrderDetail(int id)
        {
            var order = orderLogic.GetProductById(id);
            return View(order);
        }
    }
}