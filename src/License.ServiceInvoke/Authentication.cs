using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using LicenseKey;
using System.Security.Cryptography;


namespace License.ServiceInvoke
{
    public class Authentication
    {
        public string ApplicationCode { get; set; }
        public string ApplicationType { get; set; }// = "WebPortal";
        public string CentralizedToken { get; set; }
        public string OnpremiseToken { get; set; }

        private string LoginFileName = "credential.txt";
        private string featurefileName = "Products.txt";
        private APIInvoke _invoke = null;
        private UserLogic _userLogic = null;
        private FileIO _fileIO = null;

        public Authentication()
        {
            _invoke = new APIInvoke();
            _userLogic = new UserLogic();
            _fileIO = new FileIO();
        }
        public String ErrorMessage { get; set; }

        public AuthenticationResponse<Q> LoginAuthentication<T, Q>(Login loginModel)
        {
            AuthenticationResponse<Q> response = new AuthenticationResponse<Q>();
            var onPremiseToken = AuthenticateUser(loginModel, ServiceType.OnPremiseWebApi);
            if (onPremiseToken != null)
                response.OnPremiseToken = onPremiseToken;
            var centralizedToken = AuthenticateUser(loginModel, ServiceType.CentralizeWebApi);
            if (centralizedToken != null)
                response.CentralizedToken = centralizedToken;
            if (centralizedToken == null && onPremiseToken == null)
            {
                response.ErrorMessage = ErrorMessage;
                return response;
            }
            else
                response.ErrorMessage = "";

            // User record fetch
            WebAPIRequest<T> apiRequest = new WebAPIRequest<T>()
            {
                InvokeMethod = Method.GET,
                ServiceModule = Modules.User,
                Functionality = Functionality.GetById
            };
            if (response.OnPremiseToken != null)
            {
                apiRequest.AccessToken = response.OnPremiseToken.access_token;
                apiRequest.Id = response.OnPremiseToken.Id;
                apiRequest.ServiceType = ServiceType.OnPremiseWebApi;
            }
            else
            {
                apiRequest.AccessToken = response.CentralizedToken.access_token;
                apiRequest.Id = response.CentralizedToken.Id;
                apiRequest.ServiceType = ServiceType.CentralizeWebApi;
            }
            var apiResponse = _invoke.InvokeService<T, Q>(apiRequest);
            if (apiResponse.Error == null)
            {
                response.User = apiResponse.ResponseData;
                if (ApplicationType == "MetCal")
                    _userLogic.CreateCredentialFile(apiResponse.ResponseData, loginModel.Password, LoginFileName);
            }
            else
            {
                ErrorMessage = apiResponse.Error.Message;
                return null;
            }
            return response;
        }

        public AuthenticationResponse<Q> AuthenticateUserOffline<Q>(Login loginModel)
        {
            AuthenticationResponse<Q> response = new AuthenticationResponse<Q>();

            if (_fileIO.IsFileExist(LoginFileName))
            {
                var usersList = _fileIO.GetDataFromFile<List<Models.User>>(LoginFileName);
                var user = usersList.FirstOrDefault(u => u.Email == loginModel.Email);
                if (user != null)
                {
                    var hashPassword = _userLogic.CreatePasswordhash(loginModel.Password, user.ThumbPrint);
                    if (user.PasswordHash == hashPassword)
                    {
                        ErrorMessage = "";
                        response.User = (Q)Convert.ChangeType(user, typeof(Q));
                        return response;
                    }
                    else
                    {
                        response.ErrorMessage = "Invalid user name or password";
                        return response;
                    }
                }
            }

            response.ErrorMessage = "For first time login user need to be online or invalaid user name or password";
            return response;
        }

        public AuthenticationResponse<Q> DesktopAuthentication<T, Q>(Login loginModel)
        {
            AuthenticationResponse<Q> response = new AuthenticationResponse<Q>();
            if (IsNetworkAvailable())
                response = LoginAuthentication<T, Q>(loginModel);
            else
                response = AuthenticateUserOffline<Q>(loginModel);
            return response;
        }

        private AccessToken AuthenticateUser(Login model, ServiceType webApiType)
        {

            HttpClient client = _invoke.CreateClient(webApiType);
            var formContent = new FormUrlEncodedContent(new[] {
                                new KeyValuePair<string, string>("grant_type", "password"),
                                new KeyValuePair<string, string>("username", model.Email),
                                new KeyValuePair<string, string>("password", model.Password)
                            });
            if (webApiType == ServiceType.OnPremiseWebApi)
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + EncodeToBase64(string.Format("{0}:{1}", ApplicationCode, ApplicationType)));

            var response = client.PostAsync("Authenticate", formContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessToken>(data);
                return token;
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ErrorMessage = ErrorMessage + " " + obj.error + " " + obj.Message;
            }
            return null;
        }


        public WebAPIResponse<T> Register<T>(T register)
        {
            WebAPIRequest<T> request = new WebAPIRequest<T>()
            {
                ServiceType = ServiceType.CentralizeWebApi,
                Functionality = Functionality.Register,
                InvokeMethod = Method.POST,
                ServiceModule = Modules.User,
                ModelObject = register
            };
            var response = _invoke.InvokeService<T, T>(request);
            if (response.Status)
            {
                request.ServiceType = ServiceType.OnPremiseWebApi;
                request.ModelObject = (T)response.ResponseData;
                response = _invoke.InvokeService<T, T>(request);
            }
            return response;
        }

        public WebAPIResponse<T> ChangePassword<T>(T changePassword, Models.User user)
        {
            WebAPIRequest<T> request = new WebAPIRequest<T>()
            {
                ServiceType = user.Roles.Contains("BackendAdmin") ? ServiceType.CentralizeWebApi : ServiceType.OnPremiseWebApi,
                Functionality = Functionality.ChangePassword,
                InvokeMethod = Method.PUT,
                ServiceModule = Modules.User,
                AccessToken = user.Roles.Contains("BackendAdmin") ? CentralizedToken : OnpremiseToken,
                ModelObject = changePassword,
                Id = user.UserId
            };
            var response = _invoke.InvokeService<T, T>(request);
            if (response.Status && !String.IsNullOrEmpty(user.ServerUserId))
            {
                request.ServiceType = ServiceType.CentralizeWebApi;
                request.AccessToken = CentralizedToken;
                request.Id = user.ServerUserId;
                response = _invoke.InvokeService<T, T>(request);
            }
            return response;
        }

        public WebAPIResponse<T> ForgotPassword<T>(T forgottenPassword)
        {
            WebAPIRequest<T> request = new WebAPIRequest<T>()
            {
                ServiceType = ServiceType.OnPremiseWebApi,
                Functionality = Functionality.ForgotPassword,
                InvokeMethod = Method.POST,
                ServiceModule = Modules.User,
                ModelObject = forgottenPassword
            };
            var response = _invoke.InvokeService<T, T>(request);
            if (!response.Status)
            {
                request.ServiceType = ServiceType.CentralizeWebApi;
                response = _invoke.InvokeService<T, T>(request);
            }
            return response;
        }

        public WebAPIResponse<License.Models.ResetPassword> ResetPasswordAction(License.Models.ResetPassword resetPassword)
        {
            WebAPIRequest<License.Models.ResetPassword> request = new WebAPIRequest<Models.ResetPassword>();
            request.ServiceType = ServiceType.OnPremiseWebApi;
            request.Functionality = Functionality.ResetPassword;
            request.InvokeMethod = Method.POST;
            request.ServiceModule = Modules.User;
            request.ModelObject = resetPassword;

            var response = _invoke.InvokeService<Models.ResetPassword, Models.ResetPassword>(request);
            if (!response.Status || !String.IsNullOrEmpty((response.ResponseData as License.Models.ResetPassword).ServerUserId))
            {
                request.ServiceType = ServiceType.CentralizeWebApi;
                resetPassword.UserId = (response.ResponseData as License.Models.ResetPassword).ServerUserId;
                resetPassword.Token = string.Empty;
                response = _invoke.InvokeService<License.Models.ResetPassword, Models.ResetPassword>(request);
            }
            return response;
        }

        public WebAPIResponse<Models.User> UpdateProfile(Models.User user)
        {
            WebAPIRequest<Models.User> request = new WebAPIRequest<Models.User>()
            {
                ServiceType = user.Roles.Contains("BackendAdmin") ? ServiceType.CentralizeWebApi : ServiceType.OnPremiseWebApi,
                Functionality = Functionality.Update,
                InvokeMethod = Method.PUT,
                ServiceModule = Modules.User,
                AccessToken = user.Roles.Contains("BackendAdmin") ? CentralizedToken : OnpremiseToken,
                ModelObject = user,
                Id = user.UserId
            };
            var response = _invoke.InvokeService<Models.User, Models.User>(request);
            if (response.Status && !String.IsNullOrEmpty(user.ServerUserId))
            {
                request.ServiceType = ServiceType.CentralizeWebApi;
                request.AccessToken = CentralizedToken;
                response = _invoke.InvokeService<Models.User, Models.User>(request);
            }
            return response;
        }

        private static string EncodeToBase64(string value)
        {
            var toEncodeAsBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public void LogoutUser(License.Models.User userObj)
        {
            WebAPIRequest<Models.User> request = new WebAPIRequest<Models.User>()
            {
                ServiceType = userObj.Roles.Contains("BackendAdmin") ? ServiceType.CentralizeWebApi : ServiceType.OnPremiseWebApi,
                Functionality = Functionality.UpdateLogoutStatus,
                InvokeMethod = Method.PUT,
                ServiceModule = Modules.User,
                ModelObject = userObj
            };
            var response = _invoke.InvokeService<Models.User, Models.User>(request);
            if (response.Status && !String.IsNullOrEmpty(userObj.ServerUserId))
            {
                request.ServiceType = ServiceType.CentralizeWebApi;
                userObj.UserId = userObj.ServerUserId;
                response = _invoke.InvokeService<Models.User, Models.User>(request);
            }
        }

        public bool IsNetworkAvailable()
        {
            try
            {
                using (var client = new System.Net.WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
