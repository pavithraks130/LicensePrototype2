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
    [Authorize]
    [RoutePrefix("api/Role")]
    public class RolesController : BaseController
    {
        private RoleLogic logic = null;

        public RolesController()
        {
            logic = new RoleLogic();
        }

        public void Initialize()
        {
            logic.RoleManager = RoleManager;
        }

        /// <summary>
        /// Get Roles List 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetRoles()
        {
            Initialize();
            var roleList = logic.GetRoles();
            return Ok(roleList);
        }

        /// <summary>
        /// Get Method, Get the Role by Role ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public HttpResponseMessage GetRoleById(string id)
        {
            Initialize();
            var role = logic.GetRoleById(id);
            if (String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, role);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Post Method, Create the Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage Create(Role role)
        {
            Initialize();
            var roleObj = logic.CreateRole(role);
            if (String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, roleObj);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPut]
        [Route("Update/{roleId}")]
        public HttpResponseMessage Update(string roleId, Role role)
        {
            Initialize();
            var roleObj = logic.UpdateRole(roleId, role);
            if (String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, roleObj);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpDelete]
        [Route("Delete/{roleId}")]
        public HttpResponseMessage Delete(string roleId)
        {
            Initialize();
            var roleObj = logic.DeleteRole(roleId);
            if (String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, roleObj);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}
