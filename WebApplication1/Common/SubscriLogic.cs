using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.MetCalWeb.Models;
using License.Logic.ServiceLogic;

namespace License.MetCalWeb.Common
{
    public class SubscriLogic
    {
        public static void GetUserLicenseForUser()
        {
            string userId = LicenseSessionState.Instance.User.UserId;
            LicenseSessionState.Instance.ProductLicense = GetUserLicenseDetails(userId, true);
        }

        /// <summary>
        /// Function to get the Product Subscription based on the Admin Id  with count for which admin subscribed. UserId is the Admin UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IList<SubscriptionProductModel> GetSubscription(string userId)
        {
            IList<License.MetCalWeb.Models.SubscriptionProductModel> subscriptionProList = new List<License.MetCalWeb.Models.SubscriptionProductModel>();
            UserSubscriptionLogic subscriptionLogic = null;
            subscriptionLogic = new UserSubscriptionLogic();
            ProductSubscriptionLogic proSubLogic = new ProductSubscriptionLogic();
            var dataList = proSubLogic.GetSubscriptionFromFile();
            var subscriptionList = subscriptionLogic.GetSubscription(userId);
            foreach (var userSub in subscriptionList)
            {
                var subType = dataList.FirstOrDefault(s => s.Id == userSub.SubscriptionId);
                SubscriptionProductModel model = new SubscriptionProductModel();
                model.SubscriptionId = subType.Id;
                model.SubscriptionName = subType.SubscriptionName;
                foreach (var pro in subType.Product)
                {
                    UserLicenseLogic userLicLogic = new UserLicenseLogic();
                    int usedLicCount = userLicLogic.GetUserLicenseCount(userSub.Id, pro.Id);
                    model.ProductDtls.Add(new ProductDetails() { ProductId = pro.Id, ProductName = pro.Name, ProductCode = pro.ProductCode, TotalCount = (pro.QtyPerSubscription * userSub.Quantity), UsedLicenseCount = usedLicCount });
                }
                subscriptionProList.Add(model);
            }
            LicenseSessionState.Instance.SubscriptionList = subscriptionProList;
            return subscriptionProList;
        }

        /// <summary>
        /// Function to get the User License with details  for which user is authorized
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isFeatureRequired"></param>
        /// <returns></returns>
        public static List<LicenseMapModel> GetUserLicenseDetails(string userId, bool isFeatureRequired)
        {
            var licenseMapModelList = new List<LicenseMapModel>();
            UserLicenseLogic logic = new UserLicenseLogic();
            var data = logic.GetUserLicense(userId);
            ProductSubscriptionLogic proSubLogic = new ProductSubscriptionLogic();
            var dataList = proSubLogic.GetSubscriptionFromFile();
            if (data.Count > 0)
            {
                var subIdList = data.Select(ul => ul.License.UserSubscriptionId);
                UserSubscriptionLogic subscriptionLogic = new UserSubscriptionLogic();
                var userSubscriptionList = subscriptionLogic.GetSubscriptionByIDList(subIdList.ToList());
                var subscriptionIdList = userSubscriptionList.Select(s => s.SubscriptionId);
                var subscriptionList = dataList.Where(s => subscriptionIdList.Contains(s.Id)).ToList();
                foreach (var subs in subscriptionList)
                {
                    var proList = data.Where(ul => ul.License.Subscription.SubscriptionId == subs.Id).ToList().Select(u => u.License.ProductId).ToList();
                    LicenseMapModel mapModel = new LicenseMapModel();
                    mapModel.SubscriptionName = subs.SubscriptionName;
                    mapModel.UserSubscriptionId = userSubscriptionList.FirstOrDefault(us => us.SubscriptionId == subs.Id).Id;

                    foreach (var pro in subs.Product.Where(p => proList.Contains(p.Id)))
                    {
                        SubscriptionProduct prod = new SubscriptionProduct();
                        prod.ProductId = pro.Id;
                        prod.ProductName = pro.Name;
                        if (!isFeatureRequired)
                        {
                            var tempsub = LicenseSessionState.Instance.SubscriptionList.FirstOrDefault(s => s.SubscriptionId == subs.Id);
                            var tempPro = tempsub == null ? null : tempsub.ProductDtls.FirstOrDefault(p => p.ProductId == prod.ProductId);
                            if (tempPro != null)
                                prod.IsDisabled = tempPro.AvailableCount == 0;
                        }
                        else
                        {
                            foreach(var fet in pro.Features)
                            {
                                var feature = new Feature();
                                feature.Id = fet.Id;
                                feature.Name = fet.Name;
                                feature.Description = fet.Description;
                                prod.Features.Add(feature);
                            }
                        }
                        mapModel.ProductList.Add(prod);
                    }
                    licenseMapModelList.Add(mapModel);
                }
            }
            return licenseMapModelList;
        }

        public static List<LicenseMapModel> GetSubForLicenseMap(string userId, string adminUserId)
        {
            var licenseMapModelList = new List<LicenseMapModel>();
            UserLicenseLogic logic = new UserLicenseLogic();
            var data = logic.GetUserLicense(userId);

            UserSubscriptionLogic subscriptionLogic = new UserSubscriptionLogic();
            var userSubscriptionList = subscriptionLogic.GetSubscription(adminUserId);
            var subscriptionIdList = userSubscriptionList.Select(s => s.SubscriptionId).ToList();
            var subscriptionList = LicenseSessionState.Instance.SubscriptionList.Where(s => subscriptionIdList.Contains(s.SubscriptionId)).ToList();

            foreach (var subs in subscriptionList)
            {
                LicenseMapModel mapModel = new LicenseMapModel();
                mapModel.SubscriptionName = subs.SubscriptionName;
                mapModel.UserSubscriptionId = userSubscriptionList.FirstOrDefault(us => us.SubscriptionId == subs.SubscriptionId).Id;

                foreach (var pro in subs.ProductDtls)
                {
                    SubscriptionProduct prod = new SubscriptionProduct();
                    prod.ProductId = pro.ProductId;
                    prod.ProductName = pro.ProductName;
                    prod.IsDisabled = pro.AvailableCount == 0;
                    mapModel.ProductList.Add(prod);
                }
                licenseMapModelList.Add(mapModel);
            }
            foreach (var obj in data)
            {
                var subObj = licenseMapModelList.FirstOrDefault(f => f.UserSubscriptionId == obj.License.UserSubscriptionId);
                if (subObj != null)
                {
                    var pro = subObj.ProductList.FirstOrDefault(f => f.ProductId == obj.License.ProductId);
                    if (pro != null)
                        pro.IsSelected = true;
                    pro.InitialState = true;
                }
            }
            return licenseMapModelList;
        }

    }
}