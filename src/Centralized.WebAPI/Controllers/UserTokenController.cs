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

        /// <summary>
        /// GET Method. Get List of all User Token.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAll()
        {
            var listToken = logic.GetUsertokenList();
            return Ok(listToken);
        }

        /// <summary>
        /// POST Method. Create User Token for the specified Email
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
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

        /// <summary>
        /// POST Method. Verify the User Token with Email  during User Registration
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
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
