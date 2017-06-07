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
    /// <summary>
    /// History 
    ///     Created By :
    ///     Created Date ;
    ///     Purpose :  1. functionality for CRUD operation in the Team Member table
    /// </summary>
    public class TeamMemberLogic : BaseLogic
    {
        /// <summary>
        /// Create Team Member Record if the invitation exist then the record won't be created else record will be created in DB.
        /// </summary>
        /// <param name="invit"></param>
        /// <returns></returns>
        public DataModelTeamMember CreateInvite(DataModelTeamMember invit)
        {
            License.Core.Model.TeamMember userinvit = AutoMapper.Mapper.Map<Core.Model.TeamMember>(invit);
            var obj = Work.TeamMemberRepository.GetData(f => f.TeamId == invit.TeamId && f.InviteeEmail == invit.InviteeEmail).FirstOrDefault();
            if (obj == null)
            {
                obj = Work.TeamMemberRepository.Create(userinvit);
                Work.TeamMemberRepository.Save();
            }
            return AutoMapper.Mapper.Map<License.Core.Model.TeamMember, DataModelTeamMember>(obj);
        }

        /// <summary>
        /// Get team member list based on the Team ID
        /// </summary>
        /// <param name="TeamId"></param>
        /// <returns></returns>
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

        /// <summary>
        ///  Updating the user invite status when user accepts or Reject the Invitation
        /// </summary>
        /// <param name="inviteId"></param>
        /// <param name="status"></param>
        public void UpdateInviteStatus(object inviteId, string status)
        {
            Core.Model.TeamMember invite = Work.TeamMemberRepository.GetById(inviteId);
            invite.InviteeStatus = status;
            Work.TeamMemberRepository.Update(invite);
            Work.TeamMemberRepository.Save();
        }

        /// <summary>
        /// Gettin all the team Mber Record based on the InviteeUser Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<DataModel.TeamMember> GetTeamMemberDetailsByUserId(string userId)
        {
            var obj = Work.TeamMemberRepository.GetData(t => t.InviteeUserId == userId).ToList();
            return AutoMapper.Mapper.Map<List<DataModel.TeamMember>>(obj);
        }

        /// <summary>
        ///  Updating the partial adminaccess or removing the partial admin access for the User (Team Meber). If the user set as partial admin then along with updating Is Admin 
        /// in the TeamMember the Admin role will be added to the User role or removed from the user role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="adminStatus"></param>
        public void SetAsAdmin(int id, string userId, bool adminStatus)
        {

            Core.Model.TeamMember teamMember = Work.TeamMemberRepository.GetById(id);
            var team = Work.TeamRepository.GetById(teamMember.TeamId);
            var teamMemberList = Work.TeamMemberRepository.GetData(t => t.InviteeUserId == userId && t.Team.AdminId == team.AdminId).ToList();
            if (adminStatus)
            {
                if (!RoleManager.RoleExists("Admin"))
                    RoleManager.Create(new Core.Model.Role() { Name = "Admin" });
                UserManager.AddToRole(userId, "Admin");
            }
            else
                UserManager.RemoveFromRole(userId, "Admin");
            int count = 0;
            foreach (var teamMembers in teamMemberList)
            {
                teamMembers.IsAdmin = adminStatus;
                Work.TeamMemberRepository.Update(teamMembers);
                count++;
            }
            if (count > 0)
                Work.TeamMemberRepository.Save();
        }

        /// <summary>
        /// Deleteing the the Team Meber based on the user id and team id. along with the Team Member record deletion te License mapped to the User will also be removed.
        /// </summary>
        /// <param name="teamMember"></param>
        /// <returns></returns>
        public bool DeleteTeamMember(DataModelTeamMember teamMember)
        {
            var obj = Work.TeamMemberRepository.GetData(t => t.InviteeUserId == teamMember.InviteeUserId && t.TeamId == teamMember.TeamId).FirstOrDefault();
            if (obj != null)
                return DeleteTeamMember(obj.Id);
            return false;
        }

        /// <summary>
        ///  Deleting the Team Mmeber , User license and if any team License mapped to deleting b user then the team license will also be deleted.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTeamMember(int id)
        {
            try
            {
                var teamObj = Work.TeamMemberRepository.GetById(id);

                // Deleting the user License for the User based on the team from which he his removed
                var licenseData = Work.UserLicenseRepository.GetData(u => u.UserId == teamObj.InviteeUserId && u.TeamId == teamObj.TeamId);
                if (licenseData.Count() > 0)
                {
                    int i = 0;
                    foreach (var dt in licenseData)
                    {
                        i++;
                        // If the user License is a Team licensse then the Team license mapping for the user will be deleted and the Is Mapped status will be set to false in the Team license
                        // for the mapped teamLicense Id, which can be used by other members of the team.
                        if (dt.IsTeamLicense)
                        {
                            var teamLicense = Work.TeamLicenseRepository.GetById(dt.TeamLicenseId);
                            teamLicense.IsMapped = false;
                            Work.TeamLicenseRepository.Update(teamLicense);
                            Work.TeamLicenseRepository.Save();
                        }
                        Work.UserLicenseRepository.Delete(dt);
                    }
                    if (i > 0)
                        Work.UserLicenseRepository.Save();

                }
                var team = Work.TeamRepository.GetById(teamObj.TeamId);

                // if the user is not belong to any other team excluding the current team from which he his being removed and if he has admin access 
                // then the admin role will be removed for the user.
                var membList = Work.TeamMemberRepository.GetData(t => t.InviteeUserId == teamObj.InviteeUserId && team.AdminId == teamObj.Team.AdminId).ToList();
                int count = membList.Where(t => t.Id != teamObj.Id).Count();               
                if (teamObj.IsAdmin && count == 0)
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
