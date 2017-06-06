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
        /// <summary>
        /// Create the User Subscription  once the Payment is completed and get the  subscription  in the on premise with product and features
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Once Payment is done the Subscription will be renewed and the New List of License Key  will returned to sync with the On Premise with the updated expiuire date
        /// </summary>
        /// <param name="subscriptions"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updating the Subscription in the On Premise once the New Subscsription is purchased or renewed
        /// </summary>
        /// <param name="subs"></param>
        /// <param name="isRenewal"></param>
        public static void UpdateSubscriptionOnpremise(SubscriptionList subs, bool isRenewal = false)
        {
            string userId = string.Empty;
            userId = LicenseSessionState.Instance.User.UserId;
            //List<UserSubscription> subscriptionData = new List<UserSubscription>();
            //foreach (var subDtls in subs.Subscriptions)
            //{
            //    //Code to save the user Subscription details to Database.
            //    UserSubscription userSubscription = new UserSubscription()
            //    {
            //        SubscriptionDate = subDtls.SubscriptionDate,
            //        RenewalDate = subDtls.RenewalDate,
            //        SubscriptionId = subDtls.SubscriptionTypeId,
            //        UserId = userId,
            //        Quantity = subDtls.OrderdQuantity,
            //        Subscription = subDtls,
            //        LicenseKeys = subDtls.LicenseKeyProductMapping
            //    };
            //    subscriptionData.Add(userSubscription);
            //}
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            string url = string.Empty;
            if (isRenewal)
                url = "api/UserSubscription/UpdateSubscriptionRenewal/" + LicenseSessionState.Instance.User.UserId;
            else
                url = "api/UserSubscription/SyncSubscription";
            var response = client.PostAsJsonAsync(url, subs).Result;
        }

        /// <summary>
        /// Gets the list of SUbscription  which expires based on the User Id
        /// </summary>
        /// <returns></returns>
        public static List<Subscription> GetExpireSubscription()
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/UserSubscription/ExpireSubscription/" + LicenseSessionState.Instance.User.ServerUserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var expiredSubscriptipon = JsonConvert.DeserializeObject<List<Subscription>>(responseData);
                return expiredSubscriptipon;
            }
            return null;
        }
    }
}