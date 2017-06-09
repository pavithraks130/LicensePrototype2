using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class ProductLicense
    {
        public int Id { get; set; }

        public string AdminUserId { get; set; }

        public string LicenseKey { get; set; }

        public int UserSubscriptionId { get; set; }

        public int ProductId { get; set; }
    }
}