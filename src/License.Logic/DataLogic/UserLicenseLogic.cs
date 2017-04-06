using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;

namespace License.Logic.DataLogic
{
    public class UserLicenseLogic : BaseLogic
    {
        private bool CreateUserLicense(UserLicense lic)
        {
            var obj = AutoMapper.Mapper.Map<UserLicense, Core.Model.UserLicense>(lic);
            obj = Work.UserLicenseRepository.Create(obj);
            return obj.Id > 0;
        }


        /// <summary>
        /// creating the UserLicense this can be used to update the single user.
        /// </summary>
        /// <param name="licList"></param>
        /// <returns></returns>
        public bool CreataeUserLicense(List<UserLicense> licList)
        {
            LicenseLogic licLogic = new LicenseLogic();
            int i = 0;
            foreach (var lic in licList)
            {
                var userLicList = Work.UserLicenseRepository.GetData(ul => ul.UserId == lic.UserId).ToList();
                var data = Work.LicenseDataRepository.GetData(l => l.ProductId == lic.License.ProductId && l.UserSubscriptionId == lic.License.UserSubscriptionId).ToList().Select(l => l.Id);
                var obj = userLicList.FirstOrDefault(ul => data.Contains(ul.LicenseId) && ul.UserId == lic.UserId);
                if (obj == null)
                {
                    i++;
                    UserLicense ul = new UserLicense();
                    ul.UserId = lic.UserId;
                    ul.LicenseId = licLogic.GetUnassignedLicense(lic.License.UserSubscriptionId, lic.License.ProductId).Id;
                    CreateUserLicense(ul);
                }
                userLicList.Remove(obj);
            }
            if (i > 0)
                Work.UserLicenseRepository.Save();
            return true;
        }

        /// <summary>
        /// function to create the License  for multiple User . This function will be used for bulk license mapping to 
        /// multiple User.
        /// </summary>
        /// <param name="licList"></param>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        public bool CreateMultiUserLicense(List<LicenseData> licList, List<string> userIdList)
        {
            LicenseLogic licLogic = new LicenseLogic();
            int i = 0;

            foreach (var userId in userIdList)
            {
                var userLicList = Work.UserLicenseRepository.GetData(ul => ul.UserId == userId).ToList();
                foreach (var lic in licList)
                {
                    var data = Work.LicenseDataRepository.GetData(l => l.ProductId == lic.ProductId && l.UserSubscriptionId == lic.UserSubscriptionId).ToList().Select(l => l.Id);
                    var obj = userLicList.FirstOrDefault(ul => data.Contains(ul.LicenseId) && ul.UserId == userId);
                    if (obj == null)
                    {
                        i++;
                        UserLicense ul = new UserLicense();
                        ul.UserId = userId;
                        ul.LicenseId = licLogic.GetUnassignedLicense(lic.UserSubscriptionId, lic.ProductId).Id;
                        CreateUserLicense(ul);
                    }
                    userLicList.Remove(obj);
                }

                if (i > 0)
                    Work.UserLicenseRepository.Save();
            }
            return true;
        }

        private bool RevokeUserLicense(UserLicense lic)
        {
            var obj = Work.UserLicenseRepository.GetData(r => r.LicenseId == lic.LicenseId && r.UserId == lic.UserId).FirstOrDefault();
            if (obj == null)
                return false;
            var obj1 = Work.UserLicenseRepository.Delete(obj);
            return obj1 != null;
        }

        public bool RevokeUserLicense(List<LicenseData> licList, List<string> userList)
        {
            int i = 0;
            LicenseLogic licLogic = new LicenseLogic();
            foreach (var userId in userList)
            {
                var licdata = GetUserLicense(userId);
                foreach (var lic in licList)
                {
                    var obj = licdata.FirstOrDefault(l => l.License.ProductId == lic.ProductId && l.License.UserSubscriptionId == lic.UserSubscriptionId);
                    RevokeUserLicense(obj);
                    i++;
                }
                if (i > 0)
                    Work.UserLicenseRepository.Save();
            }
            return true;
        }

        public int GetUserLicenseCount(int userSubscriptionId, int productId)
        {
            var licenseIdList = Work.LicenseDataRepository.GetData(l => l.UserSubscriptionId == userSubscriptionId && l.ProductId == productId).Select(l => l.Id).ToList();
            return Work.UserLicenseRepository.GetData(ul => licenseIdList.Contains(ul.LicenseId)).Count();
        }

        public List<UserLicense> GetUserLicense(string userId)
        {
            List<UserLicense> licenses = new List<UserLicense>();
            var datas = Work.UserLicenseRepository.GetData(l => l.UserId == userId);
            foreach (var data in datas)
                licenses.Add(AutoMapper.Mapper.Map<Core.Model.UserLicense, UserLicense>(data));
            return licenses;
        }

    }
}
