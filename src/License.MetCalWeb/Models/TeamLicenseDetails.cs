using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class TeamLicenseDetails
    {
        public Team Team { get; set; }

        public List<SubscriptionDetails> SubscriptionDetails { get; set; }
    }

    public class TeamLicenseDataMapping
    {
        public int ConcurrentUserCount { get; set; }
        public List<string> TeamList { get; set; }

        public List<LicenseData> LicenseDataList { get; set; }
    }

}