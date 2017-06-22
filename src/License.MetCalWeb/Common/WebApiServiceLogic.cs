using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using License.Models;
using Newtonsoft.Json;
using License.ServiceInvoke;

namespace License.MetCalWeb.Common
{
    /// <summary>
    /// Common class for the Creation of the client object for the service call and update the required attributeds for the Client object
    /// </summary>
    public class WebApiServiceLogic
    {
        public static HttpClient CreateClient(ServiceType serviceType)
        {
            string url = string.Empty;
            switch (serviceType)
            {
                case ServiceType.OnPremiseWebApi:
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("OnPremiseWebApi");
                    break;
                case ServiceType.CentralizeWebApi:
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("CentralizeWebApi");
                    break;
            }
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            switch (serviceType)
            {
                case ServiceType.OnPremiseWebApi:
                    if (LicenseSessionState.Instance.OnPremiseToken != null)
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                    break;
                case ServiceType.CentralizeWebApi:
                    if (LicenseSessionState.Instance.CentralizedToken != null)
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
                    break;
            }
            return client;
        }

        public static HttpClient CreateClientWithoutToken(ServiceType serviceType)
        {
            string url = string.Empty;
            switch (serviceType)
            {
                case ServiceType.OnPremiseWebApi:
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("OnPremiseWebApi");
                    break;
                case ServiceType.CentralizeWebApi:
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("CentralizeWebApi");
                    break;
            }
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }


        public WebAPIResponse InvokeService(WebAPIRequest apiRequestData)
        {
            WebAPIResponse serviceResponse = new WebAPIResponse();
            var url = GetServiceUrl(apiRequestData.Id, apiRequestData.ServiceModule, apiRequestData.Functionality, apiRequestData.AdminId);

            HttpClient client = CreateClient(apiRequestData.ServiceType);
            HttpResponseMessage response = null;
            switch (apiRequestData.InvokeMethod)
            {
                case Method.POST:
                    response = client.PostAsJsonAsync(url, apiRequestData.JsonData).Result;
                    break;
                case Method.PUT:
                    response = client.PutAsJsonAsync(url, apiRequestData.JsonData).Result;
                    break;
                case Method.DELETE:
                    response = client.DeleteAsync(url).Result;
                    break;
                case Method.GET:
                    response = client.GetAsync(url).Result;
                    break;
            }
            if (response.IsSuccessStatusCode)
            {
                serviceResponse.ResponseData = response.Content.ReadAsStringAsync().ToString();
                serviceResponse.Status = true;
            }
            else
            {
                serviceResponse.Status = false;
                var errorJsonData = response.Content.ReadAsStringAsync().Result;
                serviceResponse.Error = JsonConvert.DeserializeObject<ResponseFailure>(errorJsonData);
            }
            client.Dispose();
            return serviceResponse;
        }

        public String GetServiceUrl(string id, Modules type, string functionality, string adminId = null)
        {
            string url = string.Empty;
            switch (type)
            {
                case Modules.Asset: url = GetAssetUrl(id, functionality, adminId); break;
                case Modules.Cart: url = GetCartUrl(id, functionality, adminId); break;
                case Modules.Feature: url = GetFeatureUrl(id, functionality, adminId); break;
                case Modules.Product: url = GetProductUrl(id, functionality, adminId); break;
                case Modules.PurchaseOrder: url = GetPurchaseOrderUrl(id, functionality, adminId); break;
                case Modules.Subscription: url = GetSubscriptionUrl(id, functionality, adminId); break;
                case Modules.SubscriptionCategory: url = GetSubscriptionCategoryUrl(id, functionality, adminId); break;
                case Modules.Team: url = GetTeamUrl(id, functionality, adminId); break;
                case Modules.TeamLicense: url = GetTeamLicenseUrl(id, functionality, adminId); break;
                case Modules.TeamMember: url = GetTeamMemberUrl(id, functionality, adminId); break;
                case Modules.User: url = GetUserUrl(id, functionality, adminId); break;
                case Modules.UserLicense: url = GetUserLicenseUrl(id, functionality, adminId); break;
                case Modules.UserSubscription: url = GetUserSubscriptionUrl(id, functionality, adminId); break;
                case Modules.UserToken: url = GetUserTokenUrl(id, functionality, adminId); break;
            }
            return url;
        }

        public string GetAssetUrl(string id, string functionality, string adminId)
        {
            AssetFunctionality assetFunctionality = (AssetFunctionality)Enum.Parse(typeof(AssetFunctionality), functionality);
            string url = String.Empty;
            switch (assetFunctionality)
            {
                case AssetFunctionality.All: url = "api/asset/All"; break;
                case AssetFunctionality.GetById: url = "api/asset/GetById/" + id; break;
                case AssetFunctionality.Update: url = "api/asset/Update/" + id; break;
                case AssetFunctionality.Delete: url = "api/asset/Delete/" + id; break;
                case AssetFunctionality.Create: url = "api/asset/Create"; break;
            }
            return url;
        }

        public string GetCartUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            CartFunctionality cartFunctionality = (CartFunctionality)Enum.Parse(typeof(CartFunctionality), functionality);
            switch (cartFunctionality)
            {
                case CartFunctionality.Delete: url = "api/cart/Delete/" + id; break;
                case CartFunctionality.OfflinePayment: url = "api/cart/offlinepayment/" + id; break;
                case CartFunctionality.OnlinePayment: url = "api/cart/OnlinePayment/" + id; break;
                case CartFunctionality.GetByUser: url = "api/cart/GetItemsByUser/" + id; break;
                case CartFunctionality.GetCartItemsCount: url = "api/Cart/GetCartItemCount/" + id; break;
                case CartFunctionality.CreateSubscriptionAddToCart: url = "api/cart/CreateSubscriptionAddToCart"; break;
            }
            return url;
        }

        public string GetFeatureUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            FeatureFunctionality featureFunctionality = (FeatureFunctionality)Enum.Parse(typeof(FeatureFunctionality), functionality);
            switch (featureFunctionality)
            {
                case FeatureFunctionality.All: url = "api/Feature/All"; break;
                case FeatureFunctionality.Create: url = "api/Feature/Create"; break;
                case FeatureFunctionality.GetById: url = "api/feature/GetbyId/" + id; break;
                case FeatureFunctionality.Update: url = "api/feature/Update/" + id; break;
                case FeatureFunctionality.Delete: url = "api/Feature/Delete/" + id; break;
                case FeatureFunctionality.GetByCategory: url = "api/Feature/GetByCategory/" + id; break;
            }
            return url;
        }

        public string GetProductUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            ProductFunctionality prodFunctionality = (ProductFunctionality)Enum.Parse(typeof(ProductFunctionality), functionality);
            switch (prodFunctionality)
            {
                case ProductFunctionality.All: url = "api/Product/All"; break;
                case ProductFunctionality.Create: url = "api/product/create"; break;
                case ProductFunctionality.GetById: url = "api/product/GetById/" + id; break;
                case ProductFunctionality.Update: url = "api/product/update/" + id; break;
                case ProductFunctionality.ProductDependency: url = "api/Product/ProductDependency"; break;
                case ProductFunctionality.Delete: url = "api/product/delete/" + id; break;
                case ProductFunctionality.GetProductsWithUserMappedProduct: url = "api/Product/GetProductsWithUserMappedProduct/" + adminId + "/" + id; break;
                case ProductFunctionality.GetProducts: url = "api/Product/GetProducts/" + id; break;
                case ProductFunctionality.GetCMMSProducts: url = "api/Product/GetCMMSProducts"; break;
            }
            return url;

        }

        public string GetPurchaseOrderUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            PurchaseOrderFunctionality poFunctionality = (PurchaseOrderFunctionality)Enum.Parse(typeof(PurchaseOrderFunctionality), functionality);
            switch (poFunctionality)
            {
                case PurchaseOrderFunctionality.All: url = "api/purchaseorder/All"; break;
                case PurchaseOrderFunctionality.UpdataMuliplePO: url = "/api/purchaseorder/UpdataMuliplePO"; break;
                case PurchaseOrderFunctionality.OrderByUser: url = "api/purchaseorder/OrderByUser/" + id; break;
                case PurchaseOrderFunctionality.GetById: url = "api/purchaseorder/GetById/" + id; break;

            }
            return url;
        }

        public string GetSubscriptionCategoryUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            SubscriptionCategoryFunctionality categoryFunctionality = (SubscriptionCategoryFunctionality)Enum.Parse(typeof(SubscriptionCategoryFunctionality), functionality);
            switch (categoryFunctionality)
            {
                case SubscriptionCategoryFunctionality.All: url = "api/SubscriptionCategory/All"; break;
                case SubscriptionCategoryFunctionality.Create: url = "api/SubscriptionCategory/create"; break;
                case SubscriptionCategoryFunctionality.GetById: url = "api/SubscriptionCategory/GetById/" + id; break;
                case SubscriptionCategoryFunctionality.Update: url = "api/SubscriptionCategory/update/" + id; break;
                case SubscriptionCategoryFunctionality.Delete: url = "api/SubscriptionCategory/Delete/" + id; break;
            }
            return url;
        }

        public string GetSubscriptionUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            SubscriptionFunctionality subscriptionFunctionality = (SubscriptionFunctionality)Enum.Parse(typeof(SubscriptionFunctionality), functionality);
            switch (subscriptionFunctionality)
            {
                case SubscriptionFunctionality.All: url = "api/subscription/All"; break;
                case SubscriptionFunctionality.Get: url = "api/subscription/All/" + id; break;
                case SubscriptionFunctionality.Create: url = "api/subscription/Create"; break;
            }
            return url;
        }

        public string GetTeamUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            TeamFunctionality teamFunctionality = (TeamFunctionality)Enum.Parse(typeof(TeamFunctionality), functionality);
            switch (teamFunctionality)
            {
                case TeamFunctionality.Create: url = "api/Team/Create"; break;
                case TeamFunctionality.Update: url = "api/team/Update/" + id; break;
                case TeamFunctionality.Delete: url = "api/team/Delete/" + id; break;
                case TeamFunctionality.UpdateConcurentUser: url = "api/team/UpdateConcurentUser"; break;
                case TeamFunctionality.GetById: url = "api/Team/GetById/" + id; break;
                case TeamFunctionality.GetTeamsByAdminId: url = "api/Team/GetTeamsByAdminId/" + id; break;
                case TeamFunctionality.GetTeamsByUserId: url = "api/Team/GetTeamsByUserId/" + id; break;

            }
            return url;
        }

        public string GetTeamLicenseUrl(string id, string functionality, string adminId)
        {
            string url = String.Empty;
            TeamLicenseFunctionality teamLicenseFunctionality = (TeamLicenseFunctionality)Enum.Parse(typeof(TeamLicenseFunctionality), functionality);
            switch (teamLicenseFunctionality)
            {
                case TeamLicenseFunctionality.Create: url = "api/TeamLicense/Create"; break;
                case TeamLicenseFunctionality.Revoke: url = "api/TeamLicense/Revoke"; break;
                case TeamLicenseFunctionality.GetTeamLicenseByTeam: url = "api/TeamLicense/GetTeamLicenseByTeam/" + id; break;
            }
            return url;
        }

        public string GetTeamMemberUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            TeamMemberFunctionality teamMemberFunctionality = (TeamMemberFunctionality)Enum.Parse(typeof(TeamMemberFunctionality), functionality);
            switch (teamMemberFunctionality)
            {
                case TeamMemberFunctionality.Create:
                case TeamMemberFunctionality.Assign: url = "api/TeamMember/Create"; break;
                case TeamMemberFunctionality.UpdateAdminAccess: url = "api/TeamMember/UpdateAdminAccess"; break;
                case TeamMemberFunctionality.Delete: url = "api/TeamMember/Delete/" + id; break;
                case TeamMemberFunctionality.Revoke: url = "api/TeamMember/Remove"; break;

            }
            return url;
        }

        public string GetUserUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            UserFunctionality userFunctionalaity = (UserFunctionality)Enum.Parse(typeof(UserFunctionality), functionality);
            switch (userFunctionalaity)
            {
                case UserFunctionality.All: url = "api/user/All"; break;
                case UserFunctionality.Update: url = "/api/user/update/" + id; break;
                case UserFunctionality.ChangePassword: url = "api/user/ChangePassword/" + id; break;

            }
            return url;
        }

        public string GetUserLicenseUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            UserLicenseFunctionality licenseFunctionality = (UserLicenseFunctionality)Enum.Parse(typeof(UserLicenseFunctionality), functionality);
            switch (licenseFunctionality)
            {
                case UserLicenseFunctionality.GetRequestByTeam: url = "api/UserLicense/GetRequestByTeam/" + id; break;
                case UserLicenseFunctionality.ApproveReject: url = "api/UserLicense/ApproveReject"; break;
                case UserLicenseFunctionality.Create: url = "api/UserLicense/Create"; break;
                case UserLicenseFunctionality.Revoke: url = "api/UserLicense/Revoke"; break;
                case UserLicenseFunctionality.LicenseRequest: url = "api/UserLicense/LicenseRequest"; break;
                case UserLicenseFunctionality.GetRequestStatus: url = "api/UserLicense/GetRequestStatus/" + id; break;
                case UserLicenseFunctionality.GetUserLicenseByUser: url = "api/UserLicense/GetUserLicenseByUser"; break;
            }
            return url;
        }

        public string GetUserSubscriptionUrl(string id, string functionality, string parameter2)
        {
            string url = string.Empty;
            UserSubscriptionFunctionality userSubscriptionFunctionality = (UserSubscriptionFunctionality)Enum.Parse(typeof(UserSubscriptionFunctionality), functionality);
            switch (userSubscriptionFunctionality)
            {
                case UserSubscriptionFunctionality.RenewSubscription: url = "api/UserSubscription/RenewSubscription/" + id; break;
                case UserSubscriptionFunctionality.UpdateSubscriptionRenewel: url = "api/UserSubscription/UpdateSubscriptionRenewal/" + id; break;
                case UserSubscriptionFunctionality.SynchronizeSubscription: url = "api/UserSubscription/SyncSubscription"; break;
                case UserSubscriptionFunctionality.ExpireSubscription: url = "api/UserSubscription/ExpireSubscription/" + parameter2 + "/" + id; break;
                case UserSubscriptionFunctionality.SubscriptionDetils: url = "api/UserSubscription/SubscriptionDetils/" + id; break;
                case UserSubscriptionFunctionality.GetSubscriptioDtlsForLicenseMap: url = "api/UserSubscription/GetSubscriptioDtlsForLicenseMap/" + parameter2 + "/" + id; break;
            }
            return url;
        }

        public string GetUserTokenUrl(string id, string functionality, string adminId)
        {
            string url = string.Empty;
            UserTokenFunctionality tokenfunctionality = (UserTokenFunctionality)Enum.Parse(typeof(UserTokenFunctionality), functionality);
            switch (tokenfunctionality)
            {
                case UserTokenFunctionality.All: url = "api/usertoken/All"; break;
                case UserTokenFunctionality.Create: url = "api/usertoken/create"; break;

            }
            return url;
        }
    }
}