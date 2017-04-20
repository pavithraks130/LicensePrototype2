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
    /// Interaction logic for Subscriptions.xaml
    /// </summary>
    public partial class Subscriptions : Page
    {
        public Subscriptions()
        {
            InitializeComponent();
            //Navigation Service is an inbuilt prperty of page
            var viewmodel = new SubscriptionViewModel();
            viewmodel.NavigateNextPage += NavigateNextPage;
            DataContext = viewmodel;
        }

        private void NavigateNextPage(string screenName, Dictionary<string, string> additionalInfo)
        {
            if (screenName == "CreditAndDebitCardDetails")
            {
                this.NavigationService.Navigate(new CreditAndDebitCardDetails());
            }
            if (screenName == "SubscriptionDetails")
            {
                this.NavigationService.Navigate(new SubscriptionDetails());
            }
        }
    }
}
