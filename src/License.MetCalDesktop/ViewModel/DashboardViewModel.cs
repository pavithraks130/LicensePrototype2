using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.MetCalDesktop.Common;
using License.MetCalDesktop.Model;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using License.MetCalDesktop.businessLogic;

namespace License.MetCalDesktop.ViewModel
{
    public class DashboardViewModel : BaseEntity
    {
        private bool isSuperAdmin = false;
        public bool IsSuperAdmin
        {
            get { return isSuperAdmin; }
            set { isSuperAdmin = value; }
        }
        public List<Feature> FeataureList { get; set; }

        public string LoggedInUser { get; set; }


        public ICommand LogoutCommand { get; set; }
        public DashboardViewModel()
        {
            FeataureList = new List<Feature>();
            LogoutCommand = new RelayCommand(LogOut);
            LoggedInUser = AppState.Instance.User.FirstName + ", " + AppState.Instance.User.LastName;
            isSuperAdmin = AppState.Instance.IsSuperAdmin;

            LoadFeatures();
        }
       
        public void LoadFeatures()
        {
            DashboardLogic logic = new DashboardLogic();
            if (AppState.Instance.IsNetworkAvilable())
                logic.LoadFeaturesOnline();
            else
                logic.LoadFeatureOffline();

            if (AppState.Instance.UserLicenseList != null)
            {
                foreach (var data in AppState.Instance.UserLicenseList)
                {
                    foreach (var pro in data.Products)
                    {
                        foreach (var fet in pro.Features)
                        {
                            FeataureList.Add(fet);
                        }
                    }
                }
            }
        }

        public void LogOut(object param)
        {
            UpdateLogoutStatus(AppState.Instance.User.UserId, ServiceType.OnPremiseWebApi);
            if (AppState.Instance.IsSuperAdmin)
                UpdateLogoutStatus(AppState.Instance.User.ServerUserId, ServiceType.CentralizeWebApi);
            AppState.Instance.User = null;
            AppState.Instance.UserLicenseList = null;
            AppState.Instance.IsUserLoggedIn = false;
            NavigateNextPage?.Invoke("login", null);
        }

        public void UpdateLogoutStatus(string userId, ServiceType type)
        {
            HttpClient client = AppState.CreateClient(type.ToString());
            switch (type)
            {
                case ServiceType.CentralizeWebApi:
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.CentralizedToken.access_token);
                    break;
                case ServiceType.OnPremiseWebApi:
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
                    break;
            }
            User userModel = new User()
            {
                UserId = userId,
                IsActive = false
            };
            client.PutAsJsonAsync("api/user/UpdateActiveStatus", userModel);
        }

    }
}
