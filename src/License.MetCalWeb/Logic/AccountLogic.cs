using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using License.MetCalWeb.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using License.Models;
using License.ServiceInvoke;

namespace License.MetCalWeb.Logic
{
    /// <summary>
    /// Logic class contaians the service call for the account controller for authentication, Reset password , Forgot Password
    /// </summary>
    public class AccountLogic
    {

        //private readonly string applicationCode = "kXtpZlZCESG7F8jo+uVuaA==";
        //private readonly string applicationSecretPass = "WebPortal";
        public string ErrorMessage { get; set; }

        //public User ResetUserPassword(ResetPassword model, ServiceType type)
        //{
        //    HttpClient client = WebApiServiceLogic.CreateClient(type);
        //    var response = client.PostAsJsonAsync("api/user/ResetPassword", model).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var jsondata = response.Content.ReadAsStringAsync().Result;
        //        var user = JsonConvert.DeserializeObject<User>(jsondata);
        //        return user;
        //    }
        //    else
        //    {
        //        var jsonData = response.Content.ReadAsStringAsync().Result;
        //        var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
        //        ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
        //    }
        //    return null;
        //}
        //public ForgotPasswordToken GetForgotPasswordToken(ForgotPassword model, ServiceType type)
        //{
        //    ErrorMessage = string.Empty;
        //    HttpClient client = WebApiServiceLogic.CreateClient(type);
        //    var response = client.PostAsJsonAsync("api/user/GetResetToken", model).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var jsonData = response.Content.ReadAsStringAsync().Result;
        //        var passwordtoken = JsonConvert.DeserializeObject<ForgotPasswordToken>(jsonData);
        //        return passwordtoken;
        //    }
        //    else
        //    {
        //        var jsonData = response.Content.ReadAsStringAsync().Result;
        //        var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
        //        ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
        //    }
        //    return null;
        //}

        //public void UpdateLogoutStatus(string userId, ServiceType type)
        //{
        //    if (String.IsNullOrEmpty(userId))
        //        return;
        //    HttpClient client = WebApiServiceLogic.CreateClientWithoutToken(type);
        //    User userModel = new User()
        //    {
        //        UserId = userId,
        //        IsActive = false
        //    };
        //    client.PutAsJsonAsync("api/user/UpdateActiveStatus", userModel);
        //}

        //public async Task<User> GetUserData(ServiceType webApiType)
        //{
        //    ErrorMessage = string.Empty;
        //    AccessToken token = null;
        //    switch (webApiType)
        //    {
        //        case ServiceType.CentralizeWebApi: token = LicenseSessionState.Instance.CentralizedToken; break;
        //        case ServiceType.OnPremiseWebApi: token = LicenseSessionState.Instance.OnPremiseToken; break;
        //    }
        //    HttpClient client = WebApiServiceLogic.CreateClient(webApiType);
        //    var response = await client.GetAsync("api/user/UserById/" + token.Id);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var userJson = response.Content.ReadAsStringAsync().Result;
        //        client.Dispose();
        //        var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
        //        return user;
        //    }
        //    else
        //    {
        //        var jsonData = response.Content.ReadAsStringAsync().Result;
        //        var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
        //        ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
        //    }
        //    return null;
        //}

        //public async Task<bool> AuthenticateUser(Login model, ServiceType webApiType)
        //{
        //    ErrorMessage = string.Empty;
        //    HttpClient client = WebApiServiceLogic.CreateClient(webApiType);
        //    var formContent = new FormUrlEncodedContent(new[] {
        //                        new KeyValuePair<string, string>("grant_type", "password"),
        //                        new KeyValuePair<string, string>("username", model.Email),
        //                        new KeyValuePair<string, string>("password", model.Password)
        //                    });
        //    if(webApiType == ServiceType.OnPremiseWebApi)
        //    {
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
        //       EncodeToBase64(string.Format("{0}:{1}",
        //       applicationCode,
        //       applicationSecretPass)));
        //    }

        //    var response = await client.PostAsync("Authenticate", formContent);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = response.Content.ReadAsStringAsync().Result;
        //        var token = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessToken>(data);
        //        switch (webApiType)
        //        {
        //            case ServiceType.CentralizeWebApi: LicenseSessionState.Instance.CentralizedToken = token; break;
        //            case ServiceType.OnPremiseWebApi: LicenseSessionState.Instance.OnPremiseToken = token; break;
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        var jsonData = response.Content.ReadAsStringAsync().Result;
        //        var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
        //        ErrorMessage = response.ReasonPhrase + " - " + obj.Message;
        //    }
        //    return false;
        //}

        //private static string EncodeToBase64(string value)
        //{
        //    var toEncodeAsBytes = Encoding.UTF8.GetBytes(value);
        //    return Convert.ToBase64String(toEncodeAsBytes);
        //}
    }
}