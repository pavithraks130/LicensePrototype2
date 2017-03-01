using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Logic.ServiceLogic;
using System.Web.Mvc;
using License.MetCalWeb.Models;
using LicenseServer.Logic;
using LicenseServer.DataModel;

namespace License.MetCalWeb.Controllers
{
    public class ProductController : BaseController
    {
        ProductLogic productLogic = null;
        CartLogic cartLogic = null;
        SubscriptionTypeLogic subscriptionTypeLogic = null;
        public ProductController()
        {
            productLogic = new ProductLogic();
            cartLogic = new CartLogic();
            subscriptionTypeLogic = new SubscriptionTypeLogic();
        }

        public ActionResult ProductCatalog()
        {

            var obj = productLogic.GetProducts();
            return View(obj);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductModel pro, HttpPostedFileBase productImage)
        {
            if (productImage != null)
            {
                var imageType = productImage.FileName.Substring(productImage.FileName.ToString().LastIndexOf('.'));
                if (imageType.ToString().ToLower() != ".png" && imageType.ToString().ToLower() != ".jpg" && imageType.ToString().ToLower() != ".jpeg")
                {
                    ModelState.AddModelError("", "Uploaded profile image should be jpg or png extension.");
                    return View(pro);
                }
                var imageName = pro.ProductName + imageType;
                if (productImage != null)
                {
                    pro.ImagePath = imageName;
                    productImage.SaveAs(Server.MapPath("~/ProductImages/") + imageName);

                }
            }
            productLogic.CreateProduct(pro.ModelProduct);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddProductToCart(CartItemModel cartItemModel)
        {
            CartItem item = new CartItem();
            SubscriptionType s1 = new SubscriptionType();
            s1.Id = 20;
            s1.Name = "Premium";
            s1.ActiveDays = 200;
            subscriptionTypeLogic.CreateSubscription(s1);

            item.Id = 1;
            item.SubscriptionTypeId = 10;
            item.Quantity = 2;
            item.SubType = s1;
            item.DateCreated = DateTime.Now;
            item.UserId = LicenseSessionState.Instance.User.UserId;
            cartItemModel.ModelCartItem = item;
            bool status = cartLogic.CreateCartItem(cartItemModel.ModelCartItem);
            return RedirectToAction("CartItem", "cart");
            // return View();
        }
    }
}