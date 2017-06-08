using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class UserLicense
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int LicenseId { get; set; }
        public int TeamId { get; set; }

        public User User { get; set; }

        public ProductLicense License { get; set; }
    }

    public class UserLicenseDataMapping
    {
        public int TeamId { get; set; }
        public List<User> UserList { get; set; }

        public List<ProductLicense> LicenseDataList { get; set; }
    }

    public class ProductLicense
    {
        public int Id { get; set; }

        public string AdminUserId { get; set; }

        public string LicenseKey { get; set; }

        public int UserSubscriptionId { get; set; }

        public int ProductId { get; set; }
    }

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
        public Product Product { get; set; }
        public String Comment { get; set; }
        public int TeamId { get; set; }
    }

   

    public class UserLicenseDetails
    {
        public User User { get; set; }

        public List<Product> Products { get; set; }
    }

    public class FetchUserSubscription
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public bool IsFeatureRequired { get; set; }
    }

    
}