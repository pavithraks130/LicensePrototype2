using System;
using System.Collections.Generic;
using System.Web.Mvc;
using License.Logic.ServiceLogic;

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
            var model = new List<Models.Product>();
            var product = new Models.Product();
            product.ImagePath = "../Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product.ProductName = "Calibrator";
            model.Add(product);

            var product1 = new Models.Product();
            product1.ImagePath = "../Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product1.ProductName = "f-1730-hanger";
            model.Add(product1);

            var product3 = new Models.Product();
            product3.ImagePath = "../Catalog/Images/Thumbs/f-3510-fc_01a_h-708x490.png";
            product3.ProductName = "f-3501-fc";
            model.Add(product3);

            return View(model);
        }

    }
}