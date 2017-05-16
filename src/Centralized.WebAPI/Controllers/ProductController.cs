using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;
using LicenseServer.Logic.BusinessLogic;
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

        /// <summary>
        /// GET Method. Gets all the Products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAllProducts()
        {
            var proList = logic.GetProducts();
            return Ok(proList);
        }

        /// <summary>
        /// POST Method. Create Product Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Put Method. Update the Product details based on the Product ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{id}")]
        public HttpResponseMessage UpdateProduct(int id, Product model)
        {
            var status = logic.UpdateProduct(id, model);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated the Product");
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Failed to Update Product due to internal error");
        }


        /// <summary>
        /// Delete Method. Delete Product based on the Product ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeleteProduct(int id)
        {
            var status = logic.DeleteProduct(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.Accepted, "Deleted the Product");
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Failed to Delete Product due to internal error");
        }

        [HttpGet]
        [Route("ProductDependency")]
        public HttpResponseMessage GetProductDependency()
        {
            ProductBO productBo = new ProductBO();
            var obj = productBo.GetDependencyDetails();
            if (obj != null)
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
        
        [HttpGet]
        [Route("GetById/{Id}")]
        public HttpResponseMessage GetProductById(int Id)
        {
            var obj = logic.GetProductById(Id);
            if (obj != null)
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpGet]
        [Route("GetCMMSProducts")]
        public IHttpActionResult GetCMMSProducts()
        {
            var productList = logic.GetCMMSProducts();
            return Ok(productList);
        }

        [HttpGet]
        [Route("ProductByCategory/{Id}")]
        public HttpResponseMessage GetProductByCategory(int Id)
        {
            var obj = logic.GetProductByCategoryId(Id);
            if (obj != null)
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }



    }
}
