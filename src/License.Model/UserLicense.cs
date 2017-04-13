using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class UserLicense
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int LicenseId { get; set; }

        public int TeamId { get; set; }

        public User User { get; set; }
        
        public LicenseData License { get; set; }
    }

    public class UserLicenseDetails
    {
        public User User { get; set; }

        public List<SubscriptionDetails> SubscriptionDetails { get; set; }
    }
}
