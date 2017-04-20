using License.MetCalDesktop.Common;
using License.MetCalDesktop.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace License.MetCalDesktop.ViewModel
{
    public class SubscriptionDetailsViewModel : BaseEntity
    {
        private List<SubscriptionDetails> _subscriptionDetailsList = new List<SubscriptionDetails>();
        /// <summary>
        ///performing the license purchase  action
        /// </summary>
        public ICommand RedirectToHomeCommand { get; set; }
        /// <summary>
        /// SubscriptionList collection
        /// </summary>
        public List<SubscriptionDetails> SubscriptionDetailsList
        {
            get
            {
                return _subscriptionDetailsList;
            }
            set
            {
                _subscriptionDetailsList = value;
            }
        }

        /// <summary>
        /// navigation service action
        /// </summary>
        public NavigationService Service { get; set; }
        public SubscriptionDetailsViewModel()
        {
            RedirectToHomeCommand = new RelayCommand(RedirectToHome);
            LoadSubscriptionDetails();
        }

        private void LoadSubscriptionDetails()
        {

            string adminUserId = string.Empty;
            if (AppState.Instance.IsSuperAdmin)
                adminUserId = AppState.Instance.User.UserId;
            else
            {
                adminUserId = AppState.Instance.SelectedTeam.AdminId;
            }
            HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            var response = client.GetAsync("api/UserSubscription/SubscriptionDetils/" + adminUserId).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsonData))
                    _subscriptionDetailsList = JsonConvert.DeserializeObject<List<SubscriptionDetails>>(jsonData);
            }
            client.Dispose();

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
    }
}



