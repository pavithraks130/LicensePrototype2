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
    [Authorize]
    [RoutePrefix("api/UserToken")]
    public class UserTokenController : BaseController
    {
        private UserTokenLogic logic = null;

        public UserTokenController()
        {
            logic = new UserTokenLogic();
        }

        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAll()
        {
            var listToken = logic.GetUsertokenList();
            return Ok(listToken);
        }

        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateUserToken(UserToken t)
        {
            var token = logic.CreateUserToken(t);
            if (token != null)
                return Request.CreateResponse(HttpStatusCode.OK, token);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPost]
        [Route("VerifyToken")]
        [AllowAnonymous]
        public HttpResponseMessage VerifyToken(UserToken t)
        {
            bool status = logic.VerifyUserToken(t);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}
