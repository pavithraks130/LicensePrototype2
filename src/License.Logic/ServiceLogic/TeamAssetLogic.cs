using License.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamAssetModel = License.Model.TeamAsset;

namespace License.Logic.ServiceLogic
{
    class TeamAssetLogic : BaseLogic
    {
        public TeamAssetModel CreateInvite(TeamAssetModel teamAsset)
        {
            TeamAsset convertedTeamAsset = AutoMapper.Mapper.Map<TeamAssetModel, TeamAsset>(teamAsset);
            var obj = Work.TeamAssetRepository.GetData(f => f.SerialNumber == teamAsset.SerialNumber).FirstOrDefault();
            if (obj == null)
            {
                obj = Work.TeamAssetRepository.Create(convertedTeamAsset);
                Work.TeamAssetRepository.Save();
            }
            return AutoMapper.Mapper.Map<TeamAsset, TeamAssetModel>(obj);
        }
    }
}
