using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class SubscriptionLicenseMapping
    {
        public int SubscriptionId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public DateTime RenewalDate { get; set; }
        public Subscription Subscription { get; set; }
        public DateTime ExpireDate { get; set; }
        public int UserSubscriptionId { get; set; }
        public List<LicenseKeyProductMapping> LicenseKeyProductMapping { get; set; }

        public SubscriptionLicenseMapping()
        {
            LicenseKeyProductMapping = new List<LicenseKeyProductMapping>();
        }

        public int OrderdQuantity { get; set; }
    }

}