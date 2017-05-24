using License.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Logic.DataLogic
{
   public class TeamLicenseLogic:BaseLogic
    {
        private bool CreateTeamLicense(TeamLicense teamLicense)
        {
            var obj = AutoMapper.Mapper.Map<TeamLicense, Core.Model.TeamLicense>(teamLicense);
            obj = Work.TeamLicenseRepository.Create(obj);
            Work.TeamLicenseRepository.Save();
            UpdateLicenseStatus(obj.LicenseId,true);
            return obj.Id > 0;

        }

        /// <summary>
        /// creating the UserLicense this can be used to update the single user.
        /// </summary>
        /// <param name="licList"></param>
        /// <returns></returns>
        public bool CreateTeamLicense(List<TeamLicense> licList)
        {
            LicenseLogic licLogic = new LicenseLogic();
            foreach (var lic in licList)
            {
                var teamLicList = Work.TeamLicenseRepository.GetData(tl => tl.TeamId == lic.TeamId).ToList();
                var data = Work.LicenseDataRepository.GetData(l => l.ProductId == lic.License.ProductId && l.UserSubscriptionId == lic.License.UserSubscriptionId).ToList().Select(l => l.Id);
                var obj = teamLicList.FirstOrDefault(ul => data.Contains(ul.LicenseId));
                if (obj == null)
                {
                    var licId = licLogic.GetUnassignedLicense(lic.License.UserSubscriptionId, lic.License.ProductId).Id;
                    TeamLicense teamlic = new TeamLicense();
                    teamlic.TeamId = lic.TeamId;
                    teamlic.LicenseId = licId;
                    teamlic.IsMapped = false;
                    CreateTeamLicense(teamlic);
                }
                teamLicList.Remove(obj);
            }
            return true;
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
    }
}
