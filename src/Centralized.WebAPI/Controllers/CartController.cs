using Centralized.WebAPI.Common;
using License.Models;
using LicenseServer.Logic;
using LicenseServer.Logic.BusinessLogic;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Centralized.WebAPI.Controllers
{
    /// <summary>
    /// Controller for Cart Operations
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Cart")]
    public class CartController : BaseController
    {
        private CartLogic cartLogic = null;
        private CartBO cartBOLogic = null;

        /// <summary>
        /// Constructor for Cart Class
        /// </summary>
        public CartController()
        {
            cartLogic = new CartLogic();
            cartBOLogic = new CartBO();
        }

        private void Initialize()
        {
            cartBOLogic.UserManager = UserManager;
            cartBOLogic.RoleManager = RoleManager;
        }


        /// <summary>
        /// POST Method. Creates Cart Item Record in the cart for the Subscription which is added
        /// </summary>
        /// <param name="item">The Cart Item</param>
        /// <returns>On success returns created cart else returns error</returns>
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateCart(CartItem item)
        {

            var cart = cartLogic.CreateCartItem(item);
            if (cart?.Id > 0)
                return Request.CreateResponse(HttpStatusCode.OK, cart);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartLogic.ErrorMessage);

        }

        /// <summary>
        /// Get Method. Gets the Cart Item Count based on the UserId.
        /// </summary>
        /// <param name="userId">The User ID</param>
        /// <returns>Count of Number of Cart Items</returns>
        [HttpGet]
        [Route("GetCartItemCount/{userId}")]
        public HttpResponseMessage GetCartItemCount(string userId)
        {
            var cartItemsCount = cartLogic.GetCartItems(userId).Count;
            if (cartItemsCount == 0)
                return Request.CreateResponse(HttpStatusCode.OK, "");
            else
                return Request.CreateResponse(HttpStatusCode.OK, cartItemsCount);
        }

        /// <summary>
        /// GET method. Get the Cart Items based on the UserId
        /// </summary>
        /// <param name="userId">The User ID</param>
        /// <returns>The Cart Items Based on User ID</returns>
        [HttpGet]
        [Route("GetItems/{userId}")]
        public IHttpActionResult GetCartItems(string userId)
        {
            var cartItemList = cartLogic.GetCartItems(userId);
            return Ok(cartItemList);
        }

        /// <summary>
        /// DELETE Method. Delete Item from the Cart
        /// </summary>
        /// <param name="id">The Cart Item ID</param>
        /// <returns>Status of whether Cart Item was deleted</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeleteCartItem(int id)
        {
            var status = cartLogic.DeleteCartItem(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, Constants.Success);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartLogic.ErrorMessage);
        }

        /// <summary>
        /// POST Method. Service for the Offline Payment Which creates the Purchase Order and moves the Cart for the
        /// Approval from Global Admin and returns the Purchase Order Number to the user for the further Tracking
        /// </summary>
        /// <param name="userId">The User ID</param>
        /// <returns>On success returns created Purchase Order else returns error</returns>
        [HttpPost]
        [Route("OfflinePayment/{userId}")]
        public HttpResponseMessage OfflinePayment(string userId)
        {
            Initialize();
            var purchasaeOrder = cartBOLogic.OfflinePayment(userId);
            if (purchasaeOrder != null)
                return Request.CreateResponse(HttpStatusCode.OK, purchasaeOrder);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartBOLogic.ErrorMessage);

        }

        /// <summary>
        /// POST Method. Once the payment is done the User Subscription details will be updated and returns
        /// Subscription with product Details which was  purchased to sync the data to On Premise DB.
        /// </summary>
        /// <param name="userId">The User ID</param>
        /// <returns>Current Subscription List</returns>
        [HttpPost]
        [Route("OnlinePayment/{userId}")]
        public HttpResponseMessage OnlinePayment(string userId)
        {
            Initialize();
            var currentSubscriptionList = cartBOLogic.OnlinePayment(userId);
            if (currentSubscriptionList != null)
                return Request.CreateResponse(HttpStatusCode.OK, currentSubscriptionList);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartBOLogic.ErrorMessage);

        }
        
        /// <summary>
        /// POST Method. Create the Custom Subscription 
        /// </summary>
        /// <param name="subscriptionType">Subscription Type</param>
        /// <returns>Status of Subscription creation</returns>
        [HttpPost]
        [Route("CreateSubscriptionAddToCart")]
        public HttpResponseMessage CreateSubscriptionAddToCart(Subscription subscriptionType)
        {
            bool status = cartBOLogic.CreateSubscriptionAddToCart(subscriptionType);
            if (status)
                return Request.CreateResponse(HttpStatusCode.Created, Constants.Success);
            else if (String.IsNullOrEmpty(cartBOLogic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Due to internal issue the subscription cannot be created");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartBOLogic.ErrorMessage);
        }
    }
}
