using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;
using Microsoft.AspNet.Identity;
using LicenseServer.Core.DbContext;
using LicenseServer.Core.Manager;
using Microsoft.AspNet.Identity.EntityFramework;
using LicenseServer.Core.Model;

namespace LicenseServer.Logic
{
    public class UserLogic : BaseLogic
    {
        private LicUserManager UserManager;
        private AppDbContext _context;

        public UserLogic()
        {
            _context = AppDbContext.Create();
            UserManager = new LicUserManager(new UserStore<Appuser>(_context));
        }

        public ICollection<User> GetUsers()
        {
            List<User> usersList = new List<User>();
            var users = UserManager.Users.ToList();
            foreach (var u in users)
            {
                User temp = AutoMapper.Mapper.Map<LicenseServer.Core.Model.Appuser, User>(u);
                usersList.Add(temp);
            }
            return usersList;
        }

        public string CreateUser(Registration u, string roleName = "Admin")
        {
            User ur = new User();
            ur.FirstName = u.FirstName;
            ur.LastName = u.LastName;
            ur.Email = u.Email;
            ur.PhoneNumber = u.PhoneNumber;
            ur.UserName = u.Email;
            var teamName = u.OrganizationName;
            OrganizationLogic logic = new OrganizationLogic();
            DataModel.Organization t = logic.GetTeamByName(teamName);
            if (t == null)
                t = logic.CreateTeam(new DataModel.Organization() { Name = u.OrganizationName });
            ur.OrganizationId = t.Id;
            LicenseServer.Core.Model.Appuser user = AutoMapper.Mapper.Map<User, LicenseServer.Core.Model.Appuser>(ur);
            IdentityResult result;
            try
            {
                RoleLogic rolelogic = new RoleLogic();
                result = UserManager.Create(user, u.Password);
                var roleObj = rolelogic.GetRoleByName(roleName);
                if (roleObj == null)
                    result = rolelogic.CreateRole(new Role() { Name = roleName });
                UserManager.AddToRole(user.Id, roleName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user.Id;
        }

        public User GetUserById(string id)
        {
            var u = UserManager.FindById(id);
            var user = AutoMapper.Mapper.Map<LicenseServer.Core.Model.Appuser, User>(u);
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
            return UserManager.FindByEmail<Core.Model.Appuser, string>(email) != null;
        }

        public User ForgotPassword(string email)
        {
            Core.Model.Appuser user = UserManager.FindByEmail(email);
            return AutoMapper.Mapper.Map<Core.Model.Appuser, User>(user);
        }

        public Core.Model.Appuser AutheticateUser(string userName, string password)
        {
            Core.Model.Appuser user = UserManager.Find(userName, password);
            return user;
        }

        public User GetUserDataByAppuser(Core.Model.Appuser user)
        {
            User userObj = AutoMapper.Mapper.Map<Core.Model.Appuser, User>(user);
            IList<string> roles = UserManager.GetRoles(user.Id);
            userObj.Roles = roles;
            return userObj;
        }


        public async Task<bool> ResetPassword(string userId, string newPassword)
        {
            Appuser cUser = UserManager.FindById(userId);
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
            UserStore<Appuser> store = new UserStore<Appuser>();
            await store.SetPasswordHashAsync(cUser, hashedNewPassword);
            return true;
        }

        public bool ValidateUser(string userName, string password)
        {
            var user = UserManager.Find(userName, password);
            return user != null;
        }
    }
}
