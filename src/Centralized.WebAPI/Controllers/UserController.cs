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

        private void Initialize()
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
        }

        /// <summary>
        /// POST Method. Create User record with Super Admin Role 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public HttpResponseMessage Create(Registration user)
        {
            Initialize();
            var userModel = logic.CreateUser(user, "SuperAdmin");
            if (userModel != null)
                return Request.CreateResponse<User>(HttpStatusCode.OK, userModel);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// POST Method. Gets Reset Token for the Forgot Password Functionality
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetResetToken")]
        [AllowAnonymous]
        public HttpResponseMessage GetPasswordResetToken(ForgotPassword model)
        {
            var user = UserManager.FindByEmail(model.Email);
            if (user == null)
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Invaalid Email Address");
            var token = UserManager.GeneratePasswordResetTokenAsync(user.UserId).Result;
            ForgotPasswordToken passwordToken = new ForgotPasswordToken()
            {
                UserId = user.UserId,
                Token = token
            };
            return Request.CreateResponse(HttpStatusCode.OK, passwordToken);
        }

        /// <summary>
        /// POST Method. Reset the Password with New password based on validating the Reset Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Put Method. Updating the User Status once user logIn or Log Out
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateActiveStatus")]
        [AllowAnonymous]
        public HttpResponseMessage UpdateActiveStatus(User model)
        {
            Initialize();
            logic.UpdateLogInStatus(model.UserId, model.IsActive);
            if (!String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, "updated");
        }

        /// <summary>
        /// Get Method. Get all the Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetUsers()
        {
            Initialize();
            var userList = logic.GetUsers();
            return Ok(userList);
        }

        /// <summary>
        /// GET Method. Get User by  user Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UserById/{id}")]
        public IHttpActionResult GetUserById(string id)
        {
            Initialize();
            var user = logic.GetUserById(id);
            return Ok(user);
        }

        /// <summary>
        /// GET Method. To Get User By EMail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UserByEmail/{email}")]
        public IHttpActionResult GetUserByEMail(string email)
        {
            Initialize();
            var user = logic.GetUserByEmail(email);
            return Ok(user);
        }

        /// <summary>
        /// PUT Method. Update the User Data based on the User Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update/{id}")]
        public HttpResponseMessage UpdateUser(string id, User user)
        {
            Initialize();
            bool status = logic.UpdateUser(id, user);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// Delete Method. Delete User by User ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeleteUser(string id)
        {
            Initialize();
            bool status = logic.DeleteUser(id);
            if (status)
                return Request.CreateResponse<string>("Deleted");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// PUT Method. Change Password of the user based on the User ID and Previous Password Validation
        /// which is sent in the CHangePassword Object
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ChangePassword/{userId}")]
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