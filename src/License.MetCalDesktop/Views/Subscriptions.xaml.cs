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
    /// Interaction logic for Subscriptions.xaml
    /// </summary>
    public partial class Subscriptions : UserControl
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
            NavigationWindow windowTemp = (NavigationWindow)Window.GetWindow(this);
            if (screenName == "CreditAndDebitCardDetails")
                windowTemp.NavigationService.Navigate(new CreditAndDebitCardDetails());            
        }

    }
}
