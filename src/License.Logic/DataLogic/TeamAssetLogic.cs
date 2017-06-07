using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.Model;
using TeamAssetModel = License.DataModel.TeamAsset;

namespace License.Logic.DataLogic
{
    /// <summary>
    /// History:
    ///         Created By :
    ///         Created Date:
    ///         Purpose:     Used to Perform CRUD  functionality ono team asset Table DB
    /// </summary>
    public class TeamAssetLogic : BaseLogic
    {
        /// <summary>
        /// Create Teama Asset record 
        /// </summary>
        /// <param name="teamAsset"></param>
        /// <returns></returns>
        public TeamAssetModel CreateAsset(TeamAssetModel teamAsset)
        {
            TeamAsset convertedTeamAsset = AutoMapper.Mapper.Map<TeamAssetModel, TeamAsset>(teamAsset);
            var obj = Work.TeamAssetRepository.GetData(f => f.SerialNumber == teamAsset.SerialNumber).FirstOrDefault();
            if (obj == null)
            {
                obj = Work.TeamAssetRepository.Create(convertedTeamAsset);
                Work.TeamAssetRepository.Save();
                return AutoMapper.Mapper.Map<TeamAsset, TeamAssetModel>(obj);
            }
            else
                ErrorMessage = "Serial number is already  assigned to asset.";
            return null;
           
        }

        /// <summary>
        /// Get List of all the Team Asset
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get Team Asset by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TeamAssetModel GetAssetById(int id)
        {
            var obj = Work.TeamAssetRepository.GetById(id);
            return AutoMapper.Mapper.Map<TeamAssetModel>(obj);
        }

        /// <summary>
        /// Delete Team Asset by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveAsset(int id)
        {
            try
            {
                var status = Work.TeamAssetRepository.Delete(id);
                Work.TeamAssetRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// Update Team Asset by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public TeamAssetModel UpdateAsset(int id, TeamAssetModel model)
        {
            var obj = Work.TeamAssetRepository.GetById(id);
            if (obj != null)
            {
                obj.Description = model.Description;
                obj.Model = model.Model;
                obj.Name = model.Name;
                obj.SerialNumber = model.SerialNumber;
                obj.Type = model.Type;
                obj = Work.TeamAssetRepository.Update(obj);
                Work.TeamAssetRepository.Save();
                return AutoMapper.Mapper.Map<TeamAssetModel>(obj);
            }
            else
                ErrorMessage = "Specified assset not exist";
            return null;
        }
    }
}
