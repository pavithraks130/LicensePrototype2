using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic.DataLogic;
using License.Models;

namespace Centralized.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/ClientAppVerificationSettings")]
    public class ClientApplicationSettingsController : BaseController
    {
        private ClientAppVerificationSettingsLogic _logic = null;
        public ClientApplicationSettingsController()
        {
            _logic = new ClientAppVerificationSettingsLogic();
        }

        [Route("All")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            var result = _logic.GetAll();
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage Create(ClientAppVerificationSettings settings)
        {
            var result = _logic.Create(settings);
            if (result == null)
                if (!String.IsNullOrEmpty(_logic.ErrorMessage))
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, _logic.ErrorMessage);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Specific application details not found");
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [AllowAnonymous]
        [Route("Verify")]
        [HttpPost]
        public HttpResponseMessage GetVerificationDetails(ClientAppVerificationSettings settings)
        {
            var result = _logic.ValidateKey(settings);
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Not a valid applicatioin");
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
