using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace License.Core.Model
{
    public class ProductLicense
    {
        [Key]
        public int Id { get; set; }
        
        public string LicenseKey { get; set; }

        public int ProductId { get; set; }

        public int UserSubscriptionId { get; set; }

        public bool IsMapped { get; set; }

        [ForeignKey("UserSubscriptionId")]
        public virtual UserSubscription Subscription { get; set; }
    }
}
