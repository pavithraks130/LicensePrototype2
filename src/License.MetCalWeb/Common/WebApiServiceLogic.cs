using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;


namespace License.MetCalWeb.Common
{
    public class WebApiServiceLogic
    {

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