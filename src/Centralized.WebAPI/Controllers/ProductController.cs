using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;

namespace Centralized.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Product")]
    public class ProductController : BaseController
    {
        ProductLogic logic = null;
        public ProductController()
        {
            logic = new ProductLogic();
        }

        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAllProducts()
        {
            var proList = logic.GetProducts();
            return Ok(proList);
        }

    }
}
