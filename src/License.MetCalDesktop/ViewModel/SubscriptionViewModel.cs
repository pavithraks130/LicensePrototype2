using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;
using License.MetCalDesktop.Model;
using System.Net.Http;
using License.MetCalDesktop.Common;
using Newtonsoft.Json;
namespace License.MetCalDesktop.ViewModel
{
    /// <summary>
    /// This class is used to perform new user license subscription operation.
    /// </summary>
    class SubscriptionViewModel : BaseEntity
    {

        private List<Subscription> _subscriptionList = new List<Subscription>();
        /// <summary>
        ///performing the license purchase  action
        /// </summary>
        public ICommand BuyCommand { get; set; }
        public ICommand RedirectToSubscriptionDetailsCommand { get; set; }
        /// <summary>
        /// SubscriptionList collection
        /// </summary>
        public List<Subscription> SubscriptionList
        {
            get
            {
                return _subscriptionList;
            }
            set
            {
                _subscriptionList = value;
            }
        }

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
            RedirectToSubscriptionDetailsCommand = new RelayCommand(RedirectToMain);
            if (AppState.Instance.IsNetworkAvilable() && AppState.Instance.IsSuperAdmin)
                LoadSubscriptionList();
        }

        public void RedirectToMain(object param)
        {
            if (NavigateNextPage != null)
                NavigateNextPage("Home", null);
        }

        /// <summary>
        /// To load subscription collection.
        /// </summary>
        private void LoadSubscriptionList()
        {
            HttpClient client = AppState.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.CentralizedToken.access_token);
            HttpResponseMessage response = null;
            response = client.GetAsync("api/subscription/All/" + AppState.Instance.User.ServerUserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var subscriptionList = JsonConvert.DeserializeObject<List<Subscription>>(data);
                foreach (var x in subscriptionList)
                {
                    x.ImagePath = @"..\Catalog\Products_Images\" + x.ImagePath;
                    _subscriptionList.Add(x);
                }
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var failureResult = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            }
            client.Dispose();
        }

        /// <summary>
        /// performing the license purchase  action
        /// </summary>
        /// <param name="param">param</param>
        private void RedirectToPayment(object param)
        {
            int id = Convert.ToInt32(param);
            var typeObj = SubscriptionList.FirstOrDefault(l => l.Id == id);
            AppState.Instance.SelectedSubscription = typeObj;
            if (NavigateNextPage != null)
                NavigateNextPage("CreditAndDebitCardDetails", null);
        }
    }
}
