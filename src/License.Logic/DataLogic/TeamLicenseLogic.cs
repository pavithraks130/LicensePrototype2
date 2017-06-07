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
        ProductLicenseLogic proLicLogic = null;

        public TeamLicenseLogic()
        {
            proLicLogic = new ProductLicenseLogic();
        }

        /// <summary>
        /// Function to create Team License
        /// </summary>
        /// <param name="teamLicense"></param>
        /// <returns></returns>
        private bool CreateTeamLicense(TeamLicense teamLicense)
        {
            var obj = AutoMapper.Mapper.Map<TeamLicense, Core.Model.TeamLicense>(teamLicense);
            obj = Work.TeamLicenseRepository.Create(obj);
            Work.TeamLicenseRepository.Save();
            proLicLogic.UpdateLicenseStatus(obj.LicenseId, true);
            return obj.Id > 0;

        }

        /// <summary>
        /// function to create the License  for multiple User . This function will be used for bulk license mapping to 
        /// multiple User.
        /// </summary>
        /// <param name="licList">license List</param>
        /// <param name="userIdList">user Id List</param>
        /// <returns></returns>
        public bool CreateTeamLicense(TeamLicenseDataMapping model)
        {
            ProductLicenseLogic licLogic = new ProductLicenseLogic();
            TeamLogic teamLogic = new TeamLogic();
            foreach (var team in model.TeamList)
            {
                int teamId = team.Id;
                var concurrentUser = team.ConcurrentUserCount; //teamLogic.GetTeamById(teamId).ConcurrentUserCount;
                for (int concurrentUserIndex = 0; concurrentUserIndex < concurrentUser; concurrentUserIndex++)
                {
                    for (int index = 0; index < model.LicenseDataList.Count; index++)
                    {

                        var proId = model.LicenseDataList[index].ProductId;
                        var licId = licLogic.GetUnassignedLicense(proId);
                        TeamLicense tl = new TeamLicense()
                        {
                            LicenseId = licId.Id,
                            TeamId = teamId,
                            ProductId = proId
                        };
                        CreateTeamLicense(tl);
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// Get the product list based on the Availablity of the License for the Product and Expire Date of the Product
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProductFromLicenseData()
        {
            //ListOut the product with IsMap is false;
            //Retrieve prodcut from Json File.

            List<Product> prodList = new List<Product>();
            SubscriptionBO subscriptionBO = new SubscriptionBO();

            var productIdList = Work.ProductLicenseRepository.GetData(ld => ld.IsMapped == false).ToList().Select(l => l.ProductId).ToList();
            var prodIdList = (from id in productIdList
                              group id by id into list
                              select new { list.Key, Count = list.Count() }).ToList();

            for (int index = 0; index < prodIdList.Count; index++)
            {
                var product = subscriptionBO.GetProductFromJsonFile(prodIdList[index].Key);
                if (product != null)
                {
                    product.AvailableCount = prodIdList[index].Count;
                    prodList.Add(product);
                }
            }
            return prodList;
        }

        /// <summary>
        ///  Updating the License status if the license Mapped to user or Removed from user 
        /// </summary>
        /// <param name="licId"></param>
        /// <param name="status"></param>
        public void UpdateLicenseStatus(int licId, bool status)
        {
            var obj = Work.TeamLicenseRepository.GetById(licId);
            obj.IsMapped = status;
            Work.TeamLicenseRepository.Update(obj);
            Work.TeamLicenseRepository.Save();
        }

        /// <summary>
        /// Get the Team License based on the Team ID
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public List<TeamLicense> GetTeamLicense(int teamId)
        {
            List<TeamLicense> teamLicenses = new List<TeamLicense>();
            var datas = Work.TeamLicenseRepository.GetData(t => t.TeamId == teamId);
            teamLicenses = datas.Select(data => AutoMapper.Mapper.Map<Core.Model.TeamLicense, TeamLicense>(data)).ToList();
            return teamLicenses;
        }

        /// <summary>
        /// Remove License by Team . to delete all the Team License deltion when user deletes the team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public bool RemoveLicenseByTeam(int teamId)
        {
            var status = true;
            var teamlicense = Work.TeamLicenseRepository.GetData(t => t.TeamId == teamId).ToList();
            teamlicense.ForEach((tl) =>
            {
                status = status && RemoveTeamLicenseById(tl.Id, tl.LicenseId);
            });
            return status;
        }

        /// <summary>
        /// Removing the Team License based on the Product Id  when user Revoke the Product from team 
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool RemoveLicenseByProduct(int teamId, int productId)
        {
            var teamLicense = Work.TeamLicenseRepository.GetData(tl => tl.TeamId == teamId && tl.ProductId == productId).ToList();
            teamLicense.ForEach(lic => RemoveTeamLicenseById(lic.Id, lic.LicenseId));
            return true;
        }

        /// <summary>
        ///  Removing the Team License based on the Id. Id : Team lIcense Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="licenseId"></param>
        /// <returns></returns>
        public bool RemoveTeamLicenseById(int Id, int licenseId)
        {
            var deletestatus = Work.TeamLicenseRepository.Delete(Id);
            Work.TeamLicenseRepository.Save();
            proLicLogic.UpdateLicenseStatus(licenseId, false);
            return deletestatus;
        }


    }
}
