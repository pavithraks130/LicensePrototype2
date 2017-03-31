using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LicenseServer.Logic;
using LicenseServer.DataModel;

namespace License.MetCalWeb.Controllers
{
    public class CartController : BaseController
    {

        CartLogic logic = null;

        public CartController()
        {
            logic = new CartLogic();
        }

        public ActionResult CartItem()
        {
            if (LicenseSessionState.Instance.User == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            var obj = logic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId);
            ViewData["TotalAmount"] = logic.TotalAmount;
            return View(obj);
        }

        public ActionResult RemoveItem(int id)
        {
            logic.DeleteCartItem(id);
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
            var obj = logic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId);

            List<UserSubscription> subsList = new List<UserSubscription>();
            foreach (var item in obj)
            {

                UserSubscription usersubs = new UserSubscription();
                usersubs.UserId = LicenseSessionState.Instance.User.ServerUserId;
                usersubs.SubscriptionTypeId = item.SubscriptionTypeId;
                usersubs.SubscriptionDate = DateTime.Now.Date;
                usersubs.Quantity = item.Quantity;
                subsList.Add(usersubs);
            }
            Common.CentralizedSubscriptionLogic.UpdateUserSubscription(subsList);
            foreach (var item in obj)
            {
                item.IsPurchased = true;
                logic.UpdateCartItem(item);
            }
        }

        public ActionResult OfflinePayment()
        {
            PurchaseOrder poOrder = new PurchaseOrder();
            var cartItemList = logic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId);
            if (cartItemList.Count > 0)
            {
                PurchaseOrderLogic pologic = new PurchaseOrderLogic();
                POItemLogic itemLogic = new POItemLogic();
                poOrder.UserId = LicenseSessionState.Instance.User.ServerUserId;
                poOrder.CreatedDate = DateTime.Now.Date;
                poOrder = pologic.CreatePurchaseOrder(poOrder);
                List<PurchaseOrderItem> itemList = new List<PurchaseOrderItem>();
                foreach (CartItem ci in cartItemList)
                {
                    var item = new PurchaseOrderItem();
                    item.Quantity = ci.Quantity;
                    item.SubscriptionId = ci.SubscriptionTypeId;
                    itemList.Add(item);
                }
                itemLogic.CreateItem(itemList, poOrder.Id);
                foreach (CartItem ci in cartItemList)
                {
                    ci.IsPurchased = true;
                    logic.UpdateCartItem(ci);
                }
            }
            return View(poOrder);

        }

    }
}