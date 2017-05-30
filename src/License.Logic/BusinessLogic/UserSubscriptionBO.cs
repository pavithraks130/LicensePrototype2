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
                tempObj.ProductIdList = tempObj.Products.Select(p => (dynamic)new { Id = p.Id, ProductCode = p.ProductCode }).ToList();
                typeList.Add(tempObj);

                UserSubscription sub = new UserSubscription();
                sub.Quantity = data.Quantity;
                sub.SubscriptionDate = data.SubscriptionDate;
                sub.RenewalDate = data.RenewalDate;
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

            try
            {
                if (typeList.Count > 0)
                {
                    Logic.BusinessLogic.SubscriptionBO proSubLogic = new Logic.BusinessLogic.SubscriptionBO();
                    proSubLogic.SaveToFile(typeList);
                }
            }
            catch (Exception ex)
            {
                Logger.Logger.Error(ex);
            }

        }

        public List<SubscriptionDetails> GetSubscriptionList(string adminId, string userId = "")
        {
            List<SubscriptionDetails> lstSsubscriptionDetail = new List<SubscriptionDetails>();
            var usersubList = userSubLogic.GetSubscription(adminId);
            SubscriptionBO psLogic = new SubscriptionBO();
            var subList = psLogic.GetSubscriptionFromFile();
            List<int> proList = new List<int>();
            if (!String.IsNullOrEmpty(userId))
            {
                bool isAdmin = adminId == userId;
                TeamBO teamBo = new TeamBO();
                proList = teamBo.GetTeamProductByUserId(userId, isAdmin);
            }
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
                    LicenseLogic licDataLogic = new LicenseLogic();
                    var userlicList = licDataLogic.GetLicenseList(userSub.Id);

                    foreach (var pro in subType.Products)
                    {
                        if (!proList.Contains(pro.Id))
                        {
                            var licList = userlicList.Where(p => p.ProductId == pro.Id).ToList();
                            var proObj = new ProductDetails() { Id = pro.Id, Name = pro.Name, ProductCode = pro.ProductCode, TotalLicenseCount = licList.Count, UsedLicenseCount = licList.Where(l => l.IsMapped == true).Count() };
                            proObj.IsDisabled = proObj.TotalLicenseCount == proObj.UsedLicenseCount;
                            model.Products.Add(proObj);
                        }
                    }
                    lstSsubscriptionDetail.Add(model);
                }
            }

            if (!String.IsNullOrEmpty(userId))
            {
                UserLicenseLogic logic = new UserLicenseLogic();
                var data = logic.GetUserLicense(userId);
                if(data != null && data.Count >0)
                    data = data.Where(p => p.IsTeamLicense == false).ToList();
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

        public void UpdateSubscriptionRenewal(List<UserSubscriptionData> subscriptionData, string userId)
        {
            var userSubscriptioList = userSubLogic.GetSubscription(userId);
            List<UserSubscription> userSubscriptions = new List<UserSubscription>();
            foreach (var data in subscriptionData)
            {
                var userSubscription = userSubscriptioList.FirstOrDefault(u => u.SubscriptionId == data.SubscriptionId);
                LicenseLogic licenseLogic = new LicenseLogic();
                licenseLogic.UpdateRenewalLicenseKeys(data.LicenseKeys, userSubscription.Id);
                userSubscription.RenewalDate = data.RenewalDate;
                userSubscriptions.Add(userSubscription);
            }
            if (userSubscriptions.Count > 0)
                userSubLogic.UpdateSubscriptions(userSubscriptions);
        }


    }
}
