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
            LoadTeams();
        }
        public void LoadTeams()
        {
            HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/Team/GetTeamsByUserId/" + AppState.Instance.User.UserId).Result;
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
                    LoadFeatures();
                }
            }
        }

        public void CloseWindow(object source, EventArgs e)
        {
            (source as System.Windows.Window).Close();
            LoadFeatures();
        }
        public void LoadFeatures()
        {
            HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            FetchUserSubscription userSub = new FetchUserSubscription();
            userSub.TeamId = AppState.Instance.SelectedTeam.Id;
            userSub.IsFeatureRequired = true;
            userSub.UserId = AppState.Instance.User.UserId;
            var response = client.PostAsJsonAsync("api/License/GetSubscriptionLicenseByTeam", userSub).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var details = JsonConvert.DeserializeObject<UserLicenseDetails>(jsonData);
                AppState.Instance.UserLicenseList = details.SubscriptionDetails;
            }
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
            AppState.Instance.User = null;
            AppState.Instance.UserLicenseList = null;
            AppState.Instance.IsUserLoggedIn = false;
            if (NavigateNextPage != null)
                NavigateNextPage("login", null);
        }

    }
}
