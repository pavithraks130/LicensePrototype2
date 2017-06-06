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

        private List<SubscriptionType> _subscriptionList = new List<SubscriptionType>();
        /// <summary>
        ///performing the license purchase  action
        /// </summary>
        public ICommand BuyCommand { get; set; }
        public ICommand RedirectToSubscriptionDetailsCommand { get; set; }
        /// <summary>
        /// SubscriptionList collection
        /// </summary>
        public List<SubscriptionType> SubscriptionList
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
            RedirectToSubscriptionDetailsCommand = new RelayCommand(RedirectToSubscriptionDetails);
            LoadSubscriptionList();
        }

        private void RedirectToSubscriptionDetails(object obj)
        {
            if (NavigateNextPage != null)
                NavigateNextPage("SubscriptionDetails", null);
        }

        /// <summary>
        /// To load subscription collection.
        /// </summary>
        private void LoadSubscriptionList()
        {

            HttpClient client = AppState.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.CentralizedToken.access_token);
            HttpResponseMessage response;
            if (AppState.Instance.IsSuperAdmin)
            {
                response = client.GetAsync("api/subscription/All").Result;
            }
            else
            {
                response = client.GetAsync("api/subscription/All/" + AppState.Instance.User.ServerUserId).Result;
            }
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var subscriptionList = JsonConvert.DeserializeObject<List<SubscriptionType>>(data);
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
