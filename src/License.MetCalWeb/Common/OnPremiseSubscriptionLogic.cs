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
        /// <summary>
        /// Get user license by user Id  with feature listing 
        /// </summary>
        /// <returns></returns>
        public static List<ProductDetails> GetUserLicenseForUser()
        {
            string userId = LicenseSessionState.Instance.User.UserId;
            var userLicDetails = GetUserLicenseDetails(userId, true);
            return userLicDetails.Products;
        }

        /// <summary>
        /// Function to get the Product Subscription based on the Admin Id  with count for which admin subscribed. UserId is the Admin UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IList<Subscription> GetSubscription(string adminId)
        {
            IList<Subscription> subscriptionProList = new List<Subscription>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/UserSubscription/SubscriptionDetils/" + adminId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsonData))
                    subscriptionProList = JsonConvert.DeserializeObject<List<Subscription>>(jsonData);
            }
            return subscriptionProList;
        }


        /// <summary>
        /// Function to get the Product Subscription based on the Admin Id  with count for which admin subscribed.UserId is oprtional. 
        /// If we want to get the mapped Product  Data specific to user with product list then need to pass the userId of the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IList<ProductDetails> GetProductsFromSubscription(string userId = "")
        {
            IList<ProductDetails> productsList = new List<ProductDetails>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            string adminId = string.Empty;
            if (LicenseSessionState.Instance.IsSuperAdmin)
                adminId = LicenseSessionState.Instance.User.UserId;
            else
                adminId = LicenseSessionState.Instance.SelectedTeam.AdminId;
            var url = string.Empty;
            if (!String.IsNullOrEmpty(userId))
                url = "api/Product/GetProductsWithUserMappedProduct/" + adminId + "/" + userId;
            else
                url = "api/Product/GetProducts/" + adminId;
            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsonData))
                    productsList = JsonConvert.DeserializeObject<IList<ProductDetails>>(jsonData);
            }
            return productsList;
        }
        
        /// <summary>
        /// Function to get the User License with details  for which user is authorized. By default the fetchBasedonTeam is set true because moset of the time the 
        /// licnese details based on the team context logged in.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isFeatureRequired"></param>
        /// <returns></returns>
        public static UserLicenseDetails GetUserLicenseDetails(string userId, bool isFeatureRequired, bool fetchBasedonTeam = true)
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


        /// <summary>
        /// Function to get the User License with details  for which user is authorized
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isFeatureRequired"></param>
        /// <returns></returns>
        public static List<ProductDetails> GetTeamLicenseDetails(int teamId)
        {
            var distinctProductList = new List<ProductDetails>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/License/GetSubscriptionLicenseByTeamId/" + teamId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                distinctProductList = JsonConvert.DeserializeObject<List<ProductDetails>>(jsonData);
            }
            return distinctProductList;
        }

        public static IList<Subscription> GetSubscriptionForLicenseMap(string userId, string adminUserId)
        {

            IList<Subscription> subscriptionProList = new List<Subscription>();
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/UserSubscription/GetSubscriptioDtlsForLicenseMap/" + adminUserId + "/" + userId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsonData))
                    subscriptionProList = JsonConvert.DeserializeObject<List<Subscription>>(jsonData);
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