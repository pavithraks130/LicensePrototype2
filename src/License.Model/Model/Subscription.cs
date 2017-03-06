using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Model
{
    public class Subscription
    {
        public int Id { get; set; }

        public string SubscriptionName { get; set; }

        public virtual ICollection<Product> Product { get; set; }
        
    }
}
