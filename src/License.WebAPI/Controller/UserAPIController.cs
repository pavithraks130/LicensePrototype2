using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using License.Core.Model;
using License.Logic.ServiceLogic;
using License.Model.Model;
using Microsoft.AspNet.Identity;

namespace License.WebAPI.Controller
{
    public class UserAPIController : BaseApiController
    {
        private UserLogic logic = null;

        public UserAPIController()
        {
            logic = new UserLogic();
        }

        public IHttpActionResult GetUsers()
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            return Ok(logic.GetUsers());
        }

        public HttpResponseMessage CreateUser(Registration user)
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            IdentityResult result = logic.CreateUser(user);
            if (result.Succeeded)
                return Request.CreateResponse(HttpStatusCode.Created, "Sucess");
            else
                return this.GetErrorResult(result);
        }

    }
}