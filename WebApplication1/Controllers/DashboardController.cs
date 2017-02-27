using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.Core.Manager;

using License.Core.Model;
using License.Logic.Common;
using License.Logic.ServiceLogic;
using License.MetCalWeb;
using License.MetCalWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using License.MetCalWeb.Common;
using TeamMembers = License.Model.Model.TeamMembers;

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
            var model = new List<Product>();
            var product = new Product();
            product.CategoryID = 10;
            product.ImagePath = "../Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product.ProductID = 123;
            product.ProductName = "Calibrator";
            product.UnitPrice = 499;
            model.Add(product);

            Product product1 = new Product();
            product1.CategoryID = 20;
            product1.ImagePath = "../Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product1.ProductID = 123;
            product1.ProductName = "f-1730-hanger";
            product1.UnitPrice = 600;
            model.Add(product1);

            Product product3 = new Product();
            product3.CategoryID = 30;
            product3.ImagePath = "../Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product3.ProductID = 123;
            product3.ProductName = "f-3501-fc";
            product3.UnitPrice = 588;
            model.Add(product3);

            return View(model);
        }

    }
}