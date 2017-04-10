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
            var obj = Work.TeamMemberRepository.GetData(f => f.TeamId == invit.TeamId && f.InviteeEmail == invit.InviteeEmail && f.TeamId == invit.TeamId).FirstOrDefault();
            if (obj == null)
            {
                obj = Work.TeamMemberRepository.Create(userinvit);
                Work.TeamMemberRepository.Save();
            }
            return AutoMapper.Mapper.Map<License.Core.Model.TeamMember, DataModelTeamMember>(obj);
        }

        public List<DataModelTeamMember> GetUserInviteList(string adminId)
        {
            List<DataModelTeamMember> teamMembers = new List<DataModelTeamMember>();
            var team = Work.TeamRepository.GetData(t => t.AdminId == adminId).FirstOrDefault();
            var listData = Work.TeamMemberRepository.GetData(filter: t => t.TeamId == team.Id);
            foreach (var data in listData)
                teamMembers.Add(AutoMapper.Mapper.Map<Core.Model.TeamMember, DataModel.TeamMember>(data));
            return teamMembers;
        }

        public List<DataModelTeamMember> GetTeamMembers(int TeamId)
        {
            List<DataModelTeamMember> teamMembers = new List<DataModelTeamMember>();
            var listData = Work.TeamMemberRepository.GetData(filter: t => t.TeamId == TeamId);
            foreach (var data in listData)
                teamMembers.Add(AutoMapper.Mapper.Map<Core.Model.TeamMember, DataModel.TeamMember>(data));
            return teamMembers;
        }

        public DataModelTeamMember VerifyUserInvited(string email, string adminid)
        {
            var team = Work.TeamRepository.GetData(r => r.AdminId == adminid).FirstOrDefault();
            var obj = Work.TeamMemberRepository.GetData(filter: t => t.TeamId == team.Id && t.InviteeEmail == email).FirstOrDefault();
            return AutoMapper.Mapper.Map<License.Core.Model.TeamMember, DataModelTeamMember>(obj);
        }

        public void UpdateInviteStatus(object inviteId, string status)
        {
            Core.Model.TeamMember invite = Work.TeamMemberRepository.GetById(inviteId);
            invite.InviteeStatus = status;
            Core.Model.TeamMember ember = Work.TeamMemberRepository.Update(invite);
            Work.TeamMemberRepository.Save();
        }

        public DataModel.TeamMember GetTeamMemberByUserId(string userId)
        {
            var obj = Work.TeamMemberRepository.GetData(t => t.InviteeUserId == userId).FirstOrDefault();
            return AutoMapper.Mapper.Map<DataModel.TeamMember>(obj);
        }

        public void SetAsAdmin(int id, string userId, bool adminStatus)
        {
            Core.Model.TeamMember teamMembers = Work.TeamMemberRepository.GetById(id);
            if (adminStatus)
            {
                if (!RoleManager.RoleExists("Admin"))
                    RoleManager.Create(new Core.Model.Role() { Name = "Admin" });
                UserManager.AddToRole(userId, "Admin");
            }
            else
                UserManager.RemoveFromRole(userId, "Admin");
            teamMembers.IsAdmin = adminStatus;
            Work.TeamMemberRepository.Update(teamMembers);
            Work.TeamMemberRepository.Save();
        }

        public bool DeleteTeamMember(int id)
        {
            try
            {
                var teamObj = Work.TeamMemberRepository.GetById(id);
                var licenseData = Work.UserLicenseRepository.GetData(u => u.UserId == teamObj.InviteeUserId);
                if (licenseData.Count() > 0)
                    foreach (var dt in licenseData)
                        Work.UserLicenseRepository.Delete(dt);
                if (teamObj.IsAdmin)
                    UserManager.RemoveFromRole(teamObj.InviteeUserId, "Admin");
                var status = Work.TeamMemberRepository.Delete(teamObj);
                Work.TeamMemberRepository.Save();
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
