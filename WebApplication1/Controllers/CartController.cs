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

        CartLogic cartLogic = null;

        public CartController()
        {
            cartLogic = new CartLogic();
        }

        public ActionResult CartItem()
        {
            var obj = cartLogic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId);
            return View(obj);
        }

        //[HttpPost]
        //public ActionResult AddProductToCart(int id)
        //{
        //    CartItem item = new CartItem();
        //    item.SubscriptionTypeId = id;
        //    item.Quantity = 1;
        //    item.UserId = LicenseSessionState.Instance.User.UserId;
        //    bool status = logic.CreateCartItem(item);
        //    return RedirectToAction("Index", "Cart");
        //}

        [HttpPost]
        public ActionResult RemoveItemFromCart()
        {
            var x = cartLogic.GetCartItems(LicenseSessionState.Instance.User.ServerUserId).Where(y =>y.Id==5).FirstOrDefault();
            cartLogic.CreateCartItem(x);
            return RedirectToAction("CartItem", "Cart");
        }

        [HttpPost]
        public ActionResult PaymentGateway()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DoPayment()
        {
            return View();

        }

    }
}