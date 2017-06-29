using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;
using System.Net.Http;
using Newtonsoft.Json;
using License.ServiceInvoke;
namespace License.MetCalWeb.Common
{
    public class OnPremiseSubscriptionLogic
    {
        private APIInvoke _invoke = null;

        public OnPremiseSubscriptionLogic()
        {
            _invoke = new APIInvoke();
        }
        /// <summary>
        /// Get user license by user Id  with feature listing 
        /// </summary>
        /// <returns></returns>
        public List<Product> GetUserLicenseForUser()
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
        public IList<Subscription> GetSubscription(string adminId)
        {
            IList<Subscription> subscriptionProList = new List<Subscription>();
            WebAPIRequest<List<Subscription>> request = new WebAPIRequest<List<Subscription>>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.SubscriptionDetils,
                Id = adminId,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.UserSubscription,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            var response = _invoke.InvokeService< List<Subscription>, List< Subscription > >(request);
            if (response.Status)
                subscriptionProList = response.ResponseData;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.GetAsync("api/UserSubscription/SubscriptionDetils/" + adminId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    if (!string.IsNullOrEmpty(jsonData))
            //        subscriptionProList = JsonConvert.DeserializeObject<List<Subscription>>(jsonData);
            //}
            return subscriptionProList;
        }


        /// <summary>
        /// Function to get the Product Subscription based on the Admin Id  with count for which admin subscribed.UserId is oprtional. 
        /// If we want to get the mapped Product  Data specific to user with product list then need to pass the userId of the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Product> GetProductsFromSubscription(string userId = "")
        {
            IList<Product> productsList = new List<Product>();
            WebAPIRequest<List<Product>> request = new WebAPIRequest<List<Product>>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                AdminId = LicenseSessionState.Instance.IsSuperAdmin ? LicenseSessionState.Instance.User.UserId : LicenseSessionState.Instance.SelectedTeam.AdminId,

                InvokeMethod = Method.GET,
                Functionality = !String.IsNullOrEmpty(userId) ? Functionality.GetProductsWithUserMappedProduct : Functionality.GetProducts,
                ServiceModule = Modules.Product,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            if (!String.IsNullOrEmpty(userId))
                request.Id = userId;
            else
                request.Id = request.AdminId;

            var response = _invoke.InvokeService< List<Product>, List< Product > >(request);
            if (response.Status)
                productsList = response.ResponseData;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //string adminId = string.Empty;
            //if (LicenseSessionState.Instance.IsSuperAdmin)
            //    adminId = LicenseSessionState.Instance.User.UserId;
            //else
            //    adminId = LicenseSessionState.Instance.SelectedTeam.AdminId;
            //var url = string.Empty;
            //if (!String.IsNullOrEmpty(userId))
            //    url = "api/Product/GetProductsWithUserMappedProduct/" + adminId + "/" + userId;
            //else
            //    url = "api/Product/GetProducts/" + adminId;
            //var response = client.GetAsync(url).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    if (!string.IsNullOrEmpty(jsonData))
            //        productsList = JsonConvert.DeserializeObject<IList<Product>>(jsonData);
            //}
            return productsList;
        }





        /// <summary>
        /// Function to get the User License with details  for which user is authorized. By default the fetchBasedonTeam is set true because moset of the time the 
        /// licnese details based on the team context logged in.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isFeatureRequired"></param>
        /// <returns></returns>
        public UserLicenseDetails GetUserLicenseDetails(string userId, bool isFeatureRequired, bool fetchBasedonTeam = true)
        {
            var licenseMapModelList = new UserLicenseDetails();
            // HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            FetchUserSubscription subs = new FetchUserSubscription();
            if (fetchBasedonTeam)
                subs.TeamId = LicenseSessionState.Instance.AppTeamContext.Id;
            subs.UserId = userId;
            subs.IsFeatureRequired = isFeatureRequired;
            WebAPIRequest<FetchUserSubscription> request = new WebAPIRequest<FetchUserSubscription>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.GetUserLicenseByUser,
                InvokeMethod = Method.POST,
                ModelObject = subs,
                ServiceModule = Modules.UserLicense,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            var response = _invoke.InvokeService< FetchUserSubscription, FetchUserSubscription>(request);
            if (response.Status)
                licenseMapModelList = response.ResponseData.UserLicenseDetails;
            //var response = client.PostAsJsonAsync("api/UserLicense/GetUserLicenseByUser", subs).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    licenseMapModelList = JsonConvert.DeserializeObject<UserLicenseDetails>(jsonData);
            //}
            return licenseMapModelList;
        }


        /// <summary>
        /// Function to get the User License with details  for which user is authorized
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isFeatureRequired"></param>
        /// <returns></returns>
        public List<Product> GetTeamLicenseDetails(int teamId)
        {
            var distinctProductList = new List<Product>();
            WebAPIRequest<List<Product>> request = new WebAPIRequest<List<Product>>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.GetTeamLicenseByTeam,
                Id = teamId.ToString(),
                InvokeMethod = Method.GET,
                ServiceModule = Modules.TeamLicense,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            var response = _invoke.InvokeService< List<Product>, List< Product > >(request);
            if (response.Status)
                distinctProductList = response.ResponseData;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.GetAsync("api/TeamLicense/GetTeamLicenseByTeam/" + teamId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    distinctProductList = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            //}
            return distinctProductList;
        }

        public IList<Subscription> GetSubscriptionForLicenseMap(string userId, string adminUserId)
        {

            IList<Subscription> subscriptionProList = new List<Subscription>();
            WebAPIRequest<List<Subscription>> request = new WebAPIRequest<List<Subscription>>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                AdminId = adminUserId,
                Id = userId,
                Functionality = Functionality.GetSubscriptioDtlsForLicenseMap,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.UserSubscription,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            var response = _invoke.InvokeService< List<Subscription>, List< Subscription > >(request);
            if (response.Status)
                subscriptionProList = response.ResponseData;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.GetAsync("api/UserSubscription/GetSubscriptioDtlsForLicenseMap/" + adminUserId + "/" + userId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    if (!string.IsNullOrEmpty(jsonData))
            //        subscriptionProList = JsonConvert.DeserializeObject<List<Subscription>>(jsonData);
            //}
            return subscriptionProList;
        }

        public List<Team> GetTeamList(string userId = "")
        {
            List<Team> teamlst = new List<Team>();
            WebAPIRequest<List<Team>> request = new WebAPIRequest<List<Team>>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Team,
                ServiceType = ServiceType.OnPremiseWebApi,
                Id = LicenseSessionState.Instance.IsSuperAdmin && string.IsNullOrEmpty(userId) ? LicenseSessionState.Instance.User.UserId : userId,
                Functionality = LicenseSessionState.Instance.IsSuperAdmin && string.IsNullOrEmpty(userId) ? Functionality.GetTeamsByAdminId : Functionality.GetTeamsByUserId
            };
            var response = _invoke.InvokeService< List<Team>, List< Team > >(request);

            if (response.Status)
                teamlst = response.ResponseData;
            //HttpResponseMessage response;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //if (LicenseSessionState.Instance.IsSuperAdmin && string.IsNullOrEmpty(userId))
            //    response = client.GetAsync("api/Team/GetTeamsByAdminId/" + LicenseSessionState.Instance.User.UserId).Result;
            //else
            //    response = client.GetAsync("api/Team/GetTeamsByUserId/" + userId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    teamlst = JsonConvert.DeserializeObject<List<Team>>(jsonData);
            //}
            return teamlst;

        }

    }
}