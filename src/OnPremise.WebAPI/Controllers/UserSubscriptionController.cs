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
    [RoutePrefix("api/UserSubscription")]
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
        [Route("SubscriptionDetils/{adminId}")]
        public HttpResponseMessage GetSubscriptionDetails(string adminId)
        {
            var lstSubscriptionDetails = usersubBOLogic.GetSubscriptionList(adminId);
            if (lstSubscriptionDetails.Count == 0)
                return Request.CreateResponse(HttpStatusCode.OK, "");
            else
                return Request.CreateResponse(HttpStatusCode.OK, lstSubscriptionDetails);
        }

    }
}
