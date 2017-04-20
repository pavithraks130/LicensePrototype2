using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace License.MetCalDesktop.ViewModel
{
    public class OfflinePaymentViewModel : BaseEntity
    {
        public ICommand RedirectToHomeCommand { get; set; }

        public OfflinePaymentViewModel()
        {
            RedirectToHomeCommand = new RelayCommand(RedirectToHome);
        }

        /// <summary>
        /// performing the license purchase  action
        /// </summary>
        /// <param name="param">param</param>
        private void RedirectToHome(object param)
        {
            if (NavigateNextPage != null)
                NavigateNextPage("Dashboard", null);
        }
        /// <summary>
        /// navigation service action
        /// </summary>
        public NavigationService Service { get; set; }

    }
}

