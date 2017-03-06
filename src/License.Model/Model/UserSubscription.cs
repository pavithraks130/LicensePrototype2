using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Model
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int SubscriptionId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public User User { get; set; }

        public Subscription Subscription { get; set; }

        public List<LicenseData> LicenseList { get; set; }

        public int Quantity { get; set; }
    }
}
