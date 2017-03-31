using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.Model;
using License.Logic.Common;
using License.Model;
using Microsoft.AspNet.Identity;
using License.Core.Model;
using License.Logic.ServiceLogic;

namespace License.MetCalWeb.Controllers
{
    internal class TeamAssetLogic : BaseLogic
    {
        //public TeamAsset CreateAsset(TeamAsset tAsset)
        //{
        //    var obj = Work.UserInviteRepository.GetData(f => f.AdminId == invit.AdminId && f.InviteeEmail == invit.InviteeEmail && f.TeamId == invit.TeamId).FirstOrDefault();
        //    if (obj == null)
        //    {
        //        obj = Work.UserInviteRepository.Create(userinvit);
        //        Work.UserInviteRepository.Save();
        //    }
        //    return AutoMapper.Mapper.Map<License.Core.Model.TeamMembers, TeamMembers>(obj);
        //}
    }
}