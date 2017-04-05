using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;
using System.IO;
using Newtonsoft.Json;

namespace License.Logic.DataLogic
{
    public class ProductSubscriptionLogic
    {

        public void SaveToFile(List<SubscriptionType> subscriptions)
        {

            List<SubscriptionType> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("SubscriptionDetails.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("SubscriptionDetails.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<SubscriptionType>>(existingData);
            }
            else
                subscriptionList = new List<SubscriptionType>();
            bool isDataModified = false;
            foreach (var sub in subscriptions)
            {
                if (!subscriptionList.Any(s => s.Id == sub.Id))
                {
                    isDataModified = true;
                    subscriptionList.Add(sub);
                }
            }
            if (isDataModified)
            {
                var data = JsonConvert.SerializeObject(subscriptionList);
                Common.CommonFileIO.SaveDatatoFile(data, "productSubscription.txt");
            }
        }

        public List<SubscriptionType> GetSubscriptionFromFile()
        {
            List<SubscriptionType> subscriptionList;
            if (Common.CommonFileIO.IsFileExist("SubscriptionDetails.txt"))
            {
                var existingData = Common.CommonFileIO.GetJsonDataFromFile("SubscriptionDetails.txt");
                subscriptionList = JsonConvert.DeserializeObject<List<SubscriptionType>>(existingData);
            }
            else
                subscriptionList = new List<SubscriptionType>();
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
                DateTime licExpireData = DateTime.MinValue;
                foreach (var subs in subscriptionList)
                {

                    var userLicLicst = data.Where(ul => ul.License.Subscription.SubscriptionId == subs.Id).ToList();
                    var proList = userLicLicst.Select(u => u.License.ProductId).ToList();
                    LicenseMapModel mapModel = new LicenseMapModel();
                    mapModel.SubscriptionName = subs.SubscriptionName;
                    mapModel.UserSubscriptionId = userSubscriptionList.FirstOrDefault(us => us.SubscriptionId == subs.Id).Id;

                    foreach (var pro in subs.Product.Where(p => proList.Contains(p.Id)))
                    {
                        var objLic = userLicLicst.FirstOrDefault(f => f.License.ProductId == pro.Id);
                        if (objLic != null)
                        {
                            string licenseKeydata = String.Empty;
                            licenseKeydata = objLic.License.LicenseKey;
                            var splitData = licenseKeydata.Split(new char[] { '-' });
                            var datakey = splitData[0];
                            var decryptObj = LicenseKey.LicenseKeyGen.CryptoEngine.Decrypt(datakey, true);
                            var licdataList = decryptObj.Split(new char[] { '^' });
                            licExpireData = Convert.ToDateTime(licExpireData);

                        }

                        SubscriptionProduct prod = new SubscriptionProduct();
                        prod.ProductId = pro.Id;
                        prod.ProductName = pro.Name;
                        prod.ExpireDate = licExpireData;
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
