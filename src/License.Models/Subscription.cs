using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int ActiveDays { get; set; }
        public double Price { get; set; }
        public string CreatedBy { get; set; }
        public int UserSubscriptionId { get; set; }
        public SubscriptionCategory Category { get; set; }
        public ICollection<Product> Products { get; set; }
        public List<dynamic> ProductIdList { get; set; }

    }

    public class RenewSubscriptionList
    {
        public DateTime RenewalDate { get; set; }
        public double Price { get; set; }
        public int RenewDuration { get; set; }
        public List<UserSubscription> SubscriptionList { get; set; }
    }
}
