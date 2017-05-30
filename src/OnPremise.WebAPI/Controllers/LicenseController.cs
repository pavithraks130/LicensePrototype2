using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.DataModel;
using License.Logic.DataLogic;
using License.Logic.BusinessLogic;

namespace OnPremise.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/License")]
    public class LicenseController : BaseController
    {
        UserLicenseLogic userLicenselogic = null;
        TeamLicenseLogic teamLicenselogic = null;
        UserLicenseRequestLogic reqLogic = null;

        public LicenseController()
        {
            userLicenselogic = new UserLicenseLogic();
            teamLicenselogic = new TeamLicenseLogic();
            reqLogic = new UserLicenseRequestLogic();
        }

        /// <summary>
        /// Post method. Map the License to the User. The license will be fetched based on the subscription and Product Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateUserLicence")]
        public HttpResponseMessage AddUserLicense(UserLicenseDataMapping model)
        {
            var status = userLicenselogic.CreateMultiUserLicense(model);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userLicenselogic.ErrorMessage);
        }

        /// <summary>
        /// Post method. Map the License to the Team. The license will be fetched based on the subscription and Product Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateTeamLicence")]
        public HttpResponseMessage AddTeamLicense(TeamLicenseDataMapping model)
        {
            var status = teamLicenselogic.CreateMultipleTeamLicense(model);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userLicenselogic.ErrorMessage);
        }

        /// <summary>
        /// Post Method. To remove the Mapped License from User.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RevokeUserLicence")]
        public HttpResponseMessage RemoveUserLicense(UserLicenseDataMapping model)
        {
            var status = userLicenselogic.RevokeUserLicense(model);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userLicenselogic.ErrorMessage);
        }

        /// <summary>
        /// Get Method. To get all the Requested license for the User based on UserId to check the status of the license Request.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLicenseRequestStatus/{userId}")]
        public IHttpActionResult GetLicensesRequestByUser(string userId)
        {
            var listlic = reqLogic.GetLicenseRequest(userId);
            return Ok(listlic);
        }

        /// <summary>
        /// Get Method. To Get the License Request of all the team members irrespective of the Team based on the admin ID.
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllRequestedLicense/{adminId}")]
        public IHttpActionResult GetRequestedLicnses(string adminId)
        {
            var lstLicenseRequest = reqLogic.GetAllRequestList(adminId);
            return Ok(lstLicenseRequest);
        }

        /// <summary>
        /// Method to get the License Request for the Approval or the Rejection based on the TeamId. Here 
        /// Perticular Team Member request will be fetched based on the Team Id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRequestedLicenseByTeam/{teamId}")]
        public IHttpActionResult GetRequestedLicnses(int teamId)
        {
            var lstLicenseRequest = reqLogic.GetRequestListByTeam(teamId);
            return Ok(lstLicenseRequest);
        }


        /// <summary>
        /// Post Method. To update the approve or Rejection status of the License Request. 
        /// Multiple request will be updated. Input to this Service is List of UserLicenseRequest Object which contains 
        /// </summary>
        /// <param name="licReqList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ApproveRejectLicense")]
        public HttpResponseMessage ApproveLicense(List<UserLicenseRequest> licReqList)
        {
            LicenseBO licBOLogic = new LicenseBO();
            licBOLogic.UserManager = UserManager;
            licBOLogic.RoleManager = RoleManager;
            licBOLogic.ApproveOrRejectLicense(licReqList);
            if (String.IsNullOrEmpty(licBOLogic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, licBOLogic.ErrorMessage);
        }

        /// <summary>
        /// Post Method. Used for the License Request. Multiple License will be requested
        /// </summary>
        /// <param name="licReqList"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("RequestLicense")]
        public HttpResponseMessage RequestLicense(List<UserLicenseRequest> licReqList)
        {
            reqLogic.Create(licReqList);
            if (String.IsNullOrEmpty(reqLogic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, reqLogic.ErrorMessage);
        }

        /// <summary>
        /// Get Method.Used to fetch the UserSubscribed License List with Features based on the userId 
        /// Irrespective of the Team
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isFeatureRequired"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSubscriptionLicense/{userId}/{isFeatureRequired}")]
        public HttpResponseMessage GetUserSubscripedLicense(string userId, bool isFeatureRequired)
        {
            LicenseBO licBOLogic = new LicenseBO();
            licBOLogic.UserManager = UserManager;
            licBOLogic.RoleManager = RoleManager;
            FetchUserSubscription model = new FetchUserSubscription() { UserId = userId, IsFeatureRequired = isFeatureRequired };
            var data = licBOLogic.GetUserLicenseSubscriptionDetails(model);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Post Method. Used to fetch the User Subscribed License List based on the Team Id and User Id.
        /// Features will be fetched based on the user Requirement. The input to the service is FetchUserSubscription
        /// which contains UserId, TeamId and IsFeatureRequired Property.
        /// This request can be used to fetch the User License details based on the Teama Id ad User Id.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSubscriptionLicenseByTeam")]
        public HttpResponseMessage GetUserSubscripedLicense(FetchUserSubscription model)
        {
            LicenseBO licBOLogic = new LicenseBO();
            licBOLogic.UserManager = UserManager;
            licBOLogic.RoleManager = RoleManager;
            var data = licBOLogic.GetUserLicenseSubscriptionDetails(model);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Post Method. Used to fetch the Team Subscribed License List based on the Team Id .
        /// Features will be fetched based on the user Requirement. The input to the service is FetchUserSubscription
        /// which contains UserId, TeamId and IsFeatureRequired Property.
        /// This request can be used to fetch the User License details based on the Teama Id .
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSubscriptionLicenseByTeamId/{teamId}")]
        public HttpResponseMessage GetTeamSubscritionLicense(int teamId)
        {
            TeamBO teamBOLogic = new TeamBO();
            var data = teamBOLogic.GetTeamLicenseProductByTeamId(teamId);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [HttpPost]
        [Route("Delete")]
        public HttpResponseMessage DeleteTeamLicenses(DeleteTeamDetails data)
        {
            TeamBO teamBOLogic = new TeamBO();
            var status = teamBOLogic.DeleteTeamLicense(data.productIdList, data.TeamId);
            if (status)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, teamBOLogic.ErrorMessage);
        }
    }
}
