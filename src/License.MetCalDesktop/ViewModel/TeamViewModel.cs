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

namespace License.MetCalDesktop.ViewModel
{
    public class TeamViewModel : INotifyPropertyChanged
    {
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
            ClosepoupWindow?.Invoke(this, new EventArgs());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChange(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
