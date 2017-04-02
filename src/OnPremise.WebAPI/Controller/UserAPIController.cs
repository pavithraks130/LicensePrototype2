using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using License.Core.Model;
using License.Logic.ServiceLogic;
using License.Model;
using Microsoft.AspNet.Identity;

namespace OnPremise.WebAPI.Controller
{
    [RoutePrefix("api/User")]
    public class UserAPIController : BaseApiController
    {
        private UserLogic logic = null;

        public UserAPIController()
        {
            logic = new UserLogic();
        }

        public void Initialize()
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
        }

        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            Initialize();
            return Ok(logic.GetUsers());
        }

        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage CreateUser(Registration user)
        {
            Initialize();
             var result = logic.CreateUser(user);
            if (result)
                return Request.CreateResponse(HttpStatusCode.Created, "Sucess");
            else
                return Request.CreateResponse<string>(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
      
        [Route("Update/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateRole(string id, User user)
        {
            Initialize();
            bool result = logic.UpdateUser(id, user);
            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfuly");
            else
                return Request.CreateResponse<string>(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [Route("Get/{id}")]
        [HttpGet]
        public IHttpActionResult GetUserById(string id)
        {
            Initialize();
             var result = logic.GetUserById(id);
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(string id)
        {
            Initialize();
            var result = logic.DeleteUser(id);
            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfuly");
            else
                return Request.CreateResponse<string>(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}