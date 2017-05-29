using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class LicenseData
    {
        public int Id { get; set; }

        public string AdminUserId { get; set; }

        public string LicenseKey { get; set; }

        public int UserSubscriptionId { get; set; }

        public int ProductId { get; set; }
        
        public UserSubscription Subscription { get; set; }

        public Product Product { get; set; }
    }

    public class UserLicenseDataMapping
    {
        public int TeamId { get; set; }
        public List<string> UserList { get; set; }

        public List<LicenseData> LicenseDataList { get; set; }
    }

    public class TeamLicenseDataMapping
    {
        public int ConcurrentUserCount { get; set; }

        public List<string> TeamList { get; set; }

        public List<int> ProductIdList { get; set; }
    }

    public class FetchUserSubscription
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public bool IsFeatureRequired { get; set; }
    }
}
