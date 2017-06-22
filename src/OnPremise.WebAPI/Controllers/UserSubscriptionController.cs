using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.Models;
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

        /// <summary>
        /// POST Method: Sync the subscription from Centralized to ON premise
        /// </summary>
        /// <param name="subscriptionData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncSubscription")]

        public HttpResponseMessage CreateSubscription(SubscriptionList subscriptionData)
        {
            usersubBOLogic.UpdateUserSubscription(subscriptionData);
            return Request.CreateResponse(HttpStatusCode.OK, "Updated");
        }

        /// <summary>
        ///  POST Method: To update the renewal  subscription details
        /// </summary>
        /// <param name="subscriptionList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSubscriptionRenewal/{userId}")]
        public HttpResponseMessage UpdateSubscriptionRenewal(SubscriptionList subscriptionList,string userId)
        {
            usersubBOLogic.UpdateSubscriptionRenewal(subscriptionList, userId);
            return Request.CreateResponse(HttpStatusCode.OK, "Updated");
        }

        /// <summary>
        /// GET Method : to get the subscription details by admin ID
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Method: to get the subscription License based on the admin id and User Id  for the license Map
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSubscriptioDtlsForLicenseMap/{adminId}/{userId}")]
        public HttpResponseMessage GetSubscriptionDetailsForLicenseMap(string adminId,string userId)
        {
            var lstSubscriptionDetails = usersubBOLogic.GetSubscriptionList(adminId,userId);
            if (lstSubscriptionDetails.Count == 0)
                return Request.CreateResponse(HttpStatusCode.OK, "");
            else
                return Request.CreateResponse(HttpStatusCode.OK, lstSubscriptionDetails);
        }
    }
}
