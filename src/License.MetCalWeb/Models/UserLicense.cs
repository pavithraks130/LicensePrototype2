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
}