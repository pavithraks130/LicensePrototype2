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
        public UserInvite CreateInvite(UserInvite invit)
        {
            License.Core.Model.UserInvite userinvit = AutoMapper.Mapper.Map<Model.Model.UserInvite, License.Core.Model.UserInvite>(invit);
            var obj = Work.UserInviteLicenseRepository.Create(userinvit);
            return AutoMapper.Mapper.Map<License.Core.Model.UserInvite, UserInvite>(obj);
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

        public UserInvite VerifyUserInvited(string email, string adminid)
        {
            var obj = Work.UserInviteLicenseRepository.GetData(filter: t => t.AdminId == adminid && t.InviteeEmail == email).FirstOrDefault();
            return AutoMapper.Mapper.Map<License.Core.Model.UserInvite, UserInvite>(obj);
        }

        public void UpdateInviteStatus(string inviteId, string status)
        {
            Core.Model.UserInvite invite = Work.UserInviteLicenseRepository.GetById(inviteId);
            invite.InviteeStatus = status;
            Work.UserInviteLicenseRepository.Update(invite);
        }
    }
}
