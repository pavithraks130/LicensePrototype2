using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using License.Models;
using License.ServiceInvoke;
using System.Net;

namespace License.MetCalDesktop.Common
{

    public class AppState
    {
        private static AppState _instance = null;
        public static AppState Instance
        {
            get { return _instance ?? (_instance = new AppState()); }
        }

        public List<Product> UserLicenseList { get; set; }

        public bool IsUserLoggedIn { get; set; }
        public bool IsSuperAdmin { get; set; }
        public PurchaseOrder purchaseOrder { get; set; }
       
        public bool IsNetworkAvilable()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public User User { get; set; }

        public AccessToken OnPremiseToken { get; set; }

        public AccessToken CentralizedToken { get; set; }

        public List<Team> TeamList { get; set; }

        public Team SelectedTeam { get; set; }
        public Subscription SelectedSubscription { get; set; }

        public ConcurrentUserLogin UserLogin { get; set; }
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

        public bool IsCredentialFileExist()
        {
            FileIO _fileIO = new FileIO();
            return _fileIO.IsFileExist("credential.txt");
        }

    }
}
