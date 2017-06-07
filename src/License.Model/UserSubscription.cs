using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace License.DataModel
{
    public class SubscriptionList
    {
        public string UserId { get; set; }

        public List<SubscriptionLicenseMapping> Subscriptions { get; set; }

        public SubscriptionList()
        {
            Subscriptions = new List<SubscriptionLicenseMapping>();
        }
    }

    public class SubscriptionLicenseMapping
    {
        public int SubscriptionId { get; set; }

        public DateTime SubscriptionDate { get; set; }
        public Subscription Subscription { get; set; }
        public DateTime RenewalDate { get; set; }
        public List<LicenseKeyProductMapping> LicenseKeyProductMapping { get; set; }

        public SubscriptionLicenseMapping()
        {
            LicenseKeyProductMapping = new List<LicenseKeyProductMapping>();
        }

        public int OrderdQuantity { get; set; }
    }

    public class Subscription
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int UserSubscriptionId { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public List<dynamic> ProductIdList { get; set; }

    }

   

    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
    }

    public class LicenseKeyProductMapping
    {
        public string LicenseKey { get; set; }
        public int ProductId { get; set; }

    }

    public class UserSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int SubscriptionId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public DateTime RenewalDate { get; set; }

        public int Quantity { get; set; }

        public List<LicenseKeyProductMapping> LicenseKeys { get; set; }

        public Subscription Subscription { get; set; }

        public User User { get; set; }
    }

}
