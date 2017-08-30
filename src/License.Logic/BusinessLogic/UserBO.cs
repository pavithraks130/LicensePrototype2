using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;
using License.Logic.DataLogic;
using License.Core.Manager;

namespace License.Logic.BusinessLogic
{
    public class UserBO
    {
        UserLogic _userLogic = null;
        TeamLogic _teamLogic = null;
        UserLicenseLogic _userLicenseLogic = null;

        public AppUserManager UserManager { get; set; }
        public AppRoleManager RoleManager { get; set; }
        
        public string ErrorMessage { get; set; }
        public UserBO()
        {
            _userLogic = new UserLogic();
            _teamLogic = new TeamLogic();
            _userLicenseLogic = new UserLicenseLogic();
        }

        public void Initialize()
        {
            _userLogic.UserManager = UserManager;
            _userLogic.RoleManager = RoleManager;
        }

        public UserDetails GetUserDetailsByUserId(string userId)
        {
            Initialize();
            UserDetails details = new UserDetails();
            var user = _userLogic.GetUserById(userId);
            if (user.Roles.Contains("Super Admin"))
                details.Teams = _teamLogic.GetTeamsByAdmin(userId);
            else
                details.Teams = _teamLogic.GetTeamsByUser(userId);
            ErrorMessage = _teamLogic.ErrorMessage;
            details.UserLicenses = _userLicenseLogic.GetUserLicenseByUserId(userId);
            if (!String.IsNullOrEmpty(ErrorMessage))
                ErrorMessage += " " + _userLicenseLogic.ErrorMessage;
            return details;
        }
    }
}
