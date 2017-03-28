using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using License.Logic.ServiceLogic;
using License.Model;
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

        [Route("Update/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateRole(string id, Role role)
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            IdentityResult result = logic.UpdateRole(role);
            if (result.Succeeded)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfuly");
            else
                return this.GetErrorResult(result);
        }

        [Route("Get/{id}")]
        [HttpGet]
        public IHttpActionResult GetRoleById(string id)
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            var result = logic.GetRoleById(id);
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteRole(string id)
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            IdentityResult result = logic.DeleteRole(id);
            if (result.Succeeded)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfuly");
            else
                return this.GetErrorResult(result);
        }
    }
}