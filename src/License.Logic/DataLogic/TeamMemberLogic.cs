using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.Model;
using License.Logic.Common;
using License.DataModel;
using Microsoft.AspNet.Identity;
using DataModelTeamMember = License.DataModel.TeamMember;

namespace License.Logic.DataLogic
{
    public class TeamMemberLogic : BaseLogic
    {
        public DataModelTeamMember CreateInvite(DataModelTeamMember invit)
        {
            License.Core.Model.TeamMember userinvit = AutoMapper.Mapper.Map<DataModel.TeamMember, License.Core.Model.TeamMember>(invit);
            var obj = Work.UserInviteRepository.GetData(f => f.AdminId == invit.AdminId && f.InviteeEmail == invit.InviteeEmail && f.TeamId == invit.TeamId).FirstOrDefault();
            if (obj == null)
            {
                obj = Work.UserInviteRepository.Create(userinvit);
                Work.UserInviteRepository.Save();
            }
            return AutoMapper.Mapper.Map<License.Core.Model.TeamMember, DataModelTeamMember>(obj);
        }

        public List<DataModelTeamMember> GetUserInviteList(string adminId)
        {
            List<DataModelTeamMember> teamMembers = new List<DataModelTeamMember>();
            var listData = Work.UserInviteRepository.GetData(filter: t => t.AdminId == adminId);
            foreach (var data in listData)
                teamMembers.Add(AutoMapper.Mapper.Map<Core.Model.TeamMember, DataModel.TeamMember>(data));
            return teamMembers;
        }

        public DataModelTeamMember VerifyUserInvited(string email, string adminid)
        {
            var obj = Work.UserInviteRepository.GetData(filter: t => t.AdminId == adminid && t.InviteeEmail == email).FirstOrDefault();
            return AutoMapper.Mapper.Map<License.Core.Model.TeamMember, DataModelTeamMember>(obj);
        }

        public void UpdateInviteStatus(object inviteId, string status)
        {
            Core.Model.TeamMember invite = Work.UserInviteRepository.GetById(inviteId);
            invite.InviteeStatus = status;
            Core.Model.TeamMember ember = Work.UserInviteRepository.Update(invite);
            Work.UserInviteRepository.Save();
        }

        public DataModel.TeamMember GetTeamMemberByUserId(string userId)
        {
            var obj = Work.UserInviteRepository.GetData(t => t.InviteeUserId == userId).FirstOrDefault();
            return AutoMapper.Mapper.Map<DataModel.TeamMember>(obj);
        }

        public void SetAsAdmin(int id, string userId, bool adminStatus)
        {
            Core.Model.TeamMember teamMembers = Work.UserInviteRepository.GetById(id);
            if (adminStatus)
            {
                if (!RoleManager.RoleExists("Admin"))
                    RoleManager.Create(new Core.Model.Role() { Name = "Admin" });
                UserManager.AddToRole(userId, "Admin");
            }
            else
                UserManager.RemoveFromRole(userId, "Admin");
            teamMembers.IsAdmin = adminStatus;
            Work.UserInviteRepository.Update(teamMembers);
            Work.UserInviteRepository.Save();
        }

        public bool DeleteTeamMember(int id)
        {
            try
            {
                var teamObj = Work.UserInviteRepository.GetById(id);
                var licenseData = Work.UserLicenseRepository.GetData(u => u.UserId == teamObj.InviteeUserId);
                if (licenseData.Count() > 0)
                    foreach (var dt in licenseData)
                        Work.UserLicenseRepository.Delete(dt);
                if (teamObj.IsAdmin)
                    UserManager.RemoveFromRole(teamObj.InviteeUserId, "Admin");
                var status = Work.UserInviteRepository.Delete(teamObj);
                Work.UserInviteRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;

        }

    }
}
