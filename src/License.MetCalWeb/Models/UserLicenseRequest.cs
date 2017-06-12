using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
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
        public User User { get; set; }
        public UserSubscription UserSubscription { get; set; }
        public ProductDetails Product { get; set; }
        public String Comment { get; set; }
        public int TeamId { get; set; }
    }

}