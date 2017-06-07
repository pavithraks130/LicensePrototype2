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

        public void UpdateUserSubscription(SubscriptionList userSubscriptionData)
        {
            List<Subscription> typeList = new List<Subscription>();
            foreach (var data in userSubscriptionData.Subscriptions)
            {
                var tempObj = data.Subscription;
                tempObj.ProductIdList = tempObj.Products.Select(p => (dynamic)new { Id = p.Id, ProductCode = p.ProductCode }).ToList();
                typeList.Add(tempObj);

                UserSubscription sub = new UserSubscription();
                sub.Quantity = data.OrderdQuantity;
                sub.SubscriptionDate = data.SubscriptionDate;
                sub.RenewalDate = data.RenewalDate;
                sub.SubscriptionId = data.SubscriptionId;
                sub.UserId = userSubscriptionData.UserId;
                sub.ExpireDate = data.ExpireDate;
                sub.ServerUserSubscriptionId = data.UserSubscriptionId;
                var userSubscriptionId = userSubLogic.CreateSubscription(sub);

                List<License.DataModel.ProductLicense> licenseDataList = new List<DataModel.ProductLicense>();
                foreach (var lic in data.LicenseKeyProductMapping)
                {
                    License.DataModel.ProductLicense licenseData = new DataModel.ProductLicense()
                    {
                        LicenseKey = lic.LicenseKey,
                        ProductId = lic.ProductId,
                        UserSubscriptionId = userSubscriptionId
                    };
                    licenseDataList.Add(licenseData);
                }
                ProductLicenseLogic licenseLogic = new ProductLicenseLogic();
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

        public List<Subscription> GetSubscriptionList(string adminId, string userId = "")
        {
            List<Subscription> lstSsubscriptionDetail = new List<Subscription>();
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
                    Subscription model = new Subscription()
                    {
                        Id = subType.Id,
                        UserSubscriptionId = userSub.Id,
                        Name = subType.Name
                    };
                    model.Products = new List<Product>();
                    ProductLicenseLogic licDataLogic = new ProductLicenseLogic();
                    var userlicList = licDataLogic.GetLicenseList(userSub.Id);

                    foreach (var pro in subType.Products)
                    {
                        if (!proList.Contains(pro.Id))
                        {
                            var licList = userlicList.Where(p => p.ProductId == pro.Id).ToList();
                            var proObj = new Product() { Id = pro.Id, Name = pro.Name, ProductCode = pro.ProductCode, TotalLicenseCount = licList.Count, UsedLicenseCount = licList.Where(l => l.IsMapped == true).Count() };
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
                var data = logic.GetUserLicenseByUserId(userId);
                if(data != null && data.Count >0)
                    data = data.Where(p => p.IsTeamLicense == false).ToList();
                foreach (var obj in data)
                {
                    var subObj = lstSsubscriptionDetail.FirstOrDefault(f => f.UserSubscriptionId == obj.License.UserSubscriptionId);
                    if (subObj != null)
                    {
                        var pro = subObj.Products.FirstOrDefault(f => f.Id == obj.License.ProductId);
                        if (pro != null)
                        {
                            pro.IsSelected = true;
                            pro.InitialState = true;
                        }
                    }
                }
            }
            return lstSsubscriptionDetail;
        }

        public void UpdateSubscriptionRenewal(SubscriptionList subscriptionData, string userId)
        {
            var userSubscriptioList = userSubLogic.GetSubscription(userId);
            List<UserSubscription> userSubscriptions = new List<UserSubscription>();
            foreach (var data in subscriptionData.Subscriptions)
            {
                var userSubscription = userSubscriptioList.FirstOrDefault(u => u.ServerUserSubscriptionId == data.UserSubscriptionId);
                ProductLicenseLogic licenseLogic = new ProductLicenseLogic();
                licenseLogic.UpdateRenewalLicenseKeys(data.LicenseKeyProductMapping, userSubscription.Id);
                userSubscription.RenewalDate = data.RenewalDate;
                userSubscription.ExpireDate = data.ExpireDate;
                userSubscriptions.Add(userSubscription);
            }
            if (userSubscriptions.Count > 0)
                userSubLogic.UpdateSubscriptions(userSubscriptions);
        }


    }
}
