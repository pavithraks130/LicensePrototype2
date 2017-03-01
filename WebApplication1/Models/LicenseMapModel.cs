using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class LicenseMapModel
    {
        public string SubscriptionName { get; set; }

        public int UserSubscriptionId { get; set; }

        public bool InitialSelected { get; set; }

        public int ExistingUserLicenseId { get; set; }

        public bool IsSelected { get; set; }

        public bool IsDisabled { get; set; }
    }
}