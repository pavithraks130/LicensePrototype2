using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;

namespace License.Logic.DataLogic
{
    /// <summary>
    /// History 
    /// Create By :
    /// Created Date :
    /// Purpose : 1. CRUD operation on UserLicense Table.
    ///           2. Based on the User license Removal respective Team License or product license status will be updated.
    /// </summary>
    public class UserLicenseLogic : BaseLogic
    {
        TeamLicenseLogic teamLicLogic = null;
        ProductLicenseLogic licLogic = null;

        public UserLicenseLogic()
        {
            teamLicLogic = new TeamLicenseLogic();
            licLogic = new ProductLicenseLogic();
        }
        /// Assigning the license to user and updating the License  status  to update the Mapped property
        private bool CreateUserLicense(UserLicense lic)
        {
            var obj = AutoMapper.Mapper.Map<UserLicense, Core.Model.UserLicense>(lic);
            obj = Work.UserLicenseRepository.Create(obj);
            Work.UserLicenseRepository.Save();
            // Update the Product Liccense  by setting IsMapped true.
            ProductLicenseLogic licLogic = new ProductLicenseLogic();
            licLogic.UpdateLicenseStatus(obj.LicenseId, true);
            return obj.Id > 0;
        }



        /// creating the UserLicense this can be used to update the single user.
        public bool CreataeUserLicense(List<UserLicense> licList)
        {
            foreach (var lic in licList)
            {
                var userLicList = Work.UserLicenseRepository.GetData(ul => ul.UserId == lic.UserId).ToList();
                var data = Work.ProductLicenseRepository.GetData(l => l.ProductId == lic.License.ProductId && l.UserSubscriptionId == lic.License.UserSubscriptionId).ToList().Select(l => l.Id);
                var obj = userLicList.FirstOrDefault(ul => data.Contains(ul.LicenseId));
                if (obj == null)
                {
                    var licId = licLogic.GetUnassignedLicense(lic.License.ProductId).Id;
                    UserLicense ul = new UserLicense()
                    {
                        UserId = lic.UserId,
                        LicenseId = licId,
                        TeamId = lic.TeamId
                    };
                    CreateUserLicense(ul);
                }
                userLicList.Remove(obj);
            }
            return true;
        }


        /// function to create the License  for multiple User . This function will be used for bulk license mapping to user. If the license is already mapped then it won't  readd the license
        public bool CreateMultiUserLicense(UserLicenseDataMapping model)
        {
            foreach (var user in model.UserList)
            {
                var userLicList = Work.UserLicenseRepository.GetData(ul => ul.UserId == user.UserId).ToList();
                foreach (var lic in model.LicenseDataList)
                {
                    var data = Work.ProductLicenseRepository.GetData(l => l.ProductId == lic.ProductId && l.UserSubscriptionId == lic.UserSubscriptionId).ToList().Select(l => l.Id);
                    var obj = userLicList.FirstOrDefault(ul => data.Contains(ul.LicenseId));
                    if (obj == null)
                    {
                        var licId = licLogic.GetUnassignedLicense(lic.ProductId).Id;
                        UserLicense ul = new UserLicense()
                        {
                            UserId = user.UserId,
                            LicenseId = licId,
                            TeamId = model.TeamId
                        };
                        CreateUserLicense(ul);
                    }
                    userLicList.Remove(obj);
                }

            }
            return true;
        }

        //  Removing the License mapping from user 
        private bool RevokeUserLicense(Core.Model.UserLicense lic)
        {
            var obj = Work.UserLicenseRepository.GetData(r => r.LicenseId == lic.LicenseId && r.UserId == lic.UserId).FirstOrDefault();
            if (obj == null)
                return false;
            var obj1 = Work.UserLicenseRepository.Delete(obj);
            Work.UserLicenseRepository.Save();
            licLogic.UpdateLicenseStatus(obj1.LicenseId, false);
            return obj1 != null;
        }

        //Removing the bulk License from user
        public bool RevokeUserLicense(UserLicenseDataMapping model)
        {
            ProductLicenseLogic licLogic = new ProductLicenseLogic();
            foreach (var user in model.UserList)
            {
                foreach (var lic in model.LicenseDataList)
                {
                    var obj = Work.UserLicenseRepository.GetData(l => l.UserId == user.UserId && l.License.ProductId == lic.ProductId && l.License.UserSubscriptionId == lic.UserSubscriptionId && l.TeamId == model.TeamId).FirstOrDefault();
                    RevokeUserLicense(obj);
                }
            }
            return true;
        }

        //public int GetUserLicenseCount(int userSubscriptionId, int productId)
        //{
        //    var licenseIdList = Work.ProductLicenseRepository.GetData(l => l.UserSubscriptionId == userSubscriptionId && l.ProductId == productId).Select(l => l.Id).ToList();
        //    return Work.UserLicenseRepository.GetData(ul => licenseIdList.Contains(ul.LicenseId)).Count();
        //}

        /// Get user licenses by User Id
        public List<UserLicense> GetUserLicense(string userId)
        {
            List<UserLicense> licenses = new List<UserLicense>();
            var datas = Work.UserLicenseRepository.GetData(l => l.UserId == userId && l.IsTeamLicense == false);
            licenses = datas.Select(data => AutoMapper.Mapper.Map<UserLicense>(data)).ToList();
            return licenses;
        }

        /// Get User license by Team id and User Id.
        public List<UserLicense> GetUserLicense(string userId, int teamId)
        {
            List<UserLicense> licenses = new List<UserLicense>();
            var datas = Work.UserLicenseRepository.GetData(l => l.UserId == userId && l.TeamId == teamId);
            licenses = datas.Select(data => AutoMapper.Mapper.Map<UserLicense>(data)).ToList();
            return licenses;
        }

        // Assign  team license  to user based on the teamID 
        public void AssignTeamLicenseToUser(int teamId, string userId)
        {
            var teamLicData = teamLicLogic.GetTeamLicense(teamId);
            var userLicData = Work.UserLicenseRepository.GetData(l => l.UserId == userId && l.TeamId == teamId).ToList();
            if (teamLicData == null)
                return;
            var productList = teamLicData.Select(l => l.ProductId).Distinct();
            foreach (var proId in productList)
            {
                var mappedlic = teamLicData.Where(l => l.ProductId == proId && l.IsMapped == true).ToList().Select(l => l.Id).ToList();
                var data = teamLicData.FirstOrDefault(l => l.ProductId == proId && l.IsMapped == false);
                var licMapped = userLicData.Any(l => mappedlic.Contains(l.TeamLicenseId));
                if (licMapped == false && data != null)
                {
                    UserLicense lic = new UserLicense()
                    {
                        LicenseId = data.LicenseId,
                        TeamId = teamId,
                        UserId = userId,
                        IsTeamLicense = true,
                        TeamLicenseId = data.Id
                    };
                    CreateUserLicense(lic);
                    data.IsMapped = true;
                    teamLicLogic.UpdateLicenseStatus(data.Id, true);
                }
            }
        }

        /// Revoke License from user, data is retrived based on the user Id and Team Id
        public void RevokeTeamLicenseFromUser(string userId, int teamId)
        {
            var licenseList = Work.UserLicenseRepository.GetData(u => u.UserId == userId && u.TeamId == teamId && u.IsTeamLicense == true).ToList();
            if (licenseList == null)
                return;
            foreach (var lic in licenseList)
            {
                teamLicLogic.UpdateLicenseStatus(lic.TeamLicenseId, false);
                Work.UserLicenseRepository.Delete(lic);
                Work.UserLicenseRepository.Save();
            }
        }

        //Revoking the team License from User based on userId
        public void RevokeTeamLicenseFromUser(string userId)
        {
            var teamLicenseList = Work.UserLicenseRepository.GetData(u => u.UserId == userId && u.IsTeamLicense == true).ToList();
            foreach (var teamLic in teamLicenseList)
            {
                teamLicLogic.UpdateLicenseStatus(teamLic.TeamLicenseId, false);
                Work.UserLicenseRepository.Delete(teamLic);
                Work.UserLicenseRepository.Save();
            }
        }

        /// Revoking Team License from user based on the teamLicenseId
        public void RevokeTeamLicenseFromUser(int teamLicenseId)
        {
            var lic = Work.UserLicenseRepository.GetData(l => l.TeamLicenseId == teamLicenseId).ToList();
            if (lic != null)
            {
                var selectedLic = lic.FirstOrDefault();
                Work.UserLicenseRepository.Delete(selectedLic);
                Work.UserLicenseRepository.Save();
            }
        }
    }
}
