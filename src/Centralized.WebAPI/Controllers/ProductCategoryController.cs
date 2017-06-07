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
    /// <summary>
    /// Cataegory Api Service Call for Getting the Subscription Category details.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/ProductCategory")]
    public class ProductCategoryController : BaseController
    {
        private ProductCategoryLogic logic = null;

        public ProductCategoryController()
        {
            logic = new ProductCategoryLogic();
        }
        /// <summary>
        /// Get Method: Service is used to get the Subscription Categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult Getcategories()
        {
            return Ok(logic.GetAll());
        }

        /// <summary>
        /// POSt Method: Create Subscription Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateCategory(SubscriptionCategory category)
        {
            var subCategory = logic.Create(category);
            if (subCategory != null && subCategory.Id > 0)
                return Request.CreateResponse(HttpStatusCode.Created, "Success");
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Get Method: Get Subscription category based on the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public HttpResponseMessage GetCategoryById(int id)
        {
            var obj = logic.GetById(id);
            if (obj != null)
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// PUT Method: Updating the Category changes based on the categoryId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update/{id}")]
        public HttpResponseMessage UpdateCategory(int id, SubscriptionCategory category)
        {
            var subCategory = logic.Update(id, category);
            if (subCategory != null && subCategory.Id > 0)
                return Request.CreateResponse(HttpStatusCode.OK, subCategory);
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Delete Method: Delte the Subscription Category by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeleteCategory(int id)
        {
            bool status = logic.Delete(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}
