using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using License.Core.Model;
using License.Model;
using Microsoft.AspNet.Identity;

namespace License.Logic.ServiceLogic
{
    public class UserLogic : BaseLogic
    {
        public ICollection<User> GetUsers()
        {
            List<User> usersList = new List<User>();
            var users = UserManager.Users.ToList();
            foreach (var u in users)
            {
                User temp = AutoMapper.Mapper.Map<License.Core.Model.AppUser, License.Model.User>(u);
                usersList.Add(temp);
            }
            return usersList;
        }

        public IdentityResult CreateUser(Registration u, string roleName = "Admin")
        {
            User ur = new User();
            ur.FirstName = u.FirstName;
            ur.LastName = u.LastName;
            ur.Email = u.Email;
            ur.PhoneNumber = u.PhoneNumber;
            ur.UserName = u.Email;
            ur.ServerUserId = u.ServerUserId;
            //if (t == null)
            //    t = logic.CreateTeam(new Model.Model.Organization() { Name = u.OrganizationName });
            //ur.OrganizationId = t.Id;
            AppUser user = AutoMapper.Mapper.Map<License.Model.User, License.Core.Model.AppUser>(ur);
            IdentityResult result;
            try
            {
                result = UserManager.Create(user, u.Password);
                if (RoleManager.FindByName(roleName) == null)
                {
                    RoleLogic rolelogic = new RoleLogic();
                    rolelogic.RoleManager = RoleManager;
                    result = rolelogic.CreateRole(new Model.Role() { Name = roleName });
                }
                var roleId = RoleManager.FindByName(roleName).Id;
                UserManager.AddToRole(user.Id, roleName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public User GetUserById(string id)
        {
            var u = UserManager.FindById(id);
            var user = AutoMapper.Mapper.Map<License.Core.Model.AppUser, License.Model.User>(u);
            return user;
        }

        public IdentityResult UpdateUser(string id, User user)
        {
            var appuser = UserManager.FindById(id);
            appuser.FirstName = user.FirstName;
            appuser.LastName = user.LastName;
            appuser.Email = user.Email;
            appuser.PhoneNumber = user.PhoneNumber;
            return UserManager.Update(appuser);
        }

        public IdentityResult DeleteUser(string id)
        {
            var user = UserManager.FindById(id);
            return UserManager.Delete(user);
        }

        public IdentityResult ResetPassword(string userId, string token, string password)
        {
            return UserManager.ResetPassword(userId, token, password);
        }

        public bool GetUserByEmail(string email)
        {
            return UserManager.FindByEmail<AppUser, string>(email) != null;
        }

        public User ForgotPassword(string email)
        {
            AppUser user = UserManager.FindByEmail(email);
            return AutoMapper.Mapper.Map<Core.Model.AppUser, User>(user);
        }

        public AppUser AutheticateUser(string userName, string password)
        {
            AppUser user = UserManager.Find(userName, password);
            return user;
        }

        public User GetUserDataByAppuser(AppUser user)
        {
            User userObj = AutoMapper.Mapper.Map<Core.Model.AppUser, User>(user);
            IList<string> roles = UserManager.GetRoles(user.Id);
            userObj.Roles = roles;
            return userObj;
        }
    }
}
