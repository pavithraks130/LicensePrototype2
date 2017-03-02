using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LicenseServer.Core.Model
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        public int SubscriptionTypeId { get; set; }

        [ForeignKey("SubscriptionTypeId")]
        public virtual SubscriptionType SubType { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Appuser User { get; set; }

        public bool IsPurchased { get; set; }
    }
}
