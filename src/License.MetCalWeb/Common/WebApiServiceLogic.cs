using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;


namespace License.MetCalWeb.Common
{
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
            if (LicenseSessionState.Instance.CentralizedToken != null || LicenseSessionState.Instance.OnPremiseToken != null)
                switch (serviceType)
                {
                    case ServiceType.OnPremiseWebApi:
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.OnPremiseToken.access_token);
                        break;
                    case ServiceType.CentralizeWebApi:
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LicenseSessionState.Instance.CentralizedToken.access_token);
                        break;
                }
            return client;
        }
    }
}