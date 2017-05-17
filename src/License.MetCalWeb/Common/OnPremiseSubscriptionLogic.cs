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
        public static IList<SubscriptionDetails> GetSubscription(string adminId)
        {
            IList<SubscriptionDetails> subscriptionProList = new List<SubscriptionDetails>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/UserSubscription/SubscriptionDetils/" + adminId).Result;
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
        public static UserLicenseDetails GetUserLicenseDetails(string userId, bool isFeatureRequired,bool fetchBasedonTeam = true)
        {
            var licenseMapModelList = new UserLicenseDetails();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            FetchUserSubscription subs = new FetchUserSubscription();
            if (fetchBasedonTeam)
                subs.TeamId = LicenseSessionState.Instance.AppTeamContext.Id;
            subs.UserId = userId;
            subs.IsFeatureRequired = isFeatureRequired;
            var response = client.PostAsJsonAsync("api/License/GetSubscriptionLicenseByTeam", subs).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                licenseMapModelList = JsonConvert.DeserializeObject<UserLicenseDetails>(jsonData);
            }
            return licenseMapModelList;
        }

        public static IList<SubscriptionDetails> GetSubscriptionForLicenseMap(string userId, string adminUserId)
        {

            IList<SubscriptionDetails> subscriptionProList = new List<SubscriptionDetails>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/UserSubscription/GetSubscriptioDtlsForLicenseMap/" + adminUserId + "/" + userId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsonData))
                    subscriptionProList = JsonConvert.DeserializeObject<List<SubscriptionDetails>>(jsonData);
            }
            return subscriptionProList;
        }

        public static List<Team> GetTeamList(string userId = "")
        {
            List<Team> teamlst = new List<Team>();
            HttpResponseMessage response;
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            if (LicenseSessionState.Instance.IsSuperAdmin && string.IsNullOrEmpty(userId))
                response = client.GetAsync("api/Team/GetTeamsByAdminId/" + LicenseSessionState.Instance.User.UserId).Result;
            else
                response = client.GetAsync("api/Team/GetTeamsByUserId/" + userId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                teamlst = JsonConvert.DeserializeObject<List<Team>>(jsonData);
            }
            return teamlst;

        }

    }
}