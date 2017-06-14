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
        /// <summary>
        ///  Assigning the license to user and updating the License  status  to update the Mapped property
        /// </summary>
        /// <param name="lic"></param>
        /// <returns></returns>
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



        /// <summary>
        /// creating the UserLicense this can be used to update the single user.
        /// </summary>
        /// <param name="licList"></param>
        /// <returns></returns>
        public bool CreataeUserLicense(List<UserLicense> licList)
        {
            TeamLogic teamLogic = new TeamLogic();
            foreach (var lic in licList)
            {
                // get the User subscription List  which are expired for the Admin
                var team = teamLogic.GetTeamById(lic.TeamId);
                UserSubscriptionLogic userSubscriptionLogic = new UserSubscriptionLogic();
                var userSubscriptionList = userSubscriptionLogic.GetSubscription(team.AdminId);

                // Fetches the Subscription Id Which are  expired and get the ID List
                userSubscriptionList = userSubscriptionList.Where(l => (l.ExpireDate.Date - DateTime.Today).Days < 0).ToList();
                List<int> userSubIds = new List<int>();
                userSubIds.AddRange(userSubscriptionList.Select(us => us.Id).ToList());

                // Getting existing user license
                var userLicList = Work.UserLicenseRepository.GetData(ul => ul.UserId == lic.UserId).ToList();
                // Getting Product License based on  the user subscription Id and Product id
                var data = Work.ProductLicenseRepository.GetData(l => l.ProductId == lic.License.ProductId && !userSubIds.Contains(l.UserSubscriptionId)).ToList().Select(l => l.Id);
                //fetching the user License  which matches the  Product license Id 
                var obj = userLicList.FirstOrDefault(ul => data.Contains(ul.LicenseId));
                // if the record does not exist then user license record will be created for the Product
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


        /// <summary>
        /// function to create the License  for multiple User . This function will be used for bulk license mapping to user. 
        /// If the license is already mapped then it won't  readd the license
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreateMultiUserLicense(UserLicenseDataMapping model)
        {
            UserSubscriptionLogic userSubscriptionLogic = new UserSubscriptionLogic();
            var userSubscriptionList = userSubscriptionLogic.GetSubscription(model.AdminID);

            // Fetches the Subscription Id Which are  expired and get the ID List
            userSubscriptionList = userSubscriptionList.Where(l => (l.ExpireDate.Date - DateTime.Today).Days < 0).ToList();
            List<int> userSubIds = new List<int>();
            userSubIds.AddRange(userSubscriptionList.Select(us => us.Id).ToList());

            foreach (var user in model.UserList)
            {
                var userLicList = Work.UserLicenseRepository.GetData(ul => ul.UserId == user.UserId).ToList();
                foreach (var lic in model.LicenseDataList)
                {
                    var licenseDataList = Work.ProductLicenseRepository.GetData(l => l.ProductId == lic.ProductId && !userSubIds.Contains(l.UserSubscriptionId)).ToList();
                    var data = licenseDataList.Select(l => l.Id);
                    var obj = userLicList.FirstOrDefault(ul => data.Contains(ul.LicenseId));
                    if (obj == null)
                    {
                        var licObj = licenseDataList.FirstOrDefault(l => l.ProductId == lic.ProductId && l.IsMapped == false);
                        UserLicense ul = new UserLicense()
                        {
                            UserId = user.UserId,
                            LicenseId = licObj.Id,
                            TeamId = model.TeamId
                        };
                        CreateUserLicense(ul);
                    }
                    userLicList.Remove(obj);
                }

            }
            return true;
        }

        /// <summary>
        /// Removing the License mapping from user 
        /// </summary>
        /// <param name="lic"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Removing the bulk License from user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RevokeUserLicense(UserLicenseDataMapping model)
        {
            ProductLicenseLogic licLogic = new ProductLicenseLogic();
            foreach (var user in model.UserList)
            {
                foreach (var lic in model.LicenseDataList)
                {
                    var obj = Work.UserLicenseRepository.GetData(l => l.UserId == user.UserId && l.License.ProductId == lic.ProductId && l.TeamId == model.TeamId).FirstOrDefault();
                    RevokeUserLicense(obj);
                }
            }
            return true;
        }

        /// <summary>
        /// Get user licenses by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserLicense> GetUserLicenseByUserId(string userId)
        {
            List<UserLicense> licenses = new List<UserLicense>();
            var datas = Work.UserLicenseRepository.GetData(l => l.UserId == userId && l.IsTeamLicense == false);
            licenses = datas.Select(data => AutoMapper.Mapper.Map<UserLicense>(data)).ToList();
            return licenses;
        }

        /// <summary>
        /// Get User license by Team id and User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public List<UserLicense> GetUserLicenseByUserIdTeamId(string userId, int teamId)
        {
            List<UserLicense> licenses = new List<UserLicense>();
            var datas = Work.UserLicenseRepository.GetData(l => l.UserId == userId && l.TeamId == teamId);
            licenses = datas.Select(data => AutoMapper.Mapper.Map<UserLicense>(data)).ToList();
            return licenses;
        }

        /// <summary>
        /// Assign  team license  to user based on the teamID 
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        public void AssignTeamLicenseToUser(int teamId, string userId)
        {
            // Get Team License By TeamId
            var teamLicData = teamLicLogic.GetTeamLicense(teamId);
            // Get User License Data based on the userId
            var userLicData = Work.UserLicenseRepository.GetData(l => l.UserId == userId && l.TeamId == teamId).ToList();
            if (teamLicData == null)
                return;
            // Getting list of Products from team License Data
            var productList = teamLicData.Select(l => l.ProductId).Distinct();
            //  Adding each product Team License to User
            foreach (var proId in productList)
            {
                // Fetching the mapped Team License Id
                var mappedlic = teamLicData.Where(l => l.ProductId == proId && l.IsMapped == true).ToList().Select(l => l.Id).ToList();
                // fetching unmapped license record
                var data = teamLicData.FirstOrDefault(l => l.ProductId == proId && l.IsMapped == false);
                // checking any of the team License is mapped to the User for the product
                var licMapped = userLicData.Any(l => mappedlic.Contains(l.TeamLicenseId));                
                // If No license mapped and if any Team License Exist then the License will be assigned to user
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

        /// <summary>
        /// Revoke License from user, data is retrived based on the user Id and Team Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamId"></param>
        public void RevokeTeamLicenseFromUser(string userId, int teamId)
        {
            // Getting all the team License based on the team ID
            var licenseList = Work.UserLicenseRepository.GetData(u => u.UserId == userId && u.TeamId == teamId && u.IsTeamLicense == true).ToList();
            if (licenseList == null)
                return;
            // Removing from the user License and updating Is mapped property to false in Team License to make the license is available for the next user
            foreach (var lic in licenseList)
            {
                teamLicLogic.UpdateLicenseStatus(lic.TeamLicenseId, false);
                Work.UserLicenseRepository.Delete(lic);
                Work.UserLicenseRepository.Save();
            }
        }

        /// <summary>
        /// Revoking the team License from User based on userId
        /// </summary>
        /// <param name="userId"></param>
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

        /// <summary>
        /// Revoking Team License from user based on the teamLicenseId
        /// </summary>
        /// <param name="teamLicenseId"></param>
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
