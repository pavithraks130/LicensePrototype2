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
        public DateTime ExpireDate { get; set; }
        public int UserSubscriptionId { get; set; }
        public int OrderdQuantity { get; set; }
        public SubscriptionLicenseMapping()
        {
            LicenseKeyProductMapping = new List<LicenseKeyProductMapping>();
        }

    }

    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public int UserSubscriptionId { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public List<dynamic> ProductIdList { get; set; }

    }
    
    public class LicenseKeyProductMapping
    {
        public string LicenseKey { get; set; }
        public int ProductId { get; set; }

    }    

}
