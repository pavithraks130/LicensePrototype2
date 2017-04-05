using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using License.MetCalWeb.Models;

namespace License.MetCalWeb.Common
{
    public class CentralizedSubscriptionLogic
    {
        public static async Task UpdateUserSubscription()
        {
            SubscriptionList userSubscriptionList = null;
            var serviceType = System.Configuration.ConfigurationManager.AppSettings.Get("ServiceType");
            HttpClient client = WebApiServiceLogic.CreateClient(serviceType);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.PostAsync("api/OnlinePayment/" + LicenseSessionState.Instance.User.ServerUserId, null);
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsondata))
                {
                    userSubscriptionList = JsonConvert.DeserializeObject<SubscriptionList>(jsondata);
                    UpdateSubscriptionOnpremise(userSubscriptionList);
                }
            }
        }

        public static void UpdateSubscriptionOnpremise(SubscriptionList subs)
        {
            string userId = string.Empty;
            userId = LicenseSessionState.Instance.User.UserId;
            foreach (var subDtls in subs.Subscriptions)
            {



                //License.DataModel.Subscription subsModel = new DataModel.Subscription();
                //subsModel.Id = subDtls.SubscriptionType.Id;
                //subsModel.SubscriptionName = subDtls.SubscriptionType.Name;
                //subsModel.Product = productList;

                //Logic.DataLogic.ProductSubscriptionLogic proSubLogic = new Logic.DataLogic.ProductSubscriptionLogic();
                //proSubLogic.SaveToFile(subsModel);
                ////End

                //Code to save the user Subscription details to Database.
                UserSubscriptionData userSubscription = new UserSubscriptionData();
                userSubscription.SubscriptionDate = subDtls.SubscriptionDate;
                userSubscription.SubscriptionId = subDtls.SubscriptionTypeId;
                userSubscription.UserId = userId;
                userSubscription.Quantity = subDtls.OrderdQuantity;
                userSubscription.Subscription = subDtls;
                userSubscription.LicenseKeys = subDtls.LicenseKeyProductMapping;
                //License.Logic.DataLogic.UserSubscriptionLogic userSubscriptionLogic = new Logic.DataLogic.UserSubscriptionLogic();
                //int userSubscriptionId = userSubscriptionLogic.CreateSubscription(userSubscription);


                //List<License.DataModel.LicenseData> licenseDataList = new List<DataModel.LicenseData>();
                //foreach (var lic in subDtls.LicenseKeyProductMapping)
                //{
                //    License.DataModel.LicenseData licenseData = new DataModel.LicenseData();
                //    licenseData.LicenseKey = lic.LicenseKey;
                //    licenseData.ProductId = lic.ProductId;
                //    licenseData.UserSubscriptionId = userSubscriptionId;
                //    licenseDataList.Add(licenseData);
                //}
                //License.Logic.DataLogic.LicenseLogic licenseLogic = new Logic.ServiceLogic.LicenseLogic();
                //licenseLogic.CreateLicenseData(licenseDataList);
            }
        }
    }
}