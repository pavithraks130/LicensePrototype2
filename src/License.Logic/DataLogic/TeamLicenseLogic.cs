using License.DataModel;
using License.Logic.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Logic.DataLogic
{
    public class TeamLicenseLogic : BaseLogic
    {
        private readonly object Count;
        private bool CreateTeamLicense(TeamLicense teamLicense)
        {
            var obj = AutoMapper.Mapper.Map<TeamLicense, Core.Model.TeamLicense>(teamLicense);
            obj = Work.TeamLicenseRepository.Create(obj);
            Work.TeamLicenseRepository.Save();
            UpdateLicenseStatus(obj.LicenseId, true);
            return obj.Id > 0;

        }
             
        /// <summary>
        /// function to create the License  for multiple User . This function will be used for bulk license mapping to 
        /// multiple User.
        /// </summary>
        /// <param name="licList">license List</param>
        /// <param name="userIdList">user Id List</param>
        /// <returns></returns>
        public bool CreateMultipleTeamLicense(TeamLicenseDataMapping model)
        {
            LicenseLogic licLogic = new LicenseLogic();
            foreach (var teamId in model.TeamList)
            {
                for (int concurrentUserIndex = 0; concurrentUserIndex < model.ConcurrentUserCount; concurrentUserIndex++)
                {
                    int id = int.Parse(teamId);
                    for (int index = 0; index < model.ProductIdList.Count; index++)
                    {

                        var proId = model.ProductIdList[index];
                        var data = Work.LicenseDataRepository.GetData(l => l.ProductId == proId).ToList().Select(l => l.Id).ToList();
                        var licId = licLogic.GetUnassignedLicenseForTeam(model.ProductIdList[index]);
                        TeamLicense tl = new TeamLicense()
                        {
                            LicenseId = licId.Id,
                            TeamId = id,
                            ProductId = proId
                        };
                        CreateTeamLicense(tl);
                    }
                }

            }
            return true;
        }

        public List<Products> GetProductFromLicenseData()
        {
            //ListOut the product with IsMap is false;
            //Retrieve prodcut from Json File.

            List<Products> prodList = new List<Products>();
            SubscriptionBO subscriptionBO = new SubscriptionBO();

            var productIdList = Work.LicenseDataRepository.GetData(ld => ld.IsMapped == false).ToList().Select(l => l.ProductId).ToList();
            var prodIdList = (from id in productIdList
                              group id by id into list
                              select new { list.Key, Count = list.Count() }).ToList();

            for (int index = 0; index < prodIdList.Count; index++)
            {
                var product = subscriptionBO.GetProductFromJsonFile(prodIdList[index].Key);
                if (product != null)
                {
                    Products pro = new Products();
                    pro.Product = product;
                    pro.AvailableProductCount = prodIdList[index].Count;
                    prodList.Add(pro);
                }
            }
            return prodList;
        }

        public void UpdateLicenseStatus(int licId, bool status)
        {
            var obj = Work.LicenseDataRepository.GetById(licId);
            obj.IsMapped = status;
            Work.LicenseDataRepository.Update(obj);
            Work.LicenseDataRepository.Save();
        }

        public LicenseData GetUnassignedLicense(int userSubscriptionId, int productId)
        {
            var obj = Work.LicenseDataRepository.GetData(f => f.UserSubscriptionId == userSubscriptionId && f.ProductId == productId && f.IsMapped == false).FirstOrDefault();
            return AutoMapper.Mapper.Map<Core.Model.LicenseData, LicenseData>(obj);
        }

        public List<TeamLicense> GetTeamLicense(int teamId)
        {
            List<TeamLicense> teamLicenses = new List<TeamLicense>();
            var datas = Work.TeamLicenseRepository.GetData(t => t.TeamId == teamId);
            foreach (var data in datas)
                teamLicenses.Add(AutoMapper.Mapper.Map<Core.Model.TeamLicense, TeamLicense>(data));
            return teamLicenses;
        }
        public IEnumerable<License.Core.Model.LicenseData> GetLicenseData()
        {
            return Work.LicenseDataRepository.GetData();
        }
        public void UpdateLicenseDataTableByProductId(int licId)
        {
            UpdateLicenseStatus(licId, false);
        }
        public bool RemoveLicenseByLicenseId(int licId)
        {
            var deleteRowId = Work.TeamLicenseRepository.GetData(tl => tl.LicenseId == licId).FirstOrDefault().Id;
            var deletestatus = Work.TeamLicenseRepository.Delete(deleteRowId);
            Work.TeamLicenseRepository.Save();
            return deletestatus;
        }

    }
}
