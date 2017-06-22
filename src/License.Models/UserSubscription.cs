using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int SubscriptionId { get; set; }

        public int Quantity { get; set; }

        public DateTime ExpireDate { get; set; } = new DateTime(1900, 01, 01);

        public DateTime ActivationDate { get; set; } = new DateTime(1900, 01, 01);

        public DateTime RenewalDate { get; set; } = new DateTime(1900, 01, 01);

        public Subscription Subtype { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public int ServerUserSubscriptionId { get; set; }

    }
}
