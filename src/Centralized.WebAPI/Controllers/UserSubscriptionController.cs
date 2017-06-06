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
    [RoutePrefix("api/UserSubscription")]
    public class UserSubscriptionController : BaseController
    {
        private UserSubscriptionLogic logic;

        public UserSubscriptionController()
        {
            logic = new UserSubscriptionLogic();
        }

        private void Initialize()
        {
            logic.UserManager = UserManager;
            logic.RoleManager = RoleManager;
        }

        /// <summary>
        /// Get Method: To the subscription which expires in month  based on the userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("ExpireSubscription/{userId}")]
        public HttpResponseMessage GetExpireSubscription(string userId)
        {
            var subList = logic.GetExpiringSubscription(userId);
            if (!String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, subList);
        }

        /// <summary>
        /// Post method: To renew the subscription which are selected for the renew based on the subscription ID. Input to this service
        /// is list of Subscription which need to Renewed for the  User.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="renewSubList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RenewSubscription/{userId}")]
        public HttpResponseMessage RenewSubscription(string userId, RenewSubscriptionList renewSubList)
        {
            Initialize();
            var subscriptionList = logic.RenewSubscription(renewSubList, userId);
            if (!String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, subscriptionList);
        }
    }
}
