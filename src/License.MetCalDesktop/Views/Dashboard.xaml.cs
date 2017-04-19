using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using License.MetCalDesktop.ViewModel;

namespace License.MetCalDesktop.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            var viewModel = new DashboardViewModel();
            viewModel.NavigateNextPage += NavigateNextPage;
            this.DataContext = viewModel;
            //wbSample.Navigate("http://localhost:62061/Subscription");
        }

        public void NavigateNextPage(string screenName, Dictionary<string, string> additionalInfo)
        {
            switch (screenName.ToLower())
            {
                case "login":
                    this.NavigationService.Navigate(new LoginUser());
                    break;
            }
        }
    }
}
