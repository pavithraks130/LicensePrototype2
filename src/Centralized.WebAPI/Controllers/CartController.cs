using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;
using LicenseServer.DataModel;
using LicenseServer.Logic.BusinessLogic;

namespace Centralized.WebAPI.Controllers
{
    [RoutePrefix("api/Cart")]
    public class CartController : ApiController
    {
        private CartLogic logic = null;
        private CartBO cartBOLogic = null;

        public CartController()
        {
            logic = new CartLogic();
            cartBOLogic = new CartBO();
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

        [HttpPost]
        [Route("OfflinePayment/{userId}")]
        public HttpResponseMessage OfflinePayment(string userId)
        {
            var poOrder = cartBOLogic.OfflinePayment(userId);
            if (poOrder != null)
                return Request.CreateResponse(HttpStatusCode.OK, poOrder);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartBOLogic.ErrorMessage);

        }

        [HttpPost]
        [Route("OnlinePayment/{userId}")]
        public HttpResponseMessage OnlinePayment(string userId)
        {
            var userSubscriptionList = cartBOLogic.OnlinePayment(userId);
            if (userSubscriptionList != null)
                return Request.CreateResponse(HttpStatusCode.OK, userSubscriptionList);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartBOLogic.ErrorMessage);

        }
    }
}
