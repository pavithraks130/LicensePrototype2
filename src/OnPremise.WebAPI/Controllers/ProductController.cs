using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.DataModel;
using License.Logic.DataLogic;

namespace OnPremise.WebAPI.Controllers
{
    [RoutePrefix("api/Product")]
    [Authorize]
    public class ProductController : BaseController
    {
        ProductLogic logic = null;

        public ProductController()
        {
            logic = new ProductLogic();
        }

        [HttpGet]
        [Route("GetProductsByAdminId/{adminUserId}s")]
        public IHttpActionResult GetProductsByAdmin(string adminUserId)
        {
            var productList = logic.GetProductbyAdmin(adminUserId);
            return Ok(productList);
        }

        [HttpPost]
        [Route("UpdateProducts")]
        public HttpResponseMessage UpdateProducts(List<Product> productList)
        {
            logic.UpdateProducts(productList);
            return Request.CreateResponse(HttpStatusCode.OK, "success");
        }
    }
}
