using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.MetCalDesktop.Common;
using License.DataModel;
using System.Windows.Input;

namespace License.MetCalDesktop.ViewModel
{
    public class DashboardViewModel : BaseEntity
    {
        public List<Feature> FeataureList { get; set; }

        public string LoggedInUser { get; set; }

        public ICommand LogoutCommand { get; set; }
        public DashboardViewModel()
        {
            FeataureList = new List<Feature>();
            LogoutCommand = new RelayCommand(LogOut);
            LoadFeatures();
            LoggedInUser = AppState.Instance.User.FirstName + ", " + AppState.Instance.User.LastName;
        }
        public void LoadFeatures()
        {
            if (AppState.Instance.UserLicenseList != null)
            {
                foreach (var data in AppState.Instance.UserLicenseList)
                {
                    foreach (var pro in data.ProductList)
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
