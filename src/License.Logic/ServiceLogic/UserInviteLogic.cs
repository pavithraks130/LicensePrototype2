using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model.Model;

namespace License.Logic.ServiceLogic
{
    public class UserInviteLogic :BaseLogic
    {
        public UserInvite CreateInvite(UserInvite invit)
        {
            License.Core.Model.UserInvite userinvit = AutoMapper.Mapper.Map<Model.Model.UserInvite, License.Core.Model.UserInvite>(invit);
            var obj = Work.UserInviteLicenseRepository.Create(userinvit);
            return AutoMapper.Mapper.Map<License.Core.Model.UserInvite, UserInvite>(obj);
        }


    }
}
