using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using License.Models;
using License.ServiceInvoke;
namespace License.MetCalWeb.Common
{
    /// <summary>
    /// Contains logic related to auathentication and AUthorization and process to be conduct once user logged in
    /// </summary>
    public class AuthorizeBackendService
    {
        public string ErrorMessage { get; set; }

        private APIInvoke _invoke = null;
        private CentralizedSubscriptionLogic _centralizedSubscriptionLogic = null;
        public AuthorizeBackendService()
        {
            _invoke = new APIInvoke();
            _centralizedSubscriptionLogic = new CentralizedSubscriptionLogic();
        }

        public async Task SyncDataToOnpremise()
        {
            SynchPurchaseOrder();
            SyncProductUpdates();
        }
        /// <summary>
        /// Sync Purchase Order which are approved from Centralized to On Premise
        /// </summary>
        public void SynchPurchaseOrder()
        {
            ErrorMessage = string.Empty;
            WebAPIRequest<SubscriptionList> request = new WebAPIRequest<SubscriptionList>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.syncpo,
                InvokeMethod = Method.GET,
                Id = LicenseSessionState.Instance.User.ServerUserId,
                ServiceModule = Modules.PurchaseOrder,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService<SubscriptionList,SubscriptionList>(request);
            if (response.Status && response.ResponseData.Subscriptions.Count > 0)
                _centralizedSubscriptionLogic.UpdateSubscriptionOnpremise(response.ResponseData);
            else
                ErrorMessage = response.Error.error + " " + response.Error.Message;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = client.GetAsync("api/purchaseorder/syncpo/" + LicenseSessionState.Instance.User.ServerUserId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<SubscriptionList>(jsonData);
            //    if (obj.Subscriptions.Count > 0)
            //        CentralizedSubscriptionLogic.UpdateSubscriptionOnpremise(obj);
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
            //}

        }

        /// <summary>
        /// Function to Sync the Product update from Centralized to local on Premise if any changes made to the Products
        /// </summary>
        public void SyncProductUpdates()
        {

            var products = GetOnPremiseProducts();
            if (products != null && products.Count > 0)
                products = CheckProductUpdate(products);
            if (products != null && products.Count > 0)
                UpdateProductUpdates(products);
        }

        /// <summary>
        /// Gets the list of Products which exist in the On premise to check if these products are modified in the Centralized 
        /// </summary>
        /// <returns></returns>
        public List<Product> GetOnPremiseProducts()
        {
            List<Product> products = null;
            WebAPIRequest<List<Product>> request = new WebAPIRequest<List<Product>>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.GetProductsByAdminId,
                Id = LicenseSessionState.Instance.User.UserId,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Product,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            var response = _invoke.InvokeService< List<Product>, List< Product > >(request);
            if (response.Status)
                products = response.ResponseData;
            else
                ErrorMessage = response.Error.error + " " + response.Error.Message;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.GetAsync("api/Product/GetProductsByAdminId/" + LicenseSessionState.Instance.User.UserId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var errorData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ErrorMessage = errorData.Message;
            //}
            //client.Dispose();
            return products;
        }

        /// <summary>
        /// Check for theh product updates for the specified products
        /// </summary>
        /// <param name="productDetails"></param>
        /// <returns></returns>
        public List<Product> CheckProductUpdate(List<Product> productDetails)
        {
            List<Product> products = null;
            WebAPIRequest<List<Product>> request = new WebAPIRequest<List<Product>>()
            {
                AccessToken = LicenseSessionState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.CheckProductUpdates,
                ModelObject = productDetails,
                InvokeMethod = Method.POST,
                ServiceModule = Modules.Product,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService< List<Product>, List< Product > >(request);
            if (response.Status)
                products = response.ResponseData;
            else
                ErrorMessage = response.Error.error + " " + response.Error.Message;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            //var response = client.PostAsJsonAsync("api/Product/CheckProductUpdates", productDetails).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var errorData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ErrorMessage = errorData.Message;
            //}
            //client.Dispose();
            return products;
        }

        /// <summary>
        /// Updating the changed Products in the onpremise 
        /// </summary>
        /// <param name="productDetails"></param>
        public void UpdateProductUpdates(List<Product> productDetails)
        {
            WebAPIRequest<List<Product>> request = new WebAPIRequest<List<Product>>()
            {
                AccessToken = LicenseSessionState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.UpdateProducts,
                InvokeMethod = Method.POST,
                ServiceModule = Modules.Product,
                ServiceType = ServiceType.OnPremiseWebApi,
                ModelObject = productDetails
            };
            var response = _invoke.InvokeService< List<Product>, List< Product > >(request);
            if (!response.Status)
                ErrorMessage = response.Error.error + " " + response.Error.Message;
            //HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            //var response = client.PostAsJsonAsync("api/Product/UpdateProducts", productDetails).Result;
            //if (!response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var errorData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    ErrorMessage = errorData.Message;
            //}
            //client.Dispose();
        }

    }
}