using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;


namespace License.MetCalWeb.Common
{
    /// <summary>
    /// Common class for the Creation of the client object for the service call and update the required attributeds for the Client object
    /// </summary>
    public class WebApiServiceLogic
    {
        public static HttpClient CreateClient(ServiceType serviceType)
        {
            string url = string.Empty;
            switch (serviceType)
            {
                case ServiceType.OnPremiseWebApi:
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("OnPremiseWebApi");
                    break;
                case ServiceType.CentralizeWebApi:
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("CentralizeWebApi");
                    break;
            }
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            switch (serviceType)
            {
                case ServiceType.OnPremiseWebApi:
                    if (LicenseSessionState.Instance.OnPremiseToken != null)
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                    break;
                case ServiceType.CentralizeWebApi:
                    if (LicenseSessionState.Instance.CentralizedToken != null)
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
                    break;
            }
            return client;
        }

        public static HttpClient CreateClientWithoutToken(ServiceType serviceType)
        {
            string url = string.Empty;
            switch (serviceType)
            {
                case ServiceType.OnPremiseWebApi:
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("OnPremiseWebApi");
                    break;
                case ServiceType.CentralizeWebApi:
                    url = System.Configuration.ConfigurationManager.AppSettings.Get("CentralizeWebApi");
                    break;
            }
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

    }
}