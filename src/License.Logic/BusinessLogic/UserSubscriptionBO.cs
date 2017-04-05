using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;
using License.Logic.DataLogic;
using License.Logic.Common;

namespace License.Logic.BusinessLogic
{
    public class UserSubscriptionBO
    {
        UserSubscriptionLogic userSubLogic = null;
        public UserSubscriptionBO()
        {
            userSubLogic = new UserSubscriptionLogic();
        }

        public void UpdateUserSubscription(List<UserSubscriptionData> userSubscriptionData)
        {
            List<SubscriptionType> typeList = new List<SubscriptionType>();
            foreach (var data in userSubscriptionData)
            {
                typeList.Add(data.Subscription.SubscriptionType);

                UserSubscription sub = new UserSubscription();
                sub.Quantity = data.Quantity;
                sub.SubscriptionDate = data.SubscriptionDate;
                sub.SubscriptionId = data.SubscriptionId;
                sub.UserId = data.UserId;
                var userSubscriptionId = userSubLogic.CreateSubscription(sub);

                List<License.DataModel.LicenseData> licenseDataList = new List<DataModel.LicenseData>();
                foreach (var lic in data.LicenseKeys)
                {
                    License.DataModel.LicenseData licenseData = new DataModel.LicenseData();
                    licenseData.LicenseKey = lic.LicenseKey;
                    licenseData.ProductId = lic.ProductId;
                    licenseData.UserSubscriptionId = userSubscriptionId;
                    licenseDataList.Add(licenseData);
                }
                LicenseLogic licenseLogic = new LicenseLogic();
                licenseLogic.CreateLicenseData(licenseDataList);
            }

            if(typeList.Count > 0)
            {
                Logic.DataLogic.ProductSubscriptionLogic proSubLogic = new Logic.DataLogic.ProductSubscriptionLogic();
                proSubLogic.SaveToFile(typeList);
            }

        }
    }
}
