using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using License.Core.Model;
using License.Logic.BusinessLogic;
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

        /// <summary>
        /// Get method. To Get all the User 
        /// </summary>
        /// <returns></returns>
        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            Initialize();
            return Ok(logic.GetUsers());
        }


        /// <summary>
        /// POST method. To create User 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Put Method. To Update the User Details based on the UserId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Method. Get the user By Userid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UserById/{id}")]
        public IHttpActionResult GetUserById(string id)
        {
            Initialize();
            var result = logic.GetUserById(id);
            return Ok(result);
        }

        /// <summary>
        /// Get Method. Get the user by EMail
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
        /// Delete Method. Delete user by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Post Method. Get Password Reset Token  for reseting password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// POST method. Update/Reset the password.
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
        /// PUT method. To update the User Status when User is Online or Offline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateActiveStatus")]
        [AllowAnonymous]
        public HttpResponseMessage UpdateActiveStatus(User model)
        {
            Initialize();
            logic.UpdateLogOutStatus(model.UserId, model.IsActive);
            UserLicenseLogic licLogic = new UserLicenseLogic();
            licLogic.RevokeTeamLicenseFromUser(model.UserId);
            if (!String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, "updated");
        }

        /// <summary>
        /// Pot method. ChangePassword the Password
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

        /// <summary>
        /// GET Method. Check for the concurent user 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("IsConcurrentUserLoggedIn")]
        public HttpResponseMessage IsConcurrentUserLoggedIn(ConcurrentUserLogin userLogin)
        {
            Initialize();
            LicenseBO licBO = new LicenseBO()
            {
                UserManager = UserManager
            };
            HttpStatusCode statusCode;
            var status = licBO.ValidateConcurrentUser(userLogin.TeamId, userLogin.UserId);
            if (status)
            {
                licBO.UpdateTeamLicenseToUser(userLogin.TeamId, userLogin.UserId);
                statusCode = HttpStatusCode.OK;
                userLogin.IsUserLoggedIn = true;
            }
            else
            {
                logic.UpdateLogOutStatus(userLogin.UserId, false);
                userLogin.IsUserLoggedIn = false;
                statusCode = HttpStatusCode.ExpectationFailed;
            }
            return Request.CreateResponse(statusCode, userLogin);
        }
    }
}