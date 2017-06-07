using System;
using System.Collections.Generic;
using System.Linq;
using License.Core.Model;
using License.DataModel;
using Microsoft.AspNet.Identity;

namespace License.Logic.DataLogic
{
    /// <summary>
    /// History:
    ///      Created By: 
    ///      Created Date:
    ///      Purpose : 1. Functionality related to the CRUD functionality of the User  using Identity User manager .
    /// </summary>
    public class UserLogic : BaseLogic
    {
        // Gets the user list 
        public ICollection<User> GetUsers()
        {
            List<User> usersList = new List<User>();
            var users = UserManager.Users.ToList();
            usersList = users.Select(u => AutoMapper.Mapper.Map<License.DataModel.User>(u)).ToList();
            return usersList;
        }

        // Create User record if the role is not specified then by default user is added with uper Admin Role. For Partial Admin the Role will be Admin and
        // for Team member the Role will be TeamMember
        public bool CreateUser(Registration u, string roleName = "SuperAdmin")
        {
            User ur = new User()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                UserName = u.Email,
                ServerUserId = u.ServerUserId
            };
            AppUser user = AutoMapper.Mapper.Map<License.DataModel.User, License.Core.Model.AppUser>(ur);
            IdentityResult result;
            try
            {
                // Creation Role if the Role Doen't exist.
                if (RoleManager.FindByName(roleName) == null)
                {
                    RoleLogic rolelogic = new RoleLogic();
                    rolelogic.RoleManager = RoleManager;
                    result = rolelogic.CreateRole(new DataModel.Role() { Name = roleName });
                }

                // Check if user Record is Already created. If not exist then create User using Identity User manager
                string userId = String.Empty;
                var usr = UserManager.FindByEmail(u.Email);

                if (usr == null)
                {
                    result = UserManager.Create(user, u.Password);
                    userId = user.Id;
                }
                else
                    userId = usr.Id;

                // Check if User mapped to the specified Role  if not add role to user.
                if (!UserManager.IsInRole(userId, roleName))
                    result = UserManager.AddToRole(userId, roleName);
                else
                    result = new IdentityResult(new string[] { });
                // Once User Creataed and if if user is Super Admin then create the default team  and set user as the admin to team
                if (roleName == "SuperAdmin" && !String.IsNullOrEmpty(userId))
                {
                    TeamLogic teamLogic = new TeamLogic();
                    teamLogic.UserManager = UserManager;
                    DataModel.Team team = new DataModel.Team();
                    team.AdminId = userId;
                    team.IsDefaultTeam = true;
                    team.Name = System.Configuration.ConfigurationManager.AppSettings.Get("DefaultTeamName");
                    teamLogic.CreateTeam(team);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                result = new IdentityResult(new string[] { ex.Message });
            }
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;

            return result.Succeeded;
        }

        // Get user By user Id
        public User GetUserById(string id)
        {
            var u = UserManager.FindById(id);
            var user = AutoMapper.Mapper.Map<License.Core.Model.AppUser, License.DataModel.User>(u);
            IList<string> roles = UserManager.GetRoles(user.UserId);
            user.Roles = roles;
            return user;
        }

        // Update User data based on the User Id
        public bool UpdateUser(string id, User user)
        {
            var appuser = UserManager.FindById(id);
            appuser.FirstName = user.FirstName;
            appuser.LastName = user.LastName;
            appuser.PhoneNumber = user.PhoneNumber;
            var result = UserManager.Update(appuser);
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;
            return result.Succeeded;
        }

        // Delete user based on the User Id
        public bool DeleteUser(string id)
        {
            var user = UserManager.FindById(id);
            var result = UserManager.Delete(user);
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;
            return result.Succeeded;
        }

        //public string CreateResetPasswordToken(string userId)
        //{
        //    return UserManager.GeneratePasswordResetToken(userId);
        //}

        //public bool ResetPassword(string userId, string token, string password)
        //{
        //    var result = UserManager.ResetPassword(userId, token, password);
        //    if (!result.Succeeded)
        //        foreach (string str in result.Errors)
        //            ErrorMessage += str;
        //    return result.Succeeded;
        //}

        // Get user by Emaial
        public DataModel.User GetUserByEmail(string email)
        {
            var data = UserManager.FindByEmail<AppUser, string>(email);
            return AutoMapper.Mapper.Map<User>(data);
        }

        //public User ForgotPassword(string email)
        //{
        //    AppUser user = UserManager.FindByEmail(email);
        //    return AutoMapper.Mapper.Map<Core.Model.AppUser, User>(user);
        //}

        // Update the Password based on the user Id and Old password  if the specified old password is wrong error message will be sent.
        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var result = UserManager.ChangePassword(userId, oldPassword, newPassword);
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;
            return result.Succeeded;
        }

        // Authenticating user 
        public User AuthenticateUser(string userName, string password)
        {
            AppUser user = UserManager.Find(userName, password);
            if (user != null)
            {
                user.IsActive = true;
                var result = UserManager.Update(user);
                if (!result.Succeeded)
                {
                    foreach (string str in result.Errors)
                        ErrorMessage += str;
                    return null;
                }
                User userObj = AutoMapper.Mapper.Map<Core.Model.AppUser, User>(user);
                if (userObj == null) return null;
                IList<string> roles = UserManager.GetRoles(user.Id);
                userObj.Roles = roles;
                return userObj;
            }
            return null;
        }

        // Update the Available status  based on the userId
        public void UpdateLogOutStatus(string userid, bool status)
        {
            var user = UserManager.FindById(userid);
            user.IsActive = status;
            var result = UserManager.Update(user);
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;
        }

        // Creating claims Identity which will be used in Token creation
        public System.Security.Claims.ClaimsIdentity CreateClaimsIdentity(string userId, string authType)
        {
            var obj = UserManager.FindById(userId);
            return UserManager.CreateIdentity(obj, authType);

        }

    }
}
