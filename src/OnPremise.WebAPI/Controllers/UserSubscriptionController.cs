using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.DataModel;
using License.Logic.BusinessLogic;

namespace OnPremise.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/UserSubscriptions")]
    public class UserSubscriptionController : BaseController
    {
        UserSubscriptionBO usersubBOLogic = null;
        public UserSubscriptionController()
        {
            usersubBOLogic = new UserSubscriptionBO();
        }

        [HttpPost]
        [Route("SyncSubscription")]

        public HttpResponseMessage CreateSubscription(List<UserSubscriptionData> subscriptionData)
        {
            usersubBOLogic.UpdateUserSubscription(subscriptionData);
            return Request.CreateResponse(HttpStatusCode.OK, "Updated");
        }

        [HttpGet]
        [Route("SubscriptionDetils")]
        public HttpResponseMessage GetSubscriptionDetails(string userId)
        {

        }

    }
}
