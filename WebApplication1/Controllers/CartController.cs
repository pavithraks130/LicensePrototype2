using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Logic.ServiceLogic;

namespace License.MetCalWeb.Controllers
{
    public class CartController : BaseController
    {

        CartLogic logic = null;

        public CartController()
        {
            logic = new CartLogic();
        }

        public ActionResult Index()
        {
            var obj = logic.GetCartItems(LicenseSessionState.Instance.User.UserId);
            return View(obj);
        }

        [HttpPost]
        public ActionResult AddProductToCart(int id)
        {
            Model.Model.CartItem item = new Model.Model.CartItem();
            item.ProductId = id;
            item.Quantity = 1;
            item.UserId = LicenseSessionState.Instance.User.UserId;
            bool status = logic.CreateCartItem(item);
            return RedirectToAction("Index", "cart");
        }
    }
}