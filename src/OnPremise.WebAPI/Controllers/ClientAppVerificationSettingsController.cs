using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License.Logic.DataLogic;
using License.Models;

namespace OnPremise.WebAPI.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/ClientAppVerificationSettings")]
    public class ClientAppVerificationSettingsController : BaseController
    {
        private ClientAppVerificationSettingsLogic _logic;

        public ClientAppVerificationSettingsController()
        {
            _logic = new ClientAppVerificationSettingsLogic();
        }

        [Route("Sync")]
        [HttpPost]
        public HttpResponseMessage SyncSettings(List<ClientAppVerificationSettings> settings)
        {
            _logic.UpdateAppSettings(settings);
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [Route("Verify")]
        [HttpPost]
        public HttpResponseMessage Verify(ClientAppVerificationSettings setting)
        {
            var result = _logic.ValidateKey(setting);
            if (!result)
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Not a valid applicatioin");
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
