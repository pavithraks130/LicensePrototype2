using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    public class TeamLicenseDetails
    {
        public Team Team { get; set; }
        public List<Subscription> SubscriptionDetails { get; set; }
    }
}
