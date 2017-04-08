using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using License.Core.Model;
using License.Logic.DataLogic;
using License.DataModel;
using Microsoft.AspNet.Identity;


namespace OnPremise.WebAPI.Controllers
{
    [RoutePrefix("api/User")]
    [Authorize]
    public class UserController : BaseController
    {
        private UserLogic logic = null;

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

        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            Initialize();
            return Ok(logic.GetUsers());
        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public HttpResponseMessage CreateUser(Registration user)
        {
            Initialize();
            var result = logic.CreateUser(user);
            if (result)
                return Request.CreateResponse(HttpStatusCode.Created, "Sucess");
            else
                return Request.CreateResponse<string>(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPut]
        [Route("Update/{id}")]
        public HttpResponseMessage UpdateRole(string id, User user)
        {
            Initialize();
            bool result = logic.UpdateUser(id, user);
            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfuly");
            else
                return Request.CreateResponse<string>(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpGet]
        [Route("UserById/{id}")]
        public IHttpActionResult GetUserById(string id)
        {
            Initialize();
            var result = logic.GetUserById(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("UserByEmail/{email}")]
        public IHttpActionResult GetUserByEMail(string email)
        {
            Initialize();
            var user = logic.GetUserByEmail(email);
            return Ok(user);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeleteUser(string id)
        {
            Initialize();
            var result = logic.DeleteUser(id);
            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfuly");
            else
                return Request.CreateResponse<string>(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPost]
        [Route("GetResetToken")]
        [AllowAnonymous]
        public HttpResponseMessage GetPasswordResetToken(ForgotPassword model)
        {
            var user = UserManager.FindByEmail(model.Email);
            var token = UserManager.GeneratePasswordResetTokenAsync(user.UserId).Result;
            ForgotPasswordToken passwordToken = new ForgotPasswordToken();
            passwordToken.UserId = user.UserId;
            passwordToken.Token = token;
            return Request.CreateResponse(HttpStatusCode.OK, passwordToken);
        }

        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public HttpResponseMessage ResetuserPassword(ResetPassword model)
        {
            Initialize();
            string token = string.Empty;
            if (string.IsNullOrEmpty(model.Token))
                token = UserManager.GeneratePasswordResetToken(model.UserId);
            else
                token = model.Token;
            var result = UserManager.ResetPassword(model.UserId, token, model.Password);
            var user = logic.GetUserById(model.UserId);
            if (result.Succeeded)
                return Request.CreateResponse(HttpStatusCode.OK, user);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, string.Join(",", result.Errors.ToArray()).Substring(1));
        }

        [HttpPut]
        [Route("UpdateActiveStatus")]
        public HttpResponseMessage UpdateActiveStatus(User model)
        {
            Initialize();
            logic.UpdateLogOutStatus(model.UserId, model.IsActive);
            if (!String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, "updated");
        }

        [HttpPut]
        [Route("UpdatePassword/{userId}")]
        public HttpResponseMessage UpdatePassword(string userId, ChangePassword model)
        {
            Initialize();
            var status = logic.ChangePassword(userId, model.CurrentPassword, model.NewPassword);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}