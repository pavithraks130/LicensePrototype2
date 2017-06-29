using System;
using System.Collections.Generic;
using System.Linq;
using License.Core.Model;
using License.Models;
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
        /// <summary>
        /// Gets the user list 
        /// </summary>
        /// <returns></returns>
        public ICollection<User> GetUsers()
        {
            List<User> usersList = new List<User>();
            var users = UserManager.Users.ToList();
            usersList = users.Select(u => AutoMapper.Mapper.Map<License.Models.User>(u)).ToList();
            return usersList;
        }

        /// <summary>
        ///  Create User record if the role is not specified then by default user is added with uper Admin Role. For Partial Admin the Role will be Admin and
        /// for Team member the Role will be TeamMember
        /// </summary>
        /// <param name="u"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
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
            AppUser user = AutoMapper.Mapper.Map<User, License.Core.Model.AppUser>(ur);
            IdentityResult result;
            Models.Role role = null;
            try
            {
                // Creation Role if the Role Doen't exist.
                if (RoleManager.FindByName(roleName) == null)
                {
                    RoleLogic rolelogic = new RoleLogic();
                    rolelogic.RoleManager = RoleManager;
                    bool isDefault = false;
                    if (roleName == "SuperAdmin" || roleName == "TeamMember")
                        isDefault = true;
                    rolelogic.CreateRole(new Models.Role() { Name = roleName, IsDefault = isDefault });
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
                    Models.Team team = new Models.Team();
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

        /// <summary>
        ///  Get user By user Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(string id)
        {
            var u = UserManager.FindById(id);
            var user = AutoMapper.Mapper.Map<License.Core.Model.AppUser, License.Models.User>(u);
            IList<string> roles = UserManager.GetRoles(user.UserId);
            user.Roles = roles;
            return user;
        }

        /// <summary>
        ///  Update User data based on the User Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public User UpdateUser(string id, User user)
        {
            var appuser = UserManager.FindById(id);
            appuser.FirstName = user.FirstName;
            appuser.LastName = user.LastName;
            appuser.PhoneNumber = user.PhoneNumber;
            var result = UserManager.Update(appuser);
            if (user.Roles != null && user.Roles.Count > 0)
            {
                var existingUserRoles = UserManager.GetRoles(id);
                var defaultRoles = RoleManager.Roles.Where(r => r.IsDefault == true).Select(r => r.Name).ToList();
                foreach (var role in defaultRoles)
                {
                    if (existingUserRoles.Contains(role))
                        if (!user.Roles.Contains(role))
                            user.Roles.Add(role);
                }
                result = UserManager.AddToRoles(id, user.Roles.Except(existingUserRoles).ToArray<string>());
                result = UserManager.RemoveFromRoles(id, existingUserRoles.Except(user.Roles).ToArray<string>());
            }
            if (!result.Succeeded)
            {
                foreach (string str in result.Errors)
                    ErrorMessage += str;
                return null;
            }           
            return AutoMapper.Mapper.Map<User>(appuser);
        }

        /// <summary>
        /// Delete user based on the User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUser(string id)
        {
            var user = UserManager.FindById(id);
            var result = UserManager.Delete(user);
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;
            return result.Succeeded;
        }

        /// <summary>
        /// Get user by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Models.User GetUserByEmail(string email)
        {
            var data = UserManager.FindByEmail<AppUser, string>(email);
            return AutoMapper.Mapper.Map<User>(data);
        }

        /// <summary>
        /// Update the Password based on the user Id and Old password  if the specified old password is wrong error message will be sent.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var result = UserManager.ChangePassword(userId, oldPassword, newPassword);
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;
            return result.Succeeded;
        }

        /// <summary>
        /// Authenticating user 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update the Available status  based on the userId
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="status"></param>
        public User UpdateLogOutStatus(string userid, bool status)
        {
            var user = UserManager.FindById(userid);
            user.IsActive = status;
            var result = UserManager.Update(user);
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;
            return AutoMapper.Mapper.Map<User>(user);
        }

        /// <summary>
        /// Creating claims Identity which will be used in Token creation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        public System.Security.Claims.ClaimsIdentity CreateClaimsIdentity(string userId, string authType)
        {
            var obj = UserManager.FindById(userId);
            return UserManager.CreateIdentity(obj, authType);

        }

    }
}
