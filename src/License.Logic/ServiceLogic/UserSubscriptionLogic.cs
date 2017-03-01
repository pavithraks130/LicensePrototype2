using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model.Model;
namespace License.Logic.ServiceLogic
{
    public class UserSubscriptionLogic : BaseLogic
    {
        public List<UserSubscription> GetSubscription(string userId)
        {
            List<UserSubscription> subscriptionList = new List<UserSubscription>();
            LicenseLogic logic = new LicenseLogic();
            var subsList = Work.UserSubscriptionRepository.GetData(us => us.UserId == userId);
            foreach (var obj in subsList)
            {
                var subObj = AutoMapper.Mapper.Map<Core.Model.UserSubscription, UserSubscription>(obj);
                var licenseList = logic.GetLicenseList(subObj.Id);
                foreach (var lic in licenseList)
                    subObj.LicenseDetails = GetLicenseDetailModel(lic);
                subscriptionList.Add(subObj);
            }
            return subscriptionList;
        }

        public LicenseDetailModel GetLicenseDetailModel(LicenseData lic)
        {
            LicenseDetailModel model = new LicenseDetailModel();
            model.LicenseId = lic.Id;
            model.LicenseKey = lic.LicenseKey;
            var key = model.LicenseKey.Split(new char[] { '-' })[0];
            var data = LicenseKey.LicenseKeyGen.CryptoEngine.Decrypt(key, true);
            var splitData = data.Split(new char[] { '/' });
            model.ProductCode = splitData[0];
            model.TotalLicenseCount = Convert.ToInt32(splitData[1]);
            model.ExpireDate = Convert.ToDateTime(splitData[2]);
            UserLicenseLogic userLicenseLogic = new UserLicenseLogic();
            model.UsedLicenseCount = userLicenseLogic.UserLicenseCount(lic.Id);
            return model;
        }

        public int CreateSubscription(UserSubscription subs)
        {
            var obj = AutoMapper.Mapper.Map<UserSubscription, License.Core.Model.UserSubscription>(subs);
            obj = Work.UserSubscriptionRepository.Create(obj);
            Work.UserSubscriptionRepository.Save();
            return obj.Id;
        }
    }
}
