using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LicenseServer.Core.Model
{
    public class SubscriptionDetail
    {
        [Key]
        public int Id { get; set; }

        public int SubscriptionTypeId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("SubscriptionTypeId")]
        public SubscriptionType SubscriptyType { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
