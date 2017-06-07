using LicenseServer.DataModel;
using LicenseServer.Logic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Centralized.WebAPI.Controllers
{
    /// <summary>
    /// Controller for Subscription
    /// </summary>
    [Authorize]
    [RoutePrefix("api/subscription")]
    public class SubscriptionController : BaseController
    {

        private SubscriptionLogic subscriptionLogic = null;

        /// <summary>
        /// Constructor for Subscrtiption Controller
        /// </summary>
        public SubscriptionController()
        {
            subscriptionLogic = new SubscriptionLogic();
        }

        /// <summary>
        /// GET Method. To get all the Subscriptions
        /// </summary>
        /// <returns>Subscription List</returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAllSubscriptions()
        {
            var subList = subscriptionLogic.GetAllSubscriptions();
            return Ok(subList);
        }

        /// <summary>
        /// Get List of Subscriptions Both Default and Custom Subscriptions
        /// </summary>
        /// <param name="userId">User ID to include Custom Subscriptions</param>
        /// <returns>Subscription List</returns>
        [HttpGet]
        [Route("All/{userId}")]
        public IHttpActionResult GetAllSubscription(string userId)
        {
            var subList = subscriptionLogic.GetAllSubscriptions(userId);
            return Ok(subList);
        }

        /// <summary>
        /// POST method. Creates the Subscriptioon
        /// </summary>
        /// <param name="subscriptionItem">Subscription Item to be Created</param>
        /// <returns>On success returns created Subscription else returns error</returns>
        [HttpPost]
        [Route("CreateSubscription")]
        public HttpResponseMessage CreateSubscription(Subscription subscriptionItem)
        {
            var subscriptionType = subscriptionLogic.CreateSubscriptionWithProduct(subscriptionItem);
            if (subscriptionType != null)
                return Request.CreateResponse(HttpStatusCode.Created, subscriptionType);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, subscriptionLogic.ErrorMessage);
        }
    }
}
