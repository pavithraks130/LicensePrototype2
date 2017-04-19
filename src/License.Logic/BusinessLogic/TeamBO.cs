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
        TeamLogic teamLogic = null;

        public string ErrorMessage { get; set; }

        public AppUserManager UserManager { get; set; }

        public AppRoleManager RoleManager { get; set; }
        public TeamBO()
        {
            userLogic = new UserLogic();
            logic = new TeamMemberLogic();
            teamLogic = new TeamLogic();
        }

        public void Initialize()
        {
            userLogic.UserManager = UserManager;
            userLogic.RoleManager = RoleManager;
        }

        //public TeamDetails GetUserInviteDetails(string adminId)
        //{
        //    Initialize();
        //    User user = userLogic.GetUserById(adminId);
        //    TeamDetails inviteList = new TeamDetails()
        //    {
        //        AdminUser = user
        //    };
        //    var teamMembers = logic.GetUserInviteList(adminId);
        //    if (teamMembers.Count > 0)
        //    {
        //        inviteList.PendinigUsers =
        //            teamMembers.Where(s => s.InviteeStatus == InviteStatus.Pending.ToString()).ToList();
        //        inviteList.AcceptedUsers =
        //            teamMembers.Where(s => s.InviteeStatus == InviteStatus.Accepted.ToString()).ToList();
        //        inviteList.AcceptedUsers.Add(new TeamMember()
        //        {
        //            AdminId = adminId,
        //            InviteeEmail = user.Email,
        //            InviteeStatus = InviteStatus.Accepted.ToString(),
        //            InviteeUserId = adminId,
        //            InviteeUser = inviteList.AdminUser,
        //            IsAdmin = true
        //        });
        //    }
        //    else
        //    {
        //        inviteList.AcceptedUsers.Add(new TeamMember()
        //        {
        //            AdminId = adminId,
        //            InviteeEmail = user.Email,
        //            InviteeStatus = InviteStatus.Accepted.ToString(),
        //            InviteeUserId = adminId,
        //            InviteeUser = inviteList.AdminUser,
        //            IsAdmin = true
        //        });
        //    }
        //    ErrorMessage = logic.ErrorMessage;
        //    return inviteList;
        //}

        public TeamDetails GetteamDetails(int id)
        {
            TeamLogic teamLogic = new TeamLogic();
            TeamDetails dtls = new TeamDetails();
            var team = teamLogic.GetTeamById(id);
            if (team != null)
            {
                dtls.Team = new Team();
                dtls.Team.AdminId = team.AdminId;
                dtls.Team.AdminUser = team.AdminUser;
                dtls.Team.Id = team.Id;
                dtls.Team.Name = team.Name;
                if (team.TeamMembers.Count > 0)
                {
                    dtls.PendinigUsers =
                        team.TeamMembers.Where(s => s.InviteeStatus == InviteStatus.Pending.ToString()).ToList();
                    dtls.AcceptedUsers =
                        team.TeamMembers.Where(s => s.InviteeStatus == InviteStatus.Accepted.ToString()).ToList();
                    dtls.AcceptedUsers.Add(new TeamMember()
                    {
                        InviteeEmail = team.AdminUser.Email,
                        InviteeStatus = InviteStatus.Accepted.ToString(),
                        InviteeUserId = team.AdminUser.UserId,
                        InviteeUser = team.AdminUser,
                        IsAdmin = true
                    });
                }
                else
                {
                    dtls.AcceptedUsers.Add(new TeamMember()
                    {
                        InviteeEmail = team.AdminUser.Email,
                        InviteeStatus = InviteStatus.Accepted.ToString(),
                        InviteeUserId = team.AdminUser.UserId,
                        InviteeUser = team.AdminUser,
                        IsAdmin = true
                    });
                }
            }
            return dtls;
        }

        public TeamMemberResponse CreateTeamMembereInvite(TeamMember member)
        {
            Initialize();
            var user = userLogic.GetUserByEmail(member.InviteeEmail);
            TeamMemberResponse teamMemResObj = new TeamMemberResponse();
            string password = System.Configuration.ConfigurationSettings.AppSettings.Get("TeamMemberDefaultPassword");
            if (user == null)
            {
                Registration reg = new Registration();
                reg.Email = member.InviteeEmail;
                reg.Password = password;
                var status = userLogic.CreateUser(reg, "TeamMember");
                if (status)
                    user = userLogic.GetUserByEmail(member.InviteeEmail);
                else
                {
                    ErrorMessage = userLogic.ErrorMessage;
                    return null;
                }
            }

            member.InviteeUserId = user.UserId;
            var teamMemObj = logic.CreateInvite(member);
            if (teamMemObj != null)
            {
                teamMemResObj.UserId = user.UserId;
                teamMemResObj.UserName = user.UserName;
                teamMemResObj.Password = password;
                teamMemResObj.TeamMemberId = teamMemObj.Id;
                return teamMemResObj;
            }
            else
                ErrorMessage = logic.ErrorMessage;

            return null;

        }

        public bool AddTeamMembers(List<TeamMember> teamMemberList)
        {
            Initialize();
            User user = null;
            foreach (var mem in teamMemberList)
            {
                if (user == null || user.UserId != mem.InviteeUserId)
                    user = userLogic.GetUserById(mem.InviteeUserId);
                mem.InviteeEmail = user.Email;
                mem.InvitationDate = DateTime.Now.Date;
                logic.CreateInvite(mem);
            }
            return true;
        }


        public bool RemoveTeamMembers(List<TeamMember> teamMemberList)
        {
            bool status = true;
            foreach (var mem in teamMemberList)
            {
                status &= logic.DeleteTeamMember(mem);
                var adminId = teamLogic.GetTeamById(mem.TeamId).AdminId;
                var team = teamLogic.GetTeamsByAdmin(adminId).FirstOrDefault(t => t.IsDefaultTeam);
                var memberlist = logic.GetTeamMemberDetailsByUserId(mem.InviteeUserId);
                if (memberlist.Count == 0)
                    logic.CreateInvite(new TeamMember() { InvitationDate = DateTime.Now.Date, TeamId = team.Id, InviteeEmail = mem.InviteeEmail, InviteeUserId = mem.InviteeUserId, InviteeStatus = License.Logic.Common.InviteStatus.Accepted.ToString() });
            }
            return status;
        }

    }
}
