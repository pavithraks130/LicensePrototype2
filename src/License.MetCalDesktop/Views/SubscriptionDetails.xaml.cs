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
    /// Interaction logic for SubscriptionDetails.xaml
    /// </summary>
    public partial class SubscriptionDetails : Page
    {
        public SubscriptionDetails()
        {
            InitializeComponent();
            var viewModel = new SubscriptionDetailsViewModel();
            viewModel.NavigateNextPage += delegate (string screenName, Dictionary<string, string> additionalInfo) { this.NavigationService.Navigate(new Dashboard()); };
            DataContext = viewModel;
        }
    }
}
