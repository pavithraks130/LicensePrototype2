using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace License.Core.Model
{
    public class UserLicenseRequest
    {
        public int Id { get; set; }
        public string Requested_UserId { get; set; }
        public int UserSubscriptionId { get; set; }
        public int ProductId { get; set; }
        public DateTime RequestedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public string ApprovedBy { get; set; }

        [ForeignKey("Requested_UserId")]
        public virtual AppUser User { get; set; }

        [ForeignKey("UserSubscriptionId")]
        public virtual UserSubscription UserSubscripption { get; set; }
        public String Comment { get; set; }
    }
}
