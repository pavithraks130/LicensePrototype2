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
    [RoutePrefix("api/Cart")]
    public class CartController : ApiController
    {
        private CartLogic logic = null;

        public CartController()
        {
            logic = new CartLogic();
        }

        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateCart(CartItem item)
        {

            bool status = logic.CreateCartItem(item);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);

        }

        [HttpGet]
        [Route("GetItems/{userId}")]
        public IHttpActionResult GetCartItems(string userId)
        {
            var itemList = logic.GetCartItems(userId);
            return Ok(itemList);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeleteCartItem(int id)
        {
            var status = logic.DeleteCartItem(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}
