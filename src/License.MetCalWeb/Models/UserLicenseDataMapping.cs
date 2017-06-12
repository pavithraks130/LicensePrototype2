using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class UserLicenseDataMapping
    {
        public int TeamId { get; set; }
        public List<User> UserList { get; set; }

        public List<ProductLicense> LicenseDataList { get; set; }
    }
    
}