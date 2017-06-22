using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.Models;
using License.Logic.DataLogic;
using License.Logic.BusinessLogic;

namespace OnPremise.WebAPI.Controllers
{
    [RoutePrefix("api/Product")]
    [Authorize]
    public class ProductController : BaseController
    {
        ProductLogic productLogic = null;
     

        public ProductController()
        {
            productLogic = new ProductLogic();
        }

        /// <summary>
        /// GET Method : Gets products based on the admin Id
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductsByAdminId/{adminId}")]
        public IHttpActionResult GetProductsByAdmin(string adminId)
        {
            var productList = productLogic.GetProductbyAdmin(adminId);
            return Ok(productList);
        }

        /// <summary>
        /// POST Method: Used to Update the products in bulk
        /// </summary>
        /// <param name="productList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateProducts")]
        public HttpResponseMessage UpdateProducts(List<Product> productList)
        {
            productLogic.UpdateProducts(productList);
            return Request.CreateResponse(HttpStatusCode.OK, "success");
        }

        /// <summary>
        /// Get Method: Gets the list of the Subscribed Products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProducts/{adminId}")]
        public HttpResponseMessage GetSubscriptionProduct(string adminId)
        {
            ProductBO proBo = new ProductBO();
            var data = proBo.GetProductFromLicenseData(adminId);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        /// <summary>
        /// Get Method, To fetchc the Products list along with user products which is already mapped to the User.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <returns>List of Products</returns>
        [HttpGet]
        [Route("GetProductsWithUserMappedProduct/{adminId}/{userId}")]
        public HttpResponseMessage GetProductsWithUserMappedProduct(string adminId,string userId)
        {
            ProductBO proBo = new ProductBO();
            var data = proBo.GetProductFromLicenseData(adminId,userId);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

       

    }
}
