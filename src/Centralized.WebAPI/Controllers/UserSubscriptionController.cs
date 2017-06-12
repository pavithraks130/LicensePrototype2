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
    /// Controller for User Subscription
    /// </summary>
    [RoutePrefix("api/UserSubscription")]
    public class UserSubscriptionController : BaseController
    {
        private UserSubscriptionLogic userSubscriptionLogic;

        /// <summary>
        /// Constructor for User Subscription
        /// </summary>
        public UserSubscriptionController()
        {
            userSubscriptionLogic = new UserSubscriptionLogic();
        }

        private void Initialize()
        {
            userSubscriptionLogic.UserManager = UserManager;
            userSubscriptionLogic.RoleManager = RoleManager;
        }

        /// <summary>
        /// Get Method: To the subscription which expires in month  based on the userId
        /// </summary>
        /// <param name="duration">No  of Days</param>
        /// <param name="userId">User ID</param>
        /// <returns>List of Subscriptions which are set to Expire</returns>
        [HttpGet]
        [Route("ExpireSubscription/{duration}/{userId}")]
        public HttpResponseMessage GetExpireSubscription(int duration,string userId)
        {
            var subList = userSubscriptionLogic.GetExpiringSubscription(userId, duration);
            if (!String.IsNullOrEmpty(userSubscriptionLogic.ErrorMessage))
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userSubscriptionLogic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, subList);
        }

        /// <summary>
        /// Post method: To renew the subscription which are selected for the renew based on the subscription ID. Input to this service
        /// is list of Subscription which need to Renewed for the  User.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="renewSubList">List of Subscription Renewals</param>
        /// <returns>Status Indicating Subscription Renewal</returns>
        [HttpPost]
        [Route("RenewSubscription/{userId}")]
        public HttpResponseMessage RenewSubscription(string userId, RenewSubscriptionList renewSubList)
        {
            Initialize();
            var subscriptionList = userSubscriptionLogic.RenewSubscription(renewSubList, userId);
            if (!String.IsNullOrEmpty(userSubscriptionLogic.ErrorMessage))
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userSubscriptionLogic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, subscriptionList);
        }
    }
}
