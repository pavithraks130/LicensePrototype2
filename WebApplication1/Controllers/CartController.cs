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
            var obj = logic.GetCartItems(LicenseSessionState.Instance.User.UserId);
            return View(obj);
        }

        [HttpPost]
        public ActionResult AddProductToCart(int id)
        {
            CartItem item = new CartItem();
            item.SubscriptionTypeId = id;
            item.Quantity = 1;
            item.UserId = LicenseSessionState.Instance.User.UserId;
            bool status = logic.CreateCartItem(item);
            return RedirectToAction("Index", "cart");
        }
    }
}