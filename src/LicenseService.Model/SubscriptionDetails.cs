using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    public class SubscriptionDetails
    {
        public int Id { get; set; }

        public int SubscriptionId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public Subscription Subscription { get; set; }

        public Product Product { get; set; }
    }
}
