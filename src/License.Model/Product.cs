using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    
    public class ProductSubscriptionMapping
    {
        public int ProductId { get; set; }
        public int SubscriptionId { get; set; }
        public Product Product { get; set; }
        public Subscription Subscription { get; set; }
    }
}
