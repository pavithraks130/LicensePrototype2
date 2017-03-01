using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Model.Model
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ServerUserId { get; set; }

        public string SubscriptionId { get; set; }

        public string SubscriptionName { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public User User { get; set; }

        public LicenseDetailModel LicenseDetails { get; set; }
    }
}
