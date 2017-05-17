using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using License.MetCalWeb.Models;

namespace License.MetCalWeb.Common
{
    public class AuthorizeBackendService
    {
        public string ErrorMessage { get; set; }

        public async Task SyncDataToOnpremise()
        {
            SynchPurchaseOrder();
            SyncProductUpdates();
        }
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

        public void SyncProductUpdates()
        {

            var products = GetOnPremiseProducts();
            if (products != null && products.Count > 0)
                products = CheckProductUpdate(products);
            if (products != null && products.Count > 0)
                UpdateProductUpdates(products);
        }

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