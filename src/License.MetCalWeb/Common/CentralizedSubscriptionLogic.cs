using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using License.Models;
using License.ServiceInvoke;

namespace License.MetCalWeb.Common
{
    public class CentralizedSubscriptionLogic
    {
        private APIInvoke _invoke = null;
        public CentralizedSubscriptionLogic()
        {
            _invoke = new APIInvoke();
        }
        /// <summary>
        /// Create the User Subscription  once the Payment is completed and get the  subscription  in the on premise with product and features
        /// </summary>
        /// <returns></returns>
        public async Task UpdateUserSubscription()
        {
            SubscriptionList userSubscriptionList = null;
            WebAPIRequest<SubscriptionList> request = new WebAPIRequest<SubscriptionList>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.OnlinePayment,
                InvokeMethod = Method.POST,
                Id = LicenseSessionState.Instance.User.ServerUserId,
                ServiceModule = Modules.Cart,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService<SubscriptionList, SubscriptionList>(request);
            if (response.Status && response.ResponseData != null && response.ResponseData.Subscriptions != null && response.ResponseData.Subscriptions.Count > 0)
                UpdateSubscriptionOnpremise(response.ResponseData);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = await client.PostAsync("api/cart/OnlinePayment/" + LicenseSessionState.Instance.User.ServerUserId, null);
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsondata = response.Content.ReadAsStringAsync().Result;
            //    if (!string.IsNullOrEmpty(jsondata))
            //    {
            //        userSubscriptionList = JsonConvert.DeserializeObject<SubscriptionList>(jsondata);
            //        UpdateSubscriptionOnpremise(userSubscriptionList);
            //    }
            //}

        }

        /// <summary>
        /// Once Payment is done the Subscription will be renewed and the New List of License Key  will returned to sync 
        /// with the On Premise with the updated expiuire date
        /// </summary>
        /// <param name="subscriptions"></param>
        /// <returns></returns>
        public async Task RenewSubscription(RenewSubscriptionList subscriptions)
        {
            WebAPIRequest<RenewSubscriptionList> request = new WebAPIRequest<RenewSubscriptionList>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.RenewSubscription,
                InvokeMethod = Method.POST,
                Id = LicenseSessionState.Instance.User.ServerUserId,
                ServiceModule = Modules.UserSubscription,
                ServiceType = ServiceType.CentralizeWebApi,
                 ModelObject = subscriptions
            };
            var response = _invoke.InvokeService<RenewSubscriptionList, SubscriptionList>(request);
            if (response.Status && response.ResponseData != null && response.ResponseData.Subscriptions != null && response.ResponseData.Subscriptions.Count > 0)
                UpdateSubscriptionOnpremise(response.ResponseData, true);
            //    HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = await client.PostAsJsonAsync("api/UserSubscription/RenewSubscription/" + LicenseSessionState.Instance.User.ServerUserId, subscriptions);
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var subscripListObj = JsonConvert.DeserializeObject<SubscriptionList>(jsonData);
            //    if (subscripListObj != null)
            //        UpdateSubscriptionOnpremise(subscripListObj, true);
            //}
        }

        /// <summary>
        /// Updating the Subscription in the On Premise once the New Subscsription is purchased or renewed.
        /// Note:  Here for On Premise the user Id need to Be updated  with On Premise User Id as shown Below
        /// </summary>
        /// <param name="subs"></param>
        /// <param name="isRenewal"></param>
        public void UpdateSubscriptionOnpremise(SubscriptionList subs, bool isRenewal = false)
        {
            string userId = string.Empty;
            subs.UserId = LicenseSessionState.Instance.User.UserId;
            WebAPIRequest<SubscriptionList> request = new WebAPIRequest<SubscriptionList>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = isRenewal ? Functionality.UpdateSubscriptionRenewal : Functionality.SynchronizeSubscription,
                InvokeMethod = Method.POST,
                Id = LicenseSessionState.Instance.User.UserId,
                ServiceModule = Modules.UserSubscription,
                ServiceType = ServiceType.OnPremiseWebApi,
                ModelObject = subs
            };
            _invoke.InvokeService<SubscriptionList, String>(request);
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //string url = string.Empty;

            //if (isRenewal)
            //    url = "api/UserSubscription/UpdateSubscriptionRenewal/" + LicenseSessionState.Instance.User.UserId;
            //else
            //    url = "api/UserSubscription/SyncSubscription";
            //var response = client.PostAsJsonAsync(url, subs).Result;
        }

        /// <summary>
        /// Gets the list of SUbscription  which expires based on the User Id
        /// </summary>
        /// <returns></returns>
        public List<UserSubscription> GetExpireSubscription()
        {
            int duration = 30;
            WebAPIRequest<List<UserSubscription>> request = new WebAPIRequest<List<UserSubscription>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                AdminId = duration.ToString(),
                Functionality = Functionality.ExpireSubscription,
                InvokeMethod = Method.GET,
                Id = LicenseSessionState.Instance.User.ServerUserId,
                ServiceModule = Modules.UserSubscription,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService<List<UserSubscription>, List<UserSubscription>>(request);
            if (response.Status)
                return response.ResponseData;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = client.GetAsync("api/UserSubscription/ExpireSubscription/" + duration + "/" + LicenseSessionState.Instance.User.ServerUserId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var responseData = response.Content.ReadAsStringAsync().Result;
            //    var expiredSubscriptipon = JsonConvert.DeserializeObject<List<UserSubscription>>(responseData);
            //    return expiredSubscriptipon;
            //}
            return null;
        }
    }
}