using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LicenseServer.Core.Model
{
    public class UserSubscription
    {
        [Key]
        public int id { get; set; }

        public string UserId { get; set; }

        public int SubscriptionTypeId { get; set; }

        public int Quantity { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public int ActiveDurataion { get; set; }

        public DateTime ActivationDate { get; set; }

        [ForeignKey("SubscriptionTypeId")]
        public virtual SubscriptionType Subtype { get; set; }

        [ForeignKey("UserId")]
        public Appuser User { get; set; }
    }
}
