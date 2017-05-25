using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalDesktop.Model
{
    public class UserLicenseDetails
    {
        public User User { get; set; }
        public string UserId { get; set; }

        public List<SubscriptionDetails> SubscriptionDetails { get; set; }
    }
}