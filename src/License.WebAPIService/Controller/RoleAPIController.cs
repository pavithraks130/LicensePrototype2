using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using License.Logic.ServiceLogic;
using License.Model.Model;
using Microsoft.AspNet.Identity;

namespace License.WebAPIService.Controller
{
    [RoutePrefix("api/Role")]
    public class RoleAPIController :BaseApiController
    {
        private RoleLogic logic = null;

        public RoleAPIController()
        {
            logic = new RoleLogic();
        }

        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            return Ok(logic.GetRoles());
        }

        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage CreateRole(Role r)
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            IdentityResult result = logic.CreateRole(r);
            if (result.Succeeded)
                return Request.CreateResponse(HttpStatusCode.Created, "Sucess");
            else
                return this.GetErrorResult(result);
        }
    }
}