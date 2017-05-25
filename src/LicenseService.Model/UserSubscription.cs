using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int SubscriptionTypeId { get; set; }

        public int Quantity { get; set; }

        public DateTime ExpireDate { get; set; } = new DateTime(1900, 01, 01);

        public DateTime ActivationDate { get; set; } = new DateTime(1900, 01, 01);

        public DateTime RenewalDate { get;set; } = new DateTime(1900, 01, 01);

        public SubscriptionType Subtype { get; set; }
       
    }

    public class LicenseKeyProductMapping
    {
        public string LicenseKey { get; set; }
        public int ProductId { get; set; }

    }
}
