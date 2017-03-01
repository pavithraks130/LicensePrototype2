using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    public class UserSubscription
    {
        public int id { get; set; }

        public string UserId { get; set; }

        public int SubscriptionTypeId { get; set; }

        public int Qty { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public int ActiveDurataion { get; set; }
        public List<string> LicenseKeys { get; set; }
    }
}
