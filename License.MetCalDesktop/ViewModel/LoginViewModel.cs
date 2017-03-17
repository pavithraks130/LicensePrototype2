using License.Logic.Common;
using License.Logic.ServiceLogic;
using License.MetCalDesktop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace License.MetCalDesktop.ViewModel
{
    class LoginViewModel:BaseEntity
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
        public bool IsNetworkAvilable
        {
            get { return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable(); }
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
                var logic = new UserLogic();
                IsEnableLogin = false;
                var status = IsNetworkAvilable;
                if (status)
                {
                    var result = logic.AutheticateUser(Email, Password);
                }
                else
                {
                    MessageBox.Show("Network Not available");
                    return;
                }
                if (status)
                {
                    AppState.IsUserLoggedIn = true;
                    NavigateNextPage?.Invoke(null, null);
                    IsEnableLogin = true;
                }
                else
                {
                    MessageBox.Show("Unable to login, Invalid Credentials");
                    IsEnableLogin = true;
                }
            }
        }
    }
}
