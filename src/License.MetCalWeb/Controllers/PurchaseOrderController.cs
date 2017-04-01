using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LicenseServer.Logic;
namespace License.MetCalWeb.Controllers
{
    public class PurchaseOrderController : Controller
    {
        PurchaseOrderLogic orderLogic = null;
        public PurchaseOrderController()
        {
            orderLogic = new PurchaseOrderLogic();
        }
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            var list = orderLogic.GetAllPendingPurchaseOrder();
            return View(list);
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