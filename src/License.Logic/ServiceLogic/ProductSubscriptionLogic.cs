using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;
using System.IO;
using Newtonsoft.Json;
using License.Model;

namespace License.Logic.ServiceLogic
{
    public class ProductSubscriptionLogic
    {
        public void SaveToFile(Subscription subs)
        {
            List<Subscription> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("productSubscription.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("productSubscription.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<Subscription>>(existingData);
            }
            else
                subscriptionList = new List<Subscription>();
            if (!subscriptionList.Any(s => s.Id == subs.Id))
            {
                subscriptionList.Add(subs);
                var data = JsonConvert.SerializeObject(subscriptionList);
                Common.CommonFileIO.SaveDatatoFile(data, "productSubscription.txt");
            }
        }

        public List<Subscription> GetSubscriptionFromFile()
        {
            List<Subscription> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("productSubscription.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("productSubscription.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<Subscription>>(existingData);
            }
            else
                subscriptionList = new List<Subscription>();
            return subscriptionList;

        }
        public List<LicenseMapModel> GetUserLicenseDetails(string userId, bool isFeatureRequired)
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
                        foreach (var fet in pro.Features)
                        {
                            var feature = new Feature();
                            feature.Id = fet.Id;
                            feature.Name = fet.Name;
                            feature.Description = fet.Description;
                            feature.Version = fet.Version;
                            prod.Features.Add(feature);
                        }
                        mapModel.ProductList.Add(prod);
                    }
                    licenseMapModelList.Add(mapModel);
                }
            }
            return licenseMapModelList;
        }
    }
}
