using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace License.ServiceInvoke
{
    public class APIInvoke
    {
        internal HttpClient CreateClient(ServiceType serviceType)
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
        public WebAPIResponse<Q> InvokeService<T, Q>(WebAPIRequest<T> apiRequestData)
        {
            WebAPIResponse<Q> serviceResponse = new WebAPIResponse<Q>();
            var url = GetServiceUrl(apiRequestData.Id, apiRequestData.ServiceModule, apiRequestData.Functionality, apiRequestData.AdminId);

            HttpClient client = CreateClient(apiRequestData.ServiceType);

            if (!string.IsNullOrEmpty(apiRequestData.AccessToken))
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiRequestData.AccessToken);

            HttpResponseMessage response = null;
            switch (apiRequestData.InvokeMethod)
            {
                case Method.POST:
                    response = client.PostAsJsonAsync<T>(url, apiRequestData.ModelObject).Result;
                    break;
                case Method.PUT:
                    response = client.PutAsJsonAsync<T>(url, apiRequestData.ModelObject).Result;
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
                serviceResponse.ResponseData = response.Content.ReadAsAsync<Q>().Result;
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

        private String GetServiceUrl(string id, Modules type, Functionality functionality, string adminId = null)
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
                case Modules.Notification: url = GetNotificationUrl(id, functionality, adminId); break;
                case Modules.VISMAData: url = GetVISMADataUrl(id, functionality, adminId); break;
                case Modules.Role: url = GetRoleUrl(id, functionality, adminId); break;
            }
            return url;
        }

        private string GetAssetUrl(string id, Functionality functionality, string adminId)
        {
            string url = String.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/asset/All"; break;
                case Functionality.GetById: url = "api/asset/GetById/" + id; break;
                case Functionality.Update: url = "api/asset/Update/" + id; break;
                case Functionality.Delete: url = "api/asset/Delete/" + id; break;
                case Functionality.Create: url = "api/asset/Create"; break;
            }
            return url;
        }

        private string GetCartUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.Create: url = "api/Cart/Create"; break;
                case Functionality.Delete: url = "api/cart/Delete/" + id; break;
                case Functionality.OfflinePayment: url = "api/cart/offlinepayment/" + id; break;
                case Functionality.OnlinePayment: url = "api/cart/OnlinePayment/" + id; break;
                case Functionality.GetByUser: url = "api/cart/GetItemsByUser/" + id; break;
                case Functionality.GetCartItemsCount: url = "api/Cart/GetCartItemCount/" + id; break;
                case Functionality.CreateSubscriptionAddToCart: url = "api/cart/CreateSubscriptionAddToCart"; break;
            }
            return url;
        }

        private string GetFeatureUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/Feature/All"; break;
                case Functionality.Create: url = "api/Feature/Create"; break;
                case Functionality.GetById: url = "api/feature/GetbyId/" + id; break;
                case Functionality.Update: url = "api/feature/Update/" + id; break;
                case Functionality.Delete: url = "api/Feature/Delete/" + id; break;
                case Functionality.GetByCategory: url = "api/Feature/GetByCategory/" + id; break;
            }
            return url;
        }

        private string GetProductUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/Product/All"; break;
                case Functionality.Create: url = "api/product/create"; break;
                case Functionality.GetById: url = "api/product/GetById/" + id; break;
                case Functionality.Update: url = "api/product/update/" + id; break;
                case Functionality.ProductDependency: url = "api/Product/ProductDependency"; break;
                case Functionality.Delete: url = "api/product/delete/" + id; break;
                case Functionality.GetProductsWithUserMappedProduct: url = "api/Product/GetProductsWithUserMappedProduct/" + adminId + "/" + id; break;
                case Functionality.GetProducts: url = "api/Product/GetProducts/" + id; break;
                case Functionality.GetCMMSProducts: url = "api/Product/GetCMMSProducts"; break;
            }
            return url;

        }

        private string GetPurchaseOrderUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/purchaseorder/All"; break;
                case Functionality.UpdataMuliplePO: url = "/api/purchaseorder/UpdataMuliplePO"; break;
                case Functionality.OrderByUser: url = "api/purchaseorder/OrderByUser/" + id; break;
                case Functionality.GetById: url = "api/purchaseorder/GetById/" + id; break;
                case Functionality.syncpo: url = "api/purchaseorder/syncpo/" + id; break;

            }
            return url;
        }

        private string GetSubscriptionCategoryUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/SubscriptionCategory/All"; break;
                case Functionality.Create: url = "api/SubscriptionCategory/create"; break;
                case Functionality.GetById: url = "api/SubscriptionCategory/GetById/" + id; break;
                case Functionality.Update: url = "api/SubscriptionCategory/update/" + id; break;
                case Functionality.Delete: url = "api/SubscriptionCategory/Delete/" + id; break;
            }
            return url;
        }

        private string GetSubscriptionUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/subscription/All"; break;
                case Functionality.GetByUser: url = "api/subscription/All/" + id; break;
                case Functionality.Create: url = "api/subscription/Create"; break;
            }
            return url;
        }

        private string GetTeamUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.Create: url = "api/Team/Create"; break;
                case Functionality.Update: url = "api/team/Update/" + id; break;
                case Functionality.Delete: url = "api/team/Delete/" + id; break;
                case Functionality.UpdateConcurentUser: url = "api/team/UpdateConcurentUser"; break;
                case Functionality.GetById: url = "api/Team/GetById/" + id; break;
                case Functionality.GetTeamsByAdminId: url = "api/Team/GetTeamsByAdminId/" + id; break;
                case Functionality.GetTeamsByUserId: url = "api/Team/GetTeamsByUserId/" + id; break;

            }
            return url;
        }

        private string GetTeamLicenseUrl(string id, Functionality functionality, string adminId)
        {
            string url = String.Empty;
            switch (functionality)
            {
                case Functionality.Assign: url = "api/TeamLicense/Create"; break;
                case Functionality.Revoke: url = "api/TeamLicense/Revoke"; break;
                case Functionality.GetTeamLicenseByTeam: url = "api/TeamLicense/GetTeamLicenseByTeam/" + id; break;
            }
            return url;
        }

        private string GetTeamMemberUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.Create: url = "api/TeamMember/Create"; break;
                case Functionality.Assign: url = "api/TeamMember/Assign"; break;
                case Functionality.UpdateAdminAccess: url = "api/TeamMember/UpdateAdminAccess"; break;
                case Functionality.Delete: url = "api/TeamMember/Delete/" + id; break;
                case Functionality.Revoke: url = "api/TeamMember/Revoke"; break;
                case Functionality.UpdateInvite: url = "api/TeamMember/UpdateInvitation"; break;
            }
            return url;
        }

        private string GetUserUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/user/All"; break;
                case Functionality.Update: url = "/api/user/update/" + id; break;
                case Functionality.ChangePassword: url = "api/user/ChangePassword/" + id; break;
                case Functionality.GetById: url = "api/user/UserById/" + id; break;
                case Functionality.ForgotPassword: url = "api/user/GetResetToken"; break;
                case Functionality.ResetPassword: url = "api/user/ResetPassword"; break;
                case Functionality.UpdateConcurentUser: url = "api/User/IsConcurrentUserLoggedIn"; break;
                case Functionality.UpdateLogoutStatus: url = "api/user/UpdateActiveStatus"; break;
                case Functionality.Register: url = "api/user/Create"; break;
                case Functionality.GetUserDetailsById: url = "api/User/GetDetailsById/" + id; break;
            }
            return url;
        }
        private string GetRoleUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/Role/All"; break;
                case Functionality.Create: url = "api/role/create"; break;
                case Functionality.Update: url = "api/Role/update/" + id; break;
                case Functionality.GetById: url = "api/role/GetById/" + id; break;
                case Functionality.Delete: url = "api/role/Delete/" + id; break;
            }
            return url;
        }

        private string GetUserLicenseUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.GetRequestByTeam: url = "api/UserLicense/GetRequestByTeam/" + id; break;
                case Functionality.ApproveReject: url = "api/UserLicense/ApproveReject"; break;
                case Functionality.Assign: url = "api/UserLicense/Create"; break;
                case Functionality.Revoke: url = "api/UserLicense/Revoke"; break;
                case Functionality.LicenseRequest: url = "api/UserLicense/LicenseRequest"; break;
                case Functionality.GetRequestStatus: url = "api/UserLicense/GetRequestStatus/" + id; break;
                case Functionality.GetUserLicenseByUser: url = "api/UserLicense/GetUserLicenseByUser"; break;
            }
            return url;
        }

        private string GetUserSubscriptionUrl(string id, Functionality functionality, string parameter2)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.RenewSubscription: url = "api/UserSubscription/RenewSubscription/" + id; break;
                case Functionality.UpdateSubscriptionRenewal: url = "api/UserSubscription/UpdateSubscriptionRenewal/" + id; break;
                case Functionality.SynchronizeSubscription: url = "api/UserSubscription/SyncSubscription"; break;
                case Functionality.ExpireSubscription: url = "api/UserSubscription/ExpireSubscription/" + parameter2 + "/" + id; break;
                case Functionality.SubscriptionDetils: url = "api/UserSubscription/SubscriptionDetils/" + id; break;
                case Functionality.GetSubscriptioDtlsForLicenseMap: url = "api/UserSubscription/GetSubscriptioDtlsForLicenseMap/" + parameter2 + "/" + id; break;
            }
            return url;
        }

        private string GetUserTokenUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/usertoken/All"; break;
                case Functionality.Create: url = "api/usertoken/create"; break;
            }
            return url;
        }

        private string GetNotificationUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/Notification/GetAllNotification"; break;
                case Functionality.Create: url = "api/Notification/Create"; break;
            }
            return url;
        }

        private string GetVISMADataUrl(string id, Functionality functionality, string adminId)
        {
            string url = string.Empty;
            switch (functionality)
            {
                case Functionality.All: url = "api/VISMAData/GetAllVISMAData"; break;
                case Functionality.UploadFile: url = "api/VISMAData/UploadFile"; break;
                case Functionality.GetVISMADataByTestDevice: url = "api/VISMAData/GetVISMADataByTestDevice/" + id; break;
            }
            return url;
        }
    }
}
