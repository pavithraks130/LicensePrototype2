using LicenseServer.DataModel;
using LicenseServer.Logic;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common = Centralized.WebAPI.Common;

namespace Centralized.WebAPI.Controllers
{
    /// <summary>
    /// Controller for User
    /// </summary>
    [RoutePrefix("api/user")]
    [Authorize]
    public class UserController : BaseController
    {
        UserLogic userLogic = null;

        /// <summary>
        /// Constructor for User Controller
        /// </summary>
        public UserController()
        {
            userLogic = new UserLogic();
        }

        private void Initialize()
        {
            if (userLogic.UserManager == null)
                userLogic.UserManager = UserManager;
            if (userLogic.RoleManager == null)
                userLogic.RoleManager = RoleManager;
        }

        /// <summary>
        /// POST Method. Create User record with Super Admin Role 
        /// </summary>
        /// <param name="user">User Registration</param>
        /// <returns>On success returns created User else returns error</returns>
        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public HttpResponseMessage Create(Registration user)
        {
            Initialize();
            var createdUser = userLogic.CreateUser(user, "SuperAdmin");
            if (createdUser != null)
                return Request.CreateResponse<User>(HttpStatusCode.OK, createdUser);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userLogic.ErrorMessage);
        }

        /// <summary>
        /// POST Method. Gets Reset Token for the Forgot Password Functionality
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <returns>Password Reset Token if Successful</returns>
        [HttpPost]
        [Route("GetResetToken")]
        [AllowAnonymous]
        public HttpResponseMessage GetPasswordResetToken(ForgotPassword userName)
        {
            var user = UserManager.FindByEmail(userName.Email);
            if (user == null)
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Invalid Email Address");
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
        /// <param name="resetPasswordModel">Reset Password Object</param>
        /// <returns>Status Indicating Password Change</returns>
        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public HttpResponseMessage ResetUserPassword(ResetPassword resetPasswordModel)
        {
            Initialize();
            string token = string.Empty;
            if (string.IsNullOrEmpty(resetPasswordModel.Token))
                token = UserManager.GeneratePasswordResetToken(resetPasswordModel.UserId);
            else
                token = resetPasswordModel.Token;
            var result = UserManager.ResetPassword(resetPasswordModel.UserId, token, resetPasswordModel.Password);
            var user = userLogic.GetUserById(resetPasswordModel.UserId);
            if (result.Succeeded)
                return Request.CreateResponse(HttpStatusCode.OK, user);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, string.Join(",", result.Errors.ToArray()).Substring(1));
        }

        /// <summary>
        /// Put Method. Updating the User Status once user logIn or Log Out
        /// </summary>
        /// <param name="userModel">User Object</param>
        /// <returns>Status indicating if User is Active</returns>
        [HttpPut]
        [Route("UpdateActiveStatus")]
        [AllowAnonymous]
        public HttpResponseMessage UpdateActiveStatus(User userModel)
        {
            Initialize();
            userLogic.UpdateLogInStatus(userModel.UserId, userModel.IsActive);
            if (!String.IsNullOrEmpty(userLogic.ErrorMessage))
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userLogic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, Common.Constants.Updated);
        }

        /// <summary>
        /// Get Method. Gets all the Users
        /// </summary>
        /// <returns>All Users List</returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAllUsers()
        {
            Initialize();
            var userList = userLogic.GetUsers();
            return Ok(userList);
        }

        /// <summary>
        /// GET Method. Get User by  user Id
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User if Exists</returns>
        [HttpGet]
        [Route("UserById/{id}")]
        public IHttpActionResult GetUserById(string id)
        {
            Initialize();
            var user = userLogic.GetUserById(id);
            return Ok(user);
        }

        /// <summary>
        /// GET Method. To Get User By EMail
        /// </summary>
        /// <param name="email">Email ID</param>
        /// <returns>User if Exists</returns>
        [HttpGet]
        [Route("UserByEmail/{email}")]
        public IHttpActionResult GetUserByEMail(string email)
        {
            Initialize();
            var user = userLogic.GetUserByEmail(email);
            return Ok(user);
        }

        /// <summary>
        /// PUT Method. Update the User Data based on the User Id
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="user">User to Update</param>
        /// <returns>Status indicating if User is updated</returns>
        [HttpPut]
        [Route("Update/{id}")]
        public HttpResponseMessage UpdateUser(string id, User user)
        {
            Initialize();
            bool status = userLogic.UpdateUser(id, user);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, Common.Constants.Updated);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userLogic.ErrorMessage);
        }

        /// <summary>
        /// Delete Method. Delete User by User ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Status indicating whther User is Deleted</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeleteUser(string id)
        {
            Initialize();
            bool status = userLogic.DeleteUser(id);
            if (status)
                return Request.CreateResponse<string>(Common.Constants.Deleted);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userLogic.ErrorMessage);
        }

        /// <summary>
        /// PUT Method. Change Password of the user based on the User ID and Previous Password Validation
        /// which is sent in the CHangePassword Object
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="passwordUpdateModel">Password Update Model</param>
        /// <returns>Status Indicating if Password is Updated</returns>
        [HttpPut]
        [Route("ChangePassword/{userId}")]
        public HttpResponseMessage UpdatePassword(string userId, ChangePassword passwordUpdateModel)
        {
            Initialize();
            var status = userLogic.ChangePassword(userId, passwordUpdateModel.CurrentPassword, passwordUpdateModel.NewPassword);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, Common.Constants.Updated);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, userLogic.ErrorMessage);
        }

    }
}