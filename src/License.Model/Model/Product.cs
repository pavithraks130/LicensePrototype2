using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public int QtyPerSubscription { get; set; }

    }

    public class ProductSubscriptionMapping
    {
        public int ProductId { get; set; }
        public int SubscriptionId { get; set; }
        public Product Product { get; set; }
        public Subscription Subscription { get; set; }
    }
}
