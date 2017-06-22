using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    /// <summary>
    /// Class used in the client application. The Token is returned once User Authenticated.
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
