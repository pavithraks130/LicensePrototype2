using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using Newtonsoft.Json;

namespace License.MetCalWeb.Logic
{
    public class AccountLogic
    {
        public string ErrorMessage { get; set; }

        public User ResetUserPassword(ResetPassword model, ServiceType type)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(type);
            var response = client.PostAsJsonAsync("api/user/ResetPassword", model).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<User>(jsondata);
                return user;
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
            }
            return null;
        }
        public ForgotPasswordToken GetForgotPasswordToken(ForgotPassword model, ServiceType type)
        {
            ErrorMessage = string.Empty;
            HttpClient client = WebApiServiceLogic.CreateClient(type);
            var response = client.PostAsJsonAsync("api/user/GetResetToken", model).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var passwordtoken = JsonConvert.DeserializeObject<ForgotPasswordToken>(jsonData);
                return passwordtoken;
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
            }
            return null;
        }

        public void UpdateLogoutStatus(string userId, ServiceType type)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(type);
            User userModel = new User();
            userModel.UserId = userId;
            userModel.IsActive = false;
            client.PutAsJsonAsync("api/user/UpdateActiveStatus", userModel);
        }

        public async Task<User> GetUserData(ServiceType webApiType)
        {
            ErrorMessage = string.Empty;
            AccessToken token = null;
            switch (webApiType)
            {
                case ServiceType.CentralizeWebApi: token = LicenseSessionState.Instance.CentralizedToken; break;
                case ServiceType.OnPremiseWebApi: token = LicenseSessionState.Instance.OnPremiseToken; break;
            }
            HttpClient client = WebApiServiceLogic.CreateClient(webApiType);
            var response = await client.GetAsync("api/user/UserById/" + token.Id);
            if (response.IsSuccessStatusCode)
            {
                var userJson = response.Content.ReadAsStringAsync().Result;
                client.Dispose();
                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
                return user;
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
            }
            return null;
        }

        public async Task<bool> AuthenticateUser(LoginViewModel model, ServiceType webApiType)
        {
            ErrorMessage = string.Empty;
            HttpClient client = WebApiServiceLogic.CreateClient(webApiType);
            var formContent = new FormUrlEncodedContent(new[] {
                                new KeyValuePair<string, string>("grant_type", "password"),
                                new KeyValuePair<string, string>("username", model.Email),
                                new KeyValuePair<string, string>("password", model.Password)
                            });
            var response = await client.PostAsync("Authenticate", formContent);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessToken>(data);
                switch (webApiType)
                {
                    case ServiceType.CentralizeWebApi: LicenseSessionState.Instance.CentralizedToken = token; break;
                    case ServiceType.OnPremiseWebApi: LicenseSessionState.Instance.OnPremiseToken = token; break;
                }
                return true;
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
            }
            return false;
        }

    }
}