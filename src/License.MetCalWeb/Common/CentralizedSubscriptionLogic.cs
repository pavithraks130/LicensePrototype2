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
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
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

        public static async Task RenewSubscription(RenewSubscriptionList subscriptions)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = await client.PostAsJsonAsync("api/UserSubscription/RenewSubscription/" + LicenseSessionState.Instance.User.ServerUserId, subscriptions);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var subscripListObj = JsonConvert.DeserializeObject<SubscriptionList>(jsonData);
                if (subscripListObj != null)
                    UpdateSubscriptionOnpremise(subscripListObj, true);
            }
        }

        public static void UpdateSubscriptionOnpremise(SubscriptionList subs, bool isRenewal = false)
        {
            string userId = string.Empty;
            userId = LicenseSessionState.Instance.User.UserId;
            List<UserSubscriptionData> subscriptionData = new List<UserSubscriptionData>();
            foreach (var subDtls in subs.Subscriptions)
            {
                //Code to save the user Subscription details to Database.
                UserSubscriptionData userSubscription = new UserSubscriptionData()
                {
                    SubscriptionDate = subDtls.SubscriptionDate,
                    RenewalDate = subDtls.RenewalDate,
                    SubscriptionId = subDtls.SubscriptionTypeId,
                    UserId = userId,
                    Quantity = subDtls.OrderdQuantity,
                    Subscription = subDtls,
                    LicenseKeys = subDtls.LicenseKeyProductMapping
                };
                subscriptionData.Add(userSubscription);
            }
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            string url = string.Empty;
            if (isRenewal)
                url = "api/UserSubscription/UpdateSubscriptionRenewal/" + LicenseSessionState.Instance.User.UserId;
            else
                url = "api/UserSubscription/SyncSubscription";
            var response = client.PostAsJsonAsync(url, subscriptionData).Result;
        }

        public static List<SubscriptionType> GetExpireSubscription()
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/UserSubscription/ExpireSubscription/" + LicenseSessionState.Instance.User.ServerUserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var expiredSubscriptipon = JsonConvert.DeserializeObject<List<SubscriptionType>>(responseData);
                return expiredSubscriptipon;
            }
            return null;
        }
    }
}