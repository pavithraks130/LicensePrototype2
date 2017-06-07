using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int SubscriptionId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public DateTime RenewalDate { get; set; }

        public Subscription Subtype { get; set; }
        public User User { get; set; }

        public int Quantity { get; set; }
        
    }
}