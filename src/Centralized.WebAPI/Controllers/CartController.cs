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
    [Authorize]
    [RoutePrefix("api/Cart")]
    public class CartController : BaseController
    {
        private CartLogic logic = null;
        private CartBO cartBOLogic = null;

        public CartController()
        {
            logic = new CartLogic();
            cartBOLogic = new CartBO();
        }

        public void Initialize()
        {
            cartBOLogic.UserManager = UserManager;
            cartBOLogic.RoleManager = RoleManager;
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
        [Route("GetCartItemCount/{userId}")]
        public HttpResponseMessage GetCartItemCount(string userId)
        {
            var count = logic.GetCartItems(userId).Count;
            if (count == 0)
                return Request.CreateResponse(HttpStatusCode.OK, "");
            else
                return Request.CreateResponse(HttpStatusCode.OK, count);
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
            Initialize();
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
            Initialize();
            var userSubscriptionList = cartBOLogic.OnlinePayment(userId);
            if (userSubscriptionList != null)
                return Request.CreateResponse(HttpStatusCode.OK, userSubscriptionList);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartBOLogic.ErrorMessage);

        }

        [HttpPost]
        [Route("CreateSubscriptionAddToCart")]
        public HttpResponseMessage CreateSubscriptionAddTpoCart(CustomSubscriptionType subscriptionType)
        {
            bool status = cartBOLogic.CreateSubscriptionAddToCart(subscriptionType);
            if (status)
                return Request.CreateResponse(HttpStatusCode.Created, "success");
            else if (String.IsNullOrEmpty(cartBOLogic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Due to internal issue the subscription can not be created");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartBOLogic.ErrorMessage);
        }
    }
}
