using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;
//using License.MetCalDesktop.Common;
//using CalLicenseDemo.Logic;
using License.MetCalDesktop.Model;

namespace License.MetCalDesktop.ViewModel
{
    /// <summary>
    /// This class is used to perform new user license subscription operation.
    /// </summary>
    class SubscriptionViewModel : BaseEntity
    {
        /// <summary>
        ///performing the license purchase  action
        /// </summary>
        public ICommand BuyCommand { get; set; }
        /// <summary>
        /// SubscriptionList collection
        /// </summary>
        public List<LicenseType> SubscriptionList { get; set; }

        /// <summary>
        /// navigation service action
        /// </summary>
        public NavigationService Service { get; set; }

        /// <summary>
        /// SubscriptionViewModel class constructor initialisation.
        /// </summary>
        public SubscriptionViewModel()
        {
            BuyCommand = new RelayCommand(RedirectToPayment);
            LoadSubscriptionList();
        }

        /// <summary>
        /// To load subscription collection.
        /// </summary>
        private void LoadSubscriptionList()
        {
            //TO-Do
          //  LicenseLogic logic = new LicenseLogic();
           // SubscriptionList = logic.GetSubscriptionDetails();
        }

        /// <summary>
        /// performing the license purchase  action
        /// </summary>
        /// <param name="param">param</param>
        private void RedirectToPayment(object param)
        {
            //int id = Convert.ToInt32(param);
            //LicenseType typeObj = SubscriptionList.FirstOrDefault(l => l.TypeId == id);
            ////LicenseLogic logic = new LicenseLogic();
            //SingletonLicense.Instance.SelectedSubscription = typeObj;
            ////logic.ActivateSubscription();
            //if (NavigateNextPage != null)
            //    NavigateNextPage(null, null);
        }
    }
}
