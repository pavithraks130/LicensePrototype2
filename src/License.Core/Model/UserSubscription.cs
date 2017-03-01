using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace License.Core.Model
{
    public class UserSubscription
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ServerUserId { get; set; }

        public string SubscriptionId { get; set; }

        public string SubscriptionName { get; set; }

        public DateTime SubscriptionDate { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }
    }
}
