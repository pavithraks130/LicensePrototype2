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
    /// Interaction logic for OfflinePayment.xaml
    /// </summary>
    public partial class OfflinePayment : Page
    {
        public OfflinePayment()
        {
            InitializeComponent();
            var viewmodel = new OfflinePaymentViewModel();
            viewmodel.NavigateNextPage += NavigateNextPage;
            DataContext = viewmodel;
        }

        private void NavigateNextPage(string screenName, Dictionary<string, string> additionalInfo)
        {
            if (screenName == "Dashboard")
            {
                this.NavigationService.Navigate(new Dashboard());
            }
        }
    }
}
