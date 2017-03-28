using License.MetCalDesktop.ViewModel;
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

namespace License.MetCalDesktop.Views
{
    /// <summary>
    /// Interaction logic for LoginUser.xaml
    /// </summary>
    public partial class LoginUser : Page
    {
        public LoginUser()
        {
            InitializeComponent();
            var viewModel = new LoginViewModel();
            viewModel.NavigateNextPage += NavigateNextPage;
            DataContext = viewModel;
        }

        private void NavigateNextPage(string screenName, Dictionary<string, string> additionalInfo)
        {
            if (screenName == "Dashboard")
                this.NavigationService.Navigate(new Dashboard());
        }

        private void ButtonNewUser_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
