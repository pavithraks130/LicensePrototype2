using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace License.MetCalWeb.Models
{
    /// <summary>
    /// This entire region is used to sync the subscription details between 2 servers.
    /// </summary>
    ///   
    public class SubscriptionList
    {
        public string UserId { get; set; }

        public List<SubscriptionLicenseMapping> Subscriptions { get; set; }

        public SubscriptionList()
        {
            Subscriptions = new List<SubscriptionLicenseMapping>();
        }
    }
}
