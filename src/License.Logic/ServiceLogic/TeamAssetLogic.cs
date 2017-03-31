using License.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamAssetModel = License.Model.TeamAsset;

namespace License.Logic.ServiceLogic
{
    public class TeamAssetLogic : BaseLogic
    {
        public TeamAssetModel CreateAsset(TeamAssetModel teamAsset)
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

        public List<TeamAssetModel> GetAssets()
        {
            List<TeamAssetModel> teamAssets = new List<TeamAssetModel>();
            var listData = Work.TeamAssetRepository.GetData();
            foreach (var assetItem in listData)
            {
                teamAssets.Add(AutoMapper.Mapper.Map<Core.Model.TeamAsset, TeamAssetModel>(assetItem));
            }

            return teamAssets;
        }

        public bool RemoveAsset(int id)
        {
            try
            {
                var teamObj = Work.TeamAssetRepository.GetById(id);
                var status = Work.TeamAssetRepository.Delete(teamObj);
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
