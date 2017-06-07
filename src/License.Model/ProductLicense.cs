using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class ProductLicense
    {
        public int Id { get; set; }

        public string AdminUserId { get; set; }

        public string LicenseKey { get; set; }

        public int UserSubscriptionId { get; set; }

        public bool IsMapped { get; set; }

        public int ProductId { get; set; }
        
        public UserSubscription Subscription { get; set; }

        public Product Product { get; set; }
    }

    public class UserLicenseDataMapping
    {
        public int TeamId { get; set; }

        public List<User> UserList { get; set; }

        public List<ProductLicense> LicenseDataList { get; set; }
    }

    public class TeamLicenseDataMapping
    {
        public List<Team> TeamList { get; set; }

        public List<ProductLicense> LicenseDataList { get; set; }
    }

    public class FetchUserSubscription
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public bool IsFeatureRequired { get; set; }
    }
}
