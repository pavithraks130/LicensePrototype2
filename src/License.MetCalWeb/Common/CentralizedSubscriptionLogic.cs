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
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
            var response = await client.PostAsync("api/cart/OnlinePayment/" + LicenseSessionState.Instance.User.ServerUserId, null);
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
            List<UserSubscriptionData> subscriptionData = new List<UserSubscriptionData>();
            foreach (var subDtls in subs.Subscriptions)
            {
                //Code to save the user Subscription details to Database.
                UserSubscriptionData userSubscription = new UserSubscriptionData();
                userSubscription.SubscriptionDate = subDtls.SubscriptionDate;
                userSubscription.SubscriptionId = subDtls.SubscriptionTypeId;
                userSubscription.UserId = userId;
                userSubscription.Quantity = subDtls.OrderdQuantity;
                userSubscription.Subscription = subDtls;
                userSubscription.LicenseKeys = subDtls.LicenseKeyProductMapping;
                subscriptionData.Add(userSubscription);
            }
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("api/UserSubscription/SyncSubscription", subscriptionData).Result;
        }
    }
}