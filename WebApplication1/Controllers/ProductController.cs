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
            var obj = logic.GetProducts();
            return View(obj);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product pro)
        {

            return View("Index");
        }
    }
}