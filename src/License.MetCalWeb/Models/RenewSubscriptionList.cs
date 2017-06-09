using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class RenewSubscriptionList
    {
        public DateTime RenewalDate { get; set; } = new DateTime(1900, 01, 01);
        public double Price { get; set; }
        public int RenewDuration { get; set; }
        public List<UserSubscription> SubscriptionList { get; set; }
    }
}