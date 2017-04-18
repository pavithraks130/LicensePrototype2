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


        /// <summary>
        /// Post Method. Create Item Record in the cart for the Subscription which is added
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Method. Get the Cart Item Cout based on the UserId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// GET method. Get the Cart Items based on the UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetItems/{userId}")]
        public IHttpActionResult GetCartItems(string userId)
        {
            var itemList = logic.GetCartItems(userId);
            return Ok(itemList);
        }

        /// <summary>
        /// DELETE Method. Delete Item from the Cart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// POST Method. Service for the Offline Payment Which creates the Purchase Order and moves the Cart for the
        /// Approval from Global Admin and returns the Purchase Order Number to the user for the further Tracking
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// POST Method. Once the payment is done the User Subscription details will be updated and returns
        /// Subscription with product Details which was  purchased to sync the data to On Premise DB.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// POST Method. Create the Custom Subscription 
        /// </summary>
        /// <param name="subscriptionType"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateSubscriptionAddToCart")]
        public HttpResponseMessage CreateSubscriptionAddToCart(SubscriptionType subscriptionType)
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
