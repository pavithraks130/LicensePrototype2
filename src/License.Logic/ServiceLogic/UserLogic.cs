using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.Model;
using License.Model.Model;
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
                User temp = AutoMapper.Mapper.Map<License.Core.Model.AppUser, License.Model.Model.User>(u);
                usersList.Add(temp);
            }
            return usersList;
        }

        public IdentityResult CreateUser(Registration u)
        {
            User ur = new User();
            ur.FirstName = u.FirstName;
            ur.LastName = u.LastName;
            ur.Email = u.Email;
            ur.PhoneNumber = u.PhoneNumber;
            ur.UserName = u.Email;
            var teamName = u.OrganizationName;
            TeamLogic logic = new TeamLogic();
            Model.Model.Team t = logic.GetTeamByName(teamName);
            if (t == null)
                t = logic.CreateTeam(new Model.Model.Team() { Name = u.OrganizationName });
            ur.Organization = t;
            AppUser user = AutoMapper.Mapper.Map<License.Model.Model.User, License.Core.Model.AppUser>(ur);
            IdentityResult result;
            try
            {
                result = UserManager.Create(user, u.Password);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
