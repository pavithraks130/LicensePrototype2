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
    [RoutePrefix("api/ProductCategory")]
    public class ProductCategoryController : BaseController
    {
        private ProductCategoryLogic logic = null;

        public ProductCategoryController()
        {
            logic = new ProductCategoryLogic();
        }

        [HttpGet]
        [Route("All")]
        public IHttpActionResult Getcategories()
        {
            return Ok(logic.GetAll());
        }

        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateCategory(ProductCategory category)
        {
            bool status = logic.Create(category);
            if (status)
                return Request.CreateResponse(HttpStatusCode.Created, "Success");
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

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

        [HttpPut]
        [Route("Update/{id}")]
        public HttpResponseMessage UpdateCategory(int id, ProductCategory category)
        {
            bool status = logic.Update(id, category);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated");
            else
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

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
