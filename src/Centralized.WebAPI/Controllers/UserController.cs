using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LicenseServer.Logic;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using LicenseServer.DataModel;

namespace Centralized.WebAPI.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : BaseController
    {
        UserLogic logic = null;
        public UserController()
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

        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage Create(User user)
        {
            Initialize();
            var userModel = logic.CreateUser(user);
            if (userModel != null)
                return Request.CreateResponse<User>(HttpStatusCode.OK, userModel);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetUsers()
        {
            Initialize();
            var userList = logic.GetUsers();
            return Ok(userList);
        }

        [HttpGet]
        [Route("User/{id}")]
        public IHttpActionResult GetUserById(string id)
        {
            Initialize();
            var user = logic.GetUserById(id);
            return Ok(user);
        }

        [HttpGet]
        [Route("UserByEmail/{email}")]
        public IHttpActionResult GetUserByEMail(string email)
        {
            Initialize();
            var user = logic.GetUserByEmail(email);
            return Ok(user);
        }

        [HttpPut]
        [Route("Update")]
        public HttpResponseMessage UpdateUser(User user)
        {
            bool status = logic.UpdateUser(user.UserId, user);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeleteUser(string id)
        {
            bool status = logic.DeleteUser(id);
            if (status)
                return Request.CreateResponse<string>("Deleted");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

    }
}