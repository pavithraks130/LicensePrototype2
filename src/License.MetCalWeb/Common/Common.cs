using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Common
{
    enum ServiceType
    {
        OnPremiseWebApi,
        CentralizeWebApi
    }

    public enum InviteStatus
    {
        Pending,
        Accepted,
        Declined
    }

    public class AccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }

        public string userName { get; set; }

        public string Id { get; set; }

    }
}