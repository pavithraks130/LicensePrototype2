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

        public string AdminUserId { get; set; }

        public string LicenseKey { get; set; }

        public int SubscriptionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public UserSubscription Subscription { get; set; }
    }
}
