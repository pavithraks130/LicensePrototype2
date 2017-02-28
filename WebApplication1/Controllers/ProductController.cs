using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Logic.ServiceLogic;
using System.Web.Mvc;
using License.MetCalWeb.Models;

namespace License.MetCalWeb.Controllers
{
    public class ProductController : BaseController
    {
        ProductLogic logic = null;
        public ProductController()
        {
            logic = new ProductLogic();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "DashBoard");
            //var obj = logic.GetProducts();
            //return View(obj);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product pro, HttpPostedFileBase productImage)
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
            logic.CreateProduct(pro.ModelProduct);
            return RedirectToAction("Index");
        }
    }
}