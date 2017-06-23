using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.MetCalDesktop.Common;
using License.Models;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using License.ServiceInvoke;

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
            //DashboardLogic logic = new DashboardLogic();
            //if (AppState.Instance.IsNetworkAvilable())
            //    logic.LoadFeaturesOnline();
            //else
            //    logic.LoadFeatureOffline();

            if (AppState.Instance.UserLicenseList != null)
            {
                foreach (var data in AppState.Instance.UserLicenseList)
                {
                    foreach (var fet in data.Features)
                    {
                        FeataureList.Add(fet);
                    }
                }
            }
        }

        public void LogOut(object param)
        {
            var fileName = AppState.Instance.User.UserId + ".txt";
            if (AppState.Instance.IsNetworkAvilable())
            {
                UpdateLogoutStatus(AppState.Instance.User.UserId, ServiceType.OnPremiseWebApi);
                if (AppState.Instance.IsSuperAdmin)
                    UpdateLogoutStatus(AppState.Instance.User.ServerUserId, ServiceType.CentralizeWebApi);
            }
           // Code to remove the Team License for the selected Team on User LogOut.
            var jsonData = FileIO.GetJsonDataFromFile(fileName);
            if (!string.IsNullOrEmpty(jsonData))
            {
                var details = JsonConvert.DeserializeObject<UserDetails>(jsonData);
                var licenseList = details.UserLicenses.Where(l => l.TeamId == AppState.Instance.SelectedTeam.Id && l.IsTeamLicense == true).ToList();
                if (licenseList != null && licenseList.Count != 0)
                {
                    details.UserLicenses = details.UserLicenses.Except(licenseList).ToList();
                    jsonData = JsonConvert.SerializeObject(details);
                    FileIO.SaveDatatoFile(jsonData, fileName);
                }
            }
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
