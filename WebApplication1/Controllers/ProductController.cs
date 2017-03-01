﻿using System;
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
        public ActionResult AddProductToCart(Product product)
        {
            CartItem item = new CartItem();
            item.SubscriptionTypeId = 1;
            item.Quantity = 2;
            item.DateCreated = DateTime.Now;
            item.UserId = LicenseSessionState.Instance.User.ServerUserId;
            //cartItemModel.ModelCartItem = item;
            //bool status = cartLogic.CreateCartItem(cartItemModel.ModelCartItem);

            bool status = cartLogic.CreateCartItem(item);
            return RedirectToAction("CartItem", "Cart");
            // return View();
        }



    }
}