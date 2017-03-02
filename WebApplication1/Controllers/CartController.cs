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
            var obj = logic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId);
            ViewData["TotalAmount"] = logic.TotalAmount;
            return View(obj);
        }

        public void Purchase(CartItem item)
        {
            //CartItem item = logic.GetCartItemById(id);
            UserSubscription subscription = new UserSubscription();
            subscription.SubscriptionDate = DateTime.Now.Date;
            subscription.SubscriptionTypeId = item.SubscriptionTypeId;
            subscription.Quantity = item.Quantity;
            subscription.UserId = LicenseSessionState.Instance.User.ServerUserId;
            SubscriptionTypeLogic typeLogic = new SubscriptionTypeLogic();
            var type = typeLogic.GetById(item.SubscriptionTypeId);
            subscription.ActiveDurataion = type.ActiveDays;
            UserSubscriptionLogic subscriptionLogic = new UserSubscriptionLogic();
            UserSubscription subs = subscriptionLogic.CreateUserSubscription(subscription);
            UpdateSubscriptionOnpremise(subs, type.Name);
            item.IsPurchased = true;
            logic.UpdateCartItem(item);
        }


        private void UpdateSubscriptionOnpremise(UserSubscription subs, string subscriptionName)
        {
            License.Model.Model.UserSubscription subscription = new Model.Model.UserSubscription();
            subscription.ServerUserId = LicenseSessionState.Instance.User.ServerUserId;
            subscription.UserId = LicenseSessionState.Instance.User.UserId;
            subscription.SubscriptionDate = subs.SubscriptionDate;
            subscription.SubscriptionId = subs.SubscriptionTypeId;
            subscription.SubscriptionName = subscriptionName;
            License.Logic.ServiceLogic.UserSubscriptionLogic logic = new Logic.ServiceLogic.UserSubscriptionLogic();
            int subscriptionId = logic.CreateSubscription(subscription);
            License.Logic.ServiceLogic.LicenseLogic licLogic = new Logic.ServiceLogic.LicenseLogic();
            if (subscriptionId > 0)
            {

                foreach (var str in subs.LicenseKeys)
                {
                    License.Model.Model.LicenseData data = new Model.Model.LicenseData();
                    data.AdminUserId = LicenseSessionState.Instance.User.UserId;
                    data.LicenseKey = str;
                    data.SubscriptionId = subscriptionId;
                    licLogic.CreateLicenseData(data);
                }
                if (subs.LicenseKeys.Count > 0)
                    licLogic.Save();
            }
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
            //int id = Convert.ToInt32(TempData["cartId"]);
            var obj = logic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId);
            foreach (var item in obj)
            {
                Purchase(item);
            }
            return View();
        }

    }
}