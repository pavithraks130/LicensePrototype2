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
        UserLicenseLogic logic = null;
        UserLicenseRequestLogic reqLogic = null;
       


        public LicenseController()
        {
            logic = new UserLicenseLogic();
            reqLogic = new UserLicenseRequestLogic();
        }

        [HttpPost]
        [Route("CreateUserLicence")]
        public HttpResponseMessage AddUserLicsen(UserLicesneDataMapping model)
        {
            var status = logic.CreateMultiUserLicense(model.LicenseDataList, model.UserList);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPost]
        [Route("RevokeUserLicence")]
        public HttpResponseMessage RemoveUserLicense(UserLicesneDataMapping model)
        {
            var status = logic.RevokeUserLicense(model.LicenseDataList, model.UserList);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpGet]
        [Route("GetLicenseRequestStatus/{userId}")]
        public IHttpActionResult GetLicensesRequestByUser(string userId)
        {
            var listlic = reqLogic.GetLicenseRequest(userId);
            return Ok(listlic);
        }

        [HttpGet]
        [Route("GetRequestedLicense/{adminId}")]
        public IHttpActionResult GetRequestedLicnses(string adminId)
        {
            var lstLicenseRequest = reqLogic.GetRequestList(adminId);
            return Ok(lstLicenseRequest);
        }
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


        [HttpGet]
        [Route("GetSubscriptionLicense/{userId}/{isFeatureRequired}")]
        public HttpResponseMessage GetUserSubscripedLicense(string userId, bool isFeatureRequired)
        {
            LicenseBO licBOLogic = new LicenseBO();
            licBOLogic.UserManager = UserManager;
            licBOLogic.RoleManager = RoleManager;
            var data = licBOLogic.GetUserLicenseSubscriptionDetails(userId, isFeatureRequired);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}
