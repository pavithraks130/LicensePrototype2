
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

namespace License.MetCalDesktop.ViewModel
{
    class LoginViewModel : BaseEntity
    {
        #region private field

        private string _email;

        private string _password;

        private bool _isEnableLogin = true;

        #endregion private field

        /// <summary>
        ///Initialising LoginUserViewModel 
        /// </summary>
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginUser);
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
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {
                LoginLogic logic = new LoginLogic();
                IsEnableLogin = false;
                Model.User user = null;
                if (AppState.Instance.IsNetworkAvilable())
                    user = logic.AuthenticateOnline(Email, Password);
                else if (AppState.Instance.IsCredentialFileExist())
                    user = logic.AuthenticateUser(Email, Password);
                else
                {
                    MessageBox.Show("Need to be online for first time login");
                    IsEnableLogin = true;
                    return;
                }
                if (user == null)
                {
                    MessageBox.Show(logic.ErrorMessage);
                    IsEnableLogin = true;
                    return;
                }
                AppState.Instance.User = user;
                AppState.Instance.IsUserLoggedIn = true;
                if (AppState.Instance.IsNetworkAvilable())
                    LoadTeams();
                IsEnableLogin = true;
            }
        }

        public void LoadTeams()
        {
            HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            string url = "api/Team/GetTeamsByUserId/";
            if (AppState.Instance.IsSuperAdmin)
                url = "api/Team/GetTeamsByAdminId/";
            var response = client.GetAsync(url + AppState.Instance.User.UserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var teamList = JsonConvert.DeserializeObject<List<Team>>(jsonData);
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
                    client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
                    response = client.PostAsJsonAsync("api/User/IsConcurrentUserLoggedIn", userLogin).Result;
                    jsonData = response.Content.ReadAsStringAsync().Result;
                    var userLoginObj = JsonConvert.DeserializeObject<ConcurrentUserLogin>(jsonData);
                    if (userLoginObj.IsUserLoggedIn)
                        NavigateNextPage?.Invoke("Dashboard", null);
                    else
                        MessageBox.Show(userLoginObj.ErrorOrNotificationMessage);
                }

            }
        }

        private void CloseWindow(object sender, EventArgs e1)
        {
            var e = (CustomEventArgs)e1;
            (sender as System.Windows.Window).Close();
            if (e.IsConcurrentuserLoggedIn)
                NavigateNextPage?.Invoke("Dashboard", null);
            else
            {
                AppState.Instance.User = null;
                AppState.Instance.UserLicenseList = null;
                AppState.Instance.IsUserLoggedIn = false;
                MessageBox.Show(e.ErrorMessage);
            }
        }
    }
}
