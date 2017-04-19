using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using License.MetCalDesktop.Model;

namespace License.MetCalDesktop.Common
{

    public class AppState
    {
        private static AppState _instance = null;
        public static AppState Instance
        {
            get { return _instance ?? (_instance = new AppState()); }
        }

        public List<SubscriptionDetails> UserLicenseList { get; set; }

        public bool IsUserLoggedIn { get; set; }
        public bool IsSuperAdmin { get; set; }

        public User User { get; set; }

        public AccessToken OnPremiseToken { get; set; }

        public AccessToken CentralizedToken { get; set; }

        public List<Team> TeamList { get; set; }

        public Team SelectedTeam { get; set; }
        public SubscriptionType SelectedSubscription { get; set; }

        public static HttpClient CreateClient(string serviceType)
        {
            string url = string.Empty;
            switch (serviceType)
            {
                case "OnPremiseWebApi":
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("OnPremiseWebApi");
                    break;
                case "CentralizeWebApi":
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("CentralizeWebApi");
                    break;
            }
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
