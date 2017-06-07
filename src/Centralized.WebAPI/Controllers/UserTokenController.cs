using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;
using LicenseServer.DataModel;
using Centralized.WebAPI.Common;

namespace Centralized.WebAPI.Controllers
{
    /// <summary>
    /// Controller for User Token
    /// </summary>
    [Authorize]
    [RoutePrefix("api/UserToken")]
    public class UserTokenController : BaseController
    {
        private UserTokenLogic logic = null;

        /// <summary>
        /// Constructor for User Token
        /// </summary>
        public UserTokenController()
        {
            logic = new UserTokenLogic();
        }

        /// <summary>
        /// GET Method. Get List of all User Tokens.
        /// </summary>
        /// <returns>List of All User Tokens</returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAllUserTokens()
        {
            var userTokenList = logic.GetUsertokenList();
            return Ok(userTokenList);
        }

        /// <summary>
        /// POST Method. Create User Token for the specified Email
        /// </summary>
        /// <param name="token">User Token</param>
        /// <returns>On success returns created Token else returns error</returns>
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage CreateUserToken(UserToken token)
        {
            var userToken = logic.CreateUserToken(token);
            if (userToken != null)
                return Request.CreateResponse(HttpStatusCode.OK, userToken);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// POST Method. Verify the User Token with Email  during User Registration
        /// </summary>
        /// <param name="token">User Token</param>
        /// <returns>Status Indicating if User Token is verified</returns>
        [HttpPost]
        [Route("VerifyToken")]
        [AllowAnonymous]
        public HttpResponseMessage VerifyToken(UserToken token)
        {
            bool isTokenVerified = logic.VerifyUserToken(token);
            if (isTokenVerified)
                return Request.CreateResponse(HttpStatusCode.OK, Constants.Success);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}
