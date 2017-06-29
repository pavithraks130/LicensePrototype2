
using License.MetCalDesktop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using License.MetCalDesktop.Model;
using License.MetCalDesktop.businessLogic;
using License.Models;
using License.ServiceInvoke;
namespace License.MetCalDesktop.ViewModel
{
    class LoginViewModel : BaseEntity
    {
        #region private field

        private string _email;

        private string _password;

        private bool _isEnableLogin = true;
        LoginLogic logic = null;
        private APIInvoke _invoke = null;
        private FileIO _fileIO = null;
        #endregion private field

        /// <summary>
        ///Initialising LoginUserViewModel 
        /// </summary>
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginUser);
            _invoke = new APIInvoke();
            _fileIO = new FileIO();
        }

        /// <summary>
        ///user login  email id 
        /// </summary>
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        /// <summary>
        ///user login password
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        /// <summary>
        /// Login action property
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// SignUp new user property
        /// </summary>
        public ICommand SignUpNewUser { get; set; }

        /// <summary>
        /// Login button enable or disable action property
        /// </summary>
        public bool IsEnableLogin
        {
            get
            {
                return _isEnableLogin;
            }

            set
            {
                _isEnableLogin = value;
                OnPropertyChanged("IsEnableLogin");
            }
        }


        #region IDataErrorInfo

        /// <summary>
        /// Error validation
        /// </summary>
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// property validation
        /// </summary>
        /// <param name="columnName">columnName:Property</param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                string message = "Email" == columnName ? EmailValidation() : PasswordValidation();
                return message;
            }
        }

        #endregion IDataErrorInfo

        /// <summary>
        /// Email id validation
        /// </summary>
        /// <returns></returns>
        private string EmailValidation()
        {
            if (string.IsNullOrEmpty(Email))
            {
                return "Email address field is empty";
            }
            else if (!DataValidations.IsValidEmailId(Email))
            {
                return "Please enter valid email address";
            }
            return string.Empty;

        }

        /// <summary>
        /// Password validation
        /// </summary>
        /// <returns></returns>
        private string PasswordValidation()
        {
            if (string.IsNullOrEmpty(Password))
            {
                return "Password field is empty";
            }
            return string.Empty;

        }

        /// <summary>
        /// Login user action
        /// </summary>
        /// <param name="param"></param>
        public void LoginUser(object param)
        {
            logic = new LoginLogic();
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {

                IsEnableLogin = false;
                UserExtended user = null;

                var response = logic.AuthenticateUser(Email, Password);
                //if (AppState.Instance.IsNetworkAvilable())
                //    user = logic.AuthenticateOnline(Email, Password);
                //else if (AppState.Instance.IsCredentialFileExist())
                //    user = logic.AuthenticateUser(Email, Password);
                if (response == null)
                {
                    MessageBox.Show(logic.ErrorMessage);
                    IsEnableLogin = true;
                    return;
                }
                AppState.Instance.IsUserLoggedIn = true;
                if (AppState.Instance.IsNetworkAvilable())
                    LoadTeamsOnline();
                else
                    LoadTeamsOffline();
                IsEnableLogin = true;
            }
        }

        public void LoadTeamsOnline()
        {
            List<Team> teamList = new List<Team>();
            WebAPIRequest<List<Team>> request = new WebAPIRequest<List<Team>>()
            {
                AccessToken = AppState.Instance.OnPremiseToken.access_token,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.Team,
                ServiceType = ServiceType.OnPremiseWebApi,
                Id = AppState.Instance.User.UserId,
                Functionality = AppState.Instance.IsSuperAdmin ? Functionality.GetTeamsByAdminId : Functionality.GetTeamsByUserId
            };
            var response = _invoke.InvokeService<List<Team>, List<Team>>(request);

            if (response.Status)
            {
                teamList = response.ResponseData;
                if (teamList.Count > 1)
                {
                    AppState.Instance.TeamList = teamList;
                    Views.Teams teamWindow = new Views.Teams();
                    teamWindow.ClosePopupWindow += CloseWindow;
                    teamWindow.ShowDialog();

                }
                else if (teamList.Count == 1)
                {
                    AppState.Instance.SelectedTeam = teamList.FirstOrDefault();
                    ConcurrentUserLogin userLogin = new ConcurrentUserLogin();
                    userLogin.TeamId = AppState.Instance.SelectedTeam.Id;
                    userLogin.UserId = AppState.Instance.User.UserId;

                    WebAPIRequest<ConcurrentUserLogin> _request = new WebAPIRequest<ConcurrentUserLogin>()
                    {
                        AccessToken = AppState.Instance.OnPremiseToken.access_token,
                        Functionality = Functionality.UpdateConcurentUser,
                        InvokeMethod = Method.POST,
                        ModelObject = userLogin,
                        ServiceModule = Modules.User,
                        ServiceType = ServiceType.OnPremiseWebApi
                    };
                    var response1 = _invoke.InvokeService<ConcurrentUserLogin, ConcurrentUserLogin>(_request);
                    if (response1.Error != null)
                    {
                        MessageBox.Show(response.Error.error + " " + response.Error.Message);
                        return;
                    }
                    //client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                    //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
                    //response = client.PostAsJsonAsync("api/User/IsConcurrentUserLoggedIn", userLogin).Result;
                    //jsonData = response.Content.ReadAsStringAsync().Result;
                    //var userLoginObj = JsonConvert.DeserializeObject<ConcurrentUserLogin>(jsonData);
                    var userLoginObj = response1.ResponseData;
                    AppState.Instance.UserLicenseList = userLoginObj.Products;
                    logic.UpdateFeatureToFile();
                    logic.GetUserDetails();
                    if (userLoginObj.IsUserLoggedIn)
                        NavigateNextPage?.Invoke("Dashboard", null);
                    else
                        MessageBox.Show(userLoginObj.ErrorOrNotificationMessage);
                }
            }
            //HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            //string url = "api/Team/GetTeamsByUserId/";
            //if (AppState.Instance.IsSuperAdmin)
            //    url = "api/Team/GetTeamsByAdminId/";
            //var response = client.GetAsync(url + AppState.Instance.User.UserId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var teamList = JsonConvert.DeserializeObject<List<Team>>(jsonData);
            //    if (teamList.Count > 1)
            //    {
            //        AppState.Instance.TeamList = teamList;
            //        Views.Teams teamWindow = new Views.Teams();
            //        teamWindow.ClosePopupWindow += CloseWindow;
            //        teamWindow.ShowDialog();

            //    }
            //    else if (teamList.Count == 1)
            //    {
            //        AppState.Instance.SelectedTeam = teamList.FirstOrDefault();
            //        ConcurrentUserLogin userLogin = new ConcurrentUserLogin();
            //        userLogin.TeamId = AppState.Instance.SelectedTeam.Id;
            //        userLogin.UserId = AppState.Instance.User.UserId;

            //        WebAPIRequest<ConcurrentUserLogin> _request = new WebAPIRequest<ConcurrentUserLogin>()
            //        {
            //            AccessToken = AppState.Instance.OnPremiseToken.access_token,
            //            Functionality = Functionality.UpdateConcurentUser,
            //            InvokeMethod = Method.POST,
            //            ModelObject = userLogin,
            //            ServiceModule = Modules.User,
            //            ServiceType = ServiceType.OnPremiseWebApi
            //        };
            //        var response = _invoke.InvokeService<ConcurrentUserLogin, ConcurrentUserLogin>(_request);
            //        if (response.Error != null)
            //            return null;
            //        //client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            //        //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            //        //response = client.PostAsJsonAsync("api/User/IsConcurrentUserLoggedIn", userLogin).Result;
            //        //jsonData = response.Content.ReadAsStringAsync().Result;
            //        //var userLoginObj = JsonConvert.DeserializeObject<ConcurrentUserLogin>(jsonData);
            //        var userLoginObj = response.ResponseData;
            //        AppState.Instance.UserLicenseList = userLoginObj.Products;
            //        logic.UpdateFeatureToFile();
            //        logic.GetUserDetails();
            //        if (userLoginObj.IsUserLoggedIn)
            //            NavigateNextPage?.Invoke("Dashboard", null);
            //        else
            //            MessageBox.Show(userLoginObj.ErrorOrNotificationMessage);
            //    }

            //}
        }

        public void LoadTeamsOffline()
        {
            //var jsonData = FileIO.GetJsonDataFromFile(AppState.Instance.User.UserId + ".txt");
            //var details = JsonConvert.DeserializeObject<UserDetails>(jsonData);
            var details = _fileIO.GetDataFromFile<UserDetails>(AppState.Instance.User.UserId + ".txt");
            if (details == null)
                NavigateNextPage?.Invoke("Dashboard", null);
            if (details.Teams.Count == 1)
            {
                AppState.Instance.SelectedTeam = details.Teams[0];
                var licenseList = details.UserLicenses.Where(l => l.TeamId == AppState.Instance.SelectedTeam.Id).ToList();
                var prodId = licenseList.Select(l => l.License.ProductId).Distinct().ToList();
                DashboardLogic dashboardLogic = new DashboardLogic();
                var proList = dashboardLogic.LoadFeatureOffline();
                if (proList != null && proList.Count > 0)
                    AppState.Instance.UserLicenseList = proList.Where(p => prodId.Contains(p.Id)).ToList();
                NavigateNextPage?.Invoke("Dashboard", null);
            }
            else
            {
                AppState.Instance.TeamList = details.Teams;
                Views.Teams teamWindow = new Views.Teams();
                teamWindow.ClosePopupWindow += CloseWindow;
                teamWindow.ShowDialog();
            }
        }

        private void CloseWindow(object sender, EventArgs e1)
        {

            (sender as System.Windows.Window).Close();
            if (AppState.Instance.UserLogin == null)
                return;
            if (AppState.Instance.UserLogin.IsUserLoggedIn)
                NavigateNextPage?.Invoke("Dashboard", null);
            else
            {
                AppState.Instance.User = null;
                AppState.Instance.UserLicenseList = null;
                AppState.Instance.IsUserLoggedIn = false;
                MessageBox.Show(AppState.Instance.UserLogin.ErrorOrNotificationMessage);
            }
        }
    }
}
