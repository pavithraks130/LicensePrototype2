using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.MetCalWeb.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace License.MetCalWeb.Common
{
    public class OnPremiseSubscriptionLogic
    {
        public static List<SubscriptionDetails> GetUserLicenseForUser()
        {
            string userId = LicenseSessionState.Instance.User.UserId;
            var userLicDetails = GetUserLicenseDetails(userId, true);
            return userLicDetails.SubscriptionDetails;
        }

        /// <summary>
        /// Function to get the Product Subscription based on the Admin Id  with count for which admin subscribed. UserId is the Admin UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IList<SubscriptionDetails> GetSubscription(string userId)
        {
            IList<SubscriptionDetails> subscriptionProList = new List<SubscriptionDetails>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/UserSubscription/SubscriptionDetils/" + userId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsonData))
                    subscriptionProList = JsonConvert.DeserializeObject<List<SubscriptionDetails>>(jsonData);
            }
            return subscriptionProList;
        }

        /// <summary>
        /// Function to get the User License with details  for which user is authorized
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isFeatureRequired"></param>
        /// <returns></returns>
        public static UserLicenseDetails GetUserLicenseDetails(string userId, bool isFeatureRequired)
        {
            var licenseMapModelList = new UserLicenseDetails();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/License/GetSubscriptionLicense/" + userId + "/" + isFeatureRequired).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                licenseMapModelList = JsonConvert.DeserializeObject<UserLicenseDetails>(jsonData);
            }

            //UserLicenseLogic logic = new UserLicenseLogic();
            //var data = logic.GetUserLicense(userId);
            //ProductSubscriptionLogic proSubLogic = new ProductSubscriptionLogic();
            //var dataList = proSubLogic.GetSubscriptionFromFile();
            //if (data.Count > 0)
            //{
            //    var subIdList = data.Select(ul => ul.License.UserSubscriptionId);
            //    UserSubscriptionLogic subscriptionLogic = new UserSubscriptionLogic();
            //    var userSubscriptionList = subscriptionLogic.GetSubscriptionByIDList(subIdList.ToList());
            //    var subscriptionIdList = userSubscriptionList.Select(s => s.SubscriptionId);
            //    var subscriptionList = dataList.Where(s => subscriptionIdList.Contains(s.Id)).ToList();
            //    foreach (var subs in subscriptionList)
            //    {
            //        var userLicLicst = data.Where(ul => ul.License.Subscription.SubscriptionId == subs.Id).ToList();
            //        var proList = userLicLicst.Select(u => u.License.ProductId).ToList();
            //        LicenseMapModel mapModel = new LicenseMapModel();
            //        mapModel.SubscriptionName = subs.SubscriptionName;
            //        mapModel.UserSubscriptionId = userSubscriptionList.FirstOrDefault(us => us.SubscriptionId == subs.Id).Id;
            //        DateTime licExpireData = DateTime.MinValue;

            //        foreach (var pro in subs.Product.Where(p => proList.Contains(p.Id)))
            //        {

            //            var objLic = userLicLicst.FirstOrDefault(f => f.License.ProductId == pro.Id);
            //            if (objLic != null)
            //            {
            //                string licenseKeydata = String.Empty;
            //                licenseKeydata = objLic.License.LicenseKey;
            //                var splitData = licenseKeydata.Split(new char[] { '-' });
            //                var datakey = splitData[0];
            //                var decryptObj = LicenseKey.LicenseKeyGen.CryptoEngine.Decrypt(datakey, true);
            //                var licdataList = decryptObj.Split(new char[] { '^' });
            //                licExpireData = Convert.ToDateTime(licdataList[1]);

            //            }
            //            SubscriptionProduct prod = new SubscriptionProduct();
            //            prod.ProductId = pro.Id;
            //            prod.ProductName = pro.Name;
            //            prod.ExpireDate = licExpireData;

            //            if (!isFeatureRequired)
            //            {
            //                var tempsub = LicenseSessionState.Instance.SubscriptionList?.FirstOrDefault(s => s.SubscriptionId == subs.Id);
            //                var tempPro = tempsub == null ? null : tempsub.ProductDtls.FirstOrDefault(p => p.ProductId == prod.ProductId);
            //                if (tempPro != null)
            //                    prod.IsDisabled = tempPro.AvailableCount == 0;
            //            }
            //            else
            //            {
            //                foreach (var fet in pro.Features)
            //                {
            //                    var feature = new Feature();
            //                    feature.Id = fet.Id;
            //                    feature.Name = fet.Name;
            //                    feature.Description = fet.Description;
            //                    feature.Version = fet.Version;
            //                    prod.Features.Add(feature);
            //                }
            //            }
            //            mapModel.ProductList.Add(prod);
            //        }
            //        licenseMapModelList.Add(mapModel);
            //    }
            //}
            return licenseMapModelList;
        }

        public static IList<SubscriptionDetails> GetSubscriptionForLicenseMap(string userId, string adminUserId)
        {

            IList<SubscriptionDetails> subscriptionProList = new List<SubscriptionDetails>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/UserSubscription/GetSubscriptioDtlsForLicenseMap/" + adminUserId + "/" + userId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsonData))
                    subscriptionProList = JsonConvert.DeserializeObject<List<SubscriptionDetails>>(jsonData);
            }
            return subscriptionProList;
            //UserLicenseLogic logic = new UserLicenseLogic();
            //var data = logic.GetUserLicense(userId);

            //UserSubscriptionLogic subscriptionLogic = new UserSubscriptionLogic();
            //var userSubscriptionList = subscriptionLogic.GetSubscription(adminUserId);
            //var subscriptionIdList = userSubscriptionList.Select(s => s.SubscriptionId).ToList();
            //if (LicenseSessionState.Instance.SubscriptionList == null || LicenseSessionState.Instance.SubscriptionList.Count() == 0)
            //    GetSubscription(adminUserId);
            //var subscriptionList = LicenseSessionState.Instance.SubscriptionList.Where(s => subscriptionIdList.Contains(s.SubscriptionId)).ToList();

            //foreach (var subs in subscriptionList)
            //{
            //    LicenseMapModel mapModel = new LicenseMapModel();
            //    mapModel.SubscriptionName = subs.SubscriptionName;
            //    mapModel.UserSubscriptionId = userSubscriptionList.FirstOrDefault(us => us.SubscriptionId == subs.SubscriptionId).Id;

            //    foreach (var pro in subs.ProductDtls)
            //    {
            //        SubscriptionProduct prod = new SubscriptionProduct();
            //        prod.ProductId = pro.ProductId;
            //        prod.ProductName = pro.ProductName;
            //        prod.IsDisabled = pro.AvailableCount == 0;
            //        mapModel.ProductList.Add(prod);
            //    }
            //    licenseMapModelList.Add(mapModel);
            //}
            //foreach (var obj in data)
            //{
            //    var subObj = licenseMapModelList.FirstOrDefault(f => f.UserSubscriptionId == obj.License.UserSubscriptionId);
            //    if (subObj != null)
            //    {
            //        var pro = subObj.ProductList.FirstOrDefault(f => f.ProductId == obj.License.ProductId);
            //        if (pro != null)
            //            pro.IsSelected = true;
            //        pro.InitialState = true;
            //    }
            //}
            //return licenseMapModelList;
        }

    }
}