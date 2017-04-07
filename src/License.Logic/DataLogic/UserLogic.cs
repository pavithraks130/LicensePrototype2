using System;
using System.Collections.Generic;
using System.Linq;
using License.Core.Model;
using License.DataModel;
using Microsoft.AspNet.Identity;

namespace License.Logic.DataLogic
{
    public class UserLogic : BaseLogic
    {
        public ICollection<User> GetUsers()
        {
            List<User> usersList = new List<User>();
            var users = UserManager.Users.ToList();
            foreach (var u in users)
            {
                User temp = AutoMapper.Mapper.Map<License.Core.Model.AppUser, License.DataModel.User>(u);
                usersList.Add(temp);
            }
            return usersList;
        }

        public bool CreateUser(Registration u, string roleName = "SuperAdmin")
        {
            User ur = new User();
            ur.FirstName = u.FirstName;
            ur.LastName = u.LastName;
            ur.Email = u.Email;
            ur.PhoneNumber = u.PhoneNumber;
            ur.UserName = u.Email;
            ur.ServerUserId = u.ServerUserId;

            AppUser user = AutoMapper.Mapper.Map<License.DataModel.User, License.Core.Model.AppUser>(ur);
            IdentityResult result;
            try
            {
                if (RoleManager.FindByName(roleName) == null)
                {
                    RoleLogic rolelogic = new RoleLogic();
                    rolelogic.RoleManager = RoleManager;
                    result = rolelogic.CreateRole(new DataModel.Role() { Name = roleName });
                }
                string userId = String.Empty;
                var usr = UserManager.FindByEmail(u.Email);

                if (usr == null)
                {
                    result = UserManager.Create(user, u.Password);
                    userId = user.Id;
                }
                else
                    userId = usr.Id;

                if (!UserManager.IsInRole(userId, roleName))
                    result = UserManager.AddToRole(userId, roleName);
                else
                    result = new IdentityResult(new string[] { });
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

        public User GetUserById(string id)
        {
            var u = UserManager.FindById(id);
            var user = AutoMapper.Mapper.Map<License.Core.Model.AppUser, License.DataModel.User>(u);
            IList<string> roles = UserManager.GetRoles(user.UserId);
            user.Roles = roles;
            return user;
        }

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

        public bool DeleteUser(string id)
        {
            var user = UserManager.FindById(id);
            var result = UserManager.Delete(user);
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;
            return result.Succeeded;
        }

        public string CreateResetPasswordToken(string userId)
        {
            return UserManager.GeneratePasswordResetToken(userId);
        }

        public bool ResetPassword(string userId, string token, string password)
        {
            var result = UserManager.ResetPassword(userId, token, password);
            if (!result.Succeeded)
                foreach (string str in result.Errors)
                    ErrorMessage += str;
            return result.Succeeded;
        }

        public DataModel.User GetUserByEmail(string email)
        {
            var data = UserManager.FindByEmail<AppUser, string>(email);
            return AutoMapper.Mapper.Map<User>(data);
        }

        public User ForgotPassword(string email)
        {
            AppUser user = UserManager.FindByEmail(email);
            return AutoMapper.Mapper.Map<Core.Model.AppUser, User>(user);
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var result = UserManager.ChangePassword(userId, oldPassword, newPassword);
            return result.Succeeded;
        }

        public User AuthenticateUser(string userName, string password)
        {
            AppUser user = UserManager.Find(userName, password);
            if (user != null)
            {
                user.IsActive = true;
                UserManager.Update(user);
                User userObj = AutoMapper.Mapper.Map<Core.Model.AppUser, User>(user);
                if (userObj == null) return null;
                IList<string> roles = UserManager.GetRoles(user.Id);
                userObj.Roles = roles;
                return userObj;
            }
            return null;
        }
        
        public void UpdateLogOutStatus(string userid, bool status)
        {
            var user = UserManager.FindById(userid);
            user.IsActive = status;
            UserManager.Update(user);
        }
        public System.Security.Claims.ClaimsIdentity CreateClaimsIdentity(string userId, string authType)
        {
            var obj = UserManager.FindById(userId);
            //Appuser user = AutoMapper.Mapper.Map<Appuser>(obj);
            return UserManager.CreateIdentity(obj, authType);

        }

    }
}
