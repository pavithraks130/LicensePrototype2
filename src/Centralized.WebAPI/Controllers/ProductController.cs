using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;
using LicenseServer.DataModel;

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

        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateProduct(Product model)
        {
            var status = logic.CreateProduct(model);
            if (status)
                return Request.CreateResponse(HttpStatusCode.Created, "Created the Product");
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Failed to create Product due to internal error");
        }

        [HttpPut]
        [Route("update/{id}")]
        public HttpResponseMessage CreateProduct(int id, Product model)
        {
            var status = logic.UpdateProduct(id, model);
            if (status)
                return Request.CreateResponse(HttpStatusCode.Found, "Updated the Product");
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Failed to Update Product due to internal error");
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage CreateProduct(int id)
        {
            var status = logic.DeleteProduct(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.Accepted, "Deleted the Product");
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Failed to Delete Product due to internal error");
        }

    }
}
