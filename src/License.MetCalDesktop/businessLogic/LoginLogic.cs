using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.MetCalDesktop.Model;
using System.Security.Cryptography;
using License.MetCalDesktop.Common;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace License.MetCalDesktop.businessLogic
{
    public class LoginLogic
    {
        private string LoginFileName = "credential.txt";
        private string applicationCode = "zDzH/Enor7NV/QbSy6rNbw==";
        private string applicationSecretPass = "MetCal";
        public string ErrorMessage { get; set; }

        public void CreateCredentialFile(User user, string password)
        {
            List<User> userList = new List<User>();
            string thumbPrint = string.Empty;
            user.PasswordHash = HashPassword(password, out thumbPrint);
            user.ThumbPrint = thumbPrint;
            if (FileIO.IsFileExist(LoginFileName))
            {
                var userListData = FileIO.GetJsonDataFromFile(LoginFileName);
                userList = JsonConvert.DeserializeObject<List<User>>(userListData);
            }
            if (!userList.Any(u => u.Email == user.Email))
                userList.Add(user);
            
            var jsonData = JsonConvert.SerializeObject(userList);
            FileIO.SaveDatatoFile(jsonData, LoginFileName);
        }

        public string HashPassword(string password, out string thumbprint)
        {
            int size = 10;
            thumbprint = CreateSalt(size);
            return CreatePasswordhash(password, thumbprint);
        }

        public string CreateSalt(int size)
        {
            byte[] bytedata = new byte[size];
            var rngProvider = new RNGCryptoServiceProvider();
            rngProvider.GetBytes(bytedata);
            return Convert.ToBase64String(bytedata);
        }

        public string CreatePasswordhash(string password, string thumbPrint)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password + thumbPrint);
            SHA256Managed sha256 = new SHA256Managed();
            var byteHashData = sha256.ComputeHash(bytes);
            return ByteArrayToHexString(byteHashData);
        }

        public String ByteArrayToHexString(byte[] bytes)
        {
            int length = bytes.Length;

            char[] chars = new char[length << 1];

            for (int i = 0, j = 0; i < length; i++, j++)
            {
                byte b = (byte)(bytes[i] >> 4);
                chars[j] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                j++;

                b = (byte)(bytes[i] & 0x0F);
                chars[j] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }

            return new String(chars);
        }

        public User AuthenticateUser(string email, string password)
        {
            if (FileIO.IsFileExist(LoginFileName))
            {
                var usersData = FileIO.GetJsonDataFromFile(LoginFileName);
                var usersList = JsonConvert.DeserializeObject<List<User>>(usersData);
                var user = usersList.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    var hashPassword = CreatePasswordhash(password, user.ThumbPrint);
                    if (user.PasswordHash == hashPassword)
                    {
                        ErrorMessage = "";
                        return user;
                    }
                    else
                    {
                        ErrorMessage = "Invalid user name or password";
                        return null;
                    }
                }
            }

            ErrorMessage = "For first time login user need to be online or invalaid user name or password";
            return null;
        }

        public User AuthenticateOnline(string email, string password)
        {
            User user = null;
            HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            var formData = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", email),
                        new KeyValuePair<string, string>("password", password) });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                EncodeToBase64(string.Format("{0}:{1}",
                applicationCode,
                applicationSecretPass)));
            var response = client.PostAsync("/Authenticate", formData).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var token = JsonConvert.DeserializeObject<AccessToken>(jsondata);
                AppState.Instance.OnPremiseToken = token;
                client.Dispose();
                client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);
                response = client.GetAsync("api/user/UserById/" + token.Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    jsondata = response.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<User>(jsondata);
                    AppState.Instance.IsSuperAdmin = false;
                    if (user.Roles.Contains("SuperAdmin"))
                    {
                        client.Dispose();
                        var formData1 = new FormUrlEncodedContent(new[]{
                            new KeyValuePair<string, string>("grant_type", "password"),
                            new KeyValuePair<string, string>("username", email),
                            new KeyValuePair<string, string>("password", password) });
                        client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                EncodeToBase64(string.Format("{0}:{1}",
                applicationCode,
                applicationSecretPass)));
                        response = client.PostAsync("/Authenticate", formData1).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            jsondata = response.Content.ReadAsStringAsync().Result;
                            token = JsonConvert.DeserializeObject<AccessToken>(jsondata);
                            AppState.Instance.CentralizedToken = token;
                        }

                        AppState.Instance.IsSuperAdmin = true;
                    }

                    CreateCredentialFile(user, password);
                    if (AppState.Instance.IsSuperAdmin)
                    {
                        SynchPurchaseOrder(user.ServerUserId);
                    }
                }
            }
            else
            {
                ErrorMessage = "Invalid User Name or Password";
            }
            return user;
        }

        private void SynchPurchaseOrder(string userID)
        {
            string errorMessage = string.Empty;
            HttpClient client = AppState.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.CentralizedToken.access_token);
            var response = client.GetAsync("api/purchaseorder/syncpo/" + userID).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<SubscriptionList>(jsonData);
                if (obj.Subscriptions.Count > 0)
                    UpdateSubscriptionOnpremise(obj);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                errorMessage = response.ReasonPhrase + " - " + obj.Message;
            }
        }

        private void UpdateSubscriptionOnpremise(SubscriptionList subs)
        {
            string userId = string.Empty;
            userId = AppState.Instance.User.UserId;
            List<UserSubscriptionData> subscriptionData = new List<UserSubscriptionData>();
            foreach (var subDtls in subs.Subscriptions)
            {
                //Code to save the user Subscription details to Database.
                UserSubscriptionData userSubscription = new UserSubscriptionData();
                userSubscription.SubscriptionDate = subDtls.SubscriptionDate;
                userSubscription.SubscriptionId = subDtls.SubscriptionTypeId;
                userSubscription.UserId = userId;
                userSubscription.Quantity = subDtls.OrderdQuantity;
                userSubscription.Subscription = subDtls;
                userSubscription.LicenseKeys = subDtls.LicenseKeyProductMapping;
                subscriptionData.Add(userSubscription);
            }
            HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("api/UserSubscription/SyncSubscription", subscriptionData).Result;
        }
        private static string EncodeToBase64(string value)
        {
            var toEncodeAsBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(toEncodeAsBytes);
        }
    }
}
