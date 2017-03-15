using System;
using System.Collections.Generic;
using System.Web.Mvc;
using License.Logic.ServiceLogic;
using License.MetCalWeb.Models;
using License.Core.Model;
using System.Threading;

namespace License.MetCalWeb.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private TeamMemberLogic logic = null;
        private UserLogic userLogic = null;

        public DashboardController()
        {
            logic = new TeamMemberLogic();
            userLogic = new UserLogic();
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            var model = new List<Models.ProductModel>();
            var product = new Models.ProductModel();
            product.ImagePath = "../Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product.ProductName = "Calibrator";
            model.Add(product);

            var product1 = new Models.ProductModel();
            product1.ImagePath = "../Catalog/Images/Thumbs/f-1730-4402_01a_s.png";
            product1.ProductName = "f-1730-4402_01a_s";
            model.Add(product1);

            var product3 = new Models.ProductModel();
            product3.ImagePath = "../Catalog/Images/Thumbs/f-cxt1000_02a_h.png";
            product3.ProductName = "f-cxt1000_02a_h";
            model.Add(product3);

            return View(model);
        }
       
        public ActionResult CartDetails()
        {
            return View();
        }
       
    }
}