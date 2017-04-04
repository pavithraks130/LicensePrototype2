using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LicenseServer.Logic;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using LicenseServer.DataModel;
using LicenseServer.Core.Manager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Centralized.WebAPI.Controllers
{
    [RoutePrefix("api/user")]
    [Authorize]
    public class UserController : BaseController
    {
        UserLogic logic = null;
        public UserController()
        {
            logic = new UserLogic();
        }

        private LicUserManager _userManager = null;
        public LicUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.Current.GetOwinContext().Get<LicUserManager>();
                return _userManager;
            }
        }

        private LicRoleManager _roleManager = null;
        public LicRoleManager RoleManager
        {
            get
            {
                if (_roleManager == null)
                    _roleManager = Request.GetOwinContext().GetUserManager<LicRoleManager>();
                return _roleManager;
            }
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
        [AllowAnonymous]
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
        [Route("GetResetToken/{email}")]
        public HttpResponseMessage GetPasswordResetToken(string email)
        {
            var user = UserManager.FindByEmail(email);
            var token = UserManager.GeneratePasswordResetTokenAsync(user.UserId).Result;
            ForgotPasswordToken passwordToken = new ForgotPasswordToken();
            passwordToken.UserId = user.UserId;
            passwordToken.Token = token;
            return Request.CreateResponse(HttpStatusCode.OK, passwordToken);
        }

        [HttpPost]
        [Route("ResetPassword")]
        public HttpResponseMessage ResetuserPassword(ResetPassword model)
        {
            string token = string.Empty;
            if (string.IsNullOrEmpty(model.Token))
                token = UserManager.GeneratePasswordResetToken(model.UserId);
            var result = UserManager.ResetPassword(model.UserId, token, model.Password);
            var user = logic.GetUserById(model.UserId);
            if (result.Succeeded)
                return Request.CreateResponse(HttpStatusCode.OK, user);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, string.Join(",", result.Errors.ToArray()).Substring(1));
        }

        [HttpPost]
        [Route("UpdateActiveStatus")]
        public HttpResponseMessage UpdateActiveStatus(User model)
        {
            logic.UpdateLogInStatus(model.UserId, model.IsActive);
            if (!String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, "updated");
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
        [Route("UserById/{id}")]
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
        [Route("Update/{id}")]
        public HttpResponseMessage UpdateUser(string id, User user)
        {
            bool status = logic.UpdateUser(id, user);
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

        [HttpPost]
        [Route("UpdatePassword/{userId}")]
        public HttpResponseMessage UpdatePassword(string userId, ChangePassword model)
        {
            var status = logic.ChangePassword(model.UserId, model.CurrentPassword, model.NewPassword);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

    }
}