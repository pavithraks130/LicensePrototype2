using License.MetCalDesktop.Common;
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
        
        private string _purchaseOrderId = string.Empty;
        public OfflinePaymentViewModel()
        {
            RedirectToHomeCommand = new RelayCommand(RedirectToHome);
            _purchaseOrderId = AppState.Instance.purchaseOrder.PurchaseOrderNo;
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
        public string PurchaseOrderId { get { return _purchaseOrderId; } set { _purchaseOrderId = value; } }
    }
}

