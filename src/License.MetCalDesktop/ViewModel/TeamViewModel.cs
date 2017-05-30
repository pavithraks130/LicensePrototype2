using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.MetCalDesktop.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using License.MetCalDesktop.Common;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Net.Http;

namespace License.MetCalDesktop.ViewModel
{
    public class TeamViewModel : INotifyPropertyChanged
    {
        public delegate void ClosePopupWindowEvent(object sender,CustomEventArgs e);

        public EventHandler ClosepoupWindow;

        public ICommand UpdateCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        private Team _selectedTeam;
        public Team SelectedTeam
        {
            get { return _selectedTeam; }
            set { _selectedTeam = value; OnPropertyChange("SelectedTeam"); }
        }
        private ObservableCollection<Team> _teamList;
        public ObservableCollection<Team> TeamList
        {
            get { return _teamList; }
            set
            {
                _teamList = value; OnPropertyChange("TeamList");
            }
        }

        public TeamViewModel()
        {
            TeamList = new ObservableCollection<Team>(AppState.Instance.TeamList);
            UpdateCommand = new RelayCommand(UpdateSelectedTeam);
        }

        public void UpdateSelectedTeam(object args)
        {
            AppState.Instance.SelectedTeam = (Team)args;
            ConcurrentUserLogin userLogin = new ConcurrentUserLogin();
            userLogin.TeamId = AppState.Instance.SelectedTeam.Id;
            userLogin.UserId = AppState.Instance.User.UserId;
            HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            var response = client.PostAsJsonAsync("api/User/IsConcurrentUserLoggedIn", userLogin).Result;
            var jsonData = response.Content.ReadAsStringAsync().Result;
            var userLoginObj = JsonConvert.DeserializeObject<ConcurrentUserLogin>(jsonData);
            AppState.Instance.UserLogin = userLoginObj;
            //CustomEventArgs e = new CustomEventArgs();
            //e.IsConcurrentuserLoggedIn = userLoginObj.IsUserLoggedIn;
            //if (userLoginObj.IsUserLoggedIn)
            //    e.ErrorMessage = "";
            //else
            //    e.ErrorMessage = userLoginObj.ErrorOrNotificationMessage;
            ClosepoupWindow?.Invoke(this,new EventArgs());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChange(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
