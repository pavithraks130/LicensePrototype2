using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class UserLicenseDetails
    {
        public User User { get; set; }
        public List<ProductDetails> Products { get; set; }
    }
}