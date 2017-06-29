using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;
using System.Security.Cryptography;
using License.MetCalDesktop.Common;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using License.MetCalDesktop.Model;
using License.ServiceInvoke;

namespace License.MetCalDesktop.businessLogic
{
    public class LoginLogic
    {
        private string LoginFileName = "credential.txt";
        private string applicationCode = "zDzH/Enor7NV/QbSy6rNbw==";
        private string applicationSecretPass = "MetCal";
        private string featurefileName = "Products.txt";
        public string ErrorMessage { get; set; }

        private Authentication _authentication = null;
        private APIInvoke _invoke = null;
        private FileIO _fileIO = null;
        private UserLogic _userlogic = new UserLogic();

        public LoginLogic()
        {
            _invoke = new APIInvoke();
            _authentication = new Authentication()
            {
                ApplicationType = applicationSecretPass,
                ApplicationCode = applicationCode,
                CentralizedToken = AppState.Instance.CentralizedToken == null ? "" : AppState.Instance.CentralizedToken.access_token,
                OnpremiseToken = AppState.Instance.OnPremiseToken == null ? "" : AppState.Instance.OnPremiseToken.access_token
            };
            _fileIO = new FileIO();
        }

        public User AuthenticateUser(string email, string password)
        {
            User user = null;
            var response = _authentication.DesktopAuthentication<Login, User>(new Login() { Email = email, Password = password });
            if (String.IsNullOrEmpty(response.ErrorMessage))
            {
                user = response.User;
                AppState.Instance.User = user;
                AppState.Instance.IsSuperAdmin = user.Roles.Contains("SuperAdmin");

                AppState.Instance.OnPremiseToken = response.OnPremiseToken;
                AppState.Instance.CentralizedToken = response.CentralizedToken;
                _authentication.OnpremiseToken = response.OnPremiseToken == null ? "" : response.OnPremiseToken.access_token;
                _authentication.CentralizedToken = response.CentralizedToken == null ? "" : response.CentralizedToken.access_token;

                if (AppState.Instance.IsSuperAdmin && AppState.Instance.IsNetworkAvilable())
                    SynchPurchaseOrder(user.ServerUserId);
            }
            else
                ErrorMessage = response.ErrorMessage;
            return user;
        }
        //public UserExtended AuthenticateUser(string email, string password)
        //{
        //    if (FileIO.IsFileExist(LoginFileName))
        //    {
        //        var usersData = FileIO.GetJsonDataFromFile(LoginFileName);
        //        var usersList = JsonConvert.DeserializeObject<List<UserExtended>>(usersData);
        //        var user = usersList.FirstOrDefault(u => u.Email == email);
        //        if (user != null)
        //        {
        //            var hashPassword = CreatePasswordhash(password, user.ThumbPrint);
        //            if (user.PasswordHash == hashPassword)
        //            {
        //                ErrorMessage = "";
        //                return user;
        //            }
        //            else
        //            {
        //                ErrorMessage = "Invalid user name or password";
        //                return null;
        //            }
        //        }
        //    }

        //    ErrorMessage = "For first time login user need to be online or invalaid user name or password";
        //    return null;
        //}

        //public User AuthenticateOnline(string email, string password)
        //{
        //    User user = null;
        //    var response = _authentication.LoginAuthentication<Login, User>(new Login() { Email = email, Password = password });
        //    if (String.IsNullOrEmpty(response.ErrorMessage))
        //    {
        //        AppState.Instance.OnPremiseToken = response.OnPremiseToken;
        //        AppState.Instance.CentralizedToken = response.CentralizedToken;
        //        user = response.User;
        //        AppState.Instance.IsSuperAdmin = user.Roles.Contains("SuperAdmin");
        //        if (AppState.Instance.IsSuperAdmin)
        //            SynchPurchaseOrder(user.ServerUserId);
        //    }
        //    else
        //        ErrorMessage = response.ErrorMessage;
        //    //HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
        //    //var formData = new FormUrlEncodedContent(new[]{
        //    //            new KeyValuePair<string, string>("grant_type", "password"),
        //    //            new KeyValuePair<string, string>("username", email),
        //    //            new KeyValuePair<string, string>("password", password) });
        //    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
        //    //    EncodeToBase64(string.Format("{0}:{1}",
        //    //    applicationCode,
        //    //    applicationSecretPass)));
        //    //var response = client.PostAsync("/Authenticate", formData).Result;
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    var jsondata = response.Content.ReadAsStringAsync().Result;
        //    //    var token = JsonConvert.DeserializeObject<AccessToken>(jsondata);
        //    //    AppState.Instance.OnPremiseToken = token;
        //    //    client.Dispose();
        //    //    client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
        //    //    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);
        //    //    response = client.GetAsync("api/user/UserById/" + token.Id).Result;
        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        jsondata = response.Content.ReadAsStringAsync().Result;
        //    //        user = JsonConvert.DeserializeObject<UserExtended>(jsondata);
        //    //        AppState.Instance.IsSuperAdmin = false;
        //    //        if (user.Roles.Contains("SuperAdmin"))
        //    //        {
        //    //            client.Dispose();
        //    //            var formData1 = new FormUrlEncodedContent(new[]{
        //    //                new KeyValuePair<string, string>("grant_type", "password"),
        //    //                new KeyValuePair<string, string>("username", email),
        //    //                new KeyValuePair<string, string>("password", password) });
        //    //            client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
        //    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
        //    //    EncodeToBase64(string.Format("{0}:{1}", applicationCode, applicationSecretPass)));
        //    //            response = client.PostAsync("/Authenticate", formData1).Result;
        //    //            if (response.IsSuccessStatusCode)
        //    //            {
        //    //                jsondata = response.Content.ReadAsStringAsync().Result;
        //    //                token = JsonConvert.DeserializeObject<AccessToken>(jsondata);
        //    //                AppState.Instance.CentralizedToken = token;
        //    //            }
        //    //            AppState.Instance.IsSuperAdmin = true;
        //    //        }
        //    //        CreateCredentialFile(user, password);
        //    //        if (AppState.Instance.IsSuperAdmin)
        //    //            SynchPurchaseOrder(user.ServerUserId);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    ErrorMessage = "Invalid User Name or Password";
        //    //}
        //    return user;
        //}

        private void SynchPurchaseOrder(string userID)
        {
            string errorMessage = string.Empty;
            WebAPIRequest<SubscriptionList> request = new WebAPIRequest<SubscriptionList>()
            {
                AccessToken = AppState.Instance.CentralizedToken.access_token,
                Functionality = Functionality.syncpo,
                InvokeMethod = Method.GET,
                Id = AppState.Instance.User.ServerUserId,
                ServiceModule = Modules.PurchaseOrder,
                ServiceType = ServiceType.CentralizeWebApi
            };
            var response = _invoke.InvokeService<SubscriptionList, SubscriptionList>(request);
            if (response.Status)
            {
                if (response.ResponseData != null && response.ResponseData.Subscriptions != null && response.ResponseData.Subscriptions.Count > 0)
                    UpdateSubscriptionOnpremise(response.ResponseData);
            }
            else
                ErrorMessage = response.Error.error + " " + response.Error.Message;
            //HttpClient client = AppState.CreateClient(ServiceType.CentralizeWebApi.ToString());
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.CentralizedToken.access_token);
            //var response = client.GetAsync("api/purchaseorder/syncpo/" + userID).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<SubscriptionList>(jsonData);
            //    if (obj.Subscriptions.Count > 0)
            //        UpdateSubscriptionOnpremise(obj);
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    errorMessage = response.ReasonPhrase + " - " + obj.Message;
            //}
        }

        private void UpdateSubscriptionOnpremise(SubscriptionList subs)
        {
            string userId = string.Empty;
            subs.UserId = AppState.Instance.User.UserId;
            WebAPIRequest<SubscriptionList> request = new WebAPIRequest<SubscriptionList>()
            {
                AccessToken = AppState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.SynchronizeSubscription,
                InvokeMethod = Method.POST,
                Id = AppState.Instance.User.UserId,
                ServiceModule = Modules.UserSubscription,
                ServiceType = ServiceType.OnPremiseWebApi,
                ModelObject = subs
            };
            _invoke.InvokeService<SubscriptionList, String>(request);
            //HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            //var response = client.PostAsJsonAsync("api/UserSubscription/SyncSubscription", subs).Result;
        }

        //private string EncodeToBase64(string value)
        //{
        //    var toEncodeAsBytes = Encoding.UTF8.GetBytes(value);
        //    return Convert.ToBase64String(toEncodeAsBytes);
        //}

        //public async Task UpdateFeatureToFile()
        //{
        //    string featurefileName = "Products.txt";
        //    List<Product> userlicdtls = new List<Product>();
        //    if (FileIO.IsFileExist(featurefileName))
        //    {
        //        var featuresList = FileIO.GetJsonDataFromFile(featurefileName);
        //        var licenseDetails = JsonConvert.DeserializeObject<List<Product>>(featuresList);
        //        foreach (var pro in AppState.Instance.UserLicenseList)
        //        {
        //            var proObj = licenseDetails.FirstOrDefault(p => p.Id == pro.Id);
        //            if (proObj != null)
        //                licenseDetails.Remove(proObj);
        //            licenseDetails.Add(pro);
        //        }
        //        userlicdtls = licenseDetails;
        //    }
        //    else
        //        userlicdtls.AddRange(AppState.Instance.UserLicenseList);
        //    var jsonData = JsonConvert.SerializeObject(userlicdtls);
        //    FileIO.SaveDatatoFile(jsonData, featurefileName);
        //}

        public async Task GetUserDetails()
        {
            WebAPIRequest<UserDetails> request = new WebAPIRequest<UserDetails>()
            {
                AccessToken = AppState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.GetUserDetailsById,
                Id = AppState.Instance.User.UserId,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.User,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            var response = _userlogic.GetUserDetails(request);
            if (!response.Status)
                ErrorMessage = response.Error.error + " " + response.Error.Message;
            //HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            //var response = client.GetAsync("api/User/GetDetailsById/" + AppState.Instance.User.UserId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    UserDetails details = JsonConvert.DeserializeObject<UserDetails>(jsonData);
            //    if (details != null)
            //    {
            //        var fileName = AppState.Instance.User.UserId + ".txt";
            //        jsonData = JsonConvert.SerializeObject(details);
            //        FileIO.SaveDatatoFile(jsonData, fileName);
            //    }
            //}
        }

        public void UpdateFeatureToFile()
        {
            List<Models.Product> userlicdtls = new List<Models.Product>();
            var licenseDetails = _fileIO.GetDataFromFile<List<Product>>(featurefileName);
            if (licenseDetails != null)
            {
                //if (FileIO.IsFileExist(featurefileName))
                //{
                //    var featuresList = FileIO.GetJsonDataFromFile(featurefileName);
                //    var licenseDetails = JsonConvert.DeserializeObject<List<Models.Product>>(featuresList);
                foreach (var pro in AppState.Instance.UserLicenseList)
                {
                    var proObj = licenseDetails.FirstOrDefault(p => p.Id == pro.Id);
                    if (proObj != null)
                        licenseDetails.Remove(proObj);
                    licenseDetails.Add(pro);
                }
                userlicdtls = licenseDetails;
            }
            else
                userlicdtls.AddRange(AppState.Instance.UserLicenseList);
            _fileIO.SaveDatatoFile(userlicdtls, featurefileName);
            //_authentication.UpdateFeatureToFile(userlicdtls);
        }

    }
}
