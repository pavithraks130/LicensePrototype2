using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;
using License.Logic.DataLogic;
using License.Logic.Common;
using System.Dynamic;

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
                var tempObj = data.Subscription.SubscriptionType;
                tempObj.ProductIdList = tempObj.Products.Select(p => (dynamic)new  { Id = p.Id, ProductCode = p.ProductCode }).ToList();
                typeList.Add(tempObj);

                UserSubscription sub = new UserSubscription();
                sub.Quantity = data.Quantity;
                sub.SubscriptionDate = data.SubscriptionDate;
                sub.SubscriptionId = data.SubscriptionId;
                sub.UserId = data.UserId;
                var userSubscriptionId = userSubLogic.CreateSubscription(sub);

                List<License.DataModel.LicenseData> licenseDataList = new List<DataModel.LicenseData>();
                foreach (var lic in data.LicenseKeys)
                {
                    License.DataModel.LicenseData licenseData = new DataModel.LicenseData()
                    {
                        LicenseKey = lic.LicenseKey,
                        ProductId = lic.ProductId,
                        UserSubscriptionId = userSubscriptionId
                    };
                    licenseDataList.Add(licenseData);
                }
                LicenseLogic licenseLogic = new LicenseLogic();
                licenseLogic.CreateLicenseData(licenseDataList);
            }

            if (typeList.Count > 0)
            {
                Logic.BusinessLogic.ProductSubscriptionLogic proSubLogic = new Logic.BusinessLogic.ProductSubscriptionLogic();
                proSubLogic.SaveToFile(typeList);
            }

        }

        public List<SubscriptionDetails> GetSubscriptionList(string adminId, string userId = "")
        {
            List<SubscriptionDetails> lstSsubscriptionDetail = new List<SubscriptionDetails>();
            var usersubList = userSubLogic.GetSubscription(adminId);
            ProductSubscriptionLogic psLogic = new ProductSubscriptionLogic();
            var subList = psLogic.GetSubscriptionFromFile();
            foreach (var userSub in usersubList)
            {
                var subType = subList.FirstOrDefault(s => s.Id == userSub.SubscriptionId);
                if (subType != null)
                {
                    SubscriptionDetails model = new SubscriptionDetails()
                    {
                        Id = subType.Id,
                        UserSubscriptionId = userSub.Id,
                        Name = subType.Name
                    };
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
