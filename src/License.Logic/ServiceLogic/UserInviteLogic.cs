using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model.Model;

namespace License.Logic.ServiceLogic
{
    public class UserInviteLogic : BaseLogic
    {
        public TeamMembers CreateInvite(TeamMembers invit)
        {
            License.Core.Model.TeamMembers userinvit = AutoMapper.Mapper.Map<Model.Model.TeamMembers, License.Core.Model.TeamMembers>(invit);
            var obj = Work.UserInviteLicenseRepository.Create(userinvit);
            return AutoMapper.Mapper.Map<License.Core.Model.TeamMembers, TeamMembers>(obj);
        }

        public UserInviteList GetUserInviteDetails(string adminId)
        {
            UserInviteList inviteList = new UserInviteList();
            var listData = Work.UserInviteLicenseRepository.GetData(filter: t => t.AdminId == adminId);
            foreach (var data in listData)
            {

            }
            return inviteList;
        }

        public TeamMembers VerifyUserInvited(string email, string adminid)
        {
            var obj = Work.UserInviteLicenseRepository.GetData(filter: t => t.AdminId == adminid && t.InviteeEmail == email).FirstOrDefault();
            return AutoMapper.Mapper.Map<License.Core.Model.TeamMembers, TeamMembers>(obj);
        }

        public void UpdateInviteStatus(string inviteId, string status)
        {
            Core.Model.TeamMembers invite = Work.UserInviteLicenseRepository.GetById(inviteId);
            invite.InviteeStatus = status;
            Work.UserInviteLicenseRepository.Update(invite);
        }

        public string GetUserAdminDetails(string userId)
        {
            var obj = Work.UserInviteLicenseRepository.GetData(t => t.InviteeUserId == userId).FirstOrDefault();
            if (obj != null)
                return obj.AdminId;
            return string.Empty;
        }
    }
}
