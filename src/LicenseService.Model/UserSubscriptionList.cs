using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    /// <summary>
    /// class is used for synching the subscription data from LicenseServer to OnPremise Server
    /// </summary>   

    public class SubscriptionList
    {
        public string UserId { get; set; }

        public List<Subscription> Subscriptions { get; set; }

        public SubscriptionList()
        {
            Subscriptions = new List<Subscription>();
        }
    }

    public class Subscription
    {
        public int SubscriptionTypeId { get; set; }

        public DateTime SubscriptionDate { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public List<LicenseKeyProductMapping> LicenseKeyProductMapping { get; set; }

        public Subscription()
        {
            LicenseKeyProductMapping = new List<DataModel.LicenseKeyProductMapping>();
        }

        public int OrderdQuantity { get; set; }
    }
}
