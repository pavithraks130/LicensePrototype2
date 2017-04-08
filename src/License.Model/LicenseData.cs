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

    public class UserLicesneDataMapping
    {
        public List<string> UserList { get; set; }

        public List<LicenseData> LicenseDataList { get; set; }
    }
    
}
