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
            if (AppState.Instance.IsNetworkAvilable)
                LoadTeams();
            else
                LoadFeatures();
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
            DashboardLogic logic = new DashboardLogic();
            if (AppState.Instance.IsNetworkAvilable)
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
            AppState.Instance.User = null;
            AppState.Instance.UserLicenseList = null;
            AppState.Instance.IsUserLoggedIn = false;
            if (NavigateNextPage != null)
                NavigateNextPage("login", null);
        }

    }
}
