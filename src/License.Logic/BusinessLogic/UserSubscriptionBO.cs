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

            if (typeList.Count > 0)
            {
                Logic.DataLogic.ProductSubscriptionLogic proSubLogic = new Logic.DataLogic.ProductSubscriptionLogic();
                proSubLogic.SaveToFile(typeList);
            }

        }

        public List<SubscriptionDetails> GetSubscriptionList(string adminId , string userId = "")
        {
            List<SubscriptionDetails> lstSsubscriptionDetail = new List<SubscriptionDetails>();
            var usersubList = userSubLogic.GetSubscription(adminId);
            var subIdList = usersubList.Select(p => p.SubscriptionId).ToList();
            ProductSubscriptionLogic psLogic = new ProductSubscriptionLogic();
            var subList = psLogic.GetSubscriptionFromFile();
            foreach (var userSub in usersubList)
            {
                var subType = subList.FirstOrDefault(s => s.Id == userSub.SubscriptionId);
                if (subType != null)
                {
                    SubscriptionDetails model = new SubscriptionDetails();
                    model.Id = subType.Id;
                    model.UserSubscriptionId = userSub.Id;
                    model.Name = subType.Name;
                    foreach (var pro in subType.Products)
                    {
                        UserLicenseLogic userLicLogic = new UserLicenseLogic();
                        int usedLicCount = userLicLogic.GetUserLicenseCount(userSub.Id, pro.Id);
                        var proObj = new ProductDetails() { Id = pro.Id, Name = pro.Name, ProductCode = pro.ProductCode, TotalLicenseCount = (pro.Quantity * userSub.Quantity), UsedLicenseCount = usedLicCount };
                        proObj.IsDisabled = proObj.TotalLicenseCount == proObj.UsedLicenseCount;
                        model.Products.Add(proObj);
                    }
                    lstSsubscriptionDetail.Add(model);
                }
            }

            if (!String.IsNullOrEmpty(userId))
            {
                UserLicenseLogic logic = new UserLicenseLogic();
                var data = logic.GetUserLicense(userId);
                foreach (var obj in data)
                {
                    var subObj = lstSsubscriptionDetail.FirstOrDefault(f => f.UserSubscriptionId == obj.License.UserSubscriptionId);
                    if (subObj != null)
                    {
                        var pro = subObj.Products.FirstOrDefault(f => f.Id == obj.License.ProductId);
                        if (pro != null)
                            pro.IsSelected = true;
                        pro.InitialState = true;
                    }
                }
            }
            return lstSsubscriptionDetail;

        }

        
      
    }
}
