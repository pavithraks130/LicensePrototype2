using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.Model;
using License.Logic.Common;
using License.Model.Model;
using Microsoft.AspNet.Identity;
using TeamMembers = License.Model.Model.TeamMembers;

namespace License.Logic.ServiceLogic
{
    public class TeamMemberLogic : BaseLogic
    {
        public TeamMembers CreateInvite(TeamMembers invit)
        {
            License.Core.Model.TeamMembers userinvit = AutoMapper.Mapper.Map<Model.Model.TeamMembers, License.Core.Model.TeamMembers>(invit);
            var obj = Work.UserInviteLicenseRepository.Create(userinvit);
            Work.UserInviteLicenseRepository.Save();
            return AutoMapper.Mapper.Map<License.Core.Model.TeamMembers, TeamMembers>(obj);
        }

        public UserInviteList GetUserInviteDetails(string adminId)
        {
            AppUser user = UserManager.FindById(adminId);
            UserInviteList inviteList = new UserInviteList();
            inviteList.AdminUser = AutoMapper.Mapper.Map<AppUser, User>(user);
            List<TeamMembers> teamMembers = new List<TeamMembers>();
            var listData = Work.UserInviteLicenseRepository.GetData(filter: t => t.AdminId == adminId);
            foreach (var data in listData)
                teamMembers.Add(AutoMapper.Mapper.Map<Core.Model.TeamMembers, Model.Model.TeamMembers>(data));
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
                    IsAdmin = true
                });
            }
            return inviteList;
        }

        public TeamMembers VerifyUserInvited(string email, string adminid)
        {
            var obj = Work.UserInviteLicenseRepository.GetData(filter: t => t.AdminId == adminid && t.InviteeEmail == email).FirstOrDefault();
            return AutoMapper.Mapper.Map<License.Core.Model.TeamMembers, TeamMembers>(obj);
        }

        public void UpdateInviteStatus(object inviteId, string status)
        {
            Core.Model.TeamMembers invite = Work.UserInviteLicenseRepository.GetById(inviteId);
            invite.InviteeStatus = status;
            Core.Model.TeamMembers ember = Work.UserInviteLicenseRepository.Update(invite);
            Work.UserInviteLicenseRepository.Save();
        }

        public string GetUserAdminDetails(string userId)
        {
            var obj = Work.UserInviteLicenseRepository.GetData(t => t.InviteeUserId == userId).FirstOrDefault();
            if (obj != null)
                return obj.AdminId;
            return string.Empty;
        }

        public void SetAsAdmin(int id, string userId, bool status)
        {
            Core.Model.TeamMembers teamMembers = Work.UserInviteLicenseRepository.GetById(id);
            teamMembers.IsAdmin = status;
            Work.UserInviteLicenseRepository.Update(teamMembers);
        }
      
    }
}
