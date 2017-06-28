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
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.GetAsync("api/purchaseorder/syncpo/" + LicenseSessionState.Instance.User.ServerUserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<SubscriptionList>(jsonData);
                if (obj.Subscriptions.Count > 0)
                    CentralizedSubscriptionLogic.UpdateSubscriptionOnpremise(obj);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
            }

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
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.GetAsync("api/Product/GetProductsByAdminId/" + LicenseSessionState.Instance.User.UserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var errorData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = errorData.Message;
            }
            client.Dispose();
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
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.CentralizeWebApi);
            var response = client.PostAsJsonAsync("api/Product/CheckProductUpdates", productDetails).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var errorData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = errorData.Message;
            }
            client.Dispose();
            return products;
        }

        /// <summary>
        /// Updating the changed Products in the onpremise 
        /// </summary>
        /// <param name="productDetails"></param>
        public void UpdateProductUpdates(List<Product> productDetails)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PostAsJsonAsync("api/Product/UpdateProducts", productDetails).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var errorData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = errorData.Message;
            }
            client.Dispose();
        }

    }
}