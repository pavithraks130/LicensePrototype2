using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace License.Core.Model
{
    public class LicenseData
    {
        [Key]
        public int Id { get; set; }
        
        public string LicenseKey { get; set; }

        public int ProductId { get; set; }

        public int UserSubscriptionId { get; set; }

        [ForeignKey("UserSubscriptionId")]
        public UserSubscription Subscription { get; set; }

        //[ForeignKey("ProductId")]
        //public Product Product { get; set; }
    }
}
