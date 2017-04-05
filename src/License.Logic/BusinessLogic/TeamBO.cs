using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Logic.DataLogic;
using License.DataModel;
using License.Logic.Common;
using License.Core.Manager;
namespace License.Logic.BusinessLogic
{
    public class TeamBO
    {
        UserLogic userLogic = null;
        TeamMemberLogic logic = null;

        public string ErrorMessage { get; set; }

        public AppUserManager UserManager { get; set; }

        public AppRoleManager RoleManager { get; set; }
        public TeamBO()
        {
            userLogic = new UserLogic();
            logic = new TeamMemberLogic();
        }

        public void Initialize()
        {
            userLogic.UserManager = UserManager;
            userLogic.RoleManager = RoleManager;
        }

        public UserInviteList GetUserInviteDetails(string adminId)
        {
            User user = userLogic.GetUserById(adminId);
            UserInviteList inviteList = new UserInviteList()
            {
                AdminUser = user
            };
            var teamMembers = logic.GetUserInviteList(adminId);
            if (teamMembers.Count > 0)
            {
                inviteList.PendingInvites =
                    teamMembers.Where(s => s.InviteeStatus == InviteStatus.Pending.ToString()).ToList();
                inviteList.AcceptedInvites =
                    teamMembers.Where(s => s.InviteeStatus == InviteStatus.Accepted.ToString()).ToList();
                inviteList.AcceptedInvites.Add(new TeamMembers()
                {
                    AdminId = adminId,
                    InviteeEmail = user.Email,
                    InviteeStatus = InviteStatus.Accepted.ToString(),
                    InviteeUserId = adminId,
                    InviteeUser = inviteList.AdminUser,
                    IsAdmin = true
                });
            }
            else
            {
                inviteList.AcceptedInvites.Add(new TeamMembers()
                {
                    AdminId = adminId,
                    InviteeEmail = user.Email,
                    InviteeStatus = InviteStatus.Accepted.ToString(),
                    InviteeUserId = adminId,
                    InviteeUser = inviteList.AdminUser,
                    IsAdmin = true
                });
            }
            ErrorMessage = logic.ErrorMessage;
            return inviteList;
        }

        public TeamMemberResponse CreateTeamMembereInvite(TeamMembers member)
        {
            var user = userLogic.GetUserByEmail(member.InviteeEmail);
            TeamMemberResponse teamMemResObj = new TeamMemberResponse();
            if (user == null)
            {
                Registration reg = new Registration();
                reg.Email = member.InviteeEmail;
                reg.Password = System.Configuration.ConfigurationSettings.AppSettings.Get("");
                var status = userLogic.CreateUser(reg, "TeamMember");
                if (status)
                {
                    user = userLogic.GetUserByEmail(member.InviteeEmail);
                    member.InviteeUserId = user.UserId;
                    var teamMemObj = logic.CreateInvite(member);
                    if(teamMemObj != null)
                    {
                        teamMemResObj.UserId = user.UserId;
                        teamMemResObj.UserName = user.UserName;
                        teamMemResObj.Password = reg.Password;
                        teamMemResObj.TeamMemberId = teamMemObj.Id;
                        return teamMemResObj;
                    }
                }
            }
            return null;

        }
    }
}
