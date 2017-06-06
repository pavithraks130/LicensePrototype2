using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LicenseServer.Core.Model
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int ActiveDays { get; set; }

        public double Price { get; set; }

        public string CreatedBy { get; set; }
        
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public SubscriptionCategory Category { get; set; }

        [ForeignKey("CreatedBy")]
        public Appuser CreatedUser { get; set; }
        public virtual IEnumerable<SubscriptionDetail> SubDetails { get; set; }
    }
}
