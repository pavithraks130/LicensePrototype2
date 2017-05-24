using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;

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

    }
}
