using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    /// <summary>
    /// Model for access Token
    /// </summary>
    public class AccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }

        public string userName { get; set; }

        public string Id { get; set; }

    }
}